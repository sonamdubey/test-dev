using Bikewale.Common;
using Bikewale.Entities.BikeBooking;
using Bikewale.Interfaces.BikeBooking;
using Bikewale.Interfaces.PriceQuote;
using Bikewale.Service.TCAPI;
using Bikewale.Utility;
using Carwale.BL.PaymentGateway;
using Carwale.DAL.Classified.SellCar;
using Carwale.DAL.PaymentGateway;
using Carwale.Entity.Enum;
using Carwale.Interfaces.Classified.SellCar;
using Carwale.Interfaces.PaymentGateway;
using Microsoft.Practices.Unity;
using System;
using System.Configuration;
using System.Web;

namespace Bikewale.BikeBooking
{
    /// <summary>
    /// Modified By : Lucky Rathore on 11 May 2016.
    /// Summary : VersionName added and paramete to call BookingEmailToCustomer() updated.
    /// </summary>
    public class BillDeskResponse : System.Web.UI.Page
    {
        protected PQ_DealerDetailEntity _objPQ = null;
        protected PQCustomerDetail objCustomer = null;
        protected uint totalPrice = 0;
        protected UInt32 BookingAmt = 0;
        protected string contactNo = string.Empty, organization = string.Empty, address = string.Empty, bikeName = string.Empty,
                         MakeModel = string.Empty, bookingRefNum = string.Empty, WorkingTime = string.Empty, bikeColor = String.Empty, VersionName = string.Empty;
        protected UInt32 insuranceAmount = 0;
        protected bool IsInsuranceFree = false;
        protected uint versionId = 0, cityId = 0, areaId = 0, dealerId = 0, leadId = 0;
        protected string pqId = string.Empty;
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
            BikeBookingCookie.DeleteBBCookie();
        }

