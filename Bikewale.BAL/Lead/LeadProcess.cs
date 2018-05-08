using Bikewale.Entities.Customer;
using Bikewale.Entities.Dealer;
using Bikewale.Interfaces.BikeBooking;
using Bikewale.Interfaces.Customer;
using Bikewale.Interfaces.Dealer;
using Bikewale.Interfaces.Lead;
using Bikewale.Interfaces.MobileVerification;
using Bikewale.Interfaces.PriceQuote;
using Bikewale.ManufacturerCampaign.Interface;
using RabbitMqPublishing;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.Specialized;
using Bikewale.Entities.PriceQuote;
using Bikewale.Notifications;
using Bikewale.ManufacturerCampaign.Entities;

namespace Bikewale.BAL.Lead
{
    /// <summary>
    /// Created by : Snehal Dange on 2nd May 2018
    /// Description : To manage dealer and manufacture leads related methods
    /// </summary>
    public class LeadProcess : ILead
    {
        private readonly ICustomerAuthentication<CustomerEntity, UInt32> _objAuthCustomer = null;
        private readonly ICustomer<CustomerEntity, UInt32> _objCustomer = null;
        private readonly IDealerPriceQuote _objDealerPriceQuote = null;
        private readonly IMobileVerificationRepository _mobileVerRespo = null;
        private readonly IMobileVerificationCache _mobileVerCacheRepo = null;
        private readonly IMobileVerification _mobileVerification = null;
        private readonly IDealer _objDealer = null;
        private readonly IPriceQuote _objPriceQuote = null;
        private readonly ILeadNofitication _objLeadNofitication = null;
        private readonly IManufacturerCampaignRepository _manufacturerCampaignRepo = null;


        public LeadProcess(
            ICustomerAuthentication<CustomerEntity, UInt32> objAuthCustomer,
            ICustomer<CustomerEntity, UInt32> objCustomer,
            IDealerPriceQuote objDealerPriceQuote,
            IMobileVerificationRepository mobileVerRespo,
            IMobileVerification mobileVerificetion,
            IDealer objDealer,
            IPriceQuote objPriceQuote, ILeadNofitication objLeadNofitication, IMobileVerificationCache mobileVerCacheRepo,
            IManufacturerCampaignRepository manufacturerCampaignRepo)
        {
            _objAuthCustomer = objAuthCustomer;
            _objCustomer = objCustomer;
            _objDealerPriceQuote = objDealerPriceQuote;
            _mobileVerRespo = mobileVerRespo;
            _mobileVerification = mobileVerificetion;
            _objDealer = objDealer;
            _objPriceQuote = objPriceQuote;
            _objLeadNofitication = objLeadNofitication;
            _mobileVerCacheRepo = mobileVerCacheRepo;
            _manufacturerCampaignRepo = manufacturerCampaignRepo;
        }

        /// <summary>
        /// Created By : Deepak Israni on 4 May 2018
        /// Description: BAL function to process manufacturer leads.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="headers"></param>
        public uint ProcessESLead(ManufacturerLeadEntity input, NameValueCollection headers)
        {
            uint leadId = 0;

            try
            {
                String utma = headers["UTMA"];
                String utmz = headers["UTMZ"];
                //String platformId = headers["PlatformId"];

                if (input.CityId > 0 && input.VersionId > 0 && input.PQId > 0 && !String.IsNullOrEmpty(input.Name) && !String.IsNullOrEmpty(input.Mobile) && input.DealerId > 0)
                {
                    CustomerEntity objCust = GetCustomerEntity(input.Name, input.Mobile, input.Email);

                    ES_SaveEntity leadInfo = new ES_SaveEntity
                    {
                        DealerId = input.DealerId,
                        PQId = input.PQId,
                        CustomerId = objCust.CustomerId,
                        CustomerName = input.Name,
                        CustomerEmail = input.Email,
                        CustomerMobile = input.Mobile,
                        LeadSourceId = input.LeadSourceId,
                        UTMA = utma,
                        UTMZ = utmz,
                        DeviceId = input.DeviceId,
                        CampaignId = input.CampaignId,
                        LeadId = input.LeadId
                    };

                    input.LeadId = leadId = _manufacturerCampaignRepo.SaveManufacturerCampaignLead(leadInfo);

                    if (leadId > 0)
                    {
                        IEnumerable<String> numberList = _mobileVerCacheRepo.GetBlockedNumbers();

                        if (numberList != null && !numberList.Contains(input.Mobile))
                        {
                            PushToLeadConsumer(input);

                            if (input.CampaignId == Utility.BWConfiguration.Instance.KawasakiCampaignId)
                            {
                                SMSKawasaki(input);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Exception : Bikewale.BAL.Lead.ProcessESLead : " + Newtonsoft.Json.JsonConvert.SerializeObject(input));
            }

            return leadId;
        }

        /// <summary>
        /// Created By : Deepak Israni on 4 May 2018
        /// Description: Checks if customer exists and if not creates a new customer entity.
        /// </summary>
        /// <param name="customerName"></param>
        /// <param name="mobile"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        private CustomerEntity GetCustomerEntity(string customerName, string mobile, string email)
        {
            CustomerEntity objCust = null;

            if (!_objAuthCustomer.IsRegisteredUser(email, mobile))
            {
                objCust = new CustomerEntity() { CustomerName = customerName, CustomerEmail = email, CustomerMobile = mobile, ClientIP = "" };
                objCust.CustomerId = _objCustomer.Add(objCust);
            }
            else
            {
                objCust = _objCustomer.GetByEmailMobile(email, mobile);

                objCust.CustomerName = customerName;
                objCust.CustomerEmail = !String.IsNullOrEmpty(email) ? email : objCust.CustomerEmail;
                objCust.CustomerMobile = mobile;

                _objCustomer.Update(objCust);
            }

            return objCust;
        }

        /// <summary>
        /// Created By : Deepak Israni on 4 May 2018
        /// Description: Pushes lead to Lead Processing Consumer.
        /// </summary>
        /// <param name="input"></param>
        private static void PushToLeadConsumer(ManufacturerLeadEntity input)
        {
            NameValueCollection objNVC = new NameValueCollection();

            objNVC.Add("pqId", input.PQId.ToString());
            objNVC.Add("dealerId", input.DealerId.ToString());
            objNVC.Add("customerName", input.Name);
            objNVC.Add("customerEmail", input.Email);
            objNVC.Add("customerMobile", input.Mobile);
            objNVC.Add("versionId", input.VersionId.ToString());
            objNVC.Add("pincodeId", input.PinCode.ToString());
            objNVC.Add("cityId", input.CityId.ToString());
            objNVC.Add("leadType", "2");
            objNVC.Add("manufacturerDealerId", input.ManufacturerDealerId.ToString());
            objNVC.Add("manufacturerLeadId", input.LeadId.ToString());

            RabbitMqPublish objRMQPublish = new RabbitMqPublish();
            objRMQPublish.PublishToQueue(Bikewale.Utility.BWConfiguration.Instance.LeadConsumerQueue, objNVC);
        }

        private void SMSKawasaki(ManufacturerLeadEntity objLead)
        {
            DPQSmsEntity objDPQSmsEntity = new DPQSmsEntity();
            objDPQSmsEntity.CustomerMobile = objLead.Mobile;
            objDPQSmsEntity.CustomerName = objLead.Name;
            objDPQSmsEntity.DealerName = objLead.ManufacturerDealer;
            SendEmailSMSToDealerCustomer.SendSMSToCustomer(objLead.PQId, string.Empty, objDPQSmsEntity, DPQTypes.KawasakiCampaign);
        }


    }
}
