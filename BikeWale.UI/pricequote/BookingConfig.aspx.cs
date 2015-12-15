using Bikewale.Common;
using Bikewale.DTO.BookingSummary;
using Bikewale.DTO.PriceQuote.BikeBooking;
using Bikewale.DTO.PriceQuote.DetailedDealerQuotation;
using Bikewale.Mobile.PriceQuote;
using Microsoft.Practices.Unity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Bikewale.Pricequote
{
    /// <summary>
    /// Author  : Sushil Kumar
    /// Created On : 3rd December 2015
    /// Description : Booking configurator codebehind
    /// </summary>
    public class BookingConfig : System.Web.UI.Page
    {
        protected uint dealerId = 0, versionId = 0, cityId = 0, pqId = 0, areaId = 0, versionPrice = 0, bookingAmount = 0,insuranceAmount = 0;
        protected string clientIP = String.Empty, pageUrl = String.Empty, bikeName = String.Empty, location = String.Empty;
        protected BookingSummaryBase objBookingConfig = null;
        protected Repeater rptVarients = null, rptVersionColors = null, rptDealerOffers = null, rptPriceBreakup = null;
        protected BikeDealerPriceDetailDTO selectedVarient = null;
        protected DDQDealerDetailBase DealerDetails = null;
        protected bool isOfferAvailable = false, isInsuranceFree = false;
        protected string versionWaitingPeriod = String.Empty, dealerAddress = String.Empty, latitude = "0", longitude = "0";


        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //device detection
            DeviceDetection dd = new DeviceDetection(Request.ServerVariables["HTTP_X_REWRITE_URL"].ToString());
            dd.DetectDevice();

            if (!IsPostBack)
            {
                ProcessCookie();
                GetVersionNQuotationDetails();
            }
        }

        private void GetVersionNQuotationDetails()
        {
            bool _isContentFound = true;
            try
            {
                //sets the base URI for HTTP requests
                string _abHostUrl = ConfigurationManager.AppSettings["bwHostUrl"];
                string _requestType = "application/json";

                string _apiUrl = String.Format("api/BookingSummary?pqId={0}&versionId={1}&dealerId={2}&cityId={3}", pqId, versionId, dealerId, cityId);
                // Send HTTP GET requests 

                objBookingConfig = BWHttpClient.GetApiResponseSync<BookingSummaryBase>(_abHostUrl, _requestType, _apiUrl, objBookingConfig);

                if (objBookingConfig != null && objBookingConfig.DealerQuotation != null && objBookingConfig.Varients != null)
                {
                    //if (objBookingConfig.DealerQuotation.objBookingAmt == null || (objBookingConfig.DealerQuotation.objBookingAmt != null && objBookingConfig.DealerQuotation.objBookingAmt.Amount < 1))
                    //{
                    //    HttpContext.Current.Response.Redirect("http://" + HttpContext.Current.Request.ServerVariables["HTTP_HOST"].ToString() + "/pricequote/detaileddealerquotation.aspx", false);
                    //    HttpContext.Current.ApplicationInstance.CompleteRequest();
                    //    this.Page.Visible = false;
                    //    return;
                    //}                     

                    if (objBookingConfig.Varients != null)
                    {
                        BindVarientDetails();
                    }

                    if (objBookingConfig.DealerQuotation != null)
                    {
                        GetDealerDetails();
                    }
                }
                else
                {
                    _isContentFound = false;
                }
            }
            catch (Exception err)
            {
                Trace.Warn(err.Message);
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            finally
            {
                if (!_isContentFound)
                {
                    Response.Redirect("/pagenotfound.aspx", false);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                    this.Page.Visible = false;
                }
            }
        }

        private void GetDealerDetails()
        {
            if (objBookingConfig != null && objBookingConfig.DealerQuotation != null)
            {
                DealerDetails = objBookingConfig.DealerQuotation;
                //location details
                if (DealerDetails.objDealer != null && DealerDetails.objDealer.objCity != null && !String.IsNullOrEmpty(DealerDetails.objDealer.objCity.CityName))
                {
                    if (DealerDetails.objDealer.objArea != null )
                    {
                        if (!String.IsNullOrEmpty(DealerDetails.objDealer.objArea.AreaName))
                            location = String.Format("{0}, {1}", DealerDetails.objDealer.objArea.AreaName, DealerDetails.objDealer.objCity.CityName);

                        latitude = Convert.ToString(DealerDetails.objDealer.objArea.Latitude);
                        longitude = Convert.ToString(DealerDetails.objDealer.objArea.Longitude);
                    }
                    else
                    {
                        location = DealerDetails.objDealer.objCity.CityName;
                    }
                }

                //Dealer Address
                if (DealerDetails.objDealer != null && !String.IsNullOrEmpty(DealerDetails.objDealer.Address))
                {
                    dealerAddress = String.Format("{0},{1},{2}-{3},{4}.", DealerDetails.objDealer.Address, DealerDetails.objDealer.objArea.AreaName, DealerDetails.objDealer.objCity.CityName, DealerDetails.objDealer.objArea.PinCode, DealerDetails.objDealer.objState.StateName);
                }

                //bind offers provided by dealer
                if (DealerDetails.objDealer != null && DealerDetails.objOffers != null)
                {
                    if (DealerDetails.objOffers != null && DealerDetails.objOffers.Count > 0)
                    {
                        isOfferAvailable = true;
                        rptDealerOffers.DataSource = DealerDetails.objOffers;
                        rptDealerOffers.DataBind();

                    }  
                    insuranceAmount = DealerDetails.InsuranceAmount;
                    isInsuranceFree = DealerDetails.IsInsuranceFree; 
                }

            }
        }

        private void BindVarientDetails()
        {
            if (versionId > 0 && objBookingConfig != null && objBookingConfig.Varients != null && objBookingConfig.Varients.Count > 0)
            {
                rptVarients.DataSource = objBookingConfig.Varients;
                rptVarients.DataBind();
            }
        }

        #region Private Method to process cookie
        /// <summary>
        /// Checks for the valid PQ Cookie
        /// </summary>
        private void ProcessCookie()
        {
            if (PriceQuoteCookie.IsPQCoockieExist())
            {
                if (UInt32.TryParse(PriceQuoteCookie.PQId, out pqId) && UInt32.TryParse(PriceQuoteCookie.DealerId, out dealerId) && UInt32.TryParse(PriceQuoteCookie.VersionId, out versionId))
                {
                    cityId = Convert.ToUInt32(PriceQuoteCookie.CityId);
                    areaId = Convert.ToUInt32(PriceQuoteCookie.AreaId);

                    if (dealerId > 0)
                    {
                        clientIP = Bikewale.Common.CommonOpn.GetClientIP();
                        pageUrl = Request.ServerVariables["URL"];
                    }
                    else
                    {
                        Response.Redirect("/", false);
                        HttpContext.Current.ApplicationInstance.CompleteRequest();
                        this.Page.Visible = false;
                    }
                }
                else
                {
                    Response.Redirect("/", false);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                    this.Page.Visible = false;
                }
            }
            else
            {
                Response.Redirect("/", false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
                this.Page.Visible = false;
            }
        }
        #endregion
    }
}