﻿using Bikewale.Common;
using Bikewale.Entities.BikeBooking;
using Bikewale.Interfaces.BikeBooking;
using Bikewale.Mobile.PriceQuote;
using Bikewale.Utility;
using Carwale.BL.PaymentGateway;
using Carwale.DAL.PaymentGateway;
using Carwale.Entity.PaymentGateway;
using Carwale.Interfaces.PaymentGateway;
using Microsoft.Practices.Unity;
using System;
using System.Configuration;
using System.Web;
using System.Web.UI.WebControls;

namespace Bikewale.BikeBooking
{
    /// <summary>
    /// Written By : Ashwini todkar on 15 Dec 2014
    /// </summary>
    public class BookingSummary : System.Web.UI.Page
    {
        protected PQ_DealerDetailEntity _objPQ = null;
        protected Repeater rptQuote, rptOffers, rptDisclaimer;
        protected Button btnMakePayment;

        protected string BikeName = string.Empty, cityId = string.Empty, versionId = string.Empty, dealerId = string.Empty, organization = string.Empty, address = string.Empty, color = string.Empty, MakeModel = string.Empty;
        protected UInt32 TotalPrice = 0, BooingAmt = 0;
        protected uint PqId = 0;
        protected PQCustomerDetail objCustomer = null;
        //protected bool isMailSend = false, isSMSSend = false;
        protected uint numOfDays = 0;
        uint exShowroomCost = 0;
        protected UInt32 insuranceAmount = 0;
        protected bool IsInsuranceFree = false;

        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
            btnMakePayment.Click += new EventHandler(btnMakePayment_click);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //code for device detection added by Ashwini Todkar
            DeviceDetection dd = new DeviceDetection(Request.ServerVariables["HTTP_X_REWRITE_URL"].ToString());
            dd.DetectDevice();

            if (PriceQuoteQueryString.IsPQQueryStringExists())
            {
                PqId = Convert.ToUInt32(PriceQuoteQueryString.PQId);
                dealerId = !String.IsNullOrEmpty(PriceQuoteQueryString.DealerId) ? PriceQuoteQueryString.DealerId : "0";
                versionId = PriceQuoteQueryString.VersionId;
                cityId = PriceQuoteQueryString.CityId;

                if (PqId > 0 && Convert.ToUInt32(dealerId) > 0)
                {
                    getCustomerDetails();
                    GetDetailedQuote();
                }
                else
                {
                    Response.Redirect("/pricequote/", false);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                    this.Page.Visible = false;
                }
            }
            else
            {
                Response.Redirect("/pricequote/", false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
                this.Page.Visible = false;
            }

        }

