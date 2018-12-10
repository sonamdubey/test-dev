using Carwale.Entity.Notifications;
using Carwale.Interfaces;
using Carwale.Interfaces.Notifications;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.BL.Notifications
{
    public class SMSNotification : ISMSNotifications
    {
        private readonly ISMSRepository _smsRepo;

        public SMSNotification(ISMSRepository smsRepo)
        {
            _smsRepo = smsRepo;
        }

        /// <summary>
        /// For Sending Sms To Dealer
        /// Written By : Ashish Verma on 2/6/2014
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public void ProcessSMS(SMS sms)
        {
            ProcessSMS(sms, false);
        }

        public void ProcessSMS(SMS sms, bool isPriority)
        {
            if (!String.IsNullOrEmpty(sms.Mobile))
            {
                // Check if multiple mobile nos are present.
                string[] mobileNos = sms.Mobile.Split(',');

                // Send sms to each mobile number
                foreach (var mobileNo in mobileNos)
                {
                    PushToQueue(mobileNo, sms, isPriority);
                }
            }
        }

        /// <summary>
        /// For Pushing Sms Nvc to Queue
        /// SaveSMSSentData() function to Save Data to SMSSent table
        /// Written By : Ashish Verma on 2/6/2014
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public void PushToQueue(string mobile, SMS sms, bool isPriority)
        {
            if (!string.IsNullOrWhiteSpace(mobile))
            {
                var nvc = new NameValueCollection();
                sms.Mobile = mobile.Trim();
                nvc.Add("id", _smsRepo.SaveSMSSentData(sms));
                nvc.Add("message", sms.Message);
                nvc.Add("clientno", sms.Mobile);
                nvc.Add("prefix", "CW");
                nvc.Add("provider", "");
                RabbitMqPublishing.RabbitMqPublish obj = new RabbitMqPublishing.RabbitMqPublish();
                string smsQueueName = isPriority ? ConfigurationManager.AppSettings["smspriorityqueue"] : ConfigurationManager.AppSettings["smsqueue"];
                obj.PublishToQueue(smsQueueName, nvc); 
            }
       }
    }
}
