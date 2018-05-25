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
                    var lead = GetLeadDetails(voucher.LeadId);
                    if (lead != null)
                    {
                        //Format Email and SMS code here
                        isProcessed = NotifyCustomer(lead, voucher.Status);

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

        private CapitalFirstLeadEntity GetLeadDetails(string leadId)
        {
            CapitalFirstLeadEntity lead = default(CapitalFirstLeadEntity);

            try
            {
                lead = _repo.GetLeadDetails(leadId);
                lead.FullName = string.Format("{0} {1}", lead.FirstName, lead.LastName);

                #region calculate loan variables

                lead.OnRoadPrice = lead.Exshowroom + lead.Insurance + lead.Rto;
                // Loan amount should be 80% of Bike On Road Price
                lead.LoanAmount = Convert.ToUInt32(Math.Round(lead.OnRoadPrice * 0.8));
                lead.LoanAmountStr = Bikewale.Utility.Format.FormatPrice(lead.LoanAmount.ToString());
                lead.Downpayment = Bikewale.Utility.Format.FormatPrice((lead.OnRoadPrice - lead.LoanAmount).ToString());
                double interestRate = 23;
                int months = 36;
                int emiValue = CalculateEmi(interestRate, months, lead.LoanAmount);
                lead.Emi = Bikewale.Utility.Format.FormatPrice(emiValue.ToString());

                #endregion
            }
            catch (Exception)
            {
                Logs.WriteErrorLog(string.Format("Error occured while processing Lead: GetLeadDetails() Lead Id:{0}", leadId));
            }

            return lead;

        }

        /// <summary>
        /// Calculates the emi.
        /// </summary>
        /// <param name="interestRate">The interest rate. ex, 23 or 10</param>
        /// <param name="months">The months. Ex. 12</param>
        /// <param name="loanAmount">The loan amount. Ex. 120000</param>
        /*
           Default EMI Calculation
           [P x R x (1+R)^N]/[(1+R)^N-1]
           R = 23% / 12
           N = 36 Months
           P = 80% of Bike On Road Price
        
        */
        private int CalculateEmi(double interestRate, int months, double loanAmount)
        {
            double payment = 0;
            int perMonth = 0;
            try
            {
                if (interestRate > 1)
                {
                    interestRate = interestRate / 100;
                }
                payment = (loanAmount * Math.Pow((interestRate / 12) + 1,
                          (months)) * interestRate / 12) / (Math.Pow
                          (interestRate / 12 + 1, (months)) - 1);
                perMonth = Convert.ToInt32(Math.Round(payment));
            }
            catch
            {
                Logs.WriteErrorLog(string.Format("Error occured while processing Lead: CalculateEMI() interest:{0}, months: {1}, amount: {2}", interestRate, months, loanAmount));
            }
            return perMonth;
        }


        /// <summary>
        /// Created by  :   Sumit Kate on 12 Sep 2017
        /// Description :   Notify customer with Email and SMS
        /// </summary>
        /// <param name="lead"></param>
        /// <returns></returns>
        private bool NotifyCustomer(CapitalFirstLeadEntity lead, CarTradeVoucherStatus status)
        {
            SendEmail(lead, status);
            SendSMS(lead, status);
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
        private void SendEmail(CapitalFirstLeadEntity lead, CarTradeVoucherStatus status)
        {
            try
            {
                switch (status)
                {
                    case CarTradeVoucherStatus.Pre_Approved:
                        ComposeEmailBase objEmail = new CapitalFirstSuccessEmailTemplate(lead.BikeName, lead.AgentName, lead.AgentNumber);
                        byte[] pdfFile = CreatePdf.ConvertToBytes(new CapitalFirstPdfAttachment(lead).ComposeBody());
                        string attachmentName = string.Format("{0}.pdf", string.Format("{0}_{1}_{2}", lead.FirstName, lead.LastName, lead.VoucherNumber).Replace(".", string.Empty));
                        objEmail.Send(lead.EmailId, "Bike Loan Application Pre-Approved", pdfFile, attachmentName);
                        break;
                    case CarTradeVoucherStatus.Rejected:
                    case CarTradeVoucherStatus.Credit_Refer:
                        break;
                    default:
                        break;
                }

            }
            catch
            {
                Logs.WriteErrorLog(string.Format("Error occured while processing Lead:SendEmail() LeadId: {0}", lead.CtLeadId));
            }
        }

        /// <summary>
        /// Desc : Send SMS.
        /// </summary>
        private void SendSMS(CapitalFirstLeadEntity lead, CarTradeVoucherStatus status)
        {
            try
            {
                SMSTypes newSms = new SMSTypes();
                string smsTemplate;

                switch (status)
                {
                    case CarTradeVoucherStatus.Pre_Approved:
                        smsTemplate = string.Format("Congratulations! Your bike loan has been pre-approved by Capital First. Your loan voucher code is {0}. For further steps, please get in touch with Capital First Executive {1} - {2}.", lead.VoucherNumber, lead.AgentName, lead.AgentNumber);
                        newSms.CapitalFirstLoanSms(lead.MobileNo, "", EnumSMSServiceType.SMSforCapitalFirstSuccess, smsTemplate);
                        break;
                    case CarTradeVoucherStatus.Rejected:
                    case CarTradeVoucherStatus.Credit_Refer:
                        smsTemplate = "Your bike loan application could not be approved online based on your credit profile. Thank you for visiting BikeWale.";
                        newSms.CapitalFirstLoanSms(lead.MobileNo, "", EnumSMSServiceType.SMSforCapitalFirstFailure, smsTemplate);
                        break;
                    default:
                        break;
                }
            }
            catch
            {
                Logs.WriteErrorLog(string.Format("Error occured while processing Lead:SendSMS() LeadId: {0}", lead.CtLeadId));
            }
        }

        #endregion
    }
}
