using EditCMSWindowsService.Messages;
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


        static GrpcMethods()
        {
            m_ChanelWaitTime = Convert.ToInt32(ConfigurationManager.AppSettings["GrpcChannelWaitTime"]);//5000
        }

        static DateTime GetForwardTime(int incrementMillisecond)
        {
            return DateTime.Now.AddMilliseconds(incrementMillisecond).ToUniversalTime();
        }

        public static GrpcCMSContent GetArticleListByCategory(string catIdList, uint startIdx, uint endIdx,int makeid=0,int modelid=0)
        {

            Channel ch = CustomGRPCLoadBalancerWithSingleton.GetWorkingChannel();

            while (true)
            {
                if (ch != null)
                {
                    Debug.WriteLine("Got channel for " + ch.ResolvedTarget);
                    var client = new EditCMSGrpcService.EditCMSGrpcServiceClient(ch);
                    try
                    {
                          return  client.GetContentListByCategory(new GrpcArticleByCatURI()
                        {
                            ApplicationId = 2,
                            CategoryIdList = catIdList,
                            EndIndex = endIdx,
                            MakeId = makeid,
                            ModelId = modelid,
                            StartIndex = startIdx
                        },
                        null, GetForwardTime(m_ChanelWaitTime));
                    }
                    catch (Exception e)
                    {
                        RpcException rpcEx = e as RpcException;                        
                        ch = CustomGRPCLoadBalancerWithSingleton.GetWorkingChannel();
                    }
                }
                else
                    break;
            }


            return null;

        }

        public static GrpcArticleSummaryList MostRecentList(string contenTypes, int totalRecords, int? makeId = 0, int? modelId = 0)
        {
            Channel ch = CustomGRPCLoadBalancerWithSingleton.GetWorkingChannel();

            while (true)
            {
                if (ch != null)
                {
                    Debug.WriteLine("Got channel for " + ch.ResolvedTarget);
                    var client = new EditCMSGrpcService.EditCMSGrpcServiceClient(ch);
                    try
                    {                        

                        return  client.GetMostRecentArticles
                            (new GrpcArticleRecentURI()
                            {
                                MakeId = makeId == null ? 0 : makeId.Value,
                                ModelId = modelId == null ? 0 : modelId.Value,

                                ApplicationId = 2,
                                ContentTypes = contenTypes,
                                TotalRecords = (uint)totalRecords
                            },
                             null, GetForwardTime(m_ChanelWaitTime));
                    }
                    catch (Exception e)
                    {
                        RpcException rpcEx = e as RpcException;
                        ch = CustomGRPCLoadBalancerWithSingleton.GetWorkingChannel();
                    }
                }
                else
                    break;
            }
            return null;

        }

        public static GrpcModelImageList GetArticlePhotos(ulong basicId)
        {
            Channel ch = CustomGRPCLoadBalancerWithSingleton.GetWorkingChannel();

            while (true)
            {
                if (ch != null)
                {
                    var client = new EditCMSGrpcService.EditCMSGrpcServiceClient(ch);
                    try
                    {

                        return client.GetArticlePhotos
                            (new GrpcArticleContentURI()
                            {
                               BasicId=basicId
                            },
                             null, GetForwardTime(m_ChanelWaitTime));
                    }
                    catch (Exception e)
                    {
                        RpcException rpcEx = e as RpcException;
                        ch = CustomGRPCLoadBalancerWithSingleton.GetWorkingChannel();
                    }
                }
                else
                    break;
            }
            return null;
        }

        public static GrpcModelImageList GetModelPhotosList(uint applicationId,int modelId, string categoryId)
        {
            Channel ch = CustomGRPCLoadBalancerWithSingleton.GetWorkingChannel();

            while (true)
            {
                if (ch != null)
                {
                    var client = new EditCMSGrpcService.EditCMSGrpcServiceClient(ch);
                    try
                    {

                        return client.GetModelPhotosList
                            (new GrpcModelPhotoURI()
                            {
                                ApplicationId=applicationId,
                                ModelId=modelId,
                                CategoryIdList=categoryId
                            },
                             null, GetForwardTime(m_ChanelWaitTime));
                    }
                    catch (Exception e)
                    {
                        RpcException rpcEx = e as RpcException;
                        ch = CustomGRPCLoadBalancerWithSingleton.GetWorkingChannel();
                    }
                }
                else
                    break;
            }
            return null;
        }

        public static GrpcInt GrpcGetFeaturedCar(string versions,int categoryId,int platformId)
        {
            Channel ch = CustomGRPCLoadBalancerWithSingleton.GetWorkingChannel();

            while (true)
            {
                if (ch != null)
                {
                    var client = new EditCMSGrpcService.EditCMSGrpcServiceClient(ch);
                    try
                    {

                        return client.GrpcGetFeaturedCar
                            (new GrpcFeatureCarURI()
                            {
                                CategoryId= categoryId,
                                PlatformId=platformId,
                                VersionIds=versions
                            },
                             null, GetForwardTime(m_ChanelWaitTime));
                    }
                    catch (Exception e)
                    {
                        RpcException rpcEx = e as RpcException;
                        ch = CustomGRPCLoadBalancerWithSingleton.GetWorkingChannel();
                    }
                }
                else
                    break;
            }
            return null;
        }

        public static GrpcArticleDetails GetContentDetails(ulong basicId)
        {
            Channel ch = CustomGRPCLoadBalancerWithSingleton.GetWorkingChannel();

            while (true)
            {
                if (ch != null)
                {
                    var client = new EditCMSGrpcService.EditCMSGrpcServiceClient(ch);
                    try
                    {

                        return client.GetContentDetails
                            (new GrpcArticleContentURI()
                            {
                                BasicId=basicId
                            },
                             null, GetForwardTime(m_ChanelWaitTime));
                    }
                    catch (Exception e)
                    {
                        RpcException rpcEx = e as RpcException;
                        ch = CustomGRPCLoadBalancerWithSingleton.GetWorkingChannel();
                    }
                }
                else
                    break;
            }
            return null;
        }

        public static GrpcArticlePageDetails GetContentPages(ulong basicId)
        {
            Channel ch = CustomGRPCLoadBalancerWithSingleton.GetWorkingChannel();

            while (true)
            {
                if (ch != null)
                {
                    var client = new EditCMSGrpcService.EditCMSGrpcServiceClient(ch);
                    try
                    {

                        return client.GetContentPages
                            (new GrpcArticleContentURI()
                            {
                                BasicId = basicId
                            },
                             null, GetForwardTime(m_ChanelWaitTime));
                    }
                    catch (Exception e)
                    {
                        RpcException rpcEx = e as RpcException;
                        ch = CustomGRPCLoadBalancerWithSingleton.GetWorkingChannel();
                    }
                }
                else
                    break;
            }
            return null;
        }

    }
}