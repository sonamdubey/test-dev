using Bikewale.Entities.BikeBooking;
using Bikewale.Entities.PriceQuote;
using Bikewale.Interfaces.BikeBooking;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Bikewale.DTO;
using AutoMapper;
using System.Web.Http.Description;
using Bikewale.DTO.PriceQuote;
using Bikewale.Service.AutoMappers.PriceQuote;
using Bikewale.Notifications;
using Bikewale.DTO.PriceQuote.DealerPriceQuote;
using System.Configuration;
using Bikewale.Utility;
namespace Bikewale.Service.Controllers.PriceQuote
{
    /// <summary>
    /// Price Quote Controller
    /// Author  :   Sumit Kate
    /// Created On  :   21 Aug 2015
    /// </summary>
    public class PriceQuoteController : ApiController
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
