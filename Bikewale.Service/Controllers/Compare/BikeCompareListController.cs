﻿using Bikewale.DAL.Compare;
using Bikewale.DTO.Compare;
using Bikewale.Entities.Compare;
using Bikewale.Interfaces.Compare;
using Bikewale.Notifications;
using Bikewale.Service.AutoMappers.Compare;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
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
    /// </summary>
    public class BikeCompareListController : ApiController
    {
        private readonly IBikeCompare _bikeCompare = null;
        private readonly IBikeCompareCacheRepository _cache = null;

        /// <summary>
        /// Modified By : Lucky Rathore on 06 Nov. 2015
        /// Desctption : add 'IBikeCompareCacheRepository cache' parameter 
        /// </summary>
        /// <param name="bikeCompare"></param>
        /// <param name="cache"></param>
        public BikeCompareListController(IBikeCompare bikeCompare,IBikeCompareCacheRepository cache)
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
                    return Ok(topBikeComapreList);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.Service.Controllers.Compare.BikeCompareListController.Get");
                objErr.SendMail();
                return InternalServerError();
            }
        }
    }
}
