using Carwale.DAL.ApiGateway.ApiGatewayHelper;
using Carwale.Entity.Vernam;
using Google.Protobuf;
using System.Configuration;
using VerNam.ProtoClass;
using System;
using AutoMapper;

namespace Carwale.Adapters.VerNam
{
    public class VerifyOtpAdapter : ApiGatewayAdapterBase<RequestData, bool, GrpcBool>
    {
        private static readonly string _moduleName = ConfigurationManager.AppSettings["VernamModuleName"] ?? string.Empty;
        private static string _methodName = "VerifyOtp";

        public VerifyOtpAdapter() : base(_moduleName, _methodName) { }

        /// <summary>
        /// Function to convert Entity to GRPC Message which will be passed to the APIGateway
        /// </summary>
        /// <param name="input">input entity</param>
        /// <returns>Returns GRPC message</returns>
        protected override IMessage GetRequest(RequestData input)
        {
            if (input == null)
            {
                throw new ArgumentNullException("input cannot be null");
            }
            return Mapper.Map<VerifyOtpRequest>(input);
        }

        /// <summary>
        /// Function to convert output GRPC message to the output entity
        /// </summary>
        /// <param name="responseMessage">GRPC message</param>
        /// <returns>Returns campaign</returns>
        protected override bool BuildResponse(IMessage responseMessage)
        {
            if(responseMessage == null)
            {
                throw new ArgumentNullException("responseMessage cannot be null");
            }
            var response = responseMessage as GrpcBool;
            if(response == null)
            {
                throw new ArgumentException("responseMessage must be of type IMessage");
            }
            return response.Value;
        }
    }
}
