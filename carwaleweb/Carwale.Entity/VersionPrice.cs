using Carwale.Entity.Geolocation;
using System;

namespace Carwale.Entity
{
    [Serializable]
    public class VersionPrice 
    {
        public VersionBase VersionBase { get; set; }
        public City City { get; set; }
        public string PriceLabel { get; set; }
        public int PriceVersionCount { get; set; }
        public int PriceStatus { get; set; }
        public string ReasonText { get; set; }
        public bool IsVersionBlocked { get; set; }
        public bool IsGSTPrice { get; set; }
    }
}
