using Consumer;
using Elasticsearch.Net;
using Nest;
using System;
using System.Configuration;
using System.Linq;



namespace Bikewale.Sitemap.MainSitemap
{
    /// <summary>
    /// Created by  :   Pratibha Verma on 9 April 2018
    /// Description :   Elastic Search Instance
    /// </summary>
    public class ElasticSearchInstance
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
                Logs.WriteErrorLog("ElasticClientInstance.ElasticClientInstance()", ex.InnerException);
            }

        }
    }
}

