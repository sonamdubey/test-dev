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
        protected uint dealerId = 0, versionId = 0, cityId = 0, pqId = 0, areaId = 0, versionPrice = 0, bookingAmount = 0,InsuranceAmount = 0;
        protected string clientIP = String.Empty, pageUrl = String.Empty, bikeName = String.Empty, location = String.Empty;
        protected BookingSummaryBase objBookingConfig = null;
        protected Repeater rptVarients = null, rptVersionColors = null, rptDealerOffers = null, rptPriceBreakup = null;
        protected BikeDealerPriceDetailDTO selectedVarient = null;
        protected HiddenField selectedVersionId = null;
        protected DDQDealerDetailBase DealerDetails = null;
        protected bool isOfferAvailable = false;
        protected string versionWaitingPeriod = String.Empty, dealerAddress = String.Empty, latitude = "0", longitude = "0";
        protected bool IsInsuranceFree = false;


        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //device detection
            DeviceDetection dd = new DeviceDetection(Request.ServerVariables["HTTP_X_REWRITE_URL"].ToString());
            dd.DetectDevice();

            //btn.Click += new EventHandler(btnSave_Click);
            //btnUpdateVersionColor.Click += new EventHandler(btnUpdateVersionColor_Click);

            if (!IsPostBack)
            {
                ProcessCookie();
                GetVersionNQuotationDetails();
            }
            //else
            //{

            //}

        }

        //protected void btnVariant_Command(object sender, CommandEventArgs e)
        //{
        //    if (!string.IsNullOrEmpty(e.CommandName))
        //    {
        //        versionId = Convert.ToUInt32(e.CommandArgument);
        //        loadViewStatedata();
        //    }
        //    return;
        //}

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

                if (objBookingConfig != null && objBookingConfig.DealerQuotation != null && objBookingConfig.Customer != null)
                {
                    //if (objBookingConfig.DealerQuotation.objBookingAmt == null || (objBookingConfig.DealerQuotation.objBookingAmt != null && objBookingConfig.DealerQuotation.objBookingAmt.Amount == 0))
                    //{
                    //    HttpContext.Current.Response.Redirect("http://" + HttpContext.Current.Request.ServerVariables["HTTP_HOST"].ToString() + "/pricequote/detaileddealerquotation.aspx", false);
                    //    HttpContext.Current.ApplicationInstance.CompleteRequest();
                    //    this.Page.Visible = false;
                    //    return;
                    //}

                    bikeName = String.Format("{0} {1}", objBookingConfig.DealerQuotation.objQuotation.objMake.MakeName, objBookingConfig.DealerQuotation.objQuotation.objModel.ModelName);

                    if (objBookingConfig.Varients != null)
                    {
                        BindVarientDetails();
                    }

                    ////save booking details object in viewState
                    //string jsonModel = JsonConvert.SerializeObject(objBookingConfig);
                    //ViewState["BookingConfig"] = jsonModel;

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
                    if (DealerDetails.objDealer.objArea != null && !String.IsNullOrEmpty(DealerDetails.objDealer.objArea.AreaName))
                    {
                        location = String.Format("{0}, {1}", DealerDetails.objDealer.objArea.AreaName, DealerDetails.objDealer.objCity.CityName);
                    }
                    else
                    {
                        location = DealerDetails.objDealer.objCity.CityName;
                    }
                }

                //Dealer Address
                if (DealerDetails.objDealer != null && !String.IsNullOrEmpty(DealerDetails.objDealer.Address))
                {
                    dealerAddress = DealerDetails.objDealer.Address;
                }

                //bind offers provided by dealer
                if (DealerDetails.objDealer != null && DealerDetails.objOffers != null)
                {
                    if (DealerDetails.objOffers != null && DealerDetails.objOffers.Count > 0)
                    {
                        isOfferAvailable = true;
                        rptDealerOffers.DataSource = DealerDetails.objOffers;
                        rptDealerOffers.DataBind();

                        if (DealerDetails.objDealer.objArea != null)
                        {
                            latitude = Convert.ToString(DealerDetails.objDealer.objArea.Latitude);
                            longitude = Convert.ToString(DealerDetails.objDealer.objArea.Longitude);
                        }

                    }
                    else
                    {
                        isOfferAvailable = false;
                        //get cordinates for dealer map
                        if (DealerDetails.objDealer.objArea != null)
                        {
                            latitude = Convert.ToString(DealerDetails.objDealer.objArea.Latitude);
                            longitude = Convert.ToString(DealerDetails.objDealer.objArea.Longitude);
                        }

                    }

                    InsuranceAmount = DealerDetails.InsuranceAmount;
                    IsInsuranceFree = DealerDetails.IsInsuranceFree;


                }

            }
        }

        private void BindVarientDetails()
        {

            //if (!String.IsNullOrEmpty(selectedVersionId.Value) && Convert.ToUInt32(selectedVersionId.Value) > 0)
            //{
            //    versionId = Convert.ToUInt32(selectedVersionId.Value);
            //}

            if (versionId > 0 && objBookingConfig != null && objBookingConfig.Varients != null && objBookingConfig.Varients.Count > 0)
            {

                selectedVersionId.Value = Convert.ToString(versionId);

                rptVarients.DataSource = objBookingConfig.Varients;
                rptVarients.DataBind();

                foreach (var varient in objBookingConfig.Varients)
                {
                    if (varient.MinSpec.VersionId == versionId)
                    {
                        selectedVarient = varient;

                        //#region Selected Version Waiting Period
                        ////get version waiting period
                        //if (varient.NoOfWaitingDays > 0)
                        //{
                        //    versionWaitingPeriod = Convert.ToString(varient.NoOfWaitingDays) + " days";
                        //}
                        //else
                        //{
                        //    versionWaitingPeriod = "0";
                        //}
                        //#endregion

                        //#region Selected Version Price
                        ////get version price
                        //if (selectedVarient.MinSpec.Price > 0)
                        //{
                        //    versionPrice = Convert.ToUInt32(selectedVarient.MinSpec.Price);
                        //}
                        //#endregion

                        //#region Selected Version Colors
                        ////get version colors
                        //if (varient.BikeModelColors != null)
                        //{
                        //    rptVersionColors.DataSource = varient.BikeModelColors;
                        //    rptVersionColors.DataBind();
                        //}
                        //#endregion

                        //#region Selected Varient Booking Amount
                        ////selected varient booking amount
                        //if (selectedVarient.BookingAmount > 0)
                        //{
                        //    bookingAmount = selectedVarient.BookingAmount;
                        //}
                        //#endregion


                        //#region Selected Version PriceBreakup
                        ////get version colors
                        //if (varient.PriceList != null && varient.PriceList.Count > 0)
                        //{
                        //    rptPriceBreakup.DataSource = varient.PriceList;
                        //    rptPriceBreakup.DataBind();
                        //}
                        //#endregion

                        break;
                    }
                }
            }
        }


        //protected void loadViewStatedata()
        //{

        //    //if (ViewState["BookingConfig"] != null)
        //    //{
        //    //    string json = (string)ViewState["BookingConfig"];
        //    //    objBookingConfig = JsonConvert.DeserializeObject<BookingSummaryBase>(json);
        //    //}

        //    //select versionId


        //    BindVarientDetails();
        //    GetDealerDetails();
        //}

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