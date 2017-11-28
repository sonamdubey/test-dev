using Bikewale.Entities.UrlShortner;
using Bikewale.Notifications;
using Bikewale.Utility;
using Consumer;
using System.Collections.Generic;

namespace Bikewale.UserReviewsCommunication
{
    /// <summary>
    /// Created by : Aditi Srivastava on 15 Apr 2017
    /// Summary    : Send reminder email to user for review
    /// </summary>
    public class UserReviewMailBL
    {
        #region Private Variables
        private readonly UserReviewMailDAL userReviewMailRepo = new UserReviewMailDAL();
        private readonly UrlShortner url = new UrlShortner();
        #endregion

        #region Functions
        /// <summary>
        /// Created by : Aditi Srivastava on 15 Apr 2017
        /// Summary    : Send reminder email to user for review
        /// </summary>
        public void SendReminderToUser()
        {
            IEnumerable<UserReviewMailEntity> userList = GetCustomerMailData();
            try
            {
                if (userList != null)
                {
                    foreach (var user in userList)
                    {
                        UserReviewsEmails.SendReviewReminderEmail(user.CustomerName,
                            user.CustomerEmail,
                            user.ReviewLink,
                            string.Format("{0:dd MMM yyyy}", user.EntryDate),
                            user.MakeName,
                            user.ModelName);
                    }
                }
            }
            catch
            {
                Logs.WriteInfoLog("Error in UserReviewBL.SendReminderToUser");
            }
        }

        /// <summary>
        /// Created by : Aditi Srivastava on 15 Apr 2017
        /// Summary    : Get user data to be sent into the mail body
        /// </summary>
        public IEnumerable<UserReviewMailEntity> GetCustomerMailData()
        {
            IEnumerable<UserReviewMailEntity> customerEmailList = null;
            try
            {
                customerEmailList = userReviewMailRepo.GetUserList();
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
                Logs.WriteInfoLog("Error in UserReviewBL.GetCustomerMailData");
            }
            return customerEmailList;
        }
        /// <summary>
        /// Created by : Aditi Srivastava on 15 Apr 2017
        /// Summary    : Get encrypted url
        /// </summary>
        public string GetEncryptedUrlToken(string value)
        {

            string token = string.Empty;

            token = Utils.Utils.EncryptTripleDES(value);

            return token;
        }
        #endregion
    }
}
