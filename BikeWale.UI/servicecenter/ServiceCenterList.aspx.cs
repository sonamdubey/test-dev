using Bikewale.BAL.ServiceCenter;
using Bikewale.Cache.BikeData;
using Bikewale.Cache.Core;
using Bikewale.Cache.Location;
using Bikewale.Cache.ServiceCenter;
using Bikewale.Common;
using Bikewale.Controls;
using Bikewale.DAL.BikeData;
using Bikewale.DAL.Location;
using Bikewale.DAL.ServiceCenter;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.Location;
using Bikewale.Entities.service;
using Bikewale.Entities.ServiceCenters;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.Location;
using Bikewale.Interfaces.ServiceCenter;
using Bikewale.Memcache;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;

namespace Bikewale.Service
{
    /// <summary>
    /// Created By : Sajal Gupta on 16-11-2016
    /// Class to show the bike service center listing for particular make and city.
    /// Modified By : Aditi Srivasatva on 30 Nov 2016
    /// Description : Added control to change brand and city for service centers
    /// Modified By : Sushil Kumar on 17th Jan 2016
    /// Description : Added chnage location prompt widget
    /// </summary>
    public class ServiceCenterList : Page
    {
        protected string makeName = string.Empty, cityName = string.Empty, makeMaskingName = string.Empty, urlCityMaskingName = string.Empty;
        protected uint cityId, makeId, totalServiceCenters;
        protected string clientIP = string.Empty, pageUrl = string.Empty;
        protected IEnumerable<Bikewale.Entities.ServiceCenters.ServiceCenterDetails> serviceCentersList = null;
        protected DealerCard ctrlDealerCard;
        protected BikeMakeEntityBase objBikeMakeEntityBase;
        protected CityEntityBase objCityEntityBase;
        protected UsedBikeWidget ctrlRecentUsedBikes;
        protected UsedPopularModelsInCity ctrlUsedModels;
        protected BrandCityPopUp ctrlBrandCity;
        protected ServiceCentersInNearbyCities ctrlNearbyServiceCenters;
        protected MostPopularBikes_new ctrlPopoularBikeMake;
        protected ChangeLocationPopup ctrlChangeLocation;
        protected usedBikeModel ctrlusedBikeModel;
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
            // Modified By :Lucky Rathore on 12 July 2016.
            Form.Action = Request.RawUrl;
            string originalUrl = Request.ServerVariables["HTTP_X_ORIGINAL_URL"];
            if (String.IsNullOrEmpty(originalUrl))
                originalUrl = Request.ServerVariables["URL"];

            Bikewale.Common.DeviceDetection dd = new Bikewale.Common.DeviceDetection(originalUrl);
            dd.DetectDevice();

            if (ProcessQueryString() && makeId > 0 && cityId > 0)
            {

                GetMakeNameByMakeId(makeId);
                BindServiceCentersList();
                GetCityNameByCityMaskingName(urlCityMaskingName);
                BindUserControls();
            }
            else
            {
                Response.Redirect(Bikewale.Common.CommonOpn.AppPath + "pageNotFound.aspx", false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
                this.Page.Visible = false;
            }
        }

