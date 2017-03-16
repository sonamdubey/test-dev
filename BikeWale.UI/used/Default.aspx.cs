using Bikewale.BindViewModels.Webforms.Used;
using Bikewale.Common;
using Bikewale.Controls;
using System;
using System.Web;

namespace Bikewale.Used
{
    /// <summary>
    /// Created by: Subodh Jain on 20 oct 2016
    /// Summary: Landing page for Used desktop site
    /// </summary>
    public class Default : System.Web.UI.Page
    {
        protected UsedBikeLandingPage viewModel;
        protected UsedRecentBikes ctrlRecentUsedBikes;
        protected UsedBikeInCities ctrlusedBikeInCities;
        protected int topCount = 10;
        protected string currentUser = string.Empty;
        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
        }

        protected void InitializeComponent()
        {
            base.Load += new EventHandler(Page_Load);
        }

        /// <summary>
        /// Modified by : Sajal Gupta on 21/11/2016
        /// Desc : Added device detection
        /// </summary>
        protected void Page_Load(object sender, EventArgs e)
        {
            string originalUrl = Request.ServerVariables["HTTP_X_ORIGINAL_URL"];
            if (String.IsNullOrEmpty(originalUrl))
                originalUrl = Request.ServerVariables["URL"];

            Bikewale.Common.DeviceDetection dd = new Bikewale.Common.DeviceDetection(originalUrl);
            dd.DetectDevice();

            viewModel = new UsedBikeLandingPage(topCount);//topcount=number of icons to be displayed on page
            currentUser = CurrentUser.Id;
            if (viewModel == null)
            {
                RedirectToPageNotFound();
            }
            RenderUserControls();
        }

        private void RedirectToPageNotFound()
        {
            Response.Redirect("/pageNotFound.aspx", false);
            HttpContext.Current.ApplicationInstance.CompleteRequest();
            this.Page.Visible = false;
        }
        /// Modified By :-Subodh Jain on 15 March 2017
        /// Summary :-Made count for other used bike 9
        private void RenderUserControls()
        {
            ctrlRecentUsedBikes.WidgetTitle = "Popular used bikes";
            ctrlRecentUsedBikes.TopCount = 9;
        }
    }
}