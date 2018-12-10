using AutoMapper;
using Carwale.DTOs.ES;
using Carwale.Entity.ES;
using Carwale.Interfaces.ES;
using Carwale.Notifications.Logs;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace Carwale.Service.Controllers.ES
{
    public class SponsoredNavigationController : ApiController
    {
        private readonly ISponsoredNavigationCache _navigationCache;
        public SponsoredNavigationController(ISponsoredNavigationCache navigationCache)
        {
            _navigationCache = navigationCache;
        }

        [HttpGet, Route("api/sponsored/navigation/{sectionId:int}/")]
        public IHttpActionResult GetSponsoredNavigationData(int sectionId, int platformId)
        {
            try
            {
                var sponsoredNavigationData = Mapper.Map<List<SponsoredNavigation>, List<SponsoredNavigationDto>>(_navigationCache.GetSponsoredNavigationData(sectionId, platformId));
                return Ok(sponsoredNavigationData);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return InternalServerError();
            }            
        }
    }
}
