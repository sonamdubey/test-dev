using ApiGatewayLibrary;
using Carwale.Entity.CMS.URIs;
using Carwale.Entity.Enum;
using Carwale.Notifications;
using Carwale.Utility;
using EditCMSWindowsService.Messages;
using Grpc.Core;
using log4net;
using System;
using System.Collections.Generic;
using Google.Protobuf;
using Carwale.DAL.EditCMS;
using Carwale.DAL.ApiGateway;
using Carwale.Notifications.Logs;

namespace Grpc.CMS
{
    /// <summary>
    /// Author: Prasad Gawde
    /// Summary: This class is responsible to make the Grpc call for a specific function. Also it will retry the call for few times before failing in case one/all the servers are down
    /// </summary>
    public static class GrpcMethods
    {
        static readonly int m_ChanelWaitTime = CWConfiguration.GrpcChannelWaitTime;
        static readonly int m_retryCount = CWConfiguration.GrpcRetryCount;
        static bool _logGrpcCallTimings = CWConfiguration.LogGrpcCallTimings;
        static readonly ILog _logger = LogManager.GetLogger(typeof(GrpcMethods));
        static int _grpcCallTimeLimitCheckValue = CWConfiguration.GrpcCallTimeLimitCheckValue;
        static string editCMSModuleName = CWConfiguration.EditCMSModuleName;
        
        static DateTime GetForwardTime(int incrementMillisecond)
        {
            return DateTime.UtcNow.Add(TimeSpan.FromMilliseconds(incrementMillisecond));
        }

        public static GrpcBool ClearMemCacheKey(string category, int modelId, int makeId, int basicId, string url, int applicationId)
        {
            try
            {
                IMessage message = new EditCMSCategoryV1()
                {
                    Category = ClearCacheCategory(category),
                    MakeId = makeId,
                    ModelId = modelId,
                    BasicId = basicId,
                    Url = url,
                    ApplicationId = applicationId
                };
                return GetData<GrpcBool>("ClearMemCacheKeys", message);
            }
            catch (Exception e)
            {
                ExceptionHandler objErr = new ExceptionHandler(e, "GrpcMethods ClearMemCacheKey Exception");
                objErr.LogException();
                return null;
            }
        }
        public static EditCMSCategoryEnum ClearCacheCategory(string category)
        {
            if (category.Contains("2") || category.Contains("8")) return EditCMSCategoryEnum.ExpertReviews;
            if (category.Contains("6")) return EditCMSCategoryEnum.Features;
            if (category.Contains("13")) return EditCMSCategoryEnum.Videos;
            if (category.Contains("10")) return EditCMSCategoryEnum.Photos;
            return EditCMSCategoryEnum.News;
        }
        public static GrpcCMSContent GetArticleListByCategory(ArticleByCatURI articleByCatURI)
        {
            try
            {
                IMessage message = new GrpcArticleByCatURI()
                {
                    ApplicationId = articleByCatURI.ApplicationId,
                    CategoryIdList = articleByCatURI.CategoryIdList,
                    EndIndex = articleByCatURI.EndIndex,
                    MakeId = articleByCatURI.MakeId,
                    ModelId = articleByCatURI.ModelId,
                    StartIndex = articleByCatURI.StartIndex
                };
                return GetData<GrpcCMSContent>("GetContentListByCategory", message);
            }
            catch (Exception e)
            {
                ExceptionHandler objErr = new ExceptionHandler(e, "GrpcMethods GetArticleListByCategory Exception");
                objErr.LogException();
                return null;
            }
        }

        public static GrpcArticleSummaryList GetRelatedListByCategory(ArticleByCatAndIdURI articleByCatURI)
        {
            try
            {
                IMessage message = new GrpcArticleByCatURI()
                {
                    ApplicationId = articleByCatURI.ApplicationId,
                    BasicId = (int)articleByCatURI.BasicId,
                    CategoryIdList = articleByCatURI.CategoryIdList,
                    EndIndex = articleByCatURI.EndIndex,
                    StartIndex = articleByCatURI.StartIndex
                };
                return GetData<GrpcArticleSummaryList>("GetRelatedListByCategory", message);
            }
            catch (Exception e)
            {
                ExceptionHandler objErr = new ExceptionHandler(e, "GrpcMethods GetRelatedListByCategory Exception");
                objErr.LogException();
                return null;
            }
        }

