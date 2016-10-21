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

        protected void Page_Load(object sender, EventArgs e)
        {
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

        private void RenderUserControls()
        {
            ctrlRecentUsedBikes.WidgetTitle = "Recently uploaded used bikes";
            ctrlRecentUsedBikes.TopCount = 6;
        }
    }
}