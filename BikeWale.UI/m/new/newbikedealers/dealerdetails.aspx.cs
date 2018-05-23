using Bikewale.BAL.ApiGateway.ApiGatewayHelper;
using Bikewale.BAL.Dealer;
using Bikewale.Cache.BikeData;
using Bikewale.Cache.Core;
using Bikewale.Cache.DealersLocator;
using Bikewale.Common;
using Bikewale.DAL.BikeData;
using Bikewale.DAL.Dealer;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.DealerLocator;
using Bikewale.Entities.Location;
using Bikewale.Entities.PriceQuote;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.Dealer;
using Bikewale.Mobile.Controls;
using Bikewale.Utility;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;


namespace Bikewale.Mobile
{
    /// <summary>
    /// Modified By : Sushil Kumar
    /// Modified On : 25 March 2016
    /// Description : To show dealer details based on dealer id an campaign id.
    /// Modified By : Lucky Rathore on 30 March 2016
    /// Description : dealerLat, dealerLong, dealerName, dealerArea, dealerCity added and _dealerQuery removed.
    /// </summary>
    public class DealerDetails : System.Web.UI.Page
    {
        protected Repeater rptModels, rptModelList;
        protected IEnumerable<MostPopularBikesBase> dealerModels;
        protected uint dealerId, campaignId, cityId, customerCityId, customerAreaId, areaId, pqAreaId, pqCityId;
        protected int dealerBikesCount = 0, makeId;
        protected DealerDetailEntity dealerDetails;
        protected bool isDealerDetail;
        protected double dealerLat, dealerLong;
        protected DealersCard ctrlDealerCard;
        protected LeadCaptureControl ctrlLeadCapture;
        protected string maskingNumber, makeMaskingName, customerAreaName = string.Empty, pqAreaName = string.Empty, cityMaskingName = string.Empty, clientIP = Bikewale.Utility.CurrentUser.GetClientIP(), areaName = string.Empty,
            cityName = string.Empty, makeName = string.Empty, dealerName = string.Empty, dealerArea = string.Empty, dealerCity = string.Empty, ctaSmallText = string.Empty;
        protected MMostPopularBikes ctrlPopoularBikeMake;
        protected ServiceCenterCard ctrlServiceCenterCard;
        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {

            if (ProcessQueryString() && dealerId > 0)
            {
                GetDealerDetails();
                ProcessGlobalLocationCookie();
                BindUserControl();
            }

        }
        /// <summary>
        /// Created By:-Subodh Jain 2 Dec 2016
        /// Summary :- Bind Popular Bikes By make on page
        /// Modified By :-Subodh Jain on 1 Dec 2016
        /// Summary :- Added Service center Widget
        /// </summary>
        private void BindUserControl()
        {
            try
            {
                ctrlPopoularBikeMake.makeId = makeId;
                ctrlPopoularBikeMake.cityId = dealerDetails.CityId;
                ctrlPopoularBikeMake.totalCount = 9;
                ctrlPopoularBikeMake.cityname = dealerCity;
                ctrlPopoularBikeMake.cityMaskingName = cityMaskingName;
                ctrlPopoularBikeMake.makeName = makeName;
                ctrlPopoularBikeMake.makeMaskingName = makeMaskingName;

                ctrlServiceCenterCard.MakeId = Convert.ToUInt32(makeId);
                ctrlServiceCenterCard.makeMaskingName = makeMaskingName;
                ctrlServiceCenterCard.makeName = makeName;
                ctrlServiceCenterCard.CityId = Convert.ToUInt32(dealerDetails.CityId);
                ctrlServiceCenterCard.cityName = dealerCity;
                ctrlServiceCenterCard.cityMaskingName = cityMaskingName;
                ctrlServiceCenterCard.TopCount = 9;
                ctrlServiceCenterCard.widgetHeading = string.Format("{0} service centers in {1}", makeName, dealerCity);
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass.LogError(ex, "DealerDetails.BindUserControl");

            }

        }

        /// <summary>
        /// Created by  :   Sumit Kate on 19 Jan 2017
        /// Description :   Process Global Cookie
        /// </summary>
        private void ProcessGlobalLocationCookie()
        {
            GlobalCityAreaEntity location = GlobalCityArea.GetGlobalCityArea();
            customerCityId = location.CityId;
            customerAreaId = location.AreaId;
            if (customerCityId == cityId && customerAreaId > 0)
            {
                pqCityId = cityId;
                pqAreaId = customerAreaId;
                customerAreaName = location.Area.Replace('-', ' ');
                pqAreaName = customerAreaName;
            }
            else
            {
                pqCityId = cityId;
                pqAreaId = areaId;
                pqAreaName = areaName;
            }
        }

