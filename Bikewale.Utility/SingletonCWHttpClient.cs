﻿using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Bikewale.Utility
{
    public class SingletonCWHttpClient
    {
        private static readonly object _lock = new object();
        private static HttpClient _httpClient = null;

        /// <summary>
        /// 
        /// </summary>
        private SingletonCWHttpClient() {
        }

        static SingletonCWHttpClient()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(BWConfiguration.Instance.CwApiHostUrl);

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
                //if (_httpClient == null)
                //{
                //    // Take lock while creating the object
                //    lock (_lock)
                //    {
                //        if (_httpClient == null)
                //        {
                //            _httpClient = new HttpClient();
                //            _httpClient.BaseAddress = new Uri(BWConfiguration.Instance.CwApiHostUrl);
                //            _httpClient.DefaultRequestHeaders.Accept.Clear();

                //            //sets the Accept header to "application/json", which tells the server to send data in JSON format.
                //            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(BWConfiguration.Instance.APIRequestTypeJSON));
                //        }
                //    }
                //}
                //_httpClient = new HttpClient();
                //_httpClient.BaseAddress = new Uri(BWConfiguration.Instance.CwApiHostUrl);
                //_httpClient.DefaultRequestHeaders.Accept.Clear();

                ////sets the Accept header to "application/json", which tells the server to send data in JSON format.
                //_httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(BWConfiguration.Instance.APIRequestTypeJSON));
                return _httpClient;
            }
        }
    }
}
