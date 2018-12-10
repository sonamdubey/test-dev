using System;

namespace Carwale.DAL.ApiGateway.ApiGatewayHelper
{
    public abstract class ApiGatewayWidgetAdapterBase<TInput, TResult> : IApiGatewayWidgetAdapter<TInput, TResult>
    {
        /// <summary>
        /// Response object of widget for the current method
        /// </summary>
        public TResult Output { get; private set; }

        /// <summary>
        /// Function to convert GRPC message to the respective widget entity.
        /// Function should get response from particular adapter.
        /// This function should be overridden in derived class
        /// </summary>
        /// <param name="responseMessage">GRPC message</param>
        /// <returns>Returns widget entity as a response for the current adapter method</returns>
        protected abstract TResult BuildResponse(IApiGatewayCaller caller);

        /// <summary>
        /// 1. Method to add calls into APIGateway required for particular widget.
        /// 2. Callback functions will be added to the APIGateway Helper using adapter of particular methods.
        /// </summary>
        /// <param name="caller">Object of APIGateway caller. Call will be added to this object</param>
        /// <param name="input">Input parameters required for the APIGateway method. Pass input entity</param>
        protected abstract void AddApiGatewayCallsForWidget(IApiGatewayCaller caller, TInput input);


        public virtual void AddApiGatewayCall(IApiGatewayCaller caller, TInput input)
        {
                AddApiGatewayCallsForWidget(caller, input);
                Action<IApiGatewayCaller> callBack = ParseAPIResponse;
                caller.AddCallback(callBack);
        }

        /// <summary>
        /// Callback function which will be executed after receiving response from APIGateway.
        /// Function will get response from APIGateway and convert it to widget entity
        /// </summary>
        /// <param name="caller">Instance of the APIGateway caller</param>
        private void ParseAPIResponse(IApiGatewayCaller caller)
        {
            Output = BuildResponse(caller);
        }
    }
}
