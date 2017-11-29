using Consumer;
using System;

namespace DataSync
{
    internal class Program
    {
        static void Main(string[] args)
        {
            log4net.Config.XmlConfigurator.Configure();
            try
            {
                Logs.WriteInfoLog("Started at : " + DateTime.Now);
                DataSyncConsumer dataSyncConsumer = new DataSyncConsumer();
                dataSyncConsumer.ProcessQueries();
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