        public static GrpcModelsImageList GetModelsImages(string modelIds, int requiredImageCount, string categoryIds, int applicationId)
        {
            try
            {
                IMessage message = new GrpcModelListPhotoURI()
                {
                    ModelIds = modelIds,
                    RequiredImageCount = requiredImageCount,
                    CategoryIds = categoryIds,
                    ApplicationId = applicationId
                };
                return GetData<GrpcModelsImageList>("GetModelsImages", message);
            }
            catch (Exception e)
            {
                Logger.LogException(e, "GrpcMethods GetModelsImages Exception");
                return null;
            }
        }

        public static GrpcArticleSummaryList GetFeaturedArticles(ArticleFeatureURI articleByCatURI)
        {
            try
            {
                IMessage message = new GrpcArticleRecentURI()
                            {
                                ApplicationId = articleByCatURI.ApplicationId,
                                ContentTypes = articleByCatURI.ContentTypes,
                                TotalRecords = articleByCatURI.TotalRecords
                            };
                return GetData<GrpcArticleSummaryList>("GetFeaturedArticles", message);
            }
            catch (Exception e)
            {
                ExceptionHandler objErr = new ExceptionHandler(e, "GrpcMethods GetFeaturedArticles Exception");
                objErr.LogException();
                return null;
            }
        }

        public static GrpcMakeAndModelDetailList GetMakeDetails(MakeDetailURI queryString)
        {
            try
            {
                IMessage message = new GrpcMakeDetailURI()
                {
                    ApplicationId = queryString.ApplicationId,
                    CategoryIdList = queryString.CategoryIdList
                };
                return GetData<GrpcMakeAndModelDetailList>("GetMakeDetails", message);
            }
            catch (Exception e)
            {
                ExceptionHandler objErr = new ExceptionHandler(e, "GrpcMethods GetMakeDetails Exception");
                objErr.LogException();
                return null;
            }
        }

        public static GrpcMakeAndModelDetailList GetModelDetails(ModelDetailURI queryString)
        {
            try
            {
                IMessage message = new GrpcModelDetailURI()
                {
                    ApplicationId = queryString.ApplicationId,
                    CategoryIdList = queryString.CategoryIdList,
                    MakeId = (int)queryString.MakeId
                };
                return GetData<GrpcMakeAndModelDetailList>("GetModelDetails", message);
            }
            catch (Exception e)
            {
                ExceptionHandler objErr = new ExceptionHandler(e, "GrpcMethods GetModelDetails Exception");
                objErr.LogException();
                return null;
            }
        }

        public static GrpcInt GetCMSRoadTestCount(int makeId, int modelId, int versionId, int topCount, int applicationId, string category = "8")
        {
            try
            {
                IMessage message = new GrpcRoadTestURI()
                {
                    ApplicationId = applicationId,
                    MakeId = makeId,
                    ModelId = modelId,
                    TopCount = topCount,
                    VersionId = versionId,
                    Category = category
                };
                return GetData<GrpcInt>("GetCMSRoadTestCount", message);
            }
            catch (Exception e)
            {
                ExceptionHandler objErr = new ExceptionHandler(e, "GrpcMethods GetCMSRoadTestCount Exception");
                objErr.LogException();
                return null;
            }
        }

        public static GrpcInt GetTaggedModelId(int basicId)
        {
            try
            {
                IMessage message = new GrpcInt()
                {
                    IntOutput = basicId,
                    ApplicationId = 1
                };
                return GetData<GrpcInt>("GetTaggedModelId", message);
            }
            catch (Exception e)
            {
                ExceptionHandler objErr = new ExceptionHandler(e, "GrpcMethods GetTaggedModelId Exception");
                objErr.LogException();
                return null;
            }
        }
        public static GrpcCMSSubCategoryList GetContentSegment(CommonURI queryString, bool getAllMedia = false)
        {
            try
            {
                IMessage message = new GrpcMakeModelApplicationID
                {
                    ApplicationId = queryString.ApplicationId,
                    MakeId = queryString.MakeId,
                    ModelId = queryString.ModelId,
                    GetAllVideos = getAllMedia
                };
                return GetData<GrpcCMSSubCategoryList>("GetContentSegmentCount", message);
            }
            catch (Exception e)
            {
                ExceptionHandler objErr = new ExceptionHandler(e, "GrpcMethods GetContentSegment Exception");
                objErr.LogException();
                return null;
            }
        }

        public static GrpcInt GetArticleViewsCount(int basicId)
        {
            try
            {
                IMessage message = new GrpcInt()
                {
                    IntOutput = basicId,
                    ApplicationId = 1
                };
                return GetData<GrpcInt>("GetArticleViewsCount", message);
            }
            catch (Exception e)
            {
                ExceptionHandler objErr = new ExceptionHandler(e, "GrpcMethods GetArticleViewsCount Exception");
                objErr.LogException();
                return null;
            }
        }

