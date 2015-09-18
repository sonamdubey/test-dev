using Bikewale.DTO.PriceQuote;
using Bikewale.Interfaces.BikeBooking;
using Bikewale.Notifications;
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
    /// Price Quote Bike Color Controller
    /// Author  :   Sumit Kate
    /// Created On  : 21 Aug 2015
    /// </summary>
    public class PQBikeColorController : ApiController
    {
        private readonly IDealerPriceQuote _objDealer = null;
        public PQBikeColorController(IDealerPriceQuote objDealer)
        {
            _objDealer = objDealer;
        }
        /// <summary>
        /// To update the Bike Color
        /// </summary>
        /// <param name="input">PQBikeColorInput Entity</param>
        /// <returns></returns>
        [ResponseType(typeof(PQBikeColorOutput))]
        public IHttpActionResult Post([FromBody] PQBikeColorInput input)
        {
            PQBikeColorOutput output = null;
            bool isUpdated = false;
            try
            {
                isUpdated = _objDealer.UpdatePQBikeColor(input.ColorId, input.PQId);
                if (isUpdated)
                {
                    output = new PQBikeColorOutput();
                    output.IsUpdated = isUpdated;
                    return Ok(output);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.Service.Controllers.PriceQuote.PQBikeColorController.Post");
                objErr.SendMail();
                return InternalServerError();
            }
        }
    }
}
