using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.DTO.UserReviews
{
    public class BikeReviewsData
    {
        public uint TotalReviews { get; set; }
        public uint MostHelpfulReviews { get; set; }
        public uint MostRecentReviews { get; set; }
        public uint PostiveReviews { get; set; }
        public uint NegativeReviews { get; set; }
        public uint NeutralReviews { get; set; }
    }
}
