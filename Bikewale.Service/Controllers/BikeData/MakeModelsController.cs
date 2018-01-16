using System.Collections.Generic;
using System.Web.Http;
using Bikewale.DTO.BikeData;
using Bikewale.DTO.Widgets;
using Bikewale.Entities.BikeData;
using Bikewale.Interfaces.BikeData;
using Bikewale.Notifications;
using Bikewale.Service.AutoMappers.BikeData;
using Bikewale.Service.Utilities;

namespace Bikewale.Service.Controllers.BikeData
{
    /// <summary>
    /// Created by  :   Sumit Kate on 13 Sep 2016
    /// Description :   Make Models Controller
    /// </summary>
    public class MakeModelsController : CompressionApiController
    {
        private readonly IBikeMakesCacheRepository _makesRepository;        
        private readonly IBikeModelsCacheRepository<int> _bikeMakeCache = null;
        /// <summary>
        /// Constructor to Initialize cache layer
        /// </summary>
        /// <param name="makesRepository"></param>
        public MakeModelsController(IBikeMakesCacheRepository makesRepository, IBikeModelsCacheRepository<int> bikeMakeCache)
        {
            _makesRepository = makesRepository;
            _bikeMakeCache = bikeMakeCache;
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 13 Sep 2016
        /// Description :   Gets All the makes and their models by calling cache layer
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult Get()
        {
            IEnumerable<MakeModelBase> makeModels = null;
            try
            {
                makeModels = MakeModelEntityMapper.Convert(_makesRepository.GetAllMakeModels());
            }
            catch (System.Exception ex)
            {
                ErrorClass.LogError(ex, "MakeModelsController.Get");
               
                InternalServerError();
            }
            return Ok(makeModels);
        }
         
        [HttpGet, Route("api/popularbikesbybodystyle/{modelId}/{topCount}")]
        public IHttpActionResult GetPopularBikesByBodyStyle(uint modelId, uint topCount, uint cityId)
        {
            IEnumerable<MostPopularBikes> makeModels = null;
            try
            {
                ICollection<MostPopularBikesBase> bikeModelEntity = _bikeMakeCache.GetMostPopularBikesByModelBodyStyle((int)modelId, (int)topCount, cityId);
                makeModels = MakeModelEntityMapper.Convert(bikeModelEntity);
            }
            catch (System.Exception ex)
            {
                ErrorClass.LogError(ex, "MakeModelsController.GetPopularBikesByBodyStyle");               
                InternalServerError();
            }
            return Ok(makeModels);
        }


        [HttpGet, Route("api/popularbikesbymake/{makeId}/{topCount}")]
        public IHttpActionResult GetPopularBikesByMake(uint makeId, uint topCount, uint cityId)
        {
            IEnumerable<MostPopularBikes> makeModels = null;
            try
            {
                IEnumerable<MostPopularBikesBase> bikeModelEntity = _bikeMakeCache.GetMostPopularBikesbyMakeCity(topCount, makeId, cityId);
                makeModels = MakeModelEntityMapper.Convert(bikeModelEntity);
            }
            catch (System.Exception ex)
            {
                ErrorClass.LogError(ex, "MakeModelsController.GetPopularBikesByMake");                
                InternalServerError();
            }
            return Ok(makeModels);
        }

        /// <summary>
        /// Created by : Vivek Singh Tomar on 2nd Jan 2018
        /// Summary    : Get popular bikes 
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("api/popularbikes/")]
        public IHttpActionResult GetMostPopularBikes(int topCount)
        {
            IEnumerable<MostPopularBikes> makeModels = null;
            try
            {
                IEnumerable<MostPopularBikesBase> bikeModelEntity = _bikeMakeCache.GetMostPopularBikes(topCount);
                makeModels = MakeModelEntityMapper.Convert(bikeModelEntity);
            }
            catch (System.Exception ex)
            {
                ErrorClass.LogError(ex, "MakeModelsController.GetPopularBikes");
                InternalServerError();
            }
            return Ok(makeModels);
        }
    }
}
