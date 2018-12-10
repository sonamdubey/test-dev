using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Carwale.UI.Common;
using Carwale.UI.Controls;
using Carwale.Notifications;
using Carwale.DAL.CoreDAL;
using System.Data.Common;
using Carwale.DAL.CoreDAL.MySql;

namespace Carwale.UI.PressReleases
{
    public class Default : Page
    {
        protected RepeaterPagerAdvanced rpgDetails;
        protected Repeater rptDetails;
        protected Button btnDownload;
        private string pageSize = "", pageNumber = "", prId = "";

        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
            rptDetails = (Repeater)rpgDetails.FindControl("rptDetails");
        }

        void InitializeComponent()
        {
            base.Load += new EventHandler(Page_Load);
            this.rptDetails.ItemDataBound += new RepeaterItemEventHandler(this.rptDetails_ItemBound);
            this.rptDetails.ItemCommand += new RepeaterCommandEventHandler(this.rptDetails_ItemCommand);
        }


        void Page_Load(object Sender, EventArgs e)
        {
            CommonOpn op = new CommonOpn();

            //get the page size and the page number, sortorder and sort by
            if (Request["ps"] != null && Request.QueryString["ps"] != "")
            {
                if (CommonOpn.CheckId(Request.QueryString["ps"]) == true)
                    pageSize = Request.QueryString["ps"];
            }

            if (Request["pn"] != null && Request.QueryString["pn"] != "")
            {
                if (CommonOpn.CheckId(Request.QueryString["pn"]) == true)
                    pageNumber = Request.QueryString["pn"];
            }

            if (Request["prId"] != null && Request.QueryString["prId"] != "")
            {
                if (CommonOpn.CheckId(Request.QueryString["prId"]) == true)
                    prId = Request.QueryString["prId"];
            }

            if (!IsPostBack)
            {
                string qs = "";

                if (pageSize != "")
                    qs += "ps=" + pageSize;

                if (pageNumber != "")
                    qs += "&pn=1";

                if (prId != "")
                    qs += "&prId=" + prId;

                BindGrid();
            }
        }

        protected void rptDetails_ItemBound(object source, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                string fileName = ((HtmlInputHidden)e.Item.FindControl("hdnPdfFile")).Value;
                if (fileName == "")
                {
                    Button btnDownload = (Button)e.Item.FindControl("btnDownload");
                    btnDownload.Style.Add("display", "none");
                }
            }

        }

        protected void rptDetails_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (Convert.ToString(e.CommandName) == "Update")
            {
                string fileName = ((HtmlInputHidden)e.Item.FindControl("hdnPdfFile")).Value;
                string applicationPath = Server.MapPath("/").ToLower().Replace("\\carwale\\", "\\carwaleimg\\").ToString() + "images\\media\\pressreleases\\";
                Trace.Warn(applicationPath);
                string loadFile = applicationPath + fileName;
                this.Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName);
                this.Response.ClearContent();
                this.Response.ContentType = "application/pdf";
                this.Response.WriteFile(loadFile);
                this.Response.Flush();
                this.Response.Close();
            }
        }

        void BindGrid()
        {
            string recordCntQry = "";
            recordCntQry = "GetPressReleaseCount_v16_11_7";
            try
            {
                if (pageNumber != "")
                    rpgDetails.CurrentPageIndex = Convert.ToInt32(pageNumber);

                if (pageSize != "")
                    rpgDetails.PageSize = Convert.ToInt32(pageSize);
            
                string qs = Request.ServerVariables["QUERY_STRING"];
                rpgDetails.BaseUrl = "/pressreleases/default.aspx?" + qs;
                rpgDetails.RecordCountQuery = recordCntQry;
                using (DbCommand cmd = DbFactory.GetDBCommand("GetPressReleaseData_v16_11_7"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_PrId", DbType.Int32, 1));
                    rpgDetails.MySqlCmd = cmd;
                    rpgDetails.InitializeGrid();
                }                      
            }
            catch (Exception err)
            {
                Trace.Warn("-------in Catch-------------------");
                Trace.Warn(err.Message);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }         
        }
    } // class
} // namespace