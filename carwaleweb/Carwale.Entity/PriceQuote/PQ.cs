using Carwale.Entity.CarData;
using System.Collections.Generic;
using Carwale.Entity.Geolocation;
using Carwale.Entity.Dealers;
using Carwale.Entity;
using System;

namespace Carwale.Entity.PriceQuote
{   
    [Serializable]
    public class PQ : CarEntity
    {
        public ulong PQId { get; set; }
        public List<PQItem> PriceQuoteList { get; set; }       
        public string LargePic { get; set; }
        public string SmallPic { get; set; }
        public string XtraLargePic { get; set; }
        public string HostURL { get; set; }
        public string OriginalImgPath { get; set; }
        public int CityId { get; set; }
        public string ZoneId { get; set; }
        public string CityName { get; set; }
        public string ZoneName { get; set; }
        public long OnRoadPrice { get; set; }
        public string SpecSummery { get; set; }
        public decimal ReviewRate { get; set; }
        public int PageId { get; set; }
        public int AreaId { get; set; }
    }
}
