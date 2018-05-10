﻿
using Bikewale.RabbitMq.LeadProcessingConsumer.Cache;
using Bikewale.RabbitMq.LeadProcessingConsumer.Interface;
using Consumer;
using System;
using System.Collections;
using System.Net.Http;

namespace Bikewale.RabbitMq.LeadProcessingConsumer
{
    /// <summary>
    /// Created by  :   Sumit Kate on 05 Jul 2017
    /// Description :   Honda Manufacturer Lead Handler
    /// </summary>
    internal class HondaManufacturerLeadHandler : ManufacturerLeadHandler
    {
        private readonly IHondaManufacturerCache _hondaModels;
		private Hashtable hondaModels;
		/// <summary>
		/// Type initializer
		/// </summary>
		/// <param name="manufacturerId"></param>
		/// <param name="urlAPI"></param>
		/// <param name="isAPIEnabled"></param>
		public HondaManufacturerLeadHandler(uint manufacturerId, string urlAPI, bool isAPIEnabled) : base(manufacturerId, urlAPI, isAPIEnabled)
        {
            _hondaModels = new HondaModelCacheRepository();
			hondaModels = _hondaModels.GetHondaModelMapping();
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
        /// Modified by :   Pratibha Verma on 4 April 2018
        /// Description :   pass mapped model name in api
        /// </summary>
        /// <param name="leadEntity"></param>
        /// <returns></returns>
        protected override string PushLeadToManufacturer(ManufacturerLeadEntityBase leadEntity)
        {
            string leadURL = string.Empty;
            string response = string.Empty;
            string apiModelName = string.Empty;
            GaadiLeadEntity gaadiLead = null;
            try
            {

                BikeQuotationEntity quotation = base.LeadRepostiory.GetPriceQuoteById(leadEntity.PQId);
                if (quotation != null)
                {
                    if (hondaModels != null && hondaModels.ContainsKey((int)quotation.ModelId))
                    {
                        apiModelName = Convert.ToString(hondaModels[(int)quotation.ModelId]);
                    }
                    else
                    {
                        apiModelName = quotation.ModelName;
                    }

                    gaadiLead = new GaadiLeadEntity()
                    {
                        City = quotation.City,
                        Email = leadEntity.CustomerEmail,
                        Make = quotation.MakeName,
                        Mobile = leadEntity.CustomerMobile,
                        Model = apiModelName,
                        Name = leadEntity.CustomerName,
                        State = quotation.State
                    };
                }

                using (HttpClient _httpClient = new HttpClient())
                {
                    string jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(gaadiLead);
                    byte[] toEncodeAsBytes = System.Text.ASCIIEncoding.ASCII.GetBytes(jsonString);
                    leadURL = String.Format("{0}{1}", base.APIUrl, System.Convert.ToBase64String(toEncodeAsBytes));
                    Logs.WriteInfoLog(String.Format("Honda Request : {0}", leadURL));
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
                    response = "Null response recieved from Honda manufacturer.";
                }
                Logs.WriteInfoLog(String.Format("Honda Response : {0}", response));
            }
            catch (Exception ex)
            {
                Logs.WriteErrorLog(String.Format("PushLeadToGaadi : {0}", ex.Message));
            }
            return response;
        }
    }
}
