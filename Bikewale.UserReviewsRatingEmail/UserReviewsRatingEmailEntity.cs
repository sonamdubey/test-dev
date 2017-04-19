using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.UserReviewsRatingEmail
{
    public class UserReviewsRatingEmailEntity
    {
        public uint ReviewId { get; set; }
        public string MakeName { get; set; }
        public string ModelName { get; set; }
        public uint CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public string ReviewLink { get; set; }
    }
}
