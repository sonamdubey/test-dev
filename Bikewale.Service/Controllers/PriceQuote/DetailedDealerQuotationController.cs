using AutoMapper;
using Bikewale.DTO.PriceQuote.DetailedDealerQuotation;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.Location;
using Bikewale.Entity.BikeBooking;
using Bikewale.Service.AutoMappers.PriceQuote;
using Bikewale.Utility;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace Bikewale.Service.Controllers.PriceQuote
{
    /// <summary>
    /// Detailed dealer quatation controller
    /// Author  :   Sumit Kate
    /// Created On  : 24 Aug 2015
    /// </summary>
    public class DetailedDealerQuotationController : ApiController
    {
        /// <summary>
        /// Gets Detailed Dealer Quotation
        /// </summary>
        /// <param name="versionId">Bike version</param>
        /// <param name="dealerId">Dealer Id</param>
        /// <param name="cityId">City Id</param>
        /// <returns></returns>
        [ResponseType(typeof(DDQDealerDetailBase))]
        public HttpResponseMessage Get(Int32 versionId, Int32 dealerId, Int32 cityId)
        {
            PQ_DealerDetailEntity dealerDetailEntity = null;
            DDQDealerDetailBase output = null;
            string _abHostUrl = ConfigurationManager.AppSettings["ABApiHostUrl"];
            string _requestType = "application/json";

            string _apiUrl = String.Format("/api/Dealers/GetDealerDetailsPQ/?versionId={0}&DealerId={1}&CityId={2}", versionId, dealerId, cityId);
            // Send HTTP GET requests 

            dealerDetailEntity = BWHttpClient.GetApiResponseSync<PQ_DealerDetailEntity>(_abHostUrl, _requestType, _apiUrl, dealerDetailEntity);

            if (dealerDetailEntity != null)
            {
                output = DDQDealerDetailBaseMapper.Convert(dealerDetailEntity);
                return Request.CreateResponse(HttpStatusCode.OK, output);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NoContent, "No dealer price quote found");
            }

        }
    }
}
