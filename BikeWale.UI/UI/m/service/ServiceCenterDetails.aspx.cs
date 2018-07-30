using Bikewale.BAL.ServiceCenter;
using Bikewale.Cache.Core;
using Bikewale.Cache.ServiceCenter;
using Bikewale.Common;
using Bikewale.DAL.BikeData;
using Bikewale.DAL.ServiceCenter;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.PriceQuote;
using Bikewale.Entities.service;
using Bikewale.Entities.ServiceCenters;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.ServiceCenter;
using Bikewale.Mobile.Controls;
using Bikewale.Notifications;
using Bikewale.Utility;
using Microsoft.Practices.Unity;
using System;
using System.Web;
using System.Web.UI.WebControls;


namespace Bikewale.Mobile.Service
{
    /// <summary>
    /// Modified By : Sushil Kumar
    /// Modified On : 25 March 2016
    /// Description : To show dealer details based on dealer id an campaign id.
    /// Modified By : Lucky Rathore on 30 March 2016
    /// Description : serviceLat, dealerLong, dealerName, dealerArea, dealerCity added and _dealerQuery removed.
    /// </summary>
    public class ServiceCenterDetails : System.Web.UI.Page
    {
        protected Repeater rptModels, rptModelList;
        protected uint dealerId, campaignId = 0, cityId, serviceCenterId = 0, makeId = 0;
        protected int objServicemakeId, dealerBikesCount = 0;
        protected bool isDealerDetail;
        protected string makeName = string.Empty, maskingNumber = string.Empty, makeMaskingName = string.Empty, cityMaskingName = string.Empty,
            serviceCenteName = string.Empty, serviceCity = string.Empty, clientIP = Bikewale.Utility.CurrentUser.GetClientIP();
        protected double serviceLat, serviceLong;
        protected DealersCard ctrlDealerCard;
        protected ServiceCenterCard ctrlServiceCenterCard;
        protected ServiceSchedule ctrlServiceSchedule;
        protected LeadCaptureControl ctrlLeadCapture;
        protected ServiceCenterCompleteData objServiceCenterData = null;
        protected BikeMakeEntityBase objBikeMakeEntityBase;
        protected MMostPopularBikes ctrlPopoularBikeMake;
        protected ServiceCenterData centerList = null;
        protected UsedBikeModel ctrlusedBikeModel;
        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (ProcessQueryString() && serviceCenterId > 0)
            {
                BindServiceCenterData();
                BindControls();
            }
        }
        /// <summary>
        /// Created by : Sangram Nandkhile on 16 Nov 2016
        /// Desc: Binds the bike service data by service center Id
        /// </summary>
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

                    objServiceCenterData = objServiceCenter.GetServiceCenterDataById(serviceCenterId);

                    if (objServiceCenterData != null)
                    {
                        serviceCenteName = objServiceCenterData.Name;
                        cityId = objServiceCenterData.CityId;
                        makeId = objServiceCenterData.MakeId;
                        GetMakeNameByMakeId(objServiceCenterData.MakeId);
                        serviceCity = objServiceCenterData.CityName;
                        cityMaskingName = objServiceCenterData.CityMaskingName;
                        serviceLat = Convert.ToDouble(objServiceCenterData.Lattitude);
                        serviceLong = Convert.ToDouble(objServiceCenterData.Longitude);
                    }
                }
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass.LogError(ex, string.Format("BindServiceCenterData for serviceCenterId : {0} ", serviceCenterId));
            }
        }
        /// <summary>
        /// Modified By :-Subodh Jain on 1 Dec 2016
        /// Summary :- Added Used Bike and popular bike widget
        /// Modified By :-Subodh Jain on 1 Dec 2016
        /// Summary :- Added Service center Widget
        /// Modified By :-Subodh Jain on 16 Dec 2016
        /// Summary :- Added heading to dealer widget
        /// Modified By :-Subodh Jain on 15 March 2017
        /// Summary :-Added used Bike widget
        /// <returns></returns>
        private void BindControls()
        {
            try
            {
                ctrlServiceCenterCard.ServiceCenterId = serviceCenterId;
                ctrlServiceCenterCard.MakeId = makeId;
                ctrlServiceCenterCard.makeMaskingName = makeMaskingName;
                ctrlServiceCenterCard.makeName = makeName;
                ctrlServiceCenterCard.CityId = cityId;
                ctrlServiceCenterCard.cityName = serviceCity;
                ctrlServiceCenterCard.cityMaskingName = cityMaskingName;
                ctrlServiceCenterCard.TopCount = 9;
                ctrlServiceCenterCard.widgetHeading = string.Format("Other {0} service centers in {1}", makeName, serviceCity);

                ctrlDealerCard.MakeId = makeId;
                ctrlDealerCard.makeMaskingName = makeMaskingName;
                ctrlDealerCard.CityId = cityId;
                ctrlDealerCard.cityName = serviceCity;
                ctrlDealerCard.PageName = "Service_Center_DetailsPage";
                ctrlDealerCard.TopCount = 9;
                ctrlDealerCard.PQSourceId = (int)PQSourceEnum.Mobile_ServiceCenter_DetailsPage;
                ctrlDealerCard.LeadSourceId = 17;
                ctrlDealerCard.DealerId = 0;
                ctrlDealerCard.isHeadingNeeded = true;
                ctrlDealerCard.widgetHeading = string.Format("New {0} bikes showrooms", makeName);

                ctrlServiceSchedule.MakeId = makeId;
                ctrlServiceSchedule.MakeName = makeName;


                ctrlPopoularBikeMake.makeId = (int)makeId;
                ctrlPopoularBikeMake.cityId = (int)cityId;
                ctrlPopoularBikeMake.totalCount = 9;
                ctrlPopoularBikeMake.cityname = serviceCity;
                ctrlPopoularBikeMake.cityMaskingName = cityMaskingName;
                ctrlPopoularBikeMake.makeName = makeName;
                ctrlPopoularBikeMake.makeMaskingName = makeMaskingName;
                if (ctrlusedBikeModel != null)
                {

                    ctrlusedBikeModel.MakeId = makeId;
                    if (cityId > 0)
                        ctrlusedBikeModel.CityId = cityId;
                    ctrlusedBikeModel.WidgetTitle = string.Format("Second Hand {0} Bikes in {1}", makeName, cityId > 0 ? serviceCity : "India");
                    ctrlusedBikeModel.header = string.Format("Used {0} bikes in {1}", makeName, cityId > 0 ? serviceCity : "India");
                    ctrlusedBikeModel.WidgetHref = string.Format("/m/used/{0}-bikes-in-{1}/", makeMaskingName, cityId > 0 ? cityMaskingName : "india");
                    ctrlusedBikeModel.TopCount = 9;
                }
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass.LogError(ex, "BindControls");
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
                    objBikeMakeEntityBase = makesRepository.GetMakeDetails(makeId);
                }

                if (objBikeMakeEntityBase != null)
                {
                    makeName = objBikeMakeEntityBase.MakeName;
                }
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass.LogError(ex, "ServiceCenterDetails.GetMakeNameByMakeId");
                
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
                    UrlRewrite.Return404();
                    isSucess = false;
                }
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass.LogError(ex, currentReq.ServerVariables["URL"]);
                
                isSucess = false;
            }
            return isSucess;
        }
        #endregion
    }
}