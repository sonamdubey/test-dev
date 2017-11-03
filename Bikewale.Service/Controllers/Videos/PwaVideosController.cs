using Bikewale.BAL.GrpcFiles;
using Bikewale.DTO.Videos;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.PWA.Articles;
using Bikewale.Entities.Videos;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.Videos;
using Bikewale.Models;
using Bikewale.Models.Videos;
using Bikewale.Notifications;
using Bikewale.PWA.Utils;
using Bikewale.Service.Utilities;
using Bikewale.Utility;
using Grpc.CMS;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
namespace Bikewale.Service.Controllers.Pwa.Videos
{
    /// <summary>
    ///  Bikewale Videos Api : List of Videos and Details
    /// Author : Sushil Kumar
    /// Created On : 24th August 2015
    /// Modified by :   Sumit Kate on 18 May 2016
    /// Description :   Extend from CompressionApiController instead of ApiController 
    /// </summary>
    public class PwaVideosController : CompressionApiController
    {
        static readonly ILog _logger = LogManager.GetLogger(typeof(PwaVideosController));
        private readonly IBikeMaskingCacheRepository<BikeModelEntity, int> _objMaskingCache = null;
        private readonly IVideos _videos = null;
        private readonly IBikeModelsCacheRepository<int> _modelCache = null;

        public PwaVideosController(IVideos videos, IBikeMaskingCacheRepository<BikeModelEntity, int> objModelCache, IBikeModelsCacheRepository<int> modelCache )
        {
            _videos = videos;
            _objMaskingCache = objModelCache;
            _modelCache = modelCache;
        }
        #region Top Videos List
        /// <summary>
        ///  Modified By : Ashish G. Kamble
        ///  Summary : API to get the list of videos for the specified video subcategory.
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="pageNo">Compulsory. Value should be greater than 0.</param>
        /// <param name="pageSize">Compulsory. No of videos to be shown on per page.</param>
        /// <returns>Categorized Videos List</returns>
        [ResponseType(typeof(PwaVideosLandingPageTopVideos)), Route("api/pwa/topvideos/")]
        public IHttpActionResult Get()
        {
            try
            {                
                var landingVideos = GetVideosFromCacheForCategory(EnumVideosCategory.FeaturedAndLatest, 5);

                var experReviewVideos = GetVideosFromCacheForSubCategory(55, 2,true);

                if (landingVideos != null || experReviewVideos!=null)
                {
                    var topVideos=new PwaVideosLandingPageTopVideos();
                    topVideos.LandingFirstVideos = landingVideos;
                                        
                    var expertReviews  = new PwaVideosBySubcategory();
                    expertReviews.Videos =experReviewVideos;
                    expertReviews.MoreVideosUrl=@"/expert-reviews-55/";
                    expertReviews.SectionTitle= "Expert Reviews";
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
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.Service.CMS.CMSController");
                return InternalServerError();
            }

        }  //get  Categorized Videos 

        /// <summary>
        ///  Modified By : Ashish G. Kamble
        ///  Summary : API to get the list of videos for the specified video subcategory.
        /// </summary>
        /// <param name="catId"></param>
        /// <param name="count"></param>
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

                    switch(catId)
                    {
                        case 51:
                            moreSubCatvideos.SectionTitle = "Motorsports";
                            break;

                        case 55:
                            moreSubCatvideos.SectionTitle = "Expert Reviews";
                            break;

                        case 57:
                            moreSubCatvideos.SectionTitle = "First Ride Impressions";
                            break;

                        case 58:
                            moreSubCatvideos.SectionTitle = "Miscellaneous";
                            break;

                        case 59:
                            moreSubCatvideos.SectionTitle = "Launch Alert";
                            break;

                        case 60:
                            moreSubCatvideos.SectionTitle = "PowerDrift Top Music";
                            break;

                        case 61:
                            moreSubCatvideos.SectionTitle = "First Look";
                            break;

                        case 62:
                            moreSubCatvideos.SectionTitle = "PowerDrift Blockbuster";
                            break;

                        case 63:
                            moreSubCatvideos.SectionTitle = "PowerDrift Specials";
                            break;

                    }
                    return Ok(moreSubCatvideos);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.Service.CMS.CMSController");
                return InternalServerError();
            }

        }

