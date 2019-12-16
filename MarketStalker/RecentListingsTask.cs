using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;


namespace MarketStalker
{
    class RecentListingsTask
    {

        public delegate void SellOrdersAvailable(object minPrice, MostRecentListing.SellOrder sellOrders);

        public class WarframeMarketApi
        {
            private readonly TimeSpan _pollDelay = TimeSpan.FromSeconds(5);
            public event SellOrdersAvailable SellOrdersAvailable;

            public async Task StartPolling(CancellationToken cancellationToken, IEnumerable<string> selectedItems)
            {
                MainWindow mainwindow = (MainWindow) Application.Current.MainWindow;
                mainwindow.CollectionGrid.Items.Clear();

                MostRecentListing.RootObject currentListings = await GetRecentListingsAsync();
                MostRecentListing.RootObject outdatedListings = null;

                var ListingsToCheck = currentListings.Payload.SellOrders.Take(25).ToList();

                await Task.Delay(1000);

                while (!cancellationToken.IsCancellationRequested)
                {
                    await GetSellOrdersAsnyc(ListingsToCheck, selectedItems, cancellationToken);
                    
                    await Task.Delay(_pollDelay);

                    outdatedListings = currentListings;
                    currentListings = await GetRecentListingsAsync();

                    ListingsToCheck = currentListings.Payload.SellOrders
                        .Take(300)
                        .Where(d => outdatedListings.Payload.SellOrders
                            .Take(300)
                            .All(t => t.Id != d.Id))
                        .ToList();
                }
            }

            public async Task GetSellOrdersAsnyc(
                List<MostRecentListing.SellOrder> currentListings, IEnumerable<string> selectedItems, CancellationToken cancellationToken)
            {
                foreach (MostRecentListing.SellOrder sellOrder in currentListings.Where(a => selectedItems.Contains(a.Item.Id)))
                {
                        try
                        {
                            await Task.Delay(350, cancellationToken).ConfigureAwait(false);
                        }
                        catch
                        {
                            break;
                        }

                        var reqstname = sellOrder.Item.En.ItemName.ToLower()
                            .Replace("&",
                                "and")
                            .Replace("'",
                                "")
                            .Replace("-",
                                "_")
                            .Replace(" ",
                                "_");

                        var itempage = await Request.GetAsync("https://api.warframe.market/v1/items/" +
                                                              reqstname + "/orders").ConfigureAwait(false);

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

                        SellOrdersAvailable?.Invoke(minPrice, sellOrder);
                }
            }

            public async Task<MostRecentListing.RootObject> GetRecentListingsAsync()
            {
                string data = await Request.GetAsync("https://api.warframe.market/v1/most_recent");
                MostRecentListing.RootObject rootobject =
                    JsonConvert.DeserializeObject<MostRecentListing.RootObject>(data);

                return rootobject;
            }
        }
    }
}
