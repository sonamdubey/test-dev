using AEPLCore.Utils.JsonHelper;
using Carwale.Entity.Classified.Leads;
using Carwale.Entity.Classified.SellCar;
using Carwale.Interfaces.Classified.MyListings;
using Carwale.Notifications.Logs;
using Dapper;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.IO;

namespace Carwale.DAL.Classified.MyListings
{
    public class MyListingsRepository : RepositoryBase, IMyListingsRepository
    {
        private readonly string _action = ConfigurationManager.AppSettings["C2BContactedLeadsApiAction"];
        private readonly string _apikey = ConfigurationManager.AppSettings["C2BContactedLeadsApiKey"];
        private readonly string _apiUrl = ConfigurationManager.AppSettings["C2BContactedLeadsApiUrl"];
        private readonly string _apiCartradeLeads = ConfigurationManager.AppSettings["CarTradeLeadsApiUrl"];
        private readonly string _actionCartradeLeads = ConfigurationManager.AppSettings["CarTradeLeadsApiAction"];
        private readonly string _keyCartradeLeads = ConfigurationManager.AppSettings["CarTradeLeadsApiKey"];
        private static HttpClient _httpClient = new HttpClient();
        private enum Source
        {
            Carwale = 2
        }

        public List<CustomerSellInquiry> GetListingsByMobile(string mobileNumber)
        {
            List<CustomerSellInquiry> custSellInquiries = null;
            try
            {
                using (IDbConnection con = ClassifiedMySqlReadConnection)
                {
                    var param = new DynamicParameters();
                    param.Add("v_mobile", mobileNumber);
                    custSellInquiries = con.Query<CustomerSellInquiry>("GetCustomerSellInquiries_18_4_8", param, commandType: CommandType.StoredProcedure).ToList();
                }
            }
            catch (MySqlException ex)
            {
                Logger.LogException(ex);
            }
            return custSellInquiries;

        }

        public string GetCustomerKey(int inquiryId)
        {
            string customerKey = "";
            try
            {
                using (IDbConnection con = ClassifiedMySqlReadConnection)
                {
                    var param = new DynamicParameters();
                    param.Add("v_inquiryId", inquiryId);
                    customerKey = con.Query<string>("GetCustomerKeyById", param, commandType: CommandType.StoredProcedure).FirstOrDefault();
                }
            }
            catch (MySqlException ex)
            {
                Logger.LogException(ex);
            }
            return customerKey;
        }


        public C2BLeadResponse GetC2BLeads(int inquiryId)
        {
            var requestWithInformation = new List<KeyValuePair<string, string>>();
            requestWithInformation.Add(new KeyValuePair<string, string>("listing_id", inquiryId.ToString()));
            requestWithInformation.Add(new KeyValuePair<string, string>("source", ((int)Source.Carwale).ToString()));
            requestWithInformation.Add(new KeyValuePair<string, string>("action", _action));
            requestWithInformation.Add(new KeyValuePair<string, string>("api_key", _apikey));
            var request = string.Join("||", requestWithInformation.Select(kvp => kvp.Key + ":" + kvp.Value));
            HttpResponseMessage response = new HttpResponseMessage();
            C2BLeadResponse c2bLeadResponse = new C2BLeadResponse();
            try
            {
                response = _httpClient.PostAsync(new System.Uri(_apiUrl), new FormUrlEncodedContent(requestWithInformation)).Result;
                {
                    if (response != null && response.Content != null && response.StatusCode == HttpStatusCode.OK)
                    {
                        using (var responseStream = response.Content.ReadAsStreamAsync().Result)
                        {
                            c2bLeadResponse = Deserializer<C2BLeadResponse>.DeserializeFromStream(responseStream);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex, " for Request: " + request + " Response: " + response.Content ?? response.Content.ReadAsStringAsync().Result);
            }
            finally
            {
                response.Dispose();
            }
            return c2bLeadResponse;
        }

        public CarTradeLeadResponse GetCarTradeLeads(int inquiryId)
        {
            var requestWithInformation = new List<KeyValuePair<string, string>>();
            requestWithInformation.Add(new KeyValuePair<string, string>("action", _actionCartradeLeads));
            requestWithInformation.Add(new KeyValuePair<string, string>("listing_id", inquiryId.ToString()));
            requestWithInformation.Add(new KeyValuePair<string, string>("api_key", _keyCartradeLeads));
            var request = string.Join("||", requestWithInformation.Select(kvp => kvp.Key + ":" + kvp.Value));
            HttpResponseMessage response = new HttpResponseMessage();
            CarTradeLeadResponse carTradeLeadResponse = new CarTradeLeadResponse();
            try
            {
                response = _httpClient.PostAsync(new System.Uri(_apiCartradeLeads), new FormUrlEncodedContent(requestWithInformation)).Result;
                if (response != null && response.Content != null && response.StatusCode == HttpStatusCode.OK)
                {
                    using (var responseStream = response.Content.ReadAsStreamAsync().Result)
                    {
                        carTradeLeadResponse = Deserializer<CarTradeLeadResponse>.DeserializeFromStream(responseStream);
                    }
                }

            }
            catch (Exception ex)
            {
                Logger.LogException(ex, " for Request: " + request + " Response: " + response.Content ?? response.Content.ReadAsStringAsync().Result);
            }
            finally
            {
                response.Dispose();
            }
            return carTradeLeadResponse;
        }


        public string GetMobileOfActiveInquiry(int inquiryId)
        {
            string mobile = string.Empty;
            try
            {
                using (var con = ClassifiedMySqlReadConnection)
                {
                    string searchQuery = "select customermobile from customersellinquiries where id=@inquiryId and isarchived=0;";
                    mobile = con.Query<string>(searchQuery, new { inquiryId }).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return mobile;
        }

        public bool IsCarCustomerEditable(int inquiryId)
        {
            bool isCarCustomerEditable = false;
            try
            {
                using (var con = ClassifiedMySqlReadConnection)
                {
                    string searchQuery = "select is_customer_editable from customersellinquiries where id=@inquiryId and isarchived=0;";
                    string result = con.Query<string>(searchQuery, new { inquiryId }).FirstOrDefault();
                    isCarCustomerEditable = string.IsNullOrWhiteSpace(result) || Convert.ToBoolean(result);
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return isCarCustomerEditable;
        }
    }
}
