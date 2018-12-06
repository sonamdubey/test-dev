using Bikewale.DAL.AutoBiz;
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
using Bikewale.ManufacturerCampaign.Interface;
using Bikewale.Notifications;
using Bikewale.Service.AutoMappers.ManufacturerCampaign;
using Bikewale.Service.AutoMappers.PriceQuote;
using Bikewale.Service.Utilities;
using Microsoft.Practices.Unity;
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
    public class OnRoadPriceController : CompressionApiController //ApiController
    {
        private readonly IDealerPriceQuote _objIPQ = null;
        private readonly IPriceQuote _objPriceQuote = null;
        private readonly IBikeModels<BikeModelEntity, int> _modelsRepository = null;
        private readonly IDealerPriceQuoteDetail _objDPQ = null;
        private readonly IManufacturerCampaign _objManufacturerCampaign = null;
        private readonly IPriceQuoteCache _objPriceQuoteCache = null;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="objIPQ"></param>
        /// <param name="objPriceQuote"></param>
        /// <param name="modelsRepository"></param>
        /// <param name="objDPQ"></param>
        /// <param name="objManufacturerCampaign"></param>
        /// <param name="objPriceQuoteCache"></param>
        public OnRoadPriceController(IDealerPriceQuote objIPQ, IPriceQuote objPriceQuote, IBikeModels<BikeModelEntity, int> modelsRepository, IDealerPriceQuoteDetail objDPQ, IManufacturerCampaign objManufacturerCampaign, IPriceQuoteCache objPriceQuoteCache)
        {
            _objIPQ = objIPQ;
            _objPriceQuote = objPriceQuote;
            _modelsRepository = modelsRepository;
            _objDPQ = objDPQ;
            _objManufacturerCampaign = objManufacturerCampaign;
            _objPriceQuoteCache = objPriceQuoteCache;
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

                        var objBikeQuotationEntity = _objPriceQuoteCache.GetVersionPricesByModelId(modelId, cityId);

                        bpqOutput = objBikeQuotationEntity.FirstOrDefault(m => m.VersionId == objPQ.VersionId);
                        bpqOutput.Varients = _objPriceQuoteCache.GetOtherVersionsPrices(objPQEntity.ModelId, objPQEntity.CityId);
                        if (bpqOutput != null)
                        {
                            bwPriceQuote = PQBikePriceQuoteOutputMapper.Convert(bpqOutput);

                            bpqOutput.Varients = null;

                            onRoadPrice.BPQOutput = bwPriceQuote;
                        }
                        if (objPQ.DealerId != 0)
                        {
                            using (IUnityContainer container = new UnityContainer())
                            {
                                container.RegisterType<Bikewale.Interfaces.AutoBiz.IDealerPriceQuote, DealerPriceQuoteRepository>();
                                Bikewale.Interfaces.AutoBiz.IDealerPriceQuote objPriceQuote = container.Resolve<DealerPriceQuoteRepository>();
                                PQParameterEntity objParam = new PQParameterEntity();
                                objParam.CityId = cityId;
                                objParam.DealerId = objPQ.DealerId;
                                objParam.VersionId = objPQ.VersionId;
                                objPrice = objPriceQuote.GetDealerPriceQuote(objParam);
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
                ErrorClass.LogError(ex, "Exception : Bikewale.Service.Controllers.PriceQuote.OnRoadPriceController.GET");

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
                        var objBikeQuotationEntity = _objPriceQuoteCache.GetVersionPricesByModelId(modelId, cityId);

                        bpqOutput = objBikeQuotationEntity.FirstOrDefault(m => m.VersionId == objPQ.VersionId);
                        //add bike make and model
                        objPQ.MakeName = bpqOutput.MakeName;
                        objPQ.ModelName = bpqOutput.ModelName;

                        bpqOutput.Varients = _objPriceQuoteCache.GetOtherVersionsPrices(modelId, cityId);

                        if ((objPQ.DealerId != 0) || objPQ.IsDealerAvailable)
                        {

                            DetailedDealerQuotationEntity objDealerQuotation = _objDPQ.GetDealerQuotation(cityId, objPQOutput.VersionId, objPQOutput.DealerId);

                            onRoadPrice.version = PQBikePriceQuoteOutputMapper.Convert(bpqOutput.Varients);

                            onRoadPrice.SecondaryDealers = PQBikePriceQuoteOutputMapper.Convert(objDealerQuotation.SecondaryDealers);
                            if (onRoadPrice.SecondaryDealers == null)
                            {
                                onRoadPrice.SecondaryDealers = new System.Collections.Generic.List<DTO.PriceQuote.v2.DPQDealerBase>();
                            }

                            if (objDealerQuotation != null && objDealerQuotation.PrimaryDealer != null && objDealerQuotation.PrimaryDealer.DealerDetails != null)
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
                ErrorClass.LogError(ex, "Exception : Bikewale.Service.Controllers.PriceQuote.OnRoadPriceController.GetV2");

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
                        var objBikeQuotationEntity = _objPriceQuoteCache.GetVersionPricesByModelId(modelId, cityId);

                        bpqOutput = objBikeQuotationEntity.FirstOrDefault(m => m.VersionId == objPQ.VersionId);
                        //add bike make and model
                        objPQ.MakeName = bpqOutput.MakeName;
                        objPQ.ModelName = bpqOutput.ModelName;

                        bpqOutput.Varients = _objPriceQuoteCache.GetOtherVersionsPrices(modelId, cityId);

                        if ((objPQ.DealerId != 0) || objPQ.IsDealerAvailable)
                        {

                            Bikewale.Entities.PriceQuote.v2.DetailedDealerQuotationEntity objDealerQuotation = _objDPQ.GetDealerQuotationV2(cityId, objPQOutput.VersionId, objPQOutput.DealerId, objPQEntity.AreaId);

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
                                    Name = objDealerQuotation.PrimaryDealer.DealerDetails.Organization,
                                    IsPremiumDealer = objDealerQuotation.PrimaryDealer.IsPremiumDealer
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
                ErrorClass.LogError(ex, "Exception : Bikewale.Service.Controllers.PriceQuote.OnRoadPriceController.GetV3");

                return InternalServerError();
            }
        }


        /// <summary>
        /// Summary: v4 - Gets the On Road Price Quote
        /// Created by: Sangram Nandkhile on 11th Oct 2017
        /// Modified by : Monika Korrapati on 21 Sept 2018
        /// Description : Changed LeadsButtonTextMobile value to 'Get Best Offers' in default case
        /// </summary>
        /// <returns></returns>
        [ResponseType(typeof(Bikewale.DTO.PriceQuote.v3.PQOnRoad)), Route("api/v4/OnRoadPrice")]
        public IHttpActionResult GetV4(uint cityId, uint modelId, string clientIP, PQSources sourceType, string deviceId = null, uint? areaId = null, ushort? pqLeadId = null)
        {
            Bikewale.DTO.PriceQuote.v2.PQOutput objPQ = null;
            PQOutputEntity objPQOutput = null;
            BikeQuotationEntity bpqOutput = null;
            Bikewale.DTO.PriceQuote.v4.PQOnRoad onRoadPrice = null;
            Bikewale.Entities.PriceQuote.v2.DetailedDealerQuotationEntity objDealerQuotation = null;
            Bikewale.ManufacturerCampaign.Entities.ManufacturerCampaignEntity manufacturerCampaign = null;
            uint versionPrice = 0;
            try
            {
                PriceQuoteParametersEntity objPQEntity = new PriceQuoteParametersEntity()
                {
                    CityId = cityId,
                    AreaId = areaId.HasValue ? areaId.Value : 0,
                    ClientIP = clientIP,
                    SourceId = (ushort)sourceType,
                    ModelId = modelId,
                    DeviceId = deviceId,
                    PQLeadId = pqLeadId
                };

                objPQOutput = _objIPQ.ProcessPQ(objPQEntity);
                if (objPQOutput != null)
                {
                    onRoadPrice = new Bikewale.DTO.PriceQuote.v4.PQOnRoad();
                    objPQ = PQOutputMapper.ConvertV2(objPQOutput);
                    onRoadPrice.PriceQuote = objPQ;

                    if (objPQ != null && objPQ.PQId > 0)
                    {
                        var objBikeQuotationEntity = _objPriceQuoteCache.GetVersionPricesByModelId(modelId, cityId);

                        bpqOutput = objBikeQuotationEntity.FirstOrDefault(m => m.VersionId == objPQ.VersionId);
                        //add bike make and model
                        objPQ.MakeName = bpqOutput.MakeName;
                        objPQ.ModelName = bpqOutput.ModelName;

                        bpqOutput.Varients = _objPriceQuoteCache.GetOtherVersionsPrices(modelId, cityId);

                        if (objPQ.DealerId > 0)
                        {

                            objDealerQuotation = _objDPQ.GetDealerQuotationV2(cityId, objPQOutput.VersionId, objPQOutput.DealerId, objPQEntity.AreaId);

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
                                    Name = objDealerQuotation.PrimaryDealer.DealerDetails.Organization,
                                    IsPremiumDealer = objDealerQuotation.PrimaryDealer.IsPremiumDealer
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
                        }
                        else
                        {
                            var bikePriceQuotationList = _objPriceQuote.GetVersionPricesByModelId(modelId, cityId);
                            if (bikePriceQuotationList != null && bikePriceQuotationList.Any())
                            {
                                var selversion = bikePriceQuotationList.FirstOrDefault(x => x.VersionId == objPQOutput.VersionId);
                                if (selversion != null)
                                {
                                    versionPrice = (uint)selversion.OnRoadPrice;
                                }
                            }
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

                            if (cityId > 0 && objPQ.PQId > 0)
                            {
                                manufacturerCampaign = _objManufacturerCampaign.GetCampaigns(modelId, cityId, ManufacturerCampaign.Entities.ManufacturerCampaignServingPages.Mobile_Model_Page);
                                if (manufacturerCampaign != null && manufacturerCampaign.LeadCampaign != null && manufacturerCampaign.LeadCampaign.LeadsButtonTextMobile.ToLower() == "request callback")
                                {
                                    manufacturerCampaign.LeadCampaign.LeadsButtonTextMobile = "Get Best Offers";
                                }
                            }
                        }
                        onRoadPrice.Campaign = ManufacturerCampaignMapper.Convert((ushort)sourceType, objPQ.PQId, modelId, objPQ.VersionId, cityId, objDealerQuotation, manufacturerCampaign, versionPrice, objPQ.MakeName, objPQ.ModelName);
                    }
                    return Ok(onRoadPrice);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Exception : Bikewale.Service.Controllers.PriceQuote.OnRoadPriceController.GetV4");
                return InternalServerError();
            }
        }

        /// <summary>
        /// Created By  : Pratibha Verma on 19 June 2018
        /// Description : used method GetPriceQuoteByIdV2 to remove dependency from PQId
        /// </summary>
        /// <param name="cityId"></param>
        /// <param name="modelId"></param>
        /// <param name="clientIP"></param>
        /// <param name="sourceType"></param>
        /// <param name="deviceId"></param>
        /// <param name="areaId"></param>
        /// <param name="pqLeadId"></param>
        /// <returns></returns>
        [ResponseType(typeof(Bikewale.DTO.PriceQuote.v5.PQOnRoad)), Route("api/v5/OnRoadPrice")]
        public IHttpActionResult GetV5(uint cityId, uint modelId, string clientIP, PQSources sourceType, string deviceId = null, uint? areaId = null, ushort? pqLeadId = null)
        {
            Bikewale.DTO.PriceQuote.v3.PQOutput objPQ = null;
            Bikewale.Entities.BikeBooking.v2.PQOutputEntity objPQOutput = null;
            Bikewale.Entities.PriceQuote.v2.BikeQuotationEntity bpqOutput = null;
            Bikewale.DTO.PriceQuote.v5.PQOnRoad onRoadPrice = null;
            Bikewale.Entities.PriceQuote.v2.DetailedDealerQuotationEntity objDealerQuotation = null;
            Bikewale.ManufacturerCampaign.Entities.ManufacturerCampaignEntity manufacturerCampaign = null;
            uint versionPrice = 0;
            try
            {
                Entities.PriceQuote.v2.PriceQuoteParametersEntity objPQEntity = new Entities.PriceQuote.v2.PriceQuoteParametersEntity()
                {
                    CityId = cityId,
                    AreaId = areaId.HasValue ? areaId.Value : 0,
                    ClientIP = clientIP,
                    SourceId = (ushort)sourceType,
                    ModelId = modelId,
                    DeviceId = deviceId,
                    PQLeadId = pqLeadId
                };

                objPQOutput = _objIPQ.ProcessPQV3(objPQEntity);
                if (objPQOutput != null)
                {
                    onRoadPrice = new Bikewale.DTO.PriceQuote.v5.PQOnRoad();
                    objPQ = PQOutputMapper.ConvertV3(objPQOutput);
                    onRoadPrice.PriceQuote = objPQ;

                    if (objPQ != null && !String.IsNullOrEmpty(objPQ.PQId))
                    {
                        bpqOutput = _objPriceQuote.GetPriceQuote(objPQEntity.CityId, objPQOutput.VersionId);
                        //add bike make and model
                        objPQ.MakeName = bpqOutput.MakeName;
                        objPQ.ModelName = bpqOutput.ModelName;

                        bpqOutput.Varients = _objPriceQuoteCache.GetOtherVersionsPrices(modelId, cityId);

                        if (objPQ.DealerId > 0)
                        {

                            objDealerQuotation = _objDPQ.GetDealerQuotationV2(cityId, objPQOutput.VersionId, objPQOutput.DealerId, objPQEntity.AreaId);

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
                                    Name = objDealerQuotation.PrimaryDealer.DealerDetails.Organization,
                                    IsPremiumDealer = objDealerQuotation.PrimaryDealer.IsPremiumDealer
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
                        }
                        else
                        {
                            var bikePriceQuotationList = _objPriceQuote.GetVersionPricesByModelId(modelId, cityId);
                            if (bikePriceQuotationList != null && bikePriceQuotationList.Any())
                            {
                                var selversion = bikePriceQuotationList.FirstOrDefault(x => x.VersionId == objPQOutput.VersionId);
                                if (selversion != null)
                                {
                                    versionPrice = (uint)selversion.OnRoadPrice;
                                }
                            }
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

                            if (cityId > 0 && !String.IsNullOrEmpty(objPQ.PQId))
                            {
                                manufacturerCampaign = _objManufacturerCampaign.GetCampaigns(modelId, cityId, ManufacturerCampaign.Entities.ManufacturerCampaignServingPages.Mobile_Model_Page);
                            }
                        }
                        onRoadPrice.Campaign = ManufacturerCampaignMapper.ConvertV2((ushort)sourceType, objPQ.PQId, modelId, objPQ.VersionId, cityId, objDealerQuotation, manufacturerCampaign, versionPrice, objPQ.MakeName, objPQ.ModelName);
                    }
                    return Ok(onRoadPrice);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Exception : Bikewale.Service.Controllers.PriceQuote.OnRoadPriceController.GetV5");
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
        [ResponseType(typeof(DTO.PriceQuote.v3.DPQuotationOutput)), Route("api/dealerversionprices")]
        public IHttpActionResult GetDealerVersionPrices(uint cityId, string deviceId, uint dealerId, uint modelId, uint? areaId = null, uint? versionId = null)
        {
            try
            {
                bool isPQRegistered = Request.Headers.Contains("isRegistered") ? Convert.ToBoolean(Request.Headers.GetValues("isRegistered").FirstOrDefault()) : false;
                ulong pqId = Request.Headers.Contains("quoteId") ? Convert.ToUInt64(Request.Headers.GetValues("quoteId").FirstOrDefault()) : default(UInt64);
                ushort platformId = Request.Headers.Contains("platformId") ? Convert.ToUInt16(Request.Headers.GetValues("platformId").FirstOrDefault()) : default(UInt16);

                if (pqId > 0 && platformId > 0)
                {
                    PQ_QuotationEntity bwPQ = _objDPQ.Quotation(cityId, platformId, deviceId, dealerId, modelId, ref pqId, isPQRegistered, areaId, versionId);
                    DTO.PriceQuote.v2.DPQuotationOutput dpq = null;

                    DetailedDealerQuotationEntity objDealerQuotation = _objDPQ.GetDealerQuotation(cityId, versionId.HasValue ? versionId.Value : 0, dealerId);

                    dpq = PQBikePriceQuoteOutputMapper.Convert(objDealerQuotation, bwPQ.Varients);
                    dpq.PriceQuoteId = pqId;
                    if (objDealerQuotation != null && objDealerQuotation.PrimaryDealer != null && objDealerQuotation.PrimaryDealer.DealerDetails != null)
                    {

                        dpq.Dealer = PQBikePriceQuoteOutputMapper.Convert(objDealerQuotation.PrimaryDealer.DealerDetails);
                    }
                    else if (dealerId == 0) // Show ES campaign is available
                    {
                        Bikewale.ManufacturerCampaign.Entities.ManufacturerCampaignEntity manufactureCampaign = _objManufacturerCampaign.GetCampaigns(modelId, cityId, ManufacturerCampaign.Entities.ManufacturerCampaignServingPages.Mobile_Model_Page);
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
                ErrorClass.LogError(ex, "Exception : Bikewale.Service.Controllers.PriceQuote.OnRoadPriceController.GetDealerVersionPrices");
                return InternalServerError();
            }
        }

        /// <summary>
        /// Gets the dealer version prices v2.
        /// Creted by:  Sangram Nandkhile on 13 Oct 2017
        /// </summary>
        /// <returns></returns>
        [ResponseType(typeof(DTO.PriceQuote.v3.DPQuotationOutput)), Route("api/v2/dealerversionprices")]
        public IHttpActionResult GetDealerVersionPricesV2(uint cityId, string deviceId, uint dealerId, uint modelId, uint? areaId = null, uint? versionId = null)
        {
            DTO.PriceQuote.v3.DPQuotationOutput dpQuotationOutput = null;
            Bikewale.ManufacturerCampaign.Entities.ManufacturerCampaignEntity manufacturerCampaign = null;
            try
            {
                bool isPQRegistered = Request.Headers.Contains("isRegistered") ? Convert.ToBoolean(Request.Headers.GetValues("isRegistered").FirstOrDefault()) : false;
                ulong pqId = Request.Headers.Contains("quoteId") ? Convert.ToUInt64(Request.Headers.GetValues("quoteId").FirstOrDefault()) : default(ulong);
                ushort platformId = Request.Headers.Contains("platformId") ? Convert.ToUInt16(Request.Headers.GetValues("platformId").FirstOrDefault()) : default(ushort);

                if (pqId > 0 && platformId > 0)
                {
                    PQ_QuotationEntity bwPQ = _objDPQ.Quotation(cityId, platformId, deviceId, dealerId, modelId, ref pqId, isPQRegistered, areaId, versionId);
                    Entities.PriceQuote.v2.DetailedDealerQuotationEntity objDealerQuotation = _objDPQ.GetDealerQuotationV2(cityId, versionId.HasValue ? versionId.Value : 0, dealerId, areaId.HasValue ? areaId.Value : 0);
                    dpQuotationOutput = PQBikePriceQuoteOutputMapper.Convert(objDealerQuotation, bwPQ.Varients);
                    if (dpQuotationOutput != null)
                        dpQuotationOutput.PriceQuoteId = pqId;

                    manufacturerCampaign = _objManufacturerCampaign.GetCampaigns(modelId, cityId, ManufacturerCampaign.Entities.ManufacturerCampaignServingPages.Mobile_Model_Page);
                    dpQuotationOutput.Campaign = ManufacturerCampaignMapper.Convert(platformId, pqId, modelId, (uint)bwPQ.Varients.First().objVersion.VersionId, cityId, objDealerQuotation, manufacturerCampaign, bwPQ.Varients.FirstOrDefault().OnRoadPrice, bwPQ.Varients.FirstOrDefault().objMake.MakeName, bwPQ.Varients.FirstOrDefault().objModel.ModelName);
                    return Ok(dpQuotationOutput);
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Exception : Bikewale.Service.Controllers.PriceQuote.OnRoadPriceController.GetDealerVersionPricesV2");
                return InternalServerError();

            }
        }

        /// <summary>
        /// Created by  : Pratibha Verma on 19 October 2018
        /// Description : new version from 'api/dealerversionprices'  for PQId related changes
        /// </summary>
        /// <param name="cityId"></param>
        /// <param name="deviceId"></param>
        /// <param name="dealerId"></param>
        /// <param name="modelId"></param>
        /// <param name="areaId"></param>
        /// <param name="versionId"></param>
        /// <returns></returns>
        [ResponseType(typeof(DTO.PriceQuote.v3.DPQuotationOutput)), Route("api/v3/dealerversionprices")]
        public IHttpActionResult GetDealerVersionPricesV1(uint cityId, string deviceId, uint dealerId, uint modelId, uint? areaId = null, uint? versionId = null)
        {
            try
            {
                bool isPQRegistered = Request.Headers.Contains("isRegistered") ? Convert.ToBoolean(Request.Headers.GetValues("isRegistered").FirstOrDefault()) : false;
                string pqId = Request.Headers.Contains("quoteId") ? Request.Headers.GetValues("quoteId").FirstOrDefault() : string.Empty;
                ushort platformId = Request.Headers.Contains("platformId") ? Convert.ToUInt16(Request.Headers.GetValues("platformId").FirstOrDefault()) : default(UInt16);

                if (!string.IsNullOrEmpty(pqId) && platformId > 0)
                {
                    PQ_QuotationEntity bwPQ = _objDPQ.QuotationV2(cityId, platformId, deviceId, dealerId, modelId, ref pqId, isPQRegistered, areaId, versionId);
                    DTO.PriceQuote.v4.DPQuotationOutput dpq = null;

                    DetailedDealerQuotationEntity objDealerQuotation = _objDPQ.GetDealerQuotation(cityId, versionId.HasValue ? versionId.Value : 0, dealerId);

                    dpq = PQBikePriceQuoteOutputMapper.ConvertV2(objDealerQuotation, bwPQ.Varients);
                    dpq.PriceQuoteId = pqId;
                    if (objDealerQuotation != null && objDealerQuotation.PrimaryDealer != null && objDealerQuotation.PrimaryDealer.DealerDetails != null)
                    {

                        dpq.Dealer = PQBikePriceQuoteOutputMapper.Convert(objDealerQuotation.PrimaryDealer.DealerDetails);
                    }
                    else if (dealerId == 0) // Show ES campaign is available
                    {
                        Bikewale.ManufacturerCampaign.Entities.ManufacturerCampaignEntity manufactureCampaign = _objManufacturerCampaign.GetCampaigns(modelId, cityId, ManufacturerCampaign.Entities.ManufacturerCampaignServingPages.Mobile_Model_Page);
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
                ErrorClass.LogError(ex, "Exception : Bikewale.Service.Controllers.PriceQuote.OnRoadPriceController.GetDealerVersionPricesV1(), api/v3/dealerversionprices");
                return InternalServerError();
            }
        }
    }
}