using Carwale.Entity.Geolocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.PriceQuote
{
    [Serializable]
    public class PriceOverview
    {
        public int Price { get; set; }
        public string PriceLabel { get; set; }
        public int PriceVersionCount { get; set; }
        public int PriceStatus { get; set; }
        public string ReasonText { get; set; }
        public City City { get; set; }
        public bool IsGSTPrice { get; set; }
    }

    public class CarPriceOverview
    {
        public Dictionary<int, PriceOverview> Models { get; set; }
        public Dictionary<int, PriceOverview> Versions { get; set; }
    }
}
