using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikewaleOpr.Entity.UserReviews
{
    /// <summary>
    /// Written By : Ashish G. Kamble on 19 Apr 2017
    /// Summary : Class to get the required properties to update the user review info
    /// </summary>
    public class UpdateReviewsInputEntity
    {
        public uint ReviewId { get; set; }
        public ReviewsStatus ReviewStatus { get; set; }
        public uint ModeratorId { get; set; }
        public ushort DisapprovalReasonId { get; set; } 
        public string Review { get; set; }
        public string ReviewTitle { get; set; }
        public string ReviewTips { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public string BikeName { get; set; }        
    }
}
