using Carwale.Interfaces.Author;
using Carwale.Notifications;
using Carwale.Service;
using Carwale.UI.Common;
using Carwale.UI.Controls;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Carwale.UI.Authors
{
    public class Default : System.Web.UI.Page
    {
        protected Repeater rptAuthorList;
        protected NewsRightWidget ctrlNewsRightWidget;
        protected PopularVideoWidget ctrlPopularVideoWidget;
        protected string altURL;

        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
        }

        void InitializeComponent()
        {
            base.Load += new EventHandler(this.Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            var rewrittenURL = Request.ServerVariables["HTTP_X_REWRITE_URL"];
            if (!string.IsNullOrEmpty(rewrittenURL))
            {
                DeviceDetection dd = new DeviceDetection(Request.ServerVariables["HTTP_X_REWRITE_URL"].ToString());
                dd.DetectDevice();
            }
            if (!Page.IsPostBack)
            {
                FillAuthorList();
                FillRightPanel();
                altURL = "/authors/";
            }
        }


        /// <summary>
        /// Populates the entire author list
        /// </summary>
        private void FillAuthorList()
        {
            try
            {               
                IAuthorRepository authorContainer = UnityBootstrapper.Resolve<IAuthorRepository>();
                rptAuthorList.DataSource = authorContainer.GetAuthorsList();
                rptAuthorList.DataBind();
            }       
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }


        /// <summary>
        /// Populates the right panel of popular,recent news and videos
        /// </summary>
        private void FillRightPanel()
        {
            try
            {
                ctrlNewsRightWidget.NumberofRecords = 5;
                ctrlNewsRightWidget.CategoryId = 1;
                ctrlNewsRightWidget.BasicId = 1;
                ctrlNewsRightWidget.PopulateNewsWidget();
                ctrlPopularVideoWidget.Tag = "";
                ctrlPopularVideoWidget.populateVideoWidget();
            }

            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }
    }
}