
namespace Carwale.DAL.ApiGateway.ApiGatewayHelper
{
    /// <summary>
    /// Created By : Uday Solanki
    /// Summary : Interface have methods to interact with AEPL ApiGateway.
    /// </summary>
    public interface IApiGatewayWidgetAdapter<TInput, TResult>
    {
        /// <summary>
        /// Response object of widget for the current method
        /// </summary>
        TResult Output { get; }

        void AddApiGatewayCall(IApiGatewayCaller caller, TInput input);
    }
}
