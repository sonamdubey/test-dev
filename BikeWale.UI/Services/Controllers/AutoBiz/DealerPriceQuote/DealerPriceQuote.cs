using Bikewale.DAL.AutoBiz;
using Bikewale.DTO.BikeBooking.Make;
using Bikewale.DTO.Location;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.Location;
using Bikewale.Interfaces.AutoBiz;
using Bikewale.Notifications;
using Bikewale.Service.AutoMappers.AutoBiz;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Http;

namespace Bikewale.Service.Controllers.AutoBiz
{
    /// <summary>
    /// Desc: Controller class to host Dealer Pricequote Apis
    /// </summary>
    public class DealerPriceQuoteController : ApiController
    {
        /// <summary>
        /// Written By : Ashish G. Kamble o 10 May 2015
        /// Summary : Function to get the list of cities where bike booking option is available.
        /// </summary>
        /// <returns>If success returns list of cities.</returns>
        [HttpGet]
        public IHttpActionResult GetBikeBookingCities(uint? modelId = null)
        {
            IEnumerable<CityEntityBase> cities = null;
            IEnumerable<CityEntityBaseDTO> objCities = null;
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IDealerPriceQuote, DealerPriceQuoteRepository>();
                    IDealerPriceQuote objPriceQuote = container.Resolve<DealerPriceQuoteRepository>();
                    cities = objPriceQuote.GetBikeBookingCities(modelId);

                    objCities = DealerPriceQuoteMapper.Convert(cities);
                }
            }
            catch (Exception ex)
            {
                //HttpContext.Current.Trace.Warn("GetBikeBookingCities ex : " + ex.Message + ex.Source);
                ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);
               
                return InternalServerError();
            }

            if (objCities != null)
                return Ok(objCities);
            else
                return NotFound();
        }   // End of GetBikeBookingCities

        /// <summary>
        /// Written By : Ashish G. Kamble on 10 May 2015
        /// Summary : Function to get the list of bike makes in the particular city where booking option is available.
        /// </summary>
        /// <param name="cityId">Should be greater than 0.</param>
        /// <returns>Returns list of makes.</returns>
        public IHttpActionResult GetBikeMakesInCity(uint cityId)
        {
            IEnumerable<BikeMakeEntityBase> makes = null;
            IEnumerable<BBMakeBase> objMakes = null;

            if (cityId > 0)
            {
                try
                {
                    using (IUnityContainer container = new UnityContainer())
                    {
                        container.RegisterType<IDealerPriceQuote, DealerPriceQuoteRepository>();
                        IDealerPriceQuote objPriceQuote = container.Resolve<DealerPriceQuoteRepository>();
                        makes = objPriceQuote.GetBikeMakesInCity(cityId);
                        objMakes = DealerPriceQuoteMapper.Convert(makes);
                    }
                }
                catch (Exception ex)
                {
                    //HttpContext.Current.Trace.Warn("GetBikeBookingCities ex : " + ex.Message + ex.Source);
                    ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                   
                    return InternalServerError();
                }

                if (objMakes != null)
                    return Ok(objMakes);
                else
                    return NotFound();
            }
            else
                return BadRequest();
        }   // End of GetBikeMakesInCity

    }   //End of class
}   //End of namespace