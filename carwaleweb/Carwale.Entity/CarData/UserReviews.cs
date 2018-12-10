using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.CarData
{
    /// <summary>
    /// Created By : Shalini Nair
    /// Modified By:Shalini Nair on 18/11/14; Added FormattedDateTime 
    /// </summary>
    [Serializable]
    public class UserReviews
    {
        public string MakeName { get; set; }
        public string MaskingName { get; set; }
        public long CustomerReviewId { get; set; }
        public string Title { get; set; }
        public int OverallRating { get; set; }
        public string CarName { get; set; }
        public string CustomerName { get; set; }
        public string HandleName { get; set; }
        public DateTime EntryDateTime { get; set; }
        public string Comments { get; set; }
        public int CommentsCount { get; set; }
        public string FormattedDateTime { get; set; }
    }
}
