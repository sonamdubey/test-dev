using Bikewale.Common;
using Bikewale.Entities.BikeData;
using MySql.CoreDAL;
using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI.WebControls;
using Bikewale.Utility;

namespace Bikewale.Content
{
    public class DefaultUR : System.Web.UI.Page
    {
        protected DropDownList drpMake, drpModel;
        protected Button btnWrite;
        protected Repeater rptMakes, rptMostReviewed;

        private DataSet dsMain;

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
            base.Load += new EventHandler(Page_Load);
            btnWrite.Click += new EventHandler(btnWrite_Click);
        }
        private void Page_Load(object sender, EventArgs e)
        {
            // Modified By :Lucky Rathore on 12 July 2016.
            Form.Action = Request.RawUrl;
            //code for device detection added by Ashwini Todkar
            // Modified By :Ashish Kamble on 5 Feb 2016
            string originalUrl = Request.ServerVariables["HTTP_X_ORIGINAL_URL"];
            if (String.IsNullOrEmpty(originalUrl))
                originalUrl = Request.ServerVariables["URL"];

            DeviceDetection dd = new DeviceDetection(originalUrl);
            dd.DetectDevice();


            if (!IsPostBack)
            {
                BindControl();
                FillMake();
            }
        }

        private void btnWrite_Click(object Sender, EventArgs e)
        {
            string _returnUrl = Bikewale.Utility.TripleDES.EncryptTripleDES(string.Format("returnUrl=/user-reviews/&sourceid={0}",(int)Bikewale.Entities.UserReviews.UserReviewPageSourceEnum.Desktop_UserReview_Landing));
            Response.Redirect(string.Format("/rate-your-bike/{0}/?q={1}", Request.Form["drpModel"], _returnUrl, false));
            HttpContext.Current.ApplicationInstance.CompleteRequest();
            this.Page.Visible = false;
        }

        //Modified By : Ashwini Todkar on 12nd Feb 2014
        //Description : Replaced inline query by method
        private void FillMake()
        {
            try
            {
                MakeModelVersion mmv = new MakeModelVersion();
                //dt = mmv.GetMakes("NEW");

                //if(dt.Rows.Count > 0 )
                //{
                //    drpMake.DataSource = dt;
                //    drpMake.DataTextField = "Text";
                //    drpMake.DataValueField = "Value";
                //    drpMake.DataBind();

                //    ListItem item = new ListItem("--Select--", "0");
                //    drpMake.Items.Insert(0, item);
                //}

                mmv.GetMakes(EnumBikeType.New, ref drpMake);
            }
            catch (Exception ex)
            {
                Trace.Warn(ex.Message);
                ErrorClass.LogError(ex, Request.ServerVariables["URL"]);
                
            }
        }

        //Modified By : Ashwini Todkar on 12nd feb 2014
        //Description : replaced inline query by stored procedure 
        private void BindControl()
        {
            try
            {

                using (DbCommand cmd = DbFactory.GetDBCommand("getuserreviews"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (DataSet ds = MySqlDatabase.SelectAdapterQuery(cmd, ConnectionType.ReadOnly))
                    {

                        dsMain = new DataSet();
                        dsMain = ds;

                        rptMakes.DataSource = ds.Tables[0];
                        rptMakes.DataBind();

                        rptMostReviewed.DataSource = ds.Tables[2];
                        rptMostReviewed.DataBind();
                        Trace.Warn("++dsmain rows count ", dsMain.Tables.Count.ToString());
                    }
                }

            }
            catch (SqlException sqlEx)
            {
                Trace.Warn(sqlEx.Message + sqlEx.Source);
                ErrorClass.LogError(sqlEx, Request.ServerVariables["URL"]);
                
            }
            catch (Exception err)
            {
                Trace.Warn(err.Message + err.Source);
                ErrorClass.LogError(err, Request.ServerVariables["URL"]);
                
            }
        }

        public DataSet GetDataSource(string makeId)
        {
            Trace.Warn("Binding List...");

            DataSet ds = new DataSet();
            DataTable dt = new DataTable("Wallpapers");

            dt.Columns.Add("MakeId", typeof(string));
            dt.Columns.Add("ModelId", typeof(string));
            dt.Columns.Add("ModelName", typeof(string));
            dt.Columns.Add("TotalReviews", typeof(string));
            dt.Columns.Add("BikeMake", typeof(string));
            dt.Columns.Add("BikeModel", typeof(string));
            dt.Columns.Add("ModelMaskingName", typeof(string));
            dt.Columns.Add("MakeMaskingName", typeof(string));

            try
            {
                Trace.Warn("Total Models..." + dsMain.Tables[1].Rows.Count);

                DataRow[] rows = dsMain.Tables[1].Select("MakeId=" + makeId);

                DataRow dr;
                Trace.Warn("Current Make contains..." + rows.Length);

                foreach (DataRow row in rows)
                {
                    dr = dt.NewRow();

                    dr[0] = row["MakeId"];
                    dr[1] = row["ModelId"];
                    dr[2] = row["ModelName"];
                    dr[3] = row["TotalReviews"];
                    dr[4] = row["BikeMake"];
                    dr[5] = row["BikeModel"];
                    dr[6] = row["ModelMaskingName"];
                    dr[7] = row["MakeMaskingName"];

                    dt.Rows.Add(dr);
                }

                Trace.Warn("Current Table contains..." + dt.Rows[0]["ModelMaskingName"].ToString());
                Trace.Warn("current table contain..." + dt.Rows[0]["MakeMaskingName"].ToString());
                ds.Tables.Add(dt);
            }
            catch (Exception err)
            {
                Trace.Warn(err.Message + err.Source);
                ErrorClass.LogError(err, Request.ServerVariables["URL"]);
                
            }

            return ds;
        }
    }
}