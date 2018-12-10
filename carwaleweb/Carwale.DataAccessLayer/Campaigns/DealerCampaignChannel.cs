using Carwale.DAL.Grpc;
using Grpc.Core;
using ProtoBufClass.Common;
using System.Configuration;
using static ProtoBufClass.Campaigns.Campaigns;

namespace Carwale.DAL.Campaigns
{
    /// <summary>
    /// Author: Ashish Goyal
    /// Summary: This class is responsible for fetching a GRPC channel for Dealer Campaigns Service
    /// </summary>
    static class DealerCampaignChannel
    {
        private readonly static string _serverList = ConfigurationManager.AppSettings["DealerCampaignGrpcServer"] ?? string.Empty;
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
            CampaignsClient client = new CampaignsClient(channel);
            client.CheckAsync(new NoInput());
            // This operation is supposed to be executed asynchronously
            // Donot await or use the result of above call
            // using the result would make the function synchronous 
        }
    }
}