        /// <summary>
        /// Created by : SAJAL GUPTA on 16-11-2016
        /// Description: Method to bind dealer car data.
        /// Modified By : Aditi Srivasatva on 30 Nov 2016
        /// Description : Set request type according to page for brand city pop up
        /// Modified By :-Subodh Jain on 1 Dec 2016
        /// Summary :- Added Used Bike and popular bike widget
        /// Modified By :-Subodh Jain on 16 Dec 2016
        /// Summary :- Added widget heading
        /// Modified By : Aditi Srivasatva on 19 dec 2016
        /// Description : Added widget for service centers in nearby cities
        /// Modified By : Sushil Kumar on 17th Jan 2016
        /// Description : Added chnage location prompt widget
        /// Modified By :-Subodh Jain on 15 March 2017
        /// Summary :-Added used Bike widget
        /// </summary>
        /// <returns></returns>
        private void BindUserControls()
        {
            try
            {
                ctrlDealerCard.MakeId = Convert.ToUInt32(makeId);
                ctrlDealerCard.makeName = makeName;
                ctrlDealerCard.makeMaskingName = makeMaskingName;
                ctrlDealerCard.CityId = cityId;
                ctrlDealerCard.LeadSourceId = 11;
                ctrlDealerCard.TopCount = 3;
                ctrlDealerCard.isHeading = false;
                ctrlDealerCard.widgetHeading = string.Format("{0} showrooms in {1}", makeName, cityName);
                ctrlBrandCity.requestType = EnumBikeType.ServiceCenter;
                ctrlBrandCity.makeId = makeId;
                ctrlBrandCity.cityId = cityId;

                ctrlNearbyServiceCenters.cityId = (int)cityId;
                ctrlNearbyServiceCenters.cityName = cityName;
                ctrlNearbyServiceCenters.makeId = (int)makeId;
                ctrlNearbyServiceCenters.makeName = makeName;
                ctrlNearbyServiceCenters.makeMaskingName = makeMaskingName;
                ctrlNearbyServiceCenters.topCount = 8;

                ctrlPopoularBikeMake.makeId = (int)makeId;
                ctrlPopoularBikeMake.cityId = (int)cityId;
                ctrlPopoularBikeMake.totalCount = 9;
                ctrlPopoularBikeMake.cityname = cityName;
                ctrlPopoularBikeMake.cityMaskingName = urlCityMaskingName;
                ctrlPopoularBikeMake.makeName = makeName;

                if (ctrlChangeLocation != null)
                {
                    ctrlChangeLocation.UrlCityId = cityId;
                    ctrlChangeLocation.UrlCityName = cityName;
                }
                if (ctrlusedBikeModel != null)
                {

                    ctrlusedBikeModel.MakeId = makeId;
                    if (cityId > 0)
                        ctrlusedBikeModel.CityId = cityId;
                    ctrlusedBikeModel.widgetTitle = string.Format("Second-hand Honda Bikes in {0}", cityId > 0 ? cityName : "India");
                    ctrlusedBikeModel.header = string.Format("Used {0} bikes in {1}", makeName, cityId > 0 ? cityName : "India");
                    ctrlusedBikeModel.widgetHref = string.Format("/used/{0}-bikes-in-{1}/", makeName, cityId > 0 ? urlCityMaskingName : "india");
                    ctrlusedBikeModel.TopCount = 9;
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "ServiceCenterList.GetCityNameByCityMaskingName");
            }
        }

        /// <summary>
        /// Created by : SAJAL GUPTA on 08-11-2016
        /// Description: Method to get city name by citymasking name.
        /// </summary>
        /// <param name="cityMaskingName"></param>
        private void GetCityNameByCityMaskingName(string cityMaskingName)
        {
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<ICityCacheRepository, CityCacheRepository>()
                        .RegisterType<ICity, CityRepository>()
                        .RegisterType<ICacheManager, MemcacheManager>();
                    var cityRepository = container.Resolve<ICityCacheRepository>();
                    objCityEntityBase = cityRepository.GetCityDetails(cityMaskingName);
                }

