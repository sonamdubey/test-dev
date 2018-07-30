using Bikewale.BAL.BikeBooking;
using Bikewale.Common;
using Bikewale.Controls;
using Bikewale.Entities.BikeBooking;
using Bikewale.Entities.PriceQuote;
using Bikewale.Interfaces.BikeBooking;
using Bikewale.Utility;
using Microsoft.Practices.Unity;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace Bikewale.Pricequote
{
    /// <summary>
    /// Author  : Sushil Kumar
    /// Created On : 3rd December 2015
    /// Description : Booking configurator codebehind
    /// Modified by :   Sumit Kate on 19 Jan 2016
    /// Description :   Added Users Testimonial Control
    /// </summary>
    public class BookingConfig : System.Web.UI.Page
    {
        protected uint dealerId = 0, versionId = 0, cityId = 0, pqId = 0, areaId = 0, versionPrice = 0, bookingAmount = 0, insuranceAmount = 0;
        protected string clientIP = String.Empty, pageUrl = String.Empty, bikeName = String.Empty, location = String.Empty, makeUrl = String.Empty, modelUrl = String.Empty, jsonBikeVarients = String.Empty;
        protected Repeater rptVarients = null, rptVersionColors = null, rptDealerOffers = null, rptPriceBreakup = null;
        protected bool isOfferAvailable = false, isInsuranceFree = false;
        protected string versionWaitingPeriod = String.Empty, dealerAddress = String.Empty, latitude = "0", longitude = "0";
        protected BookingPageDetailsEntity objBookingPageDetails = null;
        protected PQCustomerDetail objCustomer = null;
        protected PQ_DealerDetailEntity dealerDetailEntity = null;
        protected UsersTestimonials ctrlUsersTestimonials;


        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            // Modified By :Lucky Rathore on 12 July 2016.
            Form.Action = Request.RawUrl;
            //device detection
            // Modified By :Ashish Kamble on 5 Feb 2016
            string originalUrl = Request.ServerVariables["HTTP_X_ORIGINAL_URL"];
            if (String.IsNullOrEmpty(originalUrl))
                originalUrl = Request.ServerVariables["URL"];

            DeviceDetection dd = new DeviceDetection(originalUrl);
            dd.DetectDevice();

            if (!IsPostBack)
            {
                ProcessCookie();
                BindBookingDetails();
                ctrlUsersTestimonials.TopCount = 6;
            }
        }

        #region Bind Bike Booking Details
        /// <summary>
        /// Author  : Sushil Kumar 
        /// Created On : 8th December 2015
        /// Summary : Get version details and quotation details
        ///           Also get dealer details from autobiz
        /// </summary>
        private void BindBookingDetails()
        {
            bool _isContentFound = true;
            try
            {
                FetchBookingDetails();

                if (objBookingPageDetails != null)
                {
                    if (objBookingPageDetails.Varients != null)
                    {
                        BindVarientDetails();
                    }

                    GetDealerDetails();

                    if (String.IsNullOrEmpty(location))
                    {
                        CheckCityCookie();
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
                ErrorClass.LogError(err, Request.ServerVariables["URL"]);

            }
            finally
            {
                if (!_isContentFound)
                {
                    UrlRewrite.Return404();
                }
            }
        }

        #region Fetch Bikebooking details
        /// <summary>
        /// Author  : Sushil Kumar 
        /// Created On : 12th December 2015
        /// Summary : Fetch Bike Booking Details
        /// </summary>
        private void FetchBookingDetails()
        {
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IDealerPriceQuote, DealerPriceQuote>();
                    IDealerPriceQuote _objDealerPricequote = container.Resolve<IDealerPriceQuote>();

                    //bookingpagedetails fetch
                    objBookingPageDetails = _objDealerPricequote.FetchBookingPageDetails(cityId, versionId, dealerId);

                    //customer details
                    objCustomer = _objDealerPricequote.GetCustomerDetailsByPQId(pqId);

                    //set location details
                    if (objCustomer != null && objCustomer.objCustomerBase != null && objCustomer.objCustomerBase.cityDetails != null && !String.IsNullOrEmpty(objCustomer.objCustomerBase.cityDetails.CityName))
                    {
                        if (objCustomer.objCustomerBase.AreaDetails != null)
                        {
                            if (!String.IsNullOrEmpty(objCustomer.objCustomerBase.AreaDetails.AreaName))
                                location = String.Format("{0}, {1}", objCustomer.objCustomerBase.AreaDetails.AreaName, objCustomer.objCustomerBase.cityDetails.CityName);
                        }
                        else
                        {
                            location = objCustomer.objCustomerBase.cityDetails.CityName;
                        }
                    }

                }
            }
            catch (Exception err)
            {
                Trace.Warn(err.Message);
                ErrorClass.LogError(err, Request.ServerVariables["URL"]);

            }

        }
        #endregion

        #endregion

        #region Bind Dealer Details
        /// <summary>
        /// Author  : Sushil Kumar 
        /// Created On : 12th December 2015
        /// Summary : Fetch and bind dealer details
        /// </summary>
        private void GetDealerDetails()
        {
            //fetch dealer details
            FetchDealerDetails();

            if (dealerDetailEntity != null)
            {
                //set dealer's location longitude and latitude
                if (dealerDetailEntity.objDealer != null && dealerDetailEntity.objDealer.objArea != null)
                {

                    latitude = Convert.ToString(dealerDetailEntity.objDealer.objArea.Latitude);
                    longitude = Convert.ToString(dealerDetailEntity.objDealer.objArea.Longitude);


                }

                //Dealer Address
                if (dealerDetailEntity.objDealer != null && !String.IsNullOrEmpty(dealerDetailEntity.objDealer.Address))
                {
                    dealerAddress = String.Format("{0}<br/>{1},{2},{3}-{4},{5}.", dealerDetailEntity.objDealer.Organization, dealerDetailEntity.objDealer.Address, dealerDetailEntity.objDealer.objArea.AreaName, dealerDetailEntity.objDealer.objCity.CityName, dealerDetailEntity.objDealer.objArea.PinCode, dealerDetailEntity.objDealer.objState.StateName);
                }

                //bind offers provided by dealer
                if (dealerDetailEntity.objDealer != null && dealerDetailEntity.objOffers != null)
                {
                    BindDealerOffers();
                }

            }
            else
            {
                Response.Redirect("/pricequote/quotation.aspx?MPQ=" + EncodingDecodingHelper.EncodeTo64(PriceQuoteQueryString.QueryString), false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
                this.Page.Visible = false;
            }

        }


        #region Bind Dealer offers
        /// <summary>
        /// Author  : Sushil Kumar 
        /// Created On : 12th December 2015
        /// Summary : Bind offers available with dealer
        /// </summary>
        private void BindDealerOffers()
        {
            if (dealerDetailEntity.objOffers != null && dealerDetailEntity.objOffers.Count > 0)
            {
                isOfferAvailable = true;
                rptDealerOffers.DataSource = dealerDetailEntity.objOffers;
                rptDealerOffers.DataBind();

            }
        }
        #endregion

        #region Fetch Dealer details
        /// <summary>
        /// Author  : Sushil Kumar 
        /// Created On : 12th December 2015
        /// Summary : Fetch Dealer details
        /// </summary>
        private void FetchDealerDetails()
        {
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<Bikewale.Interfaces.AutoBiz.IDealers, Bikewale.DAL.AutoBiz.DealersRepository>();
                    Bikewale.Interfaces.AutoBiz.IDealers objDealer = container.Resolve<Bikewale.DAL.AutoBiz.DealersRepository>();
                    PQParameterEntity objParam = new PQParameterEntity();
                    objParam.CityId = cityId;
                    objParam.DealerId = dealerId;
                    objParam.VersionId = versionId; ;
                    dealerDetailEntity = objDealer.GetDealerDetailsPQ(objParam);
                }

                if (dealerDetailEntity != null)
                {
                    if (dealerDetailEntity.objQuotation != null)
                    {
                        foreach (var price in dealerDetailEntity.objQuotation.PriceList)
                        {
                            isInsuranceFree = Bikewale.Utility.DealerOfferHelper.HasFreeInsurance(dealerId.ToString(), "", price.CategoryName, price.Price, ref insuranceAmount);
                            if (isInsuranceFree)
                                break;
                        }
                        if (dealerDetailEntity.objOffers != null && dealerDetailEntity.objOffers.Count > 0)
                            dealerDetailEntity.objQuotation.discountedPriceList = OfferHelper.ReturnDiscountPriceList(dealerDetailEntity.objOffers, dealerDetailEntity.objQuotation.PriceList);
                    }
                }
            }
            catch (Exception err)
            {
                Trace.Warn(err.Message);
                ErrorClass.LogError(err, Request.ServerVariables["URL"]);

            }
        }
        #endregion

        #endregion

        #region Bind variants available with dealer
        /// <summary>
        /// Author  : Sushil Kumar 
        /// Created On : 8th December 2015
        /// Summary : Bind Varients available with dealer
        /// </summary>
        private void BindVarientDetails()
        {
            if (versionId > 0 && objBookingPageDetails != null && objBookingPageDetails.Varients != null && objBookingPageDetails.Varients.Count > 0)
            {
                rptVarients.DataSource = objBookingPageDetails.Varients;
                rptVarients.DataBind();
                jsonBikeVarients = EncodingDecodingHelper.EncodeTo64(JsonConvert.SerializeObject(objBookingPageDetails.Varients));

                if (objBookingPageDetails.Varients.FirstOrDefault().Make != null && objBookingPageDetails.Varients.FirstOrDefault().Model != null)
                {
                    makeUrl = String.Format("<a href='/{0}-bikes/' itemprop='url'><span itemprop='title'>{1}</span></a>", objBookingPageDetails.Varients.FirstOrDefault().Make.MaskingName, objBookingPageDetails.Varients.FirstOrDefault().Make.MakeName);
                    modelUrl = String.Format("<a href='/{0}-bikes/{1}/' itemprop='url'><span itemprop='title'>{2}</span></a>", objBookingPageDetails.Varients.FirstOrDefault().Make.MaskingName, objBookingPageDetails.Varients.FirstOrDefault().Model.MaskingName, objBookingPageDetails.Varients.FirstOrDefault().Model.ModelName);

                    bikeName = String.Format("{0} {1}", objBookingPageDetails.Varients.FirstOrDefault().Make.MakeName, objBookingPageDetails.Varients.FirstOrDefault().Model.ModelName);
                }

            }
            else
            {
                UrlRewrite.Return404();
            }
        }
        #endregion

        #region Private Method to process cookie
        /// <summary>
        /// Checks for the valid PQ Cookie
        /// </summary>
        private void ProcessCookie()
        {
            if (PriceQuoteQueryString.IsPQQueryStringExists())
            {
                if (UInt32.TryParse(PriceQuoteQueryString.PQId, out pqId) && UInt32.TryParse(PriceQuoteQueryString.DealerId, out dealerId) && UInt32.TryParse(PriceQuoteQueryString.VersionId, out versionId))
                {
                    cityId = Convert.ToUInt32(PriceQuoteQueryString.CityId);
                    areaId = Convert.ToUInt32(PriceQuoteQueryString.AreaId);

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

        #region Set User Location from cookie
        /// <summary>
        /// To set user location from the location cookie,if not obtained from customer object
        /// Modified By :   Sumit Kate on 23 Dec 2015
        /// Description :   UInt16 Parsing the city Id instead of city name which is throwing Exception
        /// </summary>
        private void CheckCityCookie()
        {
            try
            {
                var cookies = this.Context.Request.Cookies;
                if (cookies.AllKeys.Contains("location"))
                {
                    string cookieLocation = cookies["location"].Value.Replace('-', ' ');
                    if (!String.IsNullOrEmpty(cookieLocation) && cookieLocation.IndexOf('_') != -1)
                    {
                        string[] locArray = cookieLocation.Split('_');

                        if (locArray.Length > 3 && Convert.ToUInt16(locArray[0]) > 0)
                        {
                            location = String.Format("{0}, {1}", locArray[3], locArray[1]);
                        }
                        else
                        {
                            location = locArray[1];
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("{0} {1}", Request.ServerVariables["URL"], "CheckCityCookie"));

            }
        }
        #endregion
    }
}