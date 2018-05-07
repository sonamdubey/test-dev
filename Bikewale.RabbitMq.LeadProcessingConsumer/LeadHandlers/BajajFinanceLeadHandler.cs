using Consumer;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace Bikewale.RabbitMq.LeadProcessingConsumer
{
    /// <summary>
    /// Created by  :   Sumit Kate on 05 Jul 2017
    /// Description :   Bajaj Finance Lead Handler
    /// </summary>
    internal class BajajFinanceLeadHandler : ManufacturerLeadHandler
    {

        /// <summary>
        /// Type initializer
        /// </summary>
        /// <param name="manufacturerId"></param>
        /// <param name="urlAPI"></param>
        /// <param name="isAPIEnabled"></param>
        public BajajFinanceLeadHandler(uint manufacturerId, string urlAPI, bool isAPIEnabled) : base(manufacturerId, urlAPI, isAPIEnabled)
        {
        }

        /// <summary>
        /// Type initializer
        /// </summary>
        /// <param name="leadEntity"></param>
        /// <returns></returns>
        public override bool Process(ManufacturerLeadEntityBase leadEntity)
        {
            return base.Process(leadEntity);
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 05 Jul 2017
        /// Description :   Push Lead To Manufacturer
        /// </summary>
        /// <param name="leadEntity"></param>
        /// <returns></returns>
        protected override string PushLeadToManufacturer(ManufacturerLeadEntityBase leadEntity)
        {
            string response = string.Empty;
            try
            {
                //get Bike mapping info
                BajajFinanceLeadEntity bikeMappingInfo = base.LeadRepostiory.GetBajajFinanceBikeMappingInfo(leadEntity.VersionId, leadEntity.PinCodeId);
                if (bikeMappingInfo != null && !string.IsNullOrEmpty(bikeMappingInfo.Model) && !string.IsNullOrEmpty(bikeMappingInfo.City))
                {
                    if (!string.IsNullOrEmpty(leadEntity.CustomerName))
                    {
                        leadEntity.CustomerName = leadEntity.CustomerName.Trim();
                        int spaceIndex = leadEntity.CustomerName.IndexOf(" ");
                        if (spaceIndex > 0)
                        {
                            bikeMappingInfo.FirstName = leadEntity.CustomerName.Substring(0, spaceIndex);
                            bikeMappingInfo.LastName = leadEntity.CustomerName.Substring(spaceIndex + 1);
                        }
                        else
                        {
                            bikeMappingInfo.FirstName = leadEntity.CustomerName;
                        }
                    }

                    bikeMappingInfo.MobileNo = leadEntity.CustomerMobile;
                    bikeMappingInfo.EmailID = leadEntity.CustomerEmail;

                    BajajFinanceLeadInput bajajLeadInput = new BajajFinanceLeadInput()
                    {
                        leadData = new List<BajajFinanceLeadEntity>() { bikeMappingInfo }
                    };

                    using (HttpClient _httpClient = new HttpClient())
                    {
                        string jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(bajajLeadInput);
                        HttpContent httpContent = new StringContent(jsonString);

                        Logs.WriteInfoLog(String.Format("Bajaj Finance API Request : {0}", jsonString));

                        using (HttpResponseMessage _response = _httpClient.PostAsync(base.APIUrl, httpContent).Result)
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
                            response = "Null response recieved from Bajaj Finance API.";
                        }
                        Logs.WriteInfoLog(String.Format("Bajaj Finance API Response : {0}", response));
                    }
                }
                else
                {
                    Logs.WriteInfoLog(String.Format("Failed to push Bajaj Finance Lead - {0} Input", Newtonsoft.Json.JsonConvert.SerializeObject(bikeMappingInfo)));
                }
            }
            catch (Exception ex)
            {
                Logs.WriteErrorLog(String.Format("PushLeadToBajajFinance : {0}", ex.Message));
            }
            return response;
        }
    }
}
