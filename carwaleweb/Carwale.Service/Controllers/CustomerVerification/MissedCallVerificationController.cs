using Carwale.DAL.ApiGateway;
using Carwale.DAL.ApiGateway.ApiGatewayHelper;
using Carwale.Entity.Vernam;
using Carwale.Service.Filters;
using Microsoft.Practices.Unity;
using System.Web.Http;
using VerNam.ProtoClass;

namespace Carwale.Service.Controllers.CustomerVerification
{
    public class MissedCallVerificationController : ApiController
    {
        private readonly IApiGatewayAdapter<RequestData, bool, GrpcBool> _markVerifiedByMissedCallAdapter;
        private readonly IApiGatewayCaller _apiGatewayCaller;
        public MissedCallVerificationController([Dependency("MarkAsVerifiedAfterMissedCall")]IApiGatewayAdapter<RequestData, bool, GrpcBool> markVerifiedByMissedCallAdapter
            , IApiGatewayCaller apiGatewayCaller)
        {
            _markVerifiedByMissedCallAdapter = markVerifiedByMissedCallAdapter;
            _apiGatewayCaller = apiGatewayCaller;
        }

        [HttpGet, Route("api/mobile/verification/verifymissedcall/"), HandleException]
        public IHttpActionResult MarkVerified(string callSid, string from)
        {
            if(string.IsNullOrWhiteSpace(callSid))
            {
                return BadRequest("callSid cannot be empty");
            }
            if(string.IsNullOrWhiteSpace(from))
            {
                return BadRequest("mobile no. cannot be empty");
            }
            if(from.Length > 10)
            {
                from = from.Substring(from.Length - 10);
            }
            RequestData requestData = new RequestData
            {
                VerificationValue = from,
                VendorResponse = callSid
            };
            _markVerifiedByMissedCallAdapter.AddApiGatewayCallWithCallback(_apiGatewayCaller, requestData);
            _apiGatewayCaller.Call();
            return Ok();
        }

    }
}
