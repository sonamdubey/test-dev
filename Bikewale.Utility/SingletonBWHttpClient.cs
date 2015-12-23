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
    /// 
    /// </summary>
    public sealed class SingletonBWHttpClient
    {
        private static readonly object _lock = new object();
        private static HttpClient _httpClient = null;

        /// <summary>
        /// 
        /// </summary>
        private SingletonBWHttpClient() { }

        /// <summary>
        /// Create singleton of the http client
        /// </summary>
        /// <returns></returns>
        public static HttpClient Instance
        {
            get
            {
                //if (_httpClient == null)
                //{
                //    // Take lock while creating the object
                //    lock (_lock)
                //    {
                //        if (_httpClient == null)
                //        {
                //            _httpClient = new HttpClient();
                //            _httpClient.BaseAddress = new Uri(BWConfiguration.Instance.BwHostUrl);
                //            _httpClient.DefaultRequestHeaders.Accept.Clear();

                //            //sets the Accept header to "application/json", which tells the server to send data in JSON format.
                //            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(BWConfiguration.Instance.APIRequestTypeJSON));
                //        }
                //    }
                //}
                _httpClient = new HttpClient();
                _httpClient.BaseAddress = new Uri(BWConfiguration.Instance.BwHostUrl);
                _httpClient.DefaultRequestHeaders.Accept.Clear();

                //sets the Accept header to "application/json", which tells the server to send data in JSON format.
                _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(BWConfiguration.Instance.APIRequestTypeJSON));
                return _httpClient;
            }
        }
    }
}
