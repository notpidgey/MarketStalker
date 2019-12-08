using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Xml;
using DocumentFormat.OpenXml.Office.CustomUI;
using MahApps.Metro.Controls;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using static MarketStalker.Items;
using static MarketStalker.RandomHelpers;



namespace MarketStalker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public partial class MainWindow : MetroWindow
    {

        public MainWindow()
        {
            InitializeComponent();
            main = this;
        }

        internal static MainWindow main;
        
        internal string ConsoleLogNew
        {
            set { Dispatcher.Invoke(new Action(() => { ConsoleOutput.Text += "\n" + value; })); }
        }
        
        public bool WatchAllItems
        {
            get
            {
                return (bool) WatchAll.IsChecked;
            }
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

            ItemsIDCheckList.Items.SortDescriptions.Add(
                new SortDescription(
                    "Item",
                    ListSortDirection.Ascending));
            ItemsIDCheckList.ItemsSource = items;

            TotalItemsText.Text = rootobject.Payload.Items.Count().ToString();
            ConsoleOutput.Text += "\nUpdated Item List Using " + method;
            GC.Collect();
        }

        public void UserFilter()
        {
            ICollectionView view = CollectionViewSource.GetDefaultView(ItemsIDCheckList.ItemsSource);
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


        private async void TextFilter_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            UserFilter();
        }

        private async void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            await Task.Delay(1000);
            ConsoleOutput.Text += "Application Started...\nAttempting to update item list";

            PullData("API");
            for (double i = 1.00; i > 0; i = i - .01)
            {
                loadingImage.Opacity = i;
                loadingText.Opacity = i;
                await Task.Delay(2);
            }
            for (double i = 1.00; i > 0; i = i - .01)
            {
                loadinGrid.Opacity = i;
                await Task.Delay(1);
            }
            loadinGrid.Visibility = Visibility.Hidden;

        }

        public int InitialStartButton = 0;
        public int RunningTaskButton = 0;

        private async void IntialStart_Click(object sender, RoutedEventArgs e)
        {
            InitialStartButton++;
            if(InitialStartButton == 1)
            {
                var myItems = ItemsIDCheckList.ItemsSource as IEnumerable<ItemEntireList>;

                if (WatchAll.IsChecked == true)
                    RecentListingsTask.InitialStart(myItems.Select(p => p.Id));
                else
                    RecentListingsTask.InitialStart(myItems.Where(p => p.IsChecked).Select(p => p.Id));

                InitialStart.Content = "Pause Recent Search";
            }
            else if(IsOdd(InitialStartButton) == false)
            {
                InitialStart.Content = "Continue Listing Parse";
            }
            else
            {
                InitialStart.Content = "Pause Recent Search";
            }
        }

        private void ButtonAPI_Click(object sender, RoutedEventArgs e)
        {
            PullData("Warframe Market API");
        }

        private void RunningTask_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            TotalItemsWatchingText.Text = (Convert.ToInt32(TotalItemsWatchingText.Text) + 1).ToString();
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            TotalItemsWatchingText.Text = (Convert.ToInt32(TotalItemsWatchingText.Text) - 1).ToString();
        }
    }
}