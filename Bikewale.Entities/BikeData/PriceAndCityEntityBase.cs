using Bikewale.Entities.Location;
using System;
using System.Runtime.Serialization;

namespace Bikewale.Entities.BikeData
{
    /// <summary>
    /// Created by : Vivek Singh Tomar on 27th Sep 2017
    /// Summary : Entity to hold price and city entity
    /// </summary>
    [Serializable, DataContract]
    public class PriceAndCityEntityBase
    {
        [DataMember]
        public PriceEntityBase Price { get; set; }
        [DataMember]
        public CityEntityBase City { get; set; }
    }
}
