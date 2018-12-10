using Carwale.Entity;
using Carwale.Entity.CMS;
using Carwale.Entity.CMS.URIs;
using Carwale.Interfaces.CMS;
using Carwale.Notifications;
using Carwale.Utility;
using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Cors;

namespace Carwale.Service.Controllers.CMS
{
    /// <summary>
    /// Created on 26 Aug 2015
    /// Modified by     :   Sumit Kate on 18 Feb 2016
    /// Description     :   Added new method VideoListEntity GetVideosBySubCategories(string subCategoryIds, CMSAppId applicationId, int startIndex, int endindex, VideoSortOrderCategory sortCriteria);
    /// </summary>
    [EnableCors(origins: "http://www.bikewale.com,https://www.bikewale.com,https://staging.bikewale.com,http://localhost:9011,http://staging.bikewale.com,http://webserver:9011", headers: "*", methods: "*")]
    public class VideosController : ApiController
    {
        private readonly IVideosBL _video;

        public VideosController(IVideosBL video)
        {
            _video = video;
        }

        [HttpGet, Route("api/v1/videos/{id}")]
        public IHttpActionResult GetVideo(int id, ushort appId)
        {
            Video _result = null;
            try
            {
                _result = _video.GetVideoByBasicId(id, (Entity.CMS.CMSAppId)appId);

                if (_result.BasicId <= 0)
                    return NotFound();
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "Carwale.Service.Controllers.CMS.VideoController");
                objErr.LogException();
                return InternalServerError(ex);
            }
            return Ok(_result);
        }

        [HttpGet, Route("api/v1/videos/category/{category}")]
        public IHttpActionResult GetVideosBySubCategory(ushort category, ushort appId, ushort pageNo, ushort pageSize)
        {
            List<Video> _result = null;
            try
            {
                ushort _startIndex, _endIndex;
                Calculation.GetStartEndIndex(pageNo, pageSize, out _startIndex, out _endIndex);

                _result = _video.GetNewModelsVideosBySubCategory((EnumVideoCategory)category, (Entity.CMS.CMSAppId)appId, _startIndex, _endIndex);

            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "Carwale.Service.Controllers.CMS.VideoController");
                objErr.LogException();
                return InternalServerError(ex);
            }
            return Ok(_result);
        }

        /// <summary>        
        /// Created by  :   Sumit Kate on 18 Feb 2016
        /// Description :   Returns the Video list by Sub Categories
        /// </summary>
        /// <param name="categories">Video Category Ids (CSV) E.g.47,48,49</param>
        /// <param name="appId">1 - CarWale, 2 - BikeWale</param>
        /// <param name="pageNo"></param>
        /// <param name="pageSize"></param>
        /// <param name="sortCategory">Please refer Carwale.Entity.Enum.VideoSortOrderCategory </param>
        /// <returns></returns>
        [HttpGet, Route("api/v1/videos/subcategory/{categoryIds}")]
        public IHttpActionResult GetVideosBySubCategories(string categoryIds, ushort appId, ushort pageNo, ushort pageSize, string sortCategory = null)
        {
            var videos = _video.GetVideosBySubCategories(categoryIds, (Entity.CMS.CMSAppId)appId, pageNo, pageSize, sortCategory);

            return Ok(videos);
        }

        [Route("api/v1/videos/make/{makeId}/")]
        [HttpGet]
        public IHttpActionResult GetMakeVideos(int makeId, ushort appId, ushort pageNo, ushort pageSize)
        {
            List<Video> _result = null;
            try
            {
                ushort _startIndex, _endIndex;
                Calculation.GetStartEndIndex(pageNo, pageSize, out _startIndex, out _endIndex);

                _result = _video.GetNewModelsVideosByMakeId(makeId, (Entity.CMS.CMSAppId)appId, _startIndex, _endIndex);

            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "Carwale.Service.Controllers.CMS.VideoController");
                objErr.LogException();
                return InternalServerError(ex);
            }
            return Ok(_result);
        }

        [Route("api/v1/videos/model/{modelId}/")]
        [HttpGet]
        public IHttpActionResult GetModelVideos(int modelId, ushort appId, ushort pageNo, ushort pageSize)
        {
            List<Video> _result = null;
            try
            {
                ushort _startIndex, _endIndex;
                Calculation.GetStartEndIndex(pageNo, pageSize, out _startIndex, out _endIndex);

                _result = _video.GetVideosByModelId(modelId, (Entity.CMS.CMSAppId)appId, _startIndex, _endIndex);

            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "Carwale.Service.Controllers.CMS.VideoController");
                objErr.LogException();
                return InternalServerError(ex);
            }
            return Ok(_result);
        }

        [Route("api/v1/videos/{id}/similar/")]
        [HttpGet]
        public IHttpActionResult GetSimilarVideos(int id, ushort appId, int topCount)
        {
            List<Video> _result = null;
            try
            {
                _result = _video.GetSimilarVideos(id, (Entity.CMS.CMSAppId)appId, topCount);

            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "Carwale.Service.Controllers.CMS.VideoController");
                objErr.LogException();
                return InternalServerError(ex);
            }
            return Ok(_result);
        }

        [HttpGet, Route("api/videos/similar/")]
        public IHttpActionResult GetSimilarVideosByModel([FromUri] ArticleByCatURI queryString)
        {
            try
            {
                if (queryString.MakeId < 0 || queryString.ModelId < 0)
                    return BadRequest();

                List<Video> response = new List<Video>();
                response = _video.GetSimilarVideos(queryString);

                if (response == null || response.Count == 0)
                    return Ok(new List<Video>());

                return Ok(response);
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "VideosController.GetSimilarVideosByModel()");
                objErr.LogException();
                return InternalServerError();
            }
        }
    }
}
