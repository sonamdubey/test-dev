using AutoMapper;
using Carwale.DAL.ApiGateway.ApiGatewayHelper;
using Carwale.Entity.Vernam;
using Google.Protobuf;
using System;
using System.Configuration;
using VerNam.ProtoClass;

namespace Carwale.Adapters.VerNam
{
    public class IsVerifiedAdapter : ApiGatewayAdapterBase<RequestData, bool, GrpcBool>
    {
        private static readonly string _moduleName = ConfigurationManager.AppSettings["VernamModuleName"] ?? string.Empty;
        private const string _methodName = "IsVerified";
        public IsVerifiedAdapter() : base(_moduleName, _methodName)
        {
        }

        protected override bool BuildResponse(IMessage responseMessage)
        {
            if(responseMessage == null)
            {
                throw new ArgumentNullException("IMessage should not be null");
            }
            var response = responseMessage as GrpcBool;
            if (response == null)
            {
                throw new ArgumentException("IMessage shoould be of type GrpcBool");
            }
            return response.Value;
        }

        protected override IMessage GetRequest(RequestData input)
        {
            if(input == null)
            {
                throw new ArgumentNullException("RequestData cannot be null");
            }
            return Mapper.Map<IsVerifiedRequest>(input);
        }
    }
}
