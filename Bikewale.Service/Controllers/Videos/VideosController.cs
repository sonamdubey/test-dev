using Bikewale.DTO.Videos;
using Bikewale.Entities.Videos;
using Bikewale.Notifications;
using Bikewale.Service.AutoMappers.Videos;
using Bikewale.Service.Controllers;
using Bikewale.Utility;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;                                                    
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace Bikewale.Service.Videos.Controllers
{   
    /// <summary>
    ///  Bikewale Videos Api : List of Videos and Details
    /// Author : Sushil Kumar
    /// Created On : 24th August 2015
    /// </summary>
    public class VideosController : ApiController
    {
        //string _cwHostUrl = ConfigurationManager.AppSettings["cwApiHostUrl"];
        string _cwHostUrl = "http://172.16.1.73";
        string _applicationid = ConfigurationManager.AppSettings["applicationId"];
        string _requestType = "application/json";

        #region Videos List 
        /// <summary>
        ///  To get list of Videos based on Categories defined in Enum
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="pageNo">Min:0</param>
        /// <param name="pageSize">Total Records to be Fetched</param>
        /// <returns>Categorized Videos List</returns>
        [ResponseType(typeof(VideosList))]
        public IHttpActionResult Get(EnumVideosCategory categoryId,uint pageNo,uint pageSize)
        {              
            try
            {
                string _apiUrl = String.Format("/api/v1/videos/category/{0}/?appId={1}&pageNo={2}&pageSize={3}", (int)categoryId, _applicationid, pageNo, pageSize);
                
                List<BikeVideoEntity> objVideosList = null;

                objVideosList = BWHttpClient.GetApiResponseSync<List<BikeVideoEntity>>(_cwHostUrl, _requestType, _apiUrl, objVideosList);
                if (objVideosList != null && objVideosList.Count > 0)
                {
                    VideosList videoDTOList = new VideosList();
                    videoDTOList.Videos = VideosMapper.Convert(objVideosList); 
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
        ///  To get Videos List based on Make or Model (Classes of Videos)
        /// </summary>
        /// <param name="classType">Boolean : True for Make,False for Models</param>
        /// <param name="classId">Make/Model Id</param>
        /// <param name="pageNo"></param>
        /// <param name="pageSize"></param>
        /// <returns>Model's/Make's Videos List</returns>
        [ResponseType(typeof(VideosList))]
        public IHttpActionResult Get(uint pageNo, uint pageSize, int? makeId = null , int? modelId = null)
        {
            try
            {
                string _apiUrl = string.Empty;

                if (modelId.HasValue && modelId > 0)
                {
                    _apiUrl = String.Format("/api/v1/videos/model/{0}/?appId=2&pageNo={1}&pageSize={2}", modelId, pageNo, pageSize);
                }
                else if(makeId.HasValue && makeId > 0)
                {
                    _apiUrl = String.Format("/api/v1/videos/make/{0}/?appId=2&pageNo={1}&pageSize={2}", makeId, pageNo, pageSize);
                }

                if (modelId.HasValue || makeId.HasValue)
                {
                    List<BikeVideoEntity> objVideosList = null;

                    objVideosList = BWHttpClient.GetApiResponseSync<List<BikeVideoEntity>>(_cwHostUrl, _requestType, _apiUrl, objVideosList);

                    if (objVideosList != null && objVideosList.Count > 0)
                    {
                        VideosList videoDTOList = new VideosList();
                        videoDTOList.Videos = VideosMapper.Convert(objVideosList);
                        return Ok(videoDTOList);
                    }
                    else
                    {
                        return NotFound();
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
    }
}
