using Carwale.Entity.CustomerVerification;
using Carwale.Entity.Enum;
using Carwale.Interfaces.CustomerVerification;
using Carwale.Interfaces.Notifications;
using Carwale.Notifications;
using Carwale.Notifications.SMSTemplates;
using Carwale.Utility;
using RabbitMqPublishing;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Carwale.Entity;
using Newtonsoft.Json;

namespace Carwale.BL.CustomerVerification
{
    public class CustomerVerificationBL : ICustomerVerification
    {
        protected ICustomerVerificationRepository _customerVerificationRepo;
        private readonly ISMSNotifications _smsNotify;

        public CustomerVerificationBL(ICustomerVerificationRepository customerVerificationRepo, ISMSNotifications smsNotify)
        {
            _customerVerificationRepo = customerVerificationRepo;
            _smsNotify = smsNotify;
        }

        public MobileVerificationReponseEntity IsMobileVerified(string mobileNumber, string emailId, Platform platform, string pageUrl = "", int classifiedSourceId = 0)
        {
            MobileVerificationReponseEntity response = new MobileVerificationReponseEntity();
            string otpCode;

            response.IsMobileVerified = _customerVerificationRepo.VerifyMobile(mobileNumber, emailId, platform, out otpCode, "-1", classifiedSourceId);

            UpdateVerificationResponse(mobileNumber, emailId, platform, pageUrl, classifiedSourceId, response, otpCode, null);
            return response;
        }

        public MobileVerificationReponseEntity IsMobileAndTokenVerified(string mobileNumber, Platform platform, string clientTokenId)
        {
            MobileVerificationReponseEntity response = new MobileVerificationReponseEntity();
            string otpCode;

            response.IsMobileVerified = _customerVerificationRepo.VerifyMobile(mobileNumber, "", platform, out otpCode, "-1", 0, clientTokenId);

            UpdateVerificationResponse(mobileNumber, "", platform, "", 0, response, otpCode, clientTokenId);
            return response;
        }

        public bool ResendOtpSms(string email, string mobile, string pageUrl = "")
        {
            bool smsSent = false;
            try
            {
                if (!email.Contains("@"))
                {
                    email = mobile + "@unknown.com";
                }
                string otpCode = _customerVerificationRepo.GetOtpCode(email, mobile);
                if (!string.IsNullOrWhiteSpace(otpCode))
                {
                    SendSMS(mobile, otpCode, pageUrl);
                    smsSent = true;
                }
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, "CustomerVerificationBL.ResendOtpSms");
                objErr.SendMail();
            }
            return smsSent;
        }

        private void SendSMS(string mobileNumber, string otpCode, string pageUrl)
        {
            var sms = new OTPVerificationTemplate().GetOTPVerificationTemplate(mobileNumber, otpCode, pageUrl);
            _smsNotify.ProcessSMS(sms, true);
        }

        private void GetTollFreeNumber(Platform platformId, MobileVerificationReponseEntity missCallVerificationResponse)
        {
            if (platformId == Platform.CarwaleiOS) // for iOS as toll free numbers are considered to be US based in iOS
            {
                missCallVerificationResponse.TollFreeNumber = ConfigurationManager.AppSettings["MissedCallVerificationNumber_iOS"] ?? string.Empty;
            }
            else
            {
                missCallVerificationResponse.TollFreeNumber = ConfigurationManager.AppSettings["MissedCallVerificationNumber"] ?? string.Empty;
            }
        }

        private void UpdateVerificationResponse(string mobileNumber, string emailId, Platform platform, string pageUrl, int classifiedSourceId, MobileVerificationReponseEntity response, string otpCode, string clientTokenId)
        {
            if (!response.IsMobileVerified)
            {
                if (otpCode != "-1")
                {
                    SendSMS(mobileNumber, otpCode, pageUrl);
                }

                bool showTollFreeNumber = _customerVerificationRepo.ProcessTollFreeNumber(mobileNumber, emailId, platform, classifiedSourceId, clientTokenId);

                if (showTollFreeNumber)
                {
                    GetTollFreeNumber(platform, response);
                }
            }
        }

        public bool IsMobileVerified(string mobileNumber)
        {
            if (!string.IsNullOrWhiteSpace(mobileNumber))
            {
                return _customerVerificationRepo.IsMobileVerified(mobileNumber);
            }
            return false;
        }

        public void SendToVerificationQueue(MobileVerification mobileVerification)
        {
            NameValueCollection verificationData = new NameValueCollection
            {
                {"MobileVerification", JsonConvert.SerializeObject(mobileVerification)}
            };

            RabbitMqPublish publisher = new RabbitMqPublish();
            publisher.PublishToQueue(ConfigurationManager.AppSettings["MobileVerificationQueue"], verificationData);
        }
    }
}
