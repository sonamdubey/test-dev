using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using BikeWale.Entities.AutoBiz;

namespace BikeWale.Entities.AutoBiz
{
    /// <summary>
    /// Written By : Ashwini Todkar on 28 Oct 2014
    /// </summary>
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
