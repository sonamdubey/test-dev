using System.Collections.Generic;

namespace Bikewale.Entities.PriceQuote
{
    /// <summary>
    /// Modified By : Sushil Kumar
    /// Modified On : 6th June 2016
    /// Description : Added makeId property to get makeId for dealers card widget 
    /// Modified By : Sushil Kumar on 8th June 2016
    /// Description : Added ismodelnew and isversion new data 
    /// </summary>
    public class BikeQuotationEntity
    {
        public ulong PriceQuoteId { get; set; }

        public ulong ExShowroomPrice { get; set; }
        public uint RTO { get; set; }
        public uint Insurance { get; set; }
        public ulong OnRoadPrice { get; set; }

        public string MakeName { get; set; }
        public string MakeMaskingName { get; set; }
        public string ModelName { get; set; }
        public string ModelMaskingName { get; set; }
        public string VersionName { get; set; }
        public uint CityId { get; set; }
        public string CityMaskingName { get; set; }
        public string City { get; set; }
        public string Area { get; set; }
        public bool HasArea { get; set; }
        public uint VersionId { get; set; }

        public uint CampaignId { get; set; }

        public uint ManufacturerId { get; set; }

        public IEnumerable<OtherVersionInfoEntity> Varients { get; set; }

        public string OriginalImage { get; set; }
        public string HostUrl { get; set; }

        public uint MakeId { get; set; }
        public bool IsModelNew { get; set; }
        public bool IsVersionNew { get; set; }

    }
}
