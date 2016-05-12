using Grpc.Core;
using GRPCLoadBalancer;
using System;
using System.Configuration;
using System.Diagnostics;

namespace Grpc.CMS
{
    public static class GrpcMethods
    {
        static readonly int m_ChanelWaitTime;

        static Channel _channel;

        static GrpcMethods()
        {
            m_ChanelWaitTime = Convert.ToInt32(ConfigurationManager.AppSettings["GrpcChannelWaitTime"]);//5000
        }

        static DateTime GetForwardTime(int incrementMillisecond)
        {
            return DateTime.Now.AddMilliseconds(incrementMillisecond).ToUniversalTime();
        }

        public static GrpcCMSContent GetArticleListByCategory(string catIdList, uint startIdx, uint endIdx)
        {

            GrpcCMSContent _objArticleList = null;
            Channel ch = CustomGRPCLoadBalancerWithSingleton.GetWorkingChannel();

            while (true)
            {

                if (ch != null)
                {
                    Debug.WriteLine("Got channel for " + ch.ResolvedTarget);
                    var client = GrpcArticle.NewClient(ch);
                    try
                    {
                        _objArticleList = client.ListByCategory(new GrpcArticleByCatURI()
                        {
                            ApplicationId = 2,
                            CategoryIdList = catIdList,
                            EndIndex = endIdx,
                            MakeId = 0,
                            ModelId = 0,
                            StartIndex = startIdx
                        },
                        null, GetForwardTime(m_ChanelWaitTime));
                        break;
                    }
                    catch (Exception e)
                    {
                        RpcException rpcEx = e as RpcException;

                        if (rpcEx != null &&
                            (rpcEx.Status.StatusCode == StatusCode.DeadlineExceeded || rpcEx.Status.StatusCode == StatusCode.Unavailable))
                        {
                            CustomGRPCLoadBalancerWithSingleton.SetServerAsNotReachable(ch);
                        }
                        ch = CustomGRPCLoadBalancerWithSingleton.GetWorkingChannel();
                    }
                }
                else
                    break;
            }


            return _objArticleList;

        }

        public static GrpcArticleSummaryList MostRecentList(string contenTypes, int totalRecords, int? makeId = 0, int? modelId = 0)
        {

            GrpcArticleSummaryList _objArticleSummaryList = null;
            Channel ch = CustomGRPCLoadBalancerWithSingleton.GetWorkingChannel();

            while (true)
            {
                if (ch != null)
                {
                    Debug.WriteLine("Got channel for " + ch.ResolvedTarget);
                    var client = GrpcArticle.NewClient(ch);
                    try
                    {
                        _objArticleSummaryList = client.MostRecentList
                            (new GrpcArticleRecentURI()
                            {
                                MakeId = makeId == null ? 0 : makeId.Value,
                                ModelId = modelId == null ? 0 : modelId.Value,
                                ArticleFeatureURI = new GrpcArticleFeatureURI()
                                {
                                    ApplicationId = 2,
                                    ContentTypes = contenTypes,
                                    TotalRecords = (uint)totalRecords
                                }
                            },
                             null, GetForwardTime(m_ChanelWaitTime));

                        break;
                    }
                    catch (Exception e)
                    {
                        RpcException rpcEx = e as RpcException;


                        if (rpcEx != null &&
                            (rpcEx.Status.StatusCode == StatusCode.DeadlineExceeded || rpcEx.Status.StatusCode == StatusCode.Unavailable))
                        {
                            CustomGRPCLoadBalancerWithSingleton.SetServerAsNotReachable(ch);
                        }
                        ch = CustomGRPCLoadBalancerWithSingleton.GetWorkingChannel();
                    }
                }
                else
                    break;
            }


            return _objArticleSummaryList;

        }

    }
}