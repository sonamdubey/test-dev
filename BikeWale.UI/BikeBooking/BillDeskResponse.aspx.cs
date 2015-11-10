using Carwale.BL.PaymentGateway;
using Carwale.DAL.PaymentGateway;
using Carwale.Entity.Enum;
using Carwale.Entity.PaymentGateway;
using Carwale.Interfaces.PaymentGateway;
using Bikewale.Utility;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net.Http;
using System.Net.Http.Headers;
using Bikewale.Notifications;
using Bikewale.Interfaces.BikeBooking;
using Bikewale.Mobile.PriceQuote;
using Bikewale.Entities.BikeBooking;

namespace Bikewale.BikeBooking
{
    public class BillDeskResponse : System.Web.UI.Page
    {
        protected PQ_DealerDetailEntity _objPQ = null;
        protected PQCustomerDetail objCustomer = null;
        protected uint totalPrice = 0;
        protected UInt32 BookingAmt = 0;
        protected string contactNo = string.Empty, organization = string.Empty, address = string.Empty, bikeName = string.Empty, MakeModel = string.Empty, bookingRefNum = string.Empty, WorkingTime = string.Empty, bikeColor = String.Empty;
        protected UInt32 insuranceAmount = 0;
        protected bool IsInsuranceFree = false;
        protected void Page_Load(object sender, EventArgs e)
        {
            //if (Request.Cookies["PGTransId"] != null)
            //{
            //    HttpContext.Current.Request.Cookies.Remove("PGTransId");
            //}
            if (!IsPostBack)
            {
                if (Request.Form.Count > 0)
                {
                    CompleteTransaction();
                }
            }

        }

