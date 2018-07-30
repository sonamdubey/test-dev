using Bikewale.DTO.PriceQuote.Make;
using Bikewale.Entities.BikeData;
using Bikewale.Interfaces.BikeData;
using Bikewale.Notifications;
using Bikewale.Service.AutoMappers.PriceQuote.Make;
using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Description;

namespace Bikewale.Service.Controllers.PriceQuote.Make
{
    /// <summary>
    /// Price Quote Make List
    /// Author  : Sumit Kate
    /// Created on : 20 Aug 2015
    /// </summary>
    public class PQMakeListController : ApiController
    {
        private readonly IBikeMakes<BikeMakeEntity, int> _makesRepository = null;

        public PQMakeListController(IBikeMakes<BikeMakeEntity, int> makesRepository)
        {
            _makesRepository = makesRepository;
        }
        /// <summary>
        /// Gets Makes List
        /// </summary>
        /// <returns>Make List</returns>
        [ResponseType(typeof(PQMakeList))]
        public IHttpActionResult Get()
        {
            List<BikeMakeEntityBase> objMakeList = null;
            PQMakeList objDTOMakeList = null;
            try
            {
                objMakeList = _makesRepository.GetMakesByType(EnumBikeType.PriceQuote);
                if (objMakeList != null && objMakeList.Count > 0)
                {
                    // Auto map the properties
                    objDTOMakeList = new PQMakeList();
                    objDTOMakeList.Makes = PQMakeListMapper.Convert(objMakeList);

                    objMakeList.Clear();
                    objMakeList = null;

                    return Ok(objDTOMakeList);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Exception : Bikewale.Service.Controllers.PriceQuote.Make.PQMakeListController.Get");
               
                return InternalServerError();
            }
        }
    }
}
