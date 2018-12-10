using Carwale.Entity.Classified.Enum;
using System;
using System.Collections.Specialized;

namespace Carwale.Entity.Classified
{
    public class FilterInputs
    {
        public string make { get; set; } = string.Empty;
        public string modelName { get; set; } = string.Empty;
        public string car { get; set; } = string.Empty;
        public string city { get; set; } = string.Empty;
        public string root { get; set; } = string.Empty;
        public string filterby { get; set; } = string.Empty;
        public string filterbyadditional { get; set; } = string.Empty;
        public string fuel { get; set; } = string.Empty;
        public string bodytype { get; set; } = string.Empty;
        public string subSegmentID { get; set; } = string.Empty;
        public string sessionId { get; set; } = string.Empty;
        public string profileId { get; set; } = string.Empty;
        public string seller { get; set; } = string.Empty;
        public string owners { get; set; } = string.Empty;
        public string trans { get; set; } = string.Empty;
        public string color { get; set; } = string.Empty;
        public string kms { get; set; } = string.Empty;
        public string year { get; set; } = string.Empty;
        public string budget { get; set; } = string.Empty;
        public string sc { get; set; } = string.Empty;
        public string so { get; set; } = string.Empty;
        public string pn { get; set; } = string.Empty;
        public string ps { get; set; } = string.Empty;
        public bool bestmatch { get; set; }
        public int lcr { get; set; }
        public int ldr { get; set; }
        public int lir { get; set; }
        public int nearbyCityId { get; set; }
        public string nearbyCityIds { get; set; } = string.Empty;
        public string nearbyCityIdsStockCount { get; set; } = string.Empty;
        public int userPreferredRootId { get; set; }
        public int stockFetched { get; set; }
        public string ExcludeStocks { get; set; }

        // Nearby cars
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public bool ShouldFetchNearbyCars { get; set; }
        public NearbyCarsBucket LastNearbyCarsBucket { get; set; }
        public int Area { get; set; }

        public void SetFilterInputsFromQueryString(NameValueCollection qs)
        {
            if (qs != null)
            {
                if (!string.IsNullOrEmpty(qs["cityId"]))
                    city = qs["cityId"];
                if (!string.IsNullOrEmpty(qs["filterByAdditional"]))
                    filterbyadditional = qs["filterByAdditional"];
                if (!string.IsNullOrEmpty(qs["kms"]))
                    kms = qs["kms"];
                if (!string.IsNullOrEmpty(qs["year"]))
                    year = qs["year"];
                if (!string.IsNullOrEmpty(qs["budget"]))
                    budget = qs["budget"];
                if (!string.IsNullOrEmpty(qs["color"]))
                    color = qs["color"];
                if (!string.IsNullOrEmpty(qs["fuel"]))
                    fuel = qs["fuel"];
                if (!string.IsNullOrEmpty(qs["bodyType"]))
                    bodytype = qs["bodyType"];
                if (!string.IsNullOrEmpty(qs["owners"]))
                    owners = qs["owners"];
                if (!string.IsNullOrEmpty(qs["seller"]))
                    seller = qs["seller"];
                if (!string.IsNullOrEmpty(qs["trans"]))
                    trans = qs["trans"];
                if (!string.IsNullOrEmpty(qs["car"]))
                    car = qs["car"];
                if (!string.IsNullOrEmpty(qs["sc"]))
                    sc = qs["sc"];
                if (!string.IsNullOrEmpty(qs["so"]))
                    so = qs["so"];
                if (!string.IsNullOrEmpty(qs["lcr"]))
                {
                    short m_latestCarRank;
                    short.TryParse(qs["lcr"], out m_latestCarRank);
                    lcr = m_latestCarRank;
                }
            }
        }
    }
}
