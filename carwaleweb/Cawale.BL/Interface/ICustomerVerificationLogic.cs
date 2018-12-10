using Carwale.Entity.CustomerVerification;
using Carwale.Entity.Enum;
using System;

namespace Carwale.BL.Interface
{
    public interface ICustomerVerificationLogic
    {
        /// <summary>
        /// initiate the customer verification process
        /// </summary>
        /// <param name="mobileNumber">mobile number that needs verification</param>
        /// <param name="platform">the platform Id</param>
        /// <param name="sourceModule"></param>
        /// <param name="ipAddress"></param>
        /// <returns></returns>
        MobileVerificationReponseEntity Initiate(string mobileNumber, Platform platform, string sourceModule = "", string ipAddress = "", Uri pageUrl = null);
        /// <summary>
        /// Check if number is already verified
        /// </summary>
        /// <param name="mobileNumber"></param>
        /// <returns></returns>
        bool IsNumberVerified(string mobileNumber);
    }
}
