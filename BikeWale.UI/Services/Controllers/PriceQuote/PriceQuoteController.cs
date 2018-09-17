using Bikewale.DTO.PriceQuote;
using Bikewale.DTO.PriceQuote.Version;
using Bikewale.Entities.BikeBooking;
using Bikewale.Entities.PriceQuote;
using Bikewale.Interfaces.BikeBooking;
using Bikewale.Interfaces.PriceQuote;
using Bikewale.Notifications;
using Bikewale.Service.AutoMappers.PriceQuote;
using Bikewale.Service.Utilities;
using System;
using System.Collections.Generic;
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
        private readonly IPriceQuote _objPQ;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="objIPQ"></param>
        public PriceQuoteController(IDealerPriceQuote objIPQ, IPriceQuote objPQ)
        {
            _objIPQ = objIPQ;
            _objPQ = objPQ;
        }
        /// <summary>
        /// Bikewale Price Quote and Dealer Price Quote
        /// Modified By : Sadhana Upadhyay on 29 Dec 2015
        /// Summary : To capture device id, utma, utmz, Pq lead id etc.
        /// </summary>
        /// <param name="input">Entity contains the required details to get the price quote details</param>
        /// <returns></returns>
        [ResponseType(typeof(PQOutputEntity)), Route("api/PriceQuote/")]
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
                objPQEntity.UTMZ = Request.Headers.Contains("utmz") ? Request.Headers.GetValues("utmz").FirstOrDefault() : String.Empty;
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
                ErrorClass.LogError(ex, "Exception : Bikewale.Service.Controllers.PriceQuote.PriceQuoteController.Post");
               
                return InternalServerError();
            }
        }

        /// <summary>
        /// Author    : Kartik on 20 jun 2018 for price quote changes added
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [ResponseType(typeof(Bikewale.Entities.BikeBooking.v2.PQOutputEntity)), Route("api/v2/PriceQuote/")]
        public IHttpActionResult PostV2([FromBody]Bikewale.DTO.PriceQuote.v4.PQInput input)
        {
            string response = string.Empty;
            Bikewale.DTO.PriceQuote.v3.PQOutput objPQ = null;
            Bikewale.Entities.BikeBooking.v2.PQOutputEntity objPQOutput = null;
            try
            {
                Entities.PriceQuote.v2.PriceQuoteParametersEntity objPQEntity = new Entities.PriceQuote.v2.PriceQuoteParametersEntity();
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
                objPQEntity.RefPQId = input.RefPQId;

                objPQOutput = _objIPQ.ProcessPQV3(objPQEntity);

                if (objPQOutput != null)
                {
                    objPQ = PQOutputMapper.ConvertV3(objPQOutput);
                    return Ok(objPQ);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Exception : Bikewale.Service.Controllers.PriceQuote.PriceQuoteController.PostV2 - /api/v2/PriceQuote/");
                return InternalServerError();
            }
        }

        /// <summary>
        /// Created by  : Pratibha Verma on 29 August 2018
        /// Description : get version's price list(Exshowroom, RTO, Insurance)
        /// </summary>
        /// <param name="versionId"></param>
        /// <param name="cityId"></param>
        /// <returns></returns>
        [ResponseType(typeof(Bikewale.Entities.BikeBooking.v2.PQOutputEntity)), Route("api/prices/version/{versionid}/cityid/{cityid}")]
        public IHttpActionResult GetVersionPriceList(uint versionId, uint cityId)
        {
            if (versionId > 0 && cityId > 0)
            {
                try
                {
                    IEnumerable<PriceCategory> priceListObj = _objPQ.GetVersionPriceListByCityId(versionId, cityId);
                    if (priceListObj != null && priceListObj.Any())
                    {
                        var priceList = new { priceList = priceListObj };
                        return Ok(new[] { priceList });
                    }
                    else
                    {
                        return NotFound();
                    }
                }
                catch (Exception ex)
                {
                    ErrorClass.LogError(ex, "Exception : Bikewale.Service.Controllers.PriceQuote.GetVersionPriceList");
                    return InternalServerError();
                }
            }
            else
            {
                return BadRequest();
            }
        }

    }
}