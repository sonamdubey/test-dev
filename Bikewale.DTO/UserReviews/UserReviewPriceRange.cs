using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.DTO.UserReviews
{
    /// <summary>
    /// Created by Snehal Dange on 01-09-2017
    /// Description: For user review price range
    /// </summary>
  public  class UserReviewPriceRange
    {
        public uint Id { get; set; }
        public uint RangeId { get; set; }
        public uint MinPrice { get; set; }
        public uint MaxPrice { get; set; }
        public uint QuestionId { get; set; }
    }
}
