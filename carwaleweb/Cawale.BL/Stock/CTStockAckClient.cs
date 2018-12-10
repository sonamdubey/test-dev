using Carwale.Entity.Stock;
using Carwale.Notifications.Logs;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Linq;
using System.Net;

namespace Carwale.BL.Stock
{
    public class CTStockAckClient : IDisposable
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiUrl;
        private readonly string _apiAction;
        private readonly string _apiCode;

        public CTStockAckClient(string apiUrl, string apiAction, string apiCode)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(apiUrl);
            _apiUrl = apiUrl;
            _apiAction = apiAction;
            _apiCode = apiCode;
        }

        public bool SendAcknowledgement(StockAcknowledgement ack)
        {
            Dictionary<string, string> stockData = new Dictionary<string, string>()
                {
                    {"inventory_id", ack.StockId.ToString()},
                    {"cw_dealer_id", ack.DealerId.ToString()},
                    {"action_done", ack.Action},
                    {"status", ack.Status.ToString()},
                    {"reason", ack.Reason},
                    {"action", _apiAction},
                    {"api_code", _apiCode}
                };

            if (!String.IsNullOrEmpty(ack.ProfileId))
            {
                stockData.Add("profile_id", ack.ProfileId);
                stockData.Add("url", ack.Url);
            }
            return CallAckApi(stockData);
        }

        private bool CallAckApi(Dictionary<string, string> stockData)
        {
            string responseContent = "";
            var request = string.Join("||", stockData.Select(kvp => kvp.Key + ":" + kvp.Value));
            try
            {
                var content = new FormUrlEncodedContent(stockData);
                using (HttpResponseMessage response = _httpClient.PostAsync(_apiUrl, content).Result)
                {
                    if (response != null && response.Content != null)
                    {
                        responseContent = response.Content.ReadAsStringAsync().Result;
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            string status = JObject.Parse(responseContent).SelectToken("Status").ToString();
                            if (status.Equals("OK", StringComparison.OrdinalIgnoreCase))
                            {
                                return true;
                            }
                            else
                            {
                                Logger.LogError("Error occured in stock acknowledgement: " + responseContent + " for request: " + request);
                            }
                        }
                        else
                        {
                            Logger.LogError("Error occured in stock acknowledgement: " + responseContent + " for request: " + request);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex, "Response: " + responseContent + " Request: " + request);
            }
            return false;
        }

        public void Dispose()
        {
            _httpClient.Dispose();
        }
    }
}