        #region CompleteTransaction Method
        private void CompleteTransaction()
        {
            bool isUpdated = false;
            try
            {
                using (IUnityContainer containerTran = new UnityContainer())
                {
                    containerTran.RegisterType<ITransaction, Transaction>()
                                .RegisterType<IPaymentGateway, BillDesk>()
                                .RegisterType<ITransactionRepository, TransactionRepository>()
                                .RegisterType<IPackageRepository, PackageRepository>()
                                .RegisterType<ITransactionValidator, ValidateTransaction>();

                    ITransaction completetransaction = containerTran.Resolve<ITransaction>();
                    bool transresp = completetransaction.CompleteBillDeskTransaction();
                    Trace.Warn("transresp : " + transresp);


                    containerTran.RegisterType<IDealerPriceQuote, Bikewale.BAL.BikeBooking.DealerPriceQuote>();
                    IDealerPriceQuote objDealer = containerTran.Resolve<IDealerPriceQuote>();
                    
                    if (Convert.ToInt16(PGCookie.PGRespCode) == Convert.ToInt16(BillDeskTransactionStatusCode.Successfull))
                    {
                        isUpdated = objDealer.UpdatePQTransactionalDetail(Convert.ToUInt32(PriceQuoteCookie.PQId), Convert.ToUInt32(PGCookie.PGTransId), true, ConfigurationManager.AppSettings["OfferUniqueTransaction"]);
                        SentSuccessNotification();
                        PushBikeBookingSuccess();
                    }
                    else
                    {
                        isUpdated = objDealer.UpdatePQTransactionalDetail(Convert.ToUInt32(PriceQuoteCookie.PQId), Convert.ToUInt32(PGCookie.PGTransId), false, ConfigurationManager.AppSettings["OfferUniqueTransaction"]);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Bikewale.BikeBooking.BillDeskResponse.CompleteTransaction");
                objErr.SendMail();
            }
            finally
            {
                if (Request.QueryString["sourceid"] != null && Request.QueryString["sourceid"] != "")
                {
                    if (Request.QueryString["sourceid"].ToString() == "1")
                    {
                        if (Convert.ToInt16(PGCookie.PGRespCode) == Convert.ToInt16(BillDeskTransactionStatusCode.Successfull))
                        {
                            HttpContext.Current.Response.Redirect("/pricequote/paymentconfirmation.aspx", false);
                        }
                        else
                        {
                            HttpContext.Current.Response.Redirect("/pricequote/paymentfailure.aspx", false);
                        }
                    }
                    if (Request.QueryString["sourceid"].ToString() == "2")
                    {
                        if (Convert.ToInt16(PGCookie.PGRespCode) == Convert.ToInt16(BillDeskTransactionStatusCode.Successfull))
                        {
                            HttpContext.Current.Response.Redirect("/m/pricequote/paymentconfirmation.aspx", false);
                        }
                        else
                        {
                            HttpContext.Current.Response.Redirect("/m/pricequote/paymentfailure.aspx", false);
                        }
                    }
                }
            }
        }   //End of CompleteTransaction
        #endregion

        /// <summary>
        /// Author          :   Sumit Kate
        /// Created Date    :   21 Oct 2015
        /// Description     :   Sends the notification to Customer and Dealer
        /// </summary>
        private void SentSuccessNotification()
        {
            try
            {
                bookingRefNum = ConfigurationManager.AppSettings["OfferUniqueTransaction"] + Carwale.BL.PaymentGateway.PGCookie.PGTransId;
                GetDetailedQuote();
                getCustomerDetails();

                Bikewale.Notifications.SendEmailSMSToDealerCustomer.BookingSMSToCustomer(objCustomer.objCustomerBase.CustomerMobile, objCustomer.objCustomerBase.CustomerName, bikeName, _objPQ.objDealer.Name, _objPQ.objDealer.MobileNo, address, bookingRefNum, insuranceAmount);
                //send sms to dealer
                Bikewale.Notifications.SendEmailSMSToDealerCustomer.BookingSMSToDealer(objCustomer.objCustomerBase.CustomerMobile, objCustomer.objCustomerBase.CustomerName, bikeName, _objPQ.objDealer.Name, _objPQ.objDealer.MobileNo, _objPQ.objDealer.Address, bookingRefNum, BookingAmt, insuranceAmount);
                //send email to customer
                Bikewale.Notifications.SendEmailSMSToDealerCustomer.BookingEmailToCustomer(objCustomer.objCustomerBase.CustomerEmail, objCustomer.objCustomerBase.CustomerName, _objPQ.objOffers, bookingRefNum, _objPQ.objBookingAmt.Amount, bikeName, _objPQ.objQuotation.objMake.MakeName, _objPQ.objQuotation.objModel.ModelName, _objPQ.objDealer.Organization, address, _objPQ.objDealer.MobileNo, insuranceAmount);
                //send email to dealer
                Bikewale.Notifications.SendEmailSMSToDealerCustomer.BookingEmailToDealer(_objPQ.objDealer.EmailId, ConfigurationManager.AppSettings["OfferClaimAlertEmail"], objCustomer.objCustomerBase.CustomerName, objCustomer.objCustomerBase.CustomerMobile, objCustomer.objCustomerBase.AreaDetails.AreaName, objCustomer.objCustomerBase.CustomerEmail, totalPrice, _objPQ.objBookingAmt.Amount, totalPrice - _objPQ.objBookingAmt.Amount, _objPQ.objQuotation.PriceList, bookingRefNum, bikeName, bikeColor, _objPQ.objDealer.Name, _objPQ.objOffers, insuranceAmount);
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Bikewale.BikeBooking.BillDeskResponse.SentSuccessNotification");
                objErr.SendMail();
            }
        }

        /// <summary>
        /// Created By : Sumit Kate on 21 Oct 2015
        /// Summary : To get dealer price break up and other details
        /// </summary>
        private void GetDetailedQuote()
        {
            try
            {
                //sets the base URI for HTTP requests
                string _abHostUrl = ConfigurationManager.AppSettings["ABApiHostUrl"];
                string _requestType = "application/json";

                string _apiUrl = "/api/Dealers/GetDealerDetailsPQ/?versionId=" + PriceQuoteCookie.VersionId + "&DealerId=" + PriceQuoteCookie.DealerId + "&CityId=" + PriceQuoteCookie.CityId;
                // Send HTTP GET requests 

                _objPQ = BWHttpClient.GetApiResponseSync<PQ_DealerDetailEntity>(_abHostUrl, _requestType, _apiUrl, _objPQ);

                if (_objPQ != null)
                {
                    if (_objPQ.objDealer != null)
                    {
                        contactNo = _objPQ.objDealer.PhoneNo + (!String.IsNullOrEmpty(_objPQ.objDealer.PhoneNo) && !String.IsNullOrEmpty(_objPQ.objDealer.MobileNo) ? ", " : "") + _objPQ.objDealer.MobileNo;
                        organization = _objPQ.objDealer.Organization;
                        address = _objPQ.objDealer.Address + ", " + _objPQ.objDealer.objArea.AreaName + ", " + _objPQ.objDealer.objCity.CityName;

                        if (!String.IsNullOrEmpty(address) && !String.IsNullOrEmpty(_objPQ.objDealer.objArea.PinCode))
                        {
                            address += ", " + _objPQ.objDealer.objArea.PinCode;
                        }

                        address += ", " + _objPQ.objDealer.objState.StateName;
                    }
                    if (_objPQ.objQuotation != null)
                    {
                        bikeName = _objPQ.objQuotation.objMake.MakeName + " " + _objPQ.objQuotation.objModel.ModelName + " " + _objPQ.objQuotation.objVersion.VersionName;
                        MakeModel = _objPQ.objQuotation.objMake.MakeName + " " + _objPQ.objQuotation.objModel.ModelName;

                        bool isShowroomPriceAvail = false, isBasicAvail = false;
                        uint exShowroomCost = 0;
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

                            totalPrice += item.Price;
                        }

                        foreach (var price in _objPQ.objQuotation.PriceList)
                        {
                            Bikewale.common.DealerOfferHelper.HasFreeInsurance(_objPQ.objDealer.DealerId.ToString(), _objPQ.objQuotation.objModel.ModelId.ToString(), price.CategoryName, price.Price, ref insuranceAmount);
                        }
                        if (insuranceAmount > 0)
                        {
                            IsInsuranceFree = true;
                        }

                        if (isBasicAvail && isShowroomPriceAvail)
                            totalPrice = totalPrice - exShowroomCost;

                        BookingAmt = _objPQ.objBookingAmt.Amount;
                    }
                }
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, "Bikewale.BikeBooking.BillDeskResponse.GetDetailedQuote");
                objErr.SendMail();
            }
        }

