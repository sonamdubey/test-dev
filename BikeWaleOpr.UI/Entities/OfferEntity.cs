using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace BikeWaleOpr.Entities
{
    [Serializable]
    public class OfferEntity
    {
        public MakeEntityBase objMake { get; set; }
        public ModelEntityBase objModel { get; set; }
        public VersionEntityBase objVersion { get; set; }
        public CityEntityBase objCity { get; set; }
        public NewBikeDealers objDealer { get; set; }

        [JsonProperty("offerId")]
        public UInt32 OfferId { get; set; }

        [JsonProperty("availabilityId")]
        public UInt32 AvailabilityId { get; set; }

        [JsonProperty("AvailableLimit")]
        public UInt16 AvailableLimit { get; set; }

        [JsonProperty("offerCategoryId")]
        public UInt32 OfferCategoryId { get; set; }

        [JsonProperty("offerType")]
        public string OfferType { get; set; }

        [JsonProperty("offerText")]
        public string OfferText { get; set; }

        [JsonProperty("offerValue")]
        public UInt32 OfferValue { get; set; }

        [JsonProperty("offerValidtill")]
        public DateTime OfferValidTill { get; set; }

        [JsonProperty("offerTypeId")]
        public UInt32 OfferTypeId { get; set; }

        [JsonProperty("userId")]
        public UInt32 UserId { get; set; }
    }
}
