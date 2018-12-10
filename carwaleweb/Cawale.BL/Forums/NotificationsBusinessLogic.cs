using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlTypes;
using System.Data;
using System.Web;
using System.Web.Security;
using Carwale.Notifications;
using Carwale.Notifications.MailTemplates;
using Carwale.DAL.CoreDAL;
using Carwale.DAL.Forums;

namespace Carwale.BL.Forums
{
    public class NotificationsBusinessLogic
    {
        #region Send Mails To Users
        /// <summary>
        /// Send mail to people who have subscribed to a thread.
        /// </summary>
        /// <param name="discussionUrl"></param>
        /// <param name="threadId"></param>
        /// <param name="threadName"></param>
        public void NotifySubscribers(string discussionUrl, string threadId, string threadName,string handleName, string customerId,string eMail)
        {
            DataSet ds = new DataSet();
            string replierName = customerId == "-1" ? "Anonymous" : handleName;
            string url = "https://www.carwale.com" + discussionUrl;
           
            try
            {
                NotificationsDAL dal = new NotificationsDAL();
                ds = dal.GetSubscribers(discussionUrl, threadId, threadName, handleName, customerId, eMail);
                EmailNotifySubscribers(ds, replierName, threadName, url);
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err,"Notifications DAL");
                objErr.SendMail();
            }
            
        }
        #endregion

        /// <summary>
        /// Sends the mails to the subscribers
        /// </summary>
        /// <param name="ds"></param>
        /// <param name="replierName"></param>
        /// <param name="threadName"></param>
        /// <param name="url"></param>
        public void EmailNotifySubscribers(DataSet ds,string replierName, string threadName, string url)
        {
            foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    string name = dr["Name"].ToString();
                    string email = dr["Email"].ToString();
                    if (email.ToLower() != Email.ToLower())//send mail to the thread participants
                    {
                    //    Mails.NotifyForumSubscribedUsers(email, name, replierName, threadName, url);
                        NotifyForumSubscribedUsersTemplate template = new NotifyForumSubscribedUsersTemplate(name, replierName, threadName, url);
                        template.Send(email, "Reply to discussion '" + template.Topic + "'");
                    }
                }
        }

        /// <summary>
        /// Get email of Current User
        /// </summary>
        public string Email
        {
            get
            {
                string email = "";
                if (HttpContext.Current.User.Identity.IsAuthenticated == true)
                {
                    FormsIdentity fi = (FormsIdentity)HttpContext.Current.User.Identity;
                    FormsAuthenticationTicket ticket = fi.Ticket;
                    email = ticket.UserData.Split(':')[1].ToString();
                }

                return email;
            }
        }
    }
}
