using Bikewale.Entities.Finance.CapitalFirst;
using Bikewale.Interfaces.Finance;
using Bikewale.Interfaces.Finance.CapitalFirst;
using Bikewale.Interfaces.MobileVerification;
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
        private readonly IMobileVerificationRepository _mobileVerRespo = null;
        /// <summary>
        /// Created by  :   Sumit Kate on 11 Sep 2017
        /// Description :   Type Initializer
        /// </summary>
        /// <param name="objIFinanceRepository"></param>
        public CapitalFirst(IFinanceRepository objIFinanceRepository, IMobileVerificationRepository mobileVerRespo)
        {
            _objIFinanceRepository = objIFinanceRepository;
            _mobileVerRespo = mobileVerRespo;
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
                    objRMQPublish.PublishToQueue(Utility.BWConfiguration.Instance.LeadConsumerQueue, objNVC);
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

        public bool SaveEmployeDetails(PersonalDetails objDetails) {

            bool success = false;
            _objIFinanceRepository.SaveEmployeDetails(objDetails);
            if (_mobileVerRespo.IsMobileVerified(Convert.ToString(objDetails.MobileNumber),objDetails.EmailId))
            {
                success = true;
                // code for autobiz push()
            }


                return success;



        }

    }
}
