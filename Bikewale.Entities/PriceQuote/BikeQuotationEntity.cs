using System.Collections.Generic;

namespace Bikewale.Entities.PriceQuote
{
    public class BikeQuotationEntity
    {
        public ulong PriceQuoteId { get; set; }

        public ulong ExShowroomPrice { get; set; }
        public uint RTO { get; set; }
        public uint Insurance { get; set; }
        public ulong OnRoadPrice { get; set; }

        public string MakeName { get; set; }
        public string ModelName { get; set; }
        public string VersionName { get; set; }
        public string City { get; set; }
        public string Area { get; set; }

        public uint VersionId { get; set; }

        public uint CampaignId { get; set; }

        public uint ManufacturerId { get; set; }

        public IEnumerable<OtherVersionInfoEntity> Varients { get; set; }
    }
}
