using Carwale.Entity.Stock;
using Carwale.Notifications.Logs;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace Carwale.BL.Stock
{
    public class CTStockImageAckClient : IDisposable
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiUrl;
        private readonly string _apiAction;
        private readonly string _apiCode;

        public CTStockImageAckClient(string apiUrl, string apiAction, string apiCode)
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(apiUrl);
            _apiUrl = apiUrl;
            _apiAction = apiAction;
            _apiCode = apiCode;
        }

        public bool SendAcknowledgement(StockImageAcknowledgement ack)
        {
            Dictionary<string, string> imageData = new Dictionary<string, string>()
                {
                    {"image_id", ack.ImageId.ToString()},
                    {"inventory_id", ack.StockId.ToString()},
                    {"action_done", ack.Action},
                    {"status", ack.Status.ToString()},
                    {"reason", ack.Reason},
                    {"action", _apiAction},
                    {"api_code", _apiCode}
                };
            return CallAckApi(imageData);
        }

        private bool CallAckApi(Dictionary<string, string> imageData)
        {
            try
            {
                var content = new FormUrlEncodedContent(imageData);
                using (HttpResponseMessage response = _httpClient.PostAsync(_apiUrl, content).Result)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        string responseContent = response.Content.ReadAsStringAsync().Result;
                        string status = JObject.Parse(responseContent).SelectToken("Status").ToString();
                        if (status == "OK")
                        {
                            return true;
                        }
                        else
                        {
                            Logger.LogError("Error occured in stock image acknowledgement: " + responseContent);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return false;
        }

        public void Dispose()
        {
            _httpClient.Dispose();
        }
    }
}
