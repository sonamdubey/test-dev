using Carwale.Entity.Finance;
using Carwale.Interfaces.Finance;
using Carwale.Notifications;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Linq;
using System.Web.Http;
using Carwale.DTOs.Finance;
using AutoMapper;


namespace Carwale.Service.Controllers.Finance
{
    public class FinanceLinkDataController : ApiController
    {
        private IFinanceLinkDataCache _financeLinkCache;

        public FinanceLinkDataController(IFinanceLinkDataCache financeLinkCache)
        {
            _financeLinkCache = financeLinkCache;
        }

        [HttpGet, Route("api/finance/linkdata/")]
        public IHttpActionResult GetUrlData(int screenId)
        {
            FinanceLinkDto linkData;
            try
            {
                int platformId;

                if (Request.Headers.Contains("sourceID"))
                {
                    int.TryParse(Request.Headers.GetValues("sourceID").First(), out platformId);
                }
                else
                {
                    return BadRequest("platformid is missing");
                }

                linkData = Mapper.Map<FinanceLinkData, FinanceLinkDto>(_financeLinkCache.GetUrlData(platformId, screenId));
                if (linkData != null)
                    return Ok(linkData);
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "FinanceLinkDataController.GetUrlData() ScreenId : " + screenId);
                objErr.LogException();
            }

            return Ok();
        }
    }
}
