using Bikewale.BAL.GrpcFiles;
using Bikewale.DTO.Videos;
using Bikewale.Entities.Videos;
using Bikewale.Notifications;
using Bikewale.Service.Utilities;
using Grpc.CMS;
using log4net;
using System;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
namespace Bikewale.Service.Videos.Controllers
{
    /// <summary>
    ///  Bikewale Videos Api : List of Videos and Details
    /// Author : Sushil Kumar
    /// Created On : 24th August 2015
    /// Modified by :   Sumit Kate on 18 May 2016
    /// Description :   Extend from CompressionApiController instead of ApiController 
    /// </summary>
    public class VideosController : CompressionApiController//ApiController
    {
        static readonly ILog _logger = LogManager.GetLogger(typeof(VideosController));


        #region Videos List
        /// <summary>
        ///  Modified By : Ashish G. Kamble
        ///  Summary : API to get the list of videos for the specified video subcategory.
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="pageNo">Compulsory. Value should be greater than 0.</param>
        /// <param name="pageSize">Compulsory. No of videos to be shown on per page.</param>
        /// <returns>Categorized Videos List</returns>
        [ResponseType(typeof(VideosList)), Route("api/videos/cat/{categoryId}/pn/{pageNo}/ps/{pageSize}/")]
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