using Carwale.Cache.CarData;
using AEPLCore.Cache;
using Carwale.DAL.CarData;
using Carwale.Entity.CarData;
using Carwale.Interfaces;
using Carwale.Interfaces.CarData.RecentLaunchedCar;
using Carwale.Notifications;
using Microsoft.Practices.Unity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace Carwale.Service.Controllers.CarData
{
    public class RecentLaunchedCarController : ApiController
    {
        private readonly IRecentLaunchedCarCacheRepository _recentCarLaunchCacheRepo;

        public RecentLaunchedCarController(IRecentLaunchedCarCacheRepository recentCarLaunchCacheRepo)
        {
            _recentCarLaunchCacheRepo = recentCarLaunchCacheRepo;
        }

        /// <summary>
        /// api for recent launched car list with http response codes
        /// written by Natesh Kumar on 1/10/2014
        /// Modified by: Rakesh Yadav On 02 Sep 2015
        /// desc:Resolving dependency injection using UnityBootstraper and UnityResolver and send status code using web api 2
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult RecentLaunchedCar()
        {
            RecentLaunchObject recentLaunchedCarObj = new RecentLaunchObject()
            {
                newLaunches = _recentCarLaunchCacheRepo.GetRecentLaunchedCars()
            };

            if (recentLaunchedCarObj.newLaunches.Count == 0)
                return NotFound();

            return Ok(recentLaunchedCarObj);
        }
    }
}
