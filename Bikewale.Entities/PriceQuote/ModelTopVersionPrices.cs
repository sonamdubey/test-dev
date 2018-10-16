
using Bikewale.Entities.BikeData;
using Bikewale.Entities.Location;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Bikewale.Entities.PriceQuote
{
    [Serializable]
    public class ModelTopVersionPrices
    {
        [JsonProperty("bikeMake"), DataMember]
        public BikeMakeBase BikeMake { get; set; }
        [JsonProperty("bikeModel"), DataMember]
        public BikeModelTopVersion BikeModel { get; set; }
        [JsonProperty("cityPrice"), DataMember]
        public IEnumerable<CityPriceEntity> CityPrice { get; set; }
    }
}
