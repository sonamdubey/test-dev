using AutoMapper;
using Bikewale.DTO.PriceQuote.DealerPriceQuote;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.Location;
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
using Bikewale.Entities.BikeBooking;

namespace Bikewale.Service.Controllers.PriceQuote
{
    /// <summary>
    /// Dealer Price Quote Controller
    /// Author  :   Sumit Kate
    /// Created on  :   24 Aug 2015
    /// </summary>
    public class DealerPriceQuoteController : ApiController
    {
        private readonly IDealerPriceQuote _objDealer = null;
        public DealerPriceQuoteController(IDealerPriceQuote objDealer)
        {
            _objDealer = objDealer;
        }
        /// <summary>
        /// Gets the Dealer price Quote availability for given version and city
        /// </summary>
        /// <param name="versionId">bike version id</param>
        /// <param name="cityId">city id</param>
        /// <returns></returns>
        [ResponseType(typeof(bool))]
        public IHttpActionResult Get(uint versionId, uint cityId)
        {
            bool isDealerPricesAvailable = false;
            try
            {
                isDealerPricesAvailable = _objDealer.IsDealerPriceAvailable(versionId, cityId);
                if (isDealerPricesAvailable)
                {
                    return Ok(isDealerPricesAvailable);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.Service.Controllers.PriceQuote.DealerPriceQuoteController.Get");
                objErr.SendMail();
                return InternalServerError();
            }
        }

        /// <summary>
        /// Generates the dealer price quote
        /// </summary>
        /// <param name="input">Required parameters to generate the dealer price quote</param>
        /// <returns>Dealer Price Quotation</returns>
        [ResponseType(typeof(DPQuotationOutput))]
        public IHttpActionResult Post([FromBody]DPQuotationInput input)
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
                    output = DPQuotationOutputMapper.Convert(objPrice);

                    if (objPrice.Disclaimer != null)
                    {
                        objPrice.Disclaimer.Clear();
                        objPrice.Disclaimer = null; 
                    }

                    if (objPrice.objOffers != null)
                    {
                        objPrice.objOffers.Clear();
                        objPrice.objOffers = null; 
                    }

                    if (objPrice.PriceList != null)
                    {
                        objPrice.PriceList.Clear();
                        objPrice.PriceList = null; 
                    }

                    objPrice.Varients = null;

                    return Ok(output);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.Service.Controllers.PriceQuote.DealerPriceQuoteController.Post");
                objErr.SendMail();
                return InternalServerError();
            }
        }
    }
}
