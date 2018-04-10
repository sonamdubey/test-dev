using System;
using System.Collections.Generic;
using GatewayWebservice;
using Google.Protobuf;

namespace Bikewale.BAL.ApiGateway.ApiGatewayHelper
{
	/// <summary>
	/// Created By : Ashish G. Kamble on 2 Apr 2018
	/// Summary : Interface have methods to interact with AEPL ApiGateway.
	/// </summary>
	public interface IApiGatewayCaller
	{
		/// <summary>
		/// Function Adds a call to the APIGateway Agregator
		/// </summary>
		/// <param name="module">Module name for microservice</param>
		/// <param name="methondName">Used to execute given method in a module</param>
		/// <param name="message">Input parameters to the method</param>
		/// <returns>Returns true when operation is successfull</returns>
		bool Add(string module, string methodName, IMessage message);

		/// <summary>
		/// Function Adds a call to the APIGateway Agregator.
		/// </summary>
		/// <param name="module">Module name for microservice</param>
		/// <param name="methondName">Used to execute given method in a module</param>
		/// <param name="message">Input parameters to the method</param>
		/// <param name="callback">Callback function to convert grpc message to bikewale entities. This function will be executed after APIGateway returns result.</param>
		/// <returns>Returns index of callback function</returns>
		int Add(string module, string methodName, IMessage message, Action<IApiGatewayCaller> callback);

		/// <summary>
		/// Function Adds a call to the APIGateway Agregator.
		/// </summary>
		/// <param name="module">Module name for microservice</param>
		/// <param name="methondName">Used to execute given method in a module</param>
		/// <param name="message">Input parameters to the method</param>
		/// <param name="identifier">Identifier for group of methods added in APIGateway caller. Identifier can be used to get data.</param>
		/// <returns>Returns true when operation is successfull</returns>
		bool Add(string module, string methodName, IMessage message, string identifier);

		/// <summary>
		/// Function to Call APIGateway. It also saves response from APIGateway
		/// </summary>
		void Call();

		/// <summary>
		/// Function to get response from an aggregated call at given index. This function must be called after Call() method in this class
		/// </summary>
		/// <typeparam name="T">Specify GRPC message type.  Data will be converted into this type. T should inherit from IMesage interface.</typeparam>
		/// <param name="index">index at which data is present in response.</param>
		/// <returns>Response from APIGatway will be converted into this(T) type.</returns>
		T GetResponse<T>(int index) where T : IMessage;

		/// <summary>
		/// Function to get reponse for the given identifier
		/// </summary>
		/// <param name="outputRequest">List of reponse objects</param>
		/// <param name="Identifier">Identifier for group of methods added in APIGateway caller. Identifier used to get data from APIGatway response.</param>
		/// <returns>Complete response object from APIGateway</returns>
		IEnumerable<ApiGatewayResponse> GetResponse(OutputRequest outputRequest, string Identifier);

		/// <summary>
		/// Property to get complete response object from APIGateway
		/// </summary>
		IEnumerable<ApiGatewayResponse> GetAllResponse { get; }

	}	// class
}	// namespace
