using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Bikewale.Entities.Compare
{
    [Serializable, DataContract]
    public class CompareReviewsData
    {
        [DataMember]
        public CompareMainCategory CompareReviews { get; set; }
        [DataMember]
        public IEnumerable<MostHelpfulReviewObject> MostHelpfulReviews { get; set; }
        [DataMember]
        public IEnumerable<UserReviewComparisonObject> OverallRating { get; set; }
    }
}
