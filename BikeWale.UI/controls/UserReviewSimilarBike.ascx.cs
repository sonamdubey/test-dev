using Bikewale.BindViewModels.Controls;
using Bikewale.Entities.UserReviews;
using System;
using System.Collections.Generic;
using System.Linq;
namespace Bikewale.Controls
{
    /// <summary>
    /// Created By :- Subodh Jain 17 Jan 2017
    /// Summary :- Get User Review Similar Bike 
    /// </summary>

    public class UserReviewSimilarBike : System.Web.UI.UserControl
    {
        public uint ModelId { get; set; }
        public uint TopCount { get; set; }
        public uint FetchCount { get; set; }
        protected IEnumerable<BikeUserReviewRating> userReviewList;
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            UserReviewsSimilarBike();
        }
        /// <summary>
        /// Created By :- Subodh Jain 17 Jan 2017
        /// Summary :- Get User Review Similar Bike 
        /// </summary>
        private void UserReviewsSimilarBike()
        {
            BindUserReviewsSimilarBike objUserReview = new BindUserReviewsSimilarBike();
            if (objUserReview != null)
            {
                userReviewList = objUserReview.GetUserReviewSimilarBike(ModelId, TopCount);
                if (userReviewList != null)
                {
                    FetchCount = Convert.ToUInt16(userReviewList.Count());
                }
            }
        }
    }
}