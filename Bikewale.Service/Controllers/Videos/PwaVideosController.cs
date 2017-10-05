using Bikewale.BAL.GrpcFiles;
using Bikewale.DTO.Videos;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.PWA.Articles;
using Bikewale.Entities.Videos;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.Videos;
using Bikewale.Models.Videos;
using Bikewale.Notifications;
using Bikewale.Service.Utilities;
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
    public class PwaVideosController : CompressionApiController//ApiController
    {
        static readonly ILog _logger = LogManager.GetLogger(typeof(PwaVideosController));
        private readonly IBikeMaskingCacheRepository<BikeModelEntity, int> _objModelCache = null;
        private readonly IVideos _videos = null;
        public PwaVideosController(IVideos videos, IBikeMaskingCacheRepository<BikeModelEntity, int> objModelCache)
        {
            _videos = videos;
            _objModelCache = objModelCache;
        }
        #region Videos List
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

                var experReviewVideos = GetVideosFromCacheForSubCategory(55, 2);

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
        [ResponseType(typeof(PwaVideosLandingPageTopVideos)), Route("api/pwa/catvideos/catId/{catId}/count/{count}/")]
        public IHttpActionResult Get(uint catId,ushort count)
        {
            try
            {
                var landingVideos = GetVideosFromCacheForCategory((EnumVideosCategory)catId, count);

                var experReviewVideos = GetVideosFromCacheForSubCategory(55, 2);

                if (landingVideos != null || experReviewVideos != null)
                {
                    var topVideos = new PwaVideosLandingPageTopVideos();
                    topVideos.LandingFirstVideos = landingVideos;

                    var expertReviews = new PwaVideosBySubcategory();
                    expertReviews.Videos = experReviewVideos;
                    expertReviews.MoreVideosUrl = @"/expert-reviews-55/";
                    expertReviews.SectionTitle = "Expert Reviews";
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


        private PwaBrandsInfo GetBrandsInfo(int topCount)
        {
            var brands = _objModelCache.GetMakeIfVideo();
            int count = 0;
            PwaBrandsInfo outData = new PwaBrandsInfo();
            var topBrands = new List<PwaBikeMakeEntityBase>();
            var otherBrands = new List<PwaBikeMakeEntityBase>();

            foreach (var make in brands)
            {
                if (count < topCount)
                {
                    topBrands.Add(Convert(make));
                }
                else
                {
                    otherBrands.Add(Convert(make));
                }
                count++;
            }
            outData.TopBrands = topBrands;
            outData.OtherBrands = otherBrands;

            return outData;
        }

        private PwaBikeMakeEntityBase Convert(BikeMakeEntityBase inp)
        {
            PwaBikeMakeEntityBase outEntity = null;
            if (inp!=null)
            {
                outEntity = new PwaBikeMakeEntityBase();
                outEntity.MakeId = inp.MakeId;
                outEntity.MakeName = inp.MakeName;
                outEntity.Href = String.Format("/{0}-bikes/videos/", inp.MaskingName);
                outEntity.Title = String.Format("{0} bikes videos", inp.MakeName);
            }
            return outEntity;
        }

        private IEnumerable<PwaBikeVideoEntity> GetVideosFromCacheForCategory(EnumVideosCategory vidCat, ushort count)
        {
            try
            {
                var objLandingVideosList = _videos.GetVideosByCategory(vidCat, count);
                return Convert(objLandingVideosList);
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "VideosLandingPage.BindLandingVideos");
            }
            return null;
        }


        private IEnumerable<PwaBikeVideoEntity> GetVideosFromCacheForSubCategory(ushort vidCat,ushort count)
        {
            try
            {
                BikeVideosListEntity objLandingVideosList = _videos.GetVideosBySubCategory(vidCat.ToString(), 1,(ushort)(1+count));
                if (objLandingVideosList.Videos != null)
                    return Convert(objLandingVideosList.Videos);
                else
                    return null;
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "VideosLandingPage.BindLandingVideos");
            }
            return null;
        }


        private IEnumerable<PwaBikeVideoEntity> Convert(IEnumerable<BikeVideoEntity> inpList)
        {
            List<PwaBikeVideoEntity> outList = null;
            PwaBikeVideoEntity tempData;
            if (inpList != null && inpList.Count() > 0)
            {
                outList = new List<PwaBikeVideoEntity>();
                foreach (var inp in inpList)
                {
                    tempData = Convert(inp);
                    if (tempData != null)
                        outList.Add(tempData);
                }
            }
            return outList;
        }

        private PwaBikeVideoEntity Convert(BikeVideoEntity inpEntity)
        {
            PwaBikeVideoEntity outEntity=null;
            if(inpEntity!=null)
            {
                outEntity = new PwaBikeVideoEntity();
                outEntity.BasicId = inpEntity.BasicId;
                outEntity.Description = inpEntity.Description;
                outEntity.DisplayDate = inpEntity.DisplayDate;
                outEntity.Likes = inpEntity.BasicId;
                outEntity.VideoId = inpEntity.VideoId;
                outEntity.VideoTitle = inpEntity.VideoTitle;
                outEntity.VideoTitleUrl = inpEntity.VideoTitleUrl;
                outEntity.VideoUrl= inpEntity.VideoUrl;
                outEntity.Views = inpEntity.Views;
                outEntity.DisplayImageUrl = inpEntity.ImagePath;        
            }
            return outEntity;

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