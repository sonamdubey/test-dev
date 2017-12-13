using Bikewale.BAL.Location;
using Bikewale.Cache.BikeData;
using Bikewale.Cache.Core;
using Bikewale.Common;
using Bikewale.DAL.BikeData;
using Bikewale.DAL.Dealer;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.DealerLocator;
using Bikewale.Entities.Location;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.Dealer;
using Bikewale.Interfaces.Location;
using Bikewale.Mobile.Controls;
using Bikewale.Utility;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace Bikewale.Mobile.New
{
    /// <summary>
    /// Created by : Vivek Gupta on 28th jun 2016
    /// Summary: To show dealers in State and list of cities
    /// </summary>
    public class DealerInCountry : Page
    {
        protected BikeMakeEntityBase objMMV;
        protected MNewLaunchedBikes ctrlNewLaunchedBikes;
        protected UsedBikeModel ctrlusedBikeModel;
        protected MUpcomingBikes ctrlUpcomingBikes;
        protected DealersByBrand ctrlDealersByBrand;
        public ushort makeId;
        public string cityArr = string.Empty, makeMaskingName = string.Empty, stateMaskingName = string.Empty, stateName = string.Empty, stateArray = string.Empty;
        public uint stateCount = 0, DealerCount = 0;
        protected uint countryCount = 0;
        public uint citiesCount = 0, stateCountDealers = 0;
        public uint cityId = 0, DealerCountCity, stateId = 0;
        public DealerStateCities dealerCity;
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

        protected void Page_Load(object sender, EventArgs e)
        {
            if (ProcessQS())
            {
                checkDealersForMakeCity(makeId);

                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IBikeMakes<BikeMakeEntity, int>, BikeMakesRepository<BikeMakeEntity, int>>();
                    var makesRepository = container.Resolve<IBikeMakes<BikeMakeEntity, int>>();
                    objMMV = makesRepository.GetMakeDetails(makeId);

                }

                BindStatesCities();
                Bindwidget();
            }
        }
        /// <summary>
        /// Created By :- Subodh Jain 15 March 2017
        /// Summary :- bind wigdet function
        /// </summary>
        private void Bindwidget()
        {
            try
            {
                ctrlNewLaunchedBikes.pageSize = 6;
                ctrlNewLaunchedBikes.makeid = makeId;
                ctrlUpcomingBikes.pageSize = 6;
                ctrlUpcomingBikes.MakeId = makeId;
                ctrlDealersByBrand.WidgetTitle = "Find showroom for other brands";
                ctrlDealersByBrand.makeId = makeId;
                if (ctrlusedBikeModel != null)
                {
                    CityEntityBase _cityDetails = null;
                    if (cityId > 0)
                        _cityDetails = new CityHelper().GetCityById(cityId);
                    ctrlusedBikeModel.MakeId = makeId;

                    ctrlusedBikeModel.CityId = cityId;
                    ctrlusedBikeModel.WidgetTitle = string.Format("Second Hand Bikes in {0}", cityId > 0 ? _cityDetails.CityName : "India");
                    ctrlusedBikeModel.header = string.Format("Used {0} bikes in {1}", objMMV.MakeName, cityId > 0 ? _cityDetails.CityName : "India");
                    ctrlusedBikeModel.WidgetHref = string.Format("/m/used/{0}-bikes-in-{1}/", objMMV.MaskingName, cityId > 0 ? _cityDetails.CityMaskingName : "india");
                    ctrlusedBikeModel.TopCount = 9;
                }
            }
            catch (Exception ex)
            {

                ErrorClass.LogError(ex, "DealerInCountry.Bindwidget");
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
                DealerCount = states.TotalDealers;
                citiesCount = states.TotalCities;

            }
        }

        protected bool ProcessQS()
        {
            bool isSuccess = true;
            if (!string.IsNullOrEmpty(Request["make"]))
            {
                makeMaskingName = Request["make"].ToString();
                string _makeId = string.Empty;

                MakeMaskingResponse objMakeResponse = null;

                try
                {
                    using (IUnityContainer containerInner = new UnityContainer())
                    {
                        containerInner.RegisterType<IBikeMakesCacheRepository, BikeMakesCacheRepository>()
                              .RegisterType<ICacheManager, MemcacheManager>()
                              .RegisterType<IBikeMakes<BikeMakeEntity, int>, BikeMakesRepository<BikeMakeEntity, int>>()
                             ;
                        var objCache = containerInner.Resolve<IBikeMakesCacheRepository>();

                        objMakeResponse = objCache.GetMakeMaskingResponse(makeMaskingName);
                    }
                }
                catch (Exception ex)
                {
                    ErrorClass.LogError(ex, Request.ServerVariables["URL"] + "ParseQueryString");

                    Response.Redirect("pageNotFound.aspx", false);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                    this.Page.Visible = false;
                }
                finally
                {
                    if (objMakeResponse != null)
                    {
                        if (objMakeResponse.StatusCode == 200)
                        {
                            _makeId = Convert.ToString(objMakeResponse.MakeId);
                        }
                        else if (objMakeResponse.StatusCode == 301)
                        {
                            CommonOpn.RedirectPermanent(Request.RawUrl.Replace(makeMaskingName, objMakeResponse.MaskingName));
                        }
                        else
                        {
                            Response.Redirect(CommonOpn.AppPath + "pageNotFound.aspx", false);
                            HttpContext.Current.ApplicationInstance.CompleteRequest();
                            this.Page.Visible = false;
                        }
                    }
                    else
                    {
                        Response.Redirect(CommonOpn.AppPath + "pageNotFound.aspx", false);
                        HttpContext.Current.ApplicationInstance.CompleteRequest();
                        this.Page.Visible = false;
                    }
                }

                makeId = Convert.ToUInt16(_makeId);
                //verify the id as passed in the url
                if (CommonOpn.CheckId(_makeId) == false)
                {
                    Response.Redirect(CommonOpn.AppPath + "pageNotFound.aspx", false);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                    this.Page.Visible = false;
                    isSuccess = false;
                }


            }
            else
            {
                Response.Redirect("/m/new-bikes-in-india/", false);
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
                        container.RegisterType<IDealerRepository, DealersRepository>();
                        var objCities = container.Resolve<IDealerRepository>();
                        _cities = objCities.FetchDealerCitiesByMake(_makeId);
                        if (_cities != null && _cities.Any())
                        {
                            var _city = _cities.FirstOrDefault(x => x.CityId == cityId);
                            if (_city != null)
                            {
                                string _redirectUrl = String.Format("/m/dealer-showrooms/{0}/{1}/", makeMaskingName, _city.CityMaskingName);
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
                    ErrorClass.LogError(ex, "checkDealersForMakeCity");

                }
            }
            return false;
        }

    }   // End of class
}   // End of namespace