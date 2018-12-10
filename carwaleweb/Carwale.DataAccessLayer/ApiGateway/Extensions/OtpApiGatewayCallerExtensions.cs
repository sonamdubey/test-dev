using AEPLCore.Logging;
using Carwale.Entity.Enum;
using Google.Protobuf;
using OTP;
using System;
using System.Configuration;

namespace Carwale.DAL.ApiGateway.Extensions.Otp
{
	public static class OtpApiGatewayCallerExtensions
	{
		private static Logger Logger = LoggerFactory.GetLogger();
		private static string _module = ConfigurationManager.AppSettings["OtpModuleName"];

		/// <summary>
		/// Aggregrate the call for Generating the Otp in ApiGateway Aggregator
		/// </summary>
		/// <param name="caller"></param>
		/// <param name="mobileNumber"></param>
		/// <param name="application"></param>
		/// <param name="platform"></param>
		/// <param name="sourceModule"></param>
		/// <param name="ipAddress"></param>
		/// <returns></returns>
		public static bool AggregateGenerateOtp(this IApiGatewayCaller caller, string mobileNumber, int application, Platform platform, string sourceModule, string ipAddress)
		{
			bool isCallAdded = false;
			try
			{
				IMessage message = new GetOtpRequest
				{
					MobileNumber = mobileNumber,
					Application = application,
					Platform = (int)platform,
					SourceModule = sourceModule,
					IpAddress = ipAddress
				};
				if (caller != null)
				{
					caller.Add(_module, "GenerateOtp", message);
					isCallAdded = true; 
				}
			}
			catch (Exception e)
			{
				Logger.LogException(e);
			}
			return isCallAdded;
		}

		/// <summary>
		/// Aggregating the call for verifying the Otp in ApiGateway Aggregator
		/// </summary>
		/// <param name="caller"></param>
		/// <param name="otpCode"></param>
		/// <param name="mobileNumber"></param>
		/// <returns></returns>
		public static bool AggregateVerifyOtp(this IApiGatewayCaller caller, string otpCode, string mobileNumber)
		{
			bool isCallAdded = false;
			try
			{
				IMessage message = new VerifyRequest
				{
					MobileNumber = mobileNumber,
					OtpCode = otpCode
				};
				if (caller != null)
				{
					caller.Add(_module, "VerifyOtp", message);
					isCallAdded = true;
				}
			}
			catch (Exception e)
			{
				Logger.LogException(e);
			}
			return isCallAdded;
		}

		/// <summary>
		/// Aggregrate the call for Generating the Otp in ApiGateway Aggregator
		/// </summary>
		/// <param name="caller"></param>
		/// <param name="mobileNumber"></param>
		/// <param name="application"></param>
		/// <param name="platform"></param>
		/// <param name="sourceModule"></param>
		/// <param name="ipAddress"></param>
		/// <param name="msgId"></param>
		/// <returns></returns>
		public static bool AggregateGenerateOtpTemp(this IApiGatewayCaller caller, string mobileNumber, int application, Platform platform, string sourceModule, string ipAddress, int msgId)
		{
			bool isCallAdded = false;
			try
			{
				IMessage message = new GetOtpRequestTemp
				{
					MobileNumber = mobileNumber,
					Application = application,
					Platform = (int)platform,
					SourceModule = sourceModule,
					IpAddress = ipAddress,
					SmsSentId = msgId
				};
				if (caller != null)
				{
					caller.Add(_module, "GenerateOtpTemp", message);
					isCallAdded = true;
				}
			}
			catch (Exception e)
			{
				Logger.LogException(e);
			}
			return isCallAdded;
		}

		/// <summary>
		/// Aggregate the call to check if number is already verified
		/// </summary>
		/// <param name="caller">extending class</param>
		/// <param name="mobileNumber">The number under verification check</param>
		/// <returns></returns>
		public static bool AggregateIsNumberVerified(this IApiGatewayCaller caller, string mobileNumber)
		{
			bool isCallAdded = false;
			IMessage message = new IsNumberVerifiedRequest
			{
				MobileNumber = mobileNumber
			};
			if (caller != null)
			{
				try
				{
					caller.Add(_module, "IsNumberVerified", message);
					isCallAdded = true;
				}
				catch (Exception e)
				{
					Logger.LogException(e);
				}
			}
			return isCallAdded;
		}
	}
}
