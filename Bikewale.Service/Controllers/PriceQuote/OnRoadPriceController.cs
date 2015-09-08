﻿using Bikewale.DTO.PriceQuote;
using Bikewale.DTO.PriceQuote.BikeQuotation;
using Bikewale.DTO.PriceQuote.DealerPriceQuote;
using Bikewale.Entities.BikeBooking;
using Bikewale.Entities.PriceQuote;
using Bikewale.Interfaces.BikeBooking;
using Bikewale.Interfaces.PriceQuote;
using Bikewale.Notifications;
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
    /// On Road Price Controller
    /// Author  :   Sumit Kate
    /// Created :   08 Sept 2015
    /// </summary>
    public class OnRoadPriceController : ApiController
    {
        private readonly IDealerPriceQuote _objIPQ = null;
        private readonly IPriceQuote _objPriceQuote = null;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="objIPQ"></param>
        /// <param name="objPriceQuote"></param>
        public OnRoadPriceController(IDealerPriceQuote objIPQ, IPriceQuote objPriceQuote)
        {
            _objIPQ = objIPQ;
            _objPriceQuote = objPriceQuote;
        }

        /// <summary>
        /// Gets the On Road Price Quote
        /// Includes the Bike Wale price quote and Dealer price quote
        /// </summary>
        /// <param name="cityId">city id</param>
        /// <param name="modelId">model id</param>
        /// <param name="clientIP">client ip</param>
        /// <param name="sourceType">source type</param>
        /// <param name="areaId">area id(optional)</param>
        /// <returns></returns>
        [ResponseType(typeof(PQOnRoad))]
        public IHttpActionResult Get(uint cityId, uint modelId, string clientIP, PQSources sourceType, uint? areaId = null)
        {
            string response = string.Empty;
            string api = String.Empty;
            Bikewale.DTO.PriceQuote.PQOutput objPQ = null;
            PQOutputEntity objPQOutput = null;
            PQ_QuotationEntity objPrice = null;
            DPQuotationOutput dpqOutput = null;
            BikeQuotationEntity bpqOutput = null;
            PQBikePriceQuoteOutput bwPriceQuote = null;
            PQOnRoad onRoadPrice = null;
            try
            {
                string abHostUrl = ConfigurationManager.AppSettings["ABApiHostUrl"];
                string requestType = "application/json";
                PriceQuoteParametersEntity objPQEntity = new PriceQuoteParametersEntity();
                objPQEntity.CityId = cityId;
                objPQEntity.AreaId = areaId.HasValue ? areaId.Value : 0;
                objPQEntity.ClientIP = clientIP;
                objPQEntity.SourceId = Convert.ToUInt16(sourceType);
                objPQEntity.ModelId = modelId;
                objPQOutput = _objIPQ.ProcessPQ(objPQEntity);
                if (objPQOutput != null)
                {
                    onRoadPrice = new PQOnRoad();
                    objPQ = PQOutputMapper.Convert(objPQOutput);
                    onRoadPrice.PriceQuote = objPQ;
                    if (objPQ != null && objPQ.PQId > 0)
                    {
                        bpqOutput = _objPriceQuote.GetPriceQuoteById(objPQ.PQId);
                        if (bpqOutput != null)
                        {
                            bwPriceQuote = PQBikePriceQuoteOutputMapper.Convert(bpqOutput);
                            onRoadPrice.BPQOutput = bwPriceQuote;
                        }
                        if (objPQ.DealerId != 0)
                        {
                            api = String.Format("/api/DealerPriceQuote/GetDealerPriceQuote/?cityid={0}&versionid={1}&dealerid={2}", cityId, objPQ.VersionId, objPQ.DealerId);
                            objPrice = BWHttpClient.GetApiResponseSync<PQ_QuotationEntity>(abHostUrl, requestType, api, objPrice);
                            if (objPrice != null)
                            {
                                dpqOutput = DPQuotationOutputMapper.Convert(objPrice);
                                onRoadPrice.DPQOutput = dpqOutput;
                            }
                        }
                        return Ok(onRoadPrice);
                    }
                    else
                    {
                        return NotFound();
                    }
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
