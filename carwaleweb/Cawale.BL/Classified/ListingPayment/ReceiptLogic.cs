using Carwale.BL.Classified.UsedSellCar;
using Carwale.Entity.Classified.Leads;
using Carwale.Entity.Classified.ListingPayment;
using Carwale.Entity.Enum;
using Carwale.Entity.Notifications;
using Carwale.Interfaces.Classified;
using Carwale.Interfaces.Classified.Leads;
using Carwale.Interfaces.Classified.ListingPayment;
using Carwale.Interfaces.Template;
using Carwale.Notifications.Interface;
using Carwale.Notifications.Logs;
using Carwale.Utility;
using s3fileupload;
using System;
using System.Configuration;
using System.IO;

namespace Carwale.BL.Classified.ListingPayment
{
    public class ReceiptLogic: IReceiptLogic
    {
        private readonly IReceiptRepository _receiptRepository;
        private readonly ITemplateRender _templateRender;
        private readonly ISmsLogic _smsLogic;
        private readonly ISellerCacheRepository _sellerCacheRepository;
        private readonly IClassifiedMails _classifiedMails;
        private static readonly string _hostUrl = ConfigurationManager.AppSettings["HostUrl"];
        private static readonly string _transactionPrefix = ConfigurationManager.AppSettings["OfferUniqueTransaction"] + "SELL";
        private static readonly int _receiptSmsTemplateId = Convert.ToInt32(ConfigurationManager.AppSettings["IndividualListingReceiptSmsTemplateId"]);
        private const string _s3Bucket = "m-aeplimages";

        public ReceiptLogic(IReceiptRepository receiptRepository
            , ITemplateRender templateRender
            , ISellerCacheRepository sellerCacheRepository
            ,IClassifiedMails classifiedMails
            , ISmsLogic smsLogic)
        {
            _receiptRepository = receiptRepository;
            _templateRender = templateRender;
            _sellerCacheRepository = sellerCacheRepository;
            _classifiedMails = classifiedMails;
            _smsLogic = smsLogic;
        }
        public bool UploadPdf(Receipt receipt)
        {
            string s3Key = "";
            bool isUploaded = false;
            if (receipt != null)
            {
                Uri receiptUrl = new Uri(string.Format("https://{0}/used/listings/{1}/receipt/", _hostUrl, receipt.InquiryId));
                byte[] pdfData = PdfProcessor.GeneratePdfBytesFromUrl(receiptUrl);
                if (pdfData != null)
                {
                    UploadFile s3Upload = new UploadFile();
                    Stream objFileStream = new MemoryStream(pdfData);
                    if (_hostUrl.Equals("www.carwale.com"))
                    {
                        s3Key = string.Format("classifiedlistings/paymentreceipt/{0}_{1:ddMMyyHmmss}.pdf", receipt.InquiryId, DateTime.Now);
                    }
                    else if (_hostUrl.Equals("localhost"))
                    {
                        s3Key = string.Format("classifiedlistings/dev/paymentreceipt/{0}_{1:ddMMyyHmmss}.pdf", receipt.InquiryId, DateTime.Now);
                    }
                    else
                    {
                        s3Key = string.Format("classifiedlistings/staging/paymentreceipt/{0}_{1:ddMMyyHmmss}.pdf", receipt.InquiryId, DateTime.Now);
                    }
                    s3Upload.Bucket = _s3Bucket;
                    s3Upload.AwsAccessKey = ConfigurationManager.AppSettings["awsAccessKeyId"];
                    s3Upload.AwsSecretKey = ConfigurationManager.AppSettings["awsSecretAccessKey"];
                    try
                    {
                        s3Upload.UploadFileFromStream(s3Key, objFileStream);
                        isUploaded = _receiptRepository.Insert(receipt);
                    }
                    catch (Exception e)
                    {
                        Logger.LogException(e);
                    }

                } 
            }
            return isUploaded;
        }

        public bool SendNotification(Receipt receipt)
        {
            string errorMessage = string.Empty;
            if (receipt == null)
            {
                Logger.LogError("Method:bool SendNotification(Receipt) => Receipt object is null");
                return false;
            }
            if (!ValidateInputForSendNotification(receipt, out errorMessage))
            {
                Logger.LogError("Method:bool SendNotification(Receipt) => " + errorMessage);
                return false;
            }
            if(string.IsNullOrWhiteSpace(receipt.CustomerMobile) 
                || string.IsNullOrWhiteSpace(receipt.CustomerName)
                || string.IsNullOrWhiteSpace(receipt.CustomerEmail))
            {
                FetchCustomerDetails(receipt);
            }
            string packageName = SellCarBL.GetPlanName(receipt.PackageType);
            var model = new
            {
                CustomerName = receipt.CustomerName,
                Amount = receipt.Amount,
                InquiryId = "S" + receipt.InquiryId,
                TransactionId = _transactionPrefix + receipt.PgTransactionId,
                PackageName = packageName
            };
            string message = _templateRender.Render(_receiptSmsTemplateId, model);
            if (!string.IsNullOrWhiteSpace(message))
            {
                _smsLogic.Send(new SMS
                {
                    Mobile = receipt.CustomerMobile,
                    Message = message,
                    SourceModule = SMSType.IndividualListingReceipt,
                    Platform = receipt.Platform,
                    IpAddress = UserTracker.GetUserIp().Split(',')[0]
                });
                receipt.UniqueTransactionId = _transactionPrefix + receipt.PgTransactionId;
                _classifiedMails.SendReceiptMail(receipt, packageName);
                return true;
            }
            else
            {
                Logger.LogError("Method:bool SendNotification(Receipt) => sms message not formed. Error in template binding. InquiryId:" + receipt.InquiryId);
                return false;
            }
        }

        private static bool ValidateInputForSendNotification(Receipt receipt, out string message)
        {
            bool isValid = false;
            if (receipt == null)
            {
                message = "Receipt object is null";
            }
            else if (receipt.InquiryId <= 0)
            {
                message = "InquiryId is not valid. InquiryId =" + receipt.InquiryId;

            }
            else if (receipt.Amount <= 0)
            {
                message = "Amount is not valid. Amount =" + receipt.Amount;

            }
            else if (receipt.PgTransactionId <= 0)
            {
                message = "PgTransactionId is not valid. PgTransactionId =" + receipt.PgTransactionId;
            }
            else
            {
                message = string.Empty;
                isValid = true;
            }
            return isValid;
        }

        private void FetchCustomerDetails(Receipt receipt)
        {
            Seller seller = _sellerCacheRepository.GetIndividualSeller(receipt.InquiryId);
            if (seller != null)
            {
                receipt.CustomerMobile = seller.Mobile;
                receipt.CustomerName = seller.Name;
                receipt.CustomerEmail = seller.Email;
            }
        }
    }
}
