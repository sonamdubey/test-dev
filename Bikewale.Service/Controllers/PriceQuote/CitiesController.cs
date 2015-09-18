using Bikewale.DAL.Location;
using Bikewale.DTO.PriceQuote;
using Bikewale.Entities.Location;
using Bikewale.Interfaces.Location;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace Bikewale.Service.Controllers.PriceQuote
{
    public class CitiesController : ApiController
    {
        /// <summary>
        /// Gets the Price Quote Cities
        /// </summary>
        /// <param name="modelId">Model Id</param>
        /// <returns>List of Cities</returns>
        [ResponseType(typeof(IEnumerable<City>))]
        public IHttpActionResult Get(uint modelId)
        {
            IEnumerable<City> ctoCity = null;
            IEnumerable<CityEntityBase> cities = null;
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<ICity, CityRepository>();
                    ICity cityRepository = container.Resolve<ICity>();
                    cities = cityRepository.GetPriceQuoteCities(modelId);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            if (cities != null)
            {
                ctoCity = from city in cities
                          select new City()
                          {
                                CityId = city.CityId,
                                CityMaskingName = city.CityMaskingName,
                                CityName = city.CityName
                          };
                return Ok(ctoCity);
            }
            else
            {
                return NotFound();
            }
        }
    }
}
