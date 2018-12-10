using AutoMapper;
using Carwale.DTOs.CarData;
using Carwale.Entity.CarData;
using Carwale.Interfaces.CarData;
using Carwale.Notifications;
using Carwale.Notifications.Logs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;

namespace Carwale.Service.Controllers.CarData
{
    /// <summary>
    /// Created by  : Kirtan Shetty
    /// Date        : August 1, 2014
    /// Description : Gets the roots of a particular make
    /// </summary>
    public class CarRootsDataController : ApiController
    {
        private readonly ICarModelRootsCacheRepository _modelRootCacheRepo;
        private readonly ICarRoots _carRootsBl;

        public CarRootsDataController(ICarModelRootsCacheRepository modelRootCacheRepo, ICarRoots carRootsBl)
        {
            _modelRootCacheRepo = modelRootCacheRepo;
            _carRootsBl = carRootsBl;
        }

        /// Modified by: rakesh yadav on 1 Sep Aug 2015
        /// Desc:Resolving dependency injection using UnityBootstraper and UnityResolver
        [EnableCors("https://www-carwale-com.cdn.ampproject.org, https://cdn.ampproject.org, https://www-carwale-com.amp.cloudflare.com", "*", "GET")]
        public IHttpActionResult GetRootsByMakeId(int make)
        {
            List<RootBase> carRootList = new List<RootBase>();
            carRootList = _modelRootCacheRepo.GetRootsByMake(make);

            if (carRootList.Count == 0)
                return NotFound();

            return Ok(carRootList);
        }

        [HttpGet, Route("api/root/{rootId}/models/")]
        public IHttpActionResult GetModelsByRootAndYear(int rootId, int year)
        {
            try
            {
                List<ModelsByRootAndYear> carModelsByRoot = new List<ModelsByRootAndYear>();
                carModelsByRoot = _modelRootCacheRepo.GetModelsByRootAndYear(rootId, year);

                var results = Mapper.Map<List<ModelsByRootAndYear>, List<ModelsByRootAndYearDTO>>(carModelsByRoot);

                return Ok(results);
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "CarRootsDataController.GetModelsByRootAndYear()- RootId : " + rootId);
                return InternalServerError();
            }
        }

        [HttpGet, Route("api/roots/{rootId:int:min(1)}/models/years/")]
        public IHttpActionResult GetYearsByRootId(int rootId)
        {
            try
            {
                if (rootId > 0)
                {
                    return Ok(_carRootsBl.GetYearsByRootId(rootId));
                }
                else
                {
                    return BadRequest("RootId is missing");
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, string.Format("CarRootsDataController.GetYearsByRootId({0}): ", rootId));
                objErr.LogException();
                return InternalServerError();
            }
        }

        [Route("api/roots")]
        [HttpGet]
        public IHttpActionResult GetRoots(string ids = null)
        {
            try
            {
                IEnumerable<RootBase> rootDetails = null;
                if (!string.IsNullOrEmpty(ids))
                {
                    int temp = 0;
                    ids = string.Join(",", ids.Split(',').Where(s => int.TryParse(s, out temp)).Distinct().Select(x => temp).OrderBy(x => x)); //convert ids to comma separated sorted ids
                    rootDetails = _modelRootCacheRepo.GetRoots(ids);
                }
                if (rootDetails != null )
                {
                    return Json(rootDetails);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return InternalServerError();
            }
        }
    }
}
