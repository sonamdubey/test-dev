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

        private const string CF_MESSAGE_SUCCESS = "Capital First Voucher and Agent details are saved successfully";
        private const string CF_MESSAGE_SAVE_FAILURE = "Error occured while saving voucher details";
        private const string CF_MESSAGE_INVALID = "Invalid lead id or request body is empty";
        private const string CF_MESSAGE_ERROR = "An error occured while saving voucher details";

        
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
                message = CF_MESSAGE_ERROR;
            }
            return message;
        }

        public string SaveEmployeDetails(PersonalDetails objDetails)
        {

            string message = "";
            _objIFinanceRepository.SavePersonalDetails(objDetails);


            if (_mobileVerRespo.IsMobileVerified(Convert.ToString(objDetails.MobileNumber), objDetails.EmailId))
            {
                message = "Registered Mobile Number";
                var numberList = _mobileVerCacheRepo.GetBlockedNumbers();

                if (numberList != null && !numberList.Contains(Convert.ToString(objDetails.MobileNumber)))
                {
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
            return message;

        }
        public bool SavePersonalDetails(PersonalDetails objDetails, string Utmz, string Utma)
        {
           

            CustomerEntity objCust= GetCustomerId(objDetails, objDetails.MobileNumber);
            
            objDetails.objLead.LeadId = _manufacturerCampaignRepo.SaveManufacturerCampaignLead(
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

            _objIFinanceRepository.SavePersonalDetails(objDetails);

            return true;

        }
        private CustomerEntity GetCustomerId(PersonalDetails objDetails,string MobileNumber)
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
                        CustomerName = objDetails.objLead.Name,
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
}
