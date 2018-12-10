using AutoMapper;
using Carwale.BL.PaymentGateway;
using Carwale.DTOs;
using Carwale.DTOs.Deals;
using Carwale.Entity;
using Carwale.Entity.Dealers;
using Carwale.Entity.Deals;
using Carwale.Entity.Enum;
using Carwale.Entity.PaymentGateway;
using Carwale.Entity.Template;
using Carwale.Interfaces;
using Carwale.Interfaces.Deals;
using Carwale.Notifications;
using Carwale.Utility;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Carwale.Interfaces.Deals.Cache;
using log4net;
using Newtonsoft.Json;
using Carwale.Notifications.Logs;

namespace Carwale.BL.Deals
{
    public class DealInquiriesCL : IDealInquiriesCL
    {
        private readonly Carwale.Interfaces.PaymentGateway.ITransaction _transaction;
        private readonly IRepository<DealsInquiryDetail> _dealsRepo;
        private readonly IDeals _carDeals;
        private readonly IDealsNotification _dealsNotification;
        private readonly IDealsRepository _dealsRepository;
        private readonly IDealInquiriesRepository _dealsInquiriesRepository;
        private readonly IDealsCache _dealsCache;

        public DealInquiriesCL(Carwale.Interfaces.PaymentGateway.ITransaction transaction, IRepository<DealsInquiryDetail> dealsRepo,
            IDeals carDeals, IDealsNotification dealsNotification, IDealsRepository dealsRepository, IDealInquiriesRepository dealsInquiriesRepository,IDealsCache dealsCache)
        {
            _transaction = transaction;
            _dealsRepo = dealsRepo;
            _carDeals = carDeals;
            _dealsNotification = dealsNotification;
            _dealsRepository = dealsRepository;
            _dealsInquiriesRepository = dealsInquiriesRepository;
            _dealsCache = dealsCache;
        }

        public DealsStockDTO ThankYou(string respcd, string transId, bool paymentSuccess, string sourceIdForAutobiz)
        {
            Carwale.DTOs.Deals.DealsStockDTO dealsStockData = null;
            DealsStock dealsStock = null;
            try
            {
                var dealstock = HttpContext.Current.Request.Cookies["_dealStockId"];
                if (dealstock != null && dealstock.Value != string.Empty)
                {
                    int cityId = Convert.ToInt16(PGCookie.CustomerCity);
                    int stockId = Convert.ToInt32(dealstock.Value);
                    dealsStock = _dealsRepository.GetStockDetails(Convert.ToInt32(dealstock.Value), cityId);
                    dealsStock.StockId = stockId;
                    dealsStock.DealerDetails = _dealsCache.GetDealerDetails(dealsStock.DealerId);
                    dealsStockData = Mapper.Map<DealsStock, DealsStockDTO>(dealsStock);
                }
                if (dealsStock != null && dealsStock.StockCount > 0)
                {
                    if (paymentSuccess)
                    {
                        bool isLeasSubmitSuccess = _carDeals.AutobizPushPaidLead(paymentSuccess,sourceIdForAutobiz,dealsStock);
                    }
                    GatewayResponse pgResponse = new GatewayResponse { Name = PGCookie.CustomerName.Trim(), Email = PGCookie.CustomerEmail.Trim(), Mobile = PGCookie.CustomerMobile.Trim() };
                    SendNotification(transId, paymentSuccess, pgResponse, dealsStock);
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "DealInquiriesController.ThankYou()\n Exception : " + ex.Message);
                objErr.LogException();
            }
            return dealsStockData;
        }

        private void SendNotification(string transId, bool paymentSuccess, GatewayResponse pgResponse, DealsStock dealsStock)
        {
            TemplateContent templateContent = new TemplateContent()
            {
                MakeName = dealsStock.Make.MakeName,
                ModelName = dealsStock.Model.ModelName,
                VersionName = dealsStock.Version.VersionName,
                MailerName = dealsStock.DealerDetails.Name,
                EmailId = dealsStock.DealerDetails.Email,
                PhoneNumber = dealsStock.DealerDetails.Mobile,
                OfferExistance = dealsStock.Offers,
                TransactionId = transId,
                OnRoadPrice = Format.FormatNumericCommaSep(dealsStock.OnRoadPrice.ToString()),
                OfferPrice = Format.FormatNumericCommaSep(dealsStock.OfferPrice.ToString()),
                SavingPrice = Format.FormatNumericCommaSep(dealsStock.Savings.ToString()),
                ColourName = dealsStock.Color.ColorName,
                MakeYear = dealsStock.ManufacturingYear.ToString(),
                Amount = ConfigurationManager.AppSettings["AgedCarBlockingAmount"],
                CityName = dealsStock.City.CityName,
                ContactPerson = dealsStock.DealerDetails.ContactPerson,
                DealerAddress = dealsStock.DealerDetails.Address,
                DealerName = dealsStock.DealerDetails.Name,
                DealerMobile = dealsStock.DealerDetails.Mobile
            };
            _dealsNotification.SendDealsMailToDealer(templateContent, paymentSuccess,pgResponse);
            _dealsNotification.SendDealsMailToCustomer(templateContent, paymentSuccess, pgResponse);
        }


