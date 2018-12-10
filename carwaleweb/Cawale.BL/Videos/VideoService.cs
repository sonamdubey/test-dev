using Carwale.BL.GrpcFiles;
using Carwale.Entity;
using Carwale.Entity.CarData;
using Carwale.Entity.CMS;
using Carwale.Entity.CMS.Articles;
using Carwale.Entity.CMS.Media;
using Carwale.Entity.CMS.URIs;
using Carwale.Entity.Enum;
using Carwale.Interfaces.CarData;
using Carwale.Interfaces.CMS;
using Carwale.Notifications;
using Carwale.Utility;
using EditCMSWindowsService.Messages;
using Grpc.CMS;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Carwale.BL.Videos
{
    public class VideosBL : IVideosBL
    {
        private readonly ICarRecommendationLogic _carRecommendationLogic;

        public VideosBL(ICarRecommendationLogic carRecommendationLogic)
        {
            _carRecommendationLogic = carRecommendationLogic;
        }

        /// <summary>
        /// Created By:Prashant Vishe On 5 Dec 2013
        /// Function is used to update views and likes
        /// </summary>
        /// <param name="basicId">videoId</param>
        /// <param name="likesCount">number of likes</param>
        /// <param name="viewsCount">number of views</param>
        /// <returns>true or false</returns>
        public bool UpdateViewsAndLikes(int basicId, int likesCount, int viewsCount)
        {
            return GrpcToCarwaleConvert.ConvertFromGrpcToCarwale(GrpcMethods.UpdateVideo(basicId, likesCount, viewsCount, string.Empty, 0, string.Empty));
        }

        /// <summary>
        /// Returns Top n number of videos similar to video with specified basicid
        /// </summary>
        /// <param name="basicId"></param>
        /// <param name="applicationId"></param>
        /// <param name="topCount"></param>
        /// <returns></returns>
        public List<Video> GetSimilarVideos(int basicId, CMSAppId applicationId, int topCount)
        {
            try
            {
                string similarmodels = string.Empty;
                var result = GrpcMethods.GetTaggedModelId(basicId);
                if (result != null && result.IntOutput > 0)
                {
                    var similarcarids = GetSimilarCarIds(result.IntOutput);
                    similarmodels = similarcarids.IsNotNullOrEmpty() ? String.Join(",", similarcarids) : string.Empty;
                }
                if (string.IsNullOrEmpty(similarmodels)) return new List<Video>();

                List<Video> cacheObject = GrpcToCarwaleConvert.ConvertFromGrpcToCarwale(GrpcMethods.GetSimilarVideos(similarmodels, basicId, Convert.ToInt32(applicationId), topCount));
                topCount = topCount > cacheObject.Count ? cacheObject.Count : topCount;
                return cacheObject.GetRange(0, topCount);
            }
            catch (Exception err)
            {
                ExceptionHandler objErr = new ExceptionHandler(err, "VideoService GetSimilarVideos Exception");
                objErr.LogException();
                return null;
            }
        }
        private List<int> GetSimilarCarIds (int modelId,int count = 5)
        {
            SimilarCarRequest request = new SimilarCarRequest
            {
                CarId = modelId,
                IsVersion = false,
                Count = count,
                UserIdentifier = string.Empty,
                IsBoost = false
            };
            return _carRecommendationLogic.GetSimilarCars(request);
        }
        public List<Video> GetSimilarVideos(ArticleByCatURI queryString)
        {
            try
            {
                var similarModelsIds = GetSimilarCarIds(queryString.ModelId,10);

                if (similarModelsIds == null || similarModelsIds.Count == 0)
                    return new List<Video>();

                List<Video> modelVideos = GetVideosByModelId(queryString.ModelId, (CMSAppId)queryString.ApplicationId);
                List<Video> similarVideos = new List<Video>();

                if (modelVideos != null && modelVideos.Count > 0)
                {
                    Dictionary<string, List<Video>> similarVideosDictionary = new Dictionary<string, List<Video>>();
                    foreach (var modelId in similarModelsIds)
                    {
                        if (!similarVideosDictionary.ContainsKey(modelId.ToString()))
                        {
                            List<Video> similarModelVideos = GetVideosByModelId(modelId, (CMSAppId)queryString.ApplicationId, 1, 5);

                            similarModelVideos.RemoveAll(similarVideo => modelVideos.Any(modelVideo => modelVideo.BasicId == similarVideo.BasicId));

                            similarVideosDictionary.Add(modelId.ToString(), similarModelVideos);
                        }
                    }

                    similarVideosDictionary = similarVideosDictionary.Where((pair) => pair.Value.Count > 0).ToDictionary(pair => pair.Key, pair => pair.Value);

                    if (similarVideosDictionary.Count == 0)
                        return new List<Video>();

                    List<List<Video>> videosList = similarVideosDictionary.Values.ToList();

                    int videoIndex = 0;
                    int totalCount = videosList.Count;
                    int maxVideoCount = 0;
                    int modelIndex = 0;
                    while (modelIndex < totalCount)
                    {
                        if (similarVideos.Count == 5)
                            break;

                        if (videoIndex < videosList[modelIndex].Count && !similarVideos.Exists(video => video.BasicId == videosList[modelIndex][videoIndex].BasicId))
                            similarVideos.Add(videosList[modelIndex][videoIndex]);

                        if (maxVideoCount < videosList[modelIndex].Count)
                            maxVideoCount = videosList[modelIndex].Count;

                        if (modelIndex == totalCount - 1 && similarVideos.Count != 5 && videoIndex < maxVideoCount)
                        {
                            modelIndex = -1;
                            videoIndex++;
                        }
                        modelIndex++;
                    }
                }

                return similarVideos;
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "Carwale.BL.Videos.GetSimilarVideos");
                objErr.LogException();
                return null;
            }
        }

        /// <summary>
        /// Extracts range of Videos of specific subcategory from entire videos object in memcache((only for new cars)
        /// specify endIndex = -1 for all videos from startindex to end
        /// </summary>
        /// <param name="subCategoryId"></param>
        /// <param name="applicationId">car/bike wale</param>
        /// <param name="startIndex">starts from 1</param>
        /// <param name="endIndex">-1 for no limit on end index otherwise always >= startIndex </param>
        public List<Video> GetNewModelsVideosBySubCategory(EnumVideoCategory subCategoryId, CMSAppId applicationId, int startIndex, int endIndex)
        {
            try
            {
                var videos = GrpcMethods.GetNewModelsVideosBySubCategory(Convert.ToUInt32(subCategoryId), Convert.ToUInt32(applicationId), CustomParser.parseUIntObject(startIndex), CustomParser.parseUIntObject(endIndex));
                return GetVideosBySubCategory(GrpcToCarwaleConvert.ConvertFromGrpcToCarwale(videos), startIndex, endIndex);
            }
            catch (Exception err)
            {
                ExceptionHandler objErr = new ExceptionHandler(err, "VideoService GetNewModelsVideosBySubCategory Exception");
                objErr.LogException();
                return null;
            }
        }

        /// <summary>
        ///  Extracts new models videos by makeId(videos for new cars)
        /// </summary>
        /// <param name="makeId"></param>
        /// <param name="applicationId">car/bike wale</param>
        /// <param name="startIndex">starts from 1</param>
        /// <param name="endIndex">-1 for no limit on end index otherwise always >= startIndex </param>
        public List<Video> GetNewModelsVideosByMakeId(int makeId, CMSAppId applicationId, int startIndex, int endIndex)
        {
            try
            {
                return GrpcToCarwaleConvert.ConvertFromGrpcToCarwale(GrpcMethods.GetNewModelsVideosByMakeId(makeId, (uint)applicationId, (uint)startIndex, (uint)endIndex));
            }
            catch (Exception err)
            {
                ExceptionHandler objErr = new ExceptionHandler(err, "VideoService GetNewModelsVideosByMakeId Exception");
                objErr.LogException();
                return null;
            }
        }

        /// <summary>
        /// Not yet consumed anywhere
        /// </summary>
        /// <param name="modelId"></param>
        /// <param name="applicationId">car/bike wale</param>
        /// <param name="startIndex">starts from 1</param>
        /// <param name="endIndex">-1 for no limit on end index otherwise always >= startIndex</param>
        public List<Video> GetVideosByModelId(int modelId, CMSAppId applicationId, int startIndex = 1, int endIndex = -1)
        {
            try
            {
                return GrpcToCarwaleConvert.ConvertFromGrpcToCarwale(GrpcMethods.GetVideosByModelId(modelId, (uint)applicationId, (startIndex > 0 ? (uint)startIndex : 1), (endIndex > 0 ? (uint)endIndex : 1000)));
            }
            catch (Exception err)
            {
                ExceptionHandler objErr = new ExceptionHandler(err, "VideoService GetVideosByModelId Exception");
                objErr.LogException();
                return null;
            }
        }

        /// <summary>
        /// Not yet consumed anywhere
        /// </summary>
        /// <param name="basicId"></param>
        /// <param name="applicationId"></param>
        /// <returns></returns>
        public Video GetVideoByBasicId(int basicId, CMSAppId applicationId)
        {
            try
            {
                Video _video = GrpcToCarwaleConvert.ConvertFromGrpcToCarwale(GrpcMethods.GetVideoByBasicId(basicId, Convert.ToInt32(applicationId)));
                return _video;
            }
            catch (Exception err)
            {
                ExceptionHandler objErr = new ExceptionHandler(err, "VideoService GetVideoByBasicId Exception");
                objErr.LogException();
                return null;
            }
        }

        /// <summary>
        /// Created by      :   Sumit Kate on 19 Feb 2016
        /// Returns the Videos List by Sub Categories
        /// </summary>
        /// <param name="subCategoryIds">CSV of VideoCategory Ids
        /// For Ids Please refer Carwale.Entity.EnumVideoCategory</param>
        /// <param name="applicationId">application Id</param>
        /// <param name="startIndex">start index</param>
        /// <param name="endIndex">end index</param>
        /// <param name="sortCriteria">Please refer VideoSortOrderCategory</param>
        /// <returns></returns>
        public VideoListEntity GetVideosBySubCategories(string subCategoryIds, CMSAppId applicationId, ushort pageNo, ushort pageSize, string sortCategory)
        {
            var output = new VideoListEntity();
            try
            {
                VideoSortOrderCategory enumSortCategoryId = VideoSortOrderCategory.FeaturedAndLatest;
                enumSortCategoryId = Enum.TryParse(sortCategory, out enumSortCategoryId) ? enumSortCategoryId : VideoSortOrderCategory.MostPopular;

                ushort startIndex, endIndex;
                Carwale.Utility.Calculation.GetStartEndIndex(pageNo, pageSize, out startIndex, out endIndex);

                var cacheObject = GrpcToCarwaleConvert.ConvertFromGrpcToCarwale(GrpcMethods.GetVideosBySubCategories(subCategoryIds, Convert.ToUInt32(applicationId), startIndex, endIndex, enumSortCategoryId));
                output.Videos = cacheObject.Videos;
                output.TotalRecords = cacheObject.TotalRecords;

                if (cacheObject == null || cacheObject.Videos == null || output.TotalRecords < startIndex)
                    return output;

                var pageCount = (int)Math.Ceiling((double)output.TotalRecords / (double)pageSize);

                ushort appId = (ushort)applicationId;

                if (pageNo < pageCount)
                    output.NextPageUrl = String.Format("api/v1/videos/subcategory/{0}/?appId={1}&pageNo={2}&pageSize={3}&sortCategory={4}", subCategoryIds, appId, pageNo + 1, pageSize, enumSortCategoryId.ToString());

                if (pageNo > 1 && pageNo <= pageCount)
                    output.PrevPageUrl = String.Format("api/v1/videos/subcategory/{0}/?appId={1}&pageNo={2}&pageSize={3}&sortCategory={4}", subCategoryIds, appId, pageNo - 1, pageSize, enumSortCategoryId.ToString());

            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "Carwale.BL.Videos.VideosBL GetVideosBySubCategories()");
                objErr.LogException();
            }

            return output;
        }
        public VideoListing GetPopularNewModelVideos(ArticleByCatURI queryString)
        {
            VideoListing result = null;
            try
            {
                result = GrpcToCarwaleConvert.ConvertFromGrpcToCarwale(GrpcMethods.GetPopularNewModelVideos(queryString));
                if (result != null && result.VideoRecordCount != 0 && result.VideoRecordCount >= queryString.StartIndex)
                {
                    if (queryString.PageNo > 0 && queryString.PageSize > 0)
                    {
                        queryString.StartIndex = queryString.PageSize * queryString.PageNo - queryString.PageSize + 1;
                        queryString.EndIndex = queryString.PageSize * queryString.PageNo;
                    }
                    if (queryString.StartIndex > 0 && queryString.EndIndex > 0)
                    {
                        if ((queryString.EndIndex - queryString.StartIndex + 1) < (result.VideoRecordCount - queryString.StartIndex + 1))
                            result.VideosList = result.VideosList.GetRange((int)queryString.StartIndex - 1, (int)(queryString.EndIndex - queryString.StartIndex) + 1);
                        else
                            result.VideosList = result.VideosList.GetRange((int)queryString.StartIndex - 1, (int)(result.VideoRecordCount - queryString.StartIndex) + 1);
                    }
                }
                else
                    result = new VideoListing();
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "Carwale.BL.Videos.VideosBL GetPopularNewModelVideos()");
                objErr.LogException();
            }
            return result;
        }
        public List<VideosEntity> GetVideoList(uint basicId, uint applicationId)
        {
            try
            {
                return GrpcToCarwaleConvert.ConvertFromGrpcToCarwale(GrpcMethods.GetRelatedModelVideosByBasicId(basicId, applicationId));
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "Carwale.BL.Videos.VideosBL GetVideoList()");
                objErr.LogException();
                return null;
            }
        }

        public List<RelatedArticles> GetRelatedVideoContent(int basicId)
        {
            try
            {
                return GrpcToCarwaleConvert.ConvertFromGrpcToCarwale(GrpcMethods.GetRelatedVideoContent(basicId));
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "Carwale.BL.Videos.VideosBL GetRelatedVideoContent()");
                objErr.LogException();
                return null;
            }
        }
        public List<Video> GetVideosBySubCategory(List<Video> result, int startIndex, int endIndex)
        {
            try
            {
                int count = endIndex == -1 || endIndex > result.Count ? result.Count + 1 - startIndex : endIndex + 1 - startIndex;

                if (count <= 0)
                    return null;

                return result.GetRange(--startIndex, count);
            }
            catch (Exception err)
            {
                ExceptionHandler objErr = new ExceptionHandler(err, "VideoService GetVideosBySubCategory Exception");
                objErr.LogException();
                return null;
            }
        }

        public List<Video> GetArticleVideos(int basicId)
        {
            try
            {
                return GrpcToCarwaleConvert.ConvertFromGrpcToCarwale(GrpcMethods.GetArticleVideos(basicId));
            }
            catch (Exception err)
            {
                ExceptionHandler objErr = new ExceptionHandler(err, "VideoService GetVideosBySubCategory Exception");
                objErr.LogException();
                return null;
            }
        }
    }
}
