using Bikewale.DAL.Location;
using Bikewale.DTO.PriceQuote.City;
using Bikewale.Entities.Location;
using Bikewale.Interfaces.Location;
using Bikewale.Notifications;
using Bikewale.Service.AutoMappers.PriceQuote.City;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using System.Linq;

namespace Bikewale.Service.Controllers.PriceQuote.City
{
    /// <summary>
    /// Price Quote City Controller
    /// Author : Sumit Kate
    /// Created On : 20 Aug 2015
    /// Modified by :   Sumit Kate on 25 Jan 2016
    /// Description :   Get the PQ Cities from Cache Repository
    /// </summary>
    public class PQCityListController : ApiController
    {
        private readonly ICity _cityRepository = null;
        private readonly ICityCacheRepository _cityCache = null;
        public PQCityListController(ICity cityRepository, ICityCacheRepository cityCache)
        {
            _cityRepository = cityRepository;
            _cityCache = cityCache;
        }
        /// <summary>
        /// Returns the Price Quote City List by Model Id
        /// </summary>
        /// <param name="modelId">Model Id. Should be Positive Integer</param>
        /// <returns>List of Cities</returns>
        [ResponseType(typeof(PQCityList))]
        public IHttpActionResult Get(uint modelId)
        {
            IEnumerable<CityEntityBase> objCityList = null;
            PQCityList objDTOCityList = null;
            try
            {
                objCityList = _cityCache.GetPriceQuoteCities(modelId);

                if (objCityList != null && objCityList.Count() > 0)
                {
                    // Auto map the properties
                    objDTOCityList = new PQCityList();
                    objDTOCityList.Cities = PQCityListMapper.Convert(objCityList);
                    
                    objCityList = null;

                    return Ok(objDTOCityList);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.Service.Controllers.PriceQuote.City.PQCityListController.Get");
                objErr.SendMail();
                return InternalServerError();
            }
        }
    }
}
