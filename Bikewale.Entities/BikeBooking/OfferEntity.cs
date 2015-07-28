using Bikewale.Entity.BikeBooking;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.Location;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Entity.BikeBooking
{
    /// <summary>
    /// Written By : Ashwini Todkar on 28 Oct 2014
    /// </summary>
    [Serializable]
    public class OfferEntity
    {
        public BikeModelEntityBase objModel { get; set; }
        public CityEntityBase objCity { get; set; }
        public NewBikeDealers objDealer { get; set; }

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
    }
}
