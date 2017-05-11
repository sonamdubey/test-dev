using Bikewale.Entities.UrlShortner;
using Bikewale.Notifications;
using Bikewale.Utility;
using Consumer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.UserReviewsRatingEmail
{
    /// <summary>
    /// Created by : Aditi Srivastava on 18 Apr 2017
    /// Summary    : Functions to get list of users to send email to after submitting rating
    /// </summary>
    public class UserReviewsRatingEmailBL
    {
        #region Private variables
        private readonly UserReviewsRatingEmailDAL userRatingRepo = new UserReviewsRatingEmailDAL();
        private readonly UrlShortner url = new UrlShortner();
        #endregion

        #region Functions
        /// <summary>
        /// Created by : Aditi Srivastava on 18 Apr 2017
        /// Summary    : Send email to user after rating submission
        /// </summary>
        public void SendRatingEmailToUser()
        {
            IEnumerable<UserReviewsRatingEmailEntity> userList = GetCustomerMailData();
            try
            {
                if (userList != null)
                {
                    foreach (var user in userList)
                    {
                        UserReviewsEmails.SendRatingSubmissionEmail(
                            user.CustomerName,
                            user.CustomerEmail,
                            user.MakeName,
                            user.ModelName,
                            user.ReviewLink);
                    }
                }
            }
            catch
            {
                Logs.WriteInfoLog("Error in UserReviewsRatingEmailBL.SendRatingEmailToUser");
            }
        }

        /// <summary>
        /// Created by : Aditi Srivastava on 15 Apr 2017
        /// Summary    : Get user data to be sent into the mail body
        /// </summary>
        public IEnumerable<UserReviewsRatingEmailEntity> GetCustomerMailData()
        {
            IEnumerable<UserReviewsRatingEmailEntity> customerEmailList = null;
            try
            {
                customerEmailList = userRatingRepo.GetUserList();
                foreach (var user in customerEmailList)
                {
                    string qEncoded = GetEncryptedUrlToken(string.Format("reviewid={0}&makeid={1}&modelid={2}&overallrating={3}&customerid={4}&priceRangeId={5}&userName={6}&emailId={7}&pagesourceid={8}&isFake={9}"
                                 , user.ReviewId, user.MakeId,
                                 user.ModelId, user.OverAllRating,
                                 user.CustomerId, user.PriceRangeId,
                                 user.CustomerName, user.CustomerEmail,
                                 user.PageSourceId, user.IsFake));
                    string reviewUrl = string.Format("{0}/write-a-review/?q={1}", BWConfiguration.Instance.BwHostUrl, qEncoded);
                    UrlShortnerResponse shortUrl = url.GetShortUrl(reviewUrl);
                    if (shortUrl != null)
                        user.ReviewLink = shortUrl.ShortUrl;
                    else
                        user.ReviewLink = reviewUrl;
                }
            }
            catch
            {
                Logs.WriteInfoLog("Error in UserReviewsRatingEmailBL.GetCustomerMailData");
            }
            return customerEmailList;
        }

        /// <summary>
        /// Created by : Aditi Srivastava on 28 Apr 2017
        /// Summary    : Get encrypted url
        /// </summary>
        private string GetEncryptedUrlToken(string value)
        {

            string token = string.Empty;

            token = Utils.Utils.EncryptTripleDES(value);

            return token;
        }

        #endregion

    }
}
