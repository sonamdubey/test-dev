using Bikewale.BindViewModels.Webforms.Used;
using System;
using System.Web;

namespace Bikewale.Mobile.Used
{
    /// <summary>
    /// Created by: Sangram Nandkhile on 06 oct 2016
    /// </summary>
    public class Default : System.Web.UI.Page
    {
        public UsedBikeLandingPage viewModel;
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
            if (!IsPostBack)
            {
                viewModel = new UsedBikeLandingPage();
                if (viewModel == null)
                {
                    RedirectToPageNotFound();
                }
            }
        }

        private void RedirectToPageNotFound()
        {
            Response.Redirect("/pageNotFound.aspx", false);
            HttpContext.Current.ApplicationInstance.CompleteRequest();
            this.Page.Visible = false;
        }
    }
}