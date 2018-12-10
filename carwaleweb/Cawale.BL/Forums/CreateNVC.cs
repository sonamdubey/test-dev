using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMqPublishing;
using System.Collections.Specialized;
using Carwale.Notifications;

namespace Carwale.BL.Forums
{
    public class CreateNVC
    {
        public void AddToIndex(string threadId, string action)
        {
            bool result = false;
            try
            {
                //rabbitmq code 
                RabbitMqPublish rabbitmqPublish = new RabbitMqPublish();
                NameValueCollection nvc = new NameValueCollection();
                nvc.Add("threadid", threadId);
                nvc.Add("category", "ForumsThreads");
                nvc.Add("command", action);
                result = rabbitmqPublish.PublishToQueue("LUCENE-MYSQL", nvc);
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Error While publishing Thread To RabbitMQ.");
                objErr.SendMail();
            }
        }

        /// <summary>
        /// This function creates the nvc object for rabbitmq and subsequently publishes the same to rabbitmq.
        /// </summary>
        /// <param name="threadId"></param>
        public void UpdateLuceneIndex(string threadId,string action)
        {
            bool result = false;
            try
            {
                //rabbitmq code 
                RabbitMqPublish rabbitmqPublish = new RabbitMqPublish();
                NameValueCollection nvc = new NameValueCollection();
                nvc.Add("threadid", threadId);
                nvc.Add("category", "ForumsThreads");
                nvc.Add("command", action);
                result = rabbitmqPublish.PublishToQueue("LUCENE-MYSQL", nvc);
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Error While publishing Thread To RabbitMQ.");
                objErr.SendMail();
            }
        }
    }
}
