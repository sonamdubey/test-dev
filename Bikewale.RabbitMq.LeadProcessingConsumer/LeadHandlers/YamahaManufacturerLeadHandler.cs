

using Bikewale.RabbitMq.LeadProcessingConsumer.Entities;
using Consumer;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Bikewale.RabbitMq.LeadProcessingConsumer.LeadHandlers
{
    /// <summary>
    /// Created By  : Pratibha Verma on 18 June 2018
    /// Description : Yamaha Manufacturer Lead handler
    /// </summary>
    internal class YamahaManufacturerLeadHandler : ManufacturerLeadHandler
    {
        private Hashtable yamahaModels;
        /// <summary>
        /// Type initializer
        /// </summary>
        /// <param name="manufacturerId"></param>
        /// <param name="urlAPI"></param>
        /// <param name="isAPIEnabled"></param>
        public YamahaManufacturerLeadHandler(uint manufacturerId, string urlAPI, bool isAPIEnabled) : base(manufacturerId, urlAPI, isAPIEnabled)
        {
            yamahaModels = base.LeadRepostiory.GetYamahaModelMapping();
        }

        /// <summary>
        /// Created by  : Pratibha Verma on 18 June 2018
        /// Description : Process Yamaha Manufacturer Lead
        /// </summary>
        /// <param name="leadEntity"></param>
        /// <returns></returns>
        public override bool Process(ManufacturerLeadEntityBase leadEntity)
        {
            return base.Process(leadEntity);
        }

        /// <summary>
        /// Created by  : Pratibha Verma on 18 June 2018
        /// Description : Push Laed to Yamaha Manufacturer using API
        /// </summary>
        /// <param name="leadEntity"></param>
        /// <returns></returns>
        protected override string PushLeadToManufacturer(ManufacturerLeadEntityBase leadEntity)
        {
            string response = string.Empty;
            YamahaLeadEntity yamahaEntity = null;
            try
            {
                BikeVersionAndCityDetails versionAndCityDetails = base.LeadRepostiory.GetVersionAndCityDetails(leadEntity.VersionId, leadEntity.CityId);
                DealerEntity dealer = base.LeadRepostiory.GetDealerInfoById(leadEntity.ManufacturerDealerId);
                string mappedModelName = string.Empty;
                if (versionAndCityDetails != null && dealer != null)
                {
                    if (yamahaModels != null && yamahaModels.ContainsKey((int)versionAndCityDetails.ModelId))
                    {
                        mappedModelName = Convert.ToString(yamahaModels[(int)versionAndCityDetails.ModelId]);
                    }
                    else
                    {
                        mappedModelName = versionAndCityDetails.ModelName;
                    }
                    yamahaEntity = new YamahaLeadEntity()
                    {
                        CompanyCode = "300",
                        DealerCode = dealer.DealerCode,
                        CustomerName = leadEntity.CustomerName,
                        MobileNumber = leadEntity.CustomerMobile,
                        EmailId = leadEntity.CustomerEmail,
                        PreEnquiryStatus = "10",
                        PreEnquirySource = "002",
                        ModelName = mappedModelName
                    };
                }
                using (HttpClient _httpClient = new HttpClient())
                {
                    List<YamahaLeadEntity> yamahaObj = new List<YamahaLeadEntity>();
                    yamahaObj.Add(yamahaEntity);
                    string inputJsonString = Newtonsoft.Json.JsonConvert.SerializeObject(new { data = yamahaObj });

                    HttpContent httpContent = new StringContent(inputJsonString);
                    httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    httpContent.Headers.Add("projectkey", ConfigurationManager.AppSettings["YamahaProjectKey"]);
                    httpContent.Headers.Add("apiname", ConfigurationManager.AppSettings["YamahaApiName"]);
                    httpContent.Headers.Add("x-api-key", ConfigurationManager.AppSettings["YamahaXApiKey"]);
                    httpContent.Headers.Add("y-key", ConfigurationManager.AppSettings["YamahaYKey"]);

                    Logs.WriteInfoLog(string.Format("Yamaha Request : {0}", inputJsonString));

                    using (HttpResponseMessage _response = _httpClient.PostAsync(base.APIUrl, httpContent).Result)
                    {
                        if (_response.IsSuccessStatusCode && _response.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            response = _response.Content.ReadAsStringAsync().Result;
                            _response.Content.Dispose();
                            _response.Content = null;
                        }
                    }
                }
                if (string.IsNullOrEmpty(response))
                {
                    response = "Null response recieved from Yamaha manufacturer.";
                }
                Logs.WriteInfoLog(string.Format("Yamaha Response : {0}", response));
            }
            catch (Exception ex)
            {
                Logs.WriteErrorLog(String.Format("PushLeadToYamaha : LeadId :{0}, ErrorMessage: {1}", leadEntity.LeadId, ex.Message));
            }
            return response;
        }
    }
}
