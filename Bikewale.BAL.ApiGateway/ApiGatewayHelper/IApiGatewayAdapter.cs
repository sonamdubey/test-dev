using Google.Protobuf;

namespace Bikewale.BAL.ApiGateway.ApiGatewayHelper
{
	/// <summary>
	/// Created By : Ashish G. Kamble on 4 Apr 2018
	/// Summary : Interface have method related to APIGateway Adapters
	/// </summary>
	/// <typeparam name="TInput"></typeparam>
	/// <typeparam name="TResult"></typeparam>
	/// <typeparam name="TApigatewayResponse"></typeparam>
	public interface IApiGatewayAdapter<TInput, TResult, TApigatewayResponse> where TApigatewayResponse : IMessage
	{
		/// <summary>
		/// Function to add call to the APIGateway
		/// </summary>
		/// <param name="apiGatewayCaller">Object of APIGateway caller. Call will be added to this object</param>
		/// <param name="input">Input parameters required for the APIGateway method</param>
		void AddApiGatewayCall(IApiGatewayCaller apiGatewayCaller, TInput input);
	}
}
