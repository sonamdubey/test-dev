using Bikewale.Cache.BikeData;
using Bikewale.Cache.Core;
using Bikewale.Cache.DealersLocator;
using Bikewale.Common;
using Bikewale.DAL.BikeData;
using Bikewale.DAL.Dealer;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.DealerLocator;
using Bikewale.Entities.Location;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.Dealer;
using Bikewale.m.Controls;
using Bikewale.Memcache;
using Bikewale.Mobile.Controls;
using Bikewale.Utility;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;


namespace Bikewale.Mobile.New
{
    /// <summary>
    /// Created By : Sushil Kumar on 19th March 2016
    /// Class to show the bike dealers details
    /// Modified by : Aditi Srivastava on 5 Dec 2016
    /// Description : Added widget for to change brand and city for dealers list
    /// Modified By : Sushil Kumar on 17th Jan 2016
    /// Description : Added chnage location prompt widget
    /// </summary>
    public class NewBikeDealerList : PageBase
    {
        protected string makeName = string.Empty, modelName = string.Empty, cityName = string.Empty, areaName = string.Empty, makeMaskingName = string.Empty, cityMaskingName = string.Empty, urlCityMaskingName = string.Empty;
        protected uint cityId, makeId;
        protected ushort totalDealers;
        protected Repeater rptDealers; //rptMakes, rptCities, ;
        protected string clientIP = string.Empty, pageUrl = string.Empty;


        protected MMostPopularBikes ctrlPopoularBikeMake;
        protected LeadCaptureControl ctrlLeadCapture;
        protected BrandCityPopUp ctrlBrandCity;
        protected ServiceCenterCard ctrlServiceCenterCard;
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

            if (ProcessQueryString())
            {
                GetMakeIdByMakeMaskingName(makeMaskingName);

                if (makeId > 0 && cityId > 0)
                {
                    BindMakesDropdown();
                    BindCitiesDropdown();
                    BindDealerList();
                    BindUserControls();
                }
                else
                {
                    UrlRewrite.Return404();
                }
            }
        }

        /// <summary>
        /// Modified By :-Subodh Jain on 1 Dec 2016
        /// Summary :- Added Service center Widget
        /// Modified by : Sajal Gupta on 20-12-2016
        /// Desc : Binded Dealercount widget
        /// Modified By : Sushil Kumar on 17th Jan 2016
        /// Description : Added chnage location prompt widget
        /// Modified By :-Subodh Jain on 15 March 2017
        /// Summary :-Added used Bike widget
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
                ctrlPopoularBikeMake.makeMaskingName = makeMaskingName;
                ctrlLeadCapture.CityId = cityId;
                ctrlBrandCity.requestType = EnumBikeType.Dealer;
                ctrlBrandCity.makeId = makeId;
                ctrlBrandCity.cityId = cityId;

                ctrlServiceCenterCard.MakeId = makeId;
                ctrlServiceCenterCard.makeMaskingName = makeMaskingName;
                ctrlServiceCenterCard.makeName = makeName;
                ctrlServiceCenterCard.CityId = cityId;
                ctrlServiceCenterCard.cityName = cityName;
                ctrlServiceCenterCard.cityMaskingName = cityMaskingName;
                ctrlServiceCenterCard.TopCount = 9;
                ctrlServiceCenterCard.widgetHeading = string.Format("{0} service centers in {1}", makeName, cityName);

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
                    ctrlusedBikeModel.WidgetHref = string.Format("/m/used/{0}-bikes-in-{1}/", makeMaskingName, cityId > 0 ? cityMaskingName : "india");
                    ctrlusedBikeModel.TopCount = 9;
                }


            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "BindUserControls : ");
                
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
                             .RegisterType<IDealerRepository, DealersRepository>()
                            ;
                    var objCache = container.Resolve<IDealerCacheRepository>();
                    _dealers = objCache.GetDealerByMakeCity(cityId, makeId);

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
                ErrorClass.LogError(ex, "BindDealerList : ");
                
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
                    container.RegisterType<IBikeMakesCacheRepository, BikeMakesCacheRepository>()
                             .RegisterType<ICacheManager, MemcacheManager>()
                             .RegisterType<IBikeMakes<BikeMakeEntity, int>, BikeMakesRepository<BikeMakeEntity, int>>()
                            ;
                    var objCache = container.Resolve<IBikeMakesCacheRepository>();
                    _makes = objCache.GetMakesByType(EnumBikeType.Dealer);
                    if (_makes != null && _makes.Any())
                    {
                        //rptMakes.DataSource = _makes;
                        //rptMakes.DataBind();
                        makeName = _makes.Where(x => x.MakeId == makeId).FirstOrDefault().MakeName;
                    }
                }
            }
            catch (Exception ex)
            {

                ErrorClass.LogError(ex, "BindMakesDropdown : ");
                
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
                    container.RegisterType<IDealerRepository, DealersRepository>();

                    var objCities = container.Resolve<IDealerRepository>();
                    _cities = objCities.FetchDealerCitiesByMake(makeId);
                    if (_cities != null && _cities.Any())
                    {
                        //rptCities.DataSource = _cities;
                        //rptCities.DataBind();
                        var _city = _cities.FirstOrDefault(x => x.CityId == cityId);
                        if (_city != null)
                        {
                            cityName = _city.CityName;
                            cityMaskingName = _city.CityMaskingName;
                        }
                        else
                        {
                            UrlRewrite.Return404();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Trace.Warn(ex.Message);
                ErrorClass.LogError(ex, "BindCitiesDropdown");
                
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
                    UrlRewrite.Return404();
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
                            UrlRewrite.Return404();
                        }
                    }
                    else
                    {
                        UrlRewrite.Return404();
                    }
                }

                if (string.IsNullOrEmpty(_makeId) || !uint.TryParse(_makeId, out makeId))
                {
                    UrlRewrite.Return404();
                }
            }
            else
            {
                UrlRewrite.Return404();
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
                        UrlRewrite.Return404();
                    }
                    clientIP = Bikewale.Common.CommonOpn.GetClientIP();
                    pageUrl = currentReq.ServerVariables["URL"];
                }
                else
                {
                    UrlRewrite.Return404();
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, " : ProcessQueryString ");
                
            }
            return isValidQueryString;
        }

        /// <summary>
        /// Modified By : Lucky Rathore on 30 March 2016
        /// Description : link URL changed.
        /// </summary>
        /// <param name="dealerType"></param>
        /// <param name="dealerId"></param>
        /// <param name="campId"></param>
        /// <param name="dealerName"></param>
        /// <returns></returns>
        public string GetDealerDetailLink(string dealerType, string dealerId, string campId, string dealerName)
        {
            string retString = string.Empty;
            if (dealerType == "2" || dealerType == "3")
            {
                string link = "/m/new/newbikedealers/dealerdetails.aspx/?query=" + Bikewale.Utility.EncodingDecodingHelper.EncodeTo64(String.Format("dealerId={0}&campId={1}&cityId={2}", dealerId, campId, cityId));
                retString = String.Format("<a class=\"text-black\" href=\"{0}\">{1}</a>", link, dealerName);
            }
            else
            {
                retString = dealerName;
            }
            return retString;
        }

        #endregion
    }   // End of class
}   // End of namespace