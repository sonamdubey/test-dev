using BikewaleOpr.Entities.BikeData;
using BikewaleOpr.Entity.UserReviews;
using System.Collections.Generic;

namespace BikewaleOpr.Models.UserReviews
{
    /// <summary>
    /// Written By : Ashish G. Kamble on 18 Apr 2017
    /// Summary : Class have all properties required for manage user reviews page
    /// </summary>
    public class ManageUserReviewsPageVM
    {
        public IEnumerable<BikeMakeEntityBase> Makes { get; set; }
        public IEnumerable<ReviewBase> Reviews { get; set; }
        public ReviewsInputFilters selectedFilters { get; set; }
        public int currentUserId { get; set; }
    }   // class
}   // namespace
