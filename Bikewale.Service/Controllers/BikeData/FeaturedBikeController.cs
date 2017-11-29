using Bikewale.DTO.BikeData;
using Bikewale.Entities.BikeData;
using Bikewale.Interfaces.BikeData;
using Bikewale.Notifications;
using Bikewale.Service.AutoMappers.BikeData;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace Bikewale.Service.Controllers.BikeData
{
    /// <summary>
    /// Featured Bike Controller
    /// Created By : Sadhana Upadhyay on 21 Aug 2015
    /// </summary>
    public class FeaturedBikeController : ApiController
    {
        private readonly IBikeModelsRepository<BikeModelEntity, int> _modelRepository = null;
        public FeaturedBikeController(IBikeModelsRepository<BikeModelEntity, int> modelRepository)
        {
            _modelRepository = modelRepository;
        }
        #region GetFeaturedBikeList PopulateWhere
        /// <summary>
        /// Created By : Sadhana Upadhyay on 21 aUG 2015
        /// Summary : Get Featured Bike list
        /// </summary>
        /// <param name="topCount">Top Count</param>
        /// <returns></returns>
        public IHttpActionResult Get(uint topCount)
        {
            FeaturedBikeList FeaturedList = new FeaturedBikeList();
            List<FeaturedBikeEntity> objFeature = null;
            try
            {
                objFeature = _modelRepository.GetFeaturedBikes(topCount);                
                FeaturedList.FeaturedBike = FeaturedBikeListMapper.Convert(objFeature);
                
                if (objFeature != null && objFeature.Count > 0)
                {
                    objFeature.Clear();
                    objFeature = null;

                    return Ok(FeaturedList);
                }
                else
                    return NotFound();
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Exception : Bikewale.Service.BikeDiscover.GetFeaturedBikeList");
               
                return InternalServerError();
            }
        }
        #endregion
    }
}