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

            string data = Request.Get("https://api.warframe.market/v1/most_recent");
            MostRecentListing.RootObject rootobject = JsonConvert.DeserializeObject<MostRecentListing.RootObject>(data);

            foreach (MostRecentListing.SellOrder sellOrder in rootobject.Payload.SellOrders.Where(a => selectedItems.Contains(a.Item.Id)))
            {
                await Task.Delay(400);

                string reqstname = sellOrder.Item.En.ItemName.ToLower().Replace("&", "and").Replace("'", "").Replace("-", "_").Replace(" ", "_");

                string itempage = Request.Get("https://api.warframe.market/v1/items/" +
                                              reqstname + "/orders");
                //string statisticspage =
                //    Request.Get("https://api.warframe.market/v1/items/" + reqstname +
                //                "/statistics");

              
                ItemPage.Rootobject rootObjectItemPage =
                    JsonConvert.DeserializeObject<ItemPage.Rootobject>(itempage);

                //ItemStatistics.Rootobject rootObjectStatisticsPage =
                //      JsonConvert.DeserializeObject<ItemStatistics.Rootobject>(statisticspage);

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


                //var recentStats = rootObjectStatisticsPage.Payload.StatisticsClosed.The48Hours
                //    .OrderByDescending(joe => joe.Datetime)
                //    .First();

                string d = sellOrder.Item.En.ItemName.ToString();
                double thirdlistingprice = Convert.ToDouble(minPrice);
                //double thirdlistingprice = recentStats.AvgPrice;
                double newlistingprice = Convert.ToDouble(sellOrder.Platinum.ToString());
                double priceDifference = newlistingprice - thirdlistingprice;

                if (RandomHelpers.IsNegative(priceDifference))
                {
                    MainWindow.main.ConsoleLogNew = "Item: " + sellOrder.Item.En.ItemName + " Price: " + newlistingprice + " Difference: " + priceDifference.ToString();
                    //Its cheaper! Woweee how cool am I right? haha
                    //Lets get some more info about this sale
                    string seller = sellOrder.User.IngameName.ToString();
                    double rep = sellOrder.User.Reputation;
                    DateTime time = sellOrder.LastUpdate;

                    bool matches = Convert.ToDouble(priceDifference.ToString().Replace("-", "")) >= mainwindow.minPrice.Value && Convert.ToDouble(priceDifference.ToString().Replace("-", "")) <= mainwindow.maxPrice.Value;

                    var data2 = new Items.DataGrid { Id = sellOrder.Id.Substring(5,9), Item = sellOrder.Item.En.ItemName, Price = newlistingprice, Third = thirdlistingprice, Discount = priceDifference, Seller = seller, Rep = rep, Matches = matches, Time = time };
                    mainwindow.CollectionGrid.Items.Add(data2);
                    mainwindow.ConsoleOutput.ScrollToEnd();
                    //mainwindow.CollectionGrid.ScrollIntoView(data2);
                }
                else
                    MainWindow.main.ConsoleLogNew =
                        "Item: " + sellOrder.Item.En.ItemName + " Price: " +
                        newlistingprice + " Difference: +" + priceDifference.ToString();
                mainwindow.ConsoleOutput.ScrollToEnd();
            }
        }

    }
}
