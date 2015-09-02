﻿using Bikewale.BAL.BikeData;
using Bikewale.Common;
using Bikewale.Entities.BikeBooking;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.Customer;
using Bikewale.Interfaces.BikeBooking;
using Bikewale.Interfaces.BikeData;
using Bikewale.Mobile.PriceQuote;
using Carwale.BL.PaymentGateway;
using Carwale.DAL.PaymentGateway;
using Carwale.Entity.Enum;
using Carwale.Entity.PaymentGateway;
using Carwale.Interfaces.PaymentGateway;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Bikewale.Mobile.BikeBooking
{
    /// <summary>
    /// Written By : Ashwini todkar on 15 Dec 2014
    /// </summary>
    public class BookingSummary : System.Web.UI.Page
    {
        protected PQ_DealerDetailEntity _objPQ = null;
        protected Repeater rptQuote, rptOffers, rptPrice, rptDisclaimer;
        protected Button btnMakePayment;
        protected string BikeName = string.Empty, cityId = string.Empty, versionId = string.Empty, dealerId = string.Empty, organization = string.Empty, address = string.Empty, MakeModel = string.Empty;
        protected UInt32 TotalPrice = 0, BooingAmt = 0;
        protected uint PqId = 0;
        protected PQCustomerDetail objCustomer = null;
        protected uint numOfDays = 0;
        uint exShowroomCost = 0;
        protected UInt32 insuranceAmount = 0;
        protected bool IsInsuranceFree = false;

        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
            this.btnMakePayment.Click += new EventHandler(ProcessPayment);
        }

        private void ProcessPayment(object sender, EventArgs e)
        {
            if (_objPQ != null && _objPQ.objBookingAmt.Amount > 0)
            {
                //Billdesk transaction type source = 3
                BeginTransaction("3");
            }
        }

        private void BeginTransaction(string sourceType)
        {
            string transresp = string.Empty;

            if (objCustomer.objCustomerBase.CustomerId.ToString() != "" && objCustomer.objCustomerBase.CustomerId > 0)
            {
                var transaction = new TransactionDetails()
                {
                    CustomerID = objCustomer.objCustomerBase.CustomerId,
                    PackageId = (int)Carwale.Entity.Enum.BikeBooking.BikeBooking,
                    ConsumerType = 2,
                    Amount = _objPQ.objBookingAmt.Amount,
                    ClientIP = CommonOpn.GetClientIP(),
                    UserAgent = HttpContext.Current.Request.ServerVariables["HTTP_USER_AGENT"],
                    PGId = Convert.ToUInt64(PriceQuoteCookie.VersionId),
                    CustomerName = objCustomer.objCustomerBase.CustomerName,
                    CustEmail = objCustomer.objCustomerBase.CustomerEmail,
                    CustMobile = objCustomer.objCustomerBase.CustomerMobile,
                    CustCity = objCustomer.objCustomerBase.cityDetails.CityName,
                    PlatformId = 2,  //Mobile
                    ApplicationId = 2, //bikewale

                    RequestToPGUrl = "http://" + HttpContext.Current.Request.ServerVariables["HTTP_HOST"].ToString() + "/bikebooking/RedirectToBillDesk.aspx",

                    //sourceid = 2 to redirect response on mobile site 
                    ReturnUrl = "http://" + HttpContext.Current.Request.ServerVariables["HTTP_HOST"].ToString() + "/bikebooking/billdeskresponse.aspx?sourceId=2"
                };
                //PGCookie.PGAmount = transaction.Amount.ToString();
                PGCookie.PGCarId = transaction.PGId.ToString();
                //PGCookie.PGPkgId = transaction.PackageId.ToString();
                //PGCookie.PGRespCode = "";
                //PGCookie.PGMessage = "";

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
                    HttpContext.Current.Response.Redirect("http://" + HttpContext.Current.Request.ServerVariables["HTTP_HOST"].ToString() + "/m/pricequote/bookingsummary.aspx");
                    Trace.Warn("fail");
                }
            }
            else
            {
                HttpContext.Current.Response.Redirect("http://" + HttpContext.Current.Request.ServerVariables["HTTP_HOST"].ToString() + "/m/pricequote/bookingsummary.aspx");
                Trace.Warn("fail");
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (PriceQuoteCookie.IsPQCoockieExist())
            {
                PqId = Convert.ToUInt32(PriceQuoteCookie.PQId);
                dealerId = !String.IsNullOrEmpty(PriceQuoteCookie.DealerId) ? PriceQuoteCookie.DealerId : "0";
                versionId = PriceQuoteCookie.VersionId;
                cityId = PriceQuoteCookie.CityId;

                if (PqId > 0 && Convert.ToUInt32(dealerId) > 0)
                {
                    getCustomerDetails();
                    GetDetailedQuote();
                }
                else
                {
                    Response.Redirect("/m/pricequote/", true);
                }
            }
            else
            {
                Response.Redirect("/m/pricequote/", true);
            }
        }

        /// <summary>
        /// Written By : Ashwini Todkar on 15 Dec 2014
        /// Summary    : Method to get dealer price quote, offers, facilities, contact details 
        /// </summary>
        private void GetDetailedQuote()
        {
            bool _isContentFound = true;
            try
            {
                //sets the base URI for HTTP requests
                string _abHostUrl = ConfigurationManager.AppSettings["ABApiHostUrl"];
                string _requestType = "application/json";

                string _apiUrl = "/api/Dealers/GetDealerDetailsPQ/?versionId=" + versionId + "&DealerId=" + dealerId + "&CityId=" + cityId;
                // Send HTTP GET requests 

                _objPQ = BWHttpClient.GetApiResponseSync<PQ_DealerDetailEntity>(_abHostUrl, _requestType, _apiUrl, _objPQ);

                if (_objPQ != null)
                {
                    BikeName = _objPQ.objQuotation.objMake.MakeName + " " + _objPQ.objQuotation.objModel.ModelName + " " + _objPQ.objQuotation.objVersion.VersionName;
                    MakeModel = _objPQ.objQuotation.objMake.MakeName + " " + _objPQ.objQuotation.objModel.ModelName;
                    if (_objPQ.objQuotation.PriceList != null && _objPQ.objQuotation.PriceList.Count > 0)
                    {
                        bool isShowroomPriceAvail = false, isBasicAvail = false;

                        rptPrice.DataSource = _objPQ.objQuotation.PriceList;
                        rptPrice.DataBind();

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

                        foreach (var price in _objPQ.objQuotation.PriceList)
                        {
                            Bikewale.common.DealerOfferHelper.HasFreeInsurance(dealerId.ToString(), _objPQ.objQuotation.objModel.ModelId.ToString(), price.CategoryName, price.Price, ref insuranceAmount);
                        }
                        if (insuranceAmount > 0)
                        {
                            IsInsuranceFree = true;
                        }

                        if (isBasicAvail && isShowroomPriceAvail)
                            TotalPrice = TotalPrice - exShowroomCost;

                        BooingAmt = _objPQ.objBookingAmt.Amount;
                    }

                    if (_objPQ.objOffers != null && _objPQ.objOffers.Count > 0)
                    {

                        rptOffers.DataSource = _objPQ.objOffers;
                        rptOffers.DataBind();
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

                    if (_objPQ.objQuotation.Disclaimer != null && _objPQ.objQuotation.Disclaimer.Count > 0)
                    {
                        rptDisclaimer.DataSource = _objPQ.objQuotation.Disclaimer;
                        rptDisclaimer.DataBind();
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
                    Response.Redirect("/m/pagenotfound.aspx", true);
            }
        }

        // <summary>
        /// created By : Ashwini Todkar on 15 Dec 2014
        /// Summary : Get Customer Details 
        /// </summary>
        protected void getCustomerDetails()
        {
            using (IUnityContainer container = new UnityContainer())
            {
                container.RegisterType<IDealerPriceQuote, Bikewale.BAL.BikeBooking.DealerPriceQuote>();
                IDealerPriceQuote objDealer = container.Resolve<IDealerPriceQuote>();

                objCustomer = objDealer.GetCustomerDetails(Convert.ToUInt32(PriceQuoteCookie.PQId));
            }
        }
    }
}