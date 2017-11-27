using Bikewale.BindViewModels.Webforms;
using Bikewale.Common;
using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Bikewale.BikeBooking
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
            if (!IsPostBack)
            {
                // Modified By :Lucky Rathore on 12 July 2016.
                Form.Action = Request.RawUrl;
                // Modified By :Ashish Kamble on 5 Feb 2016
                string originalUrl = Request.ServerVariables["HTTP_X_ORIGINAL_URL"];
                if (String.IsNullOrEmpty(originalUrl))
                    originalUrl = Request.ServerVariables["URL"];

                DeviceDetection dd = new DeviceDetection(originalUrl);
                dd.DetectDevice();
            }
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