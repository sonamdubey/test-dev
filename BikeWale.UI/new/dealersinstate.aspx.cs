using Bikewale.Cache.Core;
using Bikewale.Cache.Location;
using Bikewale.Common;
using Bikewale.DAL.BikeData;
using Bikewale.DAL.Dealer;
using Bikewale.DAL.Location;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.Location;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.Cache.Core;
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
    /// Summary: To show dealers in State and list of cities
    /// </summary>
    public class DealersInState : Page
    {
        protected Repeater rptCity;
        protected DataList dlCity;

        protected DataSet dsStateCity = null;
        protected BikeMakeEntityBase objMMV;

        public ushort makeId;
        public string cityArr = string.Empty, makeMaskingName = string.Empty, stateMaskingName = string.Empty, stateName = string.Empty;
        public int citiesCount = 0;
        public uint cityId = 0, DealerCount = 0, stateId = 0;
        public DealerStateCities dealerCity;

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
                //objMMV = new MakeModelVersion();
                //objMMV.GetMakeDetails(strMakeId);
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IBikeMakes<BikeMakeEntity, int>, BikeMakesRepository<BikeMakeEntity, int>>();
                    var makesRepository = container.Resolve<IBikeMakes<BikeMakeEntity, int>>();
                    objMMV = makesRepository.GetMakeDetails(makeId.ToString());

                }
                BindCities();
            }
        }


        private void BindCities()
        {
            ICity objCities = null;
            using (IUnityContainer container = new UnityContainer())
            {
                container.RegisterType<ICity, CityRepository>();
                objCities = container.Resolve<ICity>();
                dealerCity = objCities.GetDealerStateCities(makeId, stateId);
                if (objMMV!=null && dealerCity != null && dealerCity.dealerCities != null && dealerCity.dealerCities.Count() > 0)
                {
                    foreach (var dCity in dealerCity.dealerCities)
                    {
                        dCity.Link = string.Format("/{0}-bikes/dealers-in-{1}/", objMMV.MaskingName, dCity.CityMaskingName);
                    }
                    rptCity.DataSource = dealerCity.dealerCities;
                    rptCity.DataBind();
                    cityArr = Newtonsoft.Json.JsonConvert.SerializeObject(dealerCity.dealerCities);
                    cityArr = cityArr.Replace("cityId", "id").Replace("cityName", "name");
                    DealerCount = dealerCity.dealerCities.Select(o => o.DealersCount).Aggregate((x, y) => x + y);
                    citiesCount = dealerCity.dealerCities.Count();
                    stateName = dealerCity.dealerStates.StateName;
                }
            }
        }

        protected bool ProcessQS()
        {
            bool isSuccess = true;
            if (!string.IsNullOrEmpty(Request["make"]) && !string.IsNullOrEmpty(Request["state"]))
            {
                makeMaskingName = Request["make"].ToString();
                string _makeId = MakeMapping.GetMakeId(makeMaskingName);
                makeId = Convert.ToUInt16(_makeId);
                //verify the id as passed in the url
                if (CommonOpn.CheckId(_makeId) == false)
                {
                    Response.Redirect(CommonOpn.AppPath + "pageNotFound.aspx", false);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                    this.Page.Visible = false;
                    isSuccess = false;
                }
                stateMaskingName = Request.QueryString["state"];
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<ICacheManager, MemcacheManager>()
                             .RegisterType<IState, StateRepository>()
                             .RegisterType<IStateCacheRepository, StateCacheRepository>()
                            ;
                    var objCache = container.Resolve<IStateCacheRepository>();
                    var objResponse = objCache.GetStateMaskingResponse(stateMaskingName);
                    if (objResponse != null)
                    {
                        stateId = objResponse.StateId;
                    }
                }
            }
            else
            {
                Trace.Warn("make id : ", Request.QueryString["make"]);
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