using Bikewale.Utility;
using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Configuration;

namespace GRPCLoadBalancer
{
    /// <summary>
    /// Author: Prasad Gawde
    /// Summary: This class is responsible for doing Round Robin load balancing between various GRPC Servers based on BWConfiguration.Instance.GrpcArticleServerList
    /// </summary>
    static class CustomGRPCLoadBalancerWithSingleton
    {
        private static Queue<Channel> m_WorkingQueue;
        private static Channel m_currentChosenChannel = null;
        private static object m_reachableQueueLockObject = new object();

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

            lock (m_reachableQueueLockObject)
            {
                for (int i = 0; i < m_WorkingQueue.Count; i++)
                {
                    m_currentChosenChannel = m_WorkingQueue.Dequeue();
                    m_WorkingQueue.Enqueue(m_currentChosenChannel);
                    if (m_currentChosenChannel.State == ChannelState.Idle || m_currentChosenChannel.State == ChannelState.Ready)
                    {
                        return m_currentChosenChannel;
                    }
                }
            }
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
                                 null, GetForwardTime(100));
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
            lock (m_reachableQueueLockObject)
            {
                int count = m_WorkingQueue.Count;
                for (int i = 0; i < m_WorkingQueue.Count; i++)
                {
                    m_currentChosenChannel = m_WorkingQueue.Dequeue();
                    m_currentChosenChannel.ShutdownAsync();
                }
            }
        }

    }
    
}