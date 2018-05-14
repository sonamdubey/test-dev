using Bikewale.Entities.BikeData;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Bikewale.Entities.Compare
{
    /// <summary>
    /// Created By  :   Sumit Kate on 22 Jan 2016
    /// Description :   Bike Compare Entity
    /// Modified by sajal Gupta on 13-9-2017
    /// Descriptipn: Added Reviews, CompareReviews
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

        public SpecsFeaturesEntity VersionSpecsFeatures { get; set; }
        [DataMember]
        public IEnumerable<BikeColor> Color { get; set; }
        [DataMember]
        public IEnumerable<BikeReview> Reviews { get; set; }
        [DataMember]
        public CompareBikeColorCategory CompareColors { get; set; }        
        [DataMember]
        public CompareReviewsData UserReviewData { get; set; }
    }
}
