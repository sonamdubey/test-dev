using Consumer;
using System;

namespace Bikewale.UserReviewsCommunication
{
    class Program
    {
        static void Main()
        {
            Logs.WriteInfoLog("Started follow up email for review job");

            try
            {
                Logs.WriteInfoLog("Started SendEmailRemindersForUserReview");
                (new UserReviewMailBL()).SendReminderToUser();
                Logs.WriteInfoLog("Successfully ended SendEmailRemindersForUserReview");
            }
            catch (Exception ex)
            {
                Logs.WriteErrorLog("Exception in Main : " + ex.Message);
            }
         Logs.WriteInfoLog("Ended follow up email for review job");
        }
    }
}
