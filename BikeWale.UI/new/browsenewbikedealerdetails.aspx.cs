using Bikewale.Cache.BikeData;
using Bikewale.Cache.Core;
using Bikewale.Cache.DealersLocator;
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
using Bikewale.Memcache;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Bikewale.New
{
    /// <summary>
    /// Created By : Sushil Kumar on 19th March 2016
    /// Class to show the bike dealers details
    /// Modified By : Aditi Srivasatva on 30 Nov 2016
    /// Description : Added control to change brand and city for dealers list
    /// Modified By : Sushil Kumar on 17th Jan 2016
    /// Description : Added chnage location prompt widget
    /// </summary>
    public class BrowseNewBikeDealerDetails : Page
    {
        protected string makeName = string.Empty, modelName = string.Empty, cityName = string.Empty, areaName = string.Empty, makeMaskingName = string.Empty, cityMaskingName = string.Empty, urlCityMaskingName = string.Empty;
        protected uint cityId, makeId;
        protected ushort totalDealers;
        protected Repeater rptDealers;
        protected string clientIP = string.Empty, pageUrl = string.Empty;
        protected bool areDealersPremium = false;
        protected MostPopularBikes_new ctrlPopoularBikeMake;
        protected LeadCaptureControl ctrlLeadCapture;
        protected ServiceCenterCard ctrlServiceCenterCard;
        protected BrandCityPopUp ctrlBrandCity;
        protected DealersInNearByCities ctrlDealerCount;
        protected ChangeLocationPopup ctrlChangeLocation;
        protected UsedBikeModel ctrlusedBikeModel;
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

            if (ProcessQueryString())
            {
                GetMakeIdByMakeMaskingName();

                if (makeId > 0 && cityId > 0)
                {
                    BindMakesDropdown();
                    BindCitiesDropdown();
                    BindDealerList();
                }
                else
                {
                    Response.Redirect(Bikewale.Common.CommonOpn.AppPath + "pageNotFound.aspx", false);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                    this.Page.Visible = false;
                }
            }

            BindUserControls();

        }
        /// Modified By :-Subodh Jain on 1 Dec 2016
        /// Summary :- Added Service center Widget
        /// Modified By : Aditi Srivasatva on 30 Nov 2016
        /// Description : Set request type according to page for brand city pop up
        /// Modified by : Sajal Gupta on 20-12-2016
        /// Desc : Binded dealer count widget
        /// Modified By : Sushil Kumar on 17th Jan 2016
        /// Description : Added chnage location prompt widget
        /// Modified By :-Subodh Jain on 15 March 2017
        /// Summary :-Added used bike widget
        /// </summary>
        private void BindUserControls()
        {
            try
            {
                ctrlDealerCount.MakeId = makeId;
                ctrlDealerCount.CityId = cityId;
                ctrlDealerCount.TopCount = 8;
                ctrlDealerCount.MakeMaskingName = makeMaskingName;
                ctrlDealerCount.CityName = cityName;
                ctrlDealerCount.MakeName = makeName;

                ctrlPopoularBikeMake.makeId = (int)makeId;
                ctrlPopoularBikeMake.cityId = (int)cityId;
                ctrlPopoularBikeMake.totalCount = 9;
                ctrlPopoularBikeMake.cityname = cityName;
                ctrlPopoularBikeMake.cityMaskingName = cityMaskingName;
                ctrlPopoularBikeMake.makeName = makeName;

                ctrlLeadCapture.CityId = cityId;
                ctrlBrandCity.requestType = EnumBikeType.Dealer;
                ctrlBrandCity.makeId = makeId;
                ctrlBrandCity.cityId = cityId;

                ctrlServiceCenterCard.MakeId = makeId;
                ctrlServiceCenterCard.CityId = cityId;
                ctrlServiceCenterCard.makeName = makeName;
                ctrlServiceCenterCard.cityName = cityName;
                ctrlServiceCenterCard.makeMaskingName = makeMaskingName;
                ctrlServiceCenterCard.cityMaskingName = cityMaskingName;
                ctrlServiceCenterCard.TopCount = 3;
                ctrlServiceCenterCard.widgetHeading = string.Format("{0} service centers in {1}", makeName, cityName);
                ctrlServiceCenterCard.biLineText = string.Format("Check out authorized {0} service center nearby.", makeName);

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
                    ctrlusedBikeModel.WidgetTitle = string.Format("Second Hand Bikes in {0}", cityId > 0 ? cityName : "India");
                    ctrlusedBikeModel.header = string.Format("Used {0} bikes in {1}", makeName, cityId > 0 ? cityName : "India");
                    ctrlusedBikeModel.WidgetHref = string.Format("/used/{0}-bikes-in-{1}/", makeMaskingName, cityId > 0 ? cityMaskingName : "india");
                    ctrlusedBikeModel.TopCount = 9;
                }
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, "BrowseNewBikeDealerDetails.BindUserControls");
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
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, "BindDealerList");

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
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, "BindMakesDropdown");

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
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, "BindCitiesDropdown");

            }
        }


        /// <summary>
        /// Created By  : Sushil Kumar
        /// Created On  : 20th March 2016
        /// Description : To get makeId from make masking name
        /// Modified by :   Sumit Kate on 03 Oct 2016
        /// Description :   Handle Make masking name rename 301 redirection
        /// </summary>
        private void GetMakeIdByMakeMaskingName()
        {
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
                    Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, Request.ServerVariables["URL"] + "ParseQueryString");

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
                            makeId = objMakeResponse.MakeId;
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
                Trace.Warn("ProcessQueryString Ex: ", ex.Message);
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, currentReq.ServerVariables["URL"]);

            }
            return isValidQueryString;
        }
        #endregion
    }   // End of class
}   // End of namespace