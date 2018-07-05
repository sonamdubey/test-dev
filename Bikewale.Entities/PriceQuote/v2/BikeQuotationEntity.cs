using System.Collections.Generic;

namespace Bikewale.Entities.PriceQuote.v2
{
    /// <summary>
    /// Created by  : Pratibha Verma on 20 June 2018
    /// Description : chnages PQId data type
    /// </summary>
    public class BikeQuotationEntity
    {
        public string PriceQuoteId { get; set; }
        public string ManufacturerName { get; set; }

        public string MaskingNumber { get; set; }

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
        public bool IsScooterOnly { get; set; }
        public string State { get; set; }

        public string ManufacturerAd { get; set; }
        public string LeadCapturePopupHeading { get; set; }
        public string LeadCapturePopupDescription { get; set; }
        public string LeadCapturePopupMessage { get; set; }
        public bool PinCodeRequired { get; set; }
        public bool DealersRequired { get; set; }
        public bool EmailRequired { get; set; }
        public uint ModelId { get; set; }

    }
}
