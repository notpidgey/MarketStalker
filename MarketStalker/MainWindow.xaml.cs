using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using MahApps.Metro.Controls;
using Newtonsoft.Json;
using static MarketStalker.Items;
using static MarketStalker.RandomHelpers;


namespace MarketStalker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public partial class MainWindow : MetroWindow
    {
        private readonly RecentListingsTask.WarframeMarketApi _warframeApiPoller = new RecentListingsTask.WarframeMarketApi();
        private CancellationTokenSource _cancellationTokenSource;
        private bool _isPolling;
        private bool _isPulling;
        
        public MainWindow()
        {
            InitializeComponent();

            _warframeApiPoller.SellOrdersAvailable += ProcessSellOrders;
        }

        private void PullData(string method)
        {
            string data;
            if (method == "json")
                data = File.ReadAllText("RequestedItems.json");
            else
            {
                data = Request.Get("https://api.warframe.market/v1/items");
                File.WriteAllText(@"RequestedItems.json", data);
            }

            Rootobject rootobject = JsonConvert.DeserializeObject<Rootobject>(data);
            List<ItemEntireList> items;

            items = rootobject.Payload.Items.Select(a => new ItemEntireList { Item = a.ItemName, IsChecked = false, Id = a.Id }).ToList();

            ItemsIdCheckList.Items.SortDescriptions.Add(
                new SortDescription(
                    "Item",
                    ListSortDirection.Ascending));
            ItemsIdCheckList.ItemsSource = items;

            TotalItemsText.Text = rootobject.Payload.Items.Count().ToString();
            ConsoleOutput.Text += "\nUpdated Item List Using " + method;
            GC.Collect();
        }

        public void UserFilter()
        {
            ICollectionView view = CollectionViewSource.GetDefaultView(ItemsIdCheckList.ItemsSource);
            if (view != null)
            {
                switch (TextFilter.Text.ToLower())
                {
                    case "lohk":
                    case "xata":
                    case "ris":
                    case "fass":
                    case "jahu":
                    case "vome":
                    case "netra":
                    case "khra":
                        view.Filter = item => item is ItemEntireList itemList && itemList.Item.Contains(TextFilter.Text.ToUpper());
                        break;
                    default:
                        view.Filter = item => item is ItemEntireList itemList && itemList.Item.Contains(UppercaseWords(TextFilter.Text));
                        break;
                }
            }
        }


        private void TextFilter_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            UserFilter();
        }

        private async void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            await Task.Delay(1000);
            ConsoleOutput.Text += "Application Started...\nAttempting to update item list";

            PullData("API");

            LoadingLogo.Visibility = Visibility.Visible;
            for (double i = 1.00; i > 0; i = i - .01)
            {
                LoadingLogo.Opacity = i;
                LoadingText.Opacity = i;
                await Task.Delay(2);
            }
            for (double i = 1.00; i > 0; i = i - .01)
            {
                LoadingGrid.Opacity = i;
                await Task.Delay(1);
            }
            LoadingGrid.Visibility = Visibility.Hidden;

        }

        async void IntialStart_Click(object sender, RoutedEventArgs e)
        {
            var myItems = ItemsIdCheckList.ItemsSource as IEnumerable<ItemEntireList>;

            if (!_isPulling)
            {
                CollectionGrid.Items.Clear();

                _cancellationTokenSource?.Cancel();
                _isPolling = false;
                RunningTask.Content = "Continuous Search";

                _isPulling = true;
                InitialStart.Content = "Stop Recent Listing Parse";
                _cancellationTokenSource = new CancellationTokenSource();



                var currentListings = await _warframeApiPoller.GetRecentListingsAsync();

                if (WatchAll.IsChecked == true)
                    await _warframeApiPoller.GetSellOrdersAsnyc(currentListings.Payload.SellOrders,
                        myItems.Select(p => p.Id), _cancellationTokenSource.Token);
                else
                    await _warframeApiPoller.GetSellOrdersAsnyc(currentListings.Payload.SellOrders,
                        myItems.Where(p => p.IsChecked).Select(p => p.Id), _cancellationTokenSource.Token);

                InitialStart.Content = "Recent Listing Parse";
            }
            else
            {
                _cancellationTokenSource.Cancel();
                _isPulling = false;
                InitialStart.Content = "Recent Listings Parse";
            }
        }

        async void RunningTask_Click(object sender, RoutedEventArgs e)
        {
            var myItems = ItemsIdCheckList.ItemsSource as IEnumerable<ItemEntireList>;
            
            if (!_isPolling)
            {
                CollectionGrid.Items.Clear();

                _cancellationTokenSource?.Cancel();
                _isPulling = false;
                InitialStart.Content = "Recent Listings Parse";

                _isPolling = true;
                RunningTask.Content = "Stop Task";
                _cancellationTokenSource = new CancellationTokenSource();

                if (WatchAll.IsChecked == true)
                    await _warframeApiPoller.StartPolling(_cancellationTokenSource.Token, myItems.Select(p => p.Id));
                else
                    await _warframeApiPoller.StartPolling(_cancellationTokenSource.Token, myItems.Where(p => p.IsChecked).Select(p => p.Id));
            }
            else
            {
                _cancellationTokenSource.Cancel();
                _isPolling = false;
                RunningTask.Content = "Continuous Search";
            }
        }

        void ProcessSellOrders(object minPrices, MostRecentListing.SellOrder sellOrder)
        {
            Dispatcher.Invoke(
                () =>
                {
                    var d = sellOrder.Item.En.ItemName;
                    var thirdlistingprice = Convert.ToDouble(minPrices);

                    var newlistingprice = sellOrder.Platinum;
                    var priceDifference = newlistingprice - thirdlistingprice;

                    ConsoleOutput.Text +=
                        "\nItem: " + sellOrder.Item.En.ItemName + " Price: " + newlistingprice +
                        " Difference: " + priceDifference;

                    if (RandomHelpers.IsNegative(priceDifference))
                    {
                        string seller = sellOrder.User.IngameName;
                        double rep = sellOrder.User.Reputation;
                        DateTime time = sellOrder.LastUpdate;

                        bool matches =
                            Convert.ToDouble(priceDifference.ToString().Replace("-", "")) >=
                            MinimumDiscount.Value &&
                            Convert.ToDouble(priceDifference.ToString().Replace("-", "")) <= MaximumDiscount.Value;

                        if (ShowNonMatching.IsChecked == true)
                        {
                            var data = new Items.DataGrid
                            {
                                Id = sellOrder.Id.Substring(5, 8),
                                Item = sellOrder.Item.En.ItemName,
                                Price = newlistingprice,
                                Third = thirdlistingprice,
                                Discount = priceDifference,
                                Seller = seller,
                                Rep = rep,
                                Matches = matches,
                                Time = time
                            };
                            CollectionGrid.Items.Add(data);
                            ConsoleOutput.ScrollToEnd();
                        }
                        else
                        {
                            if (!matches)
                            {

                            }
                            else
                            {
                                var data = new Items.DataGrid
                                {
                                    Id = sellOrder.Id.Substring(5, 9),
                                    Item = sellOrder.Item.En.ItemName,
                                    Price = newlistingprice,
                                    Third = thirdlistingprice,
                                    Discount = priceDifference,
                                    Seller = seller,
                                    Rep = rep,
                                    Matches = matches,
                                    Time = time
                                };
                                CollectionGrid.Items.Add(data);
                            }
                        }

                    }
                    ConsoleOutput.ScrollToEnd();
                    CollectionGrid.Items.Refresh();
                });
        }


        private void ButtonAPI_Click(object sender, RoutedEventArgs e)
        {
            PullData("Warframe Market API");
        }


        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            TotalItemsWatchingText.Text = (Convert.ToInt32(TotalItemsWatchingText.Text) + 1).ToString();
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            TotalItemsWatchingText.Text = (Convert.ToInt32(TotalItemsWatchingText.Text) - 1).ToString();
        }

        private void Application_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (Keyboard.Modifiers == ModifierKeys.Control && e.Key == Key.C)
            {
                if (CollectionGrid.SelectedCells.Count() == 9)
                {
                    var asdf = "/w " + (CollectionGrid.SelectedCells[5].Column.GetCellContent(CollectionGrid.SelectedCells[5].Item) as TextBlock).Text + " Hi! I want to buy: " +
                               (CollectionGrid.SelectedCells[1].Column.GetCellContent(CollectionGrid.SelectedCells[1].Item) as TextBlock).Text + " for " + (CollectionGrid.SelectedCells[2].Column.GetCellContent(CollectionGrid.SelectedCells[2].Item) as TextBlock).Text +
                               " platinum. (warframe.market)";
                    Clipboard.SetText(asdf);
                }

            }
        }
    }
}