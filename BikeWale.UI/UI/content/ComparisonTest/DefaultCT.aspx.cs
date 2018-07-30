using Bikewale.Common;
using Bikewale.Controls;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Bikewale.Content
{
    public class DefaultCT : System.Web.UI.Page
    {
        protected RepeaterPager rpgCarCompare;
        protected Repeater rptCarCompare;
        protected MakeModelSearch MakeModelSearch;
        protected HtmlGenericControl alertObj;

        protected string SelectClause = string.Empty, FromClause = string.Empty, WhereClause = string.Empty,
                             OrderByClause = string.Empty, RecordCntQry = string.Empty, BaseUrl = string.Empty;

        private string pageNumber = string.Empty;
        protected override void OnInit(EventArgs e)
        {
            base.Load += new EventHandler(Page_Load);
        }
        private void Page_Load(object sender, EventArgs e)
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
            string makeId = string.Empty, makeName = string.Empty, modelId = string.Empty, modelName = string.Empty;
            alertObj.Visible = false;

            if (Request["pn"] != null && Request.QueryString["pn"] != "")
            {
                if (CommonOpn.CheckId(Request.QueryString["pn"]) == true)
                    pageNumber = Request.QueryString["pn"];
                Trace.Warn("pn: " + Request.QueryString["pn"]);
            }
            Trace.Warn("pageNumber: " + pageNumber);
            if (!IsPostBack)
            {
                if (!String.IsNullOrEmpty(Request.QueryString["makeId"]))
                {
                    Trace.Warn("MakeId: " + Request.QueryString["makeId"]);
                    makeId = Request.QueryString["makeId"].ToString();
                    makeName = Request.QueryString["make"].ToString();
                    if (!String.IsNullOrEmpty(Request.QueryString["modelId"]))
                    {
                        Trace.Warn("ModelId: " + Request.QueryString["modelId"]);
                        modelId = Request.QueryString["modelId"].ToString();
                        modelName = Request.QueryString["model"].ToString();
                    }

                    FetchComparisonTests(makeId, makeName, modelId, modelName);
                }
                else
                {
                    FillComparisonTests();
                }
            }
        }

        private void FetchComparisonTests(string makeId, string makeName, string modelId, string modelName)
        {
            SelectClause = " CB.Id AS BasicId, CB.AuthorName, CB.Description, CB.DisplayDate, CB.Views, CB.Title, CB.Url," +
                            "CEI.IsMainImage, CEI.HostURL, CEI.ImagePathThumbnail, CEI.ImagePathLarge, Cmo.Name As ModelName," +
                            "Cma.Name As MakeName, SC.Name As SubCategory , CEI.ImagePathOriginal";
            FromClause = " Con_EditCms_Basic AS CB With(NoLock) Join Con_EditCms_Bikes CC With(NoLock) On CC.BasicId = CB.Id And CC.IsActive = 1 Join " +
                           " BikeModels Cmo With(NoLock) On Cmo.ID = CC.ModelId Join BikeMakes Cma On Cma.ID = CC.MakeId Left Join " +
                           " Con_EditCms_Images CEI With(NoLock) On CEI.BasicId = CB.Id And CEI.IsMainImage = 1 And CEI.IsActive = 1 " +
                           " Left Join Con_EditCms_BasicSubCategories BSC With(NoLock) On BSC.BasicId = CB.Id Left Join Con_EditCms_SubCategories SC With(NoLock) " +
                           " On SC.Id = BSC.SubCategoryId And SC.IsActive = 1";
            WhereClause = " CB.CategoryId = @CategoryId AND CB.IsActive = 1 AND CB.IsPublished = 1 AND CC.MakeId = @MakeId";
            if (modelId != string.Empty)
            {
                WhereClause += " AND CC.ModelId = @ModelId";
            }
            OrderByClause = " DisplayDate Desc ";
            RecordCntQry = " Select Count(CB.Id) From " + FromClause + " Where " + WhereClause;
            BaseUrl = "/research/" + makeName + "-cars/";
            if (modelId != "")
            {
                BaseUrl += modelName + "/";
            }
            BaseUrl += "comparos/";

            SqlCommand cmd = new SqlCommand();
            cmd.Parameters.Add("@CategoryId", SqlDbType.BigInt).Value = "2";// Default value for Articles on Comparison Tests
            cmd.Parameters.Add("@MakeId", SqlDbType.BigInt).Value = makeId;
            MakeModelSearch.MakeId = makeId;
            if (modelId != string.Empty)
            {
                cmd.Parameters.Add("@ModelId", SqlDbType.BigInt).Value = modelId;
                MakeModelSearch.ModelId = modelId;
            }
            BindData(cmd);
        }

        private void FillComparisonTests()
        {
            SelectClause = " CB.Id AS BasicId, CB.AuthorName, CB.Description, CB.DisplayDate, CB.Views, CB.Title, " +
                           " CB.Url, CEI.IsMainImage, CEI.HostURL, CEI.ImagePathThumbnail, CEI.ImagePathLarge, CEI.ImagePathOriginal ";
            FromClause = " Con_EditCms_Basic AS CB With(NoLock) Left Join Con_EditCms_Images CEI With(NoLock) On CEI.BasicId = CB.Id And CEI.IsMainImage = 1 And CEI.IsActive = 1 ";
            WhereClause = " CB.CategoryId = @CategoryId AND CB.IsActive = 1 AND CB.IsPublished = 1";
            OrderByClause = " DisplayDate Desc ";
            RecordCntQry = " Select Count(CB.Id) From " + FromClause + " Where " + WhereClause;
            BaseUrl = "/research/comparos/";

            SqlCommand cmd = new SqlCommand();
            cmd.Parameters.Add("@CategoryId", SqlDbType.BigInt).Value = "2";// Default value for Articles on Comparison Tests

            BindData(cmd);
        }

        void BindData(SqlCommand cmd)
        {
            try
            {
                if (pageNumber != string.Empty)
                    rpgCarCompare.CurrentPageIndex = Convert.ToInt32(pageNumber);

                //form the base url. 
                string qs = Request.ServerVariables["QUERY_STRING"];

                rpgCarCompare.BaseUrl = BaseUrl;

                rpgCarCompare.SelectClause = SelectClause;
                rpgCarCompare.FromClause = FromClause;
                rpgCarCompare.WhereClause = WhereClause;
                rpgCarCompare.OrderByClause = OrderByClause;
                rpgCarCompare.RecordCountQuery = RecordCntQry;
                rpgCarCompare.CmdParamQ = cmd;	//pass the parameter values for the query
                rpgCarCompare.CmdParamR = cmd.Clone();	//pass the parameter values for the record count                
                //initialize the grid, and this will also bind the repeater
                rpgCarCompare.InitializeGrid();
                //recordCount = rpgNews.RecordCount;
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