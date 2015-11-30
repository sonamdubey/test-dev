using Consumer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bikewale.Notifications;

namespace PriceQuoteLeadSMS
{
    class LeadSMS
    {
        private void GetLeadInformation()
        {
            
        
        }

        internal void SendLeadsToCustDealer()
        {
            try
            {
                // Get lead info
                GetLeadInformation();

                // Send notifications

                //SendMails mail = new SendMails();
                //mail.SendMail(sendTo, subject, ComposeBody(), replyTo);

                // push inquiry to the autobiz
                
            }
            catch (Exception ex)
            {
                Logs.WriteErrorLog("Exception in SendSMS Method : " + ex.Message);
            }
        }
    }
}
