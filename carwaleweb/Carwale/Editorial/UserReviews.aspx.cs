using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Carwale.UI.Common;
using Carwale.Notifications;
using CarwaleAjax;
using System.Data.Common;
using Carwale.DAL.CoreDAL.MySql;

namespace Carwale.UI.Editorial
{
    public class UserWrittenReviews : Page
    {
        protected DropDownList drpMake, drpModel;
        protected Button btnWrite;
        protected Repeater rptMakes, rptMostReviewed;

        private DataSet dsMain = new DataSet();
        string customerId = "";

        CommonOpn op = new CommonOpn();

        public string SelectedModel
        {
            get
            {
                if (Request.Form["drpModel"] != null && Request.Form["drpModel"].ToString() != "")
                    return Request.Form["drpModel"].ToString();
                else
                    return "-1";
            }
        }

        public string ModelContents
        {
            get
            {
                if (Request.Form["hdn_drpModel"] != null && Request.Form["hdn_drpModel"].ToString() != "")
                    return Request.Form["hdn_drpModel"].ToString();
                else
                    return "";
            }
        }

        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
        }

        void InitializeComponent()
        {
            base.Load += new EventHandler(Page_Load);
            btnWrite.Click += new EventHandler(btnWrite_Click);
        }

        void Page_Load(object Sender, EventArgs e)
        {
            DeviceDetection dd = new DeviceDetection(Request.ServerVariables["HTTP_X_REWRITE_URL"].ToString());
            dd.DetectDevice();  
            if (!IsPostBack)
            {
                BindControl();
                FillMake();
            }
            else
            {             
                AjaxFunctions aj = new AjaxFunctions();          
                aj.UpdateContents(drpModel, ModelContents, SelectedModel);
            }
        } // Page_Load

        private void btnWrite_Click(object Sender, EventArgs e)
        {
            customerId = CurrentUser.Id;
            Response.Redirect(Carwale.Utility.ManageCarUrl.CreateRatingPageUrl(Convert.ToInt32(Request.Form["drpModel"])));
        }

        private void FillMake()
        {       
            DataSet ds = new DataSet();
            using (DbCommand cmd = DbFactory.GetDBCommand("GetCarMakes_v16_11_7"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(DbFactory.GetDbParam("v_MakeCond", DbType.String, 10, "All"));
                ds = MySqlDatabase.SelectAdapterQuery(cmd, DbConnections.CarDataMySqlReadConnection);
            }
            if (ds != null && ds.Tables.Count > 0)
                op.FillDropDownMySql(ds.Tables[0], drpMake, "MakeName", "MakeId","Make"); 
        }

        private void BindControl()
        {
            DataSet ds = new DataSet();
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("GetAllMakeWithReview_v16_11_7"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    ds = MySqlDatabase.SelectAdapterQuery(cmd, DbConnections.CarDataMySqlReadConnection);
                }               
                using (DbCommand cmd = DbFactory.GetDBCommand("GetMainRepeaterData_v16_11_7"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    dsMain = MySqlDatabase.SelectAdapterQuery(cmd, DbConnections.CarDataMySqlReadConnection);
                }
                rptMakes.DataSource = ds;
                rptMakes.DataBind();

                ds = new DataSet();
                using (DbCommand cmd = DbFactory.GetDBCommand("GetReviewRepeaterData_v16_11_7"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    ds = MySqlDatabase.SelectAdapterQuery(cmd, DbConnections.CarDataMySqlReadConnection);
                }
                rptMostReviewed.DataSource = ds;
                rptMostReviewed.DataBind();     
            }
            catch (Exception err)
            {
                Trace.Warn(err.Message + err.Source);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }

        public DataSet GetDataSource(string makeId)
        {      
            DataSet ds = new DataSet();
            DataTable dt = new DataTable("Wallpapers");

            dt.Columns.Add("MakeId", typeof(string));
            dt.Columns.Add("ModelId", typeof(string));
            dt.Columns.Add("ModelName", typeof(string));
            dt.Columns.Add("TotalReviews", typeof(string));
            dt.Columns.Add("CarMake", typeof(string));
            dt.Columns.Add("CarModel", typeof(string));
            dt.Columns.Add("MaskingName", typeof(string));

            try
            {
                DataRow[] rows = dsMain.Tables[0].Select("MakeId=" + makeId);
                DataRow dr;             
                foreach (DataRow row in rows)
                {
                    dr = dt.NewRow();

                    dr[0] = row["MakeId"];
                    dr[1] = row["ModelId"];
                    dr[2] = row["ModelName"];
                    dr[3] = row["TotalReviews"];
                    dr[4] = row["CarMake"];
                    dr[5] = row["CarModel"];
                    dr[6] = row["MaskingName"];

                    dt.Rows.Add(dr);
                }
                ds.Tables.Add(dt);
            }
            catch (Exception err)
            {
                Trace.Warn(err.Message + err.Source);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            return ds;
        }
    } // class
} // namespace