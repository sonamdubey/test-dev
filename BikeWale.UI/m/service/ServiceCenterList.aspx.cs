using Bikewale.BAL.ServiceCenters;
using Bikewale.Cache.BikeData;
using Bikewale.Cache.Core;
using Bikewale.Cache.Location;
using Bikewale.Cache.ServiceCenters;
using Bikewale.Common;
using Bikewale.DAL.BikeData;
using Bikewale.DAL.Location;
using Bikewale.DAL.ServiceCenters;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.Location;
using Bikewale.Entities.PriceQuote;
using Bikewale.Entities.ServiceCenters;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.Location;
using Bikewale.Interfaces.ServiceCenters;
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
        protected BikeMakeEntityBase objBMEB;
        protected CityEntityBase objCEB;
        public IEnumerable<ServiceCenterDetails> serviceCentersList = null;
        protected DealersCard ctrlDealerCard;

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

                    BindDealerCard();
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
        /// </summary>
        private void BindDealerCard()
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
                    objCEB = cityRepository.GetCityDetails(cityMaskingName);
                }

                if (objCEB != null)
                    cityName = objCEB.CityName;
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
                    objBMEB = makesRepository.GetMakeDetails(makeId.ToString());
                }

                if (objBMEB != null)
                    makeName = objBMEB.MakeName;
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
            ServiceCenterData objSCD = null;

            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<ICacheManager, MemcacheManager>();
                    container.RegisterType<IServiceCentersRepository, ServiceCentersRepository>();
                    container.RegisterType<IServiceCentersCacheRepository, ServiceCentersCacheRepository>();
                    container.RegisterType<IServiceCenters, ServiceCenters>();
                    var objSC = container.Resolve<IServiceCenters>();
                    objSCD = objSC.GetServiceCentersByCity(cityId, (int)makeId);

                    if (objSCD != null && objSCD.Count > 0)
                    {
                        serviceCentersList = objSCD.ServiceCenters;
                        totalServiceCenters = objSCD.Count;
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
                    Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, Request.ServerVariables["URL"] + "ParseQueryString");
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