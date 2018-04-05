using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Utility
{
    /// <summary>
    /// Author  : Kartik rathod on 30 march 18 
    /// Desc    : http client Singleton class for CWOPRHostUrl
    /// </summary>
    public class SingletonCWOPRHttpClient
    {
        private static readonly object _lock = new object();
        private static HttpClient _httpClient = null;

        /// <summary>
        /// 
        /// </summary>
        private SingletonCWOPRHttpClient()
        {
        }

        static SingletonCWOPRHttpClient()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(BWOprConfiguration.Instance.CwOprHostUrl);

            _httpClient.DefaultRequestHeaders.Add("clientid", "5");
            _httpClient.DefaultRequestHeaders.Add("platformid", "2");

            //sets the Accept header to "application/json", which tells the server to send data in JSON format.
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(BWConfiguration.Instance.APIRequestTypeJSON));
        }

        /// <summary>
        /// Create singleton of the http client
        /// </summary>
        /// <returns></returns>
        public static HttpClient Instance
        {
            get
            {
                return _httpClient;
            }
        }
    }
}
