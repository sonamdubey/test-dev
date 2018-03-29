using Bikewale.Entities.Images;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Bikewale.Entities.BikeData
{
    /// <summary>
    /// Created by : Vivek Singh Tomar on 27th Sep 2017
    /// Modified by : Pratibha Verma on 28 Mar 2018
    /// Description : Added SpecsItem List
    /// </summary>
    [Serializable, DataContract]
    public class NewBikeEntityBase
    {
        public float Rating { get; set; }
        [DataMember]
        public BikeMakeBase BikeMake { get; set; }
        [DataMember]
        public BikeModelEntityBase BikeModel { get; set; }
        [DataMember]
        public BikeVersionsListEntity objVersion { get; set; }
        [DataMember]
        public PriceEntityBase Price { get; set; }
        [DataMember]
        public ImageEntityBase BikeImage { get; set; }
        [DataMember]
        public uint Count { get; set; }
        public ushort BodyStyle { get; set; }
        public IEnumerable<SpecsItem> MinSpecsList { get; set; }
    }
}
