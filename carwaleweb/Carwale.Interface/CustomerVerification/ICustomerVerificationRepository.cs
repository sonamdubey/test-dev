using Carwale.Entity.Enum;
using System.Collections.Generic;

namespace Carwale.Interfaces.CustomerVerification
{
    public interface ICustomerVerificationRepository
    {
        bool VerifyMobile(string mobileNumber, string emailId, Platform platformId, out string otpCode, string cvid, int source);
        bool VerifyMobile(string mobileNumber, string emailId, Platform platformId, out string otpCode, string cvid, int source, string clientTokenId);
        bool IsVerified(string mobile, string clientTokenId);
        bool ProcessTollFreeNumber(string mobileNumber, string emailId, Platform platformId, int source, string clientTokenId);
        string GetOtpCode(string email, string mobile);
        bool CheckVerification(string mobile, string cwiCode, string cuiCode, string email, string clientTokenId, int sourceId = 0);
        void VerifyByMissedCall(string mobile, string transToken, string toCall, out string email, int sourceId = 0);
        bool IsMobileVerified(string mobileNumber);
        void DeleteVerifiedMobileNos(IEnumerable<string> mobiles);
    }
}
