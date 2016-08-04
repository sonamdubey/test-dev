using Newtonsoft.Json;
using System;

namespace BikewaleOpr.Entities
{
    /// <summary>
    /// controller
    /// </summary>
    public class OfferEntity
    {
        public CityEntityBase objCity { get; set; }

        public NewBikeDealers objDealer { get; set; }

        public BikeMakeEntityBase objMake { get; set; }

        public BikeModelEntityBase objModel { get; set; }

        public BikeVersionEntityBase objVersion { get; set; }

        [JsonProperty("offerId")]
        public UInt32 OfferId { get; set; }

        [JsonProperty("offerCategoryId")]
        public UInt32 OfferCategoryId { get; set; }

        [JsonProperty("offerType")]
        public string OfferType { get; set; }

        [JsonProperty("offerText")]
        public string OfferText { get; set; }

        [JsonProperty("offerValue")]
        public UInt32 OfferValue { get; set; }

        [JsonProperty("isOfferTerms")]
        public bool IsOfferTerms { get; set; }

        [JsonProperty("isPriceImpact")]
        public bool IsPriceImpact { get; set; }

        [JsonProperty("availabilityId")]
        public Int32 AvailabilityId { get; set; }

        [JsonProperty("AvailableLimit")]
        public UInt16 AvailableLimit { get; set; }

        [JsonProperty("offervalidTill")]
        public DateTime OffervalidTill { get; set; }

        [JsonProperty("offerTypeId")]
        public UInt32 OfferTypeId { get; set; }

        [JsonProperty("userId")]
        public uint UserId { get; set; }

        [JsonProperty("offerValidtill")]
        public DateTime OfferValidTill { get; set; }

        [JsonProperty("terms")]
        public string Terms { get; set; }

    }
}
