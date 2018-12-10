using Carwale.Entity.Classified.CarValuation;
using Carwale.Notifications.Logs;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using AEPLCore.Utils.JsonHelper;
using System.Configuration;

namespace Carwale.DAL.Classified.CarValuation
{
    public class CTValuationClient : IDisposable
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiUrl;
        public CTValuationClient(string apiUrl)
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(apiUrl);
            _apiUrl = apiUrl;
        }

        public Valuation GetValuation(ValuationUrlParameters valuationUrlParameters)
        {
            if (valuationUrlParameters != null)
            {
                string queryString = GetQueryString(valuationUrlParameters);
                try
                {
                    using (HttpResponseMessage response = _httpClient.GetAsync(queryString).Result)
                    {
                        if (response != null && response.StatusCode == HttpStatusCode.OK)
                        {
                            using (var responseStream = response.Content.ReadAsStreamAsync().Result)
                            {
                                var valuation = Deserializer<Valuation>.DeserializeFromStream(responseStream);
                                if (valuation != null && valuation.Case > 0)
                                {
                                    return valuation;
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logger.LogException(ex);
                }
            }
            return null;
        }

        private string GetQueryString(ValuationUrlParameters valuationReq)
        {
            StringBuilder queryString = new StringBuilder("?");
            queryString.Append("makeId=" + valuationReq.MakeId);
            queryString.Append("&modelId=" + valuationReq.ModelId);
            queryString.Append("&versionId=" + valuationReq.VersionId);
            queryString.Append("&year=" + valuationReq.Year);
            queryString.Append("&cityId=" + valuationReq.CityId);
            if (valuationReq.Kilometers > 0)
            {
                queryString.Append("&kms_driven=" + valuationReq.Kilometers);
            }
            if (valuationReq.Owners > 0)
            {
                queryString.Append("&owner=" + valuationReq.Owners);
            }
            if (!valuationReq.IsSellingIndex)
            {
                queryString.Append("&iwantsell=true");
            }
            return queryString.ToString();
        }

        public void Dispose()
        {
            _httpClient.Dispose();
        }
    }
}