        [ResponseType(typeof(PwaVideosLandingPageOtherVideos)), Route("api/pwa/othervideos/count/{count}/")]
        public IHttpActionResult Get(ushort count)
        {
            try
            {

                var sportsWidget = GetVideosFromCacheForSubCategory(51, count);
                var firstRide = GetVideosFromCacheForSubCategory(57, count);
                var misc = GetVideosFromCacheForSubCategory(58, count);
                var launchAlert = GetVideosFromCacheForSubCategory(59, count);
                var powerdriftTopMusic = GetVideosFromCacheForSubCategory(60, count);
                var firstLook = GetVideosFromCacheForSubCategory(61, count);
                var powerDriftBlockBuster = GetVideosFromCacheForSubCategory(62, count);
                var powerdriftSpecial = GetVideosFromCacheForSubCategory(63, count);
                
                var otherVideos = new PwaVideosLandingPageOtherVideos();

                otherVideos.FirstLook = new PwaVideosBySubcategory();
                otherVideos.FirstLook.Videos = firstLook;
                otherVideos.FirstLook.MoreVideosUrl = @"/first-look-61/";
                otherVideos.FirstLook.SectionTitle = "First Look";

                otherVideos.FirstRide = new PwaVideosBySubcategory();
                otherVideos.FirstRide.Videos = firstRide;
                otherVideos.FirstRide.MoreVideosUrl = @"/first-ride-impressions-57/";
                otherVideos.FirstRide.SectionTitle = "First Ride Impressions";

                otherVideos.LaunchAlert = new PwaVideosBySubcategory();
                otherVideos.LaunchAlert.Videos = launchAlert;
                otherVideos.LaunchAlert.MoreVideosUrl = @"/launch-alert-59/";
                otherVideos.LaunchAlert.SectionTitle = "Launch Alert";

                otherVideos.Miscellaneous = new PwaVideosBySubcategory();
                otherVideos.Miscellaneous.Videos = misc;
                otherVideos.Miscellaneous.MoreVideosUrl = @"/miscellaneous-58/";
                otherVideos.Miscellaneous.SectionTitle = "Miscellaneous";

                otherVideos.MotorSports = new PwaVideosBySubcategory();
                otherVideos.MotorSports.Videos = sportsWidget;
                otherVideos.MotorSports.MoreVideosUrl = @"/motorsports-51/";
                otherVideos.MotorSports.SectionTitle = "Motorsports";

                otherVideos.PowerDriftBlockbuster = new PwaVideosBySubcategory();
                otherVideos.PowerDriftBlockbuster.Videos = powerDriftBlockBuster;
                otherVideos.PowerDriftBlockbuster.MoreVideosUrl = @"/powerdrift-uster-62/";
                otherVideos.PowerDriftBlockbuster.SectionTitle = "PowerDrift Blockbuster";

                otherVideos.PowerDriftSpecials = new PwaVideosBySubcategory();
                otherVideos.PowerDriftSpecials.Videos = powerdriftSpecial;
                otherVideos.PowerDriftSpecials.MoreVideosUrl = @"/powerdrift-specials-63/";
                otherVideos.PowerDriftSpecials.SectionTitle = "PowerDrift Specials";

                otherVideos.PowerDriftTopMusic = new PwaVideosBySubcategory();
                otherVideos.PowerDriftTopMusic.Videos = powerdriftTopMusic;
                otherVideos.PowerDriftTopMusic.MoreVideosUrl = @"/PowerDrift-top-music-60/";
                otherVideos.PowerDriftTopMusic.SectionTitle = "PowerDrift Top Music";

                otherVideos.Brands = GetBrandsInfo(count);

                return Ok(otherVideos);
              
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.Service.CMS.CMSController");
                return InternalServerError();
            }

        }  //get  Categorized Videos 


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
                    
                    var modelMaskingName = vidDet.MaskingName;

                    uint modelId = 0;                    
                    if (!string.IsNullOrEmpty(modelMaskingName))
                    {
                        var model = _objMaskingCache.GetModelMaskingResponse(modelMaskingName);
                        modelId = model.ModelId;                        
                    }

                    var relatedInfo = new PwaBikeVideoRelatedInfo();
                    relatedInfo.Type = PwaRelatedInfoType.Video;
                    relatedInfo.Url = string.Format("api/pwa/similarvideos/{0}/modelid/{1}",basicID, modelId);
                    relatedInfoList.Add(relatedInfo);

