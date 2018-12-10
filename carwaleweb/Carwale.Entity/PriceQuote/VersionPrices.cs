
using System;

namespace Carwale.Entity.PriceQuote
{
    [Serializable]
    public class VersionPrices
    {
        public int VersionId { get; set; }
        public string VersionName { get; set; }
        public bool IsMetallic { get; set; }
        public bool IsNew { get; set; }
        public DateTime LastUpdated { get; set; }
        public int PQItemId { get; set; }
        public string PQItemName { get; set; }
        public int PQItemValue { get; set; }
        public bool OnRoadPriceInd { get; set; }
    }
}
