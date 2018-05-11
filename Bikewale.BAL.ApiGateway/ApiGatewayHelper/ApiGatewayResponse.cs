using Google.Protobuf;

namespace Bikewale.BAL.ApiGateway.ApiGatewayHelper
{
	/// <summary>
	/// Created By : Ashish G. Kamble on 2 Apr 2018
	/// Summary : Class to hold response from APIGateway
	/// </summary>
	public class ApiGatewayResponse
	{
		/// <summary>
		/// APIGateway response
		/// </summary>
		public ByteString Payload { get; set; }

		/// <summary>
		/// Exception returned from APIGateway for a response
		/// </summary>
		public string Exception { get; set; }

		/// <summary>
		/// Exception code returned from APIGateway for a response
		/// </summary>
		public string ExceptionCode { get; set; }

	}	// class
}	// namespace
