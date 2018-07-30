using Bikewale.Entities.MobileVerification;

namespace Bikewale.Interfaces.MobileVerification
{
    /// <summary>
    /// Created by : Ashish G. Kamble on 24 Apr 2014
    /// Summary : Interface to be used for business layer.
    /// </summary>
    public interface IMobileVerification
    {
        MobileVerificationEntity ProcessMobileVerification(string email, string mobile);

    }   // class
}   // namespace
