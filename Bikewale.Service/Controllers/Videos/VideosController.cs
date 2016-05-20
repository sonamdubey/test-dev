using Bikewale.DTO.Videos;
using Bikewale.Entities.Videos;
using Bikewale.Notifications;
using Bikewale.Service.AutoMappers.Videos;
using Bikewale.Service.Utilities;
using System;
using System.Collections.Generic;
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
        string _applicationid = Utility.BWConfiguration.Instance.ApplicationId;

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
                string _apiUrl = String.Format("/api/v1/videos/category/{0}/?appId={1}&pageNo={2}&pageSize={3}", (int)categoryId, _applicationid, pageNo, pageSize);

                List<BikeVideoEntity> objVideosList = null;

                using (Utility.BWHttpClient objClient = new Utility.BWHttpClient())
                {
                    //objVideosList = objClient.GetApiResponseSync<List<BikeVideoEntity>>(Utility.BWConfiguration.Instance.CwApiHostUrl, Utility.BWConfiguration.Instance.APIRequestTypeJSON, _apiUrl, objVideosList);
                    objVideosList = objClient.GetApiResponseSync<List<BikeVideoEntity>>(Utility.APIHost.CW, Utility.BWConfiguration.Instance.APIRequestTypeJSON, _apiUrl, objVideosList);
                }

                if (objVideosList != null && objVideosList.Count > 0)
                {
                    VideosList videoDTOList = new VideosList();
                    videoDTOList.Videos = VideosMapper.Convert(objVideosList);

                    objVideosList.Clear();
                    objVideosList = null;

                    return Ok(videoDTOList);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.Service.CMS.CMSController");
                objErr.SendMail();
                return InternalServerError();
            }

        }  //get  Categorized Videos 
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
                    _apiUrl = String.Format("/api/v1/videos/make/{0}/?appId=2&pageNo={1}&pageSize={2}", makeId, pageNo, pageSize);

                    List<BikeVideoEntity> objVideosList = null;

                    using (Utility.BWHttpClient objClient = new Utility.BWHttpClient())
                    {
                        //objVideosList = objClient.GetApiResponseSync<List<BikeVideoEntity>>(Utility.BWConfiguration.Instance.CwApiHostUrl, Utility.BWConfiguration.Instance.APIRequestTypeJSON, _apiUrl, objVideosList);
                        objVideosList = objClient.GetApiResponseSync<List<BikeVideoEntity>>(Utility.APIHost.CW, Utility.BWConfiguration.Instance.APIRequestTypeJSON, _apiUrl, objVideosList);
                    }

                    if (objVideosList != null && objVideosList.Count > 0)
                    {
                        VideosList videoDTOList = new VideosList();
                        videoDTOList.Videos = VideosMapper.Convert(objVideosList);

                        objVideosList.Clear();
                        objVideosList = null;

                        return Ok(videoDTOList);
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
                string _apiUrl = string.Empty;

                if (modelId > 0)
                {
                    _apiUrl = String.Format("/api/v1/videos/model/{0}/?appId=2&pageNo={1}&pageSize={2}", modelId, pageNo, pageSize);

                    List<BikeVideoEntity> objVideosList = null;

                    using (Utility.BWHttpClient objClient = new Utility.BWHttpClient())
                    {
                        //objVideosList = objClient.GetApiResponseSync<List<BikeVideoEntity>>(Utility.BWConfiguration.Instance.CwApiHostUrl, Utility.BWConfiguration.Instance.APIRequestTypeJSON, _apiUrl, objVideosList);
                        objVideosList = objClient.GetApiResponseSync<List<BikeVideoEntity>>(Utility.APIHost.CW, Utility.BWConfiguration.Instance.APIRequestTypeJSON, _apiUrl, objVideosList);
                    }

                    if (objVideosList != null && objVideosList.Count > 0)
                    {
                        VideosList videoDTOList = new VideosList();
                        videoDTOList.Videos = VideosMapper.Convert(objVideosList);

                        objVideosList.Clear();
                        objVideosList = null;

                        return Ok(videoDTOList);
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
        }
        #endregion

    }   // class
}   // namespace