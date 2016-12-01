﻿using Bikewale.BAL.ServiceCenter;
using Bikewale.Cache.BikeData;
using Bikewale.Cache.Core;
using Bikewale.Cache.Location;
using Bikewale.Cache.ServiceCenter;
using Bikewale.Common;
using Bikewale.DAL.BikeData;
using Bikewale.DAL.Location;
using Bikewale.DAL.ServiceCenter;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.Location;
using Bikewale.Entities.PriceQuote;
using Bikewale.Entities.service;
using Bikewale.Entities.ServiceCenters;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.Location;
using Bikewale.Interfaces.ServiceCenter;
using Bikewale.Memcache;
using Bikewale.Mobile.Controls;
using Bikewale.Utility;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Web;

namespace Bikewale.Mobile.Service
{
    /// <summary>
    /// Created By : Sushil Kumar on 19th March 2016
    /// Class to show the bike dealers details
    /// </summary>
    public class ServiceCenterList : PageBase
    {
        protected string makeName = string.Empty, modelName = string.Empty, cityName = string.Empty, areaName = string.Empty, makeMaskingName = string.Empty, cityMaskingName = string.Empty, urlCityMaskingName = string.Empty;
        protected uint cityId, makeId, totalServiceCenters;
        protected string clientIP = string.Empty, pageUrl = string.Empty;
        protected LeadCaptureControl ctrlLeadCapture;
        protected BikeMakeEntityBase objBikeMakeEntityBase;
        protected CityEntityBase objCityEntityBase;
        protected IEnumerable<Bikewale.Entities.ServiceCenters.ServiceCenterDetails> serviceCentersList = null;
        protected DealersCard ctrlDealerCard;
        protected UsedBikes ctrlRecentUsedBikes;
        protected MMostPopularBikes ctrlPopoularBikeMake;

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

            if (ProcessQueryString())
            {
                GetMakeIdByMakeMaskingName(makeMaskingName);

                GetMakeNameByMakeId(makeId);

                if (makeId > 0 && cityId > 0)
                {
                    BindServiceCentersList();
                    GetCityNameByCityMaskingName(urlCityMaskingName);

                    BindWidgets();
                }
                else
                {
                    Response.Redirect(Bikewale.Common.CommonOpn.AppPath + "pageNotFound.aspx", false);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                    this.Page.Visible = false;
                }
            }
        }

        /// <summary>
        /// Created by : SAJAL GUPTA on 08-11-2016
        /// Description: Method to bind dealer card widget data.
        /// Modified By :-Subodh Jain on 1 Dec 2016
        /// Summary :- Added Used Bike and popular bike widget
        /// </summary>
        private void BindWidgets()
        {
            try
            {
                ctrlDealerCard.MakeId = makeId;
                ctrlDealerCard.makeMaskingName = makeMaskingName;
                ctrlDealerCard.CityId = cityId;
                ctrlDealerCard.cityName = cityName;
                ctrlDealerCard.PageName = "Service_Center_Listing_City";
                ctrlDealerCard.TopCount = 9;
                ctrlDealerCard.PQSourceId = (int)PQSourceEnum.Mobile_ServiceCenter_Listing_CityPage;
                ctrlDealerCard.LeadSourceId = 16;
                ctrlDealerCard.DealerId = 0;
                ctrlDealerCard.isHeadingNeeded = false;

                ctrlRecentUsedBikes.MakeId = makeId;
                ctrlRecentUsedBikes.CityId = (int?)cityId;
                ctrlRecentUsedBikes.header = string.Format("Looking to buy used {0} bikes in {1}? Explore", makeName, cityName);
                ctrlRecentUsedBikes.TopCount = 4;
                ctrlRecentUsedBikes.cityMaskingName = urlCityMaskingName;
                ctrlPopoularBikeMake.makeId = (int)makeId;
                ctrlPopoularBikeMake.cityId = (int)cityId;
                ctrlPopoularBikeMake.totalCount = 9;
                ctrlPopoularBikeMake.cityname = cityName;
                ctrlPopoularBikeMake.cityMaskingName = urlCityMaskingName;
                ctrlPopoularBikeMake.makeName = makeName;
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, "ServiceCenterList.BindWidgets");
                objErr.SendMail();

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
                    cityName = objCityEntityBase.CityName;
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, "GetCityNameByCityMaskingName");
                objErr.SendMail();
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
                    makeName = objBikeMakeEntityBase.MakeName;
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, "GetMakeNameByMakeId");
                objErr.SendMail();
            }
        }

        /// <summary>
        /// Created by : Sajal Gupta on 08-11-2016.
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
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, "BindServiceCentersList : ");
                objErr.SendMail();
            }
        }

        /// <summary>
        /// Created By  : Sushil Kumar
        /// Created On  : 20th March 2016
        /// Description : To get makeId from make masking name
        /// Modified by :   Sumit Kate on 03 Oct 2016
        /// Description :   Handle Make masking name rename 301 redirection
        /// </summary>
        private void GetMakeIdByMakeMaskingName(string maskingName)
        {

            if (!string.IsNullOrEmpty(maskingName))
            {
                string _makeId = string.Empty;

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
                    Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, "GetMakeIdByMakeMaskingName");
                    objErr.SendMail();
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

                if (string.IsNullOrEmpty(_makeId) || !uint.TryParse(_makeId, out makeId))
                {
                    Response.Redirect(Bikewale.Common.CommonOpn.AppPath + "pageNotFound.aspx", false);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                    this.Page.Visible = false;
                }
            }
            else
            {
                Response.Redirect(Bikewale.Common.CommonOpn.AppPath + "pageNotFound.aspx", false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
                this.Page.Visible = false;
            }
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
                    if (!String.IsNullOrEmpty(urlCityMaskingName) && !String.IsNullOrEmpty(makeMaskingName))
                    {
                        cityId = CitiMapping.GetCityId(urlCityMaskingName);
                        isValidQueryString = true;
                    }
                    else
                    {
                        Response.Redirect(Bikewale.Common.CommonOpn.AppPath + "pageNotFound.aspx", false);
                        HttpContext.Current.ApplicationInstance.CompleteRequest();
                        this.Page.Visible = false;
                    }
                    clientIP = Bikewale.Common.CommonOpn.GetClientIP();
                    pageUrl = currentReq.ServerVariables["URL"];
                }
                else
                {
                    Response.Redirect(Bikewale.Common.CommonOpn.AppPath + "pageNotFound.aspx", false);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                    this.Page.Visible = false;
                }
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, " : ProcessQueryString ");
                objErr.SendMail();
            }
            return isValidQueryString;
        }

        #endregion
    }   // End of class
}   // End of namespace