        #region Get Dealer Details
        /// <summary>
        /// Created By : Sushil Kumar
        /// Created On : 25th March 2016 
        /// Description : To get dealer details and bikes available at dealership
        /// Modified By : Lucky Rathore on 30 March 2016
        /// Description : dealerLat, dealerLong, dealerName, dealerArea, dealerCity Intialize, renamed dealer from _dealer.
        /// Modified By : Sajal Gupta on 26-09-2016
        /// Description : Changed method to get details only on basis of (dealerId and makeid) and added details of dealer to the controller.
        /// Modified By :-Subodh Jain on 16 Dec 2016
        /// Summary :- Added heading to dealer widget
        /// Modified by : Sajal Gupta on 29-12-2016
        /// Description : Added ctaSmallText
        /// Modified by : Rajan Chauhan on 16 Apr 2018
        /// Description : Changed call GetDealerDetailsAndBikesByDealerAndMake from cache to BAL
        /// </summary>
        private void GetDealerDetails()
        {
            DealerBikesEntity dealer = null;
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IDealer, Dealer>()
                             .RegisterType<IDealerCacheRepository, DealerCacheRepository>()
                             .RegisterType<ICacheManager, MemcacheManager>()
                             .RegisterType<IDealerRepository, DealersRepository>()
                             .RegisterType<IApiGatewayCaller, ApiGatewayCaller>();
                    var objDealer = container.Resolve<IDealer>();
                    dealer = objDealer.GetDealerDetailsAndBikesByDealerAndMake(dealerId, makeId);

                    if (dealer != null && dealer.DealerDetails != null)
                    {
                        dealerDetails = dealer.DealerDetails;

                        isDealerDetail = true;

                        cityMaskingName = dealerDetails.CityMaskingName;

                        dealerName = dealerDetails.Name;
                        dealerArea = dealerDetails.objArea.AreaName;
                        dealerCity = dealerDetails.City;

                        ctaSmallText = dealerDetails.DisplayTextSmall;

                        if (dealerDetails.Area != null)
                        {
                            areaId = dealerDetails.objArea.AreaId;
                            dealerLat = dealerDetails.objArea.Latitude;
                            dealerLong = dealerDetails.objArea.Longitude;
                        }
                        ctrlDealerCard.MakeId = (uint)dealerDetails.MakeId;
                        ctrlDealerCard.makeMaskingName = dealerDetails.MakeMaskingName;
                        ctrlDealerCard.makeName = dealerDetails.MakeName;
                        ctrlDealerCard.CityId = cityId = (uint)dealerDetails.CityId;
                        ctrlDealerCard.cityName = cityName = dealerCity;
                        ctrlDealerCard.PageName = "Dealer_Details";
                        ctrlDealerCard.TopCount = 6;
                        ctrlDealerCard.PQSourceId = (int)PQSourceEnum.Mobile_dealer_details_Get_offers;
                        ctrlDealerCard.LeadSourceId = 15;
                        ctrlDealerCard.widgetHeading = string.Format("More {0} showrooms", dealerDetails.MakeName);
                        makeName = dealerDetails.MakeName;
                        campaignId = dealerDetails.CampaignId;
                        ctrlDealerCard.DealerId = (int)dealerId;

                        ctrlLeadCapture.CityId = (uint)dealerDetails.CityId;

                        maskingNumber = dealerDetails.MaskingNumber;

                        if (dealer.Models != null && dealer.Models.Any())
                        {
                            dealerModels = dealer.Models;
                            dealerBikesCount = dealer.Models.Count();
                        }
                    }
                    else
                    {
                        UrlRewrite.Return404();
                    }
                }
            }
            catch (Exception ex)
            {
                Trace.Warn(ex.Message);
                Bikewale.Notifications.ErrorClass.LogError(ex, "GetDealerDetails");

            }

        }
        #endregion

        #region Private Method to process querystring
        /// <summary>
        /// Created By : Sushil Kumar
        /// Created On : 16th March 2016 
        /// Description : Private Method to parse encoded query string and get values for dealerId and campaignId
        /// Modified By : Lucky Rathore on 30 March 2016
        /// Description : Renamed dealerQuery from _dealerQuery.
        /// Modified By : Sajal Gupta on 29-09-2016
        /// Description : Changed query string parametres.
        /// Modified by :   Sumit Kate on 03 Oct 2016
        /// Description :   Handle Make masking name rename 301 redirection
        /// </summary>
        private bool ProcessQueryString()
        {
            var currentReq = HttpContext.Current.Request;
            MakeMaskingResponse objResponse = null;
            bool isSucess = true;
            try
            {

                if (currentReq.QueryString != null && currentReq.QueryString.HasKeys())
                {
                    uint.TryParse(currentReq.QueryString["dealerId"], out dealerId);
                    makeMaskingName = currentReq.QueryString["makemaskingname"];

                    if (!String.IsNullOrEmpty(makeMaskingName))
                    {

                        using (IUnityContainer container = new UnityContainer())
                        {
                            container.RegisterType<IBikeMakesCacheRepository, BikeMakesCacheRepository>()
                                  .RegisterType<ICacheManager, MemcacheManager>()
                                  .RegisterType<IBikeMakes<BikeMakeEntity, int>, BikeMakesRepository<BikeMakeEntity, int>>()
                                 ;
                            var objCache = container.Resolve<IBikeMakesCacheRepository>();

                            objResponse = objCache.GetMakeMaskingResponse(makeMaskingName);
                        }
                    }
                    else
                    {
                        UrlRewrite.Return404();
                        isSucess = false;
                    }
                    return true;
                }
                else
                {
                    UrlRewrite.Return404();
                    isSucess = false;
                }
            }
            catch (Exception ex)
            {
                Trace.Warn("ProcessQueryString Ex: ", ex.Message);
                Bikewale.Notifications.ErrorClass.LogError(ex, currentReq.ServerVariables["URL"]);

                isSucess = false;
            }
            finally
            {
                if (objResponse != null)
                {
                    if (objResponse.StatusCode == 200)
                    {
                        makeId = Convert.ToInt32(objResponse.MakeId);
                        isSucess = true;
                    }
                    else if (objResponse.StatusCode == 301)
                    {
                        Bikewale.Common.CommonOpn.RedirectPermanent(Request.RawUrl.Replace(makeMaskingName, objResponse.MaskingName));
                        isSucess = false;
                    }
                    else
                    {
                        UrlRewrite.Return404();
                        isSucess = false;
                    }
                }
                else
                {
                    UrlRewrite.Return404();
                    isSucess = false;
                }
            }
            return isSucess;
        }
        #endregion
    }
}