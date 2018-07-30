using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Bikewale.Common;
using System.Data;
using System.Data.SqlClient;

namespace Bikewale.Controls
{
    public class NewBikeLaunches : UserControl
    {
        private int _topCount = 5;
        private bool _verticalDisplay = true;
        protected Repeater rptData;
        protected HtmlGenericControl divNewLaunch;

        public int TopCount
        {
            get { return _topCount; }
            set { _topCount = value; }
        }

        public bool VerticalDisplay
        {
            get { return _verticalDisplay; }
            set { _verticalDisplay = value; }
        }
        public int PQSourceId { get; set; }

        protected override void OnInit(EventArgs e)
        {
            InitializeComponents();
        }

        void InitializeComponents()
        {
            this.Load += new EventHandler(this.Page_Load);
        }

        void Page_Load(object sender, EventArgs e)
        {
            if (VerticalDisplay == false) divNewLaunch.Attributes.Add("class", "nl-hor");

            if (!IsPostBack)
            {
                ShowNewLaunches();
            }
        } // Page_Load

        /// <summary>
        /// This function will store 'Road Tests' DataSet object in Cache for next 30 secs. It will save Database iteration. Hence improved page performance.
        /// This function will be called on page load event
        /// </summary>
        //void ShowNewLaunches()
        //{
        //    DataSet dsRTs = null;

        //    // Check if 'Road Tests' DataSet is available in cache or not. If not then fetch from Database and cache it for next 30 min
        //    if (Cache.Get("NewLaunchesDS") == null)
        //    {
        //        dsRTs = FetchNewLaunches();

        //        if (dsRTs != null)
        //        {
        //            // Cache the DataSet for next 30 minutes. 
        //            Cache.Insert("NewLaunchesDS", dsRTs, null, DateTime.Now.AddMinutes(30), TimeSpan.Zero);
        //            Trace.Warn("Reading from database");
        //        }
        //    }
        //    else
        //    {
        //        // Check if MakesDataSet available in cache
        //        dsRTs = (DataSet)Cache.Get("NewLaunchesDS");
        //        Trace.Warn("Reading from cache");
        //    }

        //    // Bind DataSet to Repeater        
        //    rptData.DataSource = dsRTs;
        //    rptData.DataBind();
        //}

        ///// <summary>
        /////     Function to get the top 3 recently launched bikes
        /////     Modified By : Ashish G. Kamble on 2 Aug 2013.
        /////     Desc : Old query is commented. Checked flag isdeleted from models and makes table. New sp added 'GetNewBikeLaunches'.
        ///// </summary>
        ///// <returns></returns>
        //public DataSet FetchNewLaunches()
        //{
        //    DataSet ds = null;

        //    //string sql = " SELECT TOP 3 BL.Id, BL.BikeMakeId, M.Name AS Make, BL.BikeModelId AS ModelId, MO.Name As Model, MO.HostURL, MO.SmallPic "
        //    //            + " FROM ExpectedBikeLaunches AS BL, BikeMakes AS M, BikeModels AS MO "
        //    //            + " WHERE BL.IsLaunched = 1 AND BL.BikeMakeId = M.ID AND BL.BikeModelId = MO.ID "
        //    //            + " ORDER BY BL.LaunchDate DESC ";

        //    try
        //    {
        //        Database db = new Database();

        //        using (SqlCommand cmd = new SqlCommand())
        //        {
        //            cmd.CommandType = CommandType.StoredProcedure;
        //            cmd.CommandText = "GetNewBikeLaunches";

        //            cmd.Parameters.Add("@TopCount", SqlDbType.TinyInt).Value = 3;

        //            ds = db.SelectAdaptQry(cmd);

        //            HttpContext.Current.Trace.Warn("ds count : ", ds.Tables[0].Rows.Count.ToString());
        //            Trace.Warn("New bike launch : ", ds.Tables[0].Rows[1]["ModelMaskingName"].ToString());
        //        }
        //    }
        //    catch (SqlException exSql)
        //    {
        //        ErrorClass.LogError(exSql, HttpContext.Current.Request.ServerVariables["URL"]);
        //        
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);
        //        
        //    }

        //    return ds;
        //}

        #region ShowNewLaunches 
        void ShowNewLaunches()
        {
            DataSet ds = null;
            
            try
            {                
                //Memcache implemented by Ashish G. Kamble on 31 Oct 2013
                Memcache.NewBikeLaunches objNBL = new Memcache.NewBikeLaunches();
                ds = objNBL.GetNewBikeLaunches(TopCount.ToString());
                
                rptData.DataSource = ds;
                rptData.DataBind();
            }
            catch (SqlException err)
            {
                ErrorClass.LogError(err, "Controls.NewBikeLaunches");
                
            }
            catch (Exception err)
            {
                ErrorClass.LogError(err, "Controls.NewBikeLaunches");
                
            }

        }
        #endregion


    }
}