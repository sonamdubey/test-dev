using Carwale.DAL.Grpc;
using Carwale.Utility;
using Grpc.Core;
using static Adtargeting.UserData;

namespace Carwale.DAL.AdTargeting
{
    /// <summary>
    /// Author: Piyush Sahu
    /// Summary: This class is responsible for fetching a GRPC channel for Ad Targeting service
    /// </summary>
    static class AdTargetingChannel
    {        
        private readonly static LoadBalancer _loadBalancer = new LoadBalancer(CWConfiguration._AdTargetingServerList);

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
            UserDataClient client = new UserDataClient(channel);
            client.CheckHeartBitAsync(new Adtargeting.GrpcInt
            {
                IntOutput = 1
            });
            // This operation is supposed to be executed asynchronously
            // Donot await or use the result of above call
            // using the result would make the function synchronous 
        }
    }
}
