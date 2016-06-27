using Bikewale.Common;
using Bikewale.DAL.Dealer;
using Bikewale.DAL.Location;
using Bikewale.Entities.Location;
using Bikewale.Interfaces.Dealer;
using Bikewale.Interfaces.Location;
using Bikewale.Memcache;
using Bikewale.Utility;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Bikewale.New
{
    /// <summary>
    /// Created by : Sangram Nandkhile on 27 Jun 2016
    /// Summary: To show dealers in India and list of states
    /// </summary>
    public class DealersInIndia : Page
    {
        protected Repeater rptState;
        protected DataList dlCity;

        protected DataSet dsStateCity = null;
        protected MakeModelVersion objMMV;

        public string strMakeId = string.Empty, stateArray = string.Empty, makeMaskingName = string.Empty;
        public int stateCount = 0, DealerCount = 0; protected uint countryCount = 0;
        protected uint cityId = 0;
        protected ushort makeId = 0;
        protected bool cityDetected;

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
                checkDealersForMakeCity(makeId);
                objMMV = new MakeModelVersion();
                objMMV.GetMakeDetails(strMakeId);
                BindStates();
            }
        }


        /// <summary>
        /// Modified by: Sangram Nandkhile on 24th Jun 2016
        /// </summary>
        private void BindStates()
        {
            IEnumerable<DealerStateEntity> states = null;
            IState objStates = null;
            using (IUnityContainer container = new UnityContainer())
            {
                container.RegisterType<IState, StateRepository>();
                objStates = container.Resolve<IState>();
                states = objStates.GetDealerStates(Convert.ToUInt32(strMakeId));
                if (states != null && states.Count() > 0)
                {
                    foreach (var state in states)
                    {
                        state.Link = string.Format("/{0}-bikes/dealers-in-{1}-state/", objMMV.MakeMappingName, state.StateMaskingName);
                    }
                    rptState.DataSource = states;
                    rptState.DataBind();
                    stateArray = Newtonsoft.Json.JsonConvert.SerializeObject(states);
                    // To set correct properties in json array
                    stateArray = stateArray.Replace("stateId", "id").Replace("stateName", "name");
                    countryCount = states.Select(o => o.StateCount).Aggregate((x, y) => x + y);
                    stateCount = states.Count();
                }
            }
        }


        /// <summary>
        /// Process query string and fetch make id
        /// </summary>
        /// <returns></returns>
        protected bool ProcessQS()
        {
            bool isSuccess = true;
            if (!string.IsNullOrEmpty(Request["make"]))
            {
                makeMaskingName = Request["make"].ToString();
                strMakeId = MakeMapping.GetMakeId(Request.QueryString["make"].ToLower());
                makeId = Convert.ToUInt16(strMakeId);
                if (CommonOpn.CheckId(strMakeId) == false)
                {
                    Response.Redirect(CommonOpn.AppPath + "pageNotFound.aspx", false);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                    this.Page.Visible = false;
                    isSuccess = false;
                }
            }
            else
            {
                Response.Redirect("/new/", false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
                this.Page.Visible = false;
                isSuccess = false;
            }
            return isSuccess;
        }


        /// <summary>
        /// Created By : Sushil Kumar bon 31st March 2016
        /// Description : To redirect user to dealer listing page if make and city already provided by user
        /// </summary>
        /// <param name="_makeId"></param>
        private bool checkDealersForMakeCity(ushort _makeId)
        {
            GlobalCityAreaEntity currentCityArea = GlobalCityArea.GetGlobalCityArea();
            cityId = currentCityArea.CityId;
            if (cityId > 0)
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
                                string _redirectUrl = String.Format("/{0}-bikes/dealers-in-{1}/", makeMaskingName, _city.CityMaskingName);
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
            }
            return false;
        }
    }   // End of class
}   // End of namespace