using ApiGatewayLibrary;
using Google.Protobuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.DAL.ApiGateway
{
    public interface IApiGatewayCaller
    {
        /// <summary>
        /// Call Api Gateway and saves response asynchronously
        /// </summary>
        Task CallAsync();

        bool Add(string module, string methodName, IMessage message);

        bool Add(string module, string methodName, IMessage message, string requestId, Action<IApiGatewayCaller> callback);

        bool Add(string module, string methodName, IMessage message, string requestId);

        /// <summary>
        ///Add callback in Api Gateway Caller
        /// </summary>
        void AddCallback(Action<IApiGatewayCaller> callback);

        /// <summary>
        /// Call Api Gateway and saves response
        /// </summary>
        void Call();

        /// <summary>
        /// Get Response for a aggregated call at a specific index. Should be called after Call()
        /// </summary>
        /// <typeparam name="T">IMessage Type</typeparam>
        /// <param name="index">index of the result</param>
        /// <returns>The IMessage type. GatewayException if fails</returns>
        T GetResponse<T>(int index) where T : IMessage;

        /// <summary>
        /// Get Response for a aggregated call for specific requestId. Should be called after Call()
        /// </summary>
        /// <typeparam name="T">IMessage Type</typeparam>
        /// <param name="requestId">identifier of the result</param>
        /// <returns>The IMessage type. GatewayException if fails</returns>
        T GetResponse<T>(string requestId) where T : IMessage;
    }
}