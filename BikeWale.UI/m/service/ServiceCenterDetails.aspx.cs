using Bikewale.Cache.BikeData;
using Bikewale.Cache.Core;
using Bikewale.Cache.DealersLocator;
using Bikewale.CoreDAL;
using Bikewale.DAL.BikeData;
using Bikewale.DAL.Dealer;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.DealerLocator;
using Bikewale.Entities.PriceQuote;
using Bikewale.Entities.ServiceCenters;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.Dealer;
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
        protected uint dealerId, campaignId = 0, cityId;
        protected int makeId, dealerBikesCount = 0;
        protected DealerDetailEntity dealerDetails;
        protected bool isDealerDetail;
        protected string cityName = string.Empty, makeName = string.Empty, maskingNumber = string.Empty, makeMaskingName = string.Empty, cityMaskingName = string.Empty, dealerName = string.Empty, dealerArea = string.Empty, dealerCity = string.Empty, clientIP = CommonOpn.GetClientIP();
        protected double dealerLat, dealerLong;
        protected DealersCard ctrlDealerCard;
        protected ServiceCenterCard ctrlServiceCenterCard;
        protected ServiceSchedule ctrlServiceSchedule;
        protected LeadCaptureControl ctrlLeadCapture;

        protected ServiceCenterData centerList = null;
        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {

            if (ProcessQueryString() && dealerId > 0)
            {
                GetDealerDetails();
                BindControls();
            }
        }

        private void BindControls()
        {
            ctrlServiceCenterCard.MakeId = 1; //(uint)dealerDetails.MakeId;
            //ctrlServiceCenterCard.makeMaskingName = dealerDetails.MakeMaskingName;
            //ctrlServiceCenterCard.makeName = dealerDetails.MakeName;
            ctrlServiceCenterCard.CityId = 1; //(uint)dealerDetails.CityId;
            //ctrlServiceCenterCard.cityName = dealerCity;
            //ctrlServiceCenterCard.PageName = "Dealer_Details";
            ctrlServiceCenterCard.TopCount = 9;
            //ctrlServiceCenterCard.PQSourceId = (int)PQSourceEnum.Mobile_dealer_details_Get_offers;
            //ctrlServiceCenterCard.LeadSourceId = 15;
            //ctrlServiceCenterCard.DealerId = dealerId;

            ctrlDealerCard.MakeId = (uint)dealerDetails.MakeId;
            ctrlDealerCard.makeMaskingName = dealerDetails.MakeMaskingName;
            ctrlDealerCard.makeName = dealerDetails.MakeName;
            ctrlDealerCard.CityId = (uint)dealerDetails.CityId;
            ctrlDealerCard.cityName = dealerCity;
            ctrlDealerCard.PageName = "Dealer_Details";
            ctrlDealerCard.TopCount = 6;
            ctrlDealerCard.PQSourceId = (int)PQSourceEnum.Mobile_dealer_details_Get_offers;
            ctrlDealerCard.LeadSourceId = 15;
            ctrlDealerCard.DealerId = (int)dealerId;
            ctrlLeadCapture.CityId = (uint)dealerDetails.CityId;

            ctrlServiceSchedule.makeId = 1;
            ctrlLeadCapture.CityId = (uint)dealerDetails.CityId;
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
                             .RegisterType<IDealer, DealersRepository>();
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
                        makeName = dealerDetails.MakeName;
                        campaignId = dealerDetails.CampaignId;
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
                ErrorClass objErr = new ErrorClass(ex, "GetDealerDetails");
                objErr.SendMail();
            }
        }
        #endregion

        #region Private Method to process querystring
        /// <summary>
        /// Created By : Sushil Kumar
        /// Created On : 16th March 2016 
        /// Description : Process query string dealerId and campaignId
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
                            container.RegisterType<IBikeMakesCacheRepository<int>, BikeMakesCacheRepository<BikeMakeEntity, int>>()
                                  .RegisterType<ICacheManager, MemcacheManager>()
                                  .RegisterType<IBikeMakes<BikeMakeEntity, int>, BikeMakesRepository<BikeMakeEntity, int>>();
                            var objCache = container.Resolve<IBikeMakesCacheRepository<int>>();
                            objResponse = objCache.GetMakeMaskingResponse(makeMaskingName);
                        }
                    }
                    else
                    {
                        Response.Redirect(CommonOpn.AppPath + "pageNotFound.aspx", false);
                        HttpContext.Current.ApplicationInstance.CompleteRequest();
                        this.Page.Visible = false;
                        isSucess = false;
                    }
                    return true;
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
                        CommonOpn.RedirectPermanent(Request.RawUrl.Replace(makeMaskingName, objResponse.MaskingName));
                        isSucess = false;
                    }
                    else
                    {
                        Response.Redirect(CommonOpn.AppPath + "pageNotFound.aspx", false);
                        HttpContext.Current.ApplicationInstance.CompleteRequest();
                        this.Page.Visible = false;
                        isSucess = false;
                    }
                }
                else
                {
                    Response.Redirect(CommonOpn.AppPath + "pageNotFound.aspx", false);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                    this.Page.Visible = false;
                    isSucess = false;
                }
            }
            return isSucess;
        }
        #endregion
    }
}