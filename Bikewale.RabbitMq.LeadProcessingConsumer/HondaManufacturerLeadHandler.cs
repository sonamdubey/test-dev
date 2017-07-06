﻿using Consumer;
using System;
using System.Net.Http;

namespace Bikewale.RabbitMq.LeadProcessingConsumer
{
    /// <summary>
    /// Created by  :   Sumit Kate on 05 Jul 2017
    /// Description :   Honda Manufacturer Lead Handler
    /// </summary>
    internal class HondaManufacturerLeadHandler : ManufacturerLeadHandler
    {
        private HttpClient _httpClient;
        /// <summary>
        /// Type initializer
        /// </summary>
        /// <param name="manufacturerId"></param>
        /// <param name="urlAPI"></param>
        /// <param name="isAPIEnabled"></param>
        public HondaManufacturerLeadHandler(uint manufacturerId, string urlAPI, bool isAPIEnabled) : base(manufacturerId, urlAPI, isAPIEnabled)
        {
            _httpClient = new HttpClient();
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 05 Jul 2017
        /// Description :   Process Honda Manufacturer Lead
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
            string leadURL = string.Empty;
            string response = string.Empty;
            try
            {

                BikeQuotationEntity quotation = base.LeadRepostiory.GetPriceQuoteById(leadEntity.PQId);

                GaadiLeadEntity gaadiLead = new GaadiLeadEntity()
                {
                    City = quotation.City,
                    Email = leadEntity.CustomerEmail,
                    Make = quotation.MakeName,
                    Mobile = leadEntity.CustomerMobile,
                    Model = quotation.ModelName,
                    Name = leadEntity.CustomerName,
                    State = quotation.State
                };

                if (_httpClient != null)
                {
                    string jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(gaadiLead);
                    byte[] toEncodeAsBytes = System.Text.ASCIIEncoding.ASCII.GetBytes(jsonString);
                    leadURL = String.Format("{0}{1}", base.APIUrl, System.Convert.ToBase64String(toEncodeAsBytes));
                    Logs.WriteInfoLog(String.Format("Honda Request : {0}", leadURL));
                    using (HttpResponseMessage _response = _httpClient.GetAsync(leadURL).Result)
                    {
                        if (_response.IsSuccessStatusCode)
                        {
                            if (_response.StatusCode == System.Net.HttpStatusCode.OK) //Check 200 OK Status        
                            {
                                response = _response.Content.ReadAsStringAsync().Result;
                                _response.Content.Dispose();
                                _response.Content = null;
                            }
                        }
                    }
                    if (string.IsNullOrEmpty(response))
                    {
                        response = "Null response recieved from Honda manufacturer.";
                    }
                    Logs.WriteInfoLog(String.Format("Honda Response : {0}", response));
                }
            }
            catch (Exception ex)
            {
                Logs.WriteErrorLog(String.Format("PushLeadToGaadi : {0}", ex.Message));
            }
            return response;
        }
    }
}
