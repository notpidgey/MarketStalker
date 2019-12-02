using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketStalker
{
    class ItemPage
    {
        public class Rootobject
        {
            public Payload Payload { get; set; }
        }

        public class Payload
        {
            public Order[] Orders { get; set; }
        }

        public class Order
        {
            public float Platinum { get; set; }
            public int Quantity { get; set; }
            public string order_type { get; set; }
            public User User { get; set; }
            public string Platform { get; set; }
            public string Region { get; set; }
            public DateTime creation_date { get; set; }
            public DateTime last_update { get; set; }
            public bool Visible { get; set; }
            public string Id { get; set; }
        }

        public class User
        {
            public double Reputation { get; set; }
            public double Reputation_bonus { get; set; }
            public string Region { get; set; }
            public DateTime? last_seen { get; set; }
            public string ingame_name { get; set; }
            public string Avatar { get; set; }
            public string Status { get; set; }
            public string Id { get; set; }
        }

    }
}
