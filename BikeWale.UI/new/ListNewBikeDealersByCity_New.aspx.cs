using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using Bikewale.Common;
using Bikewale.Memcache;

namespace Bikewale.New
{
    public class ListNewBikeDealersByCity_New : Page
    {
        protected Repeater rptState;
        protected DataList dlCity;

        protected DataSet dsStateCity = null;
        protected MakeModelVersion objMMV;

        public string makeId = "";
        public int StateCount = 0, DealerCount = 0;

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
            //code for device detection added by Ashwini Todkar
            // Modified By :Ashish Kamble on 5 Feb 2016
            string originalUrl = Request.ServerVariables["HTTP_X_ORIGINAL_URL"];
            if (String.IsNullOrEmpty(originalUrl))
                originalUrl = Request.ServerVariables["URL"];

            DeviceDetection dd = new DeviceDetection(originalUrl);
            dd.DetectDevice();

            if (ProcessQS())
            {
                if (!IsPostBack)
                {
                    objMMV = new MakeModelVersion();
                    objMMV.GetMakeDetails(makeId);

                    BindControl();
                    BindStates();
                }
            }
        }

        private void BindControl()
        {
            string sql = "";
            Database db = new Database();

            sql = " SELECT  C.Id AS CityId,c.MaskingName AS CityMaskingName, "
                + " C.Name AS City, COUNT(DNC.Id) AS TotalBranches, "
                + " S.Name AS [State], S.ID AS StateId, "
                + " ROW_NUMBER() Over(Partition By StateId Order by StateId) AS StateRank "
                + " FROM Dealer_NewBike AS DNC, BWCities AS C, States AS S With(NoLock) "
                + " WHERE DNC.CityId = C.Id AND C.StateId = S.ID AND DNC.IsActive = 1 "
                + " AND C.IsDeleted = 0 AND DNC.MakeId = @MakeId "
                + " GROUP By C.Id, C.Name, S.Name, S.ID, StateId,C.MaskingName "
                + " Order By [State], CityId  ";

            SqlParameter[] param = { 				
				new SqlParameter("@MakeId", makeId)
			};

            Trace.Warn(sql);

            try
            {
                dsStateCity = db.SelectAdaptQry(sql, param);
                StateCount = int.Parse(dsStateCity.Tables[0].Rows.Count.ToString());
                Trace.Warn("----------" + makeId);
            }
            catch (Exception err)
            {
                Trace.Warn(err.Message + err.Source);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }

        }//function

        private void BindStates()
        {
            DataSet dsState = new DataSet();

            DataTable dt = dsState.Tables.Add();
            dt.Columns.Add("StateId", typeof(int));
            dt.Columns.Add("State", typeof(string));

            DataRow dr;

            DataRow[] rows = dsStateCity.Tables[0].Select("StateRank = 1", "State ASC");

            foreach (DataRow row in rows)
            {
                dr = dt.NewRow();
                dr["StateId"] = row["StateId"].ToString();
                dr["State"] = row["State"].ToString();

                dt.Rows.Add(dr);
            }
            rptState.DataSource = dsState;
            rptState.DataBind();
        }

        public DataSet BindCities(int StateId)
        {
            DataSet dsCity = new DataSet();

            DataTable dt = dsCity.Tables.Add();
            dt.Columns.Add("CityId", typeof(int));
            dt.Columns.Add("City", typeof(string));
            dt.Columns.Add("StateId", typeof(int));
            dt.Columns.Add("State", typeof(string));
            dt.Columns.Add("TotalBranches", typeof(int));
            dt.Columns.Add("CityMaskingName", typeof(string));

            DataRow dr;

            DataRow[] rows = dsStateCity.Tables[0].Select("StateId = " + StateId, "City ASC");

            foreach (DataRow row in rows)
            {
                dr = dt.NewRow();
                dr["CityId"] = row["CityId"].ToString();
                dr["City"] = row["City"].ToString();
                dr["CityMaskingName"] = row["CityMaskingName"].ToString();
                dr["StateId"] = row["StateId"].ToString();
                dr["State"] = row["State"].ToString();
                dr["TotalBranches"] = row["TotalBranches"].ToString();
                DealerCount += int.Parse(row["TotalBranches"].ToString());
                dt.Rows.Add(dr);
            }
            Trace.Warn("City Dataset contains: " + dsCity.Tables[0].Rows.Count.ToString());
            return dsCity;

        }

        protected bool ProcessQS()
        {
            bool isSuccess = true;
            if (Request["make"] == null || Request.QueryString["make"] == "")
            {
                //invalid make id, hence redirect to he browsebikes.aspx page
                Trace.Warn("make id : ",Request.QueryString["make"]);
                Response.Redirect("/new/", false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
                this.Page.Visible = false;
                isSuccess = false;
            }
            else
            {
                makeId = MakeMapping.GetMakeId(Request.QueryString["make"].ToLower());

                //verify the id as passed in the url
                if (CommonOpn.CheckId(makeId) == false)
                {
                    Response.Redirect(CommonOpn.AppPath + "pageNotFound.aspx", false);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                    this.Page.Visible = false;
                    isSuccess = false;
                }
            }
            return isSuccess;
        }

    }   // End of class
}   // End of namespace