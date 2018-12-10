using System;
using System.Configuration;
using System.Net.Http;

namespace Carwale.DAL
{
    public static class HttpClientManager
    {
        private static HttpClientHandler _sharedHandler = new HttpClientHandler(); //Singleton handler to be shared in all client unless a different configuration is required. Never dispose this.
        private static HttpClient _commonClient;
        private static HttpClient _ctExchangeClient;

        private static HttpClient CreateClient()
        {
            //Set disposeHandler to false as client code can dispose HttpClient instances
            return new HttpClient(_sharedHandler, false);
        }

        public static HttpClient CommonClient
        {
            get
            {
                if (_commonClient == null)
                {
                    _commonClient = CreateClient();
                }
                return _commonClient;
            }
        }

        public static HttpClient CtExchangeClient
        {
            get
            {
                if (_ctExchangeClient == null)
                {
                    _ctExchangeClient = CreateClient();
                    _ctExchangeClient.BaseAddress = new Uri(ConfigurationManager.AppSettings["CTExchangeHostUrl"]);
                }
                return _ctExchangeClient;
            }
        }
    }
}
