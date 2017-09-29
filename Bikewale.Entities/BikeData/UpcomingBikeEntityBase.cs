using Bikewale.Entities.Images;
using System;
using System.Runtime.Serialization;

namespace Bikewale.Entities.BikeData
{
    /// <summary>
    /// Created by : Vivek Singh Tomar on 27th Sep 2017 
    /// </summary>
    [Serializable, DataContract]
    public class UpcomingBikeEntityBase
    {
        [DataMember]
        public DateTime ExpectedLaunch { get; set; }
        [DataMember]
        public uint Count { get; set; }
        [DataMember]
        public BikeMakeBase BikeMake { get; set; }
        [DataMember]
        public BikeModelEntityBase BikeModel { get; set; }
        [DataMember]
        public ImageEntityBase BikeImage { get; set; }
        [DataMember]
        public PriceEntityBase ExpectedPrice { get; set; }
        
    }
}
