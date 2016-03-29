using System;
using System.Configuration;
using System.Diagnostics;
using Grpc.Core;
using GRPCLoadBalancer;

namespace Grpc.CMS
{
    public static class GrpcMethods
    {
        static readonly int m_ChanelWaitTime;
        static GrpcMethods()
        {
            m_ChanelWaitTime = Convert.ToInt32(ConfigurationManager.AppSettings["GrpcChannelWaitTime"]);//5000
        }

        static DateTime GetForwardTime(int incrementMillisecond)
        {
            return DateTime.Now.AddMilliseconds(incrementMillisecond).ToUniversalTime();
        }

        internal static GrpcCMSContent GetArticleListByCategory(string catIdList, uint startIdx, uint endIdx)
        {
            Channel channel = null;
            GrpcCMSContent _objArticleList = null;
            string serverStr = CustomGRPCLoadBalancer.GetWorkingChannelString();

            while (true)
            {

                if (string.IsNullOrEmpty(serverStr))
                {
                    break;
                }
                channel = new Channel(serverStr, ChannelCredentials.Insecure);
                Debug.WriteLine("Got channel for " + channel.ResolvedTarget);
                if (channel != null)
                {
                    var client = GrpcArticle.NewClient(channel);
                    try
                    {
                        _objArticleList = client.ListByCategory(new GrpcArticleByCatURI() { ApplicationId = 2, CategoryIdList = catIdList, EndIndex = endIdx, MakeId = 0, ModelId = 0, StartIndex = startIdx }, null, GetForwardTime(m_ChanelWaitTime));
                        channel.ShutdownAsync().Wait();
                        channel = null;
                        break;
                    }
                    catch (Exception e)
                    {
                        RpcException rpcEx = e as RpcException;

                        if (channel != null)
                        {
                            channel.ShutdownAsync().Wait();
                            channel = null;
                        }

                        if (rpcEx != null &&
                            (rpcEx.Status.StatusCode == StatusCode.DeadlineExceeded || rpcEx.Status.StatusCode == StatusCode.Unavailable))
                        {
                            CustomGRPCLoadBalancer.SetServerAsNotReachable(serverStr);
                        }
                        serverStr = CustomGRPCLoadBalancer.GetWorkingChannelString();
                    }
                }
                else
                    break;
            }


            return _objArticleList;

        }

    }
}