using System;
using System.Collections.Generic;
using System.Web;
using Carwale.UI.Common;
using Carwale.DAL.Forums;
/// <summary>
/// Summary description for SaveUserActivity
/// </summary>
/// 
namespace Carwale.UI.Common
{
    public class SaveUserActivity
    {
        public void saveActivity(string customerId, string activityId, string categoryId, string threadId, string sessionId)
        {
            if (CurrentUser.Id.Equals("-1") || new CWCommon().IsSearchEngine() == true)
                return;
            UserDAL userDetails = new UserDAL();
            if (ForumCookies.ForumUserTrackingCookie != (customerId + "," + activityId + "," + categoryId + "," + threadId))
            {
                ForumCookies.ForumUserTrackingCookie = customerId + "," + activityId + "," + categoryId + "," + threadId;
                userDetails.SaveActivity(customerId, activityId, categoryId, threadId, sessionId);
            }
        }
    }
}