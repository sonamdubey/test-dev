using Bikewale.Entities.UserReviews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Utility
{
    /// <summary>
    /// Created by : Aditi Srivastava on 19 Apr 2017
    /// Summary    : Utility for user reviews
    /// </summary>
    public class UserReviews
    {
        /// <summary>
        /// Summary : Format previous page url 
        /// </summary>
        public static string FormatPreviousPageUrl(UserReviewPageSourceEnum pageSource,string makeMasking, string modelMasking)
        {
            string url = string.Empty;
            switch(pageSource)
            {
                case UserReviewPageSourceEnum.ModelPage_Mobile:
                    url=string.Format("/m/{0}-bikes/{1}/",makeMasking,modelMasking);
                    break;
                case UserReviewPageSourceEnum.UserReview_Listing_Mobile:
                    url =string.Format("/m/{0}-bikes/{1}/user-reviews/",makeMasking,modelMasking);
                    break;
                default:
                    url = "";
                    break;
            }
            return url;
        }
    }
}
