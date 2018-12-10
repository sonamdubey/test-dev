using Carwale.Cache.Classification;
using AEPLCore.Cache;
using Carwale.DAL.Classification;
using Carwale.Entity.Classification;
using Carwale.Interfaces;
using Carwale.Interfaces.Classification;
using Microsoft.Practices.Unity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using AEPLCore.Cache.Interfaces;

namespace Carwale.Service.Controllers.Classification
{
    /// <summary>
    /// 
    /// </summary>
    public class BodyTypeController : ApiController
    {
        private readonly ICacheManager _cacheProvider;
        private readonly IBodyTypeCache _bodyTypeCache;
        public BodyTypeController(ICacheManager cacheProvider,IBodyTypeCache bodyTypeCache)
        {
            _cacheProvider = cacheProvider;
            _bodyTypeCache = bodyTypeCache;
        }

        /// <summary>
        /// Modified By: Rakesh yadav on 09 Sep 2015
        /// Desc:Resolving dependency injection using UnityBootstraper and UnityResolver
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult Get(string type="all")
        {           
            var bodyTypes = _bodyTypeCache.GetBodyType();
            bodyTypes.ForEach(x => x.LandingUrl = "/api/NewCarSearchResult?Makes=-1&Budget=-1,-1&FuelTypes=-1&BodyTypes=" + x.Id + "&Transmission=-1&SeatingCapacity=-1&EnginePower=-1&ImportantFeatures=-1&PageNo=1&PageSize=10&SortCriteria=-1&SortOrder=-1");

            if (bodyTypes.Count == 0)
                return NotFound();

            return ResponseMessage(new HttpResponseMessage() { Content = new StringContent(JsonConvert.SerializeObject(bodyTypes)) });
        }
    }
}