                if (objCityEntityBase != null)
                {
                    cityName = objCityEntityBase.CityName;
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "GetCityNameByCityMaskingName");
            }
        }

        /// <summary>
        /// Created by : SAJAL GUPTA on 08-11-2016
        /// Description: Method to get make name by makeId.
        /// </summary>
        /// <param name="cityMaskingName"></param>
        private void GetMakeNameByMakeId(uint makeId)
        {
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IBikeMakes<BikeMakeEntity, int>, BikeMakesRepository<BikeMakeEntity, int>>();
                    var makesRepository = container.Resolve<IBikeMakes<BikeMakeEntity, int>>();
                    objBikeMakeEntityBase = makesRepository.GetMakeDetails(makeId.ToString());
                }

                if (objBikeMakeEntityBase != null)
                {
                    makeName = objBikeMakeEntityBase.MakeName;
                    makeMaskingName = objBikeMakeEntityBase.MaskingName;
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "GetMakeNameByMakeId");
            }
        }

        /// <summary>
        /// Created by : Sajal Gupta on 16-11-2016.
        /// description : To bind ServiceCenters list.
        /// </summary>
        private void BindServiceCentersList()
        {
            ServiceCenterData objServiceCenterData = null;

            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IServiceCenter, ServiceCenter<ServiceCenterLocatorList, int>>()
                    .RegisterType<IServiceCenterCacheRepository, ServiceCenterCacheRepository>()
                    .RegisterType<IServiceCenterRepository<ServiceCenterLocatorList, int>, ServiceCenterRepository<ServiceCenterLocatorList, int>>()
                    .RegisterType<ICacheManager, MemcacheManager>();
                    var objSC = container.Resolve<IServiceCenter>();

                    objServiceCenterData = objSC.GetServiceCentersByCity(cityId, (int)makeId);

                    if (objServiceCenterData != null && objServiceCenterData.Count > 0)
                    {
                        serviceCentersList = objServiceCenterData.ServiceCenters;
                        totalServiceCenters = objServiceCenterData.Count;
                    }
                }

            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "BindServiceCentersList : ");
            }
        }

        /// <summary>
        /// Created By  : Sushil Kumar
        /// Created On  : 20th March 2016
        /// Description : To get makeId from make masking name
        /// Modified by :   Sumit Kate on 03 Oct 2016
        /// Description :   Handle Make masking name rename 301 redirection
        /// </summary>
        private bool GetMakeIdByMakeMaskingName()
        {
            bool isValidMake = true;
            if (!string.IsNullOrEmpty(makeMaskingName))
            {
                MakeMaskingResponse objMakeResponse = null;

                try
                {
                    using (IUnityContainer containerInner = new UnityContainer())
                    {
                        containerInner.RegisterType<IBikeMakesCacheRepository<int>, BikeMakesCacheRepository<BikeMakeEntity, int>>()
                              .RegisterType<ICacheManager, MemcacheManager>()
                              .RegisterType<IBikeMakes<BikeMakeEntity, int>, BikeMakesRepository<BikeMakeEntity, int>>()
                             ;
                        var objCache = containerInner.Resolve<IBikeMakesCacheRepository<int>>();

                        objMakeResponse = objCache.GetMakeMaskingResponse(makeMaskingName);
                    }
                }
                catch (Exception ex)
                {
                    ErrorClass objErr = new ErrorClass(ex, "GetMakeIdByMakeMaskingName");
                    isValidMake = false;
                }
                finally
                {
                    if (objMakeResponse != null)
                    {
                        if (objMakeResponse.StatusCode == 200)
                        {
                            makeId = objMakeResponse.MakeId;
                        }
                        else if (objMakeResponse.StatusCode == 301)
                        {
                            CommonOpn.RedirectPermanent(Request.RawUrl.Replace(makeMaskingName, objMakeResponse.MaskingName));
                            isValidMake = false;
                        }
                        else
                        {
                            isValidMake = false;
                        }
                    }
                    else
                    {
                        isValidMake = false;
                    }
                }
            }
            else
            {
                isValidMake = false;
            }

            return isValidMake;
        }

        #region Private Method to process querystring
        /// <summary>
        /// Created By : Sushil Kumar
        /// Created On : 16th March 2016 
        /// Description : Private Method to query string fro make masking name and cityId
        /// </summary>
        private bool ProcessQueryString()
        {
            var currentReq = HttpContext.Current.Request;
            bool isValidQueryString = false;
            try
            {
                if (currentReq.QueryString != null && currentReq.QueryString.HasKeys())
                {
                    makeMaskingName = currentReq.QueryString["make"].ToLower();
                    urlCityMaskingName = currentReq.QueryString["city"].ToLower();
                    clientIP = Bikewale.Common.CommonOpn.GetClientIP();
                    pageUrl = currentReq.ServerVariables["URL"];

                    if (!String.IsNullOrEmpty(urlCityMaskingName) && !String.IsNullOrEmpty(makeMaskingName))
                    {
                        cityId = CitiMapping.GetCityId(urlCityMaskingName);
                        isValidQueryString = GetMakeIdByMakeMaskingName() && (cityId > 0);

                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, string.Format("ProcessQueryString for {0} makeMaskingName", makeMaskingName));
            }
            return isValidQueryString;
        }
        #endregion
    }   // End of class
}   // End of namespace