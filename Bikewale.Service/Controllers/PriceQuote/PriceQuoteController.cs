﻿using Bikewale.DTO.PriceQuote;
using Bikewale.Entities.BikeBooking;
using Bikewale.Entities.PriceQuote;
using Bikewale.Interfaces.BikeBooking;
using Bikewale.Notifications;
using Bikewale.Service.AutoMappers.PriceQuote;
using Bikewale.Service.Utilities;
using System;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
namespace Bikewale.Service.Controllers.PriceQuote
{
    /// <summary>
    /// Price Quote Controller
    /// Author  :   Sumit Kate
    /// Created On  :   21 Aug 2015
    /// </summary>
    public class PriceQuoteController : CompressionApiController//ApiController
    {
        private readonly IDealerPriceQuote _objIPQ = null;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="objIPQ"></param>
        public PriceQuoteController(IDealerPriceQuote objIPQ)
        {
            _objIPQ = objIPQ;
        }
        /// <summary>
        /// Bikewale Price Quote and Dealer Price Quote
        /// Modified By : Sadhana Upadhyay on 29 Dec 2015
        /// Summary : To capture device id, utma, utmz, Pq lead id etc.
        /// </summary>
        /// <param name="input">Entity contains the required details to get the price quote details</param>
        /// <returns></returns>
        [ResponseType(typeof(PQOutputEntity))]
        public IHttpActionResult Post([FromBody]PQInput input)
        {
            string response = string.Empty;
            Bikewale.DTO.PriceQuote.PQOutput objPQ = null;
            PQOutputEntity objPQOutput = null;
            try
            {
                PriceQuoteParametersEntity objPQEntity = new PriceQuoteParametersEntity();
                objPQEntity.CityId = input.CityId;
                objPQEntity.AreaId = input.AreaId > 0 ? input.AreaId : 0;
                objPQEntity.ClientIP = input.ClientIP;
                objPQEntity.SourceId = Convert.ToUInt16(input.SourceType);
                objPQEntity.ModelId = input.ModelId;
                objPQEntity.VersionId = input.VersionId;
                objPQEntity.UTMA = Request.Headers.Contains("utma") ? Request.Headers.GetValues("utma").FirstOrDefault() : String.Empty;
                objPQEntity.UTMZ = Request.Headers.Contains("_bwutmz") ? Request.Headers.GetValues("_bwutmz").FirstOrDefault() : String.Empty;
                objPQEntity.DeviceId = input.DeviceId;
                objPQEntity.PQLeadId = input.PQLeadId;
                objPQEntity.RefPQId = input.RefPQId;

                objPQOutput = _objIPQ.ProcessPQ(objPQEntity);

                if (objPQOutput != null)
                {
                    objPQ = PQOutputMapper.Convert(objPQOutput);
                    return Ok(objPQ);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.Service.Controllers.PriceQuote.PriceQuoteController.Post");
                objErr.SendMail();
                return InternalServerError();
            }
        }
    }
}