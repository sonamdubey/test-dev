
using Consumer;
using System;
namespace Bikewale.RabbitMq.LeadProcessingConsumer
{
    class Program
    {
        static void Main(string[] args)
        {
            log4net.Config.XmlConfigurator.Configure();
            try
            {
                Logs.WriteInfoLog("Started at : " + DateTime.Now);
                LeadConsumer consumer = new LeadConsumer();
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
