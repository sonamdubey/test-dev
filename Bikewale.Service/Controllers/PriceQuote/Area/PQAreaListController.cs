using Bikewale.DAL.BikeBooking;
using Bikewale.DTO.PriceQuote.Area;
using Bikewale.Entities.Location;
using Bikewale.Interfaces.BikeBooking;
using Bikewale.Notifications;
using Bikewale.Service.AutoMappers.PriceQuote.Area;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace Bikewale.Service.Controllers.PriceQuote.Area
{
    /// <summary>
    /// Price Quote Area List Controller
    /// Author : Sumit Kate
    /// Created : 20 Aug 2015
    /// </summary>
    public class PQAreaListController : ApiController
    {
        private readonly IDealerPriceQuote _dealerRepository = null;
        public PQAreaListController(IDealerPriceQuote dealerRepository)
        {
            _dealerRepository = dealerRepository;
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
            List<AreaEntityBase> objAreaList = null;
            PQAreaList objDTOAreaList = null;
            try
            {
                objAreaList = _dealerRepository.GetAreaList(modelId, cityId);
                if (objAreaList != null && objAreaList.Count > 0)
                {
                    // Auto map the properties
                    objDTOAreaList = new PQAreaList();
                    objDTOAreaList.Areas = PQAreaListMapper.Convert(objAreaList);
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
