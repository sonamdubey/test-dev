using Bikewale.Entities.Finance.CapitalFirst;
using Bikewale.Interfaces.Finance;
using Bikewale.Interfaces.Finance.CapitalFirst;
using Bikewale.Notifications;
using RabbitMqPublishing;
using System;
using System.Collections.Specialized;

namespace Bikewale.BAL.Finance
{
    /// <summary>
    /// Created by  :   Sumit Kate on 11 Sep 2017
    /// Description :   Capital First Business Layer.
    /// </summary>
    public class CapitalFirst : ICapitalFirst
    {
        private readonly IFinanceRepository _objIFinanceRepository = null;
        private const string CF_MESSAGE_SUCCESS = "Capital First Voucher and Agent details are saved successfully";
        private const string CF_MESSAGE_SAVE_FAILURE = "Error occured while saving voucher details";
        private const string CF_MESSAGE_INVALID = "Invalid lead id or request body is empty";
        private const string CF_MESSAGE_ERROR = "An error occured while saving voucher details";

        /// <summary>
        /// Created by  :   Sumit Kate on 11 Sep 2017
        /// Description :   Type Initializer
        /// </summary>
        /// <param name="objIFinanceRepository"></param>
        public CapitalFirst(IFinanceRepository objIFinanceRepository)
        {
            _objIFinanceRepository = objIFinanceRepository;
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
    }
}
