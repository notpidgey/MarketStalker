using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketStalker
{
    class ItemStatistics
    {

        public partial class Rootobject
        {
            [JsonProperty("payload")]
            public Payload Payload { get; set; }
        }

        public partial class Payload
        {
            [JsonProperty("statistics_closed")]
            public StatisticsClosed StatisticsClosed { get; set; }

            [JsonProperty("statistics_live")]
            public StatisticsLive StatisticsLive { get; set; }
        }

        public partial class StatisticsClosed
        {
            [JsonProperty("48hours")]
            public StatisticsClosed48Hour[] The48Hours { get; set; }

            [JsonProperty("90days")]
            public StatisticsClosed48Hour[] The90Days { get; set; }
        }

        public partial class StatisticsClosed48Hour
        {
            [JsonProperty("datetime")]
            public DateTimeOffset Datetime { get; set; }

            [JsonProperty("volume")]
            public long Volume { get; set; }

            [JsonProperty("min_price")]
            public long MinPrice { get; set; }

            [JsonProperty("max_price")]
            public long MaxPrice { get; set; }

            [JsonProperty("open_price")]
            public long OpenPrice { get; set; }

            [JsonProperty("closed_price")]
            public long ClosedPrice { get; set; }

            [JsonProperty("avg_price")]
            public double AvgPrice { get; set; }

            [JsonProperty("wa_price")]
            public double WaPrice { get; set; }

            [JsonProperty("median")]
            public double Median { get; set; }

            [JsonProperty("moving_avg")]
            public double MovingAvg { get; set; }

            [JsonProperty("donch_top")]
            public long DonchTop { get; set; }

            [JsonProperty("donch_bot")]
            public long DonchBot { get; set; }

            [JsonProperty("id")]
            public string Id { get; set; }
        }

        public partial class StatisticsLive
        {
            [JsonProperty("48hours")]
            public StatisticsLive48Hour[] The48Hours { get; set; }

            [JsonProperty("90days")]
            public StatisticsLive48Hour[] The90Days { get; set; }
        }

        public partial class StatisticsLive48Hour
        {
            [JsonProperty("datetime")]
            public DateTimeOffset Datetime { get; set; }

            [JsonProperty("volume")]
            public long Volume { get; set; }

            [JsonProperty("min_price")]
            public long MinPrice { get; set; }

            [JsonProperty("max_price")]
            public long MaxPrice { get; set; }

            [JsonProperty("avg_price")]
            public double AvgPrice { get; set; }

            [JsonProperty("wa_price")]
            public double WaPrice { get; set; }

            [JsonProperty("median")]
            public double Median { get; set; }

            [JsonProperty("order_type")]
            public OrderType OrderType { get; set; }

            [JsonProperty("moving_avg")]
            public double MovingAvg { get; set; }

            [JsonProperty("id")]
            public string Id { get; set; }
        }

        public enum OrderType { Buy, Sell };
    }
}