        public static GrpcCMSSubCategoryList GetCMSSubCategories(int cmsCategoryId)
        {
            try
            {
                IMessage message = new GrpcSmallInt()
                {
                    IntOutput = cmsCategoryId,
                    ApplicationId = 1
                };
                return GetData<GrpcCMSSubCategoryList>("GetCMSSubcategories", message);
            }
            catch (Exception e)
            {
                ExceptionHandler objErr = new ExceptionHandler(e, "GrpcMethods GetCMSSubCategories Exception");
                objErr.LogException();
                return null;
            }
        }

        public static GrpcCMSSubCategoryList GetCMSSubCategoriesByCategory(int cmsCategoryId, int applicationId)
        {
            try
            {
                IMessage message = new GrpcAutoExpoNewsURI()
                {
                    CategoryId = cmsCategoryId,
                    ApplicationId = applicationId
                };
                return GetData<GrpcCMSSubCategoryList>("GetCMSSubCategoriesByCategory", message);
            }
            catch (Exception e)
            {
                ExceptionHandler objErr = new ExceptionHandler(e, "GrpcMethods GetCMSSubCategoriesByCategory Exception");
                objErr.LogException();
                return null;
            }
        }

        public static GrpcCMSContent GetContentListBySubCategory(ArticleBySubCatURI queryString)
        {
            try
            {
                IMessage message = new GrpcArticleBySubCatURI()
                {
                    ApplicationId = queryString.ApplicationId,
                    CategoryIdList = queryString.CategoryIdList,
                    SubCategory = string.IsNullOrEmpty(queryString.SubCategories) ? queryString.SubCategoryId.ToString() : queryString.SubCategories,
                    EndIndex = queryString.EndIndex,
                    MakeId = queryString.MakeId,
                    ModelId = queryString.ModelId,
                    StartIndex = queryString.StartIndex
                };
                return GetData<GrpcCMSContent>("GetContentListBySubCategoryId", message);
            }
            catch (Exception e)
            {
                ExceptionHandler objErr = new ExceptionHandler(e, "GrpcMethods GetContentListBySubCategory Exception");
                objErr.LogException();
                return null;
            }
        }

        public static GrpcArticleSummaryList GetRelatedArticlesByBasicId(ArticleRelatedURI queryString)
        {
            try
            {
                IMessage message = new GrpcRelatedArticlesURI()
                {
                    ApplicationId = queryString.ApplicationId,
                    ContentTypes = queryString.ContentTypes,
                    BasicId = queryString.BasicId,
                    TotalRecords = queryString.TotalRecords
                };
                return GetData<GrpcArticleSummaryList>("GetRelatedArticlesByBasicId", message);
            }
            catch (Exception e)
            {
                ExceptionHandler objErr = new ExceptionHandler(e, "GrpcMethods GetRelatedArticlesByBasicId Exception");
                objErr.LogException();
                return null;
            }
        }

        public static GrpcRelatedArticlesList GetRelatedContent(int basicId, int count)
        {
            try
            {
                IMessage message = new GetRelatedContentURI()
                {
                    BasicId = basicId,
                    RecordCount = count,
                    ApplicationId = 1
                };
                return GetData<GrpcRelatedArticlesList>("GetRelatedContent", message);
            }
            catch (Exception e)
            {
                ExceptionHandler objErr = new ExceptionHandler(e, "GrpcMethods GetRelatedContent Exception");
                objErr.LogException();
                return null;
            }
        }
        public static GrpcArticleSummaryList MostRecentList(ArticleRecentURI queryString)
        {
            try
            {
                IMessage message = new GrpcArticleRecentURI()
                {
                    MakeId = queryString.MakeId,
                    ModelId = queryString.ModelId,

                    ApplicationId = queryString.ApplicationId,
                    ContentTypes = queryString.ContentTypes,
                    TotalRecords = (uint)queryString.TotalRecords
                };
                return GetData<GrpcArticleSummaryList>("GetMostRecentArticles", message);
            }
            catch (Exception e)
            {
                ExceptionHandler objErr = new ExceptionHandler(e, "GrpcMethods MostRecentList Exception");
                objErr.LogException();
                return null;
            }
        }

