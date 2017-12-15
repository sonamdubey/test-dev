using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using ApiGatewayLibrary;
using Bikewale.BAL.GrpcFiles;
using Bikewale.DTO.Videos;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.PWA.Articles;
using Bikewale.Entities.Videos;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.Videos;
using Bikewale.Models;
using Bikewale.Notifications;
using Bikewale.PWA.Utils;
using Bikewale.Service.Utilities;
using Bikewale.Utility;
using EditCMSWindowsService.Messages;
using Grpc.CMS;
using log4net;
namespace Bikewale.Service.Controllers.Pwa.Videos
{
    /// <summary>
    /// 
    /// </summary>
    public class PwaVideosController : CompressionApiController
    {
        static readonly ILog _logger = LogManager.GetLogger(typeof(PwaVideosController));
        private readonly IBikeMaskingCacheRepository<BikeModelEntity, int> _objMaskingCache = null;
        private readonly IVideos _videos = null;
        private readonly IBikeModelsCacheRepository<int> _modelCache = null;
        private readonly string EditCMSModuleName = Bikewale.Utility.BWConfiguration.Instance.EditCMSModuleName;

        public PwaVideosController(IVideos videos, IBikeMaskingCacheRepository<BikeModelEntity, int> objModelCache, IBikeModelsCacheRepository<int> modelCache )
        {
            _videos = videos;
            _objMaskingCache = objModelCache;
            _modelCache = modelCache;
        }
        #region Top Videos List
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [ResponseType(typeof(PwaVideosLandingPageTopVideos)), Route("api/pwa/topvideos/")]
        public IHttpActionResult Get()
        {
            try
            {
                ushort expertReviewCatId = 55;                      
                var landingVideos = GetVideosFromCacheForCategory(EnumVideosCategory.FeaturedAndLatest, 5);

                var experReviewVideos = GetVideosFromCacheForSubCategory(expertReviewCatId, 2,true);

                if (landingVideos != null || experReviewVideos!=null)
                {
                    var topVideos=new PwaVideosLandingPageTopVideos();
                    topVideos.LandingFirstVideos = landingVideos;
                                        
                    var expertReviews  = new PwaVideosBySubcategory();
                    string moreVidUrl, sectionTitle;
                    VideoTitleDescription.VideoGetTitleAndUrl(expertReviewCatId, out sectionTitle, out moreVidUrl);
                    expertReviews.Videos =experReviewVideos;                                     
                    expertReviews.MoreVideosUrl= moreVidUrl;
                    expertReviews.SectionTitle= sectionTitle;
                    topVideos.ExpertReviews = expertReviews;
                    return Ok(topVideos);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                PwaErrorClass objErr = new PwaErrorClass(ex, "Exception : Bikewale.Service.CMS.CMSController", "api/pwa/topvideos/");
                return InternalServerError();
            }

        }  //get  Categorized Videos 

    /// <summary>
    /// 
    /// </summary>
    /// <param name="catId"></param>
    /// <param name="count"></param>
    /// <returns></returns>
        [ResponseType(typeof(PwaVideosBySubcategory)), Route("api/pwa/catvideos/catId/{catId}/count/{count}/")]
        public IHttpActionResult Get(ushort catId,ushort count)
        {
            try
            { 
                var subCatVideos = GetVideosFromCacheForSubCategory(catId, count);

                if (subCatVideos != null)
                {
                    var moreSubCatvideos = new PwaVideosBySubcategory();
                    moreSubCatvideos.Videos = subCatVideos;
                    moreSubCatvideos.SectionTitle = VideoTitleDescription.VideoCategoryTitle(catId);
                    return Ok(moreSubCatvideos);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                PwaErrorClass objErr = new PwaErrorClass(ex, "Exception : Bikewale.Service.CMS.CMSController",string.Format("api/pwa/catvideos/catId/{0}/count/{1}/",catId,count));
                return InternalServerError();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        [ResponseType(typeof(PwaVideosLandingPageOtherVideos)), Route("api/pwa/othervideos/count/{count}/")]
        public IHttpActionResult Get(ushort count)
        {
            try
            {
                var otherVideos = new PwaVideosLandingPageOtherVideos();
                int[] categoryIds = {61,57,59,58,51,62,63,60};
                ushort[] categoryTotalRecords = new ushort[] {count,count,count,count,count,count,count,count };

                CallAggregator ca = GetVideosBySubCategoriesUsingAPIGateway(categoryIds, categoryTotalRecords);

                var apiData = ca.GetResultsFromGateway();

                if (apiData != null && apiData.OutputMessages != null)
                {
                    var objApiData = apiData.OutputMessages;

                    if (objApiData != null && objApiData.Count > 0)
                    {
                        IEnumerable<PwaBikeVideoEntity>[] apiGateWayOutput = new IEnumerable<PwaBikeVideoEntity>[objApiData.Count];
                        for (ushort i = 0; i < objApiData.Count; i++)
                        {
                            apiGateWayOutput[i] = ProcessGrpcVideoListEntity(ApiGatewayLibrary.Utilities.ConvertBytesToMsg<GrpcVideoListEntity>(objApiData[i].Payload));
                        }
                        
                        otherVideos.FirstLook = PwaCmsHelper.SetPwaSubCategoryVideos(apiGateWayOutput[0], categoryIds[0]);
                        otherVideos.FirstRide = PwaCmsHelper.SetPwaSubCategoryVideos(apiGateWayOutput[1], categoryIds[1]);                        
                        otherVideos.LaunchAlert = PwaCmsHelper.SetPwaSubCategoryVideos(apiGateWayOutput[2], categoryIds[2]);
                        otherVideos.Miscellaneous = PwaCmsHelper.SetPwaSubCategoryVideos(apiGateWayOutput[3], categoryIds[3]);
                        otherVideos.MotorSports = PwaCmsHelper.SetPwaSubCategoryVideos(apiGateWayOutput[4], categoryIds[4]);
                        otherVideos.PowerDriftBlockbuster = PwaCmsHelper.SetPwaSubCategoryVideos(apiGateWayOutput[5], categoryIds[5]);
                        otherVideos.PowerDriftSpecials = PwaCmsHelper.SetPwaSubCategoryVideos(apiGateWayOutput[6], categoryIds[6]);
                        otherVideos.PowerDriftTopMusic = PwaCmsHelper.SetPwaSubCategoryVideos(apiGateWayOutput[7], categoryIds[7]);

                        otherVideos.Brands = GetBrandsInfo(count);
                    }
                }
                return Ok(otherVideos);
              
            }
            catch (Exception ex)
            {
                PwaErrorClass objErr = new PwaErrorClass(ex, "Exception : Bikewale.Service.CMS.CMSController",string.Format("api/pwa/othervideos/count/{0}/",count));
                return InternalServerError();
            }

        }  //get  Categorized Videos 

        /// <summary>
        /// 
        /// </summary>
        /// <param name="basicID"></param>
        /// <returns></returns>
        [ResponseType(typeof(PwaBikeVideoDetails)), Route("api/pwa/videodet/{basicId}/")]
        public IHttpActionResult Get(uint basicID)
        {
            try
            {
                PwaBikeVideoDetails outObj = new PwaBikeVideoDetails();
                var vidDet = _videos.GetVideoDetails(basicID);
                if (vidDet != null)
                {
                    outObj.VideoInfo = ConverterUtility.PwaConvert(vidDet, true);
                    var relatedInfoList = new List<PwaBikeVideoRelatedInfo>();          
                    uint modelId = 0;                    
                    if (!string.IsNullOrEmpty(vidDet.MaskingName))
                    {
                        modelId = vidDet.ModelId;                        
                    }
                
                    if(modelId>0)
                        relatedInfoList.Add(new PwaBikeVideoRelatedInfo(PwaRelatedInfoType.Video, string.Format("api/pwa/similarvideos/{0}/modelid/{1}", basicID, modelId)));

                    relatedInfoList.Add(new PwaBikeVideoRelatedInfo(PwaRelatedInfoType.Bike, string.Format("api/pwa/popularbodystyle/modelid/{0}/count/9", modelId)));

                    outObj.RelatedInfoApi = relatedInfoList;
                    return Ok(outObj);
                }
                return null;

            }
            catch (Exception ex)
            {
                PwaErrorClass objErr = new PwaErrorClass(ex, "Exception : Bikewale.Service.CMS.CMSController",string.Format("api/pwa/videodet/{0}/",basicID));
                return InternalServerError();
            }

        }  //get  Categorized Videos 

        /// <summary>
        /// 
        /// </summary>
        /// <param name="basicId"></param>
        /// <param name="modelId"></param>
        /// <returns></returns>
        [ResponseType(typeof(PwaBikeVideos)), Route("api/pwa/similarvideos/{basicId}/modelid/{modelId}")]
        public IHttpActionResult Get(uint basicId, uint modelId)
        {
            try
            {
                PwaBikeVideos outObj = new PwaBikeVideos();
                var similarVidList = GetSimilarVideos(modelId,basicId);
                if (similarVidList != null)
                {
                    var videosList = ConverterUtility.PwaConvert(similarVidList.Videos);
                    if(videosList!=null)
                        outObj.VideosList = videosList.ToList();
                    outObj.CompleteListUrl = similarVidList.ViewAllLinkUrl;
                    outObj.CompleteListUrlAlternateLabel = similarVidList.ViewAllLinkTitle;
                    outObj.Heading = "More related videos";
                    outObj.CompleteListUrlLabel = similarVidList.ViewAllLinkText;
                    return Ok(outObj);
                }
                return null;

            }
            catch (Exception ex)
            {
                PwaErrorClass objErr = new PwaErrorClass(ex, "Exception : Bikewale.Service.CMS.CMSController",string.Format("api/pwa/similarvideos/{0}/modelid/{1}",basicId,modelId));
                return InternalServerError();
            }

        }  //get  Categorized Videos 

        /// <summary>
        /// 
        /// </summary>
        /// <param name="modelId"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        [ResponseType(typeof(PwaBikeNews)), Route("api/pwa/popularbodystyle/modelid/{modelId}/count/{count}")]
        public IHttpActionResult Get(int modelId,uint count)
        {
            try
            {
                var cityArea = GlobalCityArea.GetGlobalCityArea();
                uint cityId = cityArea.CityId;
                cityId = cityId == 0 ? System.Convert.ToUInt32(BWConfiguration.Instance.DefaultCity) : cityId;
                IEnumerable<MostPopularBikesBase> objPopularBodyStyle = _modelCache.GetMostPopularBikesByModelBodyStyle(modelId, 9, cityId);
                PwaBikeNews outBikeData = new PwaBikeNews();
                outBikeData.BikesList = ConverterUtility.MapMostPopularBikesBaseToPwaBikeDetails(objPopularBodyStyle, cityArea.City);
                outBikeData.Heading = "Popular bikes";
                outBikeData.CompleteListUrl = "/m/best-bikes-in-india/";
                outBikeData.CompleteListUrlAlternateLabel = "Best Bikes in India";
                outBikeData.CompleteListUrlLabel = "View all";
                return Ok(outBikeData);
            }
            catch (Exception ex)
            {
                PwaErrorClass objErr = new PwaErrorClass(ex, "Exception : Bikewale.Service.CMS.CMSController", string.Format("api/pwa/popularbodystyle/modelid/{0}/count/{1}",modelId,count));
                return InternalServerError();
            }

        }
      
        /// <summary>
        /// 
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="pageNo"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [ResponseType(typeof(VideosList)), Route("api/pwa/pwa/videos/cat/{categoryId}/pn/{pageNo}/ps/{pageSize}/")]
        public IHttpActionResult Get(EnumVideosCategory categoryId, uint pageNo, uint pageSize)
        {
            try
            {
                var objVideosList = GetVideosByCategoryIdViaGrpc((int)categoryId, pageNo, pageSize);

                if (objVideosList != null && objVideosList.Videos != null)
                {
                    return Ok(objVideosList);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                PwaErrorClass objErr = new PwaErrorClass(ex, "Exception : Bikewale.Service.CMS.CMSController",string.Format("api/pwa/pwa/videos/cat/{0}/pn/{1}/ps/{2}/",categoryId,pageNo,pageSize));
                return InternalServerError();
            }

        }  //get  Categorized Videos 

        #region PwaHelperMethods
        /// <summary>
        /// 
        /// </summary>
        /// <param name="modelId"></param>
        /// <param name="videoId"></param>
        /// <returns></returns>
        private SimilarVideoModelsVM GetSimilarVideos(uint modelId, uint videoId)
        {
            SimilarVideoModelsVM similarVideosModel = null;
            if (modelId > 0)
            {
                try
                {
                    similarVideosModel = new SimilarVideoModelsVM();
                    similarVideosModel.Videos = _videos.GetSimilarModelsVideos(videoId, modelId, 9);
                    similarVideosModel.ModelId = modelId;
                    BikeModelEntity objModel = _objMaskingCache.GetById((int)modelId);
                    if (objModel != null)
                    {
                        similarVideosModel.ViewAllLinkText = "View all";
                        similarVideosModel.ViewAllLinkUrl = String.Format("/m/{0}-bikes/{1}/videos/", objModel.MakeBase.MaskingName, objModel.MaskingName);
                        similarVideosModel.ViewAllLinkTitle = String.Format("{0} {1} Videos", objModel.MakeBase.MakeName, objModel.ModelName);
                    }
                }
                catch (Exception ex)
                {
                    Bikewale.Notifications.PwaErrorClass objErr = new Bikewale.Notifications.PwaErrorClass(ex, String.Format("PwaVideosController.GetSimilarVideos(ModelId {0}  Videoid {1})", modelId, videoId));
                }
            }
            return similarVideosModel;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="topCount"></param>
        /// <returns></returns>
        private PwaBrandsInfo GetBrandsInfo(int topCount)
        {
            var brands = _objMaskingCache.GetMakeIfVideo();
            int count = 0;
            PwaBrandsInfo outData = new PwaBrandsInfo();
            var topBrands = new List<PwaBikeMakeEntityBase>();
            var otherBrands = new List<PwaBikeMakeEntityBase>();

            foreach (var make in brands)
            {
                if (count < topCount)
                {
                    topBrands.Add(ConverterUtility.PwaConvert(make));
                }
                else
                {
                    otherBrands.Add(ConverterUtility.PwaConvert(make));
                }
                count++;
            }
            outData.TopBrands = topBrands;
            outData.OtherBrands = otherBrands;

            return outData;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vidCat"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        private IEnumerable<PwaBikeVideoEntity> GetVideosFromCacheForCategory(EnumVideosCategory vidCat, ushort count)
        {
            try
            {
                var objLandingVideosList = _videos.GetVideosByCategory(vidCat, count);
                return ConverterUtility.PwaConvert(objLandingVideosList);
            }
            catch (Exception ex)
            {
                PwaErrorClass objErr = new PwaErrorClass(ex, "GetVideosFromCacheForCategory");
            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vidCat"></param>
        /// <param name="count"></param>
        /// <param name="addShortDesc"></param>
        /// <returns></returns>
        private IEnumerable<PwaBikeVideoEntity> GetVideosFromCacheForSubCategory(ushort vidCat, ushort count, bool addShortDesc = false)
        {
            try
            {
                BikeVideosListEntity objLandingVideosList = _videos.GetVideosBySubCategory(vidCat.ToString(), 1, (ushort)(1 + count),VideosSortOrder.JustLatest);
                if (objLandingVideosList.Videos != null)
                    return ConverterUtility.PwaConvert(objLandingVideosList.Videos,addShortDesc);
                else
                    return null;
            }
            catch (Exception ex)
            {
                PwaErrorClass objErr = new PwaErrorClass(ex, "GetVideosFromCacheForSubCategory");
            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inpVideoList"></param>
        /// <returns></returns>
        private IEnumerable<PwaBikeVideoEntity> ProcessGrpcVideoListEntity(GrpcVideoListEntity inpVideoList)
        {
            if (inpVideoList != null && inpVideoList.TotalRecords > 0)
            {
                var processedVideosList = GrpcToBikeWaleConvert.ConvertFromGrpcToBikeWale(inpVideoList);
                if (processedVideosList != null)
                    return ConverterUtility.PwaConvert(processedVideosList.Videos);
            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="pageNo"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        private VideosList GetVideosByCategoryIdViaGrpc(int categoryId, uint pageNo, uint pageSize)
        {
            VideosList videoDTOList = null;
            try
            {

                int startIndex, endIndex;
                Bikewale.Utility.Paging.GetStartEndIndex((int)pageSize, (int)pageNo, out startIndex, out endIndex);

                var _objVideoList = GrpcMethods.GetVideosBySubCategory((uint)categoryId, (uint)startIndex, (uint)endIndex);

                if (_objVideoList != null)
                {
                    videoDTOList = GrpcToBikeWaleConvert.ConvertFromGrpcToBikeWale(_objVideoList);
                }
            }
            catch (Exception err)
            {
                _logger.Error(err.Message, err);
            }

            return videoDTOList;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="categoryIds"></param>
        /// <param name="categoryTotalRecords"></param>
        /// <returns></returns>
        private CallAggregator GetVideosBySubCategoriesUsingAPIGateway(int[] categoryIds, ushort[] categoryTotalRecords)
        {
            CallAggregator ca = null;
            try
            {
                bool IsRecordsCountAvailable = categoryTotalRecords != null && categoryTotalRecords.Length > 0;
                if (categoryIds != null && categoryIds.Length > 0)
                {
                    ca = new CallAggregator();
                    for (ushort i = 0; i < categoryIds.Length; i++)
                    {
                        ca.AddCall(EditCMSModuleName, "GetVideosBySubCategories", new GrpcVideosBySubCategoriesURI()
                        {
                            ApplicationId = 2,
                            SubCategoryIds = categoryIds[i].ToString(),
                            StartIndex = 1,
                            EndIndex = (uint)(IsRecordsCountAvailable ? categoryTotalRecords[i] : 9),
                            SortCategory = GrpcVideoSortOrderCategory.MostPopular
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                PwaErrorClass objErr = new PwaErrorClass(ex, "GetVideosBySubCategoriesUsingAPIGateway");
            }

            return ca;

        }

        #endregion PwaHelperMethods

        #endregion

    }   // class
}   // namespace