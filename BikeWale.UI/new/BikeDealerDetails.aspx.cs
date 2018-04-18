using Bikewale.BAL.ApiGateway.ApiGatewayHelper;
using Bikewale.BAL.Dealer;
using Bikewale.Cache.BikeData;
using Bikewale.Cache.Core;
using Bikewale.Cache.DealersLocator;
using Bikewale.Controls;
using Bikewale.CoreDAL;
using Bikewale.DAL.BikeData;
using Bikewale.DAL.Dealer;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.DealerLocator;
using Bikewale.Entities.Location;
using Bikewale.Entities.PriceQuote;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.Dealer;
using Bikewale.Notifications;
using Bikewale.Utility;
using Microsoft.Practices.Unity;
using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Bikewale.New
{
    /// <summary>
    /// Created By : Aditi Srivastava on 26 Sep 2016
    /// Class to show the bike dealers details
    /// </summary>
    public class BikeDealerDetails : Page
    {
        protected string makeName = string.Empty, modelName = string.Empty, cityName = string.Empty, areaName = string.Empty, makeMaskingName = string.Empty, cityMaskingName = string.Empty, urlCityMaskingName = string.Empty,
        address = string.Empty, maskingNumber = string.Empty, eMail = string.Empty, workingHours = string.Empty, modelImage = string.Empty, dealerName = string.Empty, dealerMaskingName = string.Empty, pincode = string.Empty,
        clientIP = string.Empty, pageUrl = string.Empty, ctaSmallText = string.Empty, customerAreaName = string.Empty, pqAreaName = string.Empty;
        protected int makeId;
        protected uint cityId, dealerId, customerCityId, customerAreaId, areaId, pqAreaId, pqCityId;
        protected ushort totalDealers;
        protected Repeater rptMakes, rptCities, rptDealers;
        protected bool areDealersPremium = false;
        protected DealerBikesEntity dealerDetails = null;
        protected DealerCard ctrlDealerCard;
        protected LeadCaptureControl ctrlLeadCapture;
        protected DealerDetailEntity dealerObj;
        protected MostPopularBikes_new ctrlPopoularBikeMake;
        protected ServiceCenterCard ctrlServiceCenterCard;
        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
        }

        void InitializeComponent()
        {
            base.Load += new EventHandler(this.Page_Load);
        }
        /// Modified By :-Subodh Jain on 16 Dec 2016
        /// Summary :- Added heading to dealer widget
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
                if (dealerId > 0)
                {
                    GetDealerDetails(dealerId);
                    ProcessGlobalLocationCookie();
                    ctrlDealerCard.MakeId = Convert.ToUInt32(makeId);
                    ctrlDealerCard.makeName = makeName;
                    ctrlDealerCard.makeMaskingName = makeMaskingName;
                    ctrlDealerCard.CityId = cityId;
                    ctrlDealerCard.PQSourceId = (int)PQSourceEnum.Desktop_dealer_details_Get_offers;
                    ctrlDealerCard.LeadSourceId = 38;
                    ctrlDealerCard.TopCount = Convert.ToUInt16(cityId > 0 ? 3 : 6);
                    ctrlDealerCard.pageName = "DealerDetail_Page_Desktop";
                    ctrlDealerCard.DealerId = (uint)dealerId;
                    ctrlDealerCard.widgetHeading = string.Format("More {0} showrooms", makeName);
                    ctrlLeadCapture.CityId = cityId;
                    ctrlLeadCapture.AreaId = 0;
                    BindUserControl();
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
                ctrlPopoularBikeMake.makeId = Convert.ToInt32(makeId);
                ctrlPopoularBikeMake.cityId = Convert.ToInt32(cityId);
                ctrlPopoularBikeMake.totalCount = 9;
                ctrlPopoularBikeMake.cityname = cityName;
                ctrlPopoularBikeMake.cityMaskingName = cityMaskingName;
                ctrlPopoularBikeMake.makeName = makeName;

                ctrlServiceCenterCard.MakeId = Convert.ToUInt32(makeId);
                ctrlServiceCenterCard.CityId = cityId;
                ctrlServiceCenterCard.makeName = makeName;
                ctrlServiceCenterCard.cityName = cityName;
                ctrlServiceCenterCard.makeMaskingName = makeMaskingName;
                ctrlServiceCenterCard.cityMaskingName = cityMaskingName;
                ctrlServiceCenterCard.TopCount = 3;
                ctrlServiceCenterCard.widgetHeading = string.Format("{0} service centers in {1}", makeName, cityName);
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "BikeDealerDetails.BindUserControl");
                
            }

        }


        /// <summary>
        /// Created by: Aditi Srivastava on 27 Sep 2016
        /// Summary: Get dealer details by dealer id and make id
        /// Modeified By:- Subodh Jain 15 dec 2016
        /// Summary:- Added pincode data
        /// Modified by : Sajal Gupta on 29-12-2016
        /// Description : Added ctaSmallText
        /// Modified by :   Sumit Kate on 19 Jan 2017
        /// Description :   Set Dealers AreaId
        /// Modified By : Rajan Chauhan on 16 Apr 2017
        /// Description : Added dependency and changed call from Cache to BAL
        /// </summary>
        /// <param name="dealerid"></param>
        private void GetDealerDetails(uint dealerid)
        {
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
                    dealerDetails = objDealer.GetDealerDetailsAndBikesByDealerAndMake(dealerId, makeId);

                    if (dealerDetails != null && dealerDetails.DealerDetails != null)
                    {
                        dealerObj = dealerDetails.DealerDetails;
                        dealerName = dealerObj.Name;
                        dealerMaskingName = UrlFormatter.RemoveSpecialCharUrl(dealerName);
                        cityName = dealerObj.City;
                        if (dealerObj.Area != null)
                        {
                            areaName = dealerObj.objArea.AreaName;
                            areaId = dealerObj.objArea.AreaId;
                        }
                        address = dealerObj.Address;
                        maskingNumber = dealerObj.MaskingNumber;
                        eMail = dealerObj.EMail;
                        workingHours = dealerObj.WorkingHours;
                        makeName = dealerObj.MakeName;
                        cityMaskingName = dealerObj.CityMaskingName;
                        cityId = (uint)dealerObj.CityId;
                        pincode = dealerObj.Pincode;
                        ctaSmallText = dealerObj.DisplayTextSmall;

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
                ErrorClass.LogError(ex, "GetDealerDetails");
                
            }
        }


        #region Private Method to process querystring
        /// <summary>
        /// Created By : Aditi Srivastava
        /// Created On : 26th Sep 2016 
        /// Description : Private Method to validate query string based on dealer id
        /// Modified by :   Sumit Kate on 03 Oct 2016
        /// Description :   Handle Make masking name rename 301 redirection
        /// </summary>
        private bool ProcessQueryString()
        {
            var currentReq = HttpContext.Current.Request;
            bool isValidQueryString = false;
            MakeMaskingResponse objResponse = null;
            try
            {
                if (currentReq.QueryString != null && currentReq.QueryString.HasKeys())
                {
                    makeMaskingName = currentReq.QueryString["make"];
                    dealerId = Convert.ToUInt32(currentReq.QueryString["dealerid"]);
                    if (dealerId > 0 && !string.IsNullOrEmpty(makeMaskingName))
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
                ErrorClass.LogError(ex, currentReq.ServerVariables["URL"]);
                
            }
            finally
            {
                if (objResponse != null)
                {
                    if (objResponse.StatusCode == 200)
                    {
                        makeId = Convert.ToInt32(objResponse.MakeId);
                        isValidQueryString = true;
                    }
                    else if (objResponse.StatusCode == 301)
                    {
                        CommonOpn.RedirectPermanent(Request.RawUrl.Replace(makeMaskingName, objResponse.MaskingName));
                        isValidQueryString = false;
                    }
                    else
                    {
                        Response.Redirect(CommonOpn.AppPath + "pageNotFound.aspx", false);
                        HttpContext.Current.ApplicationInstance.CompleteRequest();
                        this.Page.Visible = false;
                        isValidQueryString = false;
                    }
                }
                else
                {
                    Response.Redirect(CommonOpn.AppPath + "pageNotFound.aspx", false);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                    this.Page.Visible = false;
                    isValidQueryString = false;
                }
            }
            return isValidQueryString;
        }

        #endregion
    }   // End of class
}   // End of namespace