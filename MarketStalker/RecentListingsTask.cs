using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;


namespace MarketStalker
{
    class RecentListingsTask
    {
        public static async void InitialStart(IEnumerable<string> selectedItems)
        {
            MainWindow mainwindow = (MainWindow)Application.Current.MainWindow;

            mainwindow.CollectionGrid.Items.Clear();

            string data = Request.Get("https://api.warframe.market/v1/most_recent");
            MostRecentListing.RootObject rootobject = JsonConvert.DeserializeObject<MostRecentListing.RootObject>(data);

            foreach (MostRecentListing.SellOrder sellOrder in rootobject.Payload.SellOrders.Where(a => selectedItems.Contains(a.Item.Id)))
            {
                while(RandomHelpers.IsOdd(mainwindow.InitialStartButton) == false)
                {
                    await Task.Delay(100);
                }

                await Task.Delay(350);

                string reqstname = sellOrder.Item.En.ItemName.ToLower().Replace("&", "and").Replace("'", "").Replace("-", "_").Replace(" ", "_");
                string itempage = Request.Get("https://api.warframe.market/v1/items/" +
                                              reqstname + "/orders");

                ItemPage.Rootobject rootObjectItemPage =
                    JsonConvert.DeserializeObject<ItemPage.Rootobject>(itempage);

                object minPrice = null;
                try
                {
                    minPrice = rootObjectItemPage.Payload.Orders
                        .Where(w => w.User.Status == "ingame" && w.order_type == "sell")
                        .Select(s => s.Platinum)
                        .OrderBy(o => o)
                        .Take(2)
                        .Last();
                }
                catch
                {
                    minPrice = rootObjectItemPage.Payload.Orders
                        .Where(w => w.order_type == "sell")
                        .Select(s => s.Platinum)
                        .OrderBy(o => o)
                        .Take(2)
                        .Last();
                }

                string d = sellOrder.Item.En.ItemName;
                double thirdlistingprice = Convert.ToDouble(minPrice);

                double newlistingprice = sellOrder.Platinum;
                double priceDifference = newlistingprice - thirdlistingprice;

                if (RandomHelpers.IsNegative(priceDifference))
                {
                    MainWindow.main.ConsoleLogNew = "Item: " + sellOrder.Item.En.ItemName + " Price: " + newlistingprice + " Difference: " + priceDifference;

                    string seller = sellOrder.User.IngameName;
                    double rep = sellOrder.User.Reputation;
                    DateTime time = sellOrder.LastUpdate;

                    bool matches = Convert.ToDouble(priceDifference.ToString().Replace("-", "")) >= mainwindow.minPrice.Value && Convert.ToDouble(priceDifference.ToString().Replace("-", "")) <= mainwindow.maxPrice.Value;
                    if (mainwindow.showNonMatching.IsChecked == true)
                    {
                        var data2 = new Items.DataGrid { Id = sellOrder.Id.Substring(5, 8), Item = sellOrder.Item.En.ItemName, Price = newlistingprice, Third = thirdlistingprice, Discount = priceDifference, Seller = seller, Rep = rep, Matches = matches, Time = time };
                        mainwindow.CollectionGrid.Items.Add(data2);
                        mainwindow.ConsoleOutput.ScrollToEnd();
                    }
                    else
                    {
                        if (!matches)
                        {

                        }
                        else
                        {
                            var data2 = new Items.DataGrid { Id = sellOrder.Id.Substring(5, 9), Item = sellOrder.Item.En.ItemName, Price = newlistingprice, Third = thirdlistingprice, Discount = priceDifference, Seller = seller, Rep = rep, Matches = matches, Time = time };
                            mainwindow.CollectionGrid.Items.Add(data2);
                            mainwindow.ConsoleOutput.ScrollToEnd();

                        }
                    }
                }
                else
                {
                    MainWindow.main.ConsoleLogNew =
                        "Item: " + sellOrder.Item.En.ItemName + " Price: " +
                        newlistingprice + " Difference: +" + priceDifference;
                    mainwindow.ConsoleOutput.ScrollToEnd();
                }
                mainwindow.CollectionGrid.Items.Refresh();
            }
            mainwindow.showNonMatching.Content = "Recent Listings Parse";
            mainwindow.InitialStartButton = 0;
        }

    }
}
