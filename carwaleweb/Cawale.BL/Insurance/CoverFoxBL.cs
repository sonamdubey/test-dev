using Carwale.Entity.Insurance;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Carwale.Interfaces.Insurance;
using Carwale.Entity;
using AEPLCore.Cache;
using Carwale.BL.PolicyBoss;
using Carwale.DAL.Insurance;
using System.Text.RegularExpressions;
using System.Web;
using Carwale.DTOs.Insurance;
using System.Globalization;
using Carwale.Entity.Enum;
using Carwale.Notifications;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net;
using System.Security.Cryptography;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;
using Carwale.Entity.Insurance.Coverfox;
using AutoMapper;
using System.Text;

namespace Carwale.BL.Insurance
{
    public class Coverfox : IInsurance
    {
        protected readonly CacheManager _cache;
        public static string CoverFoxHost = System.Configuration.ConfigurationManager.AppSettings["coverfoxhost"] ?? "http://api.uat.coverfox.com"; //"https://www.coverfox.com"
        public static string CoverFoxLeadHost = System.Configuration.ConfigurationManager.AppSettings["coverfoxleadhost"] ?? "http://alfred.coverfox.com";//"http://jarvis.coverfox.com"
        public static string authid = System.Configuration.ConfigurationManager.AppSettings["authid"] ?? "p1IbCuYOBuQQpc4IxfJq3Tzb6iJGvTcv8UTfm7DS";
        public static string authsecret = System.Configuration.ConfigurationManager.AppSettings["authsecret"] ?? "YGsaey3BiybNedCjUdhc2p8eiL4vwhxta7xLS4RrYWPYo6jmQ0rDpJt6XITroxvwoAsC8E39y0OD7FGU53BwPSX3cbrJizoBhSWVRkrQ0SgBtvk1xA4FPtcfU8zZ5bIm";

        public static string client_id = System.Configuration.ConfigurationManager.AppSettings["coverfoxclientid"] ?? "53ba4a624658bdfd11a6";
        public static string client_secret = System.Configuration.ConfigurationManager.AppSettings["coverfoxclientsecret"] ?? "eaf20783e4f7e428a96b8f6b5548ac140ea72786";
        public static string username = System.Configuration.ConfigurationManager.AppSettings["coverfoxuser"] ?? "carwale";
        public static string password = System.Configuration.ConfigurationManager.AppSettings["coverfoxpass"] ?? "carwale123";

        public static List<int> coverfoxCities = (System.Configuration.ConfigurationManager.AppSettings["coverfoxcities"] ?? "-2").Split(',').Select(i => int.Parse(i)).ToList();
        public static List<int> coverfoxStates = (System.Configuration.ConfigurationManager.AppSettings["coverfoxstates"] ?? "-2").Split(',').Select(i => int.Parse(i)).ToList();

        public Coverfox()
        {
            _cache = new CacheManager();
        }