        /// <summary>
        /// Written By : Ashwini Todkar on 15 Dec 2014
        /// Summary    : PopulateWhere to get dealer price quote, offers, facilities, contact details 
        /// </summary>
        private void GetDetailedQuote()
        {
            bool _isContentFound = true;
            try
            {
                string _apiUrl = "/api/Dealers/GetDealerDetailsPQ/?versionId=" + versionId + "&DealerId=" + dealerId + "&CityId=" + cityId;

                using (Utility.BWHttpClient objClient = new Utility.BWHttpClient())
                {
                    _objPQ = objClient.GetApiResponseSync<PQ_DealerDetailEntity>(Utility.APIHost.AB, Utility.BWConfiguration.Instance.APIRequestTypeJSON, _apiUrl, _objPQ);
                }

                if (_objPQ != null)
                {
                    BikeName = _objPQ.objQuotation.objMake.MakeName + " " + _objPQ.objQuotation.objModel.ModelName + " " + _objPQ.objQuotation.objVersion.VersionName;
                    MakeModel = _objPQ.objQuotation.objMake.MakeName + " " + _objPQ.objQuotation.objModel.ModelName;
                    if (_objPQ.objQuotation.PriceList != null && _objPQ.objQuotation.PriceList.Count > 0)
                    {
                        rptQuote.DataSource = _objPQ.objQuotation.PriceList;
                        rptQuote.DataBind();

                        bool isShowroomPriceAvail = false, isBasicAvail = false;

                        foreach (var item in _objPQ.objQuotation.PriceList)
                        {
                            //Check if Ex showroom price for a bike is available CategoryId = 3 (exshowrrom)
                            if (item.CategoryId == 3)
                            {
                                isShowroomPriceAvail = true;
                                exShowroomCost = item.Price;
                            }

                            //if Ex showroom price for a bike is not available  then set basic cost for bike price CategoryId = 1 (basic bike cost)
                            if (!isShowroomPriceAvail && item.CategoryId == 1)
                            {
                                exShowroomCost += item.Price;
                                isBasicAvail = true;
                            }

                            if (item.CategoryId == 2 && !isShowroomPriceAvail)
                                exShowroomCost += item.Price;

                            TotalPrice += item.Price;
                        }

                        if (isBasicAvail && isShowroomPriceAvail)
                            TotalPrice = TotalPrice - exShowroomCost;

                        BooingAmt = _objPQ.objBookingAmt.Amount;

                    }
                    foreach (var price in _objPQ.objQuotation.PriceList)
                    {
                        Bikewale.common.DealerOfferHelper.HasFreeInsurance(dealerId.ToString(), _objPQ.objQuotation.objModel.ModelId.ToString(), price.CategoryName, price.Price, ref insuranceAmount);
                    }
                    if (insuranceAmount > 0)
                    {
                        IsInsuranceFree = true;
                    }
                    if (_objPQ.objOffers != null && _objPQ.objOffers.Count > 0)
                    {

                        rptOffers.DataSource = _objPQ.objOffers;
                        rptOffers.DataBind();
                    }

                    if (_objPQ.objQuotation.Disclaimer != null && _objPQ.objQuotation.Disclaimer.Count > 0)
                    {
                        rptDisclaimer.DataSource = _objPQ.objQuotation.Disclaimer;
                        rptDisclaimer.DataBind();
                    }


                    if (_objPQ.objDealer != null)
                    {
                        organization = _objPQ.objDealer.Organization;
                        address = _objPQ.objDealer.objArea.AreaName + ", " + _objPQ.objDealer.objCity.CityName;

                        if (!String.IsNullOrEmpty(address) && !String.IsNullOrEmpty(_objPQ.objDealer.objArea.PinCode))
                        {
                            address += ", " + _objPQ.objDealer.objArea.PinCode;
                        }

                        address += ", " + _objPQ.objDealer.objState.StateName;
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
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
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

        // <summary>
        /// created By : Ashwini Todkar on 12 Dec 2014
        /// Summary : Get Customer Details 
        /// </summary>
        protected void getCustomerDetails()
        {
            using (IUnityContainer container = new UnityContainer())
            {
                container.RegisterType<IDealerPriceQuote, Bikewale.BAL.BikeBooking.DealerPriceQuote>();
                IDealerPriceQuote objDealer = container.Resolve<IDealerPriceQuote>();

                objCustomer = objDealer.GetCustomerDetails(Convert.ToUInt32(PriceQuoteQueryString.PQId));
                if (objCustomer.objColor != null)
                    color = objCustomer.objColor.ColorName;
            }
        }

        void btnMakePayment_click(object Sender, EventArgs e)
        {
            if (_objPQ.objBookingAmt != null && _objPQ.objBookingAmt.Amount > 0)
                BeginTransaction("3");
        }

        protected void BeginTransaction(string sourceType)
        {
            string transresp = string.Empty;

            if (objCustomer.objCustomerBase.CustomerId.ToString() != "" && objCustomer.objCustomerBase.CustomerId > 0)
            {
                Trace.Warn("Inside begin tarns" + objCustomer.objCustomerBase.CustomerId.ToString());
                var transaction = new TransactionDetails()
                {
                    CustomerID = objCustomer.objCustomerBase.CustomerId,
                    PackageId = (int)Carwale.Entity.Enum.BikeBooking.BikeBooking,
                    ConsumerType = 2,
                    Amount = _objPQ.objBookingAmt.Amount,
                    ClientIP = CommonOpn.GetClientIP(),
                    UserAgent = HttpContext.Current.Request.ServerVariables["HTTP_USER_AGENT"],
                    PGId = Convert.ToUInt64(PriceQuoteQueryString.VersionId),
                    CustomerName = objCustomer.objCustomerBase.CustomerName,
                    CustEmail = objCustomer.objCustomerBase.CustomerEmail,
                    CustMobile = objCustomer.objCustomerBase.CustomerMobile,
                    CustCity = objCustomer.objCustomerBase.cityDetails.CityName,
                    PlatformId = 1,  //Desktop
                    ApplicationId = 2, //Carwale
                    RequestToPGUrl = "http://" + HttpContext.Current.Request.ServerVariables["HTTP_HOST"].ToString() + "/bikebooking/RedirectToBillDesk.aspx",
                    ReturnUrl = "http://" + HttpContext.Current.Request.ServerVariables["HTTP_HOST"].ToString() + "/bikebooking/billdeskresponse.aspx?sourceId=1&"
                        + "MPQ=" + EncodingDecodingHelper.EncodeTo64(PriceQuoteQueryString.QueryString)
                };
                //PGCookie.PGAmount = transaction.Amount.ToString();
                PGCookie.PGCarId = transaction.PGId.ToString();

                //Modified By : Sadhana Upadhyay on 22 Jan 2016 
                //Added Logic to save Bike Booking Cookie 
                BikeBookingCookie.SaveBBCookie(PriceQuoteQueryString.CityId, PriceQuoteQueryString.PQId, PriceQuoteQueryString.AreaId,
                    PriceQuoteQueryString.VersionId, PriceQuoteQueryString.DealerId);

                IUnityContainer container = new UnityContainer();
                container.RegisterType<ITransaction, Transaction>()
                .RegisterType<ITransactionRepository, TransactionRepository>()
                .RegisterType<IPackageRepository, PackageRepository>()
                .RegisterType<ITransactionValidator, ValidateTransaction>();

                if (sourceType == "3")
                {
                    container.RegisterType<IPaymentGateway, BillDesk>();
                    transaction.SourceId = Convert.ToInt16(sourceType);
                }

                ITransaction begintrans = container.Resolve<ITransaction>();
                transresp = begintrans.BeginTransaction(transaction);
                Trace.Warn("transresp : " + transresp);

                if (transresp == "Transaction Failure" || transresp == "Invalid information!")
                {
                    HttpContext.Current.Response.Redirect("http://" + HttpContext.Current.Request.ServerVariables["HTTP_HOST"].ToString() + "/pricequote/bookingsummary.aspx?MPQ=" + EncodingDecodingHelper.EncodeTo64(PriceQuoteQueryString.QueryString));
                    Trace.Warn("fail");
                }
            }
            else
            {
                HttpContext.Current.Response.Redirect("http://" + HttpContext.Current.Request.ServerVariables["HTTP_HOST"].ToString() + "/pricequote/bookingsummary.aspx?MPQ=" + EncodingDecodingHelper.EncodeTo64(PriceQuoteQueryString.QueryString));
                Trace.Warn("fail");
            }
        }
    }
}