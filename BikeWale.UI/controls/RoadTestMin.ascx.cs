using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Bikewale.Common;
using Bikewale.Notifications.CoreDAL;
using System.Data.Common;

namespace Bikewale.Controls
{
    public partial class RoadTestMin : System.Web.UI.UserControl
    {

        private bool _verticalDisplay = true;
        protected Repeater rptData;
        protected HtmlGenericControl divRoadTest;
        protected string detailsRTURL = String.Empty;

        public bool VerticalDisplay
        {
            get { return _verticalDisplay; }
            set { _verticalDisplay = value; }
        }

        protected override void OnInit(EventArgs e)
        {
            base.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (VerticalDisplay == false) divRoadTest.Attributes.Add("class", "rt-hor");

            if (!IsPostBack)
            {
                detailsRTURL = "http://" + Request.ServerVariables["HTTP_HOST"] + "/research/";

                FetchRoadTests();
            }
        }

        /// <summary>
        /// This function will store 'Road Tests' DataSet object in Cache for next 30 secs. It will save Database iteration. Hence improved page performance.
        /// This function will be called on page load event
        /// </summary>
        void FetchRoadTests()
        {
            //DataSet dsRTs = new DataSet();

            //ResearchContent objRC = new ResearchContent();

            //// Check if 'Road Tests' DataSet is available in cache or not. If not then fetch from Database and cache it for next 30 secs
            //if (Cache.Get("RoadTestsDS") == null)
            //{
            //    dsRTs = objRC.GetLatestRoadTests();

            //    // Cache the DataSet for next 30 minutes.
            //    Cache.Insert("RoadTestsDS", dsRTs, null, DateTime.Now.AddMinutes(30), TimeSpan.Zero);
            //}
            //else
            //{
            //    // Check if MakesDataSet available in cache
            //    dsRTs = (DataSet)Cache.Get("RoadTestsDS");
            //}

            // Bind DataSet to Repeater
            using (DataTable dtR = GetLatestRoadTests())
            {
                if (dtR.Rows.Count > 0)
                {
                    rptData.DataSource = dtR;
                    rptData.DataBind();
                }
            }
        }

        public DataTable GetLatestRoadTests()
        {
            DataTable dt = default(DataTable);
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("getroadtestdetails"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_top", DbParamTypeMapper.GetInstance[SqlDbType.SmallInt], "3"));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_category", DbParamTypeMapper.GetInstance[SqlDbType.SmallInt], "8")); // 8 category id for road test.

                    using (DataSet ds = MySqlDatabase.SelectAdapterQuery(cmd))
                    {
                        if (ds!=null && ds.Tables!=null && ds.Tables.Count > 0)
                        {
                            dt = ds.Tables[0];  
                        }
                    }

                }
            }
            catch (SqlException exSql)
            {
                ErrorClass objErr = new ErrorClass(exSql, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }

            return dt;
        }
    }//class
}//namespace