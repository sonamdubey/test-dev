using Consumer;
using System;

namespace Bikewale.UserReviewsRatingEmail
{
    class Program
    {
        static void Main()
        {
            Logs.WriteInfoLog("Started rating submission email job");
            try
            {
                Logs.WriteInfoLog("Started SendRatingSubmissionEmailForUserReview");
                (new UserReviewsRatingEmailBL()).SendRatingEmailToUser();
                Logs.WriteInfoLog("Successfully ended SendRatingSubmissionEmailForUserReview");
            }
            catch (Exception ex)
            {
                Logs.WriteErrorLog("Exception in Main : " + ex.Message);
            }
         Logs.WriteInfoLog("Ended rating submission email job");
        }
    }
}
