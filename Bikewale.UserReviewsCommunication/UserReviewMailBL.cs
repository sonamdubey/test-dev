using Bikewale.Entities.UrlShortner;
using Bikewale.Utility;
using Bikewale.Notifications;
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
                    string token = userReviewMailRepo.GetEncryptedUrlToken(string.Format("customerid={0}&reviewid={1}",user.CustomerId,user.ReviewId));
                    // review link to be changed later. Not in use currently
                    string reviewUrl=string.Format("{0}/userreviews/edit/?{1}", BWConfiguration.Instance.BwHostUrl,token);
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
        #endregion
    }
}
