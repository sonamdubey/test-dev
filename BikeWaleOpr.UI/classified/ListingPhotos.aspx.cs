using BikeWaleOpr.Common;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

/// <summary>
/// Created By Sanjay Soni ON 1/10/2014
/// </summary>
namespace BikeWaleOpr.Classified
{
    public class ListingPhotos : System.Web.UI.Page
    {
        // asp control variables
        protected Repeater rptCustomerVerifiedPhotos, rptCustomerUnVerifiedPhotos, rptCustomerFakePhotos;

        protected int ProfileId = 0;

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
            BindRepeater();
        }

        protected void BindRepeater()
        {
            DataSet ds = null;
            ClassifiedCommon cc = new ClassifiedCommon();
            ProcessQS();
            ds = cc.CustomerTotalListingPhotos(ProfileId);

            if (ds.Tables[0] != null)
            {
                rptCustomerVerifiedPhotos.DataSource = ds.Tables[0];
                rptCustomerVerifiedPhotos.DataBind();
            }

            if (ds.Tables[1] != null)
            {
                rptCustomerUnVerifiedPhotos.DataSource = ds.Tables[1];
                rptCustomerUnVerifiedPhotos.DataBind();
            }

            if (ds.Tables[2] != null)
            {
                rptCustomerFakePhotos.DataSource = ds.Tables[2];
                rptCustomerFakePhotos.DataBind();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected void ProcessQS()
        {
            try
            {
                ProfileId = Convert.ToInt32(Request.QueryString["ProfileId"]);
                Trace.Warn("Profile Id", ProfileId.ToString());
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("BindRepeater ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }

        /// <summary>
        /// Created By : Sanjay Soni ON 30/9/2014
        /// Description : To Approve Selected Photos
        /// </summary>
        /// <param name="photoIdList"></param>
        protected void approvePhotos(string photoIdList)
        {
            ClassifiedCommon cc = new ClassifiedCommon();
            cc.ApproveSelectedPhotos(photoIdList);
        }

        /// <summary>
        /// Created By : Sanjay Soni ON 30/9/2014
        /// Description : To Discard Selected Photos
        /// </summary>
        /// <param name="photoIdList"></param>
        protected void discardPhotos(string photoIdList)
        {
            ClassifiedCommon cc = new ClassifiedCommon();
            cc.DiscardSelectedPhotos(photoIdList);
        }
    } // END CLASS
} // END NAMESPACE