using Consumer;
using RabbitMqPublishing;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceQuoteLeadSMS
{
    public class SMSCommon
    {
        /// <summary>
        /// Queues the SMS to RabbitMq
        /// </summary>
        /// <param name="sms"></param>
        public void PushSMSInQueue(uint smsId, string message, string customerContact)
        {
            string appName = String.Empty;
            NameValueCollection nvcSMS = null;
            RabbitMqPublish publish = null;
            try
            {
                Logs.WriteInfoLog(String.Format("Added Into Queue : {0}", smsId));

                appName = ConfigurationManager.AppSettings["rabbitMqAppName"];

                Logs.WriteInfoLog(String.Format("Added Into DB : {0}", smsId));

                nvcSMS = new NameValueCollection();
                nvcSMS.Add("id", Convert.ToString(smsId));
                nvcSMS.Add("message", message);
                nvcSMS.Add("clientno", customerContact);
                nvcSMS.Add("prefix", "BW");
                nvcSMS.Add("provider", "");

                publish = new RabbitMqPublish();
                publish.PublishToQueue(appName, nvcSMS);

                Logs.WriteInfoLog("SMS Pushed in Queue Successfully.");
            }
            catch (Exception ex)
            {
                Logs.WriteErrorLog("Exception in SendSMSEx : " + ex.Message);
            }
        }
    }   //End of class
}   //End of namespace
