using Bikewale.Notifications;
using Nest;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.BAL.AutoComplete
{
    /// <summary>
    /// Author      :   Sadhana Upadhyay
    /// Description :   Elastic Search Manager
    /// </summary>
    public class ElasticSearchManager : IDisposable
    {
        private ElasticClient _client;

        public ElasticClient Client
        {
            get { return _client; }            
        }

        public ElasticSearchManager(string esIndex)
        {
            if (_client == null)
                _client = GetElasticClient(esIndex);
        }

        /// <summary>
        /// Created By : Sadhana Upadhyay on 26 Aug 2015
        /// Summary : To get Elastic Client Connection
        /// </summary>
        /// <param name="esIndex"></param>
        /// <returns></returns>
        private ElasticClient GetElasticClient(string esIndex)
        {
            string elasticHostUrlList = ConfigurationManager.AppSettings["ElasticHostUrl"];
            List<string> nodes = null;

            if (!String.IsNullOrEmpty(elasticHostUrlList))
                nodes = new List<string>(elasticHostUrlList.Split(';'));

            HttpWebResponse response = null;
            string hostUrl = string.Empty;
            try
            {
                while (nodes.Count > 0)
                {
                    // Round Robin Logic
                    Random Random = new Random();
                    hostUrl = nodes[Random.Next(nodes.Count)];
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://" + hostUrl);

                    // To get Server response whether requested server is active or not
                    response = (HttpWebResponse)request.GetResponse();

                    if (response != null)
                    {
                        Uri elasticHostUrl = new Uri("http://" + hostUrl);
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            var settings = new ConnectionSettings(
                                elasticHostUrl,
                             defaultIndex: esIndex
                              );
                            _client = new ElasticClient(settings);
                            break;
                        }
                    }
                    else
                        nodes.Remove(hostUrl);
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.BAL.ElasticSearchManger.GetElasticClient");
                objErr.SendMail();
            }
            return _client;
        }   //End of GetElasticClient
    
        public void Dispose()
        {
            _client=null;
        }
    }
}
