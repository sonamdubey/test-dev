using Google.Protobuf;

namespace Carwale.DAL.ApiGateway.ApiGatewayHelper
{
    /// <summary>
    /// Created By : Uday Solanki
    /// Summary : Interface have method related to APIGateway Adapters
    /// </summary>
    /// <typeparam name="TInput"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    /// <typeparam name="TApigatewayResponse"></typeparam>
    public interface IApiGatewayAdapter<TInput, TResult, TApigatewayResponse> where TApigatewayResponse : IMessage
    {
        /// <summary>
        /// Response object for the current method
        /// </summary>
        TResult Output { get; }

        /// <summary>
        /// Function to add call to the APIGateway
        /// </summary>
        /// <param name="caller">Object of APIGateway caller. Call will be added to this object</param>
        /// <param name="input">Input parameters required for the APIGateway method</param>
        void AddApiGatewayCallWithCallback(IApiGatewayCaller caller, TInput input);

        /// <summary>
        /// Function to add call to the APIGateway
        /// </summary>
        /// <param name="caller">Object of APIGateway caller. Call will be added to this object</param>
        /// <param name="input">Input parameters required for the APIGateway method</param>
        void AddApiGatewayCall(IApiGatewayCaller caller, TInput input);

        /// <summary>
        /// Function to create unique requestId string using input entity for respective call.
        /// </summary>
        /// <param name="input">Input parameters required for the APIGateway method. Pass input entity</param>
        /// <returns>Returns unique requestId string </returns>
        string SetRequestId(TInput input);
    }
}
