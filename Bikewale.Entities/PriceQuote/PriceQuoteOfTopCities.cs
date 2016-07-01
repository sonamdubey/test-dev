///Created By Vivek Gupta on 20-05-2016
///This entity is used to carry data for top city prices
using System;
using System.Runtime.Serialization;
namespace Bikewale.Entities.PriceQuote
{
    [Serializable, DataContract]
    public class PriceQuoteOfTopCities
    {
        [DataMember]
        public string CityName { get; set; }
        [DataMember]
        public string CityMaskingName { get; set; }
        [DataMember]
        public uint OnRoadPrice { get; set; }
        [DataMember]
        public string Make { get; set; }
        [DataMember]
        public string MakeMaskingName { get; set; }
        [DataMember]
        public string Model { get; set; }
        [DataMember]
        public string ModelMaskingName { get; set; }

    }
}