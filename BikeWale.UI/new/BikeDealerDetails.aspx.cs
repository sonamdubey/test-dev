using Bikewale.Cache.BikeData;
using Bikewale.Cache.Core;
using Bikewale.Cache.DealersLocator;
using Bikewale.DAL.BikeData;
using Bikewale.DAL.Dealer;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.DealerLocator;
using Bikewale.Entities.Location;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.Dealer;
using Bikewale.Memcache;
using Bikewale.Notifications;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Bikewale.Utility;
using Bikewale.Controls;
using Bikewale.Entities.PriceQuote;

namespace Bikewale.New
{
    /// <summary>
    /// Created By : Aditi Srivastava on 26 Sep 2016
    /// Class to show the bike dealers details
    /// </summary>
    public class BikeDealerDetails : Page
    {
        protected string makeName = string.Empty, modelName = string.Empty, cityName = string.Empty, areaName = string.Empty, makeMaskingName = string.Empty, cityMaskingName = string.Empty, urlCityMaskingName = string.Empty;
        protected string address = string.Empty, maskingNumber = string.Empty, eMail = string.Empty, workingHours = string.Empty, modelImage = string.Empty, dealerName = string.Empty, dealerMaskingName = string.Empty;
        protected uint cityId, makeId,cost;
        protected ushort totalDealers;
        protected Repeater rptMakes, rptCities, rptDealers;
        protected string clientIP = string.Empty, pageUrl = string.Empty;
        protected bool areDealersPremium = false;
        protected int dealerId;
        protected DealerBikesEntity dealerDetails=null;
        protected DealerCard ctrlDealerCard;
        protected LeadCaptureControl ctrlLeadCapture;
        protected DealerDetailEntity DealerDetails;
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
                GetMakeIdByMakeMaskingName(makeMaskingName);

                if (dealerId> 0)
                {
                    GetDealerDetails(dealerId);
                    ctrlDealerCard.MakeId = Convert.ToUInt32(makeId);
                    ctrlDealerCard.makeName = makeName;
                    ctrlDealerCard.makeMaskingName = makeMaskingName;
                    ctrlDealerCard.CityId = cityId;
                    ctrlDealerCard.PQSourceId = (int)PQSourceEnum.Desktop_DealerLocator_Detail_GetOfferButton;
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
        /// </summary>
        /// <param name="dealerid"></param>
        private void GetDealerDetails(int dealerid)
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
                    dealerDetails = objCache.GetDealerDetailsAndBikes(dealerId);

                    if (dealerDetails != null )
                    {
                        DealerDetails = dealerDetails.DealerDetails;
                        dealerName = DealerDetails.Name;
                        dealerMaskingName = UrlFormatter.RemoveSpecialCharUrl(dealerName);
                        cityName = DealerDetails.City;
                        areaName = DealerDetails.Area.AreaName;
                        address = DealerDetails.Address;
                        maskingNumber = DealerDetails.MaskingNumber;
                        eMail = DealerDetails.EMail;
                        workingHours = DealerDetails.WorkingHours;
                        makeName=dealerDetails.Models.FirstOrDefault().MakeName;
                       // makeMaskingName = dealerDetails.Models.FirstOrDefault().MakeMaskingName;
                       //makeId = (uint)dealerDetails.Models.FirstOrDefault().MakeId;
                        cityMaskingName = DealerDetails.CityMaskingName;
                        cityId = (uint)DealerDetails.CityId;
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="maskingName"></param>
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
                Trace.Warn(ex.Message);
                ErrorClass objErr = new ErrorClass(ex, "GetMakeIdByMakeMaskingName");
                objErr.SendMail();
            }
        }


        #region Private Method to process querystring
        /// <summary>
        /// Created By : Aditi Srivastava
        /// Created On : 26th Sep 2016 
        /// Description : Private Method to validate query string based on dealer id
        /// </summary>
        private bool ProcessQueryString()
        {
            var currentReq = HttpContext.Current.Request;
            bool isValidQueryString = false;
            try
            {
                if (currentReq.QueryString != null && currentReq.QueryString.HasKeys())
                {
                      makeMaskingName = currentReq.QueryString["make"];

                      dealerId = Convert.ToInt32(currentReq.QueryString["dealerid"]);
                      if (dealerId > 0 && !string.IsNullOrEmpty(makeMaskingName))
                    {
                       
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
                ErrorClass objErr = new ErrorClass(ex, currentReq.ServerVariables["URL"]);
                objErr.SendMail();
            }
            return isValidQueryString;
        }
        
        #endregion
    }   // End of class
}   // End of namespace