using System.Collections.Generic;

namespace Bikewale.Interfaces.MobileVerification
{
    /// <summary>
    /// Created By : Ashish G. Kamble on 18 Apr 2014
    /// Summary : Interface for entire mobile verification process.
    /// Modified by : Aditi Srivastava on 14 Feb 2017
    /// Summary     : Added function to get list of blocked numbers
    /// </summary>    
    public interface IMobileVerificationRepository
    {
        bool IsMobileVerified(string mobileNo, string emailId);
        sbyte OTPAttemptsMade(string mobileNo, string emailId);
        ulong AddMobileNoToPendingList(string mobileNo, string emailId, string cwiCode, string cuiCode);
        bool VerifyMobileVerificationCode(string mobileNo, string cwiCode, string cuiCode);
        IEnumerable<string> GetBlockedPhoneNumbers();
    }
}