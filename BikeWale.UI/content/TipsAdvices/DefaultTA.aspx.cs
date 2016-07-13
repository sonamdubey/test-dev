using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Bikewale.Common;

namespace Bikewale.Content
{
    public class DefaultTA : System.Web.UI.Page
    {
        protected RepeaterPager rpgTipsAdvices;
        protected Repeater rptTipsAdvices, rptUCL;
        protected DropDownList ddlUsedBikeLocations, ddlMake;

        protected string SelectClause = string.Empty, FromClause = string.Empty, WhereClause = string.Empty,
                             OrderByClause = string.Empty, RecordCntQry = string.Empty, BaseUrl = string.Empty;

        private string pageNumber = string.Empty;

        protected override void OnInit(EventArgs e)
        {
            rptTipsAdvices = (Repeater)rpgTipsAdvices.FindControl("rptTipsAdvices");
            InitializeComponent();
        }

        void InitializeComponent()
        {
            base.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            // Modified By :Lucky Rathore on 12 July 2016.
            Form.Action = Request.RawUrl;
            // Modified By :Ashish Kamble on 5 Feb 2016
            string originalUrl = Request.ServerVariables["HTTP_X_ORIGINAL_URL"];
            if (String.IsNullOrEmpty(originalUrl))
                originalUrl = Request.ServerVariables["URL"];

            DeviceDetection dd = new DeviceDetection(originalUrl);
            dd.DetectDevice();

            CommonOpn op = new CommonOpn();

            if (Request["pn"] != null && Request.QueryString["pn"] != "")
            {
                if (CommonOpn.CheckId(Request.QueryString["pn"]) == true)
                    pageNumber = Request.QueryString["pn"];
                Trace.Warn("pn: " + Request.QueryString["pn"]);
            }
            Trace.Warn("pageNumber: " + pageNumber);
            if (!IsPostBack)
            {
                FillComparisonTests();
            }
        }

        private void FillComparisonTests()
        {
            SelectClause = " CB.Id AS BasicId, CB.AuthorName, CB.Description, CB.DisplayDate, CB.Views, CB.Title, CB.Url, CEI.HostURL, CEI.ImagePathThumbnail, CEI.ImagePathLarge ";
            FromClause = " Con_EditCms_Basic AS CB With(NoLock) Left Join Con_EditCms_Images CEI With(NoLock) On CEI.BasicId = CB.Id And CEI.IsMainImage = 1 And CEI.IsActive = 1 ";
            WhereClause = " CB.CategoryId = @CategoryId AND CB.IsActive = 1 AND CB.IsPublished = 1";
            OrderByClause = " DisplayDate Desc ";
            RecordCntQry = " Select Count(CB.Id) From " + FromClause + " Where " + WhereClause;
            BaseUrl = "/tipsadvices/";

            SqlCommand cmd = new SqlCommand();
            cmd.Parameters.Add("@CategoryId", SqlDbType.BigInt).Value = "5";// Default value for Articles on Tips And Advices

            BindData(cmd);
        }

        void BindData(SqlCommand cmd)
        {
            try
            {
                if (pageNumber != string.Empty)
                    rpgTipsAdvices.CurrentPageIndex = Convert.ToInt32(pageNumber);

                //form the base url. 
                string qs = Request.ServerVariables["QUERY_STRING"];

                rpgTipsAdvices.BaseUrl = BaseUrl;

                rpgTipsAdvices.SelectClause = SelectClause;
                rpgTipsAdvices.FromClause = FromClause;
                rpgTipsAdvices.WhereClause = WhereClause;
                rpgTipsAdvices.OrderByClause = OrderByClause;
                rpgTipsAdvices.RecordCountQuery = RecordCntQry;
                rpgTipsAdvices.CmdParamQ = cmd;	//pass the parameter values for the query
                rpgTipsAdvices.CmdParamR = cmd.Clone();	//pass the parameter values for the record count                
                //initialize the grid, and this will also bind the repeater               
                rpgTipsAdvices.InitializeGrid();
                //recordCount = rpgNews.RecordCount;
                Trace.Warn("BaseURL" + BaseUrl);
            }
            catch (Exception err)
            {
                Trace.Warn(err.Message);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }
    }
}