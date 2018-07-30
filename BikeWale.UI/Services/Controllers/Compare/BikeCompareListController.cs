using Bikewale.DTO.Compare;
using Bikewale.Entities.Compare;
using Bikewale.Interfaces.Compare;
using Bikewale.Notifications;
using Bikewale.Service.AutoMappers.Compare;
using Bikewale.Service.Utilities;
using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Description;

namespace Bikewale.Service.Controllers.Compare
{
    /// <summary>
    /// Bike Compare List Controller
    /// Author  :   Sumit Kate
    /// Created On : 27 Aug 2015
    /// Modified By : Lucky Rathore on 06 Nov. 2015
    /// Description : cache functionality added.  
    /// Modified by :   Sumit Kate on 18 May 2016
    /// Description :   Extend from CompressionApiController instead of ApiController 
    /// </summary>
    public class BikeCompareListController : CompressionApiController//ApiController
    {
        private readonly IBikeCompare _bikeCompare = null;
        private readonly IBikeCompareCacheRepository _cache = null;

        /// <summary>
        /// Modified By : Lucky Rathore on 06 Nov. 2015
        /// Desctption : add 'IBikeCompareCacheRepository cache' parameter 
        /// </summary>
        /// <param name="bikeCompare"></param>
        /// <param name="cache"></param>
        public BikeCompareListController(IBikeCompare bikeCompare, IBikeCompareCacheRepository cache)
        {
            _bikeCompare = bikeCompare;
            _cache = cache;
        }

        /// <summary>
        /// Gets the Top 'n' Bike Compare List
        /// Modified by : Lucky Rathore on 06 Nov. 2015
        /// Description : compare List called by cache.
        /// </summary>
        /// <param name="topCount">Top count</param>
        /// <returns></returns>
        [ResponseType(typeof(IEnumerable<TopBikeCompareBase>))]
        public IHttpActionResult Get(UInt16 topCount)
        {
            IEnumerable<TopBikeCompareBase> topBikeComapreList = null;
            IEnumerable<TopBikeCompareDTO> dto = null;
            try
            {
                topBikeComapreList = _bikeCompare.CompareList(topCount);
                if (topBikeComapreList != null)
                {
                    dto = TopBikeCompareBaseMapper.Convert(topBikeComapreList);
                    return Ok(dto);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Exception : Bikewale.Service.Controllers.Compare.BikeCompareListController.Get");
               
                return InternalServerError();
            }
        }
    }
}
