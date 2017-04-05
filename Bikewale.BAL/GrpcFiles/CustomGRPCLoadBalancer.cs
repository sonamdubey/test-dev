using Bikewale.Utility;
using Grpc.Core;
using log4net;
using System;
using System.Collections.Generic;

namespace GRPCLoadBalancer
{
    /// <summary>
    /// Author: Prasad Gawde
    /// Summary: This class is responsible for doing Round Robin load balancing between various GRPC Servers based on BWConfiguration.Instance.GrpcArticleServerList
    /// </summary>
    static class CustomGRPCLoadBalancerWithSingleton
    {
        private static Queue<Channel> m_WorkingQueue;
        private static object m_reachableQueueLockObject = new object();
        static ILog _logger = LogManager.GetLogger(typeof(CustomGRPCLoadBalancerWithSingleton));

        static string serverList = BWConfiguration.Instance.GrpcArticleServerList;

        static CustomGRPCLoadBalancerWithSingleton()
        {

            m_WorkingQueue = new Queue<Channel>();

            string[] allServers = serverList.Split(';');
            for (int i = 0, j = allServers.Length; i < j; i++)
            {
                //singleton objects for all servers            
                var ch = new Channel(allServers[i], ChannelCredentials.Insecure);
                CheckIfConnectionIsWorking(ch);
                m_WorkingQueue.Enqueue(ch);
            }

        }

        internal static Channel GetWorkingChannel()
        {
            Channel currentChosenChannel;

            lock (m_reachableQueueLockObject)
            {
                for (int i = 0; i < m_WorkingQueue.Count; i++)
                {
                    currentChosenChannel = m_WorkingQueue.Dequeue();
                    m_WorkingQueue.Enqueue(currentChosenChannel);
                    if (currentChosenChannel.State == ChannelState.Idle || currentChosenChannel.State == ChannelState.Ready)
                    {
                        return currentChosenChannel;
                    }
                    else
                        _logger.Error("Error102 " + currentChosenChannel.ResolvedTarget + " " + currentChosenChannel.State);
                }
            }
            _logger.Error("Error101 No Channel Available");
            return null;

        }

        static bool CheckIfConnectionIsWorking(Channel serverChannel)
        {

            if (serverChannel != null)
            {
                try
                {

                    //send the heartbit
                    var client = new EditCMSWindowsService.Messages.EditCMSGrpcService.EditCMSGrpcServiceClient(serverChannel);
                    var output = client.CheckHeartBit
                                (new EditCMSWindowsService.Messages.GrpcInt() { IntOutput = 2 },
                                 null, GetForwardTime(1000));
                    return output.IntOutput == 2;

                }
                catch
                {
                    return false;
                }
            }
            return false;
        }


        static DateTime GetForwardTime(int incrementMillisecond)
        {
            return DateTime.Now.AddMilliseconds(incrementMillisecond).ToUniversalTime();
        }


        public static void DisposeChannels()
        {
            Channel currentChosenChannel;
            lock (m_reachableQueueLockObject)
            {
                int count = m_WorkingQueue.Count;
                for (int i = 0; i < m_WorkingQueue.Count; i++)
                {
                    currentChosenChannel = m_WorkingQueue.Dequeue();
                    currentChosenChannel.ShutdownAsync();

                    _logger.Error("Error103 disposed " + currentChosenChannel.ResolvedTarget);
                }

            }
        }

    }

}