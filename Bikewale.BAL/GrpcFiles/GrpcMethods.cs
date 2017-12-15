using System;
using System.Diagnostics;
using Bikewale.Entities.Videos;
using Bikewale.Notifications;
using Bikewale.Utility;
using EditCMSWindowsService.Messages;
using Grpc.Core;
using GRPCLoadBalancer;
using log4net;

namespace Grpc.CMS
{
    /// <summary>
    /// Author: Prasad Gawde
    /// Summary: This class is responsible to make the Grpc call for a specific function. Also it will retry the call for few times before failing in case one/all the servers are down
    /// </summary>
    public static class GrpcMethods
    {
        static readonly int m_ChanelWaitTime;
        static readonly int m_retryCount;
        static readonly ILog log = LogManager.GetLogger(typeof(GrpcMethods));
        static bool _logGrpcErrors = Convert.ToBoolean(Bikewale.Utility.BWConfiguration.Instance.LogGrpcErrors);
        static uint _msLimit = Convert.ToUInt32(Bikewale.Utility.BWConfiguration.Instance.GrpcMaxTimeLimit);

        static GrpcMethods()
        {
            m_ChanelWaitTime = Convert.ToInt32(BWConfiguration.Instance.GrpcChannelWaitTime);//2000
            m_retryCount = Convert.ToInt32(BWConfiguration.Instance.GrpcRetryCount);
        }

        static DateTime GetForwardTime(int incrementMillisecond)
        {
            return DateTime.UtcNow.Add(TimeSpan.FromMilliseconds(incrementMillisecond));
        }

        public static GrpcCMSContent GetArticleListByCategory(string catIdList, uint startIdx, uint endIdx)
        {
            Stopwatch sw = null;
            try
            {
                if (_logGrpcErrors)
                {
                    sw = Stopwatch.StartNew();
                }
                Channel ch = CustomGRPCLoadBalancerWithSingleton.GetWorkingChannel();
                int i = m_retryCount;
                while (i-- >= 0)
                {
                    if (ch != null)
                    {
                        var client = new EditCMSGrpcService.EditCMSGrpcServiceClient(ch);
                        try
                        {
                            return client.GetContentListByCategory(new GrpcArticleByCatURI()
                            {
                                ApplicationId = 2,
                                CategoryIdList = catIdList,
                                EndIndex = endIdx,
                                StartIndex = startIdx
                            },
                          null, GetForwardTime(m_ChanelWaitTime));
                        }
                        catch (RpcException e)
                        {
                            log.Error(e);
                            if (i > 0)
                            {
                                log.Error("Error104 Get another Channel " + ch.ResolvedTarget);
                                ch = CustomGRPCLoadBalancerWithSingleton.GetWorkingChannel();
                            }
                            else
                                break;
                        }
                        catch (Exception e)
                        {
                            log.Error(e);
                        }
                    }
                    else
                        break;
                }


                return null;
            }
            finally
            {
                if (_logGrpcErrors && sw != null)
                {
                    sw.Stop();
                    if (sw.ElapsedMilliseconds > _msLimit)
                        log.Error("Error105 GetArticleListByCategory took " + sw.ElapsedMilliseconds);
                }
            }

        }

