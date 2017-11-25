using Bikewale.DTO.PriceQuote.City;
using Bikewale.Entities.Location;
using Bikewale.Interfaces.Location;
using Bikewale.Notifications;
using Bikewale.Service.AutoMappers.PriceQuote.City;
using Bikewale.Service.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;

namespace Bikewale.Service.Controllers.PriceQuote.City
{
    /// <summary>
    /// Price Quote City Controller
    /// Author : Sumit Kate
    /// Created On : 20 Aug 2015
    /// Modified by :   Sumit Kate on 25 Jan 2016
    /// Description :   Get the PQ Cities from Cache Repository
    /// Modified by :   Sumit Kate on 12 May 2016
    /// Description :   Extend from CompressionApiController instead of ApiController 
    /// </summary>
    public class PQCityListController : CompressionApiController//ApiController
    {
        private readonly ICity _cityRepository = null;
        private readonly ICityCacheRepository _cityCache = null;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="cityRepository"></param>
        /// <param name="cityCache"></param>
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

                if (objCityList != null && objCityList.Any())
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
                ErrorClass.LogError(ex, "Exception : Bikewale.Service.Controllers.PriceQuote.City.PQCityListController.Get");
               
                return InternalServerError();
            }
        }


        /// <summary>
        /// Returns the Price Quote City List by Model Id
        /// </summary>
        /// <param name="modelId">Model Id. Should be Positive Integer</param>
        /// <returns>List of Cities</returns>
        [ResponseType(typeof(Bikewale.DTO.PriceQuote.City.v2.PQCityList)), Route("api/v2/PQCityList/")]
        public IHttpActionResult GetV2(uint modelId)
        {
            IEnumerable<CityEntityBase> objCityList = null;
            Bikewale.DTO.PriceQuote.City.v2.PQCityList objDTOCityList = null;
            try
            {
                objCityList = _cityCache.GetPriceQuoteCities(modelId);

                if (objCityList != null && objCityList.Any())
                {
                    // Auto map the properties
                    objDTOCityList = new Bikewale.DTO.PriceQuote.City.v2.PQCityList();
                    objDTOCityList.Cities = PQCityListMapper.ConvertV2(objCityList);

                    objCityList = null;

                    //return OkCompressed(objDTOCityList);
                    return Ok(objDTOCityList);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Exception : Bikewale.Service.Controllers.PriceQuote.City.PQCityListController.Get");
               
                return InternalServerError();
            }
        }
    }
}
