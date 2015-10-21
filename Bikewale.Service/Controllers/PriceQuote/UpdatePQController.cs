using Bikewale.DTO.PriceQuote;
using Bikewale.Entities.PriceQuote;
using Bikewale.Interfaces.PriceQuote;
using Bikewale.Notifications;
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
    /// Controller to update the PQ details
    /// </summary>
    public class UpdatePQController : ApiController
    {
        private readonly IPriceQuote _objPQ = null;
        public UpdatePQController(IPriceQuote objPQ)
        {
            _objPQ = objPQ;
        }

        /// <summary>
        /// Updates the Price Quote data for given Price Quote Id
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [ResponseType(typeof(PQUpdateOutput))]
        public IHttpActionResult Post([FromBody]PQUpdateInput input)
        {
            PQUpdateOutput output = null;
            PriceQuoteParametersEntity pqParam = null;
            try
            {
                if (input != null && ((input.PQId > 0) && (input.VersionId > 0)))
                {
                    pqParam = new PriceQuoteParametersEntity();
                    pqParam.VersionId = input.VersionId;
                    output = new PQUpdateOutput();
                    if (_objPQ.UpdatePriceQuote(input.PQId, pqParam))
                    {
                        output.IsUpdated = true;
                    }
                    return Ok(output);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.Service.Controllers.PriceQuote.UpdatePQController.Put");
                objErr.SendMail();
                return InternalServerError();
            }
        }
    }
}
