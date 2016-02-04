using Bikewale.DTO.BikeBooking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace Bikewale.Service.Controllers.BikeBooking
{
    public class BikeBookingListingController : ApiController
    {
        [ResponseType(typeof(IEnumerable<BikeBookingListingDTO>))]
        public IHttpActionResult Get([FromBody]BookingListingFilterDTO filter)
        {
            return Ok();
        }
    }
}
