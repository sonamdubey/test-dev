using AutoMapper;
using Carwale.DTOs.CMS.Media;
using Carwale.Entity.CMS;
using Carwale.Entity.CMS.Media;
using Carwale.Entity.CMS.URIs;
using Carwale.Interfaces.CMS;
using Carwale.Notifications;
using System;
using System.Linq;
using System.Web.Http;

namespace Carwale.Service.Controllers.CMS
{
    public class MediaController : ApiController
    {
        private readonly IMediaBL _media;
        public MediaController(IMediaBL media)
        {
            _media = media;
        }
        [HttpGet, Route("api/media/")]
        public IHttpActionResult GetPopularModelMedia([FromUri]ArticleByCatURI queryString)
        {
            if (string.IsNullOrEmpty(queryString.CategoryIdList) || queryString.MakeId < 0 || queryString.ModelId < 0) 
                return BadRequest();            

            try
            {
                MediaDTO response = Mapper.Map<Media, MediaDTO>(_media.GetMediaListing(queryString));
                if (response == null || (response.Photos == null && response.Videos == null))
                    return NotFound();
                if (Request.Headers.Contains("sourceId"))
                {
                    int sourceId = Convert.ToInt32(Request.Headers.GetValues("sourceId").First());
                    if (sourceId == (int)Carwale.Entity.Enum.Platform.CarwaleAndroid || sourceId == (int)Carwale.Entity.Enum.Platform.CarwaleiOS)
                    {
                        uint listCount = queryString.CategoryIdList == CMSContentType.Images.ToString("D") ? response.Photos.ImageRecordCount : response.Videos.VideoRecordCount;
                        string nextPageUrl = (listCount <= queryString.PageSize * queryString.PageNo) ?
                            "" : string.Format("{0}api/media/?categoryidlist={1}&applicationid={2}&pageNo={3}&pageSize={4}{5}{6}{7}",
                            System.Configuration.ConfigurationManager.AppSettings["WebApiHostUrl"].Replace("api/", ""), queryString.CategoryIdList,
                            queryString.ApplicationId, queryString.PageNo + 1, queryString.PageSize,
                            (queryString.MakeId > 0 ? string.Format("&makeid={0}", queryString.MakeId) : string.Empty),
                            (queryString.ModelId > 0 ? string.Format("&modelid={0}", queryString.ModelId) : string.Empty),
                            queryString.GetAllMedia ? "&getAllMedia=true" : string.Empty);

                        if (queryString.CategoryIdList == CMSContentType.Images.ToString("D"))
                            response.Photos.NextPageUrl = nextPageUrl;
                        else
                            response.Videos.NextPageUrl = nextPageUrl;

                        if (sourceId == (int)Carwale.Entity.Enum.Platform.CarwaleAndroid)
                            response.Photos.PhotosList = response.Photos.PhotosList.OrderByDescending(c => c.IsNew).ToList();
                    }
                }
                return Ok(response);
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "MediaController.GetPopularModelMedia()");
                objErr.LogException();
            }
            return InternalServerError();
        }

    }
}
