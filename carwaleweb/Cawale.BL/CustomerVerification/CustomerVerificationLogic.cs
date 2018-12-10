using System;
using Carwale.BL.Interface;
using Carwale.Entity.CustomerVerification;
using Carwale.Interfaces.Otp;
using Carwale.Entity.Enum;
using Carwale.Interfaces.Notifications;
using Carwale.Entity.Notifications;

namespace Carwale.BL.CustomerVerification
{
    public class CustomerVerificationLogic : ICustomerVerificationLogic
    {
        private readonly IOtpLogic _otpLogic;
        private readonly ISMSRepository _smsRepository;
        private const int _applicationId = 1;
        public CustomerVerificationLogic(IOtpLogic otpLogic, ISMSRepository smsRepository)
        {
            _otpLogic = otpLogic;
            _smsRepository = smsRepository;
        }
        public MobileVerificationReponseEntity Initiate(string mobileNumber, Platform platform, string sourceModule = "", string ipAddress = "", Uri pageUrl = null)
        {
            MobileVerificationReponseEntity response = new MobileVerificationReponseEntity();
            response.IsMobileVerified = _otpLogic.IsNumberVerified(mobileNumber);
            if (!response.IsMobileVerified)
            {
                int id;
                string smsId = _smsRepository.SaveSMSSentData(new SMS
                {
                    Message = "sending otp",
                    Mobile = mobileNumber,
                    ReturnedMsg = string.Empty,
                    Status = true,
                    SMSType = (int)SMSType.MobileVerification,
                    PageUrl = pageUrl != null ? pageUrl.AbsolutePath : string.Empty
                });
                if(int.TryParse(smsId, out id))
                {
                    response.IsVerificationCodeSent = _otpLogic.Generate(mobileNumber, _applicationId, platform, sourceModule, ipAddress, id);
                }
            }
            return response;
        }

        public bool IsNumberVerified(string mobileNumber)
        {
            return _otpLogic.IsNumberVerified(mobileNumber);
        }
    }
}
