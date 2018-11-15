using Bikewale.Entities.Customer;
using Bikewale.Entities.Finance.BajajAuto;
using Bikewale.Entities.MobileVerification;
using Bikewale.Interfaces.Customer;
using Bikewale.Interfaces.Finance;
using Bikewale.Interfaces.Finance.BajajAuto;
using Bikewale.Interfaces.MobileVerification;
using Bikewale.ManufacturerCampaign.Interface;
using Bikewale.Notifications;
using Bikewale.Utility;
using log4net;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using RabbitMqPublishing;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Bikewale.BAL.Finance
{
    public class BajajAuto : IBajajAuto
    {
        private readonly IBajajAutoRepository _bajajAutoRepository;
        private readonly IManufacturerCampaignRepository _manufacturerCampaignRepository;
        private readonly ICustomer<CustomerEntity, UInt32> _customer;
        private readonly IMobileVerificationRepository _mobileVerificationRepository;
        private readonly IMobileVerificationCache _mobileVerificationCache;
        private readonly IMobileVerification _mobileVerification;
        private readonly IBajajAutoCache _bajajAutoCache;
        private readonly string BajajAutoSupplierApiUrl = Utility.BWConfiguration.Instance.BajajAutoSupplierApiUrl;
        private readonly string BajajAutoOrpApiUrl = Utility.BWConfiguration.Instance.BajajAutoOrpApiUrl;
        private readonly string BajajAutoUploadApiUrl = Utility.BWConfiguration.Instance.BajajAutoUploadApiUrl;
        private readonly IList<string> _leadResponseMsgList;
        private readonly uint BajajAutoTenure = Convert.ToUInt32(Utility.BWConfiguration.Instance.BajajAutoTenure);
        private readonly float BajajAutoRateOfInterest = Convert.ToSingle(Utility.BWConfiguration.Instance.BajajAutoRateOfInterest);
        static ILog _logger = LogManager.GetLogger("BajajFinance");

        public BajajAuto(IBajajAutoRepository bajajAutoRepository, IManufacturerCampaignRepository manufacturerCampaignRepository, ICustomer<CustomerEntity, UInt32> customer, IMobileVerificationRepository mobileVerificationRepository, IMobileVerificationCache mobileVerificationCache, IMobileVerification mobileVerification, IBajajAutoCache bajajAutoCache)
        {
            _bajajAutoRepository = bajajAutoRepository;
            _manufacturerCampaignRepository = manufacturerCampaignRepository;
            _customer = customer;
            _mobileVerificationRepository = mobileVerificationRepository;
            _mobileVerificationCache = mobileVerificationCache;
            _mobileVerification = mobileVerification;
            _bajajAutoCache = bajajAutoCache;
            _leadResponseMsgList = new List<string>
            {
                "Due to some technical failure, we cannot process your loan application currently. Please try again after some time.",
                "Your loan application is being processed by our finance partner. The status of the application will be shared over SMS and E-mail.",
                "Mobile number not verified."
            };
        }
        public uint SaveBasicDetails(UserDetails userDetails)
        {
            try
            {
                return _bajajAutoRepository.SaveBasicDetails(userDetails);
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("Bikewale.BAL.Finance.BajajAuto.SaveBasicDetails_userDetails_{0}", userDetails));
            }
            return 0;
        }

        /// <summary>
        /// Modifier    : Kartik Rathod on 3 aug 2018
        /// Description : added leadsource and client ip
        /// </summary>
        /// <param name="userDetails"></param>
        /// <param name="utmz"></param>
        /// <param name="utma"></param>
        /// <param name="leadSource"></param>
        /// <returns></returns>
        private uint SubmitLead(UserDetails userDetails, string utmz, string utma, ushort leadSource)
        {
            uint leadId = 0;
            try
            {
                ulong customerId = GetCustomerId(userDetails);

                leadId = _manufacturerCampaignRepository.SaveManufacturerCampaignLead(
                      userDetails.ManufacturerLead.DealerId,
                      userDetails.ManufacturerLead.PQId,
                      customerId,
                      String.Format("{0} {1}", userDetails.FirstName, userDetails.LastName),
                      userDetails.EmailId,
                      userDetails.MobileNumber,
                      userDetails.ManufacturerLead.LeadSourceId,
                      utma,
                      utmz,
                      userDetails.ManufacturerLead.DeviceId,
                      userDetails.ManufacturerLead.CampaignId,
                      userDetails.LeadId,
                      userDetails.ManufacturerLead.CityId,
                      userDetails.ManufacturerLead.VersionId,
                      userDetails.ManufacturerLead.PQGUId,
                      leadSource,
                      CurrentUser.GetClientIP()
                     );
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("Bikewale.BAL.Finance.BajajAuto.SubmitLead_userDetails_{0", userDetails));
            }
            return leadId;
        }

        private ulong GetCustomerId(UserDetails userDetails)
        {
            try
            {
                CustomerEntity customerEntity = _customer.GetByEmailMobile(userDetails.EmailId, userDetails.MobileNumber);
                CustomerEntity objCust = null;
                if (customerEntity.IsExist)
                {
                    objCust = new CustomerEntity()
                    {
                        CustomerId = customerEntity.CustomerId,
                        CustomerName = string.Format("{0} {1}", userDetails.FirstName, userDetails.LastName),
                        CustomerEmail = userDetails.EmailId = !String.IsNullOrEmpty(userDetails.EmailId) ? userDetails.EmailId : customerEntity.CustomerEmail,
                        CustomerMobile = userDetails.MobileNumber
                    };
                    _customer.Update(objCust);
                    return customerEntity.CustomerId;
                }
                else
                {
                    objCust = new CustomerEntity()
                    {
                        CustomerName = string.Format("{0} {1}", userDetails.FirstName, userDetails.LastName),
                        CustomerEmail = userDetails.EmailId,
                        CustomerMobile = userDetails.MobileNumber
                    };
                    return _customer.Add(objCust);
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("Bikewale.BAL.Finance.BajajAuto.GetCustomerId_userDetails_{0}", userDetails));
                return 0;
            }
        }

        public BASupplierResponse SaveEmployeeDetails(UserDetails userDetails)
        {
            try
            {
                _bajajAutoRepository.SaveEmployeeDetails(userDetails);
                return GetBajajAutoSuppliers(userDetails.VersionId, userDetails.PinCodeId);

            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("Bikewale.BAL.Finance.BajajAuto.SaveBasicDetails_userDetails_{0}", userDetails));
            }
            return null;
        }

        /// <summary>
        /// Modifier    : Kartik Rathod on 3 aug 2018
        /// Description : added leadsource and client ip
        /// Modifier    : Kartik Rathod on 22 aug 2018
        /// Desc        : Added logging for api request
        /// </summary>
        /// <param name="userDetails"></param>
        /// <param name="utmz"></param>
        /// <param name="utma"></param>
        /// <param name="leadSource"></param>
        /// <returns></returns>
        public LeadResponse SaveOtherDetails(UserDetails userDetails, string utmz, string utma, ushort leadSource)
        {
            LeadResponse leadResponse = null;
            try
            {
                string apiResponseJson = string.Empty;
                if (_mobileVerificationRepository.IsMobileVerified(userDetails.MobileNumber, userDetails.EmailId))
                {
                    userDetails.LeadId = SubmitLead(userDetails, utmz, utma, leadSource);
                    BAUploadApiResponse apiResponse  = PushLeadToBajajAuto(userDetails);
                    
                    userDetails.IsMobileVerified = true;
                    userDetails.ResponseJson = apiResponse.ResponseJson;
                    userDetails.RefEnqNumber = Convert.ToUInt64(apiResponse.EnquiryRefId);
                    _bajajAutoRepository.SaveOtherDetails(userDetails);

                    PushToLeadConsumerQueue(userDetails);

                    leadResponse = new LeadResponse();
                    leadResponse.Status = (ushort)(apiResponse.Status == "Success" ? EnumUserLeadStatus.Success : EnumUserLeadStatus.Fail);
                    leadResponse.Message = _leadResponseMsgList[leadResponse.Status];
                    leadResponse.BajaAutoId = userDetails.BajajAutoId;
                    leadResponse.LeadId = userDetails.LeadId;
                }
                else
                {
                    _bajajAutoRepository.SaveOtherDetails(userDetails);
                    MobileVerificationEntity verificationEntity = _mobileVerification.ProcessMobileVerification(userDetails.EmailId, userDetails.MobileNumber);
                    SMSTypes smsTypes = new SMSTypes();
                    smsTypes.SMSMobileVerification(userDetails.MobileNumber, string.Empty, verificationEntity.CWICode, userDetails.PageUrl);

                    leadResponse = new LeadResponse()
                    {
                        BajaAutoId = userDetails.BajajAutoId,
                        Status = (ushort)EnumUserLeadStatus.MobileNotVerified,
                        Message = _leadResponseMsgList[(ushort)EnumUserLeadStatus.MobileNotVerified]
                    };
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("Bikewale.BAL.Finance.BajajAuto.SaveBasicDetails_userDetails_{0}", userDetails));
            }
            return leadResponse;
        }

        private BAUploadApiResponse PushLeadToBajajAuto(UserDetails userDetails)
        {
            try
            {
                if (userDetails != null)
                {
                    double emiAmount = 0;
                    BajajBikeMappingEntity bikeMapping = _bajajAutoCache.GetBajajFinanceBikeMappingInfo(userDetails.VersionId, userDetails.PinCodeId);
                    if (bikeMapping != null && bikeMapping.MakeId > 0 && bikeMapping.ModelId > 0 && bikeMapping.CityId > 0 && bikeMapping.StateId > 0)
                    {

                        string onRoadPrice = GetBajajAutoOrp(userDetails.BajajSupplierId, bikeMapping.ModelId, bikeMapping.CityId, bikeMapping.StateId);
                        double loanAmount = Convert.ToDouble(onRoadPrice) * 0.85;
                        if(loanAmount > 0)
                        {
                            emiAmount = Math.Ceiling((((loanAmount * BajajAutoRateOfInterest) / 100) * (BajajAutoTenure / 12) + loanAmount) / BajajAutoTenure);
                        }

                        BAUploadApiRequest bAUploadApiRequest = new BAUploadApiRequest
                        {
                            EnqId = Convert.ToString(userDetails.LeadId),
                            EnqSalutation = userDetails.Salutation,
                            EnqFirstName = userDetails.FirstName,
                            EnqLastName = userDetails.LastName,
                            EnqLikelyPurchaseDate = userDetails.LikelyPurchaseDate,
                            EnqMobileNumber = userDetails.MobileNumber,
                            EnqEmail = userDetails.EmailId,
                            EnqDealerCode = Convert.ToString(userDetails.BajajSupplierId),
                            EnqProductCategoryId = Convert.ToString(bikeMapping.MakeId),
                            EnqProductCode = Convert.ToString(bikeMapping.ModelId),
                            Dob = userDetails.DateOfBirth.ToString("yyy-MM-dd"),
                            Gender = userDetails.Gender,
                            IdProof = userDetails.IdProof,
                            IdNo = userDetails.IdProofNo,
                            PresentResidenceStatus = userDetails.ResidenceStatus,
                            AddressLine1 = userDetails.AddressLine1,
                            AddressLine2 = userDetails.AddressLine2,
                            Landmark = userDetails.LandMark,
                            Pincode = userDetails.PinCode,
                            City = Convert.ToString(bikeMapping.CityId),
                            State = Convert.ToString(bikeMapping.StateId),
                            PrimaryEmployeeType = Convert.ToString(userDetails.EmploymentType),
                            EmployeeSubType = GetEmployeeSubType(userDetails),
                            CompanyName = userDetails.CompanyId != 0 ? Convert.ToString(userDetails.CompanyId) : null,
                            OtherCompany = userDetails.OtherCompany,
                            PrimaryIncome = userDetails.PrimaryIncome,
                            Dependents = Convert.ToString(userDetails.Dependents),
                            RepaymentMode = userDetails.RepaymentMode,
                            SubInstrument = userDetails.RepaymentMode,
                            AccountNo = userDetails.BankAccountNo,
                            AccountVintage = userDetails.AccountVintage,
                            CreditSurrogate = userDetails.CompanyId > 0 ? "Top Corporates" : "App Score",
                            ResidenceSinceYears = userDetails.ResidingSince,
                            WorkingSinceMonths = Convert.ToString(userDetails.WorkingSince),
                            Orp = onRoadPrice,
                            DealerC = userDetails.BajajSupplierId,
                            EnqAmountFinanced = loanAmount > 0? Convert.ToString(loanAmount) : null,
                            EnqAdvanceEmi = emiAmount > 0 ? Convert.ToString(emiAmount) : null
                        };

                        ICollection<KeyValuePair<string, string>> formData = new Dictionary<string, string>();
                        formData.Add(new KeyValuePair<string, string>("enquiryData", JsonConvert.SerializeObject(bAUploadApiRequest, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" })));
                        HttpContent httpContent = new FormUrlEncodedContent(formData);
                        httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");

                        _logger.Error(string.Format("Bajaj Finance for lead id : {0} - header : {1} , api url : {2} ,api request : {3}", userDetails.LeadId, httpContent.Headers, BajajAutoUploadApiUrl, JsonConvert.SerializeObject(formData)));

                        HttpClient _httpClient = new HttpClient();
                        string apiResponseJson = string.Empty;
                        using (HttpResponseMessage _response = _httpClient.PostAsync(BajajAutoUploadApiUrl, httpContent).Result)
                        {
                            if (_response.IsSuccessStatusCode && _response.StatusCode == System.Net.HttpStatusCode.OK)
                            {
                                apiResponseJson = _response.Content.ReadAsStringAsync().Result;
                                _response.Content.Dispose();
                                _response.Content = null;
                            }

                            _logger.Error(string.Format("Bajaj Finance for lead id : {0} - api response : {1} , apiResponseJson : {2}", userDetails.LeadId, _response, apiResponseJson));
                        }
                        BAUploadApiResponse apiResponse = JsonConvert.DeserializeObject<BAUploadApiResponse>(apiResponseJson);
                        apiResponse.ResponseJson = apiResponseJson;
                        return apiResponse;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("Bikewale.BAL.Finance.BajajAuto.PushLeadToBajajAuto_userDetails_{0}", userDetails));
            }
            return null;
        }

        private uint GetEmployeeSubType(UserDetails userDetails)
        {
            try
            {   // Salaried 
                if (userDetails.EmploymentType == 1)
                {
                    return userDetails.CompanyId > 0 ? (uint)EnumEmployeeSubType.Top_Corporates : (uint)EnumEmployeeSubType.Pvt_LtdOrLtd;
                }
                // Self Employed
                else if (userDetails.EmploymentType == 2)
                {
                    return (uint)EnumEmployeeSubType.Trader;
                }
                else if (userDetails.EmploymentType == 4)
                {
                    return (uint)EnumEmployeeSubType.Farmer;
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("Bikewale.BAL.Finance.BajajAuto.GetEmployeeSubType_userDetails_{0}", userDetails));
            }
            return 0;
        }


        private void PushToLeadConsumerQueue(UserDetails userDetails)
        {
            try
            {
                var numberList = _mobileVerificationCache.GetBlockedNumbers();

                if (numberList != null && !numberList.Contains(userDetails.MobileNumber))
                {
                    // push in autobiz
                    NameValueCollection objNVC = new NameValueCollection();
                    objNVC.Add("pqId", Convert.ToString(userDetails.ManufacturerLead.PQId));
                    objNVC.Add("dealerId", Convert.ToString(Utility.BWConfiguration.Instance.BajajAutoFinanceDealerId));
                    objNVC.Add("customerName", String.Format("{0} {1}", userDetails.FirstName, userDetails.LastName));
                    objNVC.Add("customerEmail", userDetails.EmailId);
                    objNVC.Add("customerMobile", userDetails.MobileNumber);
                    objNVC.Add("versionId", Convert.ToString(userDetails.VersionId));
                    objNVC.Add("pincodeId", userDetails.PinCode);
                    objNVC.Add("cityId", Convert.ToString(userDetails.CityId));
                    objNVC.Add("leadType", "2");
                    objNVC.Add("manufacturerDealerId", Convert.ToString(userDetails.BajajSupplierId));
                    objNVC.Add("manufacturerLeadId", Convert.ToString(userDetails.LeadId));
                    objNVC.Add("dealerName", userDetails.ManufacturerLead.DealerName);
                    objNVC.Add("bikeName", userDetails.ManufacturerLead.BikeName);
                    objNVC.Add("sendLeadSMSCustomer", Convert.ToString(userDetails.ManufacturerLead.SendLeadSMSCustomer));
                    objNVC.Add("pqGUId", Convert.ToString(userDetails.ManufacturerLead.PQGUId));
                    objNVC.Add("campaignId", Convert.ToString(userDetails.ManufacturerLead.CampaignId));

                    RabbitMqPublish objRMQPublish = new RabbitMqPublish();
                    objRMQPublish.PublishToQueue(Bikewale.Utility.BWConfiguration.Instance.LeadConsumerQueue, objNVC);
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("PushToLeadConsumerQueue({0}, pqguid -{1})", (userDetails != null && userDetails.ManufacturerLead != null) ? userDetails.ManufacturerLead.PQId : 0, userDetails.ManufacturerLead.PQGUId));
            }
        }

        private BASupplierResponse GetBajajAutoSuppliers(uint versionId, uint pincodeId)
        {
            try
            {
                BajajBikeMappingEntity bikeMapping = _bajajAutoCache.GetBajajFinanceBikeMappingInfo(versionId, pincodeId);
                if (bikeMapping != null && bikeMapping.MakeId > 0 && bikeMapping.ModelId > 0 && bikeMapping.CityId > 0 && bikeMapping.StateId > 0)
                {
                    HttpClient _httpClient = new HttpClient();
                    string response = string.Empty;
                    string url = string.Format("{0}&cityId={1}&stateId={2}&makeId={3}&modelId={4}", BajajAutoSupplierApiUrl, bikeMapping.CityId, bikeMapping.StateId, bikeMapping.MakeId, bikeMapping.ModelId);
                    HttpContent httpContent = new FormUrlEncodedContent(new List<KeyValuePair<string,string>>());
                    httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
                    using (HttpResponseMessage _response = _httpClient.PostAsync(url, httpContent).Result)
                    {
                        if (_response.IsSuccessStatusCode && _response.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            response = _response.Content.ReadAsStringAsync().Result;
                            _response.Content.Dispose();
                            _response.Content = null;
                        }
                    }
                    return JsonConvert.DeserializeObject<BASupplierResponse>(response);
                }
                else
                {
                    BASupplierResponse bASupplierResponse = new BASupplierResponse()
                    {
                        Status = "fail",
                        ResponseMsg = "Bike mapping and pincode mapping not available"
                    };
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("Bikewale.BAL.Finance.BajajAuto.GetBajajAutoSuppliers_versionId_{0}_pincodeId_{1}", versionId, pincodeId));
            }
            return null;
        }

        private string GetBajajAutoOrp(uint supplierId, uint productCode, uint cityId, uint stateId)
        {
            try
            {
                if (supplierId > 0 && productCode > 0 && cityId > 0 && stateId > 0)
                {
                    HttpClient _httpClient = new HttpClient();
                    string response = string.Empty;
                    string url = string.Format("{0}?icasSuppId={1}&productCode={2}&cityCode={3}&stateCode{4}", BajajAutoOrpApiUrl, supplierId, productCode, cityId, stateId);
                    HttpContent httpContent = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>());
                    httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
                    using (HttpResponseMessage _response = _httpClient.PostAsync(url, httpContent).Result)
                    {
                        if (_response.IsSuccessStatusCode && _response.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            response = _response.Content.ReadAsStringAsync().Result;
                            _response.Content.Dispose();
                            _response.Content = null;
                        }
                    }
                    return JsonConvert.DeserializeObject<BAOrpApiDetailsResponse>(response).OnRoadPrice;
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("Bikewale.BAL.Finance.BajajAuto.GetBajajAutoOrpsupplierId_{0}_productCode_{1}_cityId_{2}_stateId_{3}", supplierId, productCode, cityId, stateId));
            }
            return "0";
        }
    }

    internal class BAUploadApiRequest
    {
        [JsonProperty("enqId")]
        public string EnqId { get; set; }
        [JsonProperty("fosCode")]
        public string FosCode { get { return "BikeWale"; } }
        [JsonProperty("enqDoneDateTime")]
        public DateTime EnqDoneDateTime { get { return DateTime.Now; } }
        [JsonProperty("enqSalutation")]
        public byte EnqSalutation { get; set; }
        [JsonProperty("enqFirstName")]
        public string EnqFirstName { get; set; }
        [JsonProperty("enqMiddleName")]
        public string EnqMiddleName { get { return string.Empty; } }
        [JsonProperty("enqLikelyPurchaseDate")]
        public DateTime EnqLikelyPurchaseDate { get; set; }
        [JsonProperty("creationDtm")]
        public DateTime CreationDtm { get { return DateTime.Now; } }
        [JsonProperty("exportDtm")]
        public DateTime ExportDtm { get { return DateTime.Now; } }
        [JsonProperty("enqLastName")]
        public string EnqLastName { get; set; }
        [JsonProperty("enqMobileNumber")]
        public string EnqMobileNumber { get; set; }
        [JsonProperty("enqLandlineNumber")]
        public string EnqLandlineNumber { get { return string.Empty; } }
        [JsonProperty("enqEmail")]
        public string EnqEmail { get; set; }
        [JsonProperty("isExported")]
        public byte IsExported { get { return 0; } }
        [JsonProperty("enqDealerCode")]
        public string EnqDealerCode { get; set; }
        [JsonProperty("exportRemarks")]
        public string ExportRemarks { get { return string.Empty; } }
        [JsonProperty("exportStatus")]
        public byte ExportStatus { get { return 1; } }
        [JsonProperty("uid")]
        public string Uid { get { return string.Empty; } }
        [JsonProperty("leadId")]
        public string LeadId { get { return string.Empty; } }
        [JsonProperty("enqSchemeId")]
        public string EnqSchemeId { get { return null; } }
        [JsonProperty("enqDma")]
        public string EnqDma { get { return string.Empty; } }
        [JsonProperty("enqTenure")]
        public string EnqTenure { get { return Utility.BWConfiguration.Instance.BajajAutoTenure; } }
        [JsonProperty("enqAmountFinanced")]
        public string EnqAmountFinanced { get; set; }
        [JsonProperty("enqRateOfInterest")]
        public string EnqRateOfInterest { get { return Utility.BWConfiguration.Instance.BajajAutoRateOfInterest; } }
        [JsonProperty("enqProductCategoryId")]
        public string EnqProductCategoryId { get; set; }
        [JsonProperty("enqProductCode")]
        public string EnqProductCode { get; set; }
        [JsonProperty("enqAdvanceEmi")]
        public string EnqAdvanceEmi { get; set; }
        [JsonProperty("enqCoApplicantPresent")]
        public byte EnqCoApplicantPresent { get { return 2; } }
        [JsonProperty("enqGuarantorPresent")]
        public byte EnqGuarantorPresent { get { return 2; } }
        [JsonProperty("enqLatLong")]
        public string EnqLatLong { get { return string.Empty; } }
        [JsonProperty("constitution")]
        public byte Constitution { get { return 1; } }
        [JsonProperty("appEmiCardPresent")]
        public byte AppEmiCardPresent { get { return 2; } }
        [JsonProperty("is3m")]
        public byte Is3m { get { return 1; } }
        [JsonProperty("flagConvertEnq")]
        public byte FlagConvertEnq { get { return 0; } }
        [JsonProperty("convertEnqTo3m")]
        public byte ConvertEnqTo3m { get { return 1; } }
        [JsonProperty("insertedToICRM")]
        public byte InsertedToICRM { get { return 0; } }
        [JsonProperty("dob")]
        public string Dob { get; set; }
        [JsonProperty("gender")]
        public string Gender { get; set; }
        [JsonProperty("idProof")]
        public string IdProof { get; set; }
        [JsonProperty("idNo")]
        public string IdNo { get; set; }
        [JsonProperty("presentResidenceStatus")]
        public string PresentResidenceStatus { get; set; }
        [JsonProperty("residenceSince")]
        public string ResidenceSince { get { return null; } }
        [JsonProperty("addressLine1")]
        public string AddressLine1 { get; set; }
        [JsonProperty("addressLine2")]
        public string AddressLine2 { get; set; }
        [JsonProperty("landmark")]
        public string Landmark { get; set; }
        [JsonProperty("pincode")]
        public string Pincode { get; set; }
        [JsonProperty("city")]
        public string City { get; set; }
        [JsonProperty("state")]
        public string State { get; set; }
        [JsonProperty("primaryEmployeeType")]
        public string PrimaryEmployeeType { get; set; }
        [JsonProperty("employeeSubType")]
        public uint EmployeeSubType { get; set; }
        [JsonProperty("companyName")]
        public string CompanyName { get; set; }
        [JsonProperty("otherCompany")]
        public string OtherCompany { get; set; }
        [JsonProperty("primaryIncome")]
        public string PrimaryIncome { get; set; }
        [JsonProperty("dependents")]
        public string Dependents { get; set; }
        [JsonProperty("repaymentMode")]
        public string RepaymentMode { get; set; }
        [JsonProperty("subInstrument")]
        public string SubInstrument { get; set; }
        [JsonProperty("workingSince")]
        public string WorkingSince { get { return null; } }
        [JsonProperty("lanNo")]
        public string LanNo { get { return null; } }
        [JsonProperty("creditCardBank")]
        public string CreditCardBank { get { return null; } }
        [JsonProperty("creditCardType")]
        public string CreditCardType { get { return null; } }
        [JsonProperty("accountNo")]
        public string AccountNo { get; set; }
        [JsonProperty("accountVintage")]
        public string AccountVintage { get; set; }
        [JsonProperty("creditSurrogate")]
        public string CreditSurrogate { get; set; }
        [JsonProperty("enq_is_submitted")]
        public string Enq_is_submitted { get { return "YES"; } }
        [JsonProperty("residenceSinceYears")]
        public string ResidenceSinceYears { get; set; }
        [JsonProperty("residenceSinceMonths")]
        public string ResidenceSinceMonths { get { return "0"; } }
        [JsonProperty("workingSinceYears")]
        public string WorkingSinceYears { get { return "0"; } }
        [JsonProperty("workingSinceMonths")]
        public string WorkingSinceMonths { get; set; }
        [JsonProperty("enqStatus")]
        public string EnqStatus { get { return "LEAD-3M"; } }
        [JsonProperty("id")]
        public string Id { get { return null; } }
        [JsonProperty("dmaNameC")]
        public string DmaNameC { get { return null; } }
        [JsonProperty("dmaCodeC")]
        public string DmaCodeC { get { return null; } }
        [JsonProperty("otherADCChannelC")]
        public string OtherADCChannelC { get { return null; } }
        [JsonProperty("sourceDealerChannel")]
        public string SourceDealerChannel { get { return "web sales"; } }
        [JsonProperty("orp")]
        public string Orp { get; set; }
        [JsonProperty("dealerC")]
        public uint DealerC { get; set; }
        [JsonProperty("enquiryDoneSource")]
        public string EnquiryDoneSource { get { return "BikeWale"; } }


    }
    internal class BAUploadApiResponse
    {
        [JsonProperty("status")]
        public string Status { get; set; }
        [JsonProperty("responseMsg")]
        public string ResponseMsg { get; set; }
        [JsonProperty("enquiryRefId")]
        public string EnquiryRefId { get; set; }
        [JsonIgnore]
        public string ResponseJson { get; set; }
    }
    

    internal class BAOrpApiDetailsResponse
    {
        [JsonProperty("status")]
        public string Status{ get; set; }
        [JsonProperty("onRoadPrice")]
        public string OnRoadPrice { get; set; }
    }

    internal enum EnumEmployeeSubType
    {
        Top_Corporates = 917859,
        Pvt_LtdOrLtd = 917856,
        Trader = 917865,
        Farmer = 917869
    }
    internal enum EnumUserLeadStatus
    {
        Fail = 0,
        Success = 1,
        MobileNotVerified = 2
    }
}
