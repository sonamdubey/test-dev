using System;
using System.Collections.Generic;
using System.Configuration;
using Grpc.Core;

namespace GRPCLoadBalancer
{
    static class CustomGRPCLoadBalancer
    {
        private static Queue<string> m_WorkingQueue;
        private static Queue<string> m_UnreachableQueue;
        private static readonly int m_waitLimit;
        private static readonly int m_ServerHealthCheckTimeLimitSeconds;
        private static DateTime m_lastTimeStamp = DateTime.Now;


        static CustomGRPCLoadBalancer()
        {
            m_WorkingQueue = new Queue<string>();
            m_UnreachableQueue = new Queue<string>();
            string serverList = ConfigurationManager.AppSettings["GrpcArticleServerList"];
            m_waitLimit = Convert.ToInt32(ConfigurationManager.AppSettings["GrpcArticleServerWaitLimitMilliSecond"]); //100
            m_ServerHealthCheckTimeLimitSeconds = Convert.ToInt32(ConfigurationManager.AppSettings["GrpcArticleServerHealthCheckTimeLimitSeconds"]); //10
            //string serverList = "127.0.0.1:50052;";//127.0.0.1:50051";

            string[] allServers = serverList.Split(';');
            for (int i = 0, j = allServers.Length; i < j; i++)
            {
                m_WorkingQueue.Enqueue(allServers[i]);
            }

        }

        internal static string GetWorkingChannelString()
        {
            if (m_UnreachableQueue.Count > 0 && (DateTime.Now - m_lastTimeStamp).Seconds > m_ServerHealthCheckTimeLimitSeconds)
            {
                m_lastTimeStamp = DateTime.Now;
                CheckServerHealth();
            }

            string retStr;
            for (int i = 0; i < m_WorkingQueue.Count; i++)
            {
                retStr = m_WorkingQueue.Dequeue();
                m_WorkingQueue.Enqueue(retStr);
                if (!m_UnreachableQueue.Contains(retStr))
                {
                    return retStr;
                }
            }
            return null;
        }

        static void CheckServerHealth()
        {
            string serverStr;

            for (int i = 0, j = m_UnreachableQueue.Count; i < j; i++)
            {
                serverStr = m_UnreachableQueue.Dequeue();
                if (!CheckIfConnectionIsWorking(serverStr))
                {
                    m_UnreachableQueue.Enqueue(serverStr);
                }
            }
        }


        static bool CheckIfConnectionIsWorking(string serverStr)
        {
            Channel channel = null;

            if (!string.IsNullOrEmpty(serverStr))
            {
                channel = new Channel(serverStr, ChannelCredentials.Insecure);
                try
                {
                    channel.ConnectAsync(GetForwardTime(m_waitLimit)).Wait();
                    return !(channel.State == ChannelState.FatalFailure || channel.State == ChannelState.TransientFailure);
                }
                catch
                {
                    return false;
                }
                finally
                {
                    if (channel != null)
                    {
                        channel.ShutdownAsync().Wait();
                        channel = null;
                    }
                }
            }
            return false;
        }


        static DateTime GetForwardTime(int incrementMillisecond)
        {
            return DateTime.Now.AddMilliseconds(incrementMillisecond).ToUniversalTime();
        }


        public static void SetServerAsNotReachable(string serverString)
        {
            if (!string.IsNullOrEmpty(serverString))
            {
                if (!m_UnreachableQueue.Contains(serverString))
                    m_UnreachableQueue.Enqueue(serverString);
            }
        }


    }
}