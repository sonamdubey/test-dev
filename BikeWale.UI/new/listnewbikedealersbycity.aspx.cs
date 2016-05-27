using Bikewale.Common;
using Bikewale.DAL.Dealer;
using Bikewale.Entities.Location;
using Bikewale.Entities.PriceQuote;
using Bikewale.Interfaces.Dealer;
using Bikewale.Memcache;
using Bikewale.Notifications.CoreDAL;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Bikewale.New
{
    public class ListNewBikeDealersByCity : Page
    {
        protected Repeater rptState;
        protected DataList dlCity;

        protected DataSet dsStateCity = null;
        protected MakeModelVersion objMMV;

        public string makeId = "";
        public int StateCount = 0, DealerCount = 0;
        private uint cityId;
        private string makeMaskingName = string.Empty;
        protected bool cityDetected = false;
        protected Bikewale.Controls.NewBikeLaunches ctrl_NewBikeLaunches;
        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
        }

        void InitializeComponent()
        {
            base.Load += new EventHandler(this.Page_Load);
        }

        /// <summary>
        /// Modified By : Sushil Kumar on 31st March 2016
        /// Description : Added method to redirect user to dealer listing page if make and city is available
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //code for device detection added by Ashwini Todkar
            // Modified By :Ashish Kamble on 5 Feb 2016
            string originalUrl = Request.ServerVariables["HTTP_X_ORIGINAL_URL"];
            if (String.IsNullOrEmpty(originalUrl))
                originalUrl = Request.ServerVariables["URL"];

            DeviceDetection dd = new DeviceDetection(originalUrl);
            dd.DetectDevice();
            ctrl_NewBikeLaunches.PQSourceId = (int)PQSourceEnum.Desktop_LocateDealer_NewLaunches;
            ushort _makeId = 0;

            if (ProcessQS() && ushort.TryParse(makeId, out _makeId))
            {
                GetLocationCookie();

                if (cityId > 0)
                {
                    cityDetected = checkDealersForMakeCity(_makeId);
                }
                if (_makeId > 0 && (!cityDetected))
                {
                    objMMV = new MakeModelVersion();
                    objMMV.GetMakeDetails(makeId);

                    BindControl();
                    BindStates();
                }
            }
        }

        /// <summary>
        /// Created By : Sushil Kumar bon 31st March 2016
        /// Description : To redirect user to dealer listing page if make and city already provided by user
        /// </summary>
        /// <param name="_makeId"></param>
        private bool checkDealersForMakeCity(ushort _makeId)
        {
            IEnumerable<CityEntityBase> _cities = null;
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IDealer, DealersRepository>();

                    var objCities = container.Resolve<IDealer>();
                    _cities = objCities.FetchDealerCitiesByMake(_makeId);
                    if (_cities != null && _cities.Count() > 0)
                    {
                        var _city = _cities.FirstOrDefault(x => x.CityId == cityId);
                        if (_city != null)
                        {
                            string _redirectUrl = String.Format("/new/{0}-dealers/{1}-{2}.html", makeMaskingName, cityId, _city.CityMaskingName);
                            Response.Redirect(_redirectUrl, false);
                            HttpContext.Current.ApplicationInstance.CompleteRequest();
                            this.Page.Visible = false;
                            return true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Trace.Warn(ex.Message);
                ErrorClass objErr = new ErrorClass(ex, "checkDealersForMakeCity");
                objErr.SendMail();
            }
            return false;
        }

        private void BindControl()
        {
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("getstatewisecitydealers"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_makeid", DbParamTypeMapper.GetInstance[SqlDbType.Int], makeId)); 

                    using (DataSet ds = MySqlDatabase.SelectAdapterQuery(cmd))
                    {
                        if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
                            dsStateCity = ds;
                    }
                }

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
            var _make = Request.QueryString["make"];
            bool isSuccess = true;
            if (Request["make"] == null || _make == "")
            {
                //invalid make id, hence redirect to he browsebikes.aspx page
                Trace.Warn("make id : ", _make);
                Response.Redirect("/new/", false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
                this.Page.Visible = false;
                isSuccess = false;
            }
            else
            {
                makeId = MakeMapping.GetMakeId(_make.ToLower());

                //verify the id as passed in the url
                if (CommonOpn.CheckId(makeId) == false)
                {
                    Response.Redirect(CommonOpn.AppPath + "pageNotFound.aspx", false);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                    this.Page.Visible = false;
                    isSuccess = false;
                }
                else
                {
                    makeMaskingName = _make;
                }
            }
            return isSuccess;
        }

        #region Set user location from location cookie
        /// <summary>
        /// Created By : Sushil Kumar on 31th March 2016
        /// Description : To set user location
        /// </summary>
        /// <returns></returns>
        private void GetLocationCookie()
        {
            string location = String.Empty;
            try
            {
                if (this.Context.Request.Cookies.AllKeys.Contains("location") && !string.IsNullOrEmpty(this.Context.Request.Cookies["location"].Value) && this.Context.Request.Cookies["location"].Value != "0")
                {
                    location = this.Context.Request.Cookies["location"].Value;
                    string[] arr = System.Text.RegularExpressions.Regex.Split(location, "_");

                    if (arr.Length > 0)
                    {
                        uint.TryParse(arr[0], out cityId);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "GetLocationCookie");
                objErr.SendMail();
            }
        }
        #endregion

    }   // End of class
}   // End of namespace