                    relatedInfo = new PwaBikeVideoRelatedInfo();
                    relatedInfo.Type = PwaRelatedInfoType.Bike;
                    relatedInfo.Url =  string.Format("api/pwa/popularbodystyle/modelid/{0}/count/9", modelId);
                    relatedInfoList.Add(relatedInfo);
                    outObj.RelatedInfoApi = relatedInfoList;
                    return Ok(outObj);
                }
                return null;

            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.Service.CMS.CMSController");
                return InternalServerError();
            }

        }  //get  Categorized Videos 


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
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.Service.CMS.CMSController");
                return InternalServerError();
            }

        }  //get  Categorized Videos 

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
                outBikeData.CompleteListUrlAlternateLabel = "Best {0} in India";
                outBikeData.CompleteListUrlLabel = "View all";
                return Ok(outBikeData);
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.Service.CMS.CMSController");
                return InternalServerError();
            }

        }


        private SimilarVideoModelsVM GetSimilarVideos(uint modelId,uint videoId)
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
                        similarVideosModel.ViewAllLinkUrl = String.Format("/{0}-bikes/{1}/videos/", objModel.MakeBase.MaskingName, objModel.MaskingName);
                        similarVideosModel.ViewAllLinkTitle = String.Format("{0} {1} Videos", objModel.MakeBase.MakeName, objModel.ModelName);
                    }
                }
                catch (Exception ex)
                {
                    Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, String.Format("PwaVideosController.GetSimilarVideos(ModelId {0}  Videoid {1})", modelId,videoId));
                }
            }
            return similarVideosModel;
        }

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


     
        private IEnumerable<PwaBikeVideoEntity> GetVideosFromCacheForCategory(EnumVideosCategory vidCat, ushort count)
        {
            try
            {
                var objLandingVideosList = _videos.GetVideosByCategory(vidCat, count);
                return ConverterUtility.PwaConvert(objLandingVideosList);
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "VideosLandingPage.BindLandingVideos");
            }
            return null;
        }


        private IEnumerable<PwaBikeVideoEntity> GetVideosFromCacheForSubCategory(ushort vidCat,ushort count,bool addShortDesc=false)
        {
            try
            {
                BikeVideosListEntity objLandingVideosList = _videos.GetVideosBySubCategory(vidCat.ToString(), 1,(ushort)(1+count));
                if (objLandingVideosList.Videos != null)
                    return ConverterUtility.PwaConvert(objLandingVideosList.Videos);
                else
                    return null;
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "VideosLandingPage.BindLandingVideos");
            }
            return null;
        }


        /// <summary>
        ///  Modified By : Ashish G. Kamble
        ///  Summary : API to get the list of videos for the specified video subcategory.
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="pageNo">Compulsory. Value should be greater than 0.</param>
        /// <param name="pageSize">Compulsory. No of videos to be shown on per page.</param>
        /// <returns>Categorized Videos List</returns>
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
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.Service.CMS.CMSController");
                return InternalServerError();
            }

        }  //get  Categorized Videos 

        /// <summary>
        /// Author: Prasad Gawde
        /// </summary>
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

        #endregion

        #region Videos List
        /// <summary>
        ///  Modified By : Ashish G. Kamble
        ///  Summary : API to get the list of videos for the specified make.
        /// </summary>
        /// <param name="pageNo">Compulsory. Value should be greater than 0.</param>
        /// <param name="pageSize">Compulsory. No of videos to be shown on per page.</param>
        /// <param name="makeId">Mandatory.</param>        
        /// <returns></returns>
        [ResponseType(typeof(VideosList)), Route("api/pwa/videos/pn/{pageNo}/ps/{pageSize}/make/{makeId}/")]
        public IHttpActionResult GetVideosByMakeId(uint pageNo, uint pageSize, int makeId)
        {
            try
            {
                string _apiUrl = string.Empty;

                if (makeId > 0)
                {                    
                    var objVideosList = GetVideosByMakeIdViaGrpc(pageNo, pageSize, makeId);

                    if (objVideosList != null && objVideosList.Videos != null)
                    {
                        return Ok(objVideosList);
                    }
                }

                return NotFound();
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.Service.Videos.VideosController");
                objErr.SendMail();
                return InternalServerError();
            }

        }  //get  Model/Makes Videos 

        /// <summary>
        /// Author: Prasad Gawde
        /// </summary>
        /// <returns></returns>
        private VideosList GetVideosByMakeIdViaGrpc(uint pageNo, uint pageSize, int makeId)
        {
            VideosList videoDTOList = null;
            try
            {
                int startIndex, endIndex;
                Bikewale.Utility.Paging.GetStartEndIndex((int)pageSize, (int)pageNo, out startIndex, out endIndex);

                var _objVideoList = GrpcMethods.GetVideosByMakeId(makeId, (uint)startIndex, (uint)endIndex);

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

        #endregion

        #region Videos List
        /// <summary>
        ///  Modified By : Ashish G. Kamble
        ///  Summary : API to get the list of videos for the specified model.
        /// </summary>
        /// <param name="pageNo">Compulsory. Value should be greater than 0.</param>
        /// <param name="pageSize">Compulsory. No of videos to be shown on per page.</param>        
        /// <param name="modelId">Mandatory.</param>
        /// <returns></returns>
        [ResponseType(typeof(VideosList)), Route("api/pwa/videos/pn/{pageNo}/ps/{pageSize}/model/{modelId}/")]
        public IHttpActionResult GetVideosByModelId(uint pageNo, uint pageSize, int modelId)
        {
            try
            {

                if (modelId > 0)
                {
                    var objVideosList = GetVideosByModelIdViaGrpc(pageNo, pageSize, modelId);

                    if (objVideosList != null && objVideosList.Videos != null && objVideosList.Videos.Count() > 0)
                    {
                        return Ok(objVideosList);
                    }
                }

                return NotFound();
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.Service.Videos.VideosController");
                return InternalServerError();
            }
        }

        /// <summary>
        /// Author: Prasad Gawde
        /// </summary>
        /// <returns></returns>
        private VideosList GetVideosByModelIdViaGrpc(uint pageNo, uint pageSize, int modelId)
        {
            VideosList videoDTOList = null;
            try
            {
                int startIndex, endIndex;
                Bikewale.Utility.Paging.GetStartEndIndex((int)pageSize, (int)pageNo, out startIndex, out endIndex);

                var _objVideoList = GrpcMethods.GetVideosByModelId(modelId, (uint)startIndex, (uint)endIndex);

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

        #endregion

    }   // class
}   // namespace