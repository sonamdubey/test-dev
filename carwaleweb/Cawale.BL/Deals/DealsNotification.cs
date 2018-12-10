using Carwale.BL.PaymentGateway;
using Carwale.Entity.Enum;
using Carwale.Entity.Notifications;
using Carwale.Entity.PaymentGateway;
using Carwale.Entity.Template;
using Carwale.Interfaces.Deals;
using Carwale.Interfaces.Notifications;
using Carwale.Interfaces.Template;
using Carwale.Notifications;
using System;
using System.Collections.Generic;

namespace Carwale.BL.Deals
{
    public class DealsNotification : IDealsNotification
    {

        private readonly IEmailNotifications _emailNotify;
        private readonly ITemplatesCacheRepository _templatesCacheRepository;
        private readonly ISMSNotifications _smsNotify;
        private readonly ITemplateRender _templateRender;
        private readonly static string _carwaleAdvantageMaskingNumber;

        static DealsNotification() 
        {
            _carwaleAdvantageMaskingNumber = System.Configuration.ConfigurationManager.AppSettings["CarwaleAdvantageMaskingNumber"];
        }
        public DealsNotification(IEmailNotifications emailNotify, ISMSNotifications smsNotify,
            ITemplatesCacheRepository templatesCacheRepository,ITemplateRender templateRender) 
        {
            _emailNotify = emailNotify;
            _smsNotify = smsNotify;
            _templatesCacheRepository = templatesCacheRepository;
            _templateRender = templateRender;
        }

        public void SendDealsMailToDealer(TemplateContent templateContent, bool isPaymentSucess,GatewayResponse pgResponse)
        {
            string carName = templateContent.MakeName + " " + templateContent.ModelName + " " + templateContent.VersionName;
            templateContent.TollFreeNumber = _carwaleAdvantageMaskingNumber;
            templateContent.CustomerName = pgResponse.Name;
            templateContent.CustomerEmail = pgResponse.Email;
            templateContent.CustomerMobile = pgResponse.Mobile;

            if (isPaymentSucess)
            {
                var templateListObj = _templatesCacheRepository.GetAll(1, (int)EnumTemplatesCategory.DealerCarBlockSucess);

                string subject = "Customer Booking for " + carName;
                templateContent.Subject = subject;
                SendMailsAndSMS(templateListObj, templateContent);
            }
        }

        public void SendDealsMailToCustomer(TemplateContent templateContent, bool isPaymentSucess, GatewayResponse pgResponse)
        {
            try
            {
                string carName = templateContent.MakeName + " " + templateContent.ModelName + " " + templateContent.VersionName;
                templateContent.TollFreeNumber = _carwaleAdvantageMaskingNumber;
                templateContent.EmailId = pgResponse.Email;
                templateContent.PhoneNumber = pgResponse.Mobile;
                templateContent.MailerName = pgResponse.Name;

                if (isPaymentSucess)
                {
                    var templateListObj = _templatesCacheRepository.GetAll(1, (int)EnumTemplatesCategory.CustomerPaymentSucess);

                    string subject = "Payment Receipt Confirmation for Booking of " + carName;
                    templateContent.Subject = subject;
                    SendMailsAndSMS(templateListObj, templateContent);
                }
                else
                {
                    var templateListObj = _templatesCacheRepository.GetAll(1, (int)EnumTemplatesCategory.CustomerPaymentFailed);

                    string subject = "Payment Failure on your Booking attempt for " + carName;
                    templateContent.Subject = subject;
                    SendMailsAndSMS(templateListObj, templateContent);
                }
            }

            catch(Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "DealsNotifications.SendDealsMailToCustomer()\n Exception : " + ex.Message);
                objErr.LogException();
            }
        }

        private void SendMailsAndSMS(List<Templates> templateListObj, TemplateContent templateContent)
        {
            if (templateListObj != null)
            {
                foreach (var templateObj in templateListObj)
                {
                    //message Notification
                    if (templateObj.TemplateType == 1)
                    {
                        string emailBody = _templateRender.Render<TemplateContent>(templateObj.UniqueName, templateContent, templateObj.Html);
                        _emailNotify.SendMail(templateContent.EmailId, templateContent.Subject, emailBody, null, null, new string[] { "deals@carwale.com" });                        
                    }
                    //SMS Notification
                    else if (templateObj.TemplateType == 2)
                    {
                        string messageBody = _templateRender.Render<TemplateContent>(templateObj.UniqueName, templateContent, templateObj.Html);

                        string[] mobileNos = templateContent.PhoneNumber.Split(',');
                        
                        var sms = new SMS()
                        {
                            Message = messageBody,
                            //Mobile = templateContent.PhoneNumber,
                            ReturnedMsg = "",
                            Status = true,
                            SMSType = Convert.ToInt32(SMSType.DealsCar),
                            PageUrl = "CWDealsCar"
                        };
                        foreach (var mobileNo in mobileNos) 
                        {
                            sms.Mobile = mobileNo;
                            _smsNotify.ProcessSMS(sms);
                        }
                        
                    }
                }
            }
        }
    }
}
