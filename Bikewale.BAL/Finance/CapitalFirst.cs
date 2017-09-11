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
        public string SaveVoucherDetails(string ctLeadId, string jsonData)
        {
            string message = "";
            try
            {
                bool isValidLead = _objIFinanceRepository.IsValidLead(ctLeadId);
                if (isValidLead)
                {
                    NameValueCollection objNVC = new NameValueCollection();
                    objNVC.Add("ctLeadId", ctLeadId);
                    objNVC.Add("jsonData", jsonData);
                    RabbitMqPublish objRMQPublish = new RabbitMqPublish();
                    objRMQPublish.PublishToQueue(Bikewale.Utility.BWConfiguration.Instance.LeadConsumerQueue, objNVC);
                    message = "Success";
                }
                else
                {
                    message = "Invalid lead id or request body is empty";
                }
            }
            catch (Exception ex)
            {
                ErrorClass err = new ErrorClass(ex, String.Format("CapitalFirst.SaveVoucherDetails({0},{1})", ctLeadId, jsonData));
                message = "An error occured while saving voucher details";
            }
            return message;
        }
    }
}