        public int PushLead(DealsInquiryDetailDTO dealsInquiry)
        {
            int inquiryId = 0;
            try
            {
                if (dealsInquiry != null)
                {
                    Logger.LogInfo(JsonConvert.SerializeObject(dealsInquiry));
                    dealsInquiry.CustomerCity = dealsInquiry.CityId;
                    DealsInquiryDetail dealsInquiryEntity = Mapper.Map<DealsInquiryDetailDTO, DealsInquiryDetail>(dealsInquiry);
                    inquiryId = _dealsRepo.Create(dealsInquiryEntity);

                    if ((dealsInquiry.PlatformId == (int)Platform.CarwaleDesktop || dealsInquiry.PlatformId == (int)Platform.CarwaleMobile) && inquiryId > 0)
                    {
                        HttpCookie objCookie;
                        objCookie = new HttpCookie("_dealStockId");
                        objCookie.Value = dealsInquiry.StockId.ToString();
                        objCookie.Domain = CustomerCookie.CookieDomain;
                        HttpContext.Current.Response.Cookies.Add(objCookie);
                        SetCookies(dealsInquiryEntity, inquiryId, dealsInquiry.PlatformId);
                        SetPDCookie();
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "DealInquiriesController.PushLead()\n Exception : " + ex.Message);
                objErr.LogException();
            }
            return inquiryId;
        }

        public bool PushMultipleLeads(DealsInquiryDetailDTO dealsInquiry)
        {
            bool isPushed = false;
            try
            {
                if (dealsInquiry != null)
                {
                    Logger.LogInfo(JsonConvert.SerializeObject(dealsInquiry));
                    dealsInquiry.CustomerCity = dealsInquiry.CityId;
                    DealsInquiryDetail dealsInquiryEntity = Mapper.Map<DealsInquiryDetailDTO, DealsInquiryDetail>(dealsInquiry);
                    isPushed = _dealsInquiriesRepository.PushMultipleLeads(dealsInquiryEntity);
                }
                SetPDCookie();
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "DealInquiriesController.PushLead()\n Exception : " + ex.Message);
                objErr.LogException();
            }
            return isPushed;
        }

        private void SetCookies(DealsInquiryDetail dealsInquiry, int inquiryId, int sourceId)
        {
            if (dealsInquiry.CustomerName != "unknown")
                PGCookie.CustomerName = dealsInquiry.CustomerName;
            else
                PGCookie.CustomerName = string.Empty;
            if (dealsInquiry.CustomerEmail != "unknown@unknown.com")
                PGCookie.CustomerEmail = dealsInquiry.CustomerEmail;
            else
                PGCookie.CustomerEmail = string.Empty;
            PGCookie.CustomerMobile = dealsInquiry.CustomerMobile;
            PGCookie.ResponseId = inquiryId.ToString();
            PGCookie.CustomerCity = dealsInquiry.CustomerCity.ToString();
            if (sourceId == Convert.ToInt32(Platform.CarwaleDesktop))
                PGCookie.PGResponseUrl = "/advantage/booking/confirmation";
            else
                PGCookie.PGResponseUrl = "/m/advantage/booking/confirmation";
        }

        public string BeginTransaction(DealsInquiryDetail dealsInquiry, int responseId, int sourceId)
        {
            string transresp = string.Empty;
            var blockingAmount = Convert.ToUInt16(ConfigurationManager.AppSettings["AgedCarBlockingAmount"].ToString());

            var transaction = new TransactionDetails()
            {
                CustomerID = Convert.ToUInt64(responseId),
                PackageId = (int)OfferPackage.AgedCarBooking,
                ConsumerType = 2, // consumerType is 1 for dealer and 0 for Indivisual
                Amount = blockingAmount,
                ClientIP = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"],
                UserAgent = HttpContext.Current.Request.ServerVariables["HTTP_USER_AGENT"], //HttpContext.Current.Request.ServerVariables["HTTP_USER_AGENT"],
                PGId = Convert.ToUInt64(responseId),
                CustomerName = PGCookie.CustomerName,
                CustEmail = PGCookie.CustomerEmail,
                CustMobile = PGCookie.CustomerMobile,
                CustCity = PGCookie.CustomerCity,
                PlatformId = sourceId, //Mobile website
                ApplicationId = 1, //Carwale
                RequestToPGUrl = "https://" + HttpContext.Current.Request.ServerVariables["HTTP_HOST"].ToString() + "/new/RedirectToBillDesk.aspx",
                ReturnUrl = "https://" + HttpContext.Current.Request.ServerVariables["HTTP_HOST"].ToString() + "/new/billdeskresponse.aspx?sourceId=1",
                SourceId = 3//BillDesk
            };
            PGCookie.PGAmount = transaction.Amount.ToString();
            PGCookie.PGCarId = transaction.PGId.ToString();
            PGCookie.PGPkgId = transaction.PackageId.ToString();
            PGCookie.PGRespCode = "";
            PGCookie.PGMessage = "";

            transresp = _transaction.BeginTransaction(transaction);

            return transresp;
        }

