using Consumer;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;

namespace Bikewale.RabbitMq.LeadProcessingConsumer
{
    /// <summary>
    /// Created by  :   Sumit Kate on 05 Jul 2017
    /// Description :   Tata Capital Lead Handler
    /// </summary>
    internal class TataCapitalLeadHandler : ManufacturerLeadHandler
    {


        /// <summary>
        /// Type Initializer
        /// </summary>
        /// <param name="manufacturerId"></param>
        /// <param name="urlAPI"></param>
        /// <param name="isAPIEnabled"></param>
        public TataCapitalLeadHandler(uint manufacturerId, string urlAPI, bool isAPIEnabled) : base(manufacturerId, urlAPI, isAPIEnabled)
        {
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 05 Jul 2017
        /// Description :   Process Tata Capital Lead
        /// </summary>
        /// <param name="leadEntity"></param>
        /// <returns></returns>
        public override bool Process(ManufacturerLeadEntityBase leadEntity)
        {
            return base.Process(leadEntity);
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 05 Jul 2017
        /// Description :   Override Push Lead To Manufacturer using Tata Capital API
        /// </summary>
        /// <param name="leadEntity"></param>
        /// <returns></returns>
        protected override string PushLeadToManufacturer(ManufacturerLeadEntityBase leadEntity)
        {
            string response = string.Empty;
            TataCapitalInputEntity tataLeadInput = null;
            try
            {

                #region create firstName and Lastname

                string fullName = leadEntity.CustomerName.Trim();
                fullName = Regex.Replace(fullName, @"[^a-zA-Z\s]", string.Empty);
                string firstName = string.Empty, lastName = string.Empty;
                if (fullName.Contains(" "))
                {
                    int spaceStart = fullName.IndexOf(' ');
                    firstName = fullName.Substring(0, spaceStart);
                    lastName = fullName.Substring(spaceStart + 1);
                }
                else
                {
                    firstName = fullName;
                    lastName = fullName;
                }
                #endregion

                #region Fetch city and log API inputs

                string tataCapitalCityId = base.LeadRepostiory.GetTataCapitalByCityId(leadEntity.CityId);
                tataLeadInput = new TataCapitalInputEntity()
                {
                    fname = firstName.Trim(),
                    lname = lastName.Trim(),
                    resEmailId = leadEntity.CustomerEmail,
                    resMobNo = leadEntity.CustomerMobile,
                    resCity = tataCapitalCityId
                };
                #endregion

                #region API call

                using (HttpClient _httpClient = new HttpClient())
                {
                    string jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(tataLeadInput);


                    HttpContent httpContent = new StringContent(jsonString);
                    httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                    Logs.WriteInfoLog(String.Format("Tata Capital Request : {0}", jsonString));

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
                    if (String.IsNullOrEmpty(response))
                    {
                        response = "Null response received from Tata capital";
                    }
                    Logs.WriteInfoLog(String.Format("Tata Capital Response : {0}", response));
                }
                #endregion
            }
            catch (Exception ex)
            {
                Logs.WriteErrorLog(String.Format("Failed to push Tata Capital lead : {0}", ex.Message));
            }
            return response;
        }
    }
}
