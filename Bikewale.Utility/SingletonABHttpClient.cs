﻿using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Bikewale.Utility
{
    public class SingletonABHttpClient
    {
        private static readonly object _lock = new object();
        private static HttpClient _httpClient = null;

        /// <summary>
        /// 
        /// </summary>
        private SingletonABHttpClient() { }

        /// <summary>
        /// Create singleton of the http client
        /// </summary>
        /// <returns></returns>
        public static HttpClient Instance
        {
            get
            {
                _httpClient = new HttpClient();
                _httpClient.BaseAddress = new Uri(BWConfiguration.Instance.ABApiHostUrl);
                _httpClient.DefaultRequestHeaders.Accept.Clear();

                //sets the Accept header to "application/json", which tells the server to send data in JSON format.
                _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(BWConfiguration.Instance.APIRequestTypeJSON));
                return _httpClient;
            }
        }
    }
}
