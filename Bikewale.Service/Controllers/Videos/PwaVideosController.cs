using Bikewale.BAL.GrpcFiles;
using Bikewale.DTO.Videos;
using Bikewale.Entities.PWA.Articles;
using Bikewale.Entities.Videos;
using Bikewale.Interfaces.Videos;
using Bikewale.Notifications;
using Bikewale.Service.Utilities;
using Grpc.CMS;
using log4net;
using System;
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
        private readonly IVideosCacheRepository _videosCache = null;
        private readonly IVideos _videos = null;
        public PwaVideosController(IVideos videos, IVideosCacheRepository videosCache)
        {
            _videos = videos;
            _videosCache = videosCache;
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
        [ResponseType(typeof(PwaVideosLandingPageTopVideos)), Route("api/videos/topvideos/lvcount/{lvCount}/erCount/{erCount}/")]
        public IHttpActionResult Get(uint lvCount, uint erCount)
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


        private PwaVideosLandingPageTopVideos GetDataFromApiGateWay()
        {
            bool isAPIData = Bikewale.Utility.BWConfiguration.Instance.UseAPIGateway;
            System.Diagnostics.Stopwatch watch = System.Diagnostics.Stopwatch.StartNew();
            try
            {

                VideosBySubcategory objSubCat = new VideosBySubcategory(_videos);

                if (isAPIData)
                {
                    System.Diagnostics.Stopwatch w1 = System.Diagnostics.Stopwatch.StartNew();

                    isAPIData = GetDataFromApiGateWay(objVM, objSubCat);

                    w1.Stop();
                    long elapsedMs = w1.ElapsedMilliseconds;m
                    log4net.ThreadContext.Properties["GatewayTimeTaken_Page"] = elapsedMs;
                }


                if (!isAPIData)
                {
                    System.Diagnostics.Stopwatch w2 = System.Diagnostics.Stopwatch.StartNew();
                    objVM.MotorSportsWidgetData = objSubCat.GetData("", "51", _pageNo, MotorSportsWidgetTopCount);
                    objVM.ExpertReviewsWidgetData = objSubCat.GetData("", "55", _pageNo, ExpertReviewsTopCount);
                    objVM.FirstRideWidgetData = objSubCat.GetData("", "57", _pageNo, FirstRideWidgetTopCount);
                    objVM.MiscellaneousWidgetData = objSubCat.GetData("", "58", _pageNo, MiscellaneousWidgetTopCount);
                    objVM.LaunchAlertWidgetData = objSubCat.GetData("", "59", _pageNo, LaunchAlertWidgetTopCount);
                    objVM.PowerDriftTopMusicWidgetData = objSubCat.GetData("", "60", _pageNo, PowerDriftTopMusicWidgetTopCount);
                    objVM.FirstLookWidgetData = objSubCat.GetData("", "61", _pageNo, FirstLookWidgetTopCount);
                    objVM.PowerDriftBlockbusterWidgetData = objSubCat.GetData("", "62", _pageNo, PowerDriftBlockbusterWidgetTopCount);
                    objVM.PowerDriftSpecialsWidgetData = objSubCat.GetData("", "63", _pageNo, PowerDriftSpecialsWidgetTopCount);
                    w2.Stop();
                    long elapsedMs = w2.ElapsedMilliseconds;
                    log4net.ThreadContext.Properties["GRPCTimeTaken_Page"] = elapsedMs;
                }
                objVM.Brands = new BrandWidgetModel(BrandWidgetTopCount, _bikeMakes, _objModelCache).GetData(Entities.BikeData.EnumBikeType.Videos);
                BindPageMetas(objVM);
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "VideosLandingPage.GetData");
            }
            finally
            {
                watch.Stop();
                long elapsedMs = watch.ElapsedMilliseconds;
                log4net.ThreadContext.Properties["TimeTaken_Page"] = elapsedMs;
                ErrorClass objPageLog = new ErrorClass(new Exception("Videos Page Performance"), "VideosLandingPage.GetData-Page");
            }
            return objVM;
        }

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
        [ResponseType(typeof(VideosList)), Route("api/videos/pn/{pageNo}/ps/{pageSize}/make/{makeId}/")]
        public IHttpActionResult GetVideosByMakeId(uint pageNo, uint pageSize, int makeId)
        {
            try
            {
                string _apiUrl = string.Empty;

                if (makeId > 0)
                {
                    //objVideosList = objClient.GetApiResponseSync<List<BikeVideoEntity>>(Utility.BWConfiguration.Instance.CwApiHostUrl, Utility.BWConfiguration.Instance.APIRequestTypeJSON, _apiUrl, objVideosList);
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
        [ResponseType(typeof(VideosList)), Route("api/videos/pn/{pageNo}/ps/{pageSize}/model/{modelId}/")]
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