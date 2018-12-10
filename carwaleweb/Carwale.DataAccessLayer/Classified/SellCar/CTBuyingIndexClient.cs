using Carwale.Entity.Classified.CarValuation;
using Carwale.Interfaces.Classified.SellCar;
using Carwale.Notifications.Logs;
using Newtonsoft.Json.Linq;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using Carwale.Entity.Classified;
using System.Web;

namespace Carwale.DAL.Classified.SellCar
{
    public class CTBuyingIndexClient : IDisposable, ICTBuyingIndexClient
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiUrl;

        public CTBuyingIndexClient(string apiUrl)
        {
            _apiUrl = apiUrl;
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(_apiUrl);
        }

        public int GetBuyingIndex(int inquiryId)
        {
            int buyingIndex = 0;
            if (inquiryId > 0)
            {
                SellCarRepository sellCarRepo = new SellCarRepository();
                var carData = sellCarRepo.GetCustomerSellInquiryVehicleDetails(inquiryId);
                buyingIndex = GetBuyingIndex(new ValuationUrlParameters()
                {
                    MakeId = carData.MakeId,
                    ModelId = carData.ModelId,
                    VersionId = carData.VersionId,
                    Year = (short)carData.MakeYear.Year,
                    CityId = carData.CityId,
                    Owners = carData.Owners 
                });
            }
            return buyingIndex;
        }

        public int GetBuyingIndex(ValuationUrlParameters valuationUrlParameters)
        {
            int buyingIndex = 0;            
            if (valuationUrlParameters != null)
            {
                string queryString = GetQueryString(valuationUrlParameters);
                try
                {
                    using (HttpResponseMessage response = _httpClient.GetAsync(queryString).Result)
                    {
                        Logger.LogInfo("Hit Buying Index API with qs:" + queryString);
                        if (response.StatusCode == HttpStatusCode.OK && response.Content != null)
                        {
                            string responseContent = response.Content.ReadAsStringAsync().Result;
                            if (responseContent.Contains("right_price"))
                            {
                                buyingIndex = JObject.Parse(responseContent).Value<int>("right_price");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logger.LogException(ex);
                }
            }
            return buyingIndex;
        }

        private string GetQueryString(ValuationUrlParameters valuationUrlParameters)
        {
            StringBuilder queryString = new StringBuilder("?");
            queryString.Append("makeId=" + valuationUrlParameters.MakeId);
            queryString.Append("&modelId=" + valuationUrlParameters.ModelId);
            queryString.Append("&versionId=" + valuationUrlParameters.VersionId);
            queryString.Append("&year=" + valuationUrlParameters.Year);
            queryString.Append("&cityId=" + valuationUrlParameters.CityId);
            if (valuationUrlParameters.Owners > 0)
                queryString.Append("&owners=" + (valuationUrlParameters.Owners == (int)UsedCarOwnerTypes.FourOrMoreOwners ?  HttpUtility.UrlEncode("4+") : valuationUrlParameters.Owners.ToString()));
            return queryString.ToString();
        }

        public void Dispose()
        {
            _httpClient.Dispose();
        }
    }
}
