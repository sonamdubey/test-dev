using Bikewale.DAL.BikeBooking;
using Bikewale.DTO.PriceQuote.Area;
using Bikewale.Entities.Location;
using Bikewale.Interfaces.BikeBooking;
using Bikewale.Interfaces.Location;
using Bikewale.Notifications;
using Bikewale.Service.AutoMappers.PriceQuote.Area;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using System.Linq;

namespace Bikewale.Service.Controllers.PriceQuote.Area
{
    /// <summary>
    /// Price Quote Area List Controller
    /// Author : Sumit Kate
    /// Created : 20 Aug 2015
    /// Modified by :   Sumit Kate on 25 Jan 2016
    /// Description :   Added AreaCache repository interface
    /// </summary>
    public class PQAreaListController : ApiController
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
                if (objAreaList != null && objAreaList.Count() > 0)
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
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.Service.Controllers.PriceQuote.Area.PQAreaListController.Get");
                objErr.SendMail();
                return InternalServerError();
            }
        }
    }
}
