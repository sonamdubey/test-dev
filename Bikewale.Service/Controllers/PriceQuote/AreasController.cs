using Bikewale.DAL.BikeBooking;
using Bikewale.DTO.PriceQuote;
using Bikewale.Entities.Location;
using Bikewale.Interfaces.BikeBooking;
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
    /// <summary>
    /// Price Quote Areas Controller
    /// </summary>
    public class AreasController : ApiController
    {        
        /// <summary>
        /// Gets Price Quote Areas
        /// </summary>
        /// <param name="modelId">Model Id</param>
        /// <param name="cityId">City Id</param>
        /// <returns>List of Areas</returns>
        [ResponseType(typeof(IEnumerable<Area>))]
        public IHttpActionResult Get(uint modelId,uint cityId)
        {
            IEnumerable<Area> ctoAreas = null;
            IEnumerable<AreaEntityBase> areas = null;
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IDealerPriceQuote, DealerPriceQuoteRepository>();
                    IDealerPriceQuote dealerRepository = container.Resolve<IDealerPriceQuote>();
                    areas = dealerRepository.GetAreaList(modelId, cityId);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            if (areas != null)
            {
                ctoAreas = from area in areas
                           select new Area()
                           {
                               AreaId = area.AreaId,
                               AreaName = area.AreaName,
                               Latitude = area.Latitude,
                               Longitude = area.Longitude,
                               PinCode = area.PinCode
                           };
                return Ok(ctoAreas);
            }
            else
            {
                return NotFound();
            }
        }
    }
}
