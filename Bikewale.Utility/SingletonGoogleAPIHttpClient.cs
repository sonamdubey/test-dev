using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Bikewale.Utility
{
    /// <summary>
    /// Author  : Kartik rathod on 30 march 18 
    /// Desc    : http client Singleton class for GoogleAPIHostUrl
    /// </summary>
    public class SingletonGoogleAPIHttpClient
    {
        private static readonly object _lock = new object();
        private static HttpClient _httpClient = null;

        /// <summary>
        /// 
        /// </summary>
        private SingletonGoogleAPIHttpClient()
        {
        }

        static SingletonGoogleAPIHttpClient()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(BWOprConfiguration.Instance.GoogleAPIHostUrl);


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
