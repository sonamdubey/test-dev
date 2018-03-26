using System.Collections.Generic;
using Google.Protobuf;

namespace Bikewale.Utility.ApiGatewayHelper
{
	public interface IApiGatewayCaller
	{
		/// <summary>
		/// Adds a call to the encapsulated Agregator
		/// </summary>
		/// <param name="campaignModule"></param>
		/// <param name="methondName"></param>
		/// <param name="message"></param>
		/// <returns></returns>
		bool Add(string campaignModule, string methodName, IMessage message);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="module"></param>
		/// <param name="methodName"></param>
		/// <param name="message"></param>
		/// <param name="identifier"></param>
		/// <returns></returns>
		bool Add(string module, string methodName, IMessage message, string identifier);

		/// <summary>
		/// Call Api Gateway and saves response
		/// </summary>
		void Call();
		
		/// <summary>
		/// Get Response for a aggregated call at a specific index. Should be called after Call()
		/// </summary>
		/// <typeparam name="T">IMessage Type</typeparam>
		/// <param name="index">index of the result</param>
		/// <returns></returns>
		T GetResponse<T>(int index) where T : IMessage;

		/// <summary>
		/// 
		/// </summary>
		IEnumerable<ApiGatewayResponse> GetAllResponse { get; }
	}
}
