using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Configuration;

namespace GRPCLoadBalancer
{  

    static class CustomGRPCLoadBalancerWithSingleton
    {
        private static Queue<Channel> m_WorkingQueue;
        private static Queue<Channel> m_UnreachableQueue;
        private static readonly int m_waitLimit;
        private static readonly int m_ServerHealthCheckTimeLimitSeconds;
        private static DateTime m_lastTimeStamp = DateTime.Now;
        static object _workingQueueLock = new object();
        static object _nonWorkingQueueLock = new object();

        static CustomGRPCLoadBalancerWithSingleton()
        {
            m_WorkingQueue = new Queue<Channel>();
            m_UnreachableQueue = new Queue<Channel>();
            string serverList = ConfigurationManager.AppSettings["GrpcArticleServerList"];
            m_waitLimit = Convert.ToInt32(ConfigurationManager.AppSettings["GrpcArticleServerWaitLimitMilliSecond"]); //100
            m_ServerHealthCheckTimeLimitSeconds = Convert.ToInt32(ConfigurationManager.AppSettings["GrpcArticleServerHealthCheckTimeLimitSeconds"]); //10
            //string serverList = "127.0.0.1:50052;";//127.0.0.1:50051";

            string[] allServers = serverList.Split(';');
            for (int i = 0, j = allServers.Length; i < j; i++)
            {
                m_WorkingQueue.Enqueue(new Channel(allServers[i], ChannelCredentials.Insecure));
            }

        }

        
        internal static Channel GetWorkingChannel()
        {
            if (m_UnreachableQueue.Count > 0 && (DateTime.Now - m_lastTimeStamp).Seconds > m_ServerHealthCheckTimeLimitSeconds)
            {
                m_lastTimeStamp = DateTime.Now;
                CheckServerHealth();
            }

            Channel retChannel;
            lock (_workingQueueLock)
            {
                for (int i = 0; i < m_WorkingQueue.Count; i++)
                {
                    retChannel = m_WorkingQueue.Dequeue();
                    m_WorkingQueue.Enqueue(retChannel);
                    if (!m_UnreachableQueue.Contains(retChannel))
                    {
                        return retChannel;
                    }
                } 
            }
            return null;
        }

        static void CheckServerHealth()
        {
            Channel serverChannel;

            lock (_nonWorkingQueueLock)
            {
                for (int i = 0, j = m_UnreachableQueue.Count; i < j; i++)
                {
                    serverChannel = m_UnreachableQueue.Dequeue();
                    if (!CheckIfConnectionIsWorking(serverChannel))
                    {
                        m_UnreachableQueue.Enqueue(serverChannel);
                    }
                } 
            }
        }


        static bool CheckIfConnectionIsWorking(Channel serverChannel)
        {

            if (serverChannel != null)
            {
                try
                {
                    serverChannel.ConnectAsync(GetForwardTime(m_waitLimit)).Wait();
                    return !(serverChannel.State == ChannelState.FatalFailure || serverChannel.State == ChannelState.TransientFailure);
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


        public static void SetServerAsNotReachable(Channel serverChannel)
        {
            if (serverChannel != null)
            {
                if (!m_UnreachableQueue.Contains(serverChannel))
                    m_UnreachableQueue.Enqueue(serverChannel);
            }
        }

    }
}