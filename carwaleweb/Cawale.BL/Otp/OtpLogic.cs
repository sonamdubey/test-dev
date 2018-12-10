using Carwale.DAL.ApiGateway;
using Carwale.DAL.ApiGateway.Extensions.Otp;
using Carwale.Entity.Enum;
using Carwale.Entity.Notifications;
using Carwale.Interfaces.Notifications;
using Carwale.Interfaces.Otp;
using Carwale.Notifications.Logs;
using OTP;
using System;

namespace Carwale.BL.Otp
{
    public class OtpLogic : IOtpLogic
    {

        private readonly IApiGatewayCaller _apiGatewayCaller;
        private readonly ISMSRepository _smsRepository;

        public OtpLogic(IApiGatewayCaller apiGatewayCaller, ISMSRepository smsRepository)
        {
            _apiGatewayCaller = apiGatewayCaller;
            _smsRepository = smsRepository;
        }

        /// <summary>
        /// Concrete implementation of IOtpLogic Generate method
        /// Uses Otp service through apigateway library
        /// </summary>
        /// <param name="mobileNumber"></param>
        /// <param name="application"></param>
        /// <param name="platform"></param>
        /// <param name="sourceModule"></param>
        /// <param name="ipAddress"></param>
        /// <returns></returns>
        public bool Generate(string mobileNumber, int application, Platform platform, string sourceModule, string ipAddress)
        {
            bool isCreated = false;

            if (_apiGatewayCaller.AggregateGenerateOtp(mobileNumber, application, platform, sourceModule, ipAddress))
            {
                _apiGatewayCaller.Call();

                try
                {
                    isCreated = _apiGatewayCaller.GetResponse<GrpcBool>(0).Value;
                }
                catch (GateWayException e)
                {
                    Logger.LogException(e);
                }
                catch (Exception e)
                {
                    Logger.LogException(e);
                }
            }
            return isCreated;

        }

        /// <summary>
        /// Concrete implementation of IOtpLogic Generate method
        /// Uses Otp service through apigateway library
        /// This method will be deprecated after development of sms micro service
        /// </summary>
        /// <param name="mobileNumber"></param>
        /// <param name="application"></param>
        /// <param name="platform"></param>
        /// <param name="sourceModule"></param>
        /// <param name="ipAddress"></param>
        /// <param name="msgId"></param>
        /// <returns></returns>
        public bool Generate(string mobileNumber, int application, Platform platform, string sourceModule, string ipAddress, int msgId)
        {
            bool isCreated = false;

            if (_apiGatewayCaller.AggregateGenerateOtpTemp(mobileNumber, application, platform, sourceModule, ipAddress, msgId))
            {
                _apiGatewayCaller.Call();
                try
                {
                    isCreated = _apiGatewayCaller.GetResponse<GrpcBool>(0).Value;
                }
                catch (GateWayException e)
                {
                    Logger.LogException(e);
                }
                catch (Exception e)
                {
                    Logger.LogException(e);
                }
            }
            return isCreated;

        }

        /// <summary>
        /// Concrete implementation of IOtpLogic Verify method
        /// Uses Otp service through apigateway library
        /// </summary>
        /// <param name="otpCode"></param>
        /// <param name="mobile"></param>
        /// <returns></returns>
        public bool Verify(string otpCode, string mobile)
        {
            bool isVerified = false;

            if (_apiGatewayCaller.AggregateVerifyOtp(otpCode, mobile))
            {
                _apiGatewayCaller.Call();
                try
                {
                    isVerified = _apiGatewayCaller.GetResponse<GrpcBool>(0).Value;
                }
                catch (GateWayException e)
                {
                    Logger.LogException(e);
                }
                catch (Exception e)
                {
                    Logger.LogException(e);
                }
            }
            return isVerified;

        }

        /// <summary>
        /// Concrete implementation of IOtpLogic IsNumberVerified method
        /// Uses Otp service through apigateway library
        /// </summary>
        /// <param name="mobileNumber"></param>
        /// <returns></returns>
        public bool IsNumberVerified(string mobileNumber)
        {
            bool isNumberVerified = false;
            if (_apiGatewayCaller.AggregateIsNumberVerified(mobileNumber))
            {
                _apiGatewayCaller.Call();
                try
                {
                    isNumberVerified = _apiGatewayCaller.GetResponse<GrpcBool>(0).Value;
                }
                catch (GateWayException e)
                {
                    Logger.LogException(e);
                }
                catch (Exception e)
                {
                    Logger.LogException(e);
                }
            }
            return isNumberVerified;
        }

        /// <summary>
        /// save sms data to database and return message id
        /// </summary>
        /// <param name="mobileNumber"></param>
        /// <param name="message"></param>
        /// <param name="smsType"></param>
        /// <param name="status"></param>
        /// <param name="returnedMsg"></param>
        /// <param name="pageUrl"></param>
        /// <returns></returns>
        public int SaveSmsData(string mobileNumber, string message, int smsType, bool status, string returnedMsg, Uri pageUrl = null)
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
            int.TryParse(smsId, out id);
            return id;
        }
    }
}
