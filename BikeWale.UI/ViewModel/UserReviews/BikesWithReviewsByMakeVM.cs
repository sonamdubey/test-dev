using Bikewale.Entities.UserReviews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Models.UserReviews
{
    /// <summary>
    /// Created by:Snehal Dange on 20th Nov 2017
    /// Description: View model for most helpful and most recent reviews for popular bikes by make
    /// </summary>
    public class BikesWithReviewsByMakeVM
    {
         public IEnumerable<BikesWithReviewByMake> BikesReviewsList { get; set; }
    }
}
