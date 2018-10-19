using Bikewale.BAL.ApiGateway.Adapters.Generic;
using Bikewale.BAL.ApiGateway.ApiGatewayHelper;
using Bikewale.Entities.Videos;
using Bikewale.Notifications;
using Bikewale.Utility;
using EditCMSWindowsService.Messages;
using log4net;
using System;

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
        static string _cmsModuleName = BWConfiguration.Instance.EditCMSModuleName;

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
            try
            {
                IApiGatewayCaller caller = new ApiGatewayCaller();
                GenericApiGatewayAdapter<GrpcCMSContent> adapter = new GenericApiGatewayAdapter<GrpcCMSContent>(_cmsModuleName, "GetContentListByCategory");
                adapter.AddApiGatewayCall(caller,
                             new GrpcArticleByCatURI()
                             {
                                 ApplicationId = 2,
                                 CategoryIdList = catIdList,
                                 EndIndex = endIdx,
                                 StartIndex = startIdx
                             });
                caller.Call();
                return adapter.Output;
            }
            catch (Exception e)
            {
                log.Error(e);
            }
            return null;

        }

        public static GrpcCMSContent GetArticleListByCategory(string catIdList, uint startIdx, uint endIdx, int makeid = 0, int modelid = 0)
        {
            try
            {
                IApiGatewayCaller caller = new ApiGatewayCaller();
                GenericApiGatewayAdapter<GrpcCMSContent> adapter = new GenericApiGatewayAdapter<GrpcCMSContent>(_cmsModuleName, "GetContentListByCategory");
                adapter.AddApiGatewayCall(caller,
                new GrpcArticleByCatURI()
                {
                    ApplicationId = 2,
                    CategoryIdList = catIdList,
                    EndIndex = endIdx,
                    MakeId = makeid,
                    ModelId = modelid,
                    StartIndex = startIdx
                });
                caller.Call();
                return adapter.Output;
            }
            catch (Exception e)
            {
                log.Error(e);
            }
            return null;
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
            try
            {
                IApiGatewayCaller caller = new ApiGatewayCaller();
                GenericApiGatewayAdapter<GrpcCMSContent> adapter = new GenericApiGatewayAdapter<GrpcCMSContent>(_cmsModuleName, "GetContentListByCategory");
                adapter.AddApiGatewayCall(caller,
                new GrpcArticleByCatURI()
                {
                    ApplicationId = 2,
                    CategoryIdList = catIdList,
                    EndIndex = endIdx,
                    MakeId = makeid,
                    ModelIds = modelIds,
                    StartIndex = startIdx
                });
                caller.Call();
                return adapter.Output;
            }
            catch (Exception e)
            {
                log.Error(e);
            }
            return null;
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

            try
            {
                IApiGatewayCaller caller = new ApiGatewayCaller();
                GenericApiGatewayAdapter<GrpcCMSContent> adapter = new GenericApiGatewayAdapter<GrpcCMSContent>(_cmsModuleName, "GetContentListByCategory");
                adapter.AddApiGatewayCall(caller, new GrpcArticleByCatURI()
                {
                    ApplicationId = 2,
                    CategoryIdList = catIdList,
                    EndIndex = endIdx,
                    MakeId = makeid,
                    StartIndex = startIdx,
                    BodyStyleIds = bodyStyleId
                });
                caller.Call();
                return adapter.Output;
            }
            catch (Exception e)
            {
                log.Error(e);
                ErrorClass.LogError(e, "Grpc.CMS.GrpcMethods.GrpcCMSContent");
            }

            return null;

        }

        public static GrpcArticleSummaryList MostRecentList(string contenTypes, int totalRecords, int? makeId = 0, int? modelId = 0)
        {

            try
            {

                IApiGatewayCaller caller = new ApiGatewayCaller();
                GenericApiGatewayAdapter<GrpcArticleSummaryList> adapter = new GenericApiGatewayAdapter<GrpcArticleSummaryList>(_cmsModuleName, "GetMostRecentArticles");
                adapter.AddApiGatewayCall(caller, new GrpcArticleRecentURI()
                    {
                        MakeId = makeId == null ? 0 : makeId.Value,
                        ModelId = modelId == null ? 0 : modelId.Value,

                        ApplicationId = 2,
                        ContentTypes = contenTypes,
                        TotalRecords = (uint)totalRecords
                    });
                caller.Call();
                return adapter.Output;
            }
            catch (Exception e)
            {
                log.Error(e);
            }
            return null;

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

            try
            {

                IApiGatewayCaller caller = new ApiGatewayCaller();
                GenericApiGatewayAdapter<GrpcArticleSummaryList> adapter = new GenericApiGatewayAdapter<GrpcArticleSummaryList>(_cmsModuleName, "GetMostRecentArticles");
                adapter.AddApiGatewayCall(caller, new GrpcArticleRecentURI()
                    {
                        MakeId = makeId == null ? 0 : makeId.Value,
                        ApplicationId = 2,
                        ContentTypes = contenTypes,
                        TotalRecords = (uint)totalRecords,
                        ModelIds = String.IsNullOrEmpty(modelIds) ? string.Empty : modelIds,
                    });
                caller.Call();
                return adapter.Output;
            }
            catch (Exception e)
            {
                log.Error(e);
            }
            return null;
        }

        public static GrpcArticleSummaryList MostRecentList(string contenTypes, int totalRecords, string bodyStyleIds, int? makeId = 0, int? modelId = 0)
        {
            try
            {

                IApiGatewayCaller caller = new ApiGatewayCaller();
                GenericApiGatewayAdapter<GrpcArticleSummaryList> adapter = new GenericApiGatewayAdapter<GrpcArticleSummaryList>(_cmsModuleName, "GetMostRecentArticles");
                adapter.AddApiGatewayCall(caller, new GrpcArticleRecentURI()
                    {
                        MakeId = makeId == null ? 0 : makeId.Value,
                        ModelId = modelId == null ? 0 : modelId.Value,

                        ApplicationId = 2,
                        ContentTypes = contenTypes,
                        TotalRecords = (uint)totalRecords,
                        BodyStyleIds = bodyStyleIds
                    });
                caller.Call();
                return adapter.Output;
            }
            catch (Exception e)
            {
                log.Error(e);
            }
            return null;
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

            try
            {

                IApiGatewayCaller caller = new ApiGatewayCaller();
                GenericApiGatewayAdapter<GrpcArticleSummaryList> adapter = new GenericApiGatewayAdapter<GrpcArticleSummaryList>(_cmsModuleName, "GetMostRecentArticles");
                adapter.AddApiGatewayCall(caller, new GrpcArticleRecentURI()
                    {
                        MakeId = makeId == null ? 0 : makeId.Value,
                        //ModelIds = modelIds,
                        ApplicationId = 2,
                        ContentTypes = contenTypes,
                        TotalRecords = (uint)totalRecords,
                        BodyStyleIds = bodyStyleIds
                    });
                caller.Call();
                return adapter.Output;
            }
            catch (Exception e)
            {
                log.Error(e);
            }
            return null;
        }

        public static GrpcModelImageList GetArticlePhotos(ulong basicId)
        {

            try
            {
                IApiGatewayCaller caller = new ApiGatewayCaller();
                GenericApiGatewayAdapter<GrpcModelImageList> adapter = new GenericApiGatewayAdapter<GrpcModelImageList>(_cmsModuleName, "GetArticlePhotos");
                adapter.AddApiGatewayCall(caller, new GrpcArticleContentURI()
                    {
                        BasicId = basicId,
                        ApplicationId = 2
                    });
                caller.Call();
                return adapter.Output;
            }
            catch (Exception e)
            {
                log.Error(e);
            }
            return null;
        }

        public static GrpcModelImageList GetModelPhotosList(uint applicationId, int modelId, string categoryId)
        {
            try
            {

                IApiGatewayCaller caller = new ApiGatewayCaller();
                GenericApiGatewayAdapter<GrpcModelImageList> adapter = new GenericApiGatewayAdapter<GrpcModelImageList>(_cmsModuleName, "GetModelPhotosList");
                adapter.AddApiGatewayCall(caller,
                    new GrpcModelPhotoURI()
                    {
                        ApplicationId = applicationId,
                        ModelId = modelId,
                        CategoryIdList = categoryId
                    });
                caller.Call();
                return adapter.Output;
            }
            catch (Exception e)
            {
                log.Error(e);
            }
            return null;
        }

        /// <summary>
        /// Created by  : Vivek Singh Tomar on 10th Jan 2018
        /// Description : Get list of images for provided models
        /// </summary>
        /// <param name="applicationId"></param>
        /// <param name="modelIds"></param>
        /// <param name="categoryIds"></param>
        /// <param name="requiredImageCount"></param>
        /// <returns></returns>
        public static GrpcModelsImageList GetModelsImages(string modelIds, string categoryIds, int requiredImageCount)
        {
            try
            {

                IApiGatewayCaller caller = new ApiGatewayCaller();
                GenericApiGatewayAdapter<GrpcModelsImageList> adapter = new GenericApiGatewayAdapter<GrpcModelsImageList>(_cmsModuleName, "GetModelsImages");
                adapter.AddApiGatewayCall(caller, new GrpcModelListPhotoURI()
                    {
                        ApplicationId = 2,
                        ModelIds = modelIds,
                        CategoryIds = categoryIds,
                        RequiredImageCount = requiredImageCount
                    });
                caller.Call();
                return adapter.Output;
            }
            catch (Exception e)
            {
                log.Error(e);
            }
            return null;
        }


        public static GrpcArticleDetails GetContentDetails(ulong basicId)
        {
            try
            {

                IApiGatewayCaller caller = new ApiGatewayCaller();
                GenericApiGatewayAdapter<GrpcArticleDetails> adapter = new GenericApiGatewayAdapter<GrpcArticleDetails>(_cmsModuleName, "GetContentDetails");
                adapter.AddApiGatewayCall(caller, new GrpcArticleContentURI()
                    {
                        BasicId = basicId,
                        ApplicationId = 2
                    });
                caller.Call();
                return adapter.Output;
            }
            catch (Exception e)
            {
                log.Error(e);
            }
            return null;
        }

        public static GrpcArticlePageDetails GetContentPages(ulong basicId)
        {
            try
            {

                IApiGatewayCaller caller = new ApiGatewayCaller();
                GenericApiGatewayAdapter<GrpcArticlePageDetails> adapter = new GenericApiGatewayAdapter<GrpcArticlePageDetails>(_cmsModuleName, "GetContentPages");
                adapter.AddApiGatewayCall(caller, new GrpcArticleContentURI()
                    {
                        BasicId = basicId,
                        ApplicationId = 2
                    });
                caller.Call();
                return adapter.Output;
            }
            catch (Exception e)
            {
                log.Error(e);
            }
            return null;
        }

        public static GrpcVideosList GetVideosByModelId(int modelId, uint startId, uint endId)
        {

            try
            {

                IApiGatewayCaller caller = new ApiGatewayCaller();
                GenericApiGatewayAdapter<GrpcVideosList> adapter = new GenericApiGatewayAdapter<GrpcVideosList>(_cmsModuleName, "GetVideosByModelId");
                adapter.AddApiGatewayCall(caller, new GrpcVideosByIdURI()
                    {
                        Id = modelId,
                        ApplicationId = 2,
                        StartIndex = startId,
                        EndIndex = endId
                    });
                caller.Call();
                return adapter.Output;
            }
            catch (Exception e)
            {
                log.Error(e);
            }
            return null;
        }
        public static GrpcVideosList GetVideosByModelId(int modelId, uint startId, uint endId, string bodyStyleId)
        {
            try
            {

                IApiGatewayCaller caller = new ApiGatewayCaller();
                GenericApiGatewayAdapter<GrpcVideosList> adapter = new GenericApiGatewayAdapter<GrpcVideosList>(_cmsModuleName, "GetVideosByModelId");
                adapter.AddApiGatewayCall(caller, new GrpcVideosByIdURI()
                    {
                        Id = modelId,
                        ApplicationId = 2,
                        StartIndex = startId,
                        EndIndex = endId,
                        BodyStyleIds = bodyStyleId
                    });
                caller.Call();
                return adapter.Output;
            }
            catch (Exception e)
            {
                log.Error(e);
            }
            return null;

        }
        public static GrpcVideosList GetVideosByMakeId(int makeId, uint startId, uint endId)
        {
            try
            {

                IApiGatewayCaller caller = new ApiGatewayCaller();
                GenericApiGatewayAdapter<GrpcVideosList> adapter = new GenericApiGatewayAdapter<GrpcVideosList>(_cmsModuleName, "GetVideosByMakeId");
                adapter.AddApiGatewayCall(caller, new GrpcVideosByIdURI()
                    {
                        Id = makeId,
                        ApplicationId = 2,
                        StartIndex = startId,
                        EndIndex = endId
                    });
                caller.Call();
                return adapter.Output;
            }
            catch (Exception e)
            {
                log.Error(e);
            }
            return null;
        }

        public static GrpcVideosList GetVideosByMakeId(int makeId, uint startId, uint endId, string bodyStyleId)
        {
            try
            {

                IApiGatewayCaller caller = new ApiGatewayCaller();
                GenericApiGatewayAdapter<GrpcVideosList> adapter = new GenericApiGatewayAdapter<GrpcVideosList>(_cmsModuleName, "GetVideosByMakeId");
                adapter.AddApiGatewayCall(caller, new GrpcVideosByIdURI()
                    {
                        Id = makeId,
                        ApplicationId = 2,
                        StartIndex = startId,
                        EndIndex = endId,
                        BodyStyleIds = bodyStyleId
                    });
                caller.Call();
                return adapter.Output;
            }
            catch (Exception e)
            {
                log.Error(e);
            }
            return null;
        }

        public static GrpcVideosList GetVideosBySubCategory(uint catId, uint startId, uint endId)
        {
            try
            {

                IApiGatewayCaller caller = new ApiGatewayCaller();
                GenericApiGatewayAdapter<GrpcVideosList> adapter = new GenericApiGatewayAdapter<GrpcVideosList>(_cmsModuleName, "GetVideosBySubCategory");
                adapter.AddApiGatewayCall(caller, new GrpcVideosBySubCategoryURI()
                    {
                        ApplicationId = 2,
                        SubCategoryId = catId,
                        StartIndex = startId,
                        EndIndex = endId
                    });
                caller.Call();
                return adapter.Output;
            }
            catch (Exception e)
            {
                log.Error(e);
            }
            return null;
        }

        public static GrpcVideosList GetVideosBySubCategory(uint catId, uint startId, uint endId, string bodyStyleId)
        {

            try
            {

                IApiGatewayCaller caller = new ApiGatewayCaller();
                GenericApiGatewayAdapter<GrpcVideosList> adapter = new GenericApiGatewayAdapter<GrpcVideosList>(_cmsModuleName, "GetVideosBySubCategory");
                adapter.AddApiGatewayCall(caller, new GrpcVideosBySubCategoryURI()
                    {
                        ApplicationId = 2,
                        SubCategoryId = catId,
                        StartIndex = startId,
                        EndIndex = endId,
                        BodyStyleIds = bodyStyleId
                    });
                caller.Call();
                return adapter.Output;
            }
            catch (Exception e)
            {
                log.Error(e);
            }
            return null;
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

            try
            {

                IApiGatewayCaller caller = new ApiGatewayCaller();
                GenericApiGatewayAdapter<GrpcVideoListEntity> adapter = new GenericApiGatewayAdapter<GrpcVideoListEntity>(_cmsModuleName, "GetVideosBySubCategories");
                adapter.AddApiGatewayCall(caller, new GrpcVideosBySubCategoriesURI()
                    {
                        ApplicationId = 2,
                        SubCategoryIds = catIds,
                        StartIndex = startIndex,
                        EndIndex = endIndex,
                        SortCategory = MapVideosSortOrder(sortOrder)
                    });
                caller.Call();
                return adapter.Output;
            }
            catch (Exception e)
            {
                log.Error(e);
            }
            return null;
        }

        public static GrpcVideosList GetSimilarVideos(int id, int totalCount)
        {

            try
            {

                IApiGatewayCaller caller = new ApiGatewayCaller();
                GenericApiGatewayAdapter<GrpcVideosList> adapter = new GenericApiGatewayAdapter<GrpcVideosList>(_cmsModuleName, "GetSimilarVideos");
                adapter.AddApiGatewayCall(caller, new GrpcVideosByIdURI()
                    {
                        ApplicationId = 2,
                        Id = id,
                        StartIndex = 1,
                        EndIndex = (uint)totalCount
                    });
                caller.Call();
                return adapter.Output;
            }
            catch (Exception e)
            {
                log.Error(e);
            }
            return null;
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
            try
            {

                IApiGatewayCaller caller = new ApiGatewayCaller();
                GenericApiGatewayAdapter<GrpcVideosList> adapter = new GenericApiGatewayAdapter<GrpcVideosList>(_cmsModuleName, "GetSimilarVideos");
                adapter.AddApiGatewayCall(caller, new GrpcVideosByIdURI()
                    {
                        ApplicationId = 2,
                        SimilarModels = modelIdList,
                        StartIndex = 1,
                        EndIndex = totalCount
                    });
                caller.Call();
                return adapter.Output;
            }
            catch (Exception e)
            {
                log.Error(e);
            }
            return null;
        }

        public static GrpcVideo GetVideoByBasicId(int id)
        {

            try
            {

                IApiGatewayCaller caller = new ApiGatewayCaller();
                GenericApiGatewayAdapter<GrpcVideo> adapter = new GenericApiGatewayAdapter<GrpcVideo>(_cmsModuleName, "GetVideoByBasicId");
                adapter.AddApiGatewayCall(caller, new GrpcVideosByIdURI()
                    {
                        ApplicationId = 2,
                        Id = id
                    });
                caller.Call();
                return adapter.Output;
            }
            catch (Exception e)
            {
                log.Error(e);
            }
            return null;
        }

        /// <summary>
        /// Created by : Ashutosh Sharma on 20-Sep-2017
        /// Description :  GRPC method to get author list.
        /// </summary>
        /// <param name="applicationId"></param>
        /// <returns></returns>
        public static GrpcAuthorList GetAuthorsList(int applicationId)
        {
            try
            {
                IApiGatewayCaller caller = new ApiGatewayCaller();
                GenericApiGatewayAdapter<GrpcAuthorList> adapter = new GenericApiGatewayAdapter<GrpcAuthorList>(_cmsModuleName, "GetAuthorsList");
                adapter.AddApiGatewayCall(caller, new GrpcInt()
                {
                    ApplicationId = applicationId
                });
                caller.Call();
                return adapter.Output;
            }
            catch (Exception e)
            {
                log.Error(e);
            }
            return null;
        }



        public static GrpcBool ClearMemCachedKEys(EditCMSCategoryEnum cat, int makeId, int modelId)
        {
            try
            {
                IApiGatewayCaller caller = new ApiGatewayCaller();
                GenericApiGatewayAdapter<GrpcBool> adapter = new GenericApiGatewayAdapter<GrpcBool>(_cmsModuleName, "ClearMemcachedKeys");
                adapter.AddApiGatewayCall(caller, new EditCMSCategory() { Category = cat, MakeId = makeId, ModelId = modelId });
                caller.Call();
                return adapter.Output;
            }
            catch (Exception e)
            {
                log.Error(e);
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

            try
            {
                IApiGatewayCaller caller = new ApiGatewayCaller();
                GenericApiGatewayAdapter<GrpcAuthor> adapter = new GenericApiGatewayAdapter<GrpcAuthor>(_cmsModuleName, "GetAuthorDetails");
                adapter.AddApiGatewayCall(caller, new GrpcInt()
                    {
                        IntOutput = authorId,
                        ApplicationId = 2
                    });
                caller.Call();
                return adapter.Output;
            }
            catch (Exception e)
            {
                log.Error(e);
                ErrorClass.LogError(e, "Grpc.CMS.GrpcMethods.GetAuthorDetails");
            }
            return null;
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

            try
            {
                IApiGatewayCaller caller = new ApiGatewayCaller();
                GenericApiGatewayAdapter<GrpcAuthorContentList> adapter = new GenericApiGatewayAdapter<GrpcAuthorContentList>(_cmsModuleName, "GetContentByAuthor");
                adapter.AddApiGatewayCall(caller, new GrpcContentByAuthorURI()
                    {
                        AuthorId = authorId,
                        ApplicationId = applicationId,
                        Categoryids = categoryList
                    });
                caller.Call();
                return adapter.Output;
            }
            catch (Exception e)
            {
                log.Error(e);
                ErrorClass.LogError(e, "Grpc.CMS.GrpcMethods.GetContentByAuthor");
            }
            return null;
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

            try
            {
                IApiGatewayCaller caller = new ApiGatewayCaller();
                GenericApiGatewayAdapter<GrpcAuthorList> adapter = new GenericApiGatewayAdapter<GrpcAuthorList>(_cmsModuleName, "GetAllOtherAuthors");
                adapter.AddApiGatewayCall(caller, new GrpcContentByAuthorURI()
                    {
                        AuthorId = authorId,
                        ApplicationId = applicationId
                    });
                caller.Call();
                return adapter.Output;
            }
            catch (Exception e)
            {
                log.Error(e);
                ErrorClass.LogError(e, "Grpc.CMS.GrpcMethods.GetContentByAuthor");
            }
            return null;

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

            try
            {

                IApiGatewayCaller caller = new ApiGatewayCaller();

                GenericApiGatewayAdapter<GrpcCMSContent> adapter = new GenericApiGatewayAdapter<GrpcCMSContent>(_cmsModuleName, "GetContentListBySubCategoryId");
                adapter.AddApiGatewayCall(caller, new GrpcArticleBySubCatURI()
                        {
                            ApplicationId = 2,
                            MakeId = makeId,
                            ModelId = modelId,
                            CategoryIdList = categoryIdList,
                            SubCategory = subCategoryIdList,
                            StartIndex = startIndex,
                            EndIndex = endIndex
                        });
                caller.Call();
                return adapter.Output;
            }
            catch (Exception e)
            {
                log.Error(e);
            }
            return null;
        }
    }
}