        public static GrpcCMSContent GetArticleListByCategory(string catIdList, uint startIdx, uint endIdx, int makeid = 0, int modelid = 0)
        {
            Stopwatch sw = null;
            try
            {
                if (_logGrpcErrors)
                {
                    sw = Stopwatch.StartNew();
                }
                Channel ch = CustomGRPCLoadBalancerWithSingleton.GetWorkingChannel();
                int i = m_retryCount;
                while (i-- >= 0)
                {
                    if (ch != null)
                    {
                        var client = new EditCMSGrpcService.EditCMSGrpcServiceClient(ch);
                        try
                        {
                            return client.GetContentListByCategory(new GrpcArticleByCatURI()
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
                        catch (RpcException e)
                        {
                            log.Error(e);
                            if (i > 0)
                            {
                                log.Error("Error104 Get another Channel " + ch.ResolvedTarget);
                                ch = CustomGRPCLoadBalancerWithSingleton.GetWorkingChannel();
                            }
                            else
                                break;
                        }
                        catch (Exception e)
                        {
                            log.Error(e);
                        }
                    }
                    else
                        break;
                }


                return null;
            }
            finally
            {
                if (_logGrpcErrors && sw != null)
                {
                    sw.Stop();
                    if (sw.ElapsedMilliseconds > _msLimit)
                        log.Error("Error105 GetArticleListByCategory took " + sw.ElapsedMilliseconds);
                }
            }

        }

        /// <summary>
        /// Created By  : Sushil Kumar on 22nd Sep 2017
        /// Description : Addded new overload method to fetch data according to categorylist with multiple model ids 
        /// Modified By : Vivek Singh Tomar on 27th Nov 2017
        /// Description : Added model ids as parameter in the grpc call
        /// </summary>
        /// <param name="catIdList"></param>
        /// <param name="startIdx"></param>
        /// <param name="endIdx"></param>
        /// <param name="makeid"></param>
        /// <param name="modelIds"></param>
        /// <returns></returns>
        public static GrpcCMSContent GetArticleListByCategory(string catIdList, uint startIdx, uint endIdx, int makeid = 0, string modelIds = null)
        {
            Stopwatch sw = null;
            try
            {
                if (_logGrpcErrors)
                {
                    sw = Stopwatch.StartNew();
                }
                Channel ch = CustomGRPCLoadBalancerWithSingleton.GetWorkingChannel();
                int i = m_retryCount;
                while (i-- >= 0)
                {
                    if (ch != null)
                    {
                        var client = new EditCMSGrpcService.EditCMSGrpcServiceClient(ch);
                        try
                        {
                            return client.GetContentListByCategory(new GrpcArticleByCatURI()
                            {
                                ApplicationId = 2,
                                CategoryIdList = catIdList,
                                EndIndex = endIdx,
                                MakeId = makeid,
                                ModelIds = modelIds,
                                StartIndex = startIdx
                            },
                          null, GetForwardTime(m_ChanelWaitTime));
                        }
                        catch (RpcException e)
                        {
                            log.Error(e);
                            if (i > 0)
                            {
                                log.Error("Error104 Get another Channel " + ch.ResolvedTarget);
                                ch = CustomGRPCLoadBalancerWithSingleton.GetWorkingChannel();
                            }
                            else
                                break;
                        }
                        catch (Exception e)
                        {
                            log.Error(e);
                        }
                    }
                    else
                        break;
                }


                return null;
            }
            finally
            {
                if (_logGrpcErrors && sw != null)
                {
                    sw.Stop();
                    if (sw.ElapsedMilliseconds > _msLimit)
                        log.Error("Error105 GetArticleListByCategory took " + sw.ElapsedMilliseconds);
                }
            }

        }

        /// <summary>
        /// Created by: Vivek Singh Tomar on 16th Aug 2017
        /// Summary: Get Article List by Category for provided body style
        /// </summary>
        /// <param name="catIdList"></param>
        /// <param name="startIdx"></param>
        /// <param name="endIdx"></param>
        /// <param name="bodyStyleId"></param>
        /// <param name="makeid"></param>
        /// <returns></returns>
        public static GrpcCMSContent GetArticleListByCategory(string catIdList, uint startIdx, uint endIdx, string bodyStyleId, int makeid = 0)
        {
            Stopwatch sw = null;
            try
            {
                if (_logGrpcErrors)
                {
                    sw = Stopwatch.StartNew();
                }
                Channel ch = CustomGRPCLoadBalancerWithSingleton.GetWorkingChannel();
                int i = m_retryCount;
                while (i-- >= 0)
                {
                    if (ch != null)
                    {
                        var client = new EditCMSGrpcService.EditCMSGrpcServiceClient(ch);
                        try
                        {
                            return client.GetContentListByCategory(new GrpcArticleByCatURI()
                            {
                                ApplicationId = 2,
                                CategoryIdList = catIdList,
                                EndIndex = endIdx,
                                MakeId = makeid,
                                StartIndex = startIdx,
                                BodyStyleIds = bodyStyleId
                            },
                          null, GetForwardTime(m_ChanelWaitTime));
                        }
                        catch (RpcException e)
                        {
                            log.Error(e);
                            ErrorClass.LogError(e, "Grpc.CMS.GrpcMethods.GrpcCMSContent");

                            if (i > 0)
                            {
                                log.Error("Error104 Get another Channel " + ch.ResolvedTarget);
                                ch = CustomGRPCLoadBalancerWithSingleton.GetWorkingChannel();
                            }
                            else
                                break;
                        }
                        catch (Exception e)
                        {
                            log.Error(e);
                            ErrorClass.LogError(e, "Grpc.CMS.GrpcMethods.GrpcCMSContent");
                        }
                    }
                    else
                        break;
                }


                return null;
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Grpc.CMS.GrpcMethods.GrpcCMSContent");
                return null;
            }
            finally
            {
                if (_logGrpcErrors)
                {
                    sw.Stop();
                    if (sw.ElapsedMilliseconds > _msLimit)
                        log.Error("Error105 GetArticleListByCategory took " + sw.ElapsedMilliseconds);
                }
            }

        }

        public static GrpcArticleSummaryList MostRecentList(string contenTypes, int totalRecords, int? makeId = 0, int? modelId = 0)
        {
            Stopwatch sw = null;

            try
            {
                if (_logGrpcErrors)
                {
                    sw = Stopwatch.StartNew();
                }

                Channel ch = CustomGRPCLoadBalancerWithSingleton.GetWorkingChannel();

                int i = m_retryCount;
                while (i-- >= 0)
                {
                    if (ch != null)
                    {
                        var client = new EditCMSGrpcService.EditCMSGrpcServiceClient(ch);
                        try
                        {

                            return client.GetMostRecentArticles
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
                        catch (RpcException e)
                        {
                            log.Error(e);
                            if (i > 0)
                            {
                                log.Error("Error104 Get another Channel " + ch.ResolvedTarget);
                                ch = CustomGRPCLoadBalancerWithSingleton.GetWorkingChannel();
                            }
                            else
                                break;
                        }
                        catch (Exception e)
                        {
                            log.Error(e);
                        }
                    }
                    else
                        break;
                }
                return null;
            }
            finally
            {
                if (_logGrpcErrors)
                {

                    sw.Stop();
                    if (sw.ElapsedMilliseconds > _msLimit)
                        log.Error("Error105 MostRecentList took " + sw.ElapsedMilliseconds);
                }
            }
        }

        /// <summary>
        /// Created By  : Sushil Kumar on 22nd Sep 2017
        /// Description : Addded new overload method to fetch most recent data with multiple model ids 
        /// </summary>
        /// <param name="contenTypes"></param>
        /// <param name="totalRecords"></param>
        /// <param name="makeId"></param>
        /// <param name="modelIds"></param>
        /// <returns></returns>
        public static GrpcArticleSummaryList MostRecentList(string contenTypes, int totalRecords, int? makeId = 0, string modelIds = null)
        {
            Stopwatch sw = null;

            try
            {
                if (_logGrpcErrors)
                {
                    sw = Stopwatch.StartNew();
                }

                Channel ch = CustomGRPCLoadBalancerWithSingleton.GetWorkingChannel();

                int i = m_retryCount;
                while (i-- >= 0)
                {
                    if (ch != null)
                    {
                        var client = new EditCMSGrpcService.EditCMSGrpcServiceClient(ch);
                        try
                        {

                            return client.GetMostRecentArticles
                                (new GrpcArticleRecentURI()
                                {
                                    MakeId = makeId == null ? 0 : makeId.Value,
                                    ApplicationId = 2,
                                    ContentTypes = contenTypes,
                                    TotalRecords = (uint)totalRecords,
                                    ModelIds = String.IsNullOrEmpty(modelIds) ? string.Empty : modelIds,
                                },
                                 null, GetForwardTime(m_ChanelWaitTime));
                        }
                        catch (RpcException e)
                        {
                            log.Error(e);
                            if (i > 0)
                            {
                                log.Error("Error104 Get another Channel " + ch.ResolvedTarget);
                                ch = CustomGRPCLoadBalancerWithSingleton.GetWorkingChannel();
                            }
                            else
                                break;
                        }
                        catch (Exception e)
                        {
                            log.Error(e);
                        }
                    }
                    else
                        break;
                }
                return null;
            }
            finally
            {
                if (_logGrpcErrors && sw != null)
                {

                    sw.Stop();
                    if (sw.ElapsedMilliseconds > _msLimit)
                        log.Error("Error105 MostRecentList took " + sw.ElapsedMilliseconds);
                }
            }
        }

        public static GrpcArticleSummaryList MostRecentList(string contenTypes, int totalRecords, string bodyStyleIds, int? makeId = 0, int? modelId = 0)
        {
            Stopwatch sw = null;

            try
            {
                if (_logGrpcErrors)
                {
                    sw = Stopwatch.StartNew();
                }

                Channel ch = CustomGRPCLoadBalancerWithSingleton.GetWorkingChannel();

                int i = m_retryCount;
                while (i-- >= 0)
                {
                    if (ch != null)
                    {
                        var client = new EditCMSGrpcService.EditCMSGrpcServiceClient(ch);
                        try
                        {

                            return client.GetMostRecentArticles
                                (new GrpcArticleRecentURI()
                                {
                                    MakeId = makeId == null ? 0 : makeId.Value,
                                    ModelId = modelId == null ? 0 : modelId.Value,

                                    ApplicationId = 2,
                                    ContentTypes = contenTypes,
                                    TotalRecords = (uint)totalRecords,
                                    BodyStyleIds = bodyStyleIds
                                },
                                 null, GetForwardTime(m_ChanelWaitTime));
                        }
                        catch (RpcException e)
                        {
                            log.Error(e);
                            if (i > 0)
                            {
                                log.Error("Error104 Get another Channel " + ch.ResolvedTarget);
                                ch = CustomGRPCLoadBalancerWithSingleton.GetWorkingChannel();
                            }
                            else
                                break;
                        }
                        catch (Exception e)
                        {
                            log.Error(e);
                        }
                    }
                    else
                        break;
                }
                return null;
            }
            finally
            {
                if (_logGrpcErrors && sw != null)
                {

                    sw.Stop();
                    if (sw.ElapsedMilliseconds > _msLimit)
                        log.Error("Error105 MostRecentList took " + sw.ElapsedMilliseconds);
                }
            }
        }

        /// <summary>
        /// Created By  : Sushil Kumar on 22nd Sep 2017
        /// Description : Addded new overload method to fetch most recent data with multiple model ids 
        /// </summary>
        /// <param name="contenTypes"></param>
        /// <param name="totalRecords"></param>
        /// <param name="bodyStyleIds"></param>
        /// <param name="makeId"></param>
        /// <param name="modelIds"></param>
        /// <returns></returns>
        public static GrpcArticleSummaryList MostRecentList(string contenTypes, int totalRecords, string bodyStyleIds, int? makeId = 0, string modelIds = null)
        {
            Stopwatch sw = null;

            try
            {
                if (_logGrpcErrors)
                {
                    sw = Stopwatch.StartNew();
                }

                Channel ch = CustomGRPCLoadBalancerWithSingleton.GetWorkingChannel();

                int i = m_retryCount;
                while (i-- >= 0)
                {
                    if (ch != null)
                    {
                        var client = new EditCMSGrpcService.EditCMSGrpcServiceClient(ch);
                        try
                        {

                            return client.GetMostRecentArticles
                                (new GrpcArticleRecentURI()
                                {
                                    MakeId = makeId == null ? 0 : makeId.Value,
                                    //ModelIds = modelIds,
                                    ApplicationId = 2,
                                    ContentTypes = contenTypes,
                                    TotalRecords = (uint)totalRecords,
                                    BodyStyleIds = bodyStyleIds
                                },
                                 null, GetForwardTime(m_ChanelWaitTime));
                        }
                        catch (RpcException e)
                        {
                            log.Error(e);
                            if (i > 0)
                            {
                                log.Error("Error104 Get another Channel " + ch.ResolvedTarget);
                                ch = CustomGRPCLoadBalancerWithSingleton.GetWorkingChannel();
                            }
                            else
                                break;
                        }
                        catch (Exception e)
                        {
                            log.Error(e);
                        }
                    }
                    else
                        break;
                }
                return null;
            }
            finally
            {
                if (_logGrpcErrors)
                {

                    sw.Stop();
                    if (sw.ElapsedMilliseconds > _msLimit)
                        log.Error("Error105 MostRecentList took " + sw.ElapsedMilliseconds);
                }
            }
        }

        public static GrpcModelImageList GetArticlePhotos(ulong basicId)
        {
            Stopwatch sw = null;

            try
            {
                if (_logGrpcErrors)
                {
                    sw = Stopwatch.StartNew();
                }
                Channel ch = CustomGRPCLoadBalancerWithSingleton.GetWorkingChannel();

                int i = m_retryCount;
                while (i-- >= 0)
                {
                    if (ch != null)
                    {
                        var client = new EditCMSGrpcService.EditCMSGrpcServiceClient(ch);
                        try
                        {

                            return client.GetArticlePhotos
                                (new GrpcArticleContentURI()
                                {
                                    BasicId = basicId,
                                    ApplicationId = 2
                                },
                                 null, GetForwardTime(m_ChanelWaitTime));
                        }
                        catch (RpcException e)
                        {
                            log.Error(e);
                            if (i > 0)
                            {
                                log.Error("Error104 Get another Channel " + ch.ResolvedTarget);
                                ch = CustomGRPCLoadBalancerWithSingleton.GetWorkingChannel();
                            }
                            else
                                break;
                        }
                        catch (Exception e)
                        {
                            log.Error(e);
                        }
                    }
                    else
                        break;
                }
                return null;
            }
            finally
            {
                if (_logGrpcErrors)
                {

                    sw.Stop();
                    if (sw.ElapsedMilliseconds > _msLimit)
                        log.Error("Error105 GetArticlePhotos took " + sw.ElapsedMilliseconds);
                }
            }
        }

        public static GrpcModelImageList GetModelPhotosList(uint applicationId, int modelId, string categoryId)
        {
            Stopwatch sw = null;

            try
            {
                if (_logGrpcErrors)
                {
                    sw = Stopwatch.StartNew();
                }
                Channel ch = CustomGRPCLoadBalancerWithSingleton.GetWorkingChannel();

                int i = m_retryCount;
                while (i-- >= 0)
                {
                    if (ch != null)
                    {
                        var client = new EditCMSGrpcService.EditCMSGrpcServiceClient(ch);
                        try
                        {

                            return client.GetModelPhotosList
                                (new GrpcModelPhotoURI()
                                {
                                    ApplicationId = applicationId,
                                    ModelId = modelId,
                                    CategoryIdList = categoryId
                                },
                                 null, GetForwardTime(m_ChanelWaitTime));
                        }
                        catch (RpcException e)
                        {
                            log.Error(e);
                            if (i > 0)
                            {
                                log.Error("Error104 Get another Channel " + ch.ResolvedTarget);
                                ch = CustomGRPCLoadBalancerWithSingleton.GetWorkingChannel();
                            }
                            else
                                break;
                        }
                        catch (Exception e)
                        {
                            log.Error(e);
                        }
                    }
                    else
                        break;
                }
                return null;
            }
            finally
            {
                if (_logGrpcErrors)
                {

                    sw.Stop();
                    if (sw.ElapsedMilliseconds > _msLimit)
                        log.Error("Error105 GetModelPhotosList took " + sw.ElapsedMilliseconds);
                }
            }
        }


        public static GrpcArticleDetails GetContentDetails(ulong basicId)
        {
            Stopwatch sw = null;

            try
            {
                if (_logGrpcErrors)
                {
                    sw = Stopwatch.StartNew();
                }
                Channel ch = CustomGRPCLoadBalancerWithSingleton.GetWorkingChannel();

                int i = m_retryCount;
                while (i-- >= 0)
                {
                    if (ch != null)
                    {
                        var client = new EditCMSGrpcService.EditCMSGrpcServiceClient(ch);
                        try
                        {

                            return client.GetContentDetails
                                (new GrpcArticleContentURI()
                                {
                                    BasicId = basicId,
                                    ApplicationId = 2
                                },
                                 null, GetForwardTime(m_ChanelWaitTime));
                        }
                        catch (RpcException e)
                        {
                            log.Error(e);
                            if (i > 0)
                            {
                                log.Error("Error104 Get another Channel " + ch.ResolvedTarget);
                                ch = CustomGRPCLoadBalancerWithSingleton.GetWorkingChannel();
                            }
                            else
                                break;
                        }
                        catch (Exception e)
                        {
                            log.Error(e);
                        }
                    }
                    else
                        break;
                }
                return null;
            }
            finally
            {
                if (_logGrpcErrors)
                {

                    sw.Stop();
                    if (sw.ElapsedMilliseconds > _msLimit)
                        log.Error("Error105 GetContentDetails took " + sw.ElapsedMilliseconds);
                }
            }
        }

        public static GrpcArticlePageDetails GetContentPages(ulong basicId)
        {
            Stopwatch sw = null;

            try
            {
                if (_logGrpcErrors)
                {
                    sw = Stopwatch.StartNew();
                }
                Channel ch = CustomGRPCLoadBalancerWithSingleton.GetWorkingChannel();

                int i = m_retryCount;
                while (i-- >= 0)
                {
                    if (ch != null)
                    {
                        var client = new EditCMSGrpcService.EditCMSGrpcServiceClient(ch);
                        try
                        {

                            return client.GetContentPages
                                (new GrpcArticleContentURI()
                                {
                                    BasicId = basicId,
                                    ApplicationId = 2
                                },
                                 null, GetForwardTime(m_ChanelWaitTime));
                        }
                        catch (RpcException e)
                        {
                            log.Error(e);
                            if (i > 0)
                            {
                                log.Error("Error104 Get another Channel " + ch.ResolvedTarget);
                                ch = CustomGRPCLoadBalancerWithSingleton.GetWorkingChannel();
                            }
                            else
                                break;
                        }
                        catch (Exception e)
                        {
                            log.Error(e);
                        }
                    }
                    else
                        break;
                }
                return null;
            }
            finally
            {
                if (_logGrpcErrors)
                {

                    sw.Stop();
                    if (sw.ElapsedMilliseconds > _msLimit)
                        log.Error("Error105 GetContentPages took " + sw.ElapsedMilliseconds);
                }
            }
        }

        public static GrpcVideosList GetVideosByModelId(int modelId, uint startId, uint endId)
        {
            Stopwatch sw = null;

            try
            {
                if (_logGrpcErrors)
                {
                    sw = Stopwatch.StartNew();
                }
                Channel ch = CustomGRPCLoadBalancerWithSingleton.GetWorkingChannel();

                int i = m_retryCount;
                while (i-- >= 0)
                {
                    if (ch != null)
                    {
                        var client = new EditCMSGrpcService.EditCMSGrpcServiceClient(ch);
                        try
                        {

                            return client.GetVideosByModelId
                                (new GrpcVideosByIdURI()
                                {
                                    Id = modelId,
                                    ApplicationId = 2,
                                    StartIndex = startId,
                                    EndIndex = endId
                                },

                                 null, GetForwardTime(m_ChanelWaitTime));
                        }
                        catch (RpcException e)
                        {
                            log.Error(e);
                            if (i > 0)
                            {
                                log.Error("Error104 Get another Channel " + ch.ResolvedTarget);
                                ch = CustomGRPCLoadBalancerWithSingleton.GetWorkingChannel();
                            }
                            else
                                break;
                        }
                        catch (Exception e)
                        {
                            log.Error(e);
                        }
                    }
                    else
                        break;
                }
                return null;
            }
            finally
            {
                if (_logGrpcErrors)
                {

                    sw.Stop();
                    if (sw.ElapsedMilliseconds > _msLimit)
                        log.Error("Error105 GetVideosByModelID took " + sw.ElapsedMilliseconds);
                }
            }
        }
        public static GrpcVideosList GetVideosByModelId(int modelId, uint startId, uint endId, string bodyStyleId)
        {
            Stopwatch sw = null;

            try
            {
                if (_logGrpcErrors)
                {
                    sw = Stopwatch.StartNew();
                }
                Channel ch = CustomGRPCLoadBalancerWithSingleton.GetWorkingChannel();

                int i = m_retryCount;
                while (i-- >= 0)
                {
                    if (ch != null)
                    {
                        var client = new EditCMSGrpcService.EditCMSGrpcServiceClient(ch);
                        try
                        {

                            return client.GetVideosByModelId
                                (new GrpcVideosByIdURI()
                                {
                                    Id = modelId,
                                    ApplicationId = 2,
                                    StartIndex = startId,
                                    EndIndex = endId,
                                    BodyStyleIds = bodyStyleId
                                },

                                 null, GetForwardTime(m_ChanelWaitTime));
                        }
                        catch (RpcException e)
                        {
                            log.Error(e);
                            if (i > 0)
                            {
                                log.Error("Error104 Get another Channel " + ch.ResolvedTarget);
                                ch = CustomGRPCLoadBalancerWithSingleton.GetWorkingChannel();
                            }
                            else
                                break;
                        }
                        catch (Exception e)
                        {
                            log.Error(e);
                        }
                    }
                    else
                        break;
                }
                return null;
            }
            finally
            {
                if (_logGrpcErrors)
                {

                    sw.Stop();
                    if (sw.ElapsedMilliseconds > _msLimit)
                        log.Error("Error105 GetVideosByModelID took " + sw.ElapsedMilliseconds);
                }
            }
        }
        public static GrpcVideosList GetVideosByMakeId(int makeId, uint startId, uint endId)
        {
            Stopwatch sw = null;

            try
            {
                if (_logGrpcErrors)
                {
                    sw = Stopwatch.StartNew();
                }
                Channel ch = CustomGRPCLoadBalancerWithSingleton.GetWorkingChannel();

                int i = m_retryCount;
                while (i-- >= 0)
                {
                    if (ch != null)
                    {
                        var client = new EditCMSGrpcService.EditCMSGrpcServiceClient(ch);
                        try
                        {

                            return client.GetVideosByMakeId
                                (new GrpcVideosByIdURI()
                                {
                                    Id = makeId,
                                    ApplicationId = 2,
                                    StartIndex = startId,
                                    EndIndex = endId
                                },

                                 null, GetForwardTime(m_ChanelWaitTime));
                        }
                        catch (RpcException e)
                        {
                            log.Error(e);
                            if (i > 0)
                            {
                                log.Error("Error104 Get another Channel " + ch.ResolvedTarget);
                                ch = CustomGRPCLoadBalancerWithSingleton.GetWorkingChannel();
                            }
                            else
                                break;
                        }
                        catch (Exception e)
                        {
                            log.Error(e);
                        }
                    }
                    else
                        break;
                }
                return null;
            }
            finally
            {
                if (_logGrpcErrors)
                {

                    sw.Stop();
                    if (sw.ElapsedMilliseconds > _msLimit)
                        log.Error("Error105 GetVideosByMakeId took " + sw.ElapsedMilliseconds);
                }
            }
        }

        public static GrpcVideosList GetVideosByMakeId(int makeId, uint startId, uint endId, string bodyStyleId)
        {
            Stopwatch sw = null;

            try
            {
                if (_logGrpcErrors)
                {
                    sw = Stopwatch.StartNew();
                }
                Channel ch = CustomGRPCLoadBalancerWithSingleton.GetWorkingChannel();

                int i = m_retryCount;
                while (i-- >= 0)
                {
                    if (ch != null)
                    {
                        var client = new EditCMSGrpcService.EditCMSGrpcServiceClient(ch);
                        try
                        {

                            return client.GetVideosByMakeId
                                (new GrpcVideosByIdURI()
                                {
                                    Id = makeId,
                                    ApplicationId = 2,
                                    StartIndex = startId,
                                    EndIndex = endId,
                                    BodyStyleIds = bodyStyleId
                                },

                                 null, GetForwardTime(m_ChanelWaitTime));
                        }
                        catch (RpcException e)
                        {
                            log.Error(e);
                            if (i > 0)
                            {
                                log.Error("Error104 Get another Channel " + ch.ResolvedTarget);
                                ch = CustomGRPCLoadBalancerWithSingleton.GetWorkingChannel();
                            }
                            else
                                break;
                        }
                        catch (Exception e)
                        {
                            log.Error(e);
                        }
                    }
                    else
                        break;
                }
                return null;
            }
            finally
            {
                if (_logGrpcErrors)
                {

                    sw.Stop();
                    if (sw.ElapsedMilliseconds > _msLimit)
                        log.Error("Error105 GetVideosByMakeId took " + sw.ElapsedMilliseconds);
                }
            }
        }

        public static GrpcVideosList GetVideosBySubCategory(uint catId, uint startId, uint endId)
        {
            Stopwatch sw = null;

            try
            {
                if (_logGrpcErrors)
                {
                    sw = Stopwatch.StartNew();
                }
                Channel ch = CustomGRPCLoadBalancerWithSingleton.GetWorkingChannel();

                int i = m_retryCount;
                while (i-- >= 0)
                {
                    if (ch != null)
                    {
                        var client = new EditCMSGrpcService.EditCMSGrpcServiceClient(ch);
                        try
                        {

                            return client.GetVideosBySubCategory
                                (new GrpcVideosBySubCategoryURI()
                                {
                                    ApplicationId = 2,
                                    SubCategoryId = catId,
                                    StartIndex = startId,
                                    EndIndex = endId                                    
                                },

                                 null, GetForwardTime(m_ChanelWaitTime));
                        }
                        catch (RpcException e)
                        {
                            log.Error(e);
                            if (i > 0)
                            {
                                log.Error("Error104 Get another Channel " + ch.ResolvedTarget);
                                ch = CustomGRPCLoadBalancerWithSingleton.GetWorkingChannel();
                            }
                            else
                                break;
                        }
                        catch (Exception e)
                        {
                            log.Error(e);
                        }
                    }
                    else
                        break;
                }
                return null;
            }
            finally
            {
                if (_logGrpcErrors)
                {

                    sw.Stop();
                    if (sw.ElapsedMilliseconds > _msLimit)
                        log.Error("Error105 GetVideosBySubCategory took " + sw.ElapsedMilliseconds);
                }
            }
        }

        public static GrpcVideosList GetVideosBySubCategory(uint catId, uint startId, uint endId, string bodyStyleId)
        {
            Stopwatch sw = null;

            try
            {
                if (_logGrpcErrors)
                {
                    sw = Stopwatch.StartNew();
                }
                Channel ch = CustomGRPCLoadBalancerWithSingleton.GetWorkingChannel();

                int i = m_retryCount;
                while (i-- >= 0)
                {
                    if (ch != null)
                    {
                        var client = new EditCMSGrpcService.EditCMSGrpcServiceClient(ch);
                        try
                        {

                            return client.GetVideosBySubCategory
                                (new GrpcVideosBySubCategoryURI()
                                {
                                    ApplicationId = 2,
                                    SubCategoryId = catId,
                                    StartIndex = startId,
                                    EndIndex = endId,
                                    BodyStyleIds = bodyStyleId
                                },

                                 null, GetForwardTime(m_ChanelWaitTime));
                        }
                        catch (RpcException e)
                        {
                            log.Error(e);
                            if (i > 0)
                            {
                                log.Error("Error104 Get another Channel " + ch.ResolvedTarget);
                                ch = CustomGRPCLoadBalancerWithSingleton.GetWorkingChannel();
                            }
                            else
                                break;
                        }
                        catch (Exception e)
                        {
                            log.Error(e);
                        }
                    }
                    else
                        break;
                }
                return null;
            }
            finally
            {
                if (_logGrpcErrors)
                {

                    sw.Stop();
                    if (sw.ElapsedMilliseconds > _msLimit)
                        log.Error("Error105 GetVideosBySubCategory took " + sw.ElapsedMilliseconds);
                }
            }
        }


        private static GrpcVideoSortOrderCategory MapVideosSortOrder(VideosSortOrder sortOrder)
        {
            switch (sortOrder)
            {
                case VideosSortOrder.FeaturedAndLatest:
                    return GrpcVideoSortOrderCategory.FeaturedAndLatest;
                case VideosSortOrder.MostPopular:
                    return GrpcVideoSortOrderCategory.MostPopular;
                case VideosSortOrder.JustLatest:
                    return GrpcVideoSortOrderCategory.JustLatest;
                default:
                    return GrpcVideoSortOrderCategory.MostPopular;
            }
        }

        public static GrpcVideoListEntity GetVideosBySubCategories(string catIds, uint startIndex, uint endIndex, VideosSortOrder sortOrder)
        {
            Stopwatch sw = null;

            try
            {
                if (_logGrpcErrors)
                {
                    sw = Stopwatch.StartNew();
                }

                Channel ch = CustomGRPCLoadBalancerWithSingleton.GetWorkingChannel();

                int i = m_retryCount;
                while (i-- >= 0)
                {
                    if (ch != null)
                    {
                        var client = new EditCMSGrpcService.EditCMSGrpcServiceClient(ch);
                        try
                        {

                            return client.GetVideosBySubCategories
                                (new GrpcVideosBySubCategoriesURI()
                                {
                                    ApplicationId = 2,
                                    SubCategoryIds = catIds,
                                    StartIndex = startIndex,
                                    EndIndex = endIndex,
                                    SortCategory = MapVideosSortOrder(sortOrder)
                                },

                                 null, GetForwardTime(m_ChanelWaitTime));
                        }
                        catch (RpcException e)
                        {
                            log.Error(e);
                            if (i > 0)
                            {
                                log.Error("Error104 Get another Channel " + ch.ResolvedTarget);
                                ch = CustomGRPCLoadBalancerWithSingleton.GetWorkingChannel();
                            }
                            else
                                break;
                        }
                        catch (Exception e)
                        {
                            log.Error(e);
                        }
                    }
                    else
                        break;
                }
                return null;
            }
            finally
            {
                if (_logGrpcErrors)
                {

                    sw.Stop();
                    if (sw.ElapsedMilliseconds > _msLimit)
                        log.Error("Error105 GetVideosBySubcategories took " + sw.ElapsedMilliseconds);
                }
            }
        }

        public static GrpcVideosList GetSimilarVideos(int id, int totalCount)
        {
            Stopwatch sw = null;

            try
            {
                if (_logGrpcErrors)
                {
                    sw = Stopwatch.StartNew();
                }
                Channel ch = CustomGRPCLoadBalancerWithSingleton.GetWorkingChannel();

                int i = m_retryCount;
                while (i-- >= 0)
                {
                    if (ch != null)
                    {
                        var client = new EditCMSGrpcService.EditCMSGrpcServiceClient(ch);
                        try
                        {

                            return client.GetSimilarVideos
                                (new GrpcVideosByIdURI()
                                {
                                    ApplicationId = 2,
                                    Id = id,
                                    StartIndex = 1,
                                    EndIndex = (uint)totalCount
                                },

                                 null, GetForwardTime(m_ChanelWaitTime));
                        }
                        catch (RpcException e)
                        {
                            log.Error(e);
                            if (i > 0)
                            {
                                log.Error("Error104 Get another Channel " + ch.ResolvedTarget);
                                ch = CustomGRPCLoadBalancerWithSingleton.GetWorkingChannel();
                            }
                            else
                                break;
                        }
                        catch (Exception e)
                        {
                            log.Error(e);
                        }
                    }
                    else
                        break;
                }
                return null;
            }
            finally
            {
                if (_logGrpcErrors)
                {

                    sw.Stop();
                    if (sw.ElapsedMilliseconds > _msLimit)
                        log.Error("Error105 GetSimilarVideos took " + sw.ElapsedMilliseconds);
                }
            }
        }
        /// <summary>
        /// Created by : Ashutosh Sharma on 17 Nov 2017
        /// Description : Grpc method to get videos of multiple model ids.
        /// Modified by : Ashutosh Sharma on 11 Dec 2017
        /// Description : Removed id from call of GetSimilarVideos.
        /// </summary>
        /// <param name="totalCount"></param>
        /// <param name="modelIdList"></param>
        /// <returns></returns>
		public static GrpcVideosList GetSimilarVideos(uint totalCount, string modelIdList)
		{
			Stopwatch sw = null;

			try
			{
				if (_logGrpcErrors)
				{
					sw = Stopwatch.StartNew();
				}
				Channel ch = CustomGRPCLoadBalancerWithSingleton.GetWorkingChannel();

				int i = m_retryCount;
				while (i-- >= 0)
				{
					if (ch != null)
					{
						var client = new EditCMSGrpcService.EditCMSGrpcServiceClient(ch);
						try
						{

							return client.GetSimilarVideos
								(new GrpcVideosByIdURI()
								{
									ApplicationId = 2,
									SimilarModels = modelIdList,
									StartIndex = 1,
									EndIndex = totalCount
								},

								 null, GetForwardTime(m_ChanelWaitTime));
						}
						catch (RpcException e)
						{
							log.Error(e);
							if (i > 0)
							{
								log.Error("Error104 Get another Channel " + ch.ResolvedTarget);
								ch = CustomGRPCLoadBalancerWithSingleton.GetWorkingChannel();
							}
							else
								break;
						}
						catch (Exception e)
						{
							log.Error(e);
						}
					}
					else
						break;
				}
				return null;
			}
			finally
			{
				if (_logGrpcErrors && sw != null)
				{
					sw.Stop();
					if (sw.ElapsedMilliseconds > _msLimit)
						log.Error("Error105 GetSimilarVideos took " + sw.ElapsedMilliseconds);
				}
			}
		}

		public static GrpcVideo GetVideoByBasicId(int id)
        {
            Stopwatch sw = null;

            try
            {
                if (_logGrpcErrors)
                {
                    sw = Stopwatch.StartNew();
                }
                Channel ch = CustomGRPCLoadBalancerWithSingleton.GetWorkingChannel();

                int i = m_retryCount;
                while (i-- >= 0)
                {
                    if (ch != null)
                    {
                        var client = new EditCMSGrpcService.EditCMSGrpcServiceClient(ch);
                        try
                        {

                            return client.GetVideoByBasicId
                                (new GrpcVideosByIdURI()
                                {
                                    ApplicationId = 2,
                                    Id = id
                                },

                                 null, GetForwardTime(m_ChanelWaitTime));
                        }
                        catch (RpcException e)
                        {
                            log.Error(e);
                            if (i > 0)
                            {
                                log.Error("Error104 Get another Channel " + ch.ResolvedTarget);
                                ch = CustomGRPCLoadBalancerWithSingleton.GetWorkingChannel();
                            }
                            else
                                break;
                        }
                        catch (Exception e)
                        {
                            log.Error(e);
                        }
                    }
                    else
                        break;
                }
                return null;
            }
            finally
            {
                if (_logGrpcErrors)
                {

                    sw.Stop();
                    if (sw.ElapsedMilliseconds > _msLimit)
                        log.Error("Error105 GetVideosByID took " + sw.ElapsedMilliseconds);
                }
            }
        }

        /// <summary>
        /// Created by : Ashutosh Sharma on 20-Sep-2017
        /// Description :  GRPC method to get author list.
        /// </summary>
        /// <param name="applicationId"></param>
        /// <returns></returns>
        public static GrpcAuthorList GetAuthorsList(int applicationId)
        {
            Stopwatch sw = null;
            try
            {
                if (_logGrpcErrors)
                    sw = Stopwatch.StartNew();
                Channel ch = CustomGRPCLoadBalancerWithSingleton.GetWorkingChannel();
                int i = m_retryCount;
                while (i-- >= 0)
                {
                    if (ch != null)
                    {
                        var client = new EditCMSGrpcService.EditCMSGrpcServiceClient(ch);
                        try
                        {
                            return client.GetAuthorsList(new GrpcInt()
                            {
                                ApplicationId = applicationId
                            },
                          null, GetForwardTime(m_ChanelWaitTime));
                        }
                        catch (RpcException e)
                        {
                            log.Error(e);
                            if (i > 0)
                            {
                                log.Error("Error104 Get another Channel " + ch.ResolvedTarget);
                                ch = CustomGRPCLoadBalancerWithSingleton.GetWorkingChannel();
                            }
                            else
                                break;
                        }
                        catch (Exception e)
                        {
                            log.Error(e);
                        }
                    }
                    else
                        break;
                }

                return null;
            }
            finally
            {
                if (_logGrpcErrors && sw != null)
                {

                    sw.Stop();
                    if (sw.ElapsedMilliseconds > _msLimit)
                        log.Error("Error105 GetAuthorsList took " + sw.ElapsedMilliseconds);
                }
            }
        }



        public static GrpcBool ClearMemCachedKEys(EditCMSCategoryEnum cat)
        {
            Channel ch = CustomGRPCLoadBalancerWithSingleton.GetWorkingChannel();

            int i = m_retryCount;
            while (i-- >= 0)
            {
                if (ch != null)
                {
                    var client = new EditCMSGrpcService.EditCMSGrpcServiceClient(ch);
                    try
                    {

                        return client.ClearMemcachedKeys
                            (new EditCMSCategory() { Category = cat },
                             null, GetForwardTime(m_ChanelWaitTime));
                    }
                    catch (RpcException e)
                    {
                        log.Error(e);
                        if (i > 0)
                        {
                            log.Error("Error104 Get another Channel " + ch.ResolvedTarget);
                            ch = CustomGRPCLoadBalancerWithSingleton.GetWorkingChannel();
                        }
                        else
                            break;
                    }
                    catch (Exception e)
                    {
                        log.Error(e);
                    }
                }
                else
                    break;
            }
            return null;
        }

        /// <summary>
        /// Created by : Vivek Singh Tomar on 19th Sep 2017
        /// Summary : Get author details w.r.t. author id
        /// </summary>
        /// <param name="authorId"></param>
        /// <returns></returns>
        public static GrpcAuthor GetAuthorDetails(int authorId)
        {
            Stopwatch sw = null;
            try
            {
                if (_logGrpcErrors)
                {
                    sw = Stopwatch.StartNew();
                }
                Channel ch = CustomGRPCLoadBalancerWithSingleton.GetWorkingChannel();
                int i = m_retryCount;
                while (i-- >= 0)
                {
                    if (ch != null)
                    {
                        var client = new EditCMSGrpcService.EditCMSGrpcServiceClient(ch);
                        try
                        {
                            return client.GetAuthorDetails(new GrpcInt()
                            {
                                IntOutput = authorId,
                                ApplicationId = 2
                            }, null, GetForwardTime(m_ChanelWaitTime));
                        }
                        catch (RpcException e)
                        {
                            log.Error(e);
                            ErrorClass.LogError(e, "Grpc.CMS.GrpcMethods.GetAuthorDetails");

                            if (i > 0)
                            {
                                log.Error("Error104 Get another Channel " + ch.ResolvedTarget);
                                ch = CustomGRPCLoadBalancerWithSingleton.GetWorkingChannel();
                            }
                            else
                                break;
                        }
                        catch (Exception e)
                        {
                            log.Error(e);
                            ErrorClass.LogError(e, "Grpc.CMS.GrpcMethods.GetAuthorDetails");
                        }
                    }
                    else
                        break;
                }
                return null;
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Grpc.CMS.GrpcMethods.GetAuthorDetails");
                return null;
            }
            finally
            {
                if (sw != null && _logGrpcErrors)
                {
                    sw.Stop();
                    if (sw.ElapsedMilliseconds > _msLimit)
                        log.Error("Error105 GetAuthorDetails took " + sw.ElapsedMilliseconds);
                }
            }
        }

        /// <summary>
        /// Created by : Vivek Singh Tomar on 19th Sep 2017
        /// Summary : Get Content List by Author
        /// </summary>
        /// <param name="authorId"></param>
        /// <param name="applicationId"></param>
        /// <param name="categoryList"></param>
        /// <returns></returns>
        public static GrpcAuthorContentList GetContentByAuthor(int authorId, int applicationId, string categoryList)
        {
            Stopwatch sw = null;
            try
            {
                if (_logGrpcErrors)
                {
                    sw = Stopwatch.StartNew();
                }
                Channel ch = CustomGRPCLoadBalancerWithSingleton.GetWorkingChannel();
                int i = m_retryCount;
                while (i-- >= 0)
                {
                    if (ch != null)
                    {
                        var client = new EditCMSGrpcService.EditCMSGrpcServiceClient(ch);
                        try
                        {
                            return client.GetContentByAuthor(new GrpcContentByAuthorURI()
                            {
                                AuthorId = authorId,
                                ApplicationId = applicationId,
                                Categoryids = categoryList
                            }, null, GetForwardTime(m_ChanelWaitTime));
                        }
                        catch (RpcException e)
                        {
                            log.Error(e);
                            ErrorClass.LogError(e, "Grpc.CMS.GrpcMethods.GetContentByAuthor");

                            if (i > 0)
                            {
                                log.Error("Error104 Get another Channel " + ch.ResolvedTarget);
                                ch = CustomGRPCLoadBalancerWithSingleton.GetWorkingChannel();
                            }
                            else
                                break;
                        }
                        catch (Exception e)
                        {
                            log.Error(e);
                            ErrorClass.LogError(e, "Grpc.CMS.GrpcMethods.GetContentByAuthor");
                        }
                    }
                    else
                        break;
                }
                return null;
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Grpc.CMS.GrpcMethods.GetContentByAuthor");
                return null;
            }
            finally
            {
                if (sw != null && _logGrpcErrors)
                {
                    sw.Stop();
                    if (sw.ElapsedMilliseconds > _msLimit)
                        log.Error("Error105 GetContentByAuthor took " + sw.ElapsedMilliseconds);
                }
            }
        }

        /// <summary>
        /// Created by : Vivek Singh Tomar on 20th Sep 2017
        /// Summary : Get List of other authors
        /// </summary>
        /// <param name="authorId"></param>
        /// <param name="applicationId"></param>
        /// <returns></returns>
        public static GrpcAuthorList GetAllOtherAuthors(int authorId, int applicationId)
        {
            Stopwatch sw = null;
            try
            {
                if (_logGrpcErrors)
                {
                    sw = Stopwatch.StartNew();
                }
                Channel ch = CustomGRPCLoadBalancerWithSingleton.GetWorkingChannel();
                int i = m_retryCount;
                while (i-- >= 0)
                {
                    if (ch != null)
                    {
                        var client = new EditCMSGrpcService.EditCMSGrpcServiceClient(ch);
                        try
                        {
                            return client.GetAllOtherAuthors(new GrpcContentByAuthorURI()
                            {
                                AuthorId = authorId,
                                ApplicationId = applicationId
                            }, null, GetForwardTime(m_ChanelWaitTime));
                        }
                        catch (RpcException e)
                        {
                            log.Error(e);
                            ErrorClass.LogError(e, "Grpc.CMS.GrpcMethods.GetContentByAuthor");

                            if (i > 0)
                            {
                                log.Error("Error104 Get another Channel " + ch.ResolvedTarget);
                                ch = CustomGRPCLoadBalancerWithSingleton.GetWorkingChannel();
                            }
                            else
                                break;
                        }
                        catch (Exception e)
                        {
                            log.Error(e);
                            ErrorClass.LogError(e, "Grpc.CMS.GrpcMethods.GetContentByAuthor");
                        }
                    }
                    else
                        break;
                }
                return null;
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Grpc.CMS.GrpcMethods.GetContentByAuthor");
                return null;
            }
            finally
            {
                if (sw != null && _logGrpcErrors)
                {
                    sw.Stop();
                    if (sw.ElapsedMilliseconds > _msLimit)
                        log.Error("Error105 GetContentByAuthor took " + sw.ElapsedMilliseconds);
                }
            }
        }

		/// <summary>
		/// Created by : Ashutosh Sharma on 13 Dec 2017
		/// Description : Grpc method to get content list by category and subcategory id.
		/// </summary>
		/// <param name="startIndex"></param>
		/// <param name="endIndex"></param>
		/// <param name="categoryIdList"></param>
		/// <param name="subCategoryIdList">Comma separated Ids, it can be empty string.</param>
		/// <param name="makeId"></param>
		/// <param name="modelId"></param>
		/// <returns></returns>
		public static GrpcCMSContent GetContentListBySubCategoryId(uint startIndex, uint endIndex, string categoryIdList, string subCategoryIdList, int makeId = 0, int modelId = 0)
		{
			Stopwatch sw = null;

			try
			{
				if (_logGrpcErrors)
				{
					sw = Stopwatch.StartNew();
				}

				Channel ch = CustomGRPCLoadBalancerWithSingleton.GetWorkingChannel();

				int i = m_retryCount;
				while (i-- >= 0)
				{
					if (ch != null)
					{
						var client = new EditCMSGrpcService.EditCMSGrpcServiceClient(ch);
						try
						{

							return client.GetContentListBySubCategoryId
								(new GrpcArticleBySubCatURI()
								{
									ApplicationId = 2,
									MakeId = makeId,
									ModelId = modelId,
									CategoryIdList = categoryIdList,
									SubCategory = subCategoryIdList,
									StartIndex = startIndex,
									EndIndex = endIndex
								},
								 null, GetForwardTime(m_ChanelWaitTime));
						}
						catch (RpcException e)
						{
							log.Error(e);
							if (i > 0)
							{
								log.Error("Error104 Get another Channel " + ch.ResolvedTarget);
								ch = CustomGRPCLoadBalancerWithSingleton.GetWorkingChannel();
							}
							else
								break;
						}
						catch (Exception e)
						{
							log.Error(e);
						}
					}
					else
						break;
				}
				return null;
			}
			finally
			{
				if (_logGrpcErrors)
				{

					sw.Stop();
					if (sw.ElapsedMilliseconds > _msLimit)
						log.Error("Error105 GetContentListBySubCategoryId took " + sw.ElapsedMilliseconds);
				}
			}
		}
	}
}