        public bool UpdateCustInfo(int inquiryId, DealsInquiryDetail dealsInquiry)
        {
            if (_dealsRepository.UpdateCustomerInfo(inquiryId, dealsInquiry))
            {
                PGCookie.CustomerName = dealsInquiry.CustomerName;
                PGCookie.CustomerMobile = dealsInquiry.CustomerMobile;
                PGCookie.CustomerEmail = dealsInquiry.CustomerEmail;
                return true;
            }
            else
                return false;
        }

        private void SetPDCookie()
        {
            HttpCookie isRedirectedFromPD = new HttpCookie("isRedirectedFromPD");
            isRedirectedFromPD.Value = "true";
            isRedirectedFromPD.Expires = DateTime.Now.AddMinutes(30);
            HttpContext.Current.Response.Cookies.Add(isRedirectedFromPD);
        }

        public void ProcessTransactionResults_App(GatewayResponse pgResponse)
        {
            try
            {
                int transactionId = 0;
                Int32.TryParse(pgResponse.PGTransId, out transactionId);
                DealerInquiryDetails dealerDetails = _dealsRepository.GetTransactionDetails(transactionId);
                DealsStock dealsStock = null;
                dealsStock = _dealsRepository.GetStockDetails((int)dealerDetails.DealsStockId, (int)dealerDetails.CityId);
                if (dealsStock != null)
                {
                    dealsStock.StockId = (int)dealerDetails.DealsStockId;
                    dealsStock.DealerDetails = _dealsCache.GetDealerDetails(dealsStock.DealerId);
                    SendNotification(pgResponse.PGTransId, pgResponse.IsTransactionCompleted, pgResponse, dealsStock);
                }
                if (pgResponse.IsTransactionCompleted)
                {
                    _carDeals.AutoBizDealsLeadProcessApp(dealerDetails, pgResponse.IsTransactionCompleted);
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "DealInquiriesController.ProcessTransactionResults_App()\n Exception : " + ex.Message);
                objErr.LogException();
            }
        }

        public TransactionDetails GetTransactionDetails(DealsInquiryDetailDTO dealsInquiry)
        {
            var blockingAmount = Convert.ToUInt16(ConfigurationManager.AppSettings["AgedCarBlockingAmount"].ToString());

            var transaction = new TransactionDetails()
            {
                CustomerID = Convert.ToUInt64(dealsInquiry.ResponseId),
                PackageId = (int)OfferPackage.AgedCarBooking,
                ConsumerType = 2, // consumerType is 1 for dealer and 0 for Indivisual
                Amount = blockingAmount,
                ClientIP = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"],
                UserAgent = HttpContext.Current.Request.ServerVariables["HTTP_USER_AGENT"], //HttpContext.Current.Request.ServerVariables["HTTP_USER_AGENT"],
                PGId = Convert.ToUInt64(dealsInquiry.ResponseId),
                CustomerName = dealsInquiry.CustomerName,
                CustEmail = dealsInquiry.CustomerEmail,
                CustMobile = dealsInquiry.CustomerMobile,
                CustCity = dealsInquiry.CustomerCity.ToString(),
                PlatformId = 74, //Mobile website
                ApplicationId = 1, //Carwale
                RequestToPGUrl = "https://" + HttpContext.Current.Request.ServerVariables["HTTP_HOST"].ToString() + "/new/RedirectToBillDesk.aspx",
                ReturnUrl = "https://" + HttpContext.Current.Request.ServerVariables["HTTP_HOST"].ToString() + "/billdesk/response.aspx",
                SourceId = 3//BillDesk
            };
            return transaction;
        }

        public string GetSDKMessage(DealsInquiryDetailDTO dealsInquiry, TransactionDetails transaction)
        {
            string message = "CARWALE|" + ConfigurationManager.AppSettings["OfferUniqueTransaction"].ToString() + transaction.PGRecordId + "|NA|" 
                + System.Configuration.ConfigurationManager.AppSettings["AgedCarBlockingAmount"] + "|NA|NA|NA|INR|NA|R|carwale|NA|NA|F|" + dealsInquiry.CustomerName
                + "|" + dealsInquiry.CustomerEmail + "|" + dealsInquiry.CustomerMobile + "|NA|NA|NA|NA|" + transaction.ReturnUrl;
            string hash = CarwaleSecurity.GetHMACSHA256(message, ConfigurationManager.AppSettings["BillDeskWorkingKey"].ToString()).ToUpper();
            message = message + "|" + hash;
            return message;
        }

        
    }
}
