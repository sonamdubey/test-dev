using System;
using Google.Protobuf;

namespace Bikewale.BAL.ApiGateway.ApiGatewayHelper
{
	/// <summary>
	/// Created By : Ashish G. Kamble on 4 Apr 2018	
	/// Summary : Class defines template to call the APIGateway. Adapters which needs to implement GRPC method should inherit from this class.
	/// </summary>
	/// <typeparam name="TInput">Input entity</typeparam>
	/// <typeparam name="TResult">Output entity</typeparam>
	/// <typeparam name="TApigatewayResponse">GRPC response message</typeparam>
	public abstract class AbstractApiGatewayAdapter<TInput, TResult, TApigatewayResponse> : IApiGatewayAdapter<TInput, TResult, TApigatewayResponse> where TApigatewayResponse : IMessage
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
		/// Response object for the current method
		/// </summary>
		public TResult Output { get; private set; }

		/// <summary>
		/// Index at which response object for current method is available in APIGateway response.
		/// </summary>
		private ushort ResponseIndex { get; set; }

		/// <summary>
		/// Constructor to initialize the properties required call the GRPC method
		/// </summary>
		/// <param name="moduleName"></param>
		/// <param name="methodName"></param>
		protected AbstractApiGatewayAdapter(string moduleName, string methodName)
		{
			_moduleName = moduleName;
			_methodName = methodName;
		}

		/// <summary>
		/// 1. Method to add a call into APIGateway.
		/// 2. Method will build the request where Bikewale Entity will be converted to GRPC Message.
		/// 3. Callback function will be added to the APIGateway Helper.
		/// 4. Index of the response object will be saved, when call is added.
		/// </summary>
		/// <param name="caller">Object of APIGateway caller. Call will be added to this object</param>
		/// <param name="input">Input parameters required for the APIGateway method. Pass bikewale input entity</param>
		public virtual void AddApiGatewayCall(IApiGatewayCaller caller, TInput input)
		{
			try
			{
				if (input != null)
				{
					IMessage request = BuildRequest(input);
					Action<IApiGatewayCaller> callBack = ParseAPIResponse;

                    if (!String.IsNullOrEmpty(_moduleName) && !String.IsNullOrEmpty(_methodName) && request != null && callBack != null)
                        ResponseIndex = caller.Add(_moduleName, _methodName, request, callBack);

                }
            }
			catch (Exception ex)
			{
				Notifications.ErrorClass.LogError(ex, "Bikewale.BAL.ApiGateway.ApiGatewayHelper.AbstractApiGatewayAdapater.AddApiGatewayCall");
			}
		}

		/// <summary>
		/// Function to convert bikewale entity to GRPC Message which will be passed to the APIGateway. This function should be overridden in derived class
		/// </summary>
		/// <param name="input">bikewale entity</param>
		/// <returns>Returns GRPC message</returns>
		protected abstract IMessage BuildRequest(TInput input);

		/// <summary>
		/// Function to convert GRPC message to the respective bikewale entity. This function should be overridden in derived class
		/// </summary>
		/// <param name="responseMessage">GRPC message</param>
		/// <returns>Returns bikewale entity as a response for the current adapter method</returns>
		protected abstract TResult BuildResponse(IMessage responseMessage);

		/// <summary>
		/// Callback function which will be executed after receiving response from APIGateway.
		/// Function will get response from APIGateway and convert it to bikewale entity
		/// </summary>
		/// <param name="caller">Instance of the APIGateway caller</param>
		private void ParseAPIResponse(IApiGatewayCaller caller)
		{
			try
			{
				var responseMessage = caller.GetResponse<TApigatewayResponse>(ResponseIndex);
				Output = BuildResponse(responseMessage);
			}
			catch (Exception ex)
			{
				Notifications.ErrorClass.LogError(ex, "Bikewale.BAL.ApiGateway.ApiGatewayHelper.AbstractApiGatewayAdapater.ParseAPIResponse");
			}
		}

	}	// class
}	// namespace
