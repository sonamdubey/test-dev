using Bikewale.Notifications;
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
        #endregion

        #region Functions
        /// <summary>
        /// Created by : Aditi Srivastava on 18 Apr 2017
        /// Summary    : Send email to user after rating submission
        /// </summary>
        public void SendRatingEmailToUser()
        {
            IEnumerable<UserReviewsRatingEmailEntity> userList = userRatingRepo.GetUserList();
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
                            user.ModelName);
                    }
                }
            }
            catch
            {
                Logs.WriteInfoLog("Error in UserReviewsRatingEmailBL.SendRatingEmailToUser");
            }
        }

        #endregion

    }
}
