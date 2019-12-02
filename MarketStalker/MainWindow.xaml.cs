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

        public string ConsoleOutputMethod
        {
            get
            {
                return ConsoleOutput.Text;
            }
            set
            {
                ConsoleOutput.Text = value;
            }
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

            icTodoList.Items.SortDescriptions.Add(
                new SortDescription(
                    "Item",
                    ListSortDirection.Ascending));
            icTodoList.ItemsSource = items;

            TotalItemsText.Text = rootobject.Payload.Items.Count().ToString();
            ConsoleOutput.Text += "\nUpdated Item List Using " + method;
            GC.Collect();
        }

        public void UserFilter()
        {
            ICollectionView view = CollectionViewSource.GetDefaultView(icTodoList.ItemsSource);
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
                        view.Filter = item => item is ItemEntireList itemList ? itemList.Item.Contains(TextFilter.Text.ToUpper()) : false;
                        break;
                    default:
                        view.Filter = item => item is ItemEntireList itemList ? itemList.Item.Contains(UppercaseWords(TextFilter.Text)) : false;
                        break;

                }
            }
        }


        private async void TextFilter_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            UserFilter();
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            TotalItemsWatchingText.Text = (Convert.ToInt32(TotalItemsWatchingText.Text) + 1).ToString();
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            TotalItemsWatchingText.Text = (Convert.ToInt32(TotalItemsWatchingText.Text) - 1).ToString();
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

        private async void IntialStart_Click(object sender, RoutedEventArgs e)
        {
            //var str = ItemEntireList.Where(p => p.IsChecked).Select(p => p.Item);
            //foreach(var item in str)
            //{
            //    ConsoleOutput.Text += "\n"  + item;
            //}

            var myItems = icTodoList.ItemsSource as IEnumerable<ItemEntireList>;

            if(WatchAll.IsChecked == true)
                RecentListingsTask.InitialStart(myItems.Select(p => p.Id));
            else
                RecentListingsTask.InitialStart(myItems.Where(p => p.IsChecked).Select(p => p.Id));



            //foreach (var item in myItems.Where(a => a.IsChecked)) 
            //{
            //    ConsoleOutput.Text += "\n" + item.Item;
            //}

        }

        private void ButtonAPI_Click(object sender, RoutedEventArgs e)
        {
            PullData("Warframe Market API");
        }
    }
}