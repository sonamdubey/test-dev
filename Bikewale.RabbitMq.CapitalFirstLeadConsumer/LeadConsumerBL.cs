using Bikewale.Entities.Finance.CapitalFirst;
using Bikewale.Notifications;
using Bikewale.Notifications.MailTemplates;
using Bikewale.RabbitMq.CapitalFirstLeadConsumer.htmltemplates;
using Consumer;
using System;

namespace Bikewale.RabbitMq.CapitalFirstLeadConsumer
{
    internal class LeadConsumerBL : IDisposable
    {
        private readonly LeadConsumerRepository _repo = null;
        public LeadConsumerBL()
        {
            _repo = new LeadConsumerRepository();
        }

        public bool ProcessLead(CarTradeVoucher voucher)
        {
            bool isProcessed = false;
            try
            {
                using (_repo)
                {
                    //Get Lead details from DB 
                    var lead = _repo.GetLeadDetails(voucher.LeadId);
                    if (lead != null)
                    {
                        //Format Email and SMS code here
                        isProcessed = NotifyCustomer(lead);

                        //Update lead table with notification date set to current
                        if (isProcessed)
                            _repo.UpdateCustomerNotified(voucher.LeadId);
                    }
                }
            }
            catch
            {
                Logs.WriteErrorLog(string.Format("Error occured while processing Lead: {0}", Newtonsoft.Json.JsonConvert.SerializeObject(voucher)));
            }
            return isProcessed;
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 12 Sep 2017
        /// Description :   Notify customer with Email and SMS
        /// </summary>
        /// <param name="lead"></param>
        /// <returns></returns>
        private bool NotifyCustomer(CapitalFirstLeadEntity lead)
        {
            SendEmail(lead);
            SendSMS(lead);
            return lead != null;
        }

        #region IDisposable Support

        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (_repo != null)
                    {
                        _repo.Dispose();
                    }

                    // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                    // TODO: set large fields to null.

                    disposedValue = true;
                }
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~LeadConsumerBL() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Description : Send email.
        /// </summary>
        private void SendEmail(CapitalFirstLeadEntity lead)
        {
            try
            {
                ComposeEmailBase objEmail = new CapitalFirstSuccessEmailTemplate(lead);
                byte[] pdfFile = CreatePdf.ConvertToBytes(new PdfAttachment(lead).ComposeBody());
                objEmail.Send(lead.EmailId, "Congratulations! Your loan has been approved !", pdfFile, "voucher.pdf");
            }
            catch
            {
                Logs.WriteErrorLog(string.Format("Error occured while processing Lead:SendEmail() LeadId: {0}", lead.CTLeadId));
            }
        }

        /// <summary>
        /// Desc : Send SMS.
        /// </summary>
        private void SendSMS(CapitalFirstLeadEntity lead)
        {
            try
            {
                SMSTypes newSms = new SMSTypes();
                string smsTemplate;
                if (lead.Status)
                {
                    smsTemplate = string.Format("Congratulations! Your bike loan has been pre-approved by Capital First. Your loan voucher code is {0}. For further steps, please get in touch with {1} and {2}.", lead.VoucherNumber, lead.AgentName, lead.AgentNumber);
                    newSms.CapitalFirstLoanSMS(lead.MobileNo, "", EnumSMSServiceType.SMSforCapitalFirstSuccess, smsTemplate);
                }
                else
                {
                    smsTemplate = "Your bike loan application did not get approved based on your credit profile. Thank you for visiting BikeWale.";
                    newSms.CapitalFirstLoanSMS(lead.MobileNo, "", EnumSMSServiceType.SMSforCapitalFirstFailure, smsTemplate);
                }
            }
            catch
            {
                Logs.WriteErrorLog(string.Format("Error occured while processing Lead:SendSMS() LeadId: {0}", lead.CTLeadId));
            }
        }

        #endregion
    }
}
