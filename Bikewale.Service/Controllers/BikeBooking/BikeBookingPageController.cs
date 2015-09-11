using Bikewale.DTO.PriceQuote.BikeBooking;
using Bikewale.Entities.PriceQuote;
using Bikewale.Interfaces.BikeBooking;
using Bikewale.Notifications;
using Bikewale.Service.AutoMappers.Bikebooking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Bikewale.Service.Controllers.BikeBooking
{
    /// <summary>
    /// Bike Booking Page Controller
    /// Author  :   Sumit Kate
    /// Created On  :   10 Sept 2015
    /// </summary>
    public class BikeBookingPageController : ApiController
    {
        IDealerPriceQuote _objDealerPriceQuote = null;
        /// <summary>
        /// Constructor
        /// </summary>
        public BikeBookingPageController(IDealerPriceQuote objDealerPriceQuote)
        {
            _objDealerPriceQuote = objDealerPriceQuote;
        }

        /// <summary>
        /// Get the Bike Booking Page details
        /// </summary>
        /// <param name="cityId"></param>
        /// <param name="versionId"></param>
        /// <param name="dealerId"></param>
        /// <returns></returns>
        public IHttpActionResult Get(uint cityId, uint versionId, uint dealerId)
        {
            BookingPageDetailsEntity objBookingPageDetailsEntity = null;
            BookingPageDetailsDTO objBookingPageDetailsDTO = null;
            try
            {
                objBookingPageDetailsEntity = _objDealerPriceQuote.FetchBookingPageDetails(cityId, versionId, dealerId);
                if (objBookingPageDetailsEntity != null)
                {
                    objBookingPageDetailsDTO = BookingPageDetailsEntityMapper.Convert(objBookingPageDetailsEntity);
                    return Ok(objBookingPageDetailsDTO);
                }
                else
                {
                    return NotFound();
                }                
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.Service.Controllers.PriceQuote.BikeBookingPageController.Get");
                objErr.SendMail();
                return InternalServerError();
            }            
        }
    }
}
