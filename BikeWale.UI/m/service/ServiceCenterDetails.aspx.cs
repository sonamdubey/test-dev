using Bikewale.BAL.ServiceCenter;
using Bikewale.Cache.Core;
using Bikewale.Cache.DealersLocator;
using Bikewale.Cache.ServiceCenter;
using Bikewale.CoreDAL;
using Bikewale.DAL.BikeData;
using Bikewale.DAL.Dealer;
using Bikewale.DAL.ServiceCenter;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.DealerLocator;
using Bikewale.Entities.PriceQuote;
using Bikewale.Entities.service;
using Bikewale.Entities.ServiceCenters;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.Dealer;
using Bikewale.Interfaces.ServiceCenter;
using Bikewale.Mobile.Controls;
using Bikewale.Notifications;
using Microsoft.Practices.Unity;
using System;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;


namespace Bikewale.Mobile.Service
{
    /// <summary>
    /// Modified By : Sushil Kumar
    /// Modified On : 25 March 2016
    /// Description : To show dealer details based on dealer id an campaign id.
    /// Modified By : Lucky Rathore on 30 March 2016
    /// Description : dealerLat, dealerLong, dealerName, dealerArea, dealerCity added and _dealerQuery removed.
    /// </summary>
    public class ServiceCenterDetails : System.Web.UI.Page
    {
        protected Repeater rptModels, rptModelList;
        protected uint dealerId, campaignId = 0, cityId, serviceCenterId = 0;
        protected int dealerBikesCount = 0;
        protected DealerDetailEntity dealerDetails;
        protected bool isDealerDetail;
        protected string cityName = string.Empty;
        protected string makeName = string.Empty, dealerName = string.Empty, dealerArea = string.Empty, dealerCity = string.Empty;
        protected double dealerLat, dealerLong;
        protected DealersCard ctrlDealerCard;
        protected LeadCaptureControl ctrlLeadCapture;
        protected String clientIP = CommonOpn.GetClientIP();
        protected string maskingNumber;
        protected string makeMaskingName;
        protected int makeId;
        protected string cityMaskingName = String.Empty;
        protected ServiceCenterCompleteData objServiceCenterCompleteData = null;
        protected BikeMakeEntityBase objBikeMakeEntityBase;

        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (ProcessQueryString() && serviceCenterId > 0)
            {
                BindServiceCenterData();
                //GetDealerDetails();
                BindDealerCard();
            }
        }

