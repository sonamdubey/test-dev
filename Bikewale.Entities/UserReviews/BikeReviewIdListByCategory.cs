using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Entities.UserReviews
{
    /// <summary>
    /// Created by Sajal Gupta on 06-06-2017
    /// desc: reviews id list
    /// </summary>
    [Serializable]
    public class BikeReviewIdListByCategory
    {
        public IEnumerable<uint> RecentReviews { get; set; }
        public IEnumerable<uint> HelpfulReviews { get; set; }
        public IEnumerable<uint> PositiveReviews { get; set; }
        public IEnumerable<uint> NegativeReviews { get; set; }
        public IEnumerable<uint> NeutralReviews { get; set; }
    }
}
