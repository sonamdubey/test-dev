using AutoMapper;
using Carwale.DTOs.CMS.ThreeSixtyView;
using Carwale.Entity.CMS.ThreeSixtyView;
using Carwale.Entity.Enum;
using Carwale.Interfaces.CMS;
using Carwale.Notifications.Logs;
using Carwale.Service.Filters;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Http.Cors;

namespace Carwale.Service.Controllers.CMS
{
    public class ThreeSixtyViewController : ApiController
    {
        private readonly IThreeSixtyView _threeSixtyView;

        public ThreeSixtyViewController(IThreeSixtyView threeSixtyView) {
            _threeSixtyView = threeSixtyView;
        }

        [HttpGet, Route("api/360/exterior/{modelId}/")]
        public HttpResponseMessage GetExteriorConfig([FromUri] int modelId, ThreeSixtyViewCategory category, string qualityFactor = "80", int imageCount = 36) {
            try
            {
                if (imageCount % 18 != 0)
                    imageCount = 36;

                ThreeSixty threeSixty = _threeSixtyView.GetExterior360Config(modelId, category, qualityFactor, imageCount);

                if (threeSixty == null)
                    return Request.CreateResponse(HttpStatusCode.InternalServerError);

                if (threeSixty.ExteriorImages == null)
                    return Request.CreateResponse(HttpStatusCode.NoContent);

                return Request.CreateResponse(HttpStatusCode.OK, Mapper.Map<ThreeSixty, ThreeSixtyExteriorDtoApp>(threeSixty));
            }
            catch (Exception e)
            {
                Logger.LogException(e);
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet, Route("api/360/interior/{modelId}/")]
        public HttpResponseMessage GetInteriorConfig([FromUri] int modelId, string qualityFactor = "80") {
            try
            {
                ThreeSixty threeSixty = _threeSixtyView.GetInterior360Config(modelId, qualityFactor);

                if (threeSixty == null)
                    return Request.CreateResponse(HttpStatusCode.InternalServerError);

                if (threeSixty.InteriorImages == null)
                    return Request.CreateResponse(HttpStatusCode.NoContent);

                return Request.CreateResponse(HttpStatusCode.OK, Mapper.Map<ThreeSixty, ThreeSixtyInteriorDtoApp>(threeSixty));
            }
            catch (Exception e)
            {
                Logger.LogException(e);
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        [EnableCors(origins: "http://opr.carwale.com", headers: "*", methods: "*", exposedHeaders: "X-My-Header")]
        [HttpGet, Route("api/xml/360/{modelid}/{type}/")]
        public IHttpActionResult Get360XML([FromUri] int modelid, string type, bool getHotspots = false, bool isMsite = false, int imageCount = 72, int qualityFactor= 80)
        {
            ThreeSixtyViewCategory type360;

            if (!Enum.TryParse<ThreeSixtyViewCategory>(type, true, out type360))
                return BadRequest("invalid type");

            HttpResponseMessage resp = new HttpResponseMessage();

            resp.Content = new StringContent(type360 == ThreeSixtyViewCategory.Interior ? _threeSixtyView.GetInterior360XML(modelid, getHotspots, isMsite, qualityFactor) : _threeSixtyView.GetExterior360XML(modelid, type360, getHotspots, isMsite, imageCount, qualityFactor));
            resp.StatusCode = HttpStatusCode.OK;
            resp.Content.Headers.ContentType = new MediaTypeHeaderValue("text/xml");
            resp.Content.Headers.Expires = DateTimeOffset.MaxValue;
            resp.Headers.CacheControl = new CacheControlHeaderValue { MaxAge = TimeSpan.FromDays(30), Public = true };
            return ResponseMessage(resp);

        }
    }
}
