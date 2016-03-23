﻿using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;
using System.Data.SqlClient;

using Bikewale.Memcache;
using Bikewale.Entities.PriceQuote;
using Microsoft.Practices.Unity;
using System.Collections.Generic;
using Bikewale.Entities.BikeData;
using Bikewale.Interfaces.BikeData;
using Bikewale.Cache.BikeData;
using Bikewale.Cache.Core;
using System.Linq;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.DAL.BikeData;
using Bikewale.Interfaces.Location;
using Bikewale.Cache.Location;
using Bikewale.DAL.Location;
using Bikewale.Entities.Location;
using Bikewale.Entities.DealerLocator;
using Bikewale.Notifications;
using Bikewale.Interfaces.PriceQuote;
using Bikewale.BAL.PriceQuote;
using Bikewale.Interfaces.Dealer;
using Bikewale.Cache.DealersLocator;
using Bikewale.DAL.Dealer;

namespace Bikewale.New
{
    /// <summary>
    /// Created By : Sushil Kumar on 19th March 2016
    /// Class to show the bike dealers details
    /// </summary>
    public class BrowseNewBikeDealerDetails : Page
    {
        protected string makeName = string.Empty, modelName = string.Empty, cityName = string.Empty, areaName = string.Empty, makeMaskingName = string.Empty, cityMaskingName = string.Empty;
        protected uint cityId, makeId;
        protected ushort totalDealers;
        protected Repeater rptMakes, rptCities, rptDealers;


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
            string originalUrl = Request.ServerVariables["HTTP_X_ORIGINAL_URL"];
            if (String.IsNullOrEmpty(originalUrl))
                originalUrl = Request.ServerVariables["URL"];

            Bikewale.Common.DeviceDetection dd = new Bikewale.Common.DeviceDetection(originalUrl);
            dd.DetectDevice();

            ProcessQueryString();
            GetMakeIdByMakeMaskingName(makeMaskingName);

            if (makeId > 0 && cityId > 0)
            {
                BindMakesDropdown();
                BindCitiesDropdown();

                BindDealerList();
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
                    container.RegisterType<IDealerCacheRepository,DealerCacheRepository>()
                             .RegisterType<ICacheManager, MemcacheManager>()
                             .RegisterType<IDealer, DealersRepository>()
                            ;
                    var objCache = container.Resolve<IDealerCacheRepository>();
                    _dealers = objCache.GetDealerByMakeCity(cityId,makeId);

                    if (_dealers != null && _dealers.TotalCount > 0)
                    {
                        rptDealers.DataSource = _dealers.Dealers;
                        rptDealers.DataBind();
                        totalDealers = _dealers.TotalCount;
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
                    _makes = objCache.GetMakesByType(EnumBikeType.New);
                    if (_makes != null && _makes.Count() > 0)
                    {
                        rptMakes.DataSource = _makes;
                        rptMakes.DataBind();   
                        makeName = _makes.Where(x => x.MakeId == makeId).FirstOrDefault().MakeName;
                        makeMaskingName = _makes.Where(x => x.MakeId == makeId).FirstOrDefault().MaskingName;
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
                    container.RegisterType<ICityCacheRepository, CityCacheRepository>()
                             .RegisterType<ICacheManager, MemcacheManager>()
                             .RegisterType<ICity, CityRepository>()
                            ;
                    var objCache = container.Resolve<ICityCacheRepository>();
                    _cities = objCache.GetPriceQuoteCities(59);
                    if (_cities != null && _cities.Count() > 0)
                    {
                        rptCities.DataSource = _cities;
                        rptCities.DataBind();
                        cityName = _cities.Where(x => x.CityId == cityId).FirstOrDefault().CityName;
                        cityMaskingName = _cities.Where(x => x.CityId == cityId).FirstOrDefault().CityMaskingName;
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
                Trace.Warn("GetMakeIdByMakeMaskingName Ex: ", ex.Message);
                ErrorClass objErr = new ErrorClass(ex, "BindCitiesDropdown");
                objErr.SendMail();
            }
        }



        #region Private Method to process querystring
        /// <summary>
        /// Created By : Sushil Kumar
        /// Created On : 16th March 2016 
        /// Description : Private Method to query string fro make masking name and cityId
        /// </summary>
        private void ProcessQueryString()
        {
            var currentReq = HttpContext.Current.Request;
            try
            {

                if (currentReq.QueryString != null && currentReq.QueryString.HasKeys())
                {
                    makeMaskingName = currentReq.QueryString["make"];
                    uint.TryParse(currentReq.QueryString["cityId"], out cityId);
                }
                else
                {
                    Response.Redirect("/new/locate-dealers/.aspx", false);
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

        }
        #endregion
    }   // End of class
}   // End of namespace