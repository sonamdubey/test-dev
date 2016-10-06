using Bikewale.BindViewModels.Webforms.Used;
using Bikewale.Mobile.Controls;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.Location;
using Bikewale.Interfaces.Location;
using System;
using System.Web;

namespace Bikewale.Mobile.Used
{
    /// <summary>
    /// Created by: Sangram Nandkhile on 06 oct 2016
    /// </summary>
    public class Default : System.Web.UI.Page
    {
        protected UsedBikeLandingPage viewModel;
        protected UsedRecentBikes ctrlRecentUsedBikes;

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
            viewModel = new UsedBikeLandingPage();
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