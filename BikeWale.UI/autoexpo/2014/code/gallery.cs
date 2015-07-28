using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Bikewale.Common;
using System.Configuration;
using System.Text.RegularExpressions;

namespace AutoExpo
{
    public class gallery : Page
    {
        protected RepeaterPagerGallery rpgNews;
        protected Repeater rptNews;

        protected string BasicId = string.Empty, NewsTitle = string.Empty, Url = string.Empty;

        protected string SelectClause = string.Empty, FromClause = string.Empty, WhereClause = string.Empty,
                             OrderByClause = string.Empty, BaseUrl = string.Empty, RecordCntQry = string.Empty;

        private string pageNumber = string.Empty;


        protected override void OnInit(EventArgs e)
        {
            rptNews = (Repeater)rpgNews.FindControl("rptNews");
            InitializeComponent();
        }

        void InitializeComponent()
        {
            base.Load += new EventHandler(this.Page_Load);
        }

        void Page_Load(object Sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CommonOpn op = new CommonOpn();

                if (Request["pn"] != null && Request.QueryString["pn"] != "")
                {
                    if (CommonOpn.CheckId(Request.QueryString["pn"]) == true)
                        pageNumber = Request.QueryString["pn"];
                }
                FillGallery();
            }

        } // Page_Load


        //getting details from news id 		
        private void FillGallery()
        {
            Database db = new Database();
            NewsTitle = "Auto Expo Updates";

            SelectClause = " C.Id AS BasicId,C.Title,C.Url,C.IsPublished ";
            FromClause = " Con_EditCms_Basic C INNER JOIN Con_EditCms_Images I on C.Id=I.BasicId AND I.IsActive = 1 AND IsMainImage = 1 ";
            WhereClause = " C.CategoryId = @CategoryId AND C.ShowGallery=1 AND C.IsPublished = 1 AND YEAR(C.PublishedDate) >= 2013 ";
            OrderByClause = " C.Id desc ";
            RecordCntQry = " Select Count(C.Id) From " + FromClause + " Where " + WhereClause;
            BaseUrl = "/autoexpo/2014/gallery/";

            
            SqlCommand cmd = new SqlCommand();

            cmd.Parameters.Add("@CategoryId", SqlDbType.BigInt).Value = "13"; // 13 is category id for auto expo media

            BindData(cmd);
        }

        void BindData(SqlCommand cmd)
        {
            try
            {
                if (pageNumber != string.Empty)
                    rpgNews.CurrentPageIndex = Convert.ToInt32(pageNumber);

                //form the base url. 
                string qs = Request.ServerVariables["QUERY_STRING"];

                rpgNews.BaseUrl = BaseUrl;

                rpgNews.SelectClause = SelectClause;
                rpgNews.FromClause = FromClause;
                rpgNews.WhereClause = WhereClause;
                rpgNews.OrderByClause = OrderByClause;
                rpgNews.RecordCountQuery = RecordCntQry;
                rpgNews.CmdParamQ = cmd;	//pass the parameter values for the query
                rpgNews.CmdParamR = cmd.Clone();	//pass the parameter values for the record count	
                rpgNews.PageSize = 5;
                //initialize the grid, and this will also bind the repeater
                Trace.Warn("cmd : ",cmd.CommandText);
                rpgNews.InitializeGrid();
                //recordCount = rpgNews.RecordCount;
            }
            catch (Exception err)
            {
                Trace.Warn(err.Message);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }

        protected DataSet GetImages(string bid)
        {
            DataSet ds = new DataSet();
            Database db = new Database();

            string sql = "select Id, BasicId, HostUrl,ImagePathThumbnail,ImagePathLarge from Con_EditCms_Images where BasicId=@BasicId and IsActive=1 AND YEAR(LastUpdatedTime) >= 2013";

            SqlCommand cmd = new SqlCommand(sql);
            cmd.Parameters.Add("@BasicId", SqlDbType.BigInt).Value = bid;

            try
            {
                ds = db.SelectAdaptQry(cmd);
            }
            catch (Exception ex)
            {
                Trace.Warn("error : " + ex.Message);
                ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            finally
            {
                db.CloseConnection();
            }

            return ds;
        }

    } // class
} // namespace