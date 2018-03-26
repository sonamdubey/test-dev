using Google.Protobuf;

namespace Bikewale.Utility.ApiGatewayHelper
{
	public class ApiGatewayResponse
	{
		public ByteString Payload { get; set; }
		public string Exception { get; set; }
		public string ExceptionCode { get; set; }
	}
}
