using BikewaleOpr.Entity;
using BikewaleOpr.Interface;
using System.Collections.Generic;
using System.Web.Http;

namespace BikewaleOpr.Service.Controllers
{
    /// <summary>
    /// Created by  :   Sumit Kate on 08 May 2018
    /// Description :   Cache Controller
    /// </summary>
    public class CacheController : ApiController
    {
        private readonly IBWCache _cache;
        public CacheController(IBWCache cache)
        {
            _cache = cache;
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 08 May 2018
        /// Description :   Cache clear api
        /// This api calls the BL which clears all the cache associated with the content passed in url
        /// </summary>
        /// <param name="content">Type of cache</param>
        /// <returns></returns>
        [HttpPost, Route("api/cache/{content}/clear/")]
        public IHttpActionResult Clear(CacheContents content, [FromBody] IDictionary<string, string> values)
        {
            _cache.Clear(content, values);
            return Ok();
        }
    }
}
