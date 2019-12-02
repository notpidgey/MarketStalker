using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketStalker
{
    class MostRecentListing
    {

        public class User
        {
            [JsonProperty(PropertyName = "ingame_name")]
            public string IngameName { get; set; }
            public int Reputation { get; set; }
            public string Id { get; set; }
            public string Region { get; set; }
            public string Avatar { get; set; }
            public string Status { get; set; }
        }

        public class Ko
        {
            [JsonProperty(PropertyName = "item_name")]
            public string ItemName { get; set; }
        }

        public class En
        {
            [JsonProperty(PropertyName = "item_name")]
            public string ItemName { get; set; }
        }

        public class Sv
        {
            [JsonProperty(PropertyName = "item_name")]
            public string ItemName { get; set; }
        }

        public class Zh
        {
            [JsonProperty(PropertyName = "item_name")]
            public string ItemName { get; set; }
        }

        public class Fr
        {
            [JsonProperty(PropertyName = "item_name")]
            public string ItemName { get; set; }
        }

        public class De
        {
            [JsonProperty(PropertyName = "item_name")]
            public string ItemName { get; set; }
        }

        public class Ru
        {
            [JsonProperty(PropertyName = "item_name")]
            public string ItemName { get; set; }
        }

        public class Item
        {
            public Ko Ko { get; set; }
            [JsonProperty(PropertyName = "sub_icon")]
            public string SubIcon { get; set; }
            public string Thumb { get; set; }
            public En En { get; set; }
            public Sv Sv { get; set; }
            public string Id { get; set; }
            public Zh Zh { get; set; }
            [JsonProperty(PropertyName = "mod_max_rank")]
            public int ModMaxRank { get; set; }
            public List<string> Tags { get; set; }
            public Fr Fr { get; set; }
            public De De { get; set; }
            public Ru Ru { get; set; }
            [JsonProperty(PropertyName = "url_name")]
            public string UrlName { get; set; }
            public string Icon { get; set; }
        }

        public class SellOrder
        {
            public bool Visible { get; set; }
            public int Quantity { get; set; }
            public double Platinum { get; set; }
            [JsonProperty(PropertyName = "creation_date")]
            public DateTime CreationDate { get; set; }
            public string Id { get; set; }
            public string Platform { get; set; }
            [JsonProperty(PropertyName = "last_update")]
            public DateTime LastUpdate { get; set; }
            public User User { get; set; }
            [JsonProperty(PropertyName = "mod_rank")]
            public int ModRank { get; set; }
            [JsonProperty(PropertyName = "order_type")]
            public string OrderType { get; set; }
            public string Region { get; set; }
            public Item Item { get; set; }
        }

        public class User2
        {
            [JsonProperty(PropertyName = "ingame_name")]
            public string IngameName { get; set; }
            public int Reputation { get; set; }
            public string Id { get; set; }
            public string Region { get; set; }
            public string Avatar { get; set; }
            public string Status { get; set; }
        }

        public class Ko2
        {
            [JsonProperty(PropertyName = "item_name")]
            public string ItemName { get; set; }
        }

        public class En2
        {
            [JsonProperty(PropertyName = "item_name")]
            public string ItemName { get; set; }
        }

        public class Sv2
        {
            [JsonProperty(PropertyName = "item_name")]
            public string ItemName { get; set; }
        }

        public class Zh2
        {
            [JsonProperty(PropertyName = "item_name")]
            public string ItemName { get; set; }
        }

        public class Fr2
        {
            [JsonProperty(PropertyName = "item_name")]
            public string ItemName { get; set; }
        }

        public class De2
        {
            [JsonProperty(PropertyName = "item_name")]
            public string ItemName { get; set; }
        }

        public class Ru2
        {
            [JsonProperty(PropertyName = "item_name")]
            public string ItemName { get; set; }
        }

        public class Item2
        {
            public Ko2 Ko { get; set; }
            [JsonProperty(PropertyName = "sub_icon")]
            public string SubIcon { get; set; }
            public string Thumb { get; set; }
            public En2 En { get; set; }
            public Sv2 Sv { get; set; }
            public string Id { get; set; }
            public Zh2 Zh { get; set; }
            [JsonProperty(PropertyName = "mod_max_rank")]
            public int ModMaxRank { get; set; }
            public List<string> Tags { get; set; }
            public Fr2 Fr { get; set; }
            public De2 De { get; set; }
            public Ru2 Ru { get; set; }
            [JsonProperty(PropertyName = "url_name")]
            public string UrlName { get; set; }
            public string Icon { get; set; }
        }

        public class BuyOrder
        {
            public bool Visible { get; set; }
            public int Quantity { get; set; }
            public double Platinum { get; set; }
            [JsonProperty(PropertyName = "creation_date")]
            public DateTime CreationDate { get; set; }
            public string Id { get; set; }
            public string Platform { get; set; }
            [JsonProperty(PropertyName = "last_update")]
            public DateTime LastUpdate { get; set; }
            public User2 User { get; set; }
            [JsonProperty(PropertyName = "mod_rank")]
            public int ModRank { get; set; }
            [JsonProperty(PropertyName = "order_type")]
            public string OrderType { get; set; }
            public string Region { get; set; }
            public Item2 Item { get; set; }
        }

        public class Payload
        {
            [JsonProperty(PropertyName = "sell_orders")]
            public List<SellOrder> SellOrders { get; set; }
            [JsonProperty(PropertyName = "buy_orders")]
            public List<BuyOrder> BuyOrders { get; set; }
        }

        public class RootObject
        {
            public Payload Payload { get; set; }
        }

    }
}
