using Bikewale.DAL.BikeBooking;
using Bikewale.Entities.BikeBooking;
using Bikewale.Notifications;
using Carwale.Notifications;
using Carwale.Notifications.MailTemplates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Bikewale.BikeBooking
{
    public partial class cancellation : System.Web.UI.Page
    {
        public string BWid = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(hdnBwid.Value))
                BWid = hdnBwid.Value;
        }
        /// <summary>
        /// Save feedback on submit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void feedbackBtn_Click(object sender, EventArgs e)
        {
            try
            {
                FeedBackEntity feedBkEntity = new FeedBackEntity()
                {
                    BwId = BWid,
                    FeedBack = FeedBackText.InnerText
                };
                FeedBacksRespositry feedback = new FeedBacksRespositry();
                feedback.SaveFeedBack(feedBkEntity);

                // send emails
                SendMails mail = new SendMails();
                mail.SendMail("contact@bikewale.com", "Feedback from a customer who has cancelled booking - " + BWid, ComposeBody(), "noreply@bikewale.com", null, new string[] { "piyush@bikewale.com", "saurabh@bikewale.com" });
                Response.Redirect("http://www.bikewale.com", false);
            }
            catch (Exception ex)
            {
                Bikewale.Common.ErrorClass objErr = new Bikewale.Common.ErrorClass(ex, "Bikewale.Notification.PreBookingConfirmationMailToDealer.ComposeBody");
                objErr.SendMail();
            }
        }

        // Compose email
        private string ComposeBody()
        {
            try
            {
                StringBuilder sb = null;
                try
                {
                    sb = new StringBuilder();
                    sb.Append(string.Format("<div>Dear Team,<br /><div><br /><div>Customer with Booking id :<b>{0}&nbsp;</b>has cancelled booking and following is the feedback we've received.</div></div><div><br /></div><div><b>Feedback:</b></div><div><br />{1}</div></div>", BWid, FeedBackText.InnerText));
                }
                catch (Exception ex)
                {
                    Bikewale.Common.ErrorClass objErr = new Bikewale.Common.ErrorClass(ex, "Bikewale.Notification.PreBookingConfirmationMailToDealer.ComposeBody");
                    objErr.SendMail();
                }
                return sb.ToString();
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }
    }
}