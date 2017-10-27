using Bikewale.Entities.BikeData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Entities.Compare
{
    /// <summary>
    /// Created By :Snehal Dange on 27th Oct 2017
    /// Description: Entity created to show all data related to similar bike and compared bike together.
    /// </summary>
    [Serializable, DataContract]
    public class SimilarBikeComparisonWidget
    {
        [DataMember]
        public BikeMakeBase BikeMake { get; set; }
        [DataMember]
        public BikeModelEntityBase BikeModel { get; set; }

        [DataMember]
        public string OriginalImagePath { get; set; }
        public string HostUrl { get; set; }
        [DataMember]
        public BasicBikeEntityBase CompareBike1 { get; set; }
        [DataMember]
        public BasicBikeEntityBase CompareBike2 { get; set; }
    }
}
