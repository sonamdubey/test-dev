using Carwale.Notifications.Logs;
using Google.Protobuf;
using Newtonsoft.Json;
using System;

namespace Carwale.DAL.ApiGateway.ApiGatewayHelper
{
    /// <summary>
    /// Created By : Uday Solanki 
    /// Summary : Class defines template to call the APIGateway. Adapters which needs to implement GRPC method should inherit from this class.
    /// </summary>
    /// <typeparam name="TInput">Input entity</typeparam>
    /// <typeparam name="TResult">Output entity</typeparam>
    /// <typeparam name="TApigatewayResponse">GRPC response message</typeparam>

    public abstract class ApiGatewayAdapterBase<TInput, TResult, TApigatewayResponse> : IApiGatewayAdapter<TInput, TResult, TApigatewayResponse> where TApigatewayResponse : IMessage
    {
        /// <summary>
        /// Module name for microservice
        /// </summary>
        private readonly string _moduleName;

        /// <summary>
        /// Method name in a module
        /// </summary>
        private readonly string _methodName;

        /// <summary>
        /// identifier for respective adapter apigateway call
        /// </summary>
        private string _requestId;

        /// <summary>
        /// Response object of widget for the current method
        /// </summary>
        public TResult Output { get; private set; }

        /// <summary>
        /// Constructor to initialize the properties required call the GRPC method
        /// </summary>
        /// <param name="moduleName"></param>
        /// <param name="methodName"></param>
        protected ApiGatewayAdapterBase(string moduleName, string methodName)
        {
            _moduleName = moduleName;
            _methodName = methodName;
        }

        /// <summary>
        /// Function to convert Entity to GRPC Message which will be passed to the APIGateway. This function should be overridden in derived class
        /// </summary>
        /// <param name="input">input entity</param>
        /// <returns>Returns GRPC message</returns>
        protected abstract IMessage GetRequest(TInput input);

        /// <summary>
        /// Function to convert GRPC message to the respective entity. This function should be overridden in derived class
        /// </summary>
        /// <param name="responseMessage">GRPC message</param>
        /// <returns>Returns respective entity as a response for the current adapter method</returns>
        protected abstract TResult BuildResponse(IMessage responseMessage);

        /// <summary>
        /// Function to create unique requestId string using input entity for respective call.
        /// </summary>
        /// <param name="input">Input parameters required for the APIGateway method. Pass input entity</param>
        /// <returns>Returns unique requestId string </returns>
        public virtual string SetRequestId(TInput input)
        {
            return string.Format("{0}-{1}-{2}", _moduleName, _methodName, JsonConvert.SerializeObject(input));
        }

        /// <summary>
        /// 1. Method to add a call into APIGateway.
        /// 2. Method will build the request where Entity will be converted to GRPC Message.
        /// 3. RequestId will be used to get response object
        /// </summary>
        /// <param name="caller">Object of APIGateway caller. Call will be added to this object</param>
        /// <param name="input">Input parameters required for the APIGateway method. Pass input entity</param>
        public virtual void AddApiGatewayCall(IApiGatewayCaller caller, TInput input)
        {
                IMessage request = BuildRequest(input);
                if (!String.IsNullOrEmpty(_moduleName) && !String.IsNullOrEmpty(_methodName) && request != null)
                {
                    caller.Add(_moduleName, _methodName, request, _requestId);
                }
        }

        /// <summary>
        /// 1. Method to add a call into APIGateway.
        /// 2. Method will build the request where Entity will be converted to GRPC Message.
        /// 3. Callback function will be added to the APIGateway Helper.
        /// 4. RequestId will be used to get response object
        /// </summary>
        /// <param name="caller">Object of APIGateway caller. Call will be added to this object</param>
        /// <param name="input">Input parameters required for the APIGateway method. Pass input entity</param>        
        public virtual void AddApiGatewayCallWithCallback(IApiGatewayCaller caller, TInput input)
        {
                IMessage request = BuildRequest(input);
                if (!String.IsNullOrEmpty(_moduleName) && !String.IsNullOrEmpty(_methodName) && request != null)
                {
                    Action<IApiGatewayCaller> callBack = ParseAPIResponse;
                    caller.Add(_moduleName, _methodName, request, _requestId, callBack);
                }
        }


        /// <summary>
        /// Callback function which will be executed after receiving response from APIGateway.
        /// Function will get response from APIGateway and convert it to respective entity
        /// </summary>
        /// <param name="caller">Instance of the APIGateway caller</param>
        private void ParseAPIResponse(IApiGatewayCaller caller)
        {
            var responseMessage = caller.GetResponse<TApigatewayResponse>(_requestId);
            Output = BuildResponse(responseMessage);
        }

        /// <summary>
        /// Function to convert entity to GRPC Message which will be passed to the APIGateway
        /// Set Unique RequestId for respective call.
        /// </summary>
        /// <param name="caller">Instance of the APIGateway caller</param>
        /// <returns>Returns GRPC message</returns>
        private IMessage BuildRequest(TInput input)
        {
            _requestId = SetRequestId(input);
            return GetRequest(input);
        }
    }
}
