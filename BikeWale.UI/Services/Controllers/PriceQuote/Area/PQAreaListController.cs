using Bikewale.DTO.PriceQuote.Area;
using Bikewale.Entities.Location;
using Bikewale.Interfaces.BikeBooking;
using Bikewale.Interfaces.Location;
using Bikewale.Notifications;
using Bikewale.Service.AutoMappers.PriceQuote.Area;
using Bikewale.Service.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;

namespace Bikewale.Service.Controllers.PriceQuote.Area
{
    /// <summary>
    /// Price Quote Area List Controller
    /// Author : Sumit Kate
    /// Created : 20 Aug 2015
    /// Modified by :   Sumit Kate on 25 Jan 2016
    /// Description :   Added AreaCache repository interface
    /// Modified by :   Sumit Kate on 12 May 2016
    /// Description :   Extend from CompressionApiController instead of ApiController 
    /// </summary>
    public class PQAreaListController : CompressionApiController//ApiController
    {
        private readonly IDealerPriceQuote _dealerRepository = null;
        private readonly IAreaCacheRepository _areaCache = null;

        /// <summary>
        /// Modified by :   Sumit Kate on 25 Jan 2016
        /// Description :   Initialize the Area Cache Repository with Parameterized Constructor
        /// </summary>
        /// <param name="dealerRepository"></param>
        /// <param name="areaCache"></param>
        public PQAreaListController(IDealerPriceQuote dealerRepository, IAreaCacheRepository areaCache)
        {
            _dealerRepository = dealerRepository;
            _areaCache = areaCache;
        }

        /// <summary>
        /// List of Price Quote Areas
        /// </summary>
        /// <param name="modelId">Model ID</param>
        /// <param name="cityId">City Id</param>
        /// <returns>Area List</returns>
        [ResponseType(typeof(PQAreaList))]
        public IHttpActionResult Get(uint modelId, uint cityId)
        {
            IEnumerable<AreaEntityBase> objAreaList = null;
            PQAreaList objDTOAreaList = null;
            try
            {
                objAreaList = _areaCache.GetAreaList(modelId, cityId);
                if (objAreaList != null && objAreaList.Any())
                {
                    // Auto map the properties
                    objDTOAreaList = new PQAreaList();
                    objDTOAreaList.Areas = PQAreaListMapper.Convert(objAreaList);
                    objAreaList = null;

                    return Ok(objDTOAreaList);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Exception : Bikewale.Service.Controllers.PriceQuote.Area.PQAreaListController.Get");
               
                return InternalServerError();
            }
        }

        /// <summary>
        /// List of Price Quote Areas
        /// Modified By : Sushil Kumar on 12th Feb 2016
        /// Description : API versioning to send minimal area api data
        /// </summary>
        /// <param name="modelId">Model ID</param>
        /// <param name="cityId">City Id</param>
        /// <returns>Area List</returns>
        [ResponseType(typeof(Bikewale.DTO.PriceQuote.Area.v2.PQAreaList)), Route("api/v2/PQAreaList/")]
        public IHttpActionResult GetV2(uint modelId, uint cityId)
        {
            IEnumerable<AreaEntityBase> objAreaList = null;
            Bikewale.DTO.PriceQuote.Area.v2.PQAreaList objDTOAreaList = null;
            try
            {
                objAreaList = _areaCache.GetAreaList(modelId, cityId);
                if (objAreaList != null && objAreaList.Any())
                {
                    // Auto map the properties
                    objDTOAreaList = new DTO.PriceQuote.Area.v2.PQAreaList();
                    objDTOAreaList.Areas = PQAreaListMapper.ConvertV2(objAreaList);
                    objAreaList = null;

                    return Ok(objDTOAreaList);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Exception : Bikewale.Service.Controllers.PriceQuote.Area.PQAreaListController.Get");
               
                return InternalServerError();
            }
        }
    }
}
