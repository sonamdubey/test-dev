using Carwale.Entity.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Interfaces.Otp
{
    public interface IOtpLogic
    {
        /// <summary>
        /// Generate Otp for specified number
        /// </summary>
        /// <param name="mobileNumber">mobile number on which otp will be sent</param>
        /// <param name="application">the application id (1:CarWale)</param>
        /// <param name="platform">the platform id (1: desktop, 43:mobile)</param>
        /// <param name="sourceModule">the requesting module</param>
        /// <param name="ipAddress">ip address of the requestor</param>
        /// <returns>bool</returns>
        bool Generate(string mobileNumber, int application, Platform platform, string sourceModule, string ipAddress);
        /// <summary>
        /// Generate Otp for specified number.
        /// This method will be deprecated after development of sms micro service
        /// </summary>
        /// <param name="mobileNumber">mobile number on which otp will be sent</param>
        /// <param name="application">the application id (1:CarWale)</param>
        /// <param name="platform">the platform id (1: desktop, 43:mobile)</param>
        /// <param name="sourceModule">the requesting module</param>
        /// <param name="ipAddress">ip address of the requestor</param>
        /// <param name="msgId">the smssent table id</param>
        /// <returns>bool</returns>
        bool Generate(string mobileNumber, int application, Platform platform, string sourceModule, string ipAddress, int msgId);
        /// <summary>
        /// Verify the otp for a particular mobile number
        /// </summary>
        /// <param name="otpCode">the otp code received</param>
        /// <param name="mobile">the mobile for which otp was received</param>
        /// <returns>bool</returns>
        bool Verify(string otpCode, string mobile);
        /// <summary>
        /// Check whether number was verified previously
        /// </summary>
        /// <param name="mobileNumber">mobile number under verification</param>
        /// <returns>bool</returns>
        bool IsNumberVerified(string mobileNumber);

        /// <summary>
        /// To send the otp, this method is required to get the sms sent id.[this is temporory and will be removed once sms sending functionality
        /// is moved to sms microservice]
        /// </summary>
        /// <param name="mobile">Mobile nummber to send message</param>
        /// <param name="message">Message to be sent</param>
        /// <param name="pageUrl">page url</param>
        /// <param name="smsType">Type of message i.e mobile vefication etc</param>
        /// <param name="status">status</param>
        /// <param name="returnedMsg">return message</param>
        /// <returns></returns>
        int SaveSmsData(string mobileNumber, string message, int smsType, bool status, string returnedMsg, Uri pageUrl = null);
    }
}
