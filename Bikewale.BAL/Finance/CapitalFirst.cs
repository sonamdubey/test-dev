using Bikewale.Entities.Customer;
using Bikewale.Entities.Finance.CapitalFirst;
using Bikewale.Entities.MobileVerification;
using Bikewale.Interfaces.Customer;
using Bikewale.Interfaces.Finance;
using Bikewale.Interfaces.Finance.CapitalFirst;
using Bikewale.Interfaces.MobileVerification;
using Bikewale.ManufacturerCampaign.Interface;
using Bikewale.Notifications;
using RabbitMqPublishing;
using System;
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
                ErrorClass err = new ErrorClass(ex, String.Format("CapitalFirst.SaveVoucherDetails({0},{1})", ctLeadId, Newtonsoft.Json.JsonConvert.SerializeObject(entity)));
                message = CF_MESSAGE_SAVE_FAILURE;
            }
            return message;
        }

        public string SaveEmployeDetails(PersonalDetails objDetails)
        {

            string message = "";
            try
            {
                _objIFinanceRepository.SavePersonalDetails(objDetails);
                SendCustomerDetailsToCarTrade(objDetails);

                if (_mobileVerRespo.IsMobileVerified(Convert.ToString(objDetails.MobileNumber), objDetails.EmailId))
                {
                    message = "Registered Mobile Number";
                    var numberList = _mobileVerCacheRepo.GetBlockedNumbers();

                    if (numberList != null && !numberList.Contains(Convert.ToString(objDetails.MobileNumber)))
                    {
                        // push in autobiz
                        NameValueCollection objNVC = new NameValueCollection();
                        objNVC.Add("pqId", objDetails.objLead.PQId.ToString());
                        objNVC.Add("dealerId", objDetails.objLead.DealerId.ToString());
                        objNVC.Add("customerName", objDetails.objLead.Name);
                        objNVC.Add("customerEmail", objDetails.EmailId);
                        objNVC.Add("customerMobile", Convert.ToString(objDetails.MobileNumber));
                        objNVC.Add("versionId", objDetails.objLead.VersionId.ToString());
                        objNVC.Add("pincodeId", Convert.ToString(objDetails.Pincode));
                        objNVC.Add("cityId", objDetails.objLead.CityId.ToString());
                        objNVC.Add("leadType", "2");
                        objNVC.Add("manufacturerDealerId", Convert.ToString(objDetails.objLead.ManufacturerDealerId));
                        objNVC.Add("manufacturerLeadId", Convert.ToString(objDetails.objLead.LeadId));
                        RabbitMqPublish objRMQPublish = new RabbitMqPublish();
                        objRMQPublish.PublishToQueue(Bikewale.Utility.BWConfiguration.Instance.LeadConsumerQueue, objNVC);
                    }
                }
                else
                {
                    message = "Not Registered Mobile Number";
                    MobileVerificationEntity mobileVer = null;
                    mobileVer = _mobileVerification.ProcessMobileVerification(objDetails.EmailId, Convert.ToString(objDetails.MobileNumber));
                    SMSTypes st = new SMSTypes();
                    st.SMSMobileVerification(Convert.ToString(objDetails.MobileNumber), string.Empty, mobileVer.CWICode, "PageUrl");

                }
            }
            catch (Exception ex)
            {

                ErrorClass err = new ErrorClass(ex, String.Format("CapitalFirst.SaveEmployeDetails({0})", Newtonsoft.Json.JsonConvert.SerializeObject(objDetails)));
            }
            return message;

        }
        public uint SavePersonalDetails(PersonalDetails objDetails, string Utmz, string Utma)
        {


            try
            {
                CustomerEntity objCust = GetCustomerId(objDetails, objDetails.MobileNumber);

                objDetails.LeadId = _manufacturerCampaignRepo.SaveManufacturerCampaignLead(
                     objDetails.objLead.DealerId,
                     objDetails.objLead.PQId,
                     objCust.CustomerId,
                     objDetails.objLead.Name,
                     objDetails.EmailId,
                     objDetails.MobileNumber,
                     objDetails.objLead.LeadSourceId,
                     Utma,
                     Utmz,
                     objDetails.objLead.DeviceId,
                     objDetails.objLead.CampaignId,
                     objDetails.objLead.LeadId
                    );
                //CT api

                var ctLeadId = SendCustomerDetailsToCarTrade(objDetails);

                objDetails.CTLeadId = ctLeadId;

                objDetails.Id = _objIFinanceRepository.SavePersonalDetails(objDetails);

                
            }
            catch (Exception ex)
            {

                ErrorClass err = new ErrorClass(ex, String.Format("CapitalFirst.SavePersonalDetails({0})", Newtonsoft.Json.JsonConvert.SerializeObject(objDetails)));
            }
            return objDetails.Id;

        }

        private uint SendCustomerDetailsToCarTrade(PersonalDetails objDetails)
        {
            uint ctLeadId = 0;
            try
            {
                if (objDetails != null)
                {
                    var bikemapping = _objIFinanceRepository.GetCapitalFirstBikeMapping(objDetails.objLead.VersionId);
                    if (bikemapping != null)
                    {
                        CTFormData formData = new CTFormData();
                        formData.action = CTApiAction;
                        formData.api_code = CTApiCode;
                        formData.bw_lead_id = objDetails.LeadId.ToString();
                        formData.company = objDetails.CompanyName;
                        formData.date_of_birth = objDetails.DateOfBirth.ToString("dd/MM/yyyy");
                        formData.email = objDetails.EmailId;
                        formData.emp_address1 = objDetails.OfficialAddressLine1;
                        formData.emp_address2 = objDetails.OfficialAddressLine2;
                        formData.emp_pincode = objDetails.PincodeOffice;
                        formData.emp_type = objDetails.Status == 1 ? "Salaried" : "Self-Employed";
                        formData.fname = objDetails.FirstName;
                        formData.from_source = "1"; // 1 - Desktop, 2 - Mobile
                        formData.gender = objDetails.Gender == 1 ? "Male" : "Female";
                        formData.gross_income = objDetails.AnnualIncome.ToString();
                        formData.lead_id = objDetails.CTLeadId.ToString();
                        formData.lname = objDetails.LastName;

                        formData.make = bikemapping.MakeBase.Make;
                        formData.model = bikemapping.ModelBase.ModelNo;

                        formData.mobile = objDetails.MobileNumber;
                        formData.m_status = objDetails.MaritalStatus == 1 ? "Married" : "Single";
                        formData.pan_number = objDetails.Pancard;
                        formData.res_address1 = objDetails.AddressLine1;
                        formData.res_address2 = objDetails.AddressLine2;
                        formData.res_pincode = objDetails.Pincode;


                        string jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(formData);


                        HttpContent httpContent = new StringContent(jsonString);
                        httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");


                        _httpClient = new HttpClient();
                        string response = string.Empty;
                        using (HttpResponseMessage _response = _httpClient.PostAsync(CTApiUrl, httpContent).Result)
                        {
                            if (_response.IsSuccessStatusCode && _response.StatusCode == System.Net.HttpStatusCode.OK)
                            {
                                response = _response.Content.ReadAsStringAsync().Result;
                                _response.Content.Dispose();
                                _response.Content = null;
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass err = new ErrorClass(ex, String.Format("CapitalFirst.SendCustomerDetailsToCarTrade({0})", Newtonsoft.Json.JsonConvert.SerializeObject(objDetails)));
            }
            return ctLeadId;
        }

        private CustomerEntity GetCustomerId(PersonalDetails objDetails, string MobileNumber)
        {

            CustomerEntity objCust = null;
            try
            {

                if (!_objAuthCustomer.IsRegisteredUser(objDetails.EmailId, MobileNumber))
                {
                    objCust = new CustomerEntity() { CustomerName = objDetails.objLead.Name, CustomerEmail = objDetails.EmailId, CustomerMobile = MobileNumber, ClientIP = "" };
                    objCust.CustomerId = _objCustomer.Add(objCust);

                }
                else
                {
                    var objCustomer = _objCustomer.GetByEmailMobile(objDetails.EmailId, MobileNumber);
                    objCust = new CustomerEntity()
                    {
                        CustomerId = objCustomer.CustomerId,
                        CustomerName = string.Format("{0} {1}",objDetails.FirstName,objDetails.LastName),
                        CustomerEmail = objDetails.EmailId = !String.IsNullOrEmpty(objDetails.EmailId) ? objDetails.EmailId : objCustomer.CustomerEmail,
                        CustomerMobile = MobileNumber
                    };
                    _objCustomer.Update(objCust);
                }
            }
            catch (Exception ex)
            {

                ErrorClass objErr = new ErrorClass(ex, "Bikewale.Service.Controllers.SavePersonalDetails");
            }
            return objCust;
        }
    }

    internal class CTFormData
    {
        public string lead_id { get; set; }
        public string bw_lead_id { get; set; }
        public string fname { get; set; }
        public string lname { get; set; }
        public string mobile { get; set; }
        public string email { get; set; }
        public string date_of_birth { get; set; }
        public string gender { get; set; }
        public string m_status { get; set; }
        public string res_address1 { get; set; }
        public string res_address2 { get; set; }
        public string res_pincode { get; set; }
        public string pan_number { get; set; }
        public string education { get; set; }
        public string emp_type { get; set; }
        public string company { get; set; }
        public string emp_address1 { get; set; }
        public string emp_address2 { get; set; }
        public string emp_pincode { get; set; }
        public String gross_income { get; set; }
        public string make { get; set; }
        public string model { get; set; }
        public string from_source { get; set; }
        public string action { get; set; }
        public string api_code { get; set; }
        public string button { get { return "Submit"; } }
    }
}