        #region CompleteTransaction Method
        /// <summary>
        /// Modified by :   Sumit Kate on 09 Dec 2016
        /// Description :   PG Transaction MySql Migration
        /// </summary>
        private void CompleteTransaction()
        {
            bool isUpdated = false;
            string encodedQueryString = string.Empty;
            try
            {
                using (IUnityContainer containerTran = new UnityContainer())
                {
                    containerTran.RegisterType<ITransaction, Transaction>()
                        .RegisterType<ISellCarRepository, SellCarRepository>()
                                .RegisterType<IPaymentGateway, BillDesk>()
                                .RegisterType<ITransactionRepository, TransactionRepository>()
                                .RegisterType<IPackageRepository, PackageRepository>()
                                .RegisterType<ITransactionValidator, ValidateTransaction>();

                    ITransaction completetransaction = containerTran.Resolve<ITransaction>();
                    bool transresp = completetransaction.CompleteBillDeskTransaction();
                    Trace.Warn("transresp : " + transresp);

                    containerTran.RegisterType<IDealerPriceQuote, Bikewale.BAL.BikeBooking.DealerPriceQuote>();
                    IDealerPriceQuote objDealer = containerTran.Resolve<IDealerPriceQuote>();


                    //If MPQ Doesn't exist then use Bikebooking Cookie to read Value
                    if (PriceQuoteQueryString.IsPQQueryStringExists())
                    {
                        pqId = PriceQuoteQueryString.PQId;
                        areaId = Convert.ToUInt32(PriceQuoteQueryString.AreaId);
                        cityId = Convert.ToUInt32(PriceQuoteQueryString.CityId);
                        dealerId = Convert.ToUInt32(PriceQuoteQueryString.DealerId);
                        versionId = Convert.ToUInt32(PriceQuoteQueryString.VersionId);
                        leadId = Convert.ToUInt32(PriceQuoteQueryString.LeadId);
                    }
                    else
                    {
                        if (BikeBookingCookie.IsBBCoockieExist())
                        {
                            pqId = BikeBookingCookie.PQId;
                            areaId = Convert.ToUInt32(BikeBookingCookie.AreaId);
                            cityId = Convert.ToUInt32(BikeBookingCookie.CityId);
                            dealerId = Convert.ToUInt32(BikeBookingCookie.DealerId);
                            versionId = Convert.ToUInt32(BikeBookingCookie.VersionId);
                            leadId = Convert.ToUInt32(PriceQuoteQueryString.LeadId);
                        }
                    }

                    if (Convert.ToInt16(PGCookie.PGRespCode) == Convert.ToInt16(BillDeskTransactionStatusCode.Successfull))
                    {

                        isUpdated = objDealer.UpdatePQTransactionalDetailByLeadId(leadId, Convert.ToUInt32(PGCookie.PGTransId),
                            true, ConfigurationManager.AppSettings["OfferUniqueTransaction"]);

                        SentSuccessNotification();

                        PushBikeBookingSuccess();
                    }
                    else
                    {
                        isUpdated = objDealer.UpdatePQTransactionalDetailByLeadId(Convert.ToUInt32(PriceQuoteQueryString.LeadId), Convert.ToUInt32(PGCookie.PGTransId),
                            false, ConfigurationManager.AppSettings["OfferUniqueTransaction"]);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.BikeBooking.BillDeskResponse.CompleteTransaction");
                
            }
            finally
            {
                try
                {
                    if (Request.QueryString["sourceid"] != null && Request.QueryString["sourceid"] != "")
                    {
                        using (IUnityContainer container = new UnityContainer())
                        {
                            IPriceQuote _objPriceQuote = null;
                            container.RegisterType<IPriceQuote, BAL.PriceQuote.PriceQuote>();
                            _objPriceQuote = container.Resolve<IPriceQuote>();

                            if (Convert.ToInt16(PGCookie.PGRespCode) == Convert.ToInt16(BillDeskTransactionStatusCode.Successfull))
                            {
                                _objPriceQuote.SaveBookingStateByLeadId(leadId, Entities.PriceQuote.PriceQuoteStates.SuccessfulPayment);
                            }
                            else
                            {
                                if (Convert.ToInt16(PGCookie.PGRespCode) == Convert.ToInt16(BillDeskTransactionStatusCode.InvalidAuthentication))
                                {
                                    _objPriceQuote.SaveBookingStateByLeadId(leadId, Entities.PriceQuote.PriceQuoteStates.PaymentAborted);
                                }
                                else
                                {
                                    _objPriceQuote.SaveBookingStateByLeadId(leadId, Entities.PriceQuote.PriceQuoteStates.FailurePayment);
                                }
                            }

                            // If MPQ Doesn't exist trigger error mail
                            if (!PriceQuoteQueryString.IsPQQueryStringExists())
                            {
                                Exception ex = new Exception();
                                ErrorClass.LogError(ex,
                                    "Bikewale.BikeBooking.BillDeskResponse.CompleteTransaction : PriceQuoteQueryString.IsPQQueryStringExists() = false" + Request.Url.PathAndQuery);
                                
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    ErrorClass.LogError(ex, "Bikewale.BikeBooking.BillDeskResponse.CompleteTransaction inner try catch");
                    
                }
                finally
                {
                    if (PriceQuoteQueryString.IsPQQueryStringExists())
                        encodedQueryString = EncodingDecodingHelper.EncodeTo64(PriceQuoteQueryString.QueryString);
                    else
                        encodedQueryString = PriceQuoteQueryString.FormBase64QueryString(BikeBookingCookie.CityId,
                            BikeBookingCookie.PQId, BikeBookingCookie.AreaId, BikeBookingCookie.VersionId, BikeBookingCookie.DealerId);

                    if (Request.QueryString["sourceid"].ToString() == "1")
                    {
                        if (Convert.ToInt16(PGCookie.PGRespCode) == Convert.ToInt16(BillDeskTransactionStatusCode.Successfull))
                        {
                            HttpContext.Current.Response.Redirect("/pricequote/paymentconfirmation.aspx?MPQ=" + encodedQueryString, false);
                        }
                        else
                        {
                            HttpContext.Current.Response.Redirect("/pricequote/paymentfailure.aspx?MPQ=" + encodedQueryString, false);
                        }
                    }
                    if (Request.QueryString["sourceid"].ToString() == "2")
                    {
                        if (Convert.ToInt16(PGCookie.PGRespCode) == Convert.ToInt16(BillDeskTransactionStatusCode.Successfull))
                        {
                            HttpContext.Current.Response.Redirect("/m/pricequote/paymentconfirmation.aspx?MPQ=" + encodedQueryString, false);
                        }
                        else
                        {
                            HttpContext.Current.Response.Redirect("/m/pricequote/paymentfailure.aspx?MPQ=" + encodedQueryString, false);
                        }
                    }
                }
            }

        }  //End of CompleteTransaction
        #endregion

        /// <summary>
        /// Author          :   Sumit Kate
        /// Created Date    :   21 Oct 2015
        /// Description     :   Sends the notification to Customer and Dealer 
        /// Modified By : Vivek Gupta on 11-5-2016
        /// Desc : versionName added in SendEmailSMSToDealerCustomer.BookingEmailToDealer
        /// Modified By : Lucky Rathore on 11 May 2016.
        /// Summary : paramete to call BookingEmailToCustomer() updated.
        /// Modified By: Aditi Srivastava on 14 Sep 2016
        /// Description: Changed Dealer Mobile no(masking no.) to phone no(mobile no.) for sending sms and email to customer
        /// </summary>
        private void SentSuccessNotification()
        {
            string imgPath = string.Empty;
            try
            {
                bookingRefNum = ConfigurationManager.AppSettings["OfferUniqueTransaction"] + Carwale.BL.PaymentGateway.PGCookie.PGTransId;
                GetDetailedQuote();
                getCustomerDetails();
                //send sms to customer
                Bikewale.Notifications.SendEmailSMSToDealerCustomer.BookingSMSToCustomer(objCustomer.objCustomerBase.CustomerMobile, objCustomer.objCustomerBase.CustomerName,
                    bikeName, _objPQ.objDealer.Organization, _objPQ.objDealer.PhoneNo, address, bookingRefNum, insuranceAmount);

                //send sms to dealer
                Bikewale.Notifications.SendEmailSMSToDealerCustomer.BookingSMSToDealer(objCustomer.objCustomerBase.CustomerMobile, objCustomer.objCustomerBase.CustomerName,
                    bikeName, _objPQ.objDealer.Organization, _objPQ.objDealer.MobileNo, _objPQ.objDealer.Address, bookingRefNum, BookingAmt, insuranceAmount);


                if (_objPQ.objQuotation != null && _objPQ.objQuotation.OriginalImagePath != null && _objPQ.objQuotation.HostUrl != null)
                {
                    imgPath = Bikewale.Utility.Image.GetPathToShowImages(_objPQ.objQuotation.OriginalImagePath, _objPQ.objQuotation.HostUrl, Bikewale.Utility.ImageSize._210x118);
                }

                //send email to customer
                Bikewale.Notifications.SendEmailSMSToDealerCustomer.BookingEmailToCustomer(objCustomer.objCustomerBase.CustomerEmail, objCustomer.objCustomerBase.CustomerName
                   , _objPQ.objQuotation.PriceList, _objPQ.objOffers, bookingRefNum, totalPrice, _objPQ.objBookingAmt.Amount, MakeModel, VersionName, bikeColor, imgPath,
               _objPQ.objDealer.Organization, address, _objPQ.objDealer.PhoneNo, _objPQ.objDealer.EmailId, _objPQ.objDealer.WorkingTime, _objPQ.objDealer.objArea.Latitude, _objPQ.objDealer.objArea.Longitude);

                //send email to dealer
                Bikewale.Notifications.SendEmailSMSToDealerCustomer.BookingEmailToDealer(_objPQ.objDealer.EmailId, ConfigurationManager.AppSettings["OfferClaimAlertEmail"],
                    objCustomer.objCustomerBase.CustomerName, objCustomer.objCustomerBase.CustomerMobile, objCustomer.objCustomerBase.AreaDetails.AreaName,
                    objCustomer.objCustomerBase.CustomerEmail, totalPrice, _objPQ.objBookingAmt.Amount, totalPrice - _objPQ.objBookingAmt.Amount,
                    _objPQ.objQuotation.PriceList, bookingRefNum, bikeName, bikeColor, _objPQ.objDealer.Organization, _objPQ.objOffers, imgPath, VersionName, insuranceAmount);
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.BikeBooking.BillDeskResponse.SentSuccessNotification");
                
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
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<Bikewale.Interfaces.AutoBiz.IDealers, Bikewale.DAL.AutoBiz.DealersRepository>();
                    Bikewale.Interfaces.AutoBiz.IDealers objDealer = container.Resolve<Bikewale.DAL.AutoBiz.DealersRepository>();
                    PQParameterEntity objParam = new PQParameterEntity();
                    objParam.CityId = cityId;
                    objParam.DealerId = dealerId;
                    objParam.VersionId = versionId;
                    _objPQ = objDealer.GetDealerDetailsPQ(objParam);
                }

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
                        VersionName = _objPQ.objQuotation.objVersion.VersionName;
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
                ErrorClass.LogError(err, "Bikewale.BikeBooking.BillDeskResponse.GetDetailedQuote");
                
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

                    objCustomer = objDealer.GetCustomerDetailsByLeadId(leadId);

                    if (objCustomer != null && objCustomer.objColor != null)
                    {
                        bikeColor = objCustomer.objColor.ColorName;
                    }

                    PushBikeLeadInAutoBiz(objCustomer);
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.BikeBooking.BillDeskResponse.getCustomerDetails");
                
            }
        }

        /// <summary>
        /// created By : Sumit Kate 21 Oct 2015
        /// Function used to Push Booking Request in AutoBiz
        /// </summary>
        private void PushBikeBookingSuccess()
        {
            uint bookingId = default(uint);
            BookingRequest request = null;

            try
            {
                request = new BookingRequest();
                request.BookingDate = DateTime.Now;
                request.BranchId = _objPQ.objDealer.DealerId;
                request.InquiryId = Convert.ToUInt32(objCustomer.AbInquiryId);
                request.PaymentAmount = BookingAmt;
                request.Price = totalPrice;

                string _apiUrl = "/webapi/booking/";

                using (Bikewale.Utility.BWHttpClient objClient = new BWHttpClient())
                {
                    bookingId = objClient.PostSync<BookingRequest, uint>(APIHost.AB, BWConfiguration.Instance.APIRequestTypeJSON, _apiUrl, request);
                }
            }
            catch (Exception err)
            {
                string data = string.Empty;

                if (objCustomer != null)
                {
                    data = "PQCustomerDetail object : " + Newtonsoft.Json.JsonConvert.SerializeObject(objCustomer);
                }
                else
                {
                    data = "PQCustomerDetail object : null ";
                }
                data += " : request data : " + Newtonsoft.Json.JsonConvert.SerializeObject(request) + " : bookingId : " + bookingId;
                if (HttpContext.Current.Request.QueryString != null && HttpContext.Current.Request.QueryString.HasKeys() && (!String.IsNullOrEmpty(HttpContext.Current.Request.QueryString["MPQ"])))
                {
                    if (!String.IsNullOrEmpty(HttpContext.Current.Request.QueryString["MPQ"]))
                    {
                        data += " MPQ : Decode :" + EncodingDecodingHelper.DecodeFrom64(HttpContext.Current.Request.QueryString["MPQ"]) + " Encode : " + HttpContext.Current.Request.QueryString["MPQ"];
                    }
                    else
                    {
                        data += " MPQ : is present as qs param but is null or empty";
                    }
                }
                else
                {
                    data += " MPQ : is not present as qs param";
                }


                ErrorClass.LogError(err, "Bikewale.BikeBooking.BillDeskResponse.PushBikeBookingSuccess");
                
            }
        }


        private void PushBikeLeadInAutoBiz(PQCustomerDetail customerDetails)
        {
            string abInquiryId = string.Empty;
            try
            {
                abInquiryId = AutoBizAdaptor.PushInquiryInABV2(dealerId.ToString(), leadId, customerDetails.objCustomerBase.CustomerName, customerDetails.objCustomerBase.CustomerMobile, customerDetails.objCustomerBase.CustomerEmail, versionId, cityId.ToString());
                objCustomer.AbInquiryId = abInquiryId;
            }
            catch (Exception err)
            {
                string data = string.Empty;

                if (customerDetails != null)
                {
                    data = "Customer details object : " + Newtonsoft.Json.JsonConvert.SerializeObject(customerDetails);
                }
                else
                {
                    data = "Customer details object : null ";
                }

                if (HttpContext.Current.Request.QueryString != null && HttpContext.Current.Request.QueryString.HasKeys() && (!String.IsNullOrEmpty(HttpContext.Current.Request.QueryString["MPQ"])))
                {
                    if (!String.IsNullOrEmpty(HttpContext.Current.Request.QueryString["MPQ"]))
                    {
                        data += " MPQ : Decode :" + EncodingDecodingHelper.DecodeFrom64(HttpContext.Current.Request.QueryString["MPQ"]) + " Encode : " + HttpContext.Current.Request.QueryString["MPQ"];
                    }
                    else
                    {
                        data += " MPQ : is present as qs param but is null or empty";
                    }
                }
                else
                {
                    data += " MPQ : is not present as qs param";
                }

                data += " : abinquiry Id : " + abInquiryId;

                ErrorClass.LogError(err, "Bikewale.BikeBooking.BillDeskResponse.PushBikeLeadInAutoBiz + data : " + data);
                
            }
        }
    }   //End of class
}   //End of namespace
