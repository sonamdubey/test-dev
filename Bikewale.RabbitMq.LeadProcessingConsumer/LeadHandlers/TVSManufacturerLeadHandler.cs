using Bikewale.RabbitMq.LeadProcessingConsumer.Entities;
using Consumer;
using System;
using System.Net.Http;

namespace Bikewale.RabbitMq.LeadProcessingConsumer
{
    /// <summary>
    /// Created by  :   Sumit Kate on 05 Jul 2017
    /// Description :   TVS Manufacturer Lead Handler
    /// </summary>
    internal class TVSManufacturerLeadHandler : ManufacturerLeadHandler
    {
        /// <summary>
        /// Type initializer
        /// </summary>
        /// <param name="manufacturerId"></param>
        /// <param name="urlAPI"></param>
        /// <param name="isAPIEnabled"></param>
        public TVSManufacturerLeadHandler(uint manufacturerId, string urlAPI, bool isAPIEnabled) : base(manufacturerId, urlAPI, isAPIEnabled)
        {
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 05 Jul 2017
        /// Description :   Process TVS Manufacturer Lead
        /// </summary>
        /// <param name="leadEntity"></param>
        /// <returns></returns>
        public override bool Process(ManufacturerLeadEntityBase leadEntity)
        {
            return base.Process(leadEntity);
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 05 Jul 2017
        /// Description :   Push Lead To Manufacturer using API
        /// </summary>
        /// <param name="leadEntity"></param>
        /// <returns></returns>
        protected override string PushLeadToManufacturer(ManufacturerLeadEntityBase leadEntity)
        {
            string response = string.Empty;
            try
            {

                BikeVersionAndCityDetails versionAndCityDetails = base.LeadRepostiory.GetVersionAndCityDetails(leadEntity.VersionId, leadEntity.CityId);
                string cityName = versionAndCityDetails != null ? versionAndCityDetails.CityName : leadEntity.CityId.ToString();
                string apiParams = string.Format("?name={0}&mob={1}&email={2}&loc={3}", leadEntity.CustomerName, leadEntity.CustomerMobile, string.IsNullOrEmpty(leadEntity.CustomerEmail) ? "NA": leadEntity.CustomerEmail, cityName);
                using (HttpClient _httpClient = new HttpClient())
                {

                    string leadURL = String.Format("{0}{1}", base.APIUrl, apiParams);
                    Logs.WriteInfoLog(String.Format("TVS Request : {0}", leadURL));
                    using (HttpResponseMessage _response = _httpClient.GetAsync(leadURL).Result)
                    {
                        if (_response.IsSuccessStatusCode && _response.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            //Check 200 OK Status      
                            response = _response.Content.ReadAsStringAsync().Result;
                            _response.Content.Dispose();
                            _response.Content = null;
                        }
                    }
                }

                if (string.IsNullOrEmpty(response))
                {
                    response = "Null response recieved from TVS manufacturer.";
                }
                Logs.WriteInfoLog(String.Format("TVS Response : {0}", response));
            }
            catch (Exception ex)
            {
                Logs.WriteErrorLog(String.Format("PushLeadToGaadi : {0}", ex.Message));
            }
            return response;
        }
    }
}
