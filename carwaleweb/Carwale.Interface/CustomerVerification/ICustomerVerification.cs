using Carwale.Entity.CustomerVerification;
using Carwale.Entity.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Interfaces.CustomerVerification
{
    public interface ICustomerVerification
    {
        MobileVerificationReponseEntity IsMobileVerified(string mobileNumber, string emailId, Platform platform, string pageUrl = "", int classifiedSourceId = 0);
        MobileVerificationReponseEntity IsMobileAndTokenVerified(string mobileNumber, Platform platform, string clientTokenId);
        bool ResendOtpSms(string email, string mobile, string pageUrl = "");
        bool IsMobileVerified(string mobileNumber);
        void SendToVerificationQueue(MobileVerification mobileVerification);
    }
}
