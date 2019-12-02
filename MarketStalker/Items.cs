using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketStalker
{
    class Items
    {
        public class Rootobject
        {
            public Payload Payload { get; set; }
        }

        public class Payload
        {
            public Item[] Items { get; set; }
        }

        public class Item
        {
            [JsonProperty(PropertyName = "item_name")]
            public string ItemName { get; set; }

            public string Id { get; set; }
            public string Thumb { get; set; }

            [JsonProperty(PropertyName = "url_name")]
            public string UrlName { get; set; }
        }

        public class ItemEntireList
        {
            public string Item { get; set; }
            public bool IsChecked { get; set; }
            public string Id { get; set; }
        }

        public class DataGrid
        {
            public string Id { get; set; }
            public string Item { get; set; }
            public double Price { get; set; }
            public double Third { get; set; }
            public double Discount { get; set; }
            public double Rep { get; set; }
            public string Seller { get; set; }
            public bool Matches { get; set; }
            public DateTime Time { get; set; }
        }
    }
}
