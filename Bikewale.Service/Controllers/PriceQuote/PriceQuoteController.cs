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
namespace Bikewale.Service.Controllers.PriceQuote
{
    /// <summary>
    /// Price Quote Controller
    /// Author  :   Sumit Kate
    /// Created On  :   21 Aug 2015
    /// </summary>
    public class PriceQuoteController : ApiController
    {
        /// <summary>
        /// Bikewale Price Quote and Dealer Price Quote
        /// </summary>
        /// <param name="input">Entity contains the required details to get the price quote details</param>
        /// <returns></returns>
        [ResponseType(typeof(PQOutputEntity))]
        public HttpResponseMessage Post([FromBody]PQInput input)
        {
            string response = string.Empty;
            Bikewale.DTO.PriceQuote.PQOutput objPQ = null;
            PQOutputEntity objPQOutput = null;

            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    // save price quote
                    container.RegisterType<IDealerPriceQuote, Bikewale.BAL.BikeBooking.DealerPriceQuote>();
                    IDealerPriceQuote objIPQ = container.Resolve<IDealerPriceQuote>();

                    PriceQuoteParametersEntity objPQEntity = new PriceQuoteParametersEntity();
                    objPQEntity.CityId = input.CityId;
                    objPQEntity.AreaId = input.AreaId > 0 ? input.AreaId : 0;
                    objPQEntity.ClientIP = input.ClientIP;
                    objPQEntity.SourceId = Convert.ToUInt16(input.SourceType);
                    objPQEntity.ModelId = input.ModelId;
                    objPQOutput = objIPQ.ProcessPQ(objPQEntity);
                    if (objPQOutput != null)
                    {
                        objPQ = PriceQuoteEntityToCTO.ConvertPQOutputEntity(objPQOutput);
                        return Request.CreateResponse(HttpStatusCode.OK, objPQ);
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.NoContent, "No Data Found");
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.Service.Controllers.PriceQuote.PriceQuoteController.Post");
                objErr.SendMail();
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "some error occured.");
            }
        }
    }
}