        public static GrpcContentFeedSummaryList GetNewsFeedBySlug(ContentFeedURI queryString)
        {
            try
            {
                IMessage message = new GrpcContentFeedURI()
                {
                    ApplicationId = queryString.ApplicationId,
                    EndIndex = queryString.EndIndex,
                    Slug = queryString.Slug,
                    StartIndex = queryString.StartIndex
                };

                return GetData<GrpcContentFeedSummaryList>("GetNewsFeedBySlug", message);
            }
            catch (Exception e)
            {
                ExceptionHandler objErr = new ExceptionHandler(e, "GrpcMethods GetNewsFeedBySlug Exception");
                objErr.LogException();
                return null;
            }
        }

        public static GrpcContentFeedSummaryList GetNewsFeedBySubCategory(ContentFeedURI queryString)
        {
            try
            {
                IMessage message = new GrpcContentFeedURI()
                {
                    ApplicationId = queryString.ApplicationId,
                    EndIndex = queryString.EndIndex,
                    SubCategoryId = queryString.SubCategoryId,
                    StartIndex = queryString.StartIndex
                };

                return GetData<GrpcContentFeedSummaryList>("GetNewsFeedBySubCategory", message);
            }
            catch (Exception e)
            {
                ExceptionHandler objErr = new ExceptionHandler(e, "GrpcMethods GetNewsFeedBySubCategory Exception");
                objErr.LogException();
                return null;
            }
        }

        public static GrpcContentFeedSummaryList GetAllNewsFeed(ContentFeedURI queryString)
        {
            try
            {
                IMessage message = new GrpcContentFeedURI()
                {
                    ApplicationId = queryString.ApplicationId,
                    EndIndex = queryString.EndIndex,
                    StartIndex = queryString.StartIndex
                };

                return GetData<GrpcContentFeedSummaryList>("GetAllNewsFeed", message);
            }
            catch (Exception e)
            {
                ExceptionHandler objErr = new ExceptionHandler(e, "GrpcMethods GetAllNewsFeed Exception");
                objErr.LogException();
                return null;
            }
        }

        public static GrpcCarSynopsis GetCarSynopsis(int modelId, int applicationId, int priority)
        {
            try
            {
                IMessage message = new GrpcCarSynopsisURI()
                {
                    ApplicationId = applicationId,
                    ModelId = modelId,
                    Priority = priority
                };

                return GetData<GrpcCarSynopsis>("GetCarSynopsis", message);
            }
            catch (Exception e)
            {
                ExceptionHandler objErr = new ExceptionHandler(e, "GrpcMethods GetCarSynopsis Exception");
                objErr.LogException();
                return null;
            }
        }

        public static GrpcPopularModelPhotosData GetPopularModelPhotos(ArticleByCatURI queryString)
        {
            try
            {
                IMessage message = new GrpcMakeModelApplicationID()
                {
                    MakeId = queryString.MakeId,
                    ModelId = queryString.ModelId,
                    ApplicationId = queryString.ApplicationId
                };

                return GetData<GrpcPopularModelPhotosData>("GetPopularModelPhotosCount", message);
            }
            catch (Exception e)
            {
                ExceptionHandler objErr = new ExceptionHandler(e, "GrpcMethods GetCarSynopsis Exception");
                objErr.LogException();
                return null;
            }
        }

        public static GrpcPopularModelVideoData GetPopularModelVideos(ArticleByCatURI queryString)
        {
            try
            {
                IMessage message = new GrpcMakeModelApplicationID
                {
                    MakeId = queryString.MakeId,
                    ModelId = queryString.ModelId,
                    ApplicationId = queryString.ApplicationId,
                    GetAllVideos = queryString.GetAllMedia
                };

                return GetData<GrpcPopularModelVideoData>("GetPopularModelVideoCount", message);
            }
            catch (Exception e)
            {
                ExceptionHandler objErr = new ExceptionHandler(e, "GrpcMethods GetPopularModelVideos Exception");
                objErr.LogException();
                return null;
            }
        }
        public static GrpcModelImageList GetArticlePhotos(ArticlePhotoUri queryString)
        {
            try
            {
                IMessage message = new GrpcArticleContentURI()
                {
                    BasicId = queryString.basicId
                };

                return GetData<GrpcModelImageList>("GetArticlePhotos", message);
            }
            catch (Exception e)
            {
                ExceptionHandler objErr = new ExceptionHandler(e, "GrpcMethods GetArticlePhotos Exception");
                objErr.LogException();
                return null;
            }
        }

