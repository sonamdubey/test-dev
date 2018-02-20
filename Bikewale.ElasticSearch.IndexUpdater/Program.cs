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
                IndexUpdateConsumer consumer = new IndexUpdateConsumer(queueName, consumerName, retryCount, rabbitMsgTTL);
                consumer.ProcessMessages();
                Logs.WriteInfoLog("Started at : " + DateTime.Now);
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