        /// <summary>
        /// created By : Sumit Kate on 21 Oct 2015
        /// Summary : Get Customer Details 
        /// </summary>
        protected void getCustomerDetails()
        {
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IDealerPriceQuote, Bikewale.BAL.BikeBooking.DealerPriceQuote>();
                    IDealerPriceQuote objDealer = container.Resolve<IDealerPriceQuote>();

                    objCustomer = objDealer.GetCustomerDetails(Convert.ToUInt32(PriceQuoteCookie.PQId));

                    if (objCustomer != null && objCustomer.objColor != null)
                    {
                        bikeColor = objCustomer.objColor.ColorName;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Bikewale.BikeBooking.BillDeskResponse.getCustomerDetails");
                objErr.SendMail();
            }
        }

        /// <summary>
        /// created By : Sumit Kate 21 Oct 2015
        /// Function used to Push Booking Request in AutoBiz
        /// </summary>
        private void PushBikeBookingSuccess()
        {
            try
            {
                BookingRequest request = new BookingRequest();
                request.BookingDate = DateTime.Now;
                request.BranchId = _objPQ.objDealer.DealerId;
                request.InquiryId = Convert.ToUInt32(objCustomer.AbInquiryId);
                request.PaymentAmount = BookingAmt;
                request.Price = totalPrice;
                string _apiHostUrl = ConfigurationManager.AppSettings["ABApiHostUrl"];
                string _requestType = "application/json";
                string _apiUrl = String.Format("/webapi/booking/");
                uint bookingId = default(uint);
                bookingId = Bikewale.Utility.BWHttpClient.PostSync<BookingRequest, uint>(_apiHostUrl, _requestType, _apiUrl, request);
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, "Bikewale.BikeBooking.BillDeskResponse.PushBikeBookingSuccess");
                objErr.SendMail();
            }
        }

    }   //End of class
}   //End of namespace
