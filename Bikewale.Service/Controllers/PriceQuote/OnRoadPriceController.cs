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
using Bikewale.Service.Utilities;
using System;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;

namespace Bikewale.Service.Controllers.PriceQuote
{
    /// <summary>
    /// On Road Price Controller
    /// Author  :   Sumit Kate
    /// Created :   08 Sept 2015
    /// Modified by :   Sumit Kate on 18 May 2016
    /// Description :   Extend from CompressionApiController instead of ApiController 
    /// </summary>
    public class OnRoadPriceController : CompressionApiController//ApiController
    {
        private readonly IDealerPriceQuote _objIPQ = null;
        private readonly IPriceQuote _objPriceQuote = null;
        private readonly IBikeModels<BikeModelEntity, int> _modelsRepository = null;
        private readonly IDealerPriceQuoteDetail _objDPQ = null;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="objIPQ"></param>
        /// <param name="objPriceQuote"></param>
        /// <param name="modelsRepository"></param>
        public OnRoadPriceController(IDealerPriceQuote objIPQ, IPriceQuote objPriceQuote, IBikeModels<BikeModelEntity, int> modelsRepository, IDealerPriceQuoteDetail objDPQ)
        {
            _objIPQ = objIPQ;
            _objPriceQuote = objPriceQuote;
            _modelsRepository = modelsRepository;
            _objDPQ = objDPQ;
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
        public IHttpActionResult Get(uint cityId, uint modelId, string clientIP, PQSources sourceType, string deviceId = null, uint? areaId = null, ushort? pqLeadId = null)
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


        /// <summary>
        /// Gets the On Road Price Quote
        /// Includes the Bike Wale price quote and Dealer price quote
        /// Modified By : Sushil kumar  on  2nd Dec 2015
        /// Description : Added bike details to response (for android)
        /// Modified By : Sadhana Upadhyay on 29 Dec 2015
        /// Summary : To capture device id, utma, utmz, Pq lead id etc.
        /// Modified By : Sushil Kumar on 7th June 2016 
        /// Description : Added empty dealer to list for dealers in case no primary dealer available
        /// </summary>
        /// <param name="cityId"></param>
        /// <param name="modelId"></param>
        /// <param name="clientIP"></param>
        /// <param name="sourceType"></param>
        /// <param name="deviceId"></param>
        /// <param name="areaId"></param>
        /// <param name="pqLeadId"></param>
        /// <returns></returns>
        [ResponseType(typeof(Bikewale.DTO.PriceQuote.v2.PQOnRoad)), Route("api/v2/OnRoadPrice")]
        public IHttpActionResult GetV2(uint cityId, uint modelId, string clientIP, PQSources sourceType, string deviceId = null, uint? areaId = null, ushort? pqLeadId = null)
        {
            string response = string.Empty;
            string api = String.Empty;
            Bikewale.DTO.PriceQuote.v2.PQOutput objPQ = null;
            PQOutputEntity objPQOutput = null;
            BikeQuotationEntity bpqOutput = null;
            Bikewale.DTO.PriceQuote.v2.PQOnRoad onRoadPrice = null;
            try
            {
                PriceQuoteParametersEntity objPQEntity = new PriceQuoteParametersEntity();
                objPQEntity.CityId = cityId;
                objPQEntity.AreaId = areaId.HasValue ? areaId.Value : 0;
                objPQEntity.ClientIP = clientIP;
                objPQEntity.SourceId = Convert.ToUInt16(sourceType);
                objPQEntity.ModelId = modelId;
                objPQEntity.DeviceId = deviceId;
                objPQEntity.PQLeadId = pqLeadId;

                objPQOutput = _objIPQ.ProcessPQ(objPQEntity);
                if (objPQOutput != null)
                {
                    onRoadPrice = new Bikewale.DTO.PriceQuote.v2.PQOnRoad();
                    objPQ = PQOutputMapper.ConvertV2(objPQOutput);
                    onRoadPrice.PriceQuote = objPQ;

                    if (objPQ != null && objPQ.PQId > 0)
                    {
                        bpqOutput = _objPriceQuote.GetPriceQuoteById(objPQ.PQId);
                        //add bike make and model
                        objPQ.MakeName = bpqOutput.MakeName;
                        objPQ.ModelName = bpqOutput.ModelName;

                        bpqOutput.Varients = _objPriceQuote.GetOtherVersionsPrices(objPQ.PQId);

                        if ((objPQ.DealerId != 0) || objPQ.IsDealerAvailable)
                        {

                            DetailedDealerQuotationEntity objDealerQuotation = _objDPQ.GetDealerQuotation(cityId, objPQOutput.VersionId, objPQOutput.DealerId);

                            onRoadPrice.version = PQBikePriceQuoteOutputMapper.Convert(bpqOutput.Varients);

                            onRoadPrice.SecondaryDealers = PQBikePriceQuoteOutputMapper.Convert(objDealerQuotation.SecondaryDealers);
                            if (onRoadPrice.SecondaryDealers == null)
                            {
                                onRoadPrice.SecondaryDealers = new System.Collections.Generic.List<DTO.PriceQuote.v2.DPQDealerBase>();
                            }

                            if (objDealerQuotation.PrimaryDealer != null && objDealerQuotation.PrimaryDealer.DealerDetails != null)
                            {
                                onRoadPrice.SecondaryDealers.Insert(0, new DTO.PriceQuote.v2.DPQDealerBase()
                                {
                                    Area = objDealerQuotation.PrimaryDealer.DealerDetails.objArea.AreaName,
                                    DealerId = objDealerQuotation.PrimaryDealer.DealerDetails.DealerId,
                                    MaskingNumber = objDealerQuotation.PrimaryDealer.DealerDetails.MaskingNumber,
                                    Name = objDealerQuotation.PrimaryDealer.DealerDetails.Organization
                                });

                                if (objPQ.DealerId == 0)
                                {
                                    onRoadPrice.SecondaryDealers.Insert(0, new DTO.PriceQuote.v2.DPQDealerBase()
                                    {
                                        Area = String.Empty,
                                        DealerId = 0,
                                        MaskingNumber = String.Empty,
                                        Name = String.Empty
                                    });
                                }
                            }
                            else
                            {
                                onRoadPrice.SecondaryDealers.Insert(0, new DTO.PriceQuote.v2.DPQDealerBase()
                                {
                                    Area = String.Empty,
                                    DealerId = 0,
                                    MaskingNumber = String.Empty,
                                    Name = string.Empty
                                });
                            }

                            return Ok(onRoadPrice);
                        }
                        else
                        {
                            onRoadPrice.version = PQBikePriceQuoteOutputMapper.Convert(bpqOutput.Varients);
                            if (onRoadPrice.SecondaryDealers == null)
                            {
                                onRoadPrice.SecondaryDealers = new System.Collections.Generic.List<DTO.PriceQuote.v2.DPQDealerBase>();
                            }
                            onRoadPrice.SecondaryDealers.Insert(0, new DTO.PriceQuote.v2.DPQDealerBase()
                            {
                                Area = String.Empty,
                                DealerId = 0,
                                MaskingNumber = String.Empty,
                                Name = string.Empty
                            });
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
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.Service.Controllers.PriceQuote.OnRoadPriceController.GetV2");
                objErr.SendMail();
                return InternalServerError();
            }
        }

        /// <summary>
        /// Created By : Sumit Kate
        /// Created on : 3th June 2016
        /// Description : To provide dealer quotation and its bike's version prices 
        /// Modified By : Sushil Kumar on 7th June 2016
        /// Description : Added Try catch and null check for versionid
        /// </summary>
        /// <param name="cityId"></param>
        /// <param name="deviceId"></param>
        /// <param name="dealerId"></param>
        /// <param name="modelId"></param>
        /// <param name="areaId"></param>
        /// <param name="versionId"></param>
        /// <returns></returns>
        [ResponseType(typeof(Bikewale.DTO.PriceQuote.v2.PQOnRoad)), Route("api/dealerversionprices")]
        public IHttpActionResult GetDealerVersionPrices(uint cityId, string deviceId, uint dealerId, uint modelId, uint? areaId = null, uint? versionId = null)
        {
            try
            {
                bool isPQRegistered = Request.Headers.Contains("isRegistered") ? Convert.ToBoolean(Request.Headers.GetValues("isRegistered").FirstOrDefault()) : false;
                UInt64 pqId = Request.Headers.Contains("quoteId") ? Convert.ToUInt64(Request.Headers.GetValues("quoteId").FirstOrDefault()) : default(UInt64);
                UInt16 platformId = Request.Headers.Contains("platformId") ? Convert.ToUInt16(Request.Headers.GetValues("platformId").FirstOrDefault()) : default(UInt16);

                if (pqId > 0 && platformId > 0)
                {
                    PQ_QuotationEntity bwPQ = _objDPQ.Quotation(cityId, platformId, deviceId, dealerId, modelId, ref pqId, isPQRegistered, areaId, versionId);
                    DTO.PriceQuote.v2.DPQuotationOutput dpq = null;

                    DetailedDealerQuotationEntity objDealerQuotation = _objDPQ.GetDealerQuotation(cityId, versionId.HasValue ? versionId.Value : 0, dealerId);

                    dpq = PQBikePriceQuoteOutputMapper.Convert(objDealerQuotation, bwPQ.Varients);
                    dpq.PriceQuoteId = pqId;
                    if (objDealerQuotation.PrimaryDealer != null && objDealerQuotation.PrimaryDealer.DealerDetails != null)
                    {

                        dpq.Dealer = PQBikePriceQuoteOutputMapper.Convert(objDealerQuotation.PrimaryDealer.DealerDetails);
                    }
                    return Ok(dpq);
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.Service.Controllers.PriceQuote.OnRoadPriceController.GetDealerVersionPrices");
                objErr.SendMail();
                return InternalServerError();
            }
        }

        /// <summary>
        /// Gets the On Road Price Quote
        /// Includes the Bike Wale price quote and Dealer price quote
        /// Modified By : Sushil kumar  on  2nd Dec 2015
        /// Description : Added bike details to response (for android)
        /// Modified By : Sadhana Upadhyay on 29 Dec 2015
        /// Summary : To capture device id, utma, utmz, Pq lead id etc.
        /// Modified By : Sushil Kumar on 7th June 2016 
        /// Description : Added empty dealer to list for dealers in case no primary dealer available
        /// </summary>
        /// <param name="cityId"></param>
        /// <param name="modelId"></param>
        /// <param name="clientIP"></param>
        /// <param name="sourceType"></param>
        /// <param name="deviceId"></param>
        /// <param name="areaId"></param>
        /// <param name="pqLeadId"></param>
        /// <returns></returns>
        [ResponseType(typeof(Bikewale.DTO.PriceQuote.v3.PQOnRoad)), Route("api/v3/OnRoadPrice")]
        public IHttpActionResult GetV3(uint cityId, uint modelId, string clientIP, PQSources sourceType, string deviceId = null, uint? areaId = null, ushort? pqLeadId = null)
        {
            string response = string.Empty;
            string api = String.Empty;
            Bikewale.DTO.PriceQuote.v2.PQOutput objPQ = null;
            PQOutputEntity objPQOutput = null;
            BikeQuotationEntity bpqOutput = null;
            Bikewale.DTO.PriceQuote.v3.PQOnRoad onRoadPrice = null;
            try
            {
                PriceQuoteParametersEntity objPQEntity = new PriceQuoteParametersEntity();
                objPQEntity.CityId = cityId;
                objPQEntity.AreaId = areaId.HasValue ? areaId.Value : 0;
                objPQEntity.ClientIP = clientIP;
                objPQEntity.SourceId = Convert.ToUInt16(sourceType);
                objPQEntity.ModelId = modelId;
                objPQEntity.DeviceId = deviceId;
                objPQEntity.PQLeadId = pqLeadId;

                objPQOutput = _objIPQ.ProcessPQ(objPQEntity);
                if (objPQOutput != null)
                {
                    onRoadPrice = new Bikewale.DTO.PriceQuote.v3.PQOnRoad();
                    objPQ = PQOutputMapper.ConvertV2(objPQOutput);
                    onRoadPrice.PriceQuote = objPQ;

                    if (objPQ != null && objPQ.PQId > 0)
                    {
                        bpqOutput = _objPriceQuote.GetPriceQuoteById(objPQ.PQId);
                        //add bike make and model
                        objPQ.MakeName = bpqOutput.MakeName;
                        objPQ.ModelName = bpqOutput.ModelName;

                        bpqOutput.Varients = _objPriceQuote.GetOtherVersionsPrices(objPQ.PQId);

                        if ((objPQ.DealerId != 0) || objPQ.IsDealerAvailable)
                        {

                            Bikewale.Entities.PriceQuote.v2.DetailedDealerQuotationEntity objDealerQuotation = _objDPQ.GetDealerQuotationV2(cityId, objPQOutput.VersionId, objPQOutput.DealerId);

                            onRoadPrice.Versions = PQBikePriceQuoteOutputMapper.Convert(bpqOutput.Varients);

                            if (objDealerQuotation != null && objDealerQuotation.SecondaryDealers != null)
                            {
                                onRoadPrice.SecondaryDealers = PQBikePriceQuoteOutputMapper.Convert(objDealerQuotation.SecondaryDealers);
                            }
                            
                            if (onRoadPrice.SecondaryDealers == null)
                            {
                                onRoadPrice.SecondaryDealers = new System.Collections.Generic.List<DTO.PriceQuote.v3.DPQDealerBase>();
                            }

                            if (objDealerQuotation != null && objDealerQuotation.PrimaryDealer != null && objDealerQuotation.PrimaryDealer.DealerDetails != null)
                            {
                                onRoadPrice.SecondaryDealers.Insert(0, new DTO.PriceQuote.v3.DPQDealerBase()
                                {
                                    Area = objDealerQuotation.PrimaryDealer.DealerDetails.objArea.AreaName,
                                    DealerId = objDealerQuotation.PrimaryDealer.DealerDetails.DealerId,
                                    MaskingNumber = objDealerQuotation.PrimaryDealer.DealerDetails.MaskingNumber,
                                    Name = objDealerQuotation.PrimaryDealer.DealerDetails.Organization
                                });

                                if (objPQ.DealerId == 0)
                                {
                                    onRoadPrice.SecondaryDealers.Insert(0, new DTO.PriceQuote.v3.DPQDealerBase()
                                    {
                                        Area = String.Empty,
                                        DealerId = 0,
                                        MaskingNumber = String.Empty,
                                        Name = String.Empty,
                                        IsPremiumDealer = true
                                    });
                                }
                            }
                            else
                            {
                                onRoadPrice.SecondaryDealers.Insert(0, new DTO.PriceQuote.v3.DPQDealerBase()
                                {
                                    Area = String.Empty,
                                    DealerId = 0,
                                    MaskingNumber = String.Empty,
                                    Name = string.Empty,
                                    IsPremiumDealer = true
                                });
                            }

                            return Ok(onRoadPrice);
                        }
                        else
                        {
                            onRoadPrice.Versions = PQBikePriceQuoteOutputMapper.Convert(bpqOutput.Varients);
                            if (onRoadPrice.SecondaryDealers == null)
                            {
                                onRoadPrice.SecondaryDealers = new System.Collections.Generic.List<DTO.PriceQuote.v3.DPQDealerBase>();
                            }
                            onRoadPrice.SecondaryDealers.Insert(0, new DTO.PriceQuote.v3.DPQDealerBase()
                            {
                                Area = String.Empty,
                                DealerId = 0,
                                MaskingNumber = String.Empty,
                                Name = string.Empty,
                                IsPremiumDealer = true
                            });
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
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.Service.Controllers.PriceQuote.OnRoadPriceController.GetV3");
                objErr.SendMail();
                return InternalServerError();
            }
        }

    }
}