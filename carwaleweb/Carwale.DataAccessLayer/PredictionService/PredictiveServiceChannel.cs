using Grpc.Core;
using System.Configuration;
using Predictive;
using Carwale.DAL.Grpc;

namespace Carwale.DAL.PredictionService
{
    /// <summary>
    /// Author: Anchal Gupta
    /// Summary: This class is responsible for fetching a GRPC channel for Score Prediction Service
    /// </summary>
    static class PredictiveServiceChannel
    {
        private readonly static string _serverList = ConfigurationManager.AppSettings["PredictiveGrpcServer"] ?? string.Empty;
        private readonly static LoadBalancer _loadBalancer = new LoadBalancer(_serverList);

        /// <summary>
        /// Returns a READY channel
        /// </summary>
        /// <returns></returns>
        public static Channel Channel
        {
            get
            {
                return _loadBalancer.GetWorkingChannel(HeartBeat);
            }
        }

        /// <summary>
        /// Author: Ashish Goyal
        /// Description: Executes a heartBeat RPC asynchronously
        /// Purpose: Updates the channel status from idle to ready or transient-failure
        /// </summary>
        /// <param name="channel"></param>
        public static void HeartBeat(Channel channel)
        {
            PredictiveScore.PredictiveScoreClient client = new PredictiveScore.PredictiveScoreClient(channel);
            client.CheckHeartbeatAsync(new HeartbeatFlag());
            // This operation is supposed to be executed asynchronously
            // Donot await or use the result of above call
            // using the result would make the function synchronous 
        }
    }
}