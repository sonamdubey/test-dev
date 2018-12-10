using System;
using System.Linq;
using System.Web.Http;
using Carwale.Notifications.Logs;
using Carwale.Service.Filters;
using Carwale.Entity.CentralizedCacheRefresh;
using Carwale.Interfaces.CentralizedCacheRefresh;

namespace Carwale.Service.Controllers
{
	public class CacheRefreshController : ApiController
	{
        private readonly ICentralizedCacheRefreshBl _cacheRefreshBl;
        public CacheRefreshController(ICentralizedCacheRefreshBl cacheRefreshBl)
		{
            _cacheRefreshBl = cacheRefreshBl;
		}
		[Route("api/centralizedcacherefresh/"), HandleException]
        public IHttpActionResult Post([FromBody]CacheRefreshWrapper refreshWrapper)
		{
            try
            {
                if (refreshWrapper == null || (refreshWrapper.MakeAttribute == null
											   && refreshWrapper.ModelRootAttribute == null
                                               && refreshWrapper.ModelAttribute == null
                                               && refreshWrapper.VersionAttribute == null && refreshWrapper.NewCarFinderAttribute == null))
                {
                    return BadRequest();
                }
                else
                {
                    bool isRefreshed = _cacheRefreshBl.RefreshCentralizedCache(refreshWrapper);
                    if(isRefreshed)
                    {
                        return Ok();
                    }                   
                    return InternalServerError();                  
                }
            }
            catch(Exception ex)
            {
                Logger.LogException(ex);
                return InternalServerError();
            }
		}
	}
}
