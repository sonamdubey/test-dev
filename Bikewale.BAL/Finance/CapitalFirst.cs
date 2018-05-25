using Bikewale.Entities.Customer;
using Bikewale.Entities.Finance.CapitalFirst;
using Bikewale.Interfaces.Customer;
using Bikewale.Interfaces.Finance;
using Bikewale.Interfaces.Finance.CapitalFirst;
using Bikewale.Interfaces.MobileVerification;
using Bikewale.ManufacturerCampaign.Interface;
using Bikewale.Notifications;
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
        private readonly IDictionary<ushort, String> _leadStatusCollection = null;
        private const ushort SUCCESS_STATUS = 6;
        private readonly string _mediaContentType = "application/x-www-form-urlencoded";
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
            _leadStatusCollection.Add(1, "Mobile not verified.");
            _leadStatusCollection.Add(2, "Lead already exists.");
            _leadStatusCollection.Add(8, "Some error occured while processing your request. Please try after sometime.");
            _leadStatusCollection.Add(12, "Currently, our finance partner does not provide loan in your area.");
            _leadStatusCollection.Add(6, "Your application is submitted successfully.");
        }

        private CTFormResponse SendCustomerDetailsToCarTrade(PersonalDetails objDetails, ushort leadSource, bool isMobileVerified)
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
                        formData.Add(new KeyValuePair<string, string>("from_source", leadSource.ToString())); // 1 - Desktop, 2 - Mobile
                        if (objDetails.CtLeadId > 0)
                        {
                            formData.Add(new KeyValuePair<string, string>("lead_id", objDetails.CtLeadId.ToString()));
                        }
                        if (isMobileVerified)
                        {
                            formData.Add(new KeyValuePair<string, string>("otp_verified", "y"));
                        }
                        formData.Add(new KeyValuePair<string, string>("fname", objDetails.FirstName));
                        formData.Add(new KeyValuePair<string, string>("lname", objDetails.LastName));
                        formData.Add(new KeyValuePair<string, string>("email", objDetails.EmailId));
                        formData.Add(new KeyValuePair<string, string>("make", bikemapping.MakeBase.Make));
                        formData.Add(new KeyValuePair<string, string>("model", bikemapping.ModelBase.ModelNo));
                        formData.Add(new KeyValuePair<string, string>("mobile", objDetails.MobileNumber));

                        if (!String.IsNullOrEmpty(objDetails.Pancard))
                        {
                            formData.Add(new KeyValuePair<string, string>("pan_number", objDetails.Pancard));
                        }

                        formData.Add(new KeyValuePair<string, string>("res_pincode", objDetails.Pincode));

                        using (HttpContent httpContent = new FormUrlEncodedContent(formData))
                        {
                            httpContent.Headers.ContentType = new MediaTypeHeaderValue(_mediaContentType);
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

        /// <summary>
        /// Checks for customer validation and registration
        /// </summary>
        /// <param name="objDetails"></param>
        /// <param name="MobileNumber"></param>
        /// <returns></returns>
        private CustomerEntity GetCustomerId(PersonalDetails objDetails, string MobileNumber)
        {
            CustomerEntity objCust = null;
            try
            {
                //Check if customer already exists
                if (!_objAuthCustomer.IsRegisteredUser(objDetails.EmailId, MobileNumber))
                {
                    objCust = new CustomerEntity() { CustomerName = string.Format("{0} {1}", objDetails.FirstName, objDetails.LastName), CustomerEmail = objDetails.EmailId, CustomerMobile = MobileNumber };
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

        /// <summary>
        /// Saves the lead data to pq_newbikepricequote table
        /// </summary>
        /// <param name="objDetails"></param>
        /// <param name="Utmz"></param>
        /// <param name="Utma"></param>
        /// <param name="isMobileVerified"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Created by  :   Sumit Kate on 25 Jan 2018
        /// Description :   Push To Lead ConsumerQueue
        /// Saves the data to manufacturer lead table via lead push to Lead Consumer
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

        /// <summary>
        /// Created by  :   Sumit Kate on 24 May 2018
        /// Description :   Business Layer for Capital First lead
        /// </summary>
        /// <param name="objDetails"></param>
        /// <param name="utmz"></param>
        /// <param name="utma"></param>
        /// <param name="leadSource"></param>
        /// <returns></returns>
        public LeadResponseMessage SaveLead(PersonalDetails objDetails, string utmz, string utma, ushort leadSource)
        {
            LeadResponseMessage objId = null;
            bool isMobileVerified = false;
            try
            {
                #region Do not change the sequence
                if (objDetails.LeadId == 0)
                {
                    //Save Lead data to pq_newbikepricequote table
                    objDetails.LeadId = SubmitLead(objDetails, utmz, utma);
                }
                isMobileVerified = _mobileVerRespo.IsMobileVerified(objDetails.MobileNumber, objDetails.EmailId);
                //Push lead to consumer where data is saved to manufaturerlead table and lead is further pushed to AutoBiz
                PushToLeadConsumerQueue(objDetails);
                //Save Lead Details to Bikewale Capital First Lead Table with 
                objDetails.Id = _objIFinanceRepository.SaveCapitalFirstLeadData(objDetails, null);
                //Sent Details to CT API
                var ctResponse = SendCustomerDetailsToCarTrade(objDetails, leadSource, isMobileVerified);
                objId = new LeadResponseMessage();
                if (ctResponse != null)
                {
                    objId.CTleadId = objDetails.CtLeadId = ctResponse.LeadId;
                    objId.Status = ctResponse.Status;
                    objId.Message = _leadStatusCollection[ctResponse.Status];
                    //Update ct api response
                    _objIFinanceRepository.SaveCapitalFirstLeadData(objDetails, ctResponse);
                    if (ctResponse.Status == SUCCESS_STATUS)
                    {
                        NotifyCustomer(objDetails, ctResponse);
                    }
                }
                else
                {
                    //API call fails sent error code
                    objId.Status = 0;
                    objId.Message = _leadStatusCollection[0];
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

        /// <summary>
        /// Created by  :   Sumit Kate on 24 May 2018
        /// Description :   Send Email and SMS to customer
        /// </summary>
        /// <param name="objDetails"></param>
        /// <param name="ctResponse"></param>
        private void NotifyCustomer(PersonalDetails objDetails, CTFormResponse ctResponse)
        {

        }
    }
}
