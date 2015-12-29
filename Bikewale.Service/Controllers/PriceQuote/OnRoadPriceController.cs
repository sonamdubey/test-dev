using Bikewale.DTO.Model;
using Bikewale.DTO.PriceQuote;
using Bikewale.DTO.PriceQuote.BikeQuotation;
using Bikewale.DTO.PriceQuote.DealerPriceQuote;
using Bikewale.Entities.BikeBooking;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.PriceQuote;
using Bikewale.Interfaces.BikeBooking;
using Bikewale.Interfaces.BikeData;
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
        private readonly IBikeModels<BikeModelEntity, int> _modelsRepository = null;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="objIPQ"></param>
        /// <param name="objPriceQuote"></param>
        /// <param name="modelsRepository"></param>
        public OnRoadPriceController(IDealerPriceQuote objIPQ, IPriceQuote objPriceQuote, IBikeModels<BikeModelEntity, int> modelsRepository)
        {
            _objIPQ = objIPQ;
            _objPriceQuote = objPriceQuote;
            _modelsRepository = modelsRepository;
        }

        /// <summary>
        /// Gets the On Road Price Quote
        /// Includes the Bike Wale price quote and Dealer price quote
        /// Modified By : Sushil kumar  on  2nd Dec 2015
        /// Description : Added bike details to response (for android)
        /// Modified By : Sadhana Upadhyay on 29 Dec 2015
        /// Summary : To capture device id, utma, utmz, Pq lead id etc.
        /// </summary>
        /// <param name="cityId"></param>
        /// <param name="modelId"></param>
        /// <param name="clientIP"></param>
        /// <param name="sourceType"></param>
        /// <param name="deviceId"></param>
        /// <param name="areaId"></param>
        /// <param name="pqLeadId"></param>
        /// <returns></returns>
        [ResponseType(typeof(PQOnRoad))]
        public IHttpActionResult Get(uint cityId, uint modelId, string clientIP, PQSources sourceType, string deviceId, uint? areaId = null, ushort? pqLeadId = null)
        {
            string response = string.Empty;
            string api = String.Empty;
            Bikewale.DTO.PriceQuote.PQOutput objPQ = null;
            BikeModelEntity objModel = null;
            ModelDetail objModelDetails = null;
            PQOutputEntity objPQOutput = null;
            PQ_QuotationEntity objPrice = null;
            DPQuotationOutput dpqOutput = null;
            BikeQuotationEntity bpqOutput = null;
            PQBikePriceQuoteOutput bwPriceQuote = null;
            PQOnRoad onRoadPrice = null;
            try
            {
                PriceQuoteParametersEntity objPQEntity = new PriceQuoteParametersEntity();
                objPQEntity.CityId = cityId;
                objPQEntity.AreaId = areaId.HasValue ? areaId.Value : 0;
                objPQEntity.ClientIP = clientIP;
                objPQEntity.SourceId = Convert.ToUInt16(sourceType);
                objPQEntity.ModelId = modelId;
                objPQEntity.UTMA = Request.Headers.Contains("utma") ? Request.Headers.GetValues("utma").FirstOrDefault() : String.Empty;
                objPQEntity.UTMZ = Request.Headers.Contains("utmz") ? Request.Headers.GetValues("utmz").FirstOrDefault() : String.Empty;
                objPQEntity.DeviceId = deviceId;
                objPQEntity.PQLeadId = pqLeadId;

                objPQOutput = _objIPQ.ProcessPQ(objPQEntity);
                if (objPQOutput != null)
                {
                    onRoadPrice = new PQOnRoad();
                    objPQ = PQOutputMapper.Convert(objPQOutput);
                    onRoadPrice.PriceQuote = objPQ;

                    //To get bike details 
                    objModel = _modelsRepository.GetById(Convert.ToInt32(modelId));
                    objModelDetails = PQOutputMapper.Convert(objModel);
                    onRoadPrice.BikeDetails = objModelDetails;

                    if (objPQ != null && objPQ.PQId > 0)
                    {
                        bpqOutput = _objPriceQuote.GetPriceQuoteById(objPQ.PQId);
                        bpqOutput.Varients = _objPriceQuote.GetOtherVersionsPrices(objPQ.PQId);
                        if (bpqOutput != null)
                        {
                            bwPriceQuote = PQBikePriceQuoteOutputMapper.Convert(bpqOutput);

                            bpqOutput.Varients = null;

                            onRoadPrice.BPQOutput = bwPriceQuote;
                        }
                        if (objPQ.DealerId != 0)
                        {
                            api = String.Format("/api/DealerPriceQuote/GetDealerPriceQuote/?cityid={0}&versionid={1}&dealerid={2}", cityId, objPQ.VersionId, objPQ.DealerId);

                            using (Utility.BWHttpClient objClient = new Utility.BWHttpClient())
                            {
                                //objPrice = objClient.GetApiResponseSync<PQ_QuotationEntity>(Utility.BWConfiguration.Instance.ABApiHostUrl, Utility.BWConfiguration.Instance.APIRequestTypeJSON, api, objPrice);
                                objPrice = objClient.GetApiResponseSync<PQ_QuotationEntity>(Utility.APIHost.AB, Utility.BWConfiguration.Instance.APIRequestTypeJSON, api, objPrice);
                            }

                            if (objPrice != null)
                            {
                                dpqOutput = DPQuotationOutputMapper.Convert(objPrice);

                                uint insuranceAmount = 0;

                                foreach (var price in objPrice.PriceList)
                                {
                                    onRoadPrice.IsInsuranceFree = Bikewale.Utility.DealerOfferHelper.HasFreeInsurance(objPQ.DealerId.ToString(), "", price.CategoryName, price.Price, ref insuranceAmount);
                                }

                                onRoadPrice.IsInsuranceFree = true;
                                onRoadPrice.DPQOutput = dpqOutput;
                                onRoadPrice.InsuranceAmount = insuranceAmount;

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
                            }
                        }
                        return Ok(onRoadPrice);
                    }
                    else
                    {
                        return Ok(onRoadPrice);
                    }
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.Service.Controllers.PriceQuote.OnRoadPriceController.GET");
                objErr.SendMail();
                return InternalServerError();
            }
        }
    }
}
