using AutoMapper;
using Carwale.DAL.ApiGateway.ApiGatewayHelper;
using Carwale.Entity.CarData;
using Google.Protobuf;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Adapters.Specs
{
    public class SpecsImageDetailsAdaptor : ApiGatewayAdapterBase<SpecsImageDetailRequest, SpecsInfo, VehicleData.Service.ProtoClass.SpecsInfo>
    {
        private static readonly string _moduleName = ConfigurationManager.AppSettings["CarDataModuleName"] ?? string.Empty;
        private static readonly string _methodName = "GetSpecsInfo";
        public SpecsImageDetailsAdaptor() : base(_moduleName,_methodName)
        {

        }
        /// <summary>
        /// Function to convert Entity to GRPC Message which will be passed to the APIGateway
        /// </summary>
        /// <param name="input">input entity</param>
        /// <returns>Returns GRPC message</returns>
        protected override IMessage GetRequest(SpecsImageDetailRequest input)
        {
            SpecsImageDetailRequest request = input;
            var specsDetailsRequest = Mapper.Map<VehicleData.Service.ProtoClass.SpecsInfoRequest>(input);
            return specsDetailsRequest;
        }

        /// <summary>
        /// Function to convert output GRPC message to the output entity
        /// </summary>
        /// <param name="responseMessage">GRPC message</param>
        /// <returns>Returns Specs Image Details</returns>
        protected override SpecsInfo BuildResponse(IMessage responseMessage)
        {
            if (responseMessage == null)
            {
                return null;
            }

            var specsImageDetails = new SpecsInfo();
            specsImageDetails = Mapper.Map<VehicleData.Service.ProtoClass.SpecsInfo, SpecsInfo>(responseMessage as VehicleData.Service.ProtoClass.SpecsInfo);
            return specsImageDetails;
        }
    }
}