        public static GrpcModelImageList GetModelPhotosList(ModelPhotoURI queryString)
        {
            try
            {
                IMessage message = new GrpcModelPhotoURI()
                {
                    ApplicationId = queryString.ApplicationId,
                    ModelId = queryString.ModelId,
                    CategoryIdList = queryString.CategoryIdList
                };

                return GetData<GrpcModelImageList>("GetModelPhotosList", message);
            }
            catch (Exception e)
            {
                ExceptionHandler objErr = new ExceptionHandler(e, "GrpcMethods GetModelPhotosList Exception");
                objErr.LogException();
                return null;
            }
        }

        public static GrpcArticleDetails GetContentDetails(ArticleContentURI queryString)
        {
            try
            {
                IMessage message = new GrpcArticleContentURI()
                {
                    BasicId = queryString.BasicId,
                    ApplicationId = queryString.ApplicationId
                };

                return GetData<GrpcArticleDetails>("GetContentDetails", message);
            }
            catch (Exception e)
            {
                ExceptionHandler objErr = new ExceptionHandler(e, "GrpcMethods GetContentDetails Exception");
                objErr.LogException();
                return null;
            }
        }

        public static GrpcArticlePageDetails GetContentPages(ArticleContentURI content)
        {
            try
            {
                IMessage message = new GrpcArticleContentURI()
                {
                    BasicId = content.BasicId,
                    ApplicationId = content.ApplicationId
                };

                return GetData<GrpcArticlePageDetails>("GetContentPages", message);
            }
            catch (Exception e)
            {
                ExceptionHandler objErr = new ExceptionHandler(e, "GrpcMethods GetContentPages Exception");
                objErr.LogException();
                return null;
            }
        }

        public static GrpcVideosEntityList GetRelatedModelVideosByBasicId(uint basicId, uint applicationId)
        {
            try
            {
                IMessage message = new GrpcArticleContentURI()
                {
                    ApplicationId = applicationId,
                    BasicId = basicId
                };

                return GetData<GrpcVideosEntityList>("GetRelatedModelVideosByBasicId", message);
            }
            catch (Exception e)
            {
                ExceptionHandler objErr = new ExceptionHandler(e, "GrpcMethods GetRelatedModelVideosByBasicId Exception");
                objErr.LogException();
                return null;
            }
        }

        public static GrpcRelatedArticlesList GetRelatedVideoContent(int basicId)
        {
            try
            {
                IMessage message = new GrpcInt()
                {
                    IntOutput = basicId,
                    ApplicationId = 1
                };

                return GetData<GrpcRelatedArticlesList>("GetRelatedVideoContent", message);
            }
            catch (Exception e)
            {
                ExceptionHandler objErr = new ExceptionHandler(e, "GrpcMethods GetRelatedVideoContent Exception");
                objErr.LogException();
                return null;
            }
        }

        public static GrpcRelatedArticlesList GetTopArticlesByBasicId(int basicId)
        {
            try
            {
                IMessage message = new GrpcInt()
                {
                    IntOutput = basicId,
                    ApplicationId = 1
                };

                return GetData<GrpcRelatedArticlesList>("GetTopArticlesByBasicId", message);
            }
            catch (Exception e)
            {
                ExceptionHandler objErr = new ExceptionHandler(e, "GrpcMethods GetTopArticlesByBasicId Exception");
                objErr.LogException();
                return null;
            }
        }

        public static GrpcAuthorList GetAuthorsList(int applicationId)
        {
            try
            {
                IMessage message = new GrpcInt()
                {
                    IntOutput = applicationId
                };

                return GetData<GrpcAuthorList>("GetAuthorsList", message);
            }
            catch (Exception e)
            {
                ExceptionHandler objErr = new ExceptionHandler(e, "GrpcMethods GetAuthorsList Exception");
                objErr.LogException();
                return null;
            }
        }

        public static GrpcAuthor GetAuthorDetails(int authorId)
        {
            try
            {
                IMessage message = new GrpcInt()
                {
                    IntOutput = authorId,
                    ApplicationId = 1
                };

                return GetData<GrpcAuthor>("GetAuthorDetails", message);
            }
            catch (Exception e)
            {
                ExceptionHandler objErr = new ExceptionHandler(e, "GrpcMethods GetAuthorDetails Exception");
                objErr.LogException();
                return null;
            }
        }

        public static GrpcAuthorList GetAllOtherAuthors(int authorId, int applicationId)
        {
            try
            {
                IMessage message = new GrpcContentByAuthorURI()
                {
                    AuthorId = authorId,
                    ApplicationId = applicationId
                };

                return GetData<GrpcAuthorList>("GetAllOtherAuthors", message);
            }
            catch (Exception e)
            {
                ExceptionHandler objErr = new ExceptionHandler(e, "GrpcMethods GetAllOtherAuthors Exception");
                objErr.LogException();
                return null;
            }
        }

