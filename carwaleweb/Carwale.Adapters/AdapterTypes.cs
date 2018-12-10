using Carwale.Adapters.VerNam;
using Carwale.DAL.ApiGateway.ApiGatewayHelper;
using Carwale.Entity.Vernam;
using Microsoft.Practices.Unity;
using VerNam.ProtoClass;

namespace Carwale.Adapters
{
    public static class AdapterTypes
    {
        public static void RegisterTypes(UnityContainer container)
        {
            container
                .RegisterType<IApiGatewayAdapter<RequestData, bool, GrpcBool>, VerifyOtpAdapter>("VerifyOtp")
                .RegisterType<IApiGatewayAdapter<RequestData, bool, GrpcBool>, IsVerifiedAdapter>("IsVerified")
                .RegisterType<IApiGatewayAdapter<RequestData, bool, GrpcBool>, IsVerifiedForDeviceAdapter>("IsVerifiedForDevice")
                .RegisterType<IApiGatewayAdapter<RequestData, bool, GrpcBool>, InitiateOtpAdapter>("InitiateOtp")
                .RegisterType<IApiGatewayAdapter<RequestData, string, GrpcString>, InitiateMissedCallVerificationAdapter>("InitiateMissedCallVerification")
                .RegisterType<IApiGatewayAdapter<RequestData, bool, GrpcBool>, MarkAsVerifiedAfterMissedCallAdapter>("MarkAsVerifiedAfterMissedCall");
        }
    }
}
