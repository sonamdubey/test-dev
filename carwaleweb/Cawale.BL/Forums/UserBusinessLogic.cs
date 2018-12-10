using Carwale.DAL.Forums;
using Carwale.Entity.Forums;
using Carwale.Notifications;
using System;
using System.Web;

namespace Carwale.BL.Forums
{
    public class UserBusinessLogic
    {
        /// <summary>
        /// Sets Cookie for login
        /// </summary>
        /// <param name="lastLogin"></param>
        UserDAL dal = new UserDAL();    
        public void SetUserCookie(DateTime lastLogin)
        {
            HttpCookie lastLoginCookie = new HttpCookie("ForumLastLogin");
            lastLoginCookie.Value = lastLogin.ToString();
            lastLoginCookie.Expires = DateTime.Now.AddHours(2);
            HttpContext.Current.Response.Cookies.Add(lastLoginCookie);
        }

        #region Manage Last Login
        /// <summary>
        /// This method gets the last login time of the user and updates the same to current date time.
        /// </summary>
        public void ManageLastLogin(string customerId, string forumLastLogin)
        {
            DateTime lastLogin = new DateTime();           
            if (customerId != "-1") // user is logged in
            {
                if (forumLastLogin == null) // cookie is not available.
                {
                    try    // try fetching last login from database.
                    {
                        lastLogin = dal.GetLastLogin(customerId, forumLastLogin);

                        if (lastLogin.Year < 2000)// in case not available in db, keep last visit as two days ago.
                        {
                            lastLogin = DateTime.Now.AddDays(-2);
                        }
                        SetUserCookie(lastLogin);
                    }
                    catch (Exception err)
                    {
                        ErrorClass objErr = new ErrorClass(err, "UserBusinessLogic - ManageLastLogin.");
                        objErr.SendMail();
                    }
                }
            }

        }
        #endregion
        public UserProfile GetProfileDetails(int UserId)
        {                         
           return dal.GetProfileDetails(UserId);
        }

        public UserProfile GetExistingHandleDetails(int UserId)
        {
            return dal.GetExistingHandleDetails(UserId);
        }

        public bool InsertHandle(int UserId,string HandleName,bool IsUpdated)
        {
            return dal.InsertHandle(UserId, HandleName, IsUpdated);
        }

        public bool InsertImages(string userId, UserProfile param)
        {
            return dal.InsertImages(userId, param);
        }
    }
}
