using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Carwale.UI.Common;
using Carwale.Notifications;
using Carwale.DAL.CoreDAL;
using System.Data.Common;
using Carwale.DAL.CoreDAL.MySql;

namespace Carwale.UI.PressReleases
{
    public class PressReleaseDetails : Page
    {
        protected Button btnDownload;
        protected string title, detailSumm, attachFile = "";
        protected DateTime releaseDate;
        int prId = 0;

        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
        }

        void InitializeComponent()
        {
            base.Load += new EventHandler(Page_Load);
            btnDownload.Click += new EventHandler(btnDownload_Click);
        }

        void Page_Load(object Sender, EventArgs e)
        {
            if (Request["prid"] != null && Request.QueryString["prid"] != "")
            {
                if (CommonOpn.CheckId(Request.QueryString["prid"]) == true)
                    prId = int.Parse(Request.QueryString["prid"].ToString());
                else
                    UrlRewrite.Return404();
            }
            FetchDetails();
        }

        void FetchDetails()
        {        
            try
            {            
                using (DbCommand cmd = DbFactory.GetDBCommand("GetPressReleaseDetails_v16_11_7"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_PrId", DbType.Int64, prId));
                    using(IDataReader dr = MySqlDatabase.SelectQuery(cmd,DbConnections.CarDataMySqlReadConnection))
                    {
                        while (dr.Read())
                        {                      
                            title = (string)dr["Title"];
                            detailSumm = (string)dr["Detailed"];
                            releaseDate = (DateTime)dr["ReleaseDate"];
                            attachFile = dr["AttachedFile"].ToString();
                        }
                    }
                }
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }

        void btnDownload_Click(object Sender, EventArgs e)
        {
            string fileName = attachFile;
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
    } // class
} // namespace