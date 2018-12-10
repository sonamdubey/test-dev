using Carwale.Entity.Enum;
using Carwale.Utility;
using Grpc.Core;
using log4net;
using System;

namespace Carwale.DAL.Grpc
{
    /// <summary>
    /// Author: Ashish Goyal
    /// Summary: This class is responsible for doing Round Robin load balancing between various GRPC Servers
    /// </summary>
    class LoadBalancer
    {
        static bool _logGrpcCallTimings = CWConfiguration.LogGrpcCallTimings;
        static readonly ILog _logger = LogManager.GetLogger(typeof(LoadBalancer));
        static readonly int _poolSize = CWConfiguration.GrpcPoolSize > 0 ? CWConfiguration.GrpcPoolSize : 1;
        private readonly Channel[] _WorkingQueue;
        private readonly int _WorkingQueueSize;
        int _currentIndex = 0;

        public LoadBalancer(string serverlist)
        {
            string[] allServers = serverlist.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
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
                        _WorkingQueue[idx] = new Channel(allServers[i], ChannelCredentials.Insecure);
                        idx++;
                    }
                }

            }
        }

        internal Channel GetWorkingChannel(Action<Channel> heartBeat)
        {
            Channel currentChosenChannel = null;
            int curId = _currentIndex % _WorkingQueueSize;
            int idx = 0;
            while (idx < _WorkingQueueSize)
            {
                currentChosenChannel = _WorkingQueue[curId];

                if (currentChosenChannel.State == ChannelState.Ready)
                {
                    _currentIndex = ++curId;
                    return currentChosenChannel;
                }
                else if (currentChosenChannel.State == ChannelState.Idle)
                {
                    heartBeat(currentChosenChannel);
                }
                else if (_logGrpcCallTimings)
                {
                    _logger.Error(GrpcTimingLogEnum.T102.ToString() + " " + currentChosenChannel.ResolvedTarget + " " + currentChosenChannel.State);
                }

                curId++;
                curId = curId % _WorkingQueueSize;
                idx++;
            }

            if (_logGrpcCallTimings)
            {
                _logger.Error(GrpcTimingLogEnum.T101.ToString() + " No Channel Available");
            }
            return _WorkingQueue[_currentIndex % _WorkingQueueSize]; ;
        }
    }
}