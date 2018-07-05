using Bikewale.Entities.BikeData;
using Bikewale.ManufacturerCampaign.Entities;
using System.Collections.Generic;

namespace Bikewale.Entities.PriceQuote.v3
{
    /// <summary>
    /// Created by  : Pratibha Verma on 19 JUne 2018
    /// Description : changes PQId data type
    /// </summary>
    public class PQByCityAreaEntity
    {
        public bool IsCityExists { get; set; }
        public bool IsAreaExists { get; set; }
        public bool IsAreaSelected { get; set; }
        public bool IsExShowroomPrice { get; set; }
        public uint DealerId { get; set; }
        public string PqId { get; set; }
        public IEnumerable<BikeVersionMinSpecs> VersionList { get; set; }
        public DealerQuotationEntity PrimaryDealer { get; set; }
        public bool IsPremium { get; set; }
        public int SecondaryDealerCount { get; set; }
        public Bikewale.Entities.PriceQuote.v2.DetailedDealerQuotationEntity DealerEntity { get; set; }
        public ManufacturerCampaignEntity ManufacturerCampaign { get; set; }
        public Entities.Location.CityEntityBase City { get; set; }
        public Entities.Location.AreaEntityBase Area { get; set; }
    }
}
