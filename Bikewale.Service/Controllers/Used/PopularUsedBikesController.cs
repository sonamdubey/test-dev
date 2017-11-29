using Bikewale.DTO.UsedBikes;
using Bikewale.Entities.UsedBikes;
using Bikewale.Interfaces.UsedBikes;
using Bikewale.Notifications;
using Bikewale.Service.AutoMappers.UsedBikes;
using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Description;

namespace Bikewale.Service.Controllers.UsedBikes
{
    /// <summary>
    /// To Get Popular used Bikes Details
    /// Author : Sushil Kumar
    /// Created On : 28th August 2015
    /// </summary>
    public class PopularUsedBikesController : ApiController
    { 
        private readonly  IUsedBikesRepository _usedBikesRepo = null;
        private readonly IPopularUsedBikesCacheRepository _cache = null;
        public PopularUsedBikesController(IUsedBikesRepository usedBikesRepo, IPopularUsedBikesCacheRepository cache)
        {
            _usedBikesRepo = usedBikesRepo;
            _cache = cache;
        }
        
        #region Popular Used Bikes List
        /// <summary>
        /// To get popular used bikes 
        /// </summary>
        /// <param name="topCount">Number of Records to be fetched</param>
        /// <param name="cityId">Optional</param>
        /// <returns></returns>
        [ResponseType(typeof(PopularUsedBikesEntity))]
        public IHttpActionResult Get(uint topCount,int? cityId = null)
        {
            IEnumerable<PopularUsedBikesEntity> objUsedBikesList = null;
            IEnumerable<PopularUsedBikesBase> objDTOUsedBikesList = null;
            try
            {
                objUsedBikesList = _cache.GetPopularUsedBikes(topCount, cityId);
                    //_usedBikesRepo.GetPopularUsedBikes(topCount, cityId);

                if (objUsedBikesList != null)
                {
                    objDTOUsedBikesList = new List<PopularUsedBikesBase>();
                    objDTOUsedBikesList = PopularUsedBikesMapper.Convert(objUsedBikesList);

                    objUsedBikesList = null;

                    return Ok(objDTOUsedBikesList);
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Exception : Bikewale.Service.UsedBikes.PopularUsedBikesController");
               
                return InternalServerError();
            }
            return NotFound();
        }   // Get PopularUsedBikes 
        #endregion

    }
}
