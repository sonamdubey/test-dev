﻿using Bikewale.Cache.BikeData;
using Bikewale.Cache.Core;
using Bikewale.Cache.DealersLocator;
using Bikewale.DAL.BikeData;
using Bikewale.DAL.Dealer;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.DealerLocator;
using Bikewale.Entities.Location;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.Dealer;
using Bikewale.Memcache;
using Bikewale.Notifications;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Bikewale.Utility;
using Bikewale.Controls;
using Bikewale.Entities.PriceQuote;

namespace Bikewale.New
{
    /// <summary>
    /// Created By : Sushil Kumar on 19th March 2016
    /// Class to show the bike dealers details
    /// </summary>
    public class BikeDealerDetails : Page
    {
        protected string makeName = string.Empty, modelName = string.Empty, cityName = string.Empty, areaName = string.Empty, makeMaskingName = string.Empty, cityMaskingName = string.Empty, urlCityMaskingName = string.Empty;
        protected string address = string.Empty, maskingNumber = string.Empty, eMail = string.Empty, workingHours = string.Empty, modelImage = string.Empty, dealerName = string.Empty, dealerMaskingName = string.Empty;
        protected uint cityId, makeId,cost;
        protected ushort totalDealers;
        protected Repeater rptMakes, rptCities, rptDealers;
        protected string clientIP = string.Empty, pageUrl = string.Empty;
        protected bool areDealersPremium = false;
        protected uint dealerId;
        protected DealerBikesEntity dealerDetails=null;
        protected DealerCard ctrlDealerCard;
        protected LeadCaptureControl ctrlLeadCapture;

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
          
            Form.Action = Request.RawUrl;
            string originalUrl = Request.ServerVariables["HTTP_X_ORIGINAL_URL"];
            if (String.IsNullOrEmpty(originalUrl))
                originalUrl = Request.ServerVariables["URL"];

            Bikewale.Common.DeviceDetection dd = new Bikewale.Common.DeviceDetection(originalUrl);
            dd.DetectDevice();

