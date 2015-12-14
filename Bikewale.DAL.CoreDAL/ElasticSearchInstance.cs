using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nest;
using System.Configuration;
using Elasticsearch.Net.ConnectionPool;
using Bikewale.Notifications; 

namespace Bikewale.DAL.CoreDAL
{
    public sealed class ElasticSearchInstance
    {
        private static readonly ElasticSearchInstance _clientInstance = new ElasticSearchInstance();

        private ElasticClient _client;

        static ElasticSearchInstance()
        {
            
        }

        public static ElasticClient GetInstance() {
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
                    connectionPool,
                    defaultIndex: ConfigurationManager.AppSettings["ElasticIndexName"]
                ).SetTimeout(1000 * 30)     // 30 seconds timeout
                 .MaximumRetries(3)         // 3 times retry
                 .SniffOnConnectionFault(true)
                 .SniffOnStartup(true)
                 .SniffLifeSpan(TimeSpan.FromMinutes(1));

                _client = new ElasticClient(settings);
            }
            catch (Exception ex) 
            {
                var objErr = new ErrorClass(ex, "ElasticClientInstance.ElasticClientInstance()" + ex.InnerException);
                objErr.SendMail();
            }

        }
    }
}
