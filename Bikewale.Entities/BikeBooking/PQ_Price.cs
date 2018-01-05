using System;
using System.Runtime.Serialization;

namespace Bikewale.Entities.BikeBooking
{

    /// <summary>
    /// Modified by : Ashutosh Sharma on 30 Aug 2017 
    /// Description : Removed IsGstPrice property    
    /// </summary>
    [Serializable, DataContract]
    public class PQ_Price
    {
        [DataMember]
        public UInt32 CategoryId { get; set; }
        [DataMember]
        public string CategoryName { get; set; }
        [DataMember]
        public UInt32 Price { get; set; }
        [DataMember]
        public UInt32 DealerId { get; set; }

    }
}
