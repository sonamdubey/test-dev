using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Entities.UserReviews
{
    public class ReviewEntityBase
    {
        public int ReviewId { get; set; }
        public string ReviewTitle { get; set; }        
        public DateTime ReviewDate { get; set; }
        public string WrittenBy { get; set; }
    }
}
