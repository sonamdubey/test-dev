using Consumer;
using System;
using System.Configuration;

namespace Bikewale.ElasticSearch.IndexUpdaterConsumer
{
    class Program
    {
        static void Main(string[] args)
        {
            string queueName = ConfigurationManager.AppSettings["QueueName"].ToUpper();
            string consumerName = ConfigurationManager.AppSettings["ConsumerName"].ToString();
            string retryCount = ConfigurationManager.AppSettings["RetryCount"];
            string rabbitMsgTTL = ConfigurationManager.AppSettings["RabbitMsgTTL"];
            log4net.Config.XmlConfigurator.Configure();
            try
            {
                Logs.WriteInfoLog("Started at : " + DateTime.Now);
                IndexUpdateConsumer consumer = new IndexUpdateConsumer(queueName, consumerName, retryCount, rabbitMsgTTL);
                consumer.ProcessMessages();                
            }
            catch (Exception ex)
            {
                Logs.WriteErrorLog("Exception : " + ex.Message);
            }
            finally
            {
                Logs.WriteInfoLog("End at : " + DateTime.Now);
            }
        }
    }
}