            if (ProcessQueryString())
            {
                GetMakeIdByMakeMaskingName(makeMaskingName);

                if (dealerId> 0)
                {
                
                    GetDealerDetails(dealerId);
                    BindMakesDropdown();
                    ctrlDealerCard.MakeId = Convert.ToUInt32(makeId);
                    ctrlDealerCard.makeName = makeName;
                    ctrlDealerCard.makeMaskingName = makeMaskingName;
                    ctrlDealerCard.CityId = cityId;
                    ctrlDealerCard.PQSourceId = (int)PQSourceEnum.Desktop_DealerLocator_Detail_GetOfferButton;
                    ctrlDealerCard.LeadSourceId = 38;
                    ctrlDealerCard.TopCount = Convert.ToUInt16(cityId > 0 ? 3 : 6);
                    ctrlDealerCard.pageName = "DealerDetail_Page_Desktop";
                    ctrlLeadCapture.CityId = cityId;
                    ctrlLeadCapture.AreaId = 0;
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
        /// Created By  : Sushil Kumar
        /// Created On  : 20th March 2016
        /// Description : To bind dealerlist
        /// </summary>
        private void BindDealerList()
        {
            DealersEntity _dealers = null;
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IDealerCacheRepository, DealerCacheRepository>()
                             .RegisterType<ICacheManager, MemcacheManager>()
                             .RegisterType<IDealer, DealersRepository>()
                            ;
                    var objCache = container.Resolve<IDealerCacheRepository>();
                    _dealers = objCache.GetDealerByMakeCity(cityId, makeId);

                    if (_dealers != null && _dealers.TotalCount > 0)
                    {
                        rptDealers.DataSource = _dealers.Dealers;
                        rptDealers.DataBind();
                        totalDealers = _dealers.TotalCount;

                        if (totalDealers > 0)
                        {
                            int _countStdDealers = _dealers.Dealers.Count(m => m.DealerType < 2);// counting only standard or invalid dealers
                            int _countPreDel = _dealers.Dealers.Count(m => m.DealerType > 1);// counting only deluxe or premium dealers
                            if (_countStdDealers < 3 && _countPreDel > 0)
                            {
                                areDealersPremium = true;
                            }
                        }
                    }
                    else
                    {
                        Response.Redirect(Bikewale.Common.CommonOpn.AppPath + "pageNotFound.aspx", false);
                        HttpContext.Current.ApplicationInstance.CompleteRequest();
                        this.Page.Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                Trace.Warn(ex.Message);
                ErrorClass objErr = new ErrorClass(ex, "BindDealerList");
                objErr.SendMail();
            }
        }


        /// <summary>
        /// Created By  : Sushil Kumar
        /// Created On  : 20th March 2016
        /// Description : To bind makes list to dropdown
        /// Modified by :   Sumit Kate on 29 Mar 2016
        /// Description :   Get the makes list of BW and AB dealers
        /// </summary>
        private void BindMakesDropdown()
        {
            IEnumerable<BikeMakeEntityBase> _makes = null;
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IBikeMakesCacheRepository<int>, BikeMakesCacheRepository<BikeMakeEntity, int>>()
                             .RegisterType<ICacheManager, MemcacheManager>()
                             .RegisterType<IBikeMakes<BikeMakeEntity, int>, BikeMakesRepository<BikeMakeEntity, int>>()
                            ;
                    var objCache = container.Resolve<IBikeMakesCacheRepository<int>>();
                    _makes = objCache.GetMakesByType(EnumBikeType.Dealer);
                    if (_makes != null && _makes.Count() > 0)
                    {
                       // rptMakes.DataSource = _makes;
                      //  rptMakes.DataBind();
                        var firstMake = _makes.FirstOrDefault(x => x.MakeId == makeId);
                        if (firstMake != null)
                        {
                            makeName = firstMake.MakeName;
                            makeMaskingName = firstMake.MaskingName;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Trace.Warn(ex.Message);
                ErrorClass objErr = new ErrorClass(ex, "BindMakesDropdown");
                objErr.SendMail();
            }
        }


        /// <summary>
        /// Created By  : Sushil Kumar
        /// Created On  : 20th March 2016
        /// Description : To bind cities list to dropdown
        /// </summary>
        private void BindCitiesDropdown()
        {
            IEnumerable<CityEntityBase> _cities = null;
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IDealer, DealersRepository>();

                    var objCities = container.Resolve<IDealer>();
                    _cities = objCities.FetchDealerCitiesByMake(makeId);
                    if (_cities != null && _cities.Count() > 0)
                    {
                        rptCities.DataSource = _cities;
                        rptCities.DataBind();
                        var firstCity = _cities.FirstOrDefault(x => x.CityId == cityId);
                        if (firstCity != null)
                        {
                            cityName = firstCity.CityName;
                            cityMaskingName = firstCity.CityMaskingName;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Trace.Warn(ex.Message);
                ErrorClass objErr = new ErrorClass(ex, "BindCitiesDropdown");
                objErr.SendMail();
            }
        }


        /// <summary>
        /// Created By  : Sushil Kumar
        /// Created On  : 20th March 2016
        /// Description : To get makeId from make masking name
        /// </summary>
        private void GetMakeIdByMakeMaskingName(string maskingName)
        {
            try
            {
                if (!string.IsNullOrEmpty(maskingName))
                {
                    string _makeId = MakeMapping.GetMakeId(maskingName);
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
            catch (Exception ex)
            {
                Trace.Warn(ex.Message);
                ErrorClass objErr = new ErrorClass(ex, "GetMakeIdByMakeMaskingName");
                objErr.SendMail();
            }
        }

        /// <summary>
        /// Created by: Aditi Srivastava on 27 Sep 2016
        /// </summary>
        /// <param name="dealerid"></param>
        private void GetDealerDetails(uint dealerid)
        {
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IDealerCacheRepository, DealerCacheRepository>()
                             .RegisterType<ICacheManager, MemcacheManager>()
                             .RegisterType<IDealer, DealersRepository>()
                            ;
                    var objCache = container.Resolve<IDealerCacheRepository>();
                    dealerDetails = objCache.GetDealerDetailsAndBikes(dealerId);

                    if (dealerDetails != null )
                    {
                        
                        dealerName = dealerDetails.DealerDetails.Name;
                        dealerMaskingName = UrlFormatter.RemoveSpecialCharUrl(dealerName);
                        cityName = dealerDetails.DealerDetails.City;
                        areaName = dealerDetails.DealerDetails.Area.AreaName;
                        address = dealerDetails.DealerDetails.Address;
                        maskingNumber = dealerDetails.DealerDetails.MaskingNumber;
                        eMail = dealerDetails.DealerDetails.EMail;
                        workingHours = dealerDetails.DealerDetails.WorkingHours;
                        
                          }
                    else
                    {
                        Response.Redirect(Bikewale.Common.CommonOpn.AppPath + "pageNotFound.aspx", false);
                        HttpContext.Current.ApplicationInstance.CompleteRequest();
                        this.Page.Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                Trace.Warn(ex.Message);
                ErrorClass objErr = new ErrorClass(ex, "BindDealerList");
                objErr.SendMail();
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
                    makeMaskingName = currentReq.QueryString["make"];
                   urlCityMaskingName = currentReq.QueryString["city"];
                    dealerId = Convert.ToUInt32(currentReq.QueryString["dealerid"]);
                      if (dealerId > 0 && !String.IsNullOrEmpty(urlCityMaskingName) && !String.IsNullOrEmpty(makeMaskingName))
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
                Trace.Warn("ProcessQueryString Ex: ", ex.Message);
                ErrorClass objErr = new ErrorClass(ex, currentReq.ServerVariables["URL"]);
                objErr.SendMail();
            }
            return isValidQueryString;
        }
        
        #endregion
    }   // End of class
}   // End of namespace