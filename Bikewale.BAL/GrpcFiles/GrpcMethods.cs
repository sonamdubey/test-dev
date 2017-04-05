using Bikewale.Entities.Videos;
using Bikewale.Utility;
using EditCMSWindowsService.Messages;
using Grpc.Core;
using GRPCLoadBalancer;
using log4net;
using System;
using System.Diagnostics;

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
        static int _msLimit = 100;
        static GrpcMethods()
        {
            m_ChanelWaitTime = Convert.ToInt32(BWConfiguration.Instance.GrpcChannelWaitTime);//2000
            m_retryCount = Convert.ToInt32(BWConfiguration.Instance.GrpcRetryCount);
        }

        static DateTime GetForwardTime(int incrementMillisecond)
        {
            return DateTime.UtcNow.Add(TimeSpan.FromMilliseconds(incrementMillisecond));
        }

        public static GrpcCMSContent GetArticleListByCategory(string catIdList, uint startIdx, uint endIdx, int makeid = 0, int modelid = 0)
        {
            Stopwatch sw = Stopwatch.StartNew();
            try
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
                                log.Error("Error104 Get another Channel "+ch.ResolvedTarget);
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
                sw.Stop();
                if(sw.ElapsedMilliseconds>_msLimit)
                    log.Error("Error105 GetArticleListByCategory took "+sw.ElapsedMilliseconds);                
            }

        }

        public static GrpcArticleSummaryList MostRecentList(string contenTypes, int totalRecords, int? makeId = 0, int? modelId = 0)
        {
            Stopwatch sw = Stopwatch.StartNew();
            try { 
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
                                 log.Error("Error104 Get another Channel "+ch.ResolvedTarget);
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
                sw.Stop();
                if (sw.ElapsedMilliseconds > _msLimit)
                    log.Error("Error105 MostRecentList took " + sw.ElapsedMilliseconds);
            }
        }

        public static GrpcModelImageList GetArticlePhotos(ulong basicId)
        {
            Stopwatch sw = Stopwatch.StartNew();
            try { 
                
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
                                 log.Error("Error104 Get another Channel "+ch.ResolvedTarget);
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
                sw.Stop();
                if (sw.ElapsedMilliseconds > _msLimit)
                    log.Error("Error105 GetArticlePhotos took " + sw.ElapsedMilliseconds);
            }
        }

        public static GrpcModelImageList GetModelPhotosList(uint applicationId, int modelId, string categoryId)
        {
            Stopwatch sw = Stopwatch.StartNew();
            try
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
                                 log.Error("Error104 Get another Channel "+ch.ResolvedTarget);
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
                sw.Stop();
                if (sw.ElapsedMilliseconds > _msLimit)
                    log.Error("Error105 GetModelPhotosList took " + sw.ElapsedMilliseconds);
            }
        }
      

        public static GrpcArticleDetails GetContentDetails(ulong basicId)
        {
            Stopwatch sw = Stopwatch.StartNew();
            try { 
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
                                 log.Error("Error104 Get another Channel "+ch.ResolvedTarget);
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
                sw.Stop();
                if (sw.ElapsedMilliseconds > _msLimit)
                    log.Error("Error105 GetContentDetails took " + sw.ElapsedMilliseconds);
            }
        }

        public static GrpcArticlePageDetails GetContentPages(ulong basicId)
        {
            Stopwatch sw = Stopwatch.StartNew();
            try { 
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
                                 log.Error("Error104 Get another Channel "+ch.ResolvedTarget);
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
                sw.Stop();
                if (sw.ElapsedMilliseconds > _msLimit)
                    log.Error("Error105 GetContentPages took " + sw.ElapsedMilliseconds);
            }
        }

        public static GrpcVideosList GetVideosByModelId(int modelId, uint startId, uint endId)
        {
            Stopwatch sw = Stopwatch.StartNew();
            try { 
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
                                 log.Error("Error104 Get another Channel "+ch.ResolvedTarget);
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
                sw.Stop();
                if (sw.ElapsedMilliseconds > _msLimit)
                    log.Error("Error105 GetVideosByModelID took " + sw.ElapsedMilliseconds);
            }
        }

        public static GrpcVideosList GetVideosByMakeId(int makeId, uint startId, uint endId)
        {
            Stopwatch sw = Stopwatch.StartNew();
            try { 
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
                                 log.Error("Error104 Get another Channel "+ch.ResolvedTarget);
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
                sw.Stop();
                if (sw.ElapsedMilliseconds > _msLimit)
                    log.Error("Error105 GetVideosByMakeId took " + sw.ElapsedMilliseconds);
            }
        }

        public static GrpcVideosList GetVideosBySubCategory(uint catId, uint startId, uint endId)
        {
            Stopwatch sw = Stopwatch.StartNew();
            try
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
                                 log.Error("Error104 Get another Channel "+ch.ResolvedTarget);
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
                sw.Stop();
                if (sw.ElapsedMilliseconds > _msLimit)
                    log.Error("Error105 GetVideosBySubCategory took " + sw.ElapsedMilliseconds);
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
            Stopwatch sw = Stopwatch.StartNew();
            try { 

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
                                 log.Error("Error104 Get another Channel "+ch.ResolvedTarget);
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
                sw.Stop();
                if (sw.ElapsedMilliseconds > _msLimit)
                    log.Error("Error105 GetVideosBySubcategories took " + sw.ElapsedMilliseconds);
            }
        }

        public static GrpcVideosList GetSimilarVideos(int id, int totalCount)
        {
            Stopwatch sw = Stopwatch.StartNew();
            try { 
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
                                 log.Error("Error104 Get another Channel "+ch.ResolvedTarget);
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
                sw.Stop();
                if (sw.ElapsedMilliseconds > _msLimit)
                    log.Error("Error105 GetSimilarVideos took " + sw.ElapsedMilliseconds);
            }
        }

        public static GrpcVideo GetVideoByBasicId(int id)
        {
            Stopwatch sw = Stopwatch.StartNew();
            try
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
                                 log.Error("Error104 Get another Channel "+ch.ResolvedTarget);
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
                sw.Stop();
                if (sw.ElapsedMilliseconds > _msLimit)
                    log.Error("Error105 GetVideosByID took " + sw.ElapsedMilliseconds);
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
                             log.Error("Error104 Get another Channel "+ch.ResolvedTarget);
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
    }
}