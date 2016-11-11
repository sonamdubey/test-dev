using Bikewale.BAL.ServiceCenter;
using Bikewale.Cache.Core;
using Bikewale.Cache.ServiceCenter;
using Bikewale.CoreDAL;
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
    /// Description : dealerLat, dealerLong, dealerName, dealerArea, dealerCity added and _dealerQuery removed.
    /// </summary>
    public class ServiceCenterDetails : System.Web.UI.Page
    {
        protected Repeater rptModels, rptModelList;
        protected uint dealerId, campaignId = 0, cityId, serviceCenterId = 0;
        protected int makeId, dealerBikesCount = 0;
        protected bool isDealerDetail;
        protected string cityName = string.Empty, makeName = string.Empty, maskingNumber = string.Empty, makeMaskingName = string.Empty, cityMaskingName = string.Empty, dealerName = string.Empty, dealerArea = string.Empty, dealerCity = string.Empty, clientIP = CommonOpn.GetClientIP();
        protected double dealerLat, dealerLong;
        protected DealersCard ctrlDealerCard;
        protected ServiceCenterCard ctrlServiceCenterCard;
        protected ServiceSchedule ctrlServiceSchedule;
        protected LeadCaptureControl ctrlLeadCapture;
        protected ServiceCenterCompleteData objServiceCenterData = null;
        protected BikeMakeEntityBase objBikeMakeEntityBase;

        protected ServiceCenterData centerList = null;
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
                BindControls();
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

                    objServiceCenterData = objServiceCenter.GetServiceCenterDataById(serviceCenterId);

                    if (objServiceCenterData != null)
                    {
                        cityId = objServiceCenterData.CityId;
                        makeId = (int)objServiceCenterData.MakeId;
                        GetMakeNameByMakeId(objServiceCenterData.MakeId);
                        dealerLat = Convert.ToDouble(objServiceCenterData.Lattitude);
                        dealerLong = Convert.ToDouble(objServiceCenterData.Longitude);
                    }
                }
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, string.Format("BindServiceCenterData for serviceCenterId : {0} ", serviceCenterId));
                objErr.SendMail();
            }
        }

        private void BindControls()
        {
            ctrlServiceCenterCard.MakeId = (uint)makeId; //(uint)dealerDetails.MakeId;
            //ctrlServiceCenterCard.makeMaskingName = dealerDetails.MakeMaskingName;
            //ctrlServiceCenterCard.makeName = dealerDetails.MakeName;
            ctrlServiceCenterCard.CityId = cityId; //(uint)dealerDetails.CityId;
            //ctrlServiceCenterCard.cityName = dealerCity;
            //ctrlServiceCenterCard.PageName = "Dealer_Details";
            ctrlServiceCenterCard.TopCount = 9;
            //ctrlServiceCenterCard.PQSourceId = (int)PQSourceEnum.Mobile_dealer_details_Get_offers;
            //ctrlServiceCenterCard.LeadSourceId = 15;
            //ctrlServiceCenterCard.DealerId = dealerId;
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

            ctrlLeadCapture.CityId = cityId;

            ctrlServiceSchedule.makeId = 1;
            ctrlLeadCapture.CityId = cityId;
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