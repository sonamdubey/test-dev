using Bikewale.DTO.Videos;
using Bikewale.Entities.Videos;
using Bikewale.Notifications;
using Bikewale.Service.AutoMappers.Videos;
using Bikewale.Utility;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace Bikewale.Service.Controllers
{   
    /// <summary>
    /// Bikewale Videos Api : List of Videos and Details 
    /// </summary>
    public class VideosController : ApiController
    {
        string _cwHostUrl = ConfigurationManager.AppSettings["cwApiHostUrl"];
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
        [ResponseType(typeof(IEnumerable<VideosList>))]
        public IHttpActionResult Get(EnumVideosCategory categoryId,uint pageNo,uint pageSize)
        {              
            try
            {                     

                string _apiUrl = String.Format("webapi/v1/categoryId/{0}/?appId={1}&pageNo={2}&pageSize={3}",categoryId, _applicationid, pageNo, pageSize);
                
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
        [ResponseType(typeof(IEnumerable<VideosList>))]
        public IHttpActionResult Get(bool classType,uint classId, uint pageNo, uint pageSize)
        {
            try
            {
                string className = (classType) ? "make" : "model";
                
                string _apiUrl = String.Format("webapi/v1/{0}/{1}/?appId={2}&pageNo={3}&pageSize={4}",className,classId,_applicationid,pageNo,pageSize);
                
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
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.Service.Videos.VideosController");
                objErr.SendMail();
                return InternalServerError();
            }

        }  //get  Model/Makes Videos 
        #endregion 
    }
}
