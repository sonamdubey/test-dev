using Bikewale.BAL.Location;
using Bikewale.Cache.BikeData;
using Bikewale.Cache.Core;
using Bikewale.Common;
using Bikewale.Controls;
using Bikewale.DAL.BikeData;
using Bikewale.DAL.Dealer;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.DealerLocator;
using Bikewale.Entities.Location;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.Dealer;
using Bikewale.Interfaces.Location;
using Bikewale.Utility;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace Bikewale.New
{
    /// <summary>
    /// Created by : Sangram Nandkhile on 27 Jun 2016
    /// Summary: To show dealers in State and list of cities
    /// </summary>
    public class DealersInCountry : Page
    {
        protected BikeMakeEntityBase objMMV;
        protected NewLaunchedBikes_new ctrlNewLaunchedBikes;
        protected UsedBikeModel ctrlusedBikeModel;
        protected UpcomingBikes_new ctrlUpcomingBikes;
        protected DealersByBrand ctrlDealerByBrand;
        public ushort makeId;
        public string cityArr = string.Empty, makeMaskingName = string.Empty, stateMaskingName = string.Empty, stateName = string.Empty, stateArray = string.Empty;
        public uint stateCount = 0, DealerCount = 0;
        protected uint countryCount = 0;
        public uint citiesCount = 0, stateCountDealers = 0;
        public uint cityId = 0, DealerCountCity, stateId = 0;
        public DealerLocatorList states = null;
        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
        }

        void InitializeComponent()
        {
            base.Load += new EventHandler(this.Page_Load);
        }
        /// Modified by  :   Subodh jain on 20 Dec 2016
        /// Description :   Get Dealer By BrandList widget
        /// <summary>
        /// Modified By :- Subodh Jain 15 March 2017
        /// Summary :- Added used bike widget
        /// </summary>
        protected void Page_Load(object sender, EventArgs e)
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

            if (ProcessQS())
            {
                checkDealersForMakeCity(makeId);

                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IBikeMakes<BikeMakeEntity, int>, BikeMakesRepository<BikeMakeEntity, int>>();
                    var makesRepository = container.Resolve<IBikeMakes<BikeMakeEntity, int>>();
                    objMMV = makesRepository.GetMakeDetails(makeId.ToString());

                }
                ctrlNewLaunchedBikes.pageSize = 6;
                ctrlNewLaunchedBikes.makeid = makeId;
                ctrlUpcomingBikes.pageSize = 6;
                ctrlUpcomingBikes.MakeId = makeId;
                ctrlDealerByBrand.WidgetTitle = "Find showroom for other brands";
                ctrlDealerByBrand.makeId = makeId;
                if (ctrlusedBikeModel != null)
                {
                    CityEntityBase _cityDetails = null;
                    if (cityId > 0)
                        _cityDetails = new CityHelper().GetCityById(cityId);
                    ctrlusedBikeModel.MakeId = makeId;

                    ctrlusedBikeModel.CityId = cityId;
                    ctrlusedBikeModel.WidgetTitle = string.Format("Second Hand Bikes in {0}", cityId > 0 ? _cityDetails.CityName : "India");
                    ctrlusedBikeModel.header = string.Format("Used {0} bikes in {1}", objMMV.MakeName, cityId > 0 ? _cityDetails.CityName : "India");
                    ctrlusedBikeModel.WidgetHref = string.Format("/used/{0}-bikes-in-{1}/", objMMV.MaskingName, cityId > 0 ? _cityDetails.CityMaskingName : "india");
                    ctrlusedBikeModel.TopCount = 9;
                }
                BindStatesCities();

            }
        }
        private void BindStatesCities()
        {
            IState objStatesCity = null;
            using (IUnityContainer container = new UnityContainer())
            {
                container.RegisterType<IState, States>();
                objStatesCity = container.Resolve<IState>();
                states = objStatesCity.GetDealerStatesCities(Convert.ToUInt32(makeId));
                DealerCount = states.totalDealers;
                citiesCount = states.totalCities;

            }
        }



        /// <summary>
        /// Modified by :   Sumit Kate on 03 Oct 2016
        /// Description :   Handle Make masking name rename 301 redirection
        /// </summary>
        /// <returns></returns>
        protected bool ProcessQS()
        {
            bool isSuccess = true;
            MakeMaskingResponse objResponse = null;

            if (!string.IsNullOrEmpty(Request["make"]))
            {
                makeMaskingName = Request["make"].ToString();
                try
                {
                    using (IUnityContainer container = new UnityContainer())
                    {
                        container.RegisterType<IBikeMakesCacheRepository<int>, BikeMakesCacheRepository<BikeMakeEntity, int>>()
                              .RegisterType<ICacheManager, MemcacheManager>()
                              .RegisterType<IBikeMakes<BikeMakeEntity, int>, BikeMakesRepository<BikeMakeEntity, int>>()
                             ;
                        var objCache = container.Resolve<IBikeMakesCacheRepository<int>>();

                        objResponse = objCache.GetMakeMaskingResponse(makeMaskingName);
                    }

                }
                catch (Exception ex)
                {
                    Trace.Warn("ProcessQueryString Ex: ", ex.Message);
                    ErrorClass objErr = new ErrorClass(ex, "");
                    objErr.SendMail();
                    isSuccess = false;
                }
                finally
                {
                    if (objResponse != null)
                    {
                        if (objResponse.StatusCode == 200)
                        {
                            makeId = Convert.ToUInt16(objResponse.MakeId);
                            isSuccess = true;
                        }
                        else if (objResponse.StatusCode == 301)
                        {
                            CommonOpn.RedirectPermanent(Request.RawUrl.Replace(makeMaskingName, objResponse.MaskingName));
                            isSuccess = false;
                        }
                        else
                        {
                            Response.Redirect(CommonOpn.AppPath + "pageNotFound.aspx", false);
                            HttpContext.Current.ApplicationInstance.CompleteRequest();
                            this.Page.Visible = false;
                            isSuccess = false;
                        }
                    }
                    else
                    {
                        Response.Redirect(CommonOpn.AppPath + "pageNotFound.aspx", false);
                        HttpContext.Current.ApplicationInstance.CompleteRequest();
                        this.Page.Visible = false;
                        isSuccess = false;
                    }
                }
            }
            else
            {
                Trace.Warn("make id : ", Request.QueryString["make"]);
                Response.Redirect("/new-bikes-in-india/", false);
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
                                string _redirectUrl = String.Format("/{0}-dealer-showrooms-in-{1}/", makeMaskingName, _city.CityMaskingName);
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