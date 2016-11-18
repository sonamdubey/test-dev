using Bikewale.Cache.BikeData;
using Bikewale.Cache.Core;
using Bikewale.Cache.DealersLocator;
using Bikewale.Controls;
using Bikewale.CoreDAL;
using Bikewale.DAL.BikeData;
using Bikewale.DAL.Dealer;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.DealerLocator;
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

namespace Bikewale.Service
{
    /// <summary>
    /// Created By : Aditi Srivastava on 26 Sep 2016
    /// Class to show the bike dealers details
    /// </summary>
    public class ServiceCenterDetails : Page
    {
        protected string makeName = string.Empty, modelName = string.Empty, cityName = string.Empty, areaName = string.Empty, makeMaskingName = string.Empty, cityMaskingName = string.Empty, urlCityMaskingName = string.Empty,
        address = string.Empty, maskingNumber = string.Empty, eMail = string.Empty, workingHours = string.Empty, modelImage = string.Empty, dealerName = string.Empty, dealerMaskingName = string.Empty,
        clientIP = string.Empty, pageUrl = string.Empty;
        protected int makeId;
        protected uint cityId, dealerId;
        protected ushort totalDealers;
        protected Repeater rptMakes, rptCities, rptDealers;
        protected bool areDealersPremium = false;
        protected DealerBikesEntity dealerDetails = null;
        protected DealerCard ctrlDealerCard;
        protected LeadCaptureControl ctrlLeadCapture;
        protected DealerDetailEntity dealerObj;
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
                if (dealerId > 0)
                {
                    GetDealerDetails(dealerId);
                    ctrlDealerCard.MakeId = Convert.ToUInt32(makeId);
                    ctrlDealerCard.makeName = makeName;
                    ctrlDealerCard.makeMaskingName = makeMaskingName;
                    ctrlDealerCard.CityId = cityId;
                    ctrlDealerCard.PQSourceId = (int)PQSourceEnum.Desktop_dealer_details_Get_offers;
                    ctrlDealerCard.LeadSourceId = 38;
                    ctrlDealerCard.TopCount = Convert.ToUInt16(cityId > 0 ? 3 : 6);
                    ctrlDealerCard.pageName = "DealerDetail_Page_Desktop";
                    ctrlDealerCard.DealerId = (uint)dealerId;
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
        /// Created by: Aditi Srivastava on 27 Sep 2016
        /// Summary: Get dealer details by dealer id and make id
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
                    dealerDetails = objCache.GetDealerDetailsAndBikesByDealerAndMake(dealerId, makeId);

                    if (dealerDetails != null && dealerDetails.DealerDetails != null)
                    {
                        dealerObj = dealerDetails.DealerDetails;
                        dealerName = dealerObj.Name;
                        dealerMaskingName = UrlFormatter.RemoveSpecialCharUrl(dealerName);
                        cityName = dealerObj.City;
                        if (dealerObj.Area != null)
                            areaName = dealerObj.Area.AreaName;
                        address = dealerObj.Address;
                        maskingNumber = dealerObj.MaskingNumber;
                        eMail = dealerObj.EMail;
                        workingHours = dealerObj.WorkingHours;
                        makeName = dealerObj.MakeName;
                        cityMaskingName = dealerObj.CityMaskingName;
                        cityId = (uint)dealerObj.CityId;
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
                ErrorClass objErr = new ErrorClass(ex, "GetDealerDetails");
                objErr.SendMail();
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
                        if (!String.IsNullOrEmpty(makeMaskingName))
                        {

                            using (IUnityContainer container = new UnityContainer())
                            {
                                container.RegisterType<IBikeMakesCacheRepository<int>, BikeMakesCacheRepository<BikeMakeEntity, int>>()
                                      .RegisterType<ICacheManager, MemcacheManager>()
                                      .RegisterType<IBikeMakes<BikeMakeEntity, int>, BikeMakesRepository<BikeMakeEntity, int>>()
                                     ;
                                var objCache = container.Resolve<IBikeMakesCacheRepository<int>>();

                                objResponse = objCache.GetMakeMaskingResponse(makeMaskingName);
                            }
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