        public static GrpcAuthorContentList GetContentByAuthor(int authorId, int applicationId, string categoryList)
        {
            try
            {
                IMessage message = new GrpcContentByAuthorURI()
                {
                    AuthorId = authorId,
                    ApplicationId = applicationId,
                    Categoryids = categoryList
                };

                return GetData<GrpcAuthorContentList>("GetContentByAuthor", message);
            }
            catch (Exception e)
            {
                ExceptionHandler objErr = new ExceptionHandler(e, "GrpcMethods GetContentByAuthor Exception");
                objErr.LogException();
                return null;
            }
        }

        public static GrpcVideosList GetVideosByModelId(int modelId, uint applicationId, uint startId, uint endId)
        {
            try
            {
                IMessage message = new GrpcVideosByIdURI()
                {
                    Id = modelId,
                    ApplicationId = applicationId,
                    StartIndex = startId,
                    EndIndex = endId
                };
                return GetData<GrpcVideosList>("GetVideosByModelId", message);
            }
            catch(Exception e)
            {
                ExceptionHandler objErr = new ExceptionHandler(e, "GrpcMethods GetVideosByModelId Exception");
                objErr.LogException();
                return null;
            }
            
        }

        public static GrpcVideosList GetVideosByMakeId(int makeId, uint applicationId, uint startId, uint endId)
        {
            try
            {
                IMessage message = new GrpcVideosByIdURI()
                {
                    Id = makeId,
                    ApplicationId = applicationId,
                    StartIndex = startId,
                    EndIndex = endId
                };
                return GetData<GrpcVideosList>("GetVideosByMakeId", message);
            }
            catch (Exception e)
            {
                ExceptionHandler objErr = new ExceptionHandler(e, "GrpcMethods GetVideosByMakeId Exception");
                objErr.LogException();
                return null;
            }
        }

        public static GrpcVideosList GetVideosBySubCategory(uint catId, uint applicationId, uint startId, uint endId)
        {
            try
            {
                IMessage message = new GrpcVideosBySubCategoryURI()
                {
                    ApplicationId = applicationId,
                    SubCategoryId = catId,
                    StartIndex = startId,
                    EndIndex = endId
                };
                return GetData<GrpcVideosList>("GetVideosBySubCategory", message);
            }
            catch (Exception e)
            {
                ExceptionHandler objErr = new ExceptionHandler(e, "GrpcMethods GetVideosBySubCategory Exception");
                objErr.LogException();
                return null;
            }
        }

        private static GrpcVideoSortOrderCategory MapVideosSortOrder(VideoSortOrderCategory sortOrder)
        {
            switch (sortOrder)
            {
                case VideoSortOrderCategory.FeaturedAndLatest:
                    return GrpcVideoSortOrderCategory.FeaturedAndLatest;
                case VideoSortOrderCategory.MostPopular:
                    return GrpcVideoSortOrderCategory.MostPopular;
                case VideoSortOrderCategory.JustLatest:
                    return GrpcVideoSortOrderCategory.JustLatest;
                default:
                    return GrpcVideoSortOrderCategory.MostPopular;
            }
        }

        public static GrpcVideoListEntity GetVideosBySubCategories(string catIds, uint applicationId, uint startIndex, uint endIndex, VideoSortOrderCategory sortOrder)
        {
            try
            {
                IMessage message = new GrpcVideosBySubCategoriesURI()
                {
                    ApplicationId = applicationId,
                    SubCategoryIds = catIds,
                    StartIndex = startIndex,
                    EndIndex = endIndex,
                    SortCategory = MapVideosSortOrder(sortOrder)
                };
                return GetData<GrpcVideoListEntity>("GetVideosBySubCategories", message);
            }
            catch (Exception e)
            {
                ExceptionHandler objErr = new ExceptionHandler(e, "GrpcMethods GetVideosBySubCategories Exception");
                objErr.LogException();
                return null;
            }
        }

        public static GrpcVideosList GetSimilarVideos(string similarmodels, int id, int applicationId, int totalCount)
        {
            try
            {
                IMessage message = new GrpcVideosByIdURI()
                {
                    ApplicationId = (uint)applicationId,
                    Id = id,
                    StartIndex = 1,
                    EndIndex = (uint)totalCount,
                    SimilarModels = similarmodels
                };
                return GetData<GrpcVideosList>("GetSimilarVideos", message);
            }
            catch (Exception e)
            {
                ExceptionHandler objErr = new ExceptionHandler(e, "GrpcMethods GetSimilarVideos Exception");
                objErr.LogException();
                return null;
            }
        }

