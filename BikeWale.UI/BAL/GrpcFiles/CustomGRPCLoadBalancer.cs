using Bikewale.Utility;
using Grpc.Core;
using log4net;
using System;

namespace GRPCLoadBalancer
{
    /// <summary>
    /// Author: Prasad Gawde
    /// Summary: This class is responsible for doing Round Robin load balancing between various GRPC Servers based on BWConfiguration.Instance.GrpcArticleServerList
    /// </summary>
    static class CustomGRPCLoadBalancerWithSingleton
    {
        static Channel[] _WorkingQueue;

        static ILog _logger = LogManager.GetLogger(typeof(CustomGRPCLoadBalancerWithSingleton));
        static bool _logGrpcErrors = Convert.ToBoolean(BWConfiguration.Instance.LogGrpcErrors);
        static string _serverList = BWConfiguration.Instance.GrpcArticleServerList;
        static int _poolSize = (int)BWConfiguration.Instance.GrpcPoolSize;
        static int _WorkingQueueSize;
        static int _currentIndex = 0;

        static CustomGRPCLoadBalancerWithSingleton()
        {
            if (_poolSize <= 0)
                _poolSize = 1;

            string[] allServers = _serverList.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            if (allServers.Length > 0)
            {
                _WorkingQueueSize = allServers.Length * _poolSize;
                _WorkingQueue = new Channel[_WorkingQueueSize];
                int idx = 0;
                for (int j = 0; j < _poolSize; j++)
                {
                    for (int i = 0; i < allServers.Length; i++)
                    {
                        //singleton objects for all servers            
                        var ch = new Channel(allServers[i], ChannelCredentials.Insecure);
                        _WorkingQueue[idx] = ch;
                        idx++;
                    }
                }

            }

        }

        internal static Channel GetWorkingChannel()
        {
            Channel currentChosenChannel = null;
            int curId = _currentIndex % _WorkingQueueSize;
            int idx = 0;
            while (idx < _WorkingQueueSize)
            {
                currentChosenChannel = _WorkingQueue[curId];
                if (currentChosenChannel.State == ChannelState.Idle || currentChosenChannel.State == ChannelState.Ready)
                {
                    _currentIndex = ++curId;
                    return currentChosenChannel;
                }
                else if (_logGrpcErrors)
                {
                    _logger.Error("Error102 " + currentChosenChannel.ResolvedTarget + " " + currentChosenChannel.State);
                }
                curId++;
                curId = curId % _WorkingQueueSize;
                idx++;
            }
            if (_logGrpcErrors)
            {
                _logger.Error("Error101 No Channel Available");
            }
            return null;

        }
        public static void DisposeChannels()
        {
            for (int i = 0; i < _WorkingQueueSize; i++)
            {
                _WorkingQueue[i].ShutdownAsync();
            }
        }
    }

}

