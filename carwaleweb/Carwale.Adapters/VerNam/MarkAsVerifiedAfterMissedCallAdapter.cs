using AutoMapper;
using Carwale.DAL.ApiGateway.ApiGatewayHelper;
using Carwale.Entity.Vernam;
using Google.Protobuf;
using System;
using System.Configuration;
using VerNam.ProtoClass;

namespace Carwale.Adapters.VerNam
{
    public class MarkAsVerifiedAfterMissedCallAdapter : ApiGatewayAdapterBase<RequestData, bool, GrpcBool>
    {
        private static readonly string _moduleName = ConfigurationManager.AppSettings["VernamModuleName"] ?? string.Empty;
        private const string _methodName = "VerifyMissedCall";

        public MarkAsVerifiedAfterMissedCallAdapter() : base(_moduleName, _methodName)
        {
        }
        protected override bool BuildResponse(IMessage responseMessage)
        {
            if (responseMessage == null)
            {
                throw new ArgumentNullException("Parameter of type IMessage is null");
            }
            var response = responseMessage as GrpcBool;
            if (response == null)
            {
                throw new ArgumentException("IMessage should be of type GrpcString");
            }
            return response.Value;
        }
        protected override IMessage GetRequest(RequestData input)
        {
            if (input == null)
            {
                throw new ArgumentNullException("RequestData is null");
            }
            return Mapper.Map<VerifyMissedCallRequest>(input);
        }
    }
}
