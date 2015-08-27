﻿using AutoMapper;
using Bikewale.DTO.PriceQuote.DealerPriceQuote;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.Location;
using Bikewale.Entity.BikeBooking;
using Bikewale.Interfaces.BikeBooking;
using Bikewale.Notifications;
using Bikewale.Service.AutoMappers.PriceQuote;
using Bikewale.Utility;
using Microsoft.Practices.Unity;
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
    /// Dealer Price Quote Controller
    /// Author  :   Sumit Kate
    /// Created on  :   24 Aug 2015
    /// </summary>
    public class DealerPriceQuoteController : ApiController
    {
        /// <summary>
        /// Gets the Dealer price Quote availability for given version and city
        /// </summary>
        /// <param name="versionId">bike version id</param>
        /// <param name="cityId">city id</param>
        /// <returns></returns>
        [ResponseType(typeof(bool))]
        public HttpResponseMessage Get(uint versionId, uint cityId)
        {
            bool isDealerPricesAvailable = false;
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IDealerPriceQuote, Bikewale.BAL.BikeBooking.DealerPriceQuote>();
                    IDealerPriceQuote objDealer = container.Resolve<IDealerPriceQuote>();
                    isDealerPricesAvailable = objDealer.IsDealerPriceAvailable(versionId, cityId);
                    if (isDealerPricesAvailable)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, isDealerPricesAvailable);
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.NoContent, "No Data Found");
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.Service.Controllers.PriceQuote.DealerPriceQuoteController.Get");
                objErr.SendMail();
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "some error occured.");
            }
        }

        /// <summary>
        /// Generates the dealer price quote
        /// </summary>
        /// <param name="input">Required parameters to generate the dealer price quote</param>
        /// <returns>Dealer Price Quotation</returns>
        [ResponseType(typeof(DPQuotationOutput))]
        public HttpResponseMessage Post([FromBody]DPQuotationInput input)
        {
            PQ_QuotationEntity objPrice = null;
            DPQuotationOutput output = null;
            try
            {
                string abHostUrl = ConfigurationManager.AppSettings["ABApiHostUrl"];
                string requestType = "application/json";
                string api = String.Format("/api/DealerPriceQuote/GetDealerPriceQuote/?cityid={0}&versionid={1}&dealerid={2}", input.CityId, input.VersionId, input.DealerId);

                objPrice = BWHttpClient.GetApiResponseSync<PQ_QuotationEntity>(abHostUrl, requestType, api, objPrice);

                if (objPrice != null)
                {
                    output = PriceQuoteEntityToCTO.ConvertDPQuotation(objPrice);
                    return Request.CreateResponse(HttpStatusCode.OK, output);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NoContent, "No dealer price quote found");
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.Service.Controllers.PriceQuote.DealerPriceQuoteController.Post");
                objErr.SendMail();
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "some error occured.");
            }
        }
    }
}
