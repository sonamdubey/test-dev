using System;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;
using Carwale.UI.Common;
using Carwale.Notifications;
using System.Data.Common;
using Carwale.DAL.CoreDAL.MySql;
using System.Data;

namespace Carwale.UI.Community.Mods
{
    public class AbuseDetails : Page
    {

        protected Repeater rptAbuseDetails;
        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
        }

        void InitializeComponent()
        {
            base.Load += new EventHandler(Page_Load);
        }

        void Page_Load(object Sender, EventArgs e)
        {
            FetchAbuseDetails();
        }

        private void FetchAbuseDetails()
        {        
            CommonOpn objCom = new CommonOpn();
            DataSet ds = new DataSet();
            try
            {
                if (Request.QueryString["reviewId"] != null && Request.QueryString["reviewId"].ToString() != "")
                {
                    using (DbCommand cmd = DbFactory.GetDBCommand("GetAbusedReviewDetails_v16_11_7"))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DbFactory.GetDbParam("v_ReviewId", DbType.Int64, Request.QueryString["reviewId"].ToString()));
                        ds = MySqlDatabase.SelectAdapterQuery(cmd, DbConnections.CarDataMySqlReadConnection);
                    }
                    objCom.BindRepeaterReaderDataSet(ds, rptAbuseDetails);
                }
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }     
    }
}