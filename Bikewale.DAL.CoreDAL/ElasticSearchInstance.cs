using Bikewale.Notifications;
using Elasticsearch.Net;
using Nest;
using System;
using System.Configuration;
using System.Linq;

namespace Bikewale.DAL.CoreDAL
{
    /// <summary>
    /// Created by  :   Pratibha Verma on 9 April 2018
    /// Description :   client for ES created
    /// </summary>
    public sealed class ElasticSearchInstance
    {
        private static readonly ElasticSearchInstance _clientInstance = new ElasticSearchInstance();

        private ElasticClient _client;

        static ElasticSearchInstance()
        {

        }

        public static ElasticClient GetInstance()
        {
            return _clientInstance._client;
        }

        private ElasticSearchInstance()
        {
            try
            {

                Uri[] nodes = ConfigurationManager.AppSettings["ElasticHostUrl"].Split(';')
                                             .Select(s => new Uri("http://" + s)).ToArray();
                var connectionPool = new SniffingConnectionPool(nodes);
                var settings = new ConnectionSettings(
                    connectionPool
                ).MaximumRetries(3)
                .DisableDirectStreaming()// 3 times retry
                 .SniffOnConnectionFault(true)
                 .SniffOnStartup(true)
                 .SniffLifeSpan(TimeSpan.FromMinutes(1));

                _client = new ElasticClient(settings);

            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "ElasticClientInstance.ElasticClientInstance()" + ex.InnerException);
            }

        }
    }
}
