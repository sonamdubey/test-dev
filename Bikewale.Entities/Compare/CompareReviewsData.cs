using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Entities.Compare
{
    [Serializable, DataContract]
    public class CompareReviewsData
    {
        [DataMember]
        public CompareMainCategory CompareReviews { get; set; }
        [DataMember]
        public IEnumerable<MostHelpfulReviewObject> MostHelpfulReviewList { get; set; }
        [DataMember]
        public IEnumerable<UserReviewComparisonObject> OverallRatingObject { get; set; }
    }
}
