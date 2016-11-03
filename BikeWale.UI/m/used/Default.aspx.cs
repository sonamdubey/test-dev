using Bikewale.BindViewModels.Webforms.Used;
using Bikewale.Mobile.Controls;
using System;
using System.Web;

namespace Bikewale.Mobile.Used
{
    /// <summary>
    /// Created by: Sangram Nandkhile on 06 oct 2016
    /// Summary: Landing page for Used msite
    /// </summary>
    public class Default : System.Web.UI.Page
    {
        protected UsedBikeLandingPage viewModel;
        protected UsedRecentBikes ctrlRecentUsedBikes;
        protected int topCount = 6;

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