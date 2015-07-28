using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Interfaces.MobileVerification
{
    /// <summary>
    /// Created By : Ashish G. Kamble on 18 Apr 2014
    /// Summary : Interface for entire mobile verification process.
    /// </summary>    
    public interface IMobileVerificationRepository
    {
        bool IsMobileVerified(string mobileNo, string emailId);
        ulong AddMobileNoToPendingList(string mobileNo, string emailId, string cwiCode, string cuiCode);
        bool VerifyMobileVerificationCode(string mobileNo, string cwiCode, string cuiCode);
    }
}