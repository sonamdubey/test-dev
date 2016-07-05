using BikeWale.DTO.AutoBiz;
using Newtonsoft.Json;
using System;

namespace Bikewale.DTO.AutoBiz
{
    /// <summary>
    /// Written By : Ashwini Todkar on 28 Oct 2014
    /// </summary>
    public class OfferEntityDTO
    {
        public MakeEntityBaseDTO objMake { get; set; }
        public ModelEntityBaseDTO objModel { get; set; }
        public VersionEntityBaseDTO objVersion { get; set; }
        public CityEntityBaseDTO objCity { get; set; }
        public NewBikeDealersDTO objDealer { get; set; }

        [JsonProperty("offerId")]
        public UInt32 OfferId { get; set; }

        [JsonProperty("availabilityId")]
        public Int32 AvailabilityId { get; set; }

        [JsonProperty("offerCategoryId")]
        public UInt32 OfferCategoryId { get; set; }

        [JsonProperty("offerType")]
        public string OfferType { get; set; }

        [JsonProperty("offerText")]
        public string OfferText { get; set; }

        [JsonProperty("AvailableLimit")]
        public UInt16 AvailableLimit { get; set; }

        [JsonProperty("offerValue")]
        public UInt32 OfferValue { get; set; }

        [JsonProperty("offervalidTill")]
        public DateTime OffervalidTill { get; set; }

        [JsonProperty("offerTypeId")]
        public UInt32 OfferTypeId { get; set; }

        [JsonProperty("userId")]
        public uint UserId { get; set; }

        [JsonProperty("isOfferTerms")]
        public bool IsOfferTerms { get; set; }

        [JsonProperty("isPriceImpact")]
        public bool IsPriceImpact { get; set; }

    }
}
