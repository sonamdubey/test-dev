using Bikewale.BindViewModels.Webforms;
using Bikewale.DAL.BikeBooking;
using Bikewale.Entities.BikeBooking;
using Bikewale.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Bikewale.Mobile.bikebooking
{
    public partial class Cancellation : System.Web.UI.Page
    {
        public string BWid = string.Empty;
        protected Button feedbackBtn;
        protected HiddenField hdnBwid;
        protected HtmlTextArea FeedBackText;

        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
            feedbackBtn.Click += new EventHandler(feedbackBtn_Click);
        }
		
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
                if (!String.IsNullOrEmpty(FeedBackText.InnerText) && (!string.IsNullOrEmpty(BWid)))
                {
                    FeedbackCancellationModel cancelled = new FeedbackCancellationModel();
                    cancelled.ProcessFeedbackEmail(BWid, FeedBackText.InnerText);
                }
                Response.Redirect("https://www.bikewale.com", false);
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass.LogError(ex, "Cancellation.feedbackBtn_Click");
                
            }
        }        
    }
}