        public static GrpcVideo GetVideoByBasicId(int id, int applicationId)
        {
            try
            {
                IMessage message = new GrpcVideosByIdURI()
                {
                    ApplicationId = (uint)applicationId,
                    Id = id
                };
                return GetData<GrpcVideo>("GetVideoByBasicId", message);
            }
            catch (Exception e)
            {
                ExceptionHandler objErr = new ExceptionHandler(e, "GrpcMethods GetVideoByBasicId Exception");
                objErr.LogException();
                return null;
            }
        }

        public static GrpcCMSImage GetOtherModelPhotosList(RelatedPhotoURI otherModelsPhotos)
        {
            try
            {
                IMessage message = new GrpcRelatedPhotoURI()
                {
                    ApplicationId = otherModelsPhotos.ApplicationId,
                    CategoryIdList = otherModelsPhotos.CategoryIdList,
                    EndIndex = otherModelsPhotos.EndIndex,
                    ModelId = otherModelsPhotos.ModelId,
                    StartIndex = otherModelsPhotos.StartIndex
                };
                return GetData<GrpcCMSImage>("GetOtherModelPhotosList", message);
            }
            catch (Exception e)
            {
                ExceptionHandler objErr = new ExceptionHandler(e, "GrpcMethods GetOtherModelPhotosList Exception");
                objErr.LogException();
                return null;
            }
        }

        public static GrpcCMSImage GetSimilarModelPhotosList(RelatedPhotoURI otherModelsPhotos)
        {
            try
            {
                IMessage message = new GrpcRelatedPhotoURI()
                {
                    ApplicationId = otherModelsPhotos.ApplicationId,
                    CategoryIdList = otherModelsPhotos.CategoryIdList,
                    EndIndex = otherModelsPhotos.EndIndex,
                    ModelId = otherModelsPhotos.ModelId,
                    StartIndex = otherModelsPhotos.StartIndex,
                    SimilarModelsList = otherModelsPhotos.SimilarModelsList
                };
                return GetData<GrpcCMSImage>("GetSimilarModelPhotosList", message);
            }
            catch (Exception e)
            {
                ExceptionHandler objErr = new ExceptionHandler(e, "GrpcMethods GetSimilarModelPhotosList Exception");
                objErr.LogException();
                return null;
            }
        }
        public static GrpcBool UpdateVideo(int basicId, int likes, int views, string videoUrl, int duration, string videoId)
        {
            try
            {
                IMessage message = new GrpcVideo()
                {
                    BasicId = basicId,
                    Likes = likes,
                    Views = views,
                    VideoUrl = videoUrl,
                    Duration = duration
                };
                return GetData<GrpcBool>("UpdateVideo", message);
            }
            catch (Exception e)
            {
                ExceptionHandler objErr = new ExceptionHandler(e, "GrpcMethods UpdateVideo Exception");
                objErr.LogException();
                return null;
            }
        }

        public static GrpcArticleSummaryList GoogleSiteMapDetails(int applicationId)
        {
            try
            {
                IMessage message = new GrpcArticleContentURI()
                {
                    ApplicationId = Convert.ToUInt16(applicationId)
                };
                return GetData<GrpcArticleSummaryList>("GoogleSiteMapDetails", message);
            }
            catch (Exception e)
            {
                ExceptionHandler objErr = new ExceptionHandler(e, "GrpcMethods GoogleSiteMapDetails Exception");
                objErr.LogException();
                return null;
            }
        }

        public static GrpcArticleSummary GetSponsoredArticle(string categoryList, string authorId)
        {
            try
            {
                IMessage message = new GetSponsoredArticleURI()
                {
                    CategoryList = categoryList,
                    Author = authorId
                };
                return GetData<GrpcArticleSummary>("GetSponsoredArticle", message);
            }
            catch (Exception e)
            {
                ExceptionHandler objErr = new ExceptionHandler(e, "GrpcMethods GetSponsoredArticle Exception");
                objErr.LogException();
                return null;
            }
        }

        public static GrpcArticleSummaryList GetSponsoredArticlesByCategory(string categoryList, int applicationId)
        {
            try
            {
                IMessage message = new GetSponsoredArticleURI()
                {
                    CategoryList = categoryList,
                    ApplicationId = applicationId
                };
                return GetData<GrpcArticleSummaryList>("GetSponsoredArticlesByCategory", message);
            }
            catch (Exception e)
            {
                ExceptionHandler objErr = new ExceptionHandler(e, "GrpcMethods GetSponsoredArticlesByCategory Exception");
                objErr.LogException();
                return null;
            }
        }