        public QuotationDto SubmitLead(InsuranceLead inputs)
        {
            QuotationDto dto = null;

            try
            {
                inputs.Platform = inputs.Platform;
                double premiumAmount = 0;
                IInsuranceRepository objInsurance = new InsuranceRepository();

                //inputs.LeadSource = (int)inputs.Platform;

                var cookieObj = HttpContext.Current.Request.Cookies["_CustCityIdMaster"];
                if (cookieObj != null && Utility.RegExValidations.IsNumeric(cookieObj.Value))
                {
                    inputs.CityId = Convert.ToInt32(cookieObj.Value);
                }

                inputs.InsuranceLeadId = objInsurance.SaveLead(inputs);

                dto = new QuotationDto();
                LeadResponse leadResponse = new LeadResponse() { Success = false };
                if (!string.IsNullOrWhiteSpace(inputs.Mobile))
                {
                    try
                    {
                        leadResponse = JsonConvert.DeserializeObject<LeadResponse>(PostLeadAPI(inputs.Mobile));
                    }
                    catch (Exception ex)
                    {
                        ExceptionHandler objErr = new ExceptionHandler(ex, "Coverfox.SubmitLead()leadResponse");
                        objErr.LogException();
                    }
                    dto.ConfirmationStatus = leadResponse == null ? "False" : leadResponse.Success.ToString();
                }
                else
                    dto.ConfirmationStatus = "failure";

                string[] registrationNumber = inputs.StateName.Split('-');
                string newPolicyStartDate = DateTime.Now.AddDays(7).ToString("dd-MM-yyyy");//"23-04-2016";
                string pastPolicyExpiryDate = DateTime.Now.AddDays(6).ToString("dd-MM-yyyy");//"22-04-2016";
                string registrationDate = inputs.InsuranceNew ? DateTime.Now.ToString("dd-MM-yyyy") : "01-01-" + inputs.CarManufactureYear;
                string manufacturingDate = "01-01-" + inputs.CarManufactureYear;

                string submitJson = "{\"idvElectrical\":\"0\",\"addon_isDepreciationWaiver\":\"0\",\"expiry_error\":\"false\",\"addon_is247RoadsideAssistance\":\"0\",\"isNCBCertificate\":\"0\",\"extra_paPassenger\":\"0\",\"cngKitValue\":\"0\",\"payment_mode\":\"PAYMENT_GATEWAY\",\"addon_isInvoiceCover\":\"0\",\"addon_isDriveThroughProtected\":\"0\",\"extra_user_dob\":\"\",\"quoteId\":\"\",\"isCNGFitted\":\"0\",\"isUsedVehicle\":\"0\",\"isNewVehicle\":\"1\",\"extra_isAntiTheftFitted\":\"0\",\"extra_isTPPDDiscount\":\"0\",\"voluntaryDeductible\":\"0\",\"idvNonElectrical\":\"0\",\"expirySlot\":\"7\",\"idv\":\"0\",\"isClaimedLastYear\":\"0\",\"discountCode\":\"\",\"extra_isLegalLiability\":\"0\",\"registrationNumber[]\":[\"" + registrationNumber[0].Trim() + "\",\"" + registrationNumber[1].Trim() + "\",\"0\",\"0\"],\"newNCB\":\"" + inputs.NCBPercent + "\",\"previousNCB\":\"0\",\"addon_isEngineProtector\":\"0\",\"manufacturingDate\":\"" + manufacturingDate + "\",\"newPolicyStartDate\":\"" + newPolicyStartDate + "\",\"vehicleId\":\"" + inputs.VersionId + "\",\"addon_isKeyReplacement\":\"0\",\"extra_isMemberOfAutoAssociation\":\"0\",\"addon_isNcbProtection\":\"0\",\"pastPolicyExpiryDate\":\"" + pastPolicyExpiryDate + "\",\"registrationDate\":\"" + registrationDate + "\"}";
                string responseJson = null;
                try
                {
                    responseJson = PostAPIResponse("/apis/motor/" + (inputs.Application == Application.BikeWale ? "twowheeler" : "fourwheeler") + "/quotes/", submitJson);
                }
                catch (Exception ex)
                {
                    ExceptionHandler objErr = new ExceptionHandler(ex, "Coverfox.SubmitLead()QuoteResponse");
                    objErr.LogException();
                }
                QuoteResponse response = responseJson == null ? new QuoteResponse() : JsonConvert.DeserializeObject<QuoteResponse>(responseJson);

                if (response.Data.Quotes.Count > 0) premiumAmount = response.Data.Quotes.Min().Premium;
                else premiumAmount = 0;

                dto.UniqueId = inputs.InsuranceLeadId;

                dto.Quotation = premiumAmount.ToString();//min premium;

                inputs.ApiResponse = dto.ConfirmationStatus.ToLower() + "|" + response.Data.QuoteId ?? "null";//##quoteID + success(bool)

                objInsurance.UpdateLeadResponse(inputs.InsuranceLeadId, inputs.ApiResponse, premiumAmount);
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "Coverfox.SubmitLead() cityname : " + inputs.StateName ?? "NULL" + " version :" + inputs.VersionId + " mobile: " + inputs.Mobile ?? "NULL");
                objErr.LogException();
            }
            return dto;
        }

