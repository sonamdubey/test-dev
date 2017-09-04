using Bikewale.Entities.BikeData;
using Bikewale.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Entities.UserReviews
{
    /// <summary>
    /// Created by : Snehal Dange on 1st Sep 2017
    /// Summary     : Entity for Rate bike page (1st page)
    /// </summary>
    public class UserReviewRatingData 
    {
        public BikeModelEntity ObjModelEntity { get; set; }
        public string OverAllRatingText { get; set; }
        public string RatingQuestion { get; set; }
        public string ErrorMessage { get; set; }
        public uint PriceRangeId { get; set; }
        public string ReviewsOverAllrating { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public uint ReviewId { get; set; }
        public bool IsFake { get; set; }
        public uint SelectedRating { get; set; }
        public string ReturnUrl { get; set; }
        public ushort SourceId { get; set; }
        public int ContestSrc { get; set; }
        public ushort PlatFormId { get; set; }

        public string UtmzCookieValue { get; set; }

    }
}