        private static Channel RpcExceptionHandler(int retryId, RpcException e, Channel ch, string errorMessage)
        {
            if (retryId > 0 && (e.Status.StatusCode == StatusCode.Unavailable || e.Status.StatusCode == StatusCode.Cancelled || (e.Status.StatusCode == StatusCode.DeadlineExceeded && ch.State == ChannelState.TransientFailure)))
            {
                if (_logGrpcCallTimings)
                    _logger.Error(GrpcTimingLogEnum.T104.ToString() + " RPC Exception on " + ch.ResolvedTarget + "Retrying Now");
                return EditCMSChannel.Channel;
            }
            else
            {
                ExceptionHandler objErr = new ExceptionHandler(e, errorMessage);
                objErr.LogException();
                return null;
            }
        }

        /// <summary>
        /// key- function call name
        /// value- parameters to be passed to function
        /// </summary>
        /// <param name="calls"></param>
        /// <returns></returns>
        public static GatewayWebservice.OutputRequest GetDataFromGateway(KeyValuePair<string, IMessage>[] calls)
        {
            CallAggregator ca = new CallAggregator();
            foreach (var call in calls)
            {
                ca.AddCall(editCMSModuleName, call.Key, call.Value);
            }
            return ca.GetResultsFromGateway();
        }

        public static GrpcVideosList GetArticleVideos(int basicId)
        {
            try
            {
                IMessage message = new GrpcInt
                {
                    IntOutput = basicId
                };
                return GetData<GrpcVideosList>("GetArticleVideos", message);
            }
            catch (Exception e)
            {
                ExceptionHandler objErr = new ExceptionHandler(e, "GrpcMethods GetArticleVideos Exception");
                objErr.LogException();
                return null;
            }
        }

        public static GrpcAutoExpoGalleryListing GetAutoExpoGalleryListing(int applicationId)
        {
            try
            {
                IMessage message = new GrpcInt()
                {
                    ApplicationId = applicationId
                };
                return GetData<GrpcAutoExpoGalleryListing>("GetAutoExpoGalleryListing", message);
            }
            catch (Exception e)
            {
                ExceptionHandler objErr = new ExceptionHandler(e, "GrpcMethods GetAutoExpoGalleryListing Exception");
                objErr.LogException();
                return null;
            }
        }

        public static GrpcAutoExpoGalleryDetails GetAutoExpoGalleryDetails(int applicationId, int subCategoryId)
        {
            try
            {
                IMessage message = new GrpcAutoExpoGalleryURI()
                {
                    ApplicationId = applicationId,
                    SubCategoryId = subCategoryId
                };
                return GetData<GrpcAutoExpoGalleryDetails>("GetAutoExpoGalleryDetails", message);
            }
            catch (Exception e)
            {
                ExceptionHandler objErr = new ExceptionHandler(e, "GrpcMethods GrpcAutoExpoGalleryDetails Exception");
                objErr.LogException();
                return null;
            }
        }

        public static GrpcInt GetBasicIdFromArticleUrl(string maskingName)
        {
            try
            {
                IMessage message = new GrpcString
                {
                    StringOutput = maskingName
                };
                return GetData<GrpcInt>("GetBasicIdFromArticleUrl", message);
            }
            catch (Exception e)
            {
                ExceptionHandler objErr = new ExceptionHandler(e, "GrpcMethods GetBasicIdFromArticleUrl Exception");
                objErr.LogException();
                return null;
            }
        }

        public static GrpcString GetArticleUrlFromBasicId(ulong basicId)
        {
            try
            {
                IMessage message = new GrpcArticleContentURI
                {
                    ApplicationId = (uint)Application.CarWale,
                    BasicId = basicId
                };
                return GetData<GrpcString>("GetArticleUrlFromBasicId", message);
            }
            catch (Exception e)
            {
                ExceptionHandler objErr = new ExceptionHandler(e, "GrpcMethods GetArticleUrlFromBasicId Exception");
                objErr.LogException();
                return null;
            }
        }

        public static T GetData<T>(string methodName,IMessage message) where T : IMessage
        {
            IApiGatewayCaller apiGatewayCaller = new ApiGatewayCaller();

            apiGatewayCaller.Add(CWConfiguration.CMSModule, methodName, message);
            apiGatewayCaller.Call();
            return ExecutionTimeLogger.LogExecutionTime(() =>  apiGatewayCaller.GetResponse<T>(0));
        }
    }
}
