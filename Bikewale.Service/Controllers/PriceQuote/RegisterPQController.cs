using Bikewale.DTO.PriceQuote;
using Bikewale.Entities.BikeBooking;
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
    /// Created by  :   Sumit Kate on 08 Feb 2016
    /// Description :   Register PQ
    /// </summary>
    public class RegisterPQController : ApiController
    {
        private readonly IPriceQuote _objIPQ = null;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="objIPQ"></param>
        public RegisterPQController(IPriceQuote objIPQ)
        {
            _objIPQ = objIPQ;
        }
        /// <summary>
        /// Registers a new price quote
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [ResponseType(typeof(PQOutputEntity))]
        public IHttpActionResult Post([FromBody]PQInput input)
        {
            string response = string.Empty;
            Bikewale.DTO.PriceQuote.PQOutput objPQ = null;
            UInt64 pqId = 0;
            try
            {
                if (input.DealerId.HasValue && input.CityId > 0 && input.VersionId > 0)
                {
                    PriceQuoteParametersEntity objPQEntity = new PriceQuoteParametersEntity();
                    objPQEntity.CityId = input.CityId;
                    objPQEntity.AreaId = input.AreaId > 0 ? input.AreaId : 0;
                    objPQEntity.ClientIP = input.ClientIP;
                    objPQEntity.SourceId = Convert.ToUInt16(input.SourceType);
                    objPQEntity.ModelId = input.ModelId;
                    objPQEntity.VersionId = input.VersionId;
                    objPQEntity.UTMA = Request.Headers.Contains("utma") ? Request.Headers.GetValues("utma").FirstOrDefault() : String.Empty;
                    objPQEntity.UTMZ = Request.Headers.Contains("utmz") ? Request.Headers.GetValues("utmz").FirstOrDefault() : String.Empty;
                    objPQEntity.DeviceId = input.DeviceId;
                    objPQEntity.PQLeadId = input.PQLeadId;
                    objPQEntity.VersionId = input.VersionId;

                    pqId = _objIPQ.RegisterPriceQuote(objPQEntity); 
                }

                if (pqId > 0)
                {
                    objPQ = new PQOutput();
                    objPQ.DealerId = input.DealerId.Value;
                    objPQ.PQId = pqId;
                    objPQ.VersionId = input.VersionId;
                    return Ok(objPQ);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, String.Format("Exception : Bikewale.Service.Controllers.PriceQuote.RegisterPQController.Post({0})", Newtonsoft.Json.JsonConvert.SerializeObject(input)));
                objErr.SendMail();
                return InternalServerError();
            }
        }
    }
}
