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
        public List<uint> RecentReviews { get; set; }
        public List<uint> HelpfulReviews { get; set; }
        public List<uint> PositiveReviews { get; set; }
        public List<uint> NegativeReviews { get; set; }
        public List<uint> NeutralReviews { get; set; }
    }
}