        private void BindServiceCenterData()
        {
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IServiceCenter, ServiceCenter<ServiceCenterLocatorList, int>>()
                    .RegisterType<IServiceCenterCacheRepository, ServiceCenterCacheRepository>()
                    .RegisterType<IServiceCenterRepository<ServiceCenterLocatorList, int>, ServiceCenterRepository<ServiceCenterLocatorList, int>>()
                    .RegisterType<ICacheManager, MemcacheManager>();
                    var objServiceCenter = container.Resolve<IServiceCenter>();

                    objServiceCenterCompleteData = objServiceCenter.GetServiceCenterDataById(serviceCenterId);

                    if (objServiceCenterCompleteData != null)
                    {
                        cityId = objServiceCenterCompleteData.CityId;
                        makeId = (int)objServiceCenterCompleteData.MakeId;
                        GetMakeNameByMakeId(objServiceCenterCompleteData.MakeId);
                        dealerLat = Convert.ToDouble(objServiceCenterCompleteData.Lattitude);
                        dealerLong = Convert.ToDouble(objServiceCenterCompleteData.Longitude);
                    }
                }
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, string.Format("BindServiceCenterData for serviceCenterId : {0} ", serviceCenterId));
                objErr.SendMail();
            }
        }

        /// <summary>
        /// Created by : SAJAL GUPTA on 08-11-2016
        /// Description: Method to bind dealer card widget data.
        /// </summary>
        private void BindDealerCard()
        {
            ctrlDealerCard.MakeId = (uint)makeId;
            ctrlDealerCard.makeMaskingName = makeMaskingName;
            ctrlDealerCard.CityId = cityId;
            ctrlDealerCard.cityName = cityName;
            ctrlDealerCard.PageName = "Service_Center_DetailsPage";
            ctrlDealerCard.TopCount = 9;
            ctrlDealerCard.PQSourceId = (int)PQSourceEnum.Mobile_ServiceCenter_DetailsPage;
            ctrlDealerCard.LeadSourceId = 17;
            ctrlDealerCard.DealerId = 0;
            ctrlDealerCard.isHeadingNeeded = false;
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
        /// </summary>
        private void GetDealerDetails()
        {
            DealerBikesEntity dealer = null;
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IDealerCacheRepository, DealerCacheRepository>()
                             .RegisterType<ICacheManager, MemcacheManager>()
                             .RegisterType<IDealer, DealersRepository>()
                            ;
                    var objCache = container.Resolve<IDealerCacheRepository>();
                    dealer = objCache.GetDealerDetailsAndBikesByDealerAndMake(dealerId, makeId);

                    if (dealer != null && dealer.DealerDetails != null)
                    {
                        dealerDetails = dealer.DealerDetails;

                        isDealerDetail = true;

                        cityMaskingName = dealerDetails.CityMaskingName;

                        dealerName = dealerDetails.Name;
                        dealerArea = dealerDetails.Area.AreaName;
                        dealerCity = dealerDetails.City;
                        if (dealerDetails.Area != null)
                        {
                            dealerLat = dealerDetails.Area.Latitude;
                            dealerLong = dealerDetails.Area.Longitude;
                        }
                        ctrlDealerCard.MakeId = (uint)dealerDetails.MakeId;
                        ctrlDealerCard.makeMaskingName = dealerDetails.MakeMaskingName;
                        ctrlDealerCard.makeName = dealerDetails.MakeName;
                        ctrlDealerCard.CityId = (uint)dealerDetails.CityId;
                        ctrlDealerCard.cityName = dealerCity;
                        ctrlDealerCard.PageName = "Dealer_Details";
                        ctrlDealerCard.TopCount = 6;
                        ctrlDealerCard.PQSourceId = (int)PQSourceEnum.Mobile_dealer_details_Get_offers;
                        ctrlDealerCard.LeadSourceId = 15;

                        makeName = dealerDetails.MakeName;
                        campaignId = dealerDetails.CampaignId;
                        ctrlDealerCard.DealerId = (int)dealerId;

                        ctrlLeadCapture.CityId = (uint)dealerDetails.CityId;

                        maskingNumber = dealerDetails.MaskingNumber;

                        if (dealer.Models != null && dealer.Models.Count() > 0)
                        {
                            rptModels.DataSource = dealer.Models;
                            rptModels.DataBind();
                            dealerBikesCount = dealer.Models.Count();
                        }
                    }
                    else
                    {
                        Response.Redirect("/pagenotfound.aspx", false);
                        HttpContext.Current.ApplicationInstance.CompleteRequest();
                        this.Page.Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                Trace.Warn(ex.Message);
                ErrorClass objErr = new ErrorClass(ex, "GetDealerDetails");
                objErr.SendMail();
            }

        }
        #endregion

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
                }
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, "GetMakeNameByMakeId");
                objErr.SendMail();
            }
        }

        #region Private Method to process querystring
        /// <summary>
        /// Created By : Sajal Gupta
        /// Created On : 09-11-2016
        /// Description : Private Method to parse encoded query string and get values for serviceCenterId
        /// </summary>
        private bool ProcessQueryString()
        {
            var currentReq = HttpContext.Current.Request;
            bool isSucess = true;
            try
            {
                if (currentReq.QueryString != null && currentReq.QueryString.HasKeys())
                {
                    uint.TryParse(currentReq.QueryString["id"], out serviceCenterId);
                    makeMaskingName = currentReq.QueryString["makemaskingname"];
                }
                else
                {
                    Response.Redirect("/pagenotfound.aspx", false);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                    this.Page.Visible = false;
                    isSucess = false;
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, currentReq.ServerVariables["URL"]);
                objErr.SendMail();
                isSucess = false;
            }
            return isSucess;
        }
        #endregion
    }
}