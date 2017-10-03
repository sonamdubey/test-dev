using System;
using System.Runtime.Serialization;

namespace Bikewale.Entities.BikeData
{
    /// <summary>
    /// Created by : Vivek Singh Tomar on 27th Sep 2017
    /// Summary : Entity to hold price
    /// </summary>
    [Serializable, DataContract]
    public class PriceEntityBase
    {
        [DataMember]
        public UInt32 MinPrice { get; set; }
        [DataMember]
        public UInt32 MaxPrice { get; set; }
        [DataMember]
        public UInt32 AvgPrice { get; set; }
    }
}