        public List<MakeEntity> GetMakes(Application application)
        {
            throw new NotImplementedException();
        }

        public List<ModelBase> GetModels(int makeId, Application application)
        {
            return _cache.GetFromCache<List<ModelBase>>("CoverFoxModels" + application, new TimeSpan(1, 0, 0, 0), () => GetModelsFromCoverFox(makeId, application));
        }

        public List<VersionBase> GetVersions(int modelId, Application application)
        {
            return _cache.GetFromCache<List<VersionBase>>("CoverFoxVersions_" + modelId + "_" + application, new TimeSpan(1, 0, 0, 0), () => GetVersionsFromCoverFox(modelId, application));
        }

        public List<InsuranceCity> GetCities(Application application)
        {
            return _cache.GetFromCache<List<InsuranceCity>>("CoverFoxRTOs_" + application, new TimeSpan(30, 0, 0, 0), () => GetCitiesFromCoverFox(application));
        }

        List<MakeEntity> GetMakesFromPolicyBoss(Application application)
        {
            throw new NotImplementedException();
        }

        List<ModelBase> GetModelsFromCoverFox(int makeId, Application application)
        {
            List<ModelBase> retValue = null;
            string modelJson = PostAPIResponse("/apis/motor/" + (application == Application.BikeWale ? "twowheeler" : "fourwheeler") + "/vehicles/");
            try
            {
                var response = JsonConvert.DeserializeObject<ModelsResponse>(modelJson);
                retValue = Mapper.Map<List<Model>, List<ModelBase>>(response.Data.Models);
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "Coverfox.GetModelsFromCoverFox() makeid=" + makeId + "|" + application.ToString());
                objErr.LogException();
            }
            return retValue;
        }

        List<VersionBase> GetVersionsFromCoverFox(int modelId, Application application)
        {
            List<VersionBase> retValue = null;

            string variantJSON = PostAPIResponse("/apis/motor/" + (application == Application.BikeWale ? "twowheeler" : "fourwheeler") + "/vehicles/" + modelId + "/variants/");

            try
            {
                var response = JsonConvert.DeserializeObject<VersionsResponse>(variantJSON);
                retValue = Mapper.Map<List<Carwale.Entity.Insurance.Coverfox.Version>, List<VersionBase>>(response.Data.Versions);
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "Coverfox.GetVersionsFromCoverFox() modelId=" + modelId + "|" + application.ToString());
                objErr.LogException();
            }
            return retValue;
        }

        List<InsuranceCity> GetCitiesFromCoverFox(Application application)
        {
            List<InsuranceCity> retValue = null;

            string rtoJSON = PostAPIResponse("/apis/motor/" + (application == Application.BikeWale ? "twowheeler" : "fourwheeler") + "/rtos/");

            try
            {
                var response = JsonConvert.DeserializeObject<RTOsResponse>(rtoJSON);
                retValue = Mapper.Map<List<Carwale.Entity.Insurance.Coverfox.RTO>, List<InsuranceCity>>(response.Data.RTOs);
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "Coverfox.GetCitiesFromCoverFox() app=" + application.ToString());
                objErr.LogException();
            }
            return retValue;
        }

        public string PostAPIResponse(string path, string JSON = null, string leadpushtoken = null)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {

                    WebRequest request = HttpWebRequest.Create((leadpushtoken == null ? CoverFoxHost : CoverFoxLeadHost) + path);
                    request.Method = "POST";

                    if (leadpushtoken == null)
                    {
                        string date = string.Concat(DateTime.UtcNow.ToString("s"), "Z");

                        MethodInfo priMethod = request.Headers.GetType().GetMethod("AddWithoutValidate", BindingFlags.Instance | BindingFlags.NonPublic);
                        priMethod.Invoke(request.Headers, new[] { "Date", date });

                        var header = GetAuthHeaders(path, date);
                        request.Headers.Add("Authorization", header);
                    }
                    else request.Headers.Add("Authorization", "Bearer " + leadpushtoken);

                    if (JSON != null && !string.IsNullOrWhiteSpace(JSON))
                    {
                        ASCIIEncoding encoding = new ASCIIEncoding();

                        string jsonData = JSON;

                        byte[] data = encoding.GetBytes(jsonData);

                        request.ContentType = "application/json";
                        request.ContentLength = data.Length;

                        Stream newStream = request.GetRequestStream();
                        newStream.Write(data, 0, data.Length);
                        newStream.Close();
                    }

                    WebResponse response = request.GetResponse();

                    Stream dataStream = response.GetResponseStream();
                    StreamReader reader = new StreamReader(dataStream);
                    string responseFromServer = reader.ReadToEnd();

                    return responseFromServer;

                }
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "Coverfox.POST() inputs=" + path ?? "NULL" + "|" + JSON ?? "NULL" + "|" + leadpushtoken ?? "NULL");
                objErr.LogException();
            }
            return null;
        }

        public string GetAuthHeaders(string path, string date)
        {
            string signature;
            string message = path + " " + date;
            string secret = authsecret;

            secret = secret ?? "";
            var encoding = new System.Text.ASCIIEncoding();
            byte[] keyByte = encoding.GetBytes(secret);
            byte[] messageBytes = encoding.GetBytes(message);
            using (var hmacsha1 = new HMACSHA1(keyByte))
            {
                byte[] hashmessage = hmacsha1.ComputeHash(messageBytes);
                signature = Convert.ToBase64String(hashmessage);
            }

            signature = "CVFX" + " " + authid + ":" + signature;
            return (signature);
        }

        public string PostLeadAPI(string mobile)
        {
            string tokenfromcache = null, newtokenResponse = null, token = null;
            try
            {
                token = _cache.GetFromCache<string>("coverfoxtoken");
                tokenfromcache = token;
                string time = token == null ? "" : token.Split('|')[1];

                if (token == null || (DateTime.Now - DateTime.Parse(time)).TotalSeconds >= 86400)
                {
                    //refresh token
                    string createJson = "{\"client_id\": \"" + client_id + "\", \"client_secret\":\"" + client_secret + "\", \"grant_type\": \"password\",\"username\": \"" + username + "\", \"password\": \"" + password + "\"}";
                    newtokenResponse = PostAPIResponse("/oauth2/token/", createJson, "n/a");

                    TokenResponse obj = JsonConvert.DeserializeObject<TokenResponse>(newtokenResponse);
                    token = obj.AccessToken;

                    if (token != null)
                    {
                        _cache.ExpireCache("coverfoxtoken");
                        _cache.GetFromCache("coverfoxtoken", new TimeSpan(1, 0, 0, 0), () => (token + "|" + DateTime.Now.ToString() + "|"));
                    }
                }
                else token = token.Split('|')[0];
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "Coverfox.PostLeadAPI() inputs=" + mobile ?? "NULL" + "|" + DateTime.Now.ToString() + "|" + tokenfromcache ?? "NULL" + "|" + newtokenResponse ?? "NULL");
                objErr.LogException();
            }
            //push token
            string postJson = null;
            postJson = "{\"mobile\": \"" + mobile + "\", \"campaign\":\"motor-affiliate\", \"" + "ad" + "-" + "category" + "\": \"carwale\", \"network\": \"affiliate\" }";
            return PostAPIResponse("/affiliate-leads/", postJson, token);
        }


        public InsuranceResponse SubmitLeadV2(InsuranceLead inputs)
        {
            throw new NotImplementedException();
        }
    }
}

