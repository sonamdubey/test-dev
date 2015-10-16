using AutoMapper;
using Bikewale.DTO.PriceQuote.BikeQuotation;
using Bikewale.Entities.PriceQuote;
using Bikewale.Interfaces.PriceQuote;
using Bikewale.Notifications;
using Bikewale.Service.AutoMappers.PriceQuote;
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
    /// Bikewale Price Quote Controller
    /// Author  :   Sumit Kate
    /// Created On  :   25 Aug 2015
    /// </summary>
    public class BikePQController : ApiController
    {
        private readonly IPriceQuote _objPriceQuote = null;
        public BikePQController(IPriceQuote objPriceQuote)
        {
            _objPriceQuote = objPriceQuote;
        }
        /// <summary>
        /// Gets the BikeWale Price Quote from the Price Quote Id
        /// </summary>
        /// <param name="pqId">Price Quote Id</param>
        /// <returns>BikeWale Price Quote</returns>
        [ResponseType(typeof(PQBikePriceQuoteOutput))]
        public IHttpActionResult Get(UInt64 pqId)
        {            
            BikeQuotationEntity quotation = null;
            PQBikePriceQuoteOutput bwPriceQuote = null;
            try
            {
                quotation = _objPriceQuote.GetPriceQuoteById(pqId);
                if (quotation != null)
                {
                    bwPriceQuote = PQBikePriceQuoteOutputMapper.Convert(quotation);

                    quotation.Varients = null;

                    return Ok(bwPriceQuote);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.Service.Controllers.PriceQuote.BikePQController.Get");
                objErr.SendMail();
                return InternalServerError();
            }
        }
    }
}
