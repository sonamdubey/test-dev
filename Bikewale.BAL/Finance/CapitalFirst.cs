using Bikewale.Entities.Customer;
using Bikewale.Entities.Finance.CapitalFirst;
using Bikewale.Entities.MobileVerification;
using Bikewale.Interfaces.Customer;
using Bikewale.Interfaces.Finance;
using Bikewale.Interfaces.Finance.CapitalFirst;
using Bikewale.Interfaces.MobileVerification;
using Bikewale.ManufacturerCampaign.Interface;
using Bikewale.Notifications;
using Newtonsoft.Json;
using RabbitMqPublishing;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Bikewale.BAL.Finance
{
    /// <summary>
    /// Created by  :   Sumit Kate on 11 Sep 2017
    /// Description :   Capital First Business Layer.
    /// </summary>
    public class CapitalFirst : ICapitalFirst
    {
        private readonly IFinanceRepository _objIFinanceRepository = null;
        private readonly IMobileVerificationRepository _mobileVerRespo = null;
        private readonly IMobileVerification _mobileVerification = null;
        private readonly IMobileVerificationCache _mobileVerCacheRepo = null;
        private readonly IManufacturerCampaignRepository _manufacturerCampaignRepo = null;
        private readonly ICustomerAuthentication<CustomerEntity, UInt32> _objAuthCustomer = null;
        private readonly ICustomer<CustomerEntity, UInt32> _objCustomer = null;

        private HttpClient _httpClient = null;

        private readonly String CTApiUrl = Bikewale.Utility.BWConfiguration.Instance.CarTradeLeadUrl;
        private readonly String CTApiAction = Bikewale.Utility.BWConfiguration.Instance.CarTradeLeadApiAction;
        private readonly String CTApiCode = Bikewale.Utility.BWConfiguration.Instance.CarTradeLeadApiCode;

        private const string CF_MESSAGE_SUCCESS = "Data saved successfully";
        private const string CF_MESSAGE_SAVE_FAILURE = "Error occured while saving data";
        private const string CF_MESSAGE_INVALID = "Invalid lead id or request body is empty";

        private readonly IDictionary<ushort, String> _leadStatusCollection = null;

        /// <summary>
        /// Created by  :   Sumit Kate on 11 Sep 2017
        /// Description :   Type Initializer
        /// </summary>
        /// <param name="objIFinanceRepository"></param>
        public CapitalFirst(IManufacturerCampaignRepository manufacturerCampaignRepo, IFinanceRepository objIFinanceRepository, IMobileVerificationRepository mobileVerRespo, IMobileVerification mobileVerification, IMobileVerificationCache mobileVerCacheRepo, ICustomerAuthentication<CustomerEntity, UInt32> objAuthCustomer, ICustomer<CustomerEntity, UInt32> objCustomer)
        {
            _objIFinanceRepository = objIFinanceRepository;
            _mobileVerRespo = mobileVerRespo;
            _mobileVerification = mobileVerification;
            _mobileVerCacheRepo = mobileVerCacheRepo;
            _manufacturerCampaignRepo = manufacturerCampaignRepo;
            _objAuthCustomer = objAuthCustomer;
            _objCustomer = objCustomer;

            _leadStatusCollection = new Dictionary<ushort, String>();
            _leadStatusCollection.Add(0, "Some error occured.");
            _leadStatusCollection.Add(1, "Proceed to step 2");
            _leadStatusCollection.Add(2, "Proceed to step 2");
            _leadStatusCollection.Add(3, "Your loan application has already got pre-approved. Please contact your Capital First executive (Details shared in email).");
            _leadStatusCollection.Add(4, "Your loan application could not be processed online. Thanks for applying.");
            _leadStatusCollection.Add(5, "Your loan application could not be processed online. Thanks for applying.");
            _leadStatusCollection.Add(6, "Your loan application is in process. We will share the status of your application over email / SMS.");
            _leadStatusCollection.Add(8, "Some error occured while processing your request. Please try after sometime.");
            _leadStatusCollection.Add(12, "Currently, our finance partner does not provide loan in your area.");
            _leadStatusCollection.Add(13, "Mobile not verified.");
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 11 Sep 2017
        /// Description :   To save Capital First voucher details sent by CarTrade
        /// </summary>
        /// <param name="ctLeadId"></param>
        /// <param name="jsonData"></param>
        /// <returns></returns>
        public string SaveVoucherDetails(string ctLeadId, CapitalFirstVoucherEntityBase entity)
        {
            string message = "";
            try
            {
                bool isSuccess = _objIFinanceRepository.IsValidLead(ctLeadId);
                if (isSuccess && entity != null)
                {
                    isSuccess = _objIFinanceRepository.SaveVoucherDetails(ctLeadId, entity);

                    if (isSuccess)
                    {
                        NameValueCollection objNVC = new NameValueCollection();
                        objNVC.Add("ctLeadId", ctLeadId);
                        objNVC.Add("agentContactNumber", entity.AgentContactNumber);
                        objNVC.Add("agentName", entity.AgentName);
                        objNVC.Add("expiryDate", entity.ExpiryDate.ToShortDateString());
                        objNVC.Add("voucherCode", entity.VoucherCode);
                        objNVC.Add("status", ((int)entity.Status).ToString());
                        RabbitMqPublish objRMQPublish = new RabbitMqPublish();
                        objRMQPublish.PublishToQueue(Bikewale.Utility.BWConfiguration.Instance.CapitalFirstConsumerQueue, objNVC);
                        message = CF_MESSAGE_SUCCESS;
                    }
                    else
                    {
                        message = CF_MESSAGE_SAVE_FAILURE;
                    }
                }
                else
                {
                    message = CF_MESSAGE_INVALID;
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("CapitalFirst.SaveVoucherDetails({0},{1})", ctLeadId, Newtonsoft.Json.JsonConvert.SerializeObject(entity)));
                message = CF_MESSAGE_SAVE_FAILURE;
            }
            return message;
        }

        public LeadResponseMessage SaveEmployeDetails(PersonalDetails objDetails, string Utmz, string Utma, ushort leadSource)
        {
            LeadResponseMessage response = null;
            try
            {
                response = new LeadResponseMessage();
                if (objDetails.LeadId == 0)
                {

                    objDetails.LeadId = SubmitLead(objDetails, Utmz, Utma);
                    response.CpId = 0;
                    response.CTleadId = 0;
                }

                response.LeadId = objDetails.LeadId;
                if (_mobileVerRespo.IsMobileVerified(Convert.ToString(objDetails.MobileNumber), objDetails.EmailId))
                {
                    response = PushLeadinCTandAutoBiz(objDetails, leadSource);
                }
                else
                {
                    response.Message = _leadStatusCollection[13];
                    response.Status = 13;
                    response.CpId = objDetails.Id;
                    response.CTleadId = objDetails.CtLeadId;
                    MobileVerificationEntity mobileVer = null;
                    mobileVer = _mobileVerification.ProcessMobileVerification(objDetails.EmailId, Convert.ToString(objDetails.MobileNumber));
                    SMSTypes st = new SMSTypes();
                    st.SMSMobileVerification(Convert.ToString(objDetails.MobileNumber), string.Empty, mobileVer.CWICode, "PageUrl");

                }
            }
            catch (Exception ex)
            {

                ErrorClass.LogError(ex, String.Format("CapitalFirst.SaveEmployeDetails({0})", Newtonsoft.Json.JsonConvert.SerializeObject(objDetails)));
            }
            return response;

        }
        public LeadResponseMessage SavePersonalDetails(PersonalDetails objDetails, string Utmz, string Utma, ushort leadSource)
        {
            LeadResponseMessage objId = null;

            try
            {
                if (objDetails.LeadId == 0)
                {
                    objDetails.LeadId = SubmitLead(objDetails, Utmz, Utma);
                }
                PushToLeadConsumerQueue(objDetails);

                #region Do not change the sequence
                //Save Lead Details to Bikewale Capital First Lead Table
                objDetails.Id = _objIFinanceRepository.SavePersonalDetails(objDetails);
                //Sent Details to CT API
                var ctResponse = SendCustomerDetailsToCarTrade(objDetails, leadSource);
                objId = new LeadResponseMessage();
                if (ctResponse != null)
                {
                    objId.CTleadId = objDetails.CtLeadId = ctResponse.LeadId;
                    objId.Status = ctResponse.Status;
                    objId.Message = _leadStatusCollection[ctResponse.Status];
                    //Update ct api response
                    _objIFinanceRepository.SaveCTApiResponse(objDetails.LeadId, ctResponse.LeadId, ctResponse.Status, ctResponse.Message);
                }
                objId.CpId = objDetails.Id;
                objId.LeadId = objDetails.LeadId;
                #endregion


            }
            catch (Exception ex)
            {

                ErrorClass.LogError(ex, String.Format("CapitalFirst.SavePersonalDetails({0})", Newtonsoft.Json.JsonConvert.SerializeObject(objDetails)));
            }
            return objId;

        }

        private CTFormResponse SendCustomerDetailsToCarTrade(PersonalDetails objDetails, ushort leadSource)
        {
            CTFormResponse ctResp = null;
            string response = string.Empty;
            try
            {
                if (objDetails != null)
                {
                    var bikemapping = _objIFinanceRepository.GetCapitalFirstBikeMapping(objDetails.objLead.VersionId);
                    if (bikemapping != null)
                    {


                        ICollection<KeyValuePair<string, string>> formData = new Dictionary<string, string>();
                        formData.Add(new KeyValuePair<string, string>("action", CTApiAction));
                        formData.Add(new KeyValuePair<string, string>("api_code", CTApiCode));
                        formData.Add(new KeyValuePair<string, string>("bw_lead_id", objDetails.LeadId.ToString()));
                        formData.Add(new KeyValuePair<string, string>("company", objDetails.CompanyName));
                        formData.Add(new KeyValuePair<string, string>("date_of_birth", objDetails.DateOfBirth.ToString("yyyy/MM/dd")));
                        formData.Add(new KeyValuePair<string, string>("email", objDetails.EmailId));
                        formData.Add(new KeyValuePair<string, string>("emp_address1", objDetails.OfficialAddressLine1));
                        formData.Add(new KeyValuePair<string, string>("emp_address2", objDetails.OfficialAddressLine2));
                        formData.Add(new KeyValuePair<string, string>("emp_pincode", objDetails.PincodeOffice));
                        formData.Add(new KeyValuePair<string, string>("emp_type", objDetails.Status == 1 ? "Salaried" : "Self Employed"));
                        formData.Add(new KeyValuePair<string, string>("fname", objDetails.FirstName));
                        formData.Add(new KeyValuePair<string, string>("from_source", leadSource.ToString())); // 1 - Desktop, 2 - Mobile
                        formData.Add(new KeyValuePair<string, string>("gender", objDetails.Gender == 1 ? "Male" : "Female"));
                        formData.Add(new KeyValuePair<string, string>("gross_income", objDetails.AnnualIncome.ToString()));
                        formData.Add(new KeyValuePair<string, string>("amount_needed", objDetails.LoanAmount.ToString()));
                        formData.Add(new KeyValuePair<string, string>("lead_id", objDetails.CtLeadId.ToString()));
                        formData.Add(new KeyValuePair<string, string>("lname", objDetails.LastName));
                        formData.Add(new KeyValuePair<string, string>("make", bikemapping.MakeBase.Make));
                        formData.Add(new KeyValuePair<string, string>("model", bikemapping.ModelBase.ModelNo));
                        formData.Add(new KeyValuePair<string, string>("mobile", objDetails.MobileNumber));
                        formData.Add(new KeyValuePair<string, string>("m_status", objDetails.MaritalStatus == 1 ? "Married" : "Single"));
                        formData.Add(new KeyValuePair<string, string>("pan_number", objDetails.Pancard));
                        formData.Add(new KeyValuePair<string, string>("res_address1", objDetails.AddressLine1));
                        formData.Add(new KeyValuePair<string, string>("res_address2", objDetails.AddressLine2));
                        formData.Add(new KeyValuePair<string, string>("res_pincode", objDetails.Pincode));


                        HttpContent httpContent = new FormUrlEncodedContent(formData);
                        httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");


                        _httpClient = new HttpClient();
                        using (HttpResponseMessage _response = _httpClient.PostAsync(CTApiUrl, httpContent).Result)
                        {
                            if (_response.IsSuccessStatusCode && _response.StatusCode == System.Net.HttpStatusCode.OK)
                            {
                                response = _response.Content.ReadAsStringAsync().Result;
                                _response.Content.Dispose();
                                _response.Content = null;
                            }
                        }
                        if (!String.IsNullOrEmpty(response))
                        {
                            ctResp = Newtonsoft.Json.JsonConvert.DeserializeObject<CTFormResponse>(response);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("CapitalFirst.SendCustomerDetailsToCarTrade({0})", Newtonsoft.Json.JsonConvert.SerializeObject(objDetails)));
            }
            finally
            {
                if (ctResp == null)
                {
                    ctResp = new CTFormResponse();
                }
            }
            return ctResp;
        }

        private CustomerEntity GetCustomerId(PersonalDetails objDetails, string MobileNumber)
        {

            CustomerEntity objCust = null;
            try
            {

                if (!_objAuthCustomer.IsRegisteredUser(objDetails.EmailId, MobileNumber))
                {
                    objCust = new CustomerEntity() { CustomerName = string.Format("{0} {1}", objDetails.FirstName, objDetails.LastName), CustomerEmail = objDetails.EmailId, CustomerMobile = MobileNumber, ClientIP = "" };
                    objCust.CustomerId = _objCustomer.Add(objCust);

                }
                else
                {
                    var objCustomer = _objCustomer.GetByEmailMobile(objDetails.EmailId, MobileNumber);
                    objCust = new CustomerEntity()
                    {
                        CustomerId = objCustomer.CustomerId,
                        CustomerName = string.Format("{0} {1}", objDetails.FirstName, objDetails.LastName),
                        CustomerEmail = objDetails.EmailId = !String.IsNullOrEmpty(objDetails.EmailId) ? objDetails.EmailId : objCustomer.CustomerEmail,
                        CustomerMobile = MobileNumber
                    };
                    _objCustomer.Update(objCust);
                }
            }
            catch (Exception ex)
            {

                ErrorClass.LogError(ex, "Bikewale.Service.Controllers.SavePersonalDetails");
            }
            return objCust;
        }

        private uint SubmitLead(PersonalDetails objDetails, string Utmz, string Utma)
        {
            uint id = 0;
            try
            {
                CustomerEntity objCust = GetCustomerId(objDetails, objDetails.MobileNumber);

                id = _manufacturerCampaignRepo.SaveManufacturerCampaignLead(
                      objDetails.objLead.DealerId,
                      objDetails.objLead.PQId,
                      objCust.CustomerId,
                      String.Format("{0} {1}", objDetails.FirstName, objDetails.LastName),
                      objDetails.EmailId,
                      objDetails.MobileNumber,
                      objDetails.objLead.LeadSourceId,
                      Utma,
                      Utmz,
                      objDetails.objLead.DeviceId,
                      objDetails.objLead.CampaignId,
                      objDetails.LeadId
                     );
            }
            catch (Exception ex)
            {

                ErrorClass.LogError(ex, String.Format("CapitalFirst.SubmitLead({0})", Newtonsoft.Json.JsonConvert.SerializeObject(objDetails)));
            }
            return id;
        }

        public LeadResponseMessage PushLeadinCTandAutoBiz(PersonalDetails objDetails, ushort leadSource)
        {
            LeadResponseMessage response = null;
            try
            {

                #region Do not change sequence
                var ctResponse = SendCustomerDetailsToCarTrade(objDetails, leadSource);

                objDetails.Id = _objIFinanceRepository.SavePersonalDetails(objDetails);

                response = new LeadResponseMessage();
                response.CpId = objDetails.Id;
                response.LeadId = objDetails.LeadId;

                if (ctResponse != null)
                {
                    response.CTleadId = objDetails.CtLeadId = ctResponse.LeadId;
                    response.Status = ctResponse.Status;
                    response.Message = _leadStatusCollection[ctResponse.Status];
                    _objIFinanceRepository.SaveCTApiResponse(objDetails.LeadId, ctResponse.LeadId, ctResponse.Status, ctResponse.Message);
                }
                #endregion
                objDetails.Id = _objIFinanceRepository.SavePersonalDetails(objDetails);
            }
            catch (Exception ex)
            {

                ErrorClass.LogError(ex, String.Format("CapitalFirst.PushLeadinCTandAutoBiz({0})", Newtonsoft.Json.JsonConvert.SerializeObject(objDetails)));
            }
            return response;
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 25 Jan 2018
        /// Description :   Push To Lead ConsumerQueue
        /// </summary>
        /// <param name="objDetails"></param>
        private void PushToLeadConsumerQueue(PersonalDetails objDetails)
        {
            try
            {
                var numberList = _mobileVerCacheRepo.GetBlockedNumbers();

                if (numberList != null && !numberList.Contains(Convert.ToString(objDetails.MobileNumber)))
                {
                    // push in autobiz
                    NameValueCollection objNVC = new NameValueCollection();
                    objNVC.Add("pqId", objDetails.objLead.PQId.ToString());
                    objNVC.Add("dealerId", objDetails.objLead.DealerId.ToString());
                    objNVC.Add("customerName", String.Format("{0} {1}", objDetails.FirstName, objDetails.LastName));
                    objNVC.Add("customerEmail", objDetails.EmailId);
                    objNVC.Add("customerMobile", Convert.ToString(objDetails.MobileNumber));
                    objNVC.Add("versionId", objDetails.objLead.VersionId.ToString());
                    objNVC.Add("pincodeId", Convert.ToString(objDetails.Pincode));
                    objNVC.Add("cityId", objDetails.objLead.CityId.ToString());
                    objNVC.Add("leadType", "2");
                    objNVC.Add("manufacturerDealerId", Convert.ToString(objDetails.objLead.ManufacturerDealerId));
                    objNVC.Add("manufacturerLeadId", objDetails.LeadId.ToString());
                    RabbitMqPublish objRMQPublish = new RabbitMqPublish();
                    objRMQPublish.PublishToQueue(Bikewale.Utility.BWConfiguration.Instance.LeadConsumerQueue, objNVC);
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("PushToLeadConsumerQueue({0})", (objDetails != null && objDetails.objLead != null) ? objDetails.objLead.PQId : 0));
            }
        }

    }

    internal class CTFormResponse
    {
        [JsonProperty("status")]
        public ushort Status { get; set; }
        [JsonProperty("details")]
        public String Details { get; set; }
        [JsonProperty("message")]
        public String Message { get; set; }
        [JsonProperty("lead_id")]
        public uint LeadId { get; set; }
        [JsonProperty("lead_status")]
        public ushort LeadStatus { get; set; }
    }
}
