using RabbitMqPublishing;
using System;
using System.Collections.Specialized;

namespace Bikewale.Utility
{
    /// <summary>
    /// Created by : Sangram Nandkhile on 13 July 2016
    /// Summary: To push chages in make, model and Versions to rabbitMQ
    /// </summary>
    public static class SyncBWData
    {
        /// <summary>
        /// To push Make Model version related info into the RabbitMQ
        /// </summary>
        /// <param name="spName">Name of the stored Procedure to be executed</param>
        /// <param name="dbName">Database name</param>
        /// <param name="nvc">Param and value collection</param>
        /// <returns></returns>
        public static bool PushToQueue(string spName, DataBaseName dbEnum, NameValueCollection nvc)
        {
            try
            {
                string dbName = string.Empty;
                switch (dbEnum)
                {
                    case DataBaseName.BW:
                        dbName = "BW";
                        break;
                    case DataBaseName.CW:
                        dbName = "CW";
                        break;
                }
                nvc.Add("DBNAME", dbName);
                nvc.Add("SPNAME", spName);
                RabbitMqPublish publish = new RabbitMqPublish();
                publish.PublishToQueue(BWConfiguration.Instance.BWDataSynchQueue, nvc);
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }
    }

    public enum DataBaseName
    {
        CW = 1,
        BW = 2
    }
}
