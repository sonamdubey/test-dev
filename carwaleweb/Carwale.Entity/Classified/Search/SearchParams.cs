
using Carwale.Entity.Classified.Enum;

namespace Carwale.Entity.Classified.Search
{
    public class SearchParams
    {
        public bool IsFilter { get; set; }
        public bool IsSort { get; set; }
        public bool IsSold { get; set; }
        public bool IsAmp { get; set; }

        #region Filter Params
        public string Car { get; set; }
        public int Make { get; set; }
        public string MakeName { get; set; } = string.Empty;
        public string Root { get; set; } = string.Empty;
        public int City { get; set; }
        public string CityName { get; set; } = string.Empty;
        public string Year { get; set; }
        public string Budget { get; set; }
        public string Kms { get; set; }
        public string Fuel { get; set; }
        public string Color { get; set; }
        public string BodyType { get; set; }
        public string Trans { get; set; }
        public string Owners { get; set; }
        public string Seller { get; set; }
        public string FilterByAdditional { get; set; }
        public int FilterAppliedCount { get; set; }
        public string ExcludeStocks { get; set; }
        #endregion

        #region Sort & Pagination
        public int Sc { get; set; } = -1;
        public int So { get; set; } = -1;
        public int Pn { get; set; } = 1;
        public int Ps { get; set; }
        public int Lcr { get; set; }
        public int Ldr { get; set; }
        public int Lir { get; set; }
        #endregion

        #region Nearby city params
        public int NearbyCityId { get; set; }
        public string NearbyCityIds { get; set; }
        public string NearbyCityIdsStockCount { get; set; }
        public int StockFetched { get; set; }
        #endregion

        #region User Preferences
        public int UserPreferredRootId { get; set; }
        #endregion

        #region Nearby Cars
        public double Latitude { get; set; } = -100; //invalid lat value
        public double Longitude { get; set; } = -200; //invalid long value
        public NearbyCarsBucket LastNearbyCarsBucket { get; set; }
        public bool ShouldFetchNearbyCars { get; set; } = true;
        public string CustAreaName { get; set; }
        public int Area { get; set; }
        public bool IsLocationDetected { get; set; }
        #endregion
    }
}
