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
        /// <summary>
        /// Gets the BikeWale Price Quote from the Price Quote Id
        /// </summary>
        /// <param name="pqId">Price Quote Id</param>
        /// <returns>BikeWale Price Quote</returns>
        [ResponseType(typeof(PQBikePriceQuoteOutput))]
        public HttpResponseMessage Get(UInt64 pqId)
        {
            IPriceQuote objPriceQuote = null;
            BikeQuotationEntity quotation = null;
            PQBikePriceQuoteOutput bwPriceQuote = null;
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IPriceQuote, BAL.PriceQuote.PriceQuote>();
                    objPriceQuote = container.Resolve<IPriceQuote>();
                    quotation = objPriceQuote.GetPriceQuoteById(pqId);
                }
                if (quotation != null)
                {
                    bwPriceQuote = PriceQuoteEntityToCTO.ConvertBikePriceQuote(quotation);
                    return Request.CreateResponse(HttpStatusCode.OK, bwPriceQuote);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Price Quote not found");
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.Service.Controllers.PriceQuote.BikePQController.Get");
                objErr.SendMail();
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "some error occured.");
            }
        }
    }
}
