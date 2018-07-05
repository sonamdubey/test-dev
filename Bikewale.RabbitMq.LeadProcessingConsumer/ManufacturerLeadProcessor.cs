using Bikewale.Notifications;
using Bikewale.RabbitMq.LeadProcessingConsumer.AutoBizServiceRef;
using Consumer;
using System;

namespace Bikewale.RabbitMq.LeadProcessingConsumer
{
    /// <summary>
    /// Created by  :   Sumit Kate on 05 Jul 2017
    /// Description :   Manufacturer Lead Handler acts as base class for all manufacturer lead handlers
    /// </summary>
    internal abstract class ManufacturerLeadHandler : IManufacturerLeadHandler
    {
        protected LeadProcessingRepository LeadRepostiory { get { return _repository; } }
        protected string APIUrl { get { return _APIUrl; } }
        protected uint ManufacturerId { get { return _manufacturerId; } }

        private readonly LeadProcessingRepository _repository = null;
        private readonly string _APIUrl = "";
        private readonly uint _manufacturerId;
        private readonly bool _isAPIEnabled = false;
        private readonly bool _submitDuplicateLead = true;

        /// <summary>
        /// Type initializer
        /// </summary>
        /// <param name="manufacturerId"></param>
        public ManufacturerLeadHandler(uint manufacturerId)
        {
            _manufacturerId = manufacturerId;
            _repository = new LeadProcessingRepository();
        }
        /// <summary>
        /// Overloaded 1 Type initializer
        /// </summary>
        /// <param name="manufacturerId"></param>
        /// <param name="urlAPI"></param>
        /// <param name="isAPIEnabled"></param>
        public ManufacturerLeadHandler(uint manufacturerId, string urlAPI, bool isAPIEnabled) : this(manufacturerId)
        {
            _APIUrl = urlAPI;
            _isAPIEnabled = isAPIEnabled;
        }

        /// <summary>
        /// Overloaded 2 Type initializer
        /// </summary>
        /// <param name="manufacturerId"></param>
        /// <param name="urlAPI"></param>
        /// <param name="isAPIEnabled"></param>
        /// <param name="submitDuplicateLead"></param>
        public ManufacturerLeadHandler(uint manufacturerId, string urlAPI, bool isAPIEnabled, bool submitDuplicateLead) : this(manufacturerId, urlAPI, isAPIEnabled)
        {
            _submitDuplicateLead = submitDuplicateLead;
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 05 Jul 2017
        /// Description :   Lead processing functionality. This functional can be overridden by the child classes
        /// Modified BY : Kartik Rathod on 16 may 2018, send sms to customer on successfully pushing es campaign lead to AB system
        /// </summary>
        /// <param name="leadEntity"></param>
        /// <returns></returns>
        public virtual bool Process(ManufacturerLeadEntityBase leadEntity)
        {
            bool isSuccess = false;
            uint abInqId = 0;
            try
            {
                //Check if Manufacturer Lead Limit Exceeded
                if (!_repository.IsManufacturerLeadLimitExceed(leadEntity.CampaignId))
                {
                    //Push lead to AutoBiz
                    abInqId = PushLeadToAutoBiz(leadEntity.RetryAttempt, leadEntity.DealerId, leadEntity.InquiryJSON);
                    if (abInqId > 0)
                    {
                        // Send SMS to Customer for ES Manufacturer campaign if SendLeadSMSCustomer flag from manufacturercampaign table is true
                        if (leadEntity.SendLeadSMSCustomer)
                        {
                            SMSTypes.ESCampaignLeadSMSToCustomer(leadEntity.CustomerName, leadEntity.CustomerMobile, leadEntity.DealerName, leadEntity.BikeName);
                            Logs.WriteInfoLog(String.Format("EsCmapaign message successfully sent to customer on number - {0}", leadEntity.CustomerMobile));
                        }

                        //Update campaign daily lead count
                        isSuccess = _repository.UpdateManufacturerDailyLeadCount(leadEntity.CampaignId, abInqId);
                        //Update ABInquiry Id 
                        isSuccess = UpdateABInquiryId(leadEntity.LeadId, abInqId, leadEntity.RetryAttempt);

                        if (isSuccess && _isAPIEnabled)
                        {
                            if (IsDuplicateLead(leadEntity))
                            {
                                string response = PushLeadToManufacturer(leadEntity);
                                if (!String.IsNullOrEmpty(response))
                                {
                                    isSuccess = _repository.UpdateManufacturerLead(response, leadEntity.LeadId);
                                }
                            }
                        }
                    }
                }
                else
                {
                    //Send status as success if lead limit is reached. It will prevent message to re-queue for processing
                    isSuccess = true;
                }
            }
            catch (Exception ex)
            {
                Logs.WriteErrorLog(String.Format("ManufacturerLeadHandler.Process Err Msg : {0}, Lead : {1}", ex.Message, Newtonsoft.Json.JsonConvert.SerializeObject(leadEntity)));
            }
            return isSuccess;
        }

        protected virtual bool IsDuplicateLead(ManufacturerLeadEntityBase leadEntity)
        {
            return _submitDuplicateLead;
        }

        protected abstract string PushLeadToManufacturer(ManufacturerLeadEntityBase leadEntity);

        private UInt32 PushLeadToAutoBiz(ushort retryAttempt, uint dealerId, string inquiryJSON)
        {
            uint abInqId = 0;
            string abInquiryId = string.Empty;

            try
            {
                using (TCApi_Inquiry _inquiryAPI = new TCApi_Inquiry())
                {
                    abInquiryId = _inquiryAPI.AddNewCarInquiry(dealerId.ToString(), inquiryJSON);
                }
            }
            catch (Exception ex)
            {
                Logs.WriteErrorLog(String.Format("ManufacturerLeadHandler.PushLeadToAutoBiz(Msg : {0},{1},{2},{3})", ex.Message, retryAttempt, dealerId, inquiryJSON));
            }
            return (UInt32.TryParse(abInquiryId, out abInqId) && abInqId > 0) ? abInqId : 0;
        }

        private bool UpdateABInquiryId(uint leadId, uint abInqId, ushort retryAttempt)
        {
            return _repository.UpdateManufacturerABInquiry(leadId, abInqId, retryAttempt);
        }
    }
}
