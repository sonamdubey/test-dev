using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Entities.Compare.V2
{
    /// <summary>
    /// Created By  :   Snehal Dange on 11 Sep 2017
    /// Description :   Bike Compare Entity with Reviews properties
    /// </summary>
    [Serializable, DataContract]
    public class BikeCompareEntity
    {
        [DataMember]
        public IEnumerable<BikeEntityBase> BasicInfo { get; set; }
        [DataMember]
        public IEnumerable<BikeSpecification> Specifications { get; set; }
        [DataMember]
        public IEnumerable<BikeFeature> Features { get; set; }
        [DataMember]
        public IEnumerable<BikeColor> Color { get; set; }
        [DataMember]
        public IEnumerable<BikeReview> Reviews { get; set; }

        [DataMember]
        public CompareMainCategory CompareSpecifications { get; set; }
        [DataMember]
        public CompareMainCategory CompareFeatures { get; set; }
        [DataMember]
        public CompareBikeColorCategory CompareColors { get; set; }

        [DataMember]
        public CompareMainCategory CompareReviews { get; set; }
    }
}
