using Bikewale.Entities.BikeBooking;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.Location;
using Bikewale.Entities.PriceQuote;
using Bikewale.Interfaces.BikeBooking;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.Location;
using Bikewale.Interfaces.PriceQuote;
using Bikewale.ManufacturerCampaign.Interface;
using Bikewale.Notifications;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
namespace Bikewale.BAL.PriceQuote
{
    /// <summary>
    /// Created By: Sangram Nandkhile on 13 Apr 2016
    /// summary   : Class to have a common logic to get pq by Model, city and area
    /// </summary>
    public class PQByCityArea : IPQByCityArea
    {

        private readonly ICityCacheRepository objcity = null;
        private readonly IBikeModels<BikeModelEntity, int> objClient = null;
        private readonly IPriceQuote objPq = null;
        private readonly IDealerPriceQuote objDealer = null;
        private readonly Interfaces.AutoBiz.IDealerPriceQuote objPriceQuote = null;
        private readonly IAreaCacheRepository objArea = null;
        private readonly IDealerPriceQuoteDetail objIPQ = null;
        private readonly IPriceQuoteCache _objPQCache = null;
        private readonly IManufacturerCampaign _objManufacturerCampaign = null;

        private readonly ILog _logger = LogManager.GetLogger("PQByCityArea");


        public PQByCityArea(ICityCacheRepository objcity,
            IBikeModels<BikeModelEntity, int> objClient,
            IPriceQuote objPq,
            IDealerPriceQuote objDealer,
            Interfaces.AutoBiz.IDealerPriceQuote objPriceQuote,
            IAreaCacheRepository objArea,
            IDealerPriceQuoteDetail objIPQ,
            IPriceQuoteCache _objPQCache,
            IManufacturerCampaign _objManufacturerCampaign)
        {
            this.objcity = objcity;
            this.objClient = objClient;
            this.objPq = objPq;
            this.objDealer = objDealer;
            this.objPriceQuote = objPriceQuote;
            this.objArea = objArea;
            this.objIPQ = objIPQ;
            this._objPQCache = _objPQCache;
            this._objManufacturerCampaign = _objManufacturerCampaign;
        }

        /// <summary>
        /// Created By: Sangram Nandkhile on 13 Apr 2016
        /// summary   : Get On road price of all the version for modelId
        /// </summary>
        /// <param name="modelId">model Id</param>
        /// <param name="variantId">Model's version id</param>
        /// <param name="cityId">City id (optional)</param>
        /// <param name="areaId">Area id (optional)</param>
        /// <returns></returns>
        public PQOnRoadPrice GetOnRoadPrice(int modelId, int? cityId = null, int? areaId = null, int? versionId = null, ushort? sourceId = null, string UTMA = null, string UTMZ = null, string DeviceId = null, string clientIP = null, string PQLeadId = null, bool isManufacturerCampaignRequired = false)
        {
            PQOnRoadPrice pqOnRoad = null;
            try
            {

                BikeQuotationEntity bpqOutput = null;
                PriceQuoteParametersEntity objPQEntity = new PriceQuoteParametersEntity();
                objPQEntity.CityId = Convert.ToUInt32(cityId);
                objPQEntity.AreaId = Convert.ToUInt32(areaId);
                objPQEntity.ClientIP = clientIP;
                objPQEntity.SourceId = Convert.ToUInt16(sourceId);
                objPQEntity.ModelId = Convert.ToUInt32(modelId);
                objPQEntity.VersionId = Convert.ToUInt32(versionId);
                objPQEntity.UTMA = UTMA;
                objPQEntity.UTMZ = UTMZ;
                objPQEntity.DeviceId = DeviceId;
                objPQEntity.ManufacturerCampaignPageId = ManufacturerCampaign.Entities.ManufacturerCampaignServingPages.Mobile_Model_Page;
                PQOutputEntity objPQOutput = objDealer.ProcessPQ(objPQEntity, isManufacturerCampaignRequired);
                if (objPQOutput != null)
                {
                    versionId = Convert.ToInt32(objPQOutput.VersionId);
                    pqOnRoad = new PQOnRoadPrice();
                    pqOnRoad.BaseVersion = objPQOutput.VersionId;
                    pqOnRoad.PriceQuote = objPQOutput;
                    if (objPQOutput != null && objPQOutput.PQId > 0)
                    {
                        bpqOutput = objPq.GetPriceQuoteById(objPQOutput.PQId);
                        bpqOutput.Varients = _objPQCache.GetOtherVersionsPrices(objPQEntity.ModelId, objPQEntity.CityId);
                        if (bpqOutput != null)
                        {
                            pqOnRoad.BPQOutput = bpqOutput;
                        }
                        if (objPQOutput.DealerId > 0 && pqOnRoad.PriceQuote != null && pqOnRoad.PriceQuote.ManufacturerCampaign == null)
                        {
                            PQ_QuotationEntity oblDealerPQ = null;
                            try
                            {
                                oblDealerPQ = GetDealePQEntity((uint)cityId, objPQOutput.DealerId, (uint)versionId);

                                if (oblDealerPQ != null)
                                {
                                    uint insuranceAmount = 0;
                                    foreach (var price in oblDealerPQ.PriceList)
                                    {
                                        pqOnRoad.IsInsuranceFree = Bikewale.Utility.DealerOfferHelper.HasFreeInsurance(objPQOutput.DealerId.ToString(), "", price.CategoryName, price.Price, ref insuranceAmount);
                                    }
                                    pqOnRoad.IsInsuranceFree = true;
                                    pqOnRoad.DPQOutput = oblDealerPQ;
                                    pqOnRoad.InsuranceAmount = insuranceAmount;
                                }
                            }

                            catch (Exception ex)
                            {
                                ErrorClass.LogError(ex, "PQByCityArea: " + "GetOnRoadPrice");

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "PQByCityArea: " + "GetOnRoadPrice");

            }
            return pqOnRoad;
        }

        /// <summary>
        /// Created By  : Pratibha Verma on 19 June 2018
        /// Description : 'GetPriceQuote' method used to remove PQId dependency
        /// </summary>
        /// <param name="modelId"></param>
        /// <param name="cityId"></param>
        /// <param name="areaId"></param>
        /// <param name="versionId"></param>
        /// <param name="sourceId"></param>
        /// <param name="UTMA"></param>
        /// <param name="UTMZ"></param>
        /// <param name="DeviceId"></param>
        /// <param name="clientIP"></param>
        /// <param name="PQLeadId"></param>
        /// <returns></returns>
        public Bikewale.Entities.PriceQuote.v2.PQOnRoadPrice GetOnRoadPriceV2(int modelId, int? cityId = null, int? areaId = null, int? versionId = null, ushort? sourceId = null, string UTMA = null, string UTMZ = null, string DeviceId = null, string clientIP = null, string PQLeadId = null, bool isManufacturerCampaignRequired = false)
        {
            Bikewale.Entities.PriceQuote.v2.PQOnRoadPrice pqOnRoad = null;
            try
            {

                Bikewale.Entities.PriceQuote.v2.BikeQuotationEntity bpqOutput = null;
                Entities.PriceQuote.v2.PriceQuoteParametersEntity objPQEntity = new Entities.PriceQuote.v2.PriceQuoteParametersEntity();
                objPQEntity.CityId = Convert.ToUInt32(cityId);
                objPQEntity.AreaId = Convert.ToUInt32(areaId);
                objPQEntity.ClientIP = clientIP;
                objPQEntity.SourceId = Convert.ToUInt16(sourceId);
                objPQEntity.ModelId = Convert.ToUInt32(modelId);
                objPQEntity.VersionId = Convert.ToUInt32(versionId);
                objPQEntity.UTMA = UTMA;
                objPQEntity.UTMZ = UTMZ;
                objPQEntity.DeviceId = DeviceId;
                objPQEntity.ManufacturerCampaignPageId = ManufacturerCampaign.Entities.ManufacturerCampaignServingPages.Mobile_Model_Page;
                Bikewale.Entities.BikeBooking.v2.PQOutputEntity objPQOutput = objDealer.ProcessPQV4(objPQEntity, isManufacturerCampaignRequired);
                if (objPQOutput != null)
                {
                    versionId = Convert.ToInt32(objPQOutput.VersionId);
                    pqOnRoad = new Bikewale.Entities.PriceQuote.v2.PQOnRoadPrice();
                    pqOnRoad.BaseVersion = objPQOutput.VersionId;
                    pqOnRoad.PriceQuote = objPQOutput;
                    if (objPQOutput != null && !string.IsNullOrEmpty(objPQOutput.PQId))
                    {
                        bpqOutput = objPq.GetPriceQuote(objPQEntity.CityId, objPQOutput.VersionId);
                        bpqOutput.PriceQuoteId = objPQOutput.PQId;
                        bpqOutput.Varients = _objPQCache.GetOtherVersionsPrices(objPQEntity.ModelId, objPQEntity.CityId);
                        if (bpqOutput != null)
                        {
                            pqOnRoad.BPQOutput = bpqOutput;
                        }
                        if (objPQOutput.DealerId > 0)
                        {
                            PQ_QuotationEntity oblDealerPQ = null;
                            try
                            {
                                oblDealerPQ = GetDealePQEntity((uint)cityId, objPQOutput.DealerId, (uint)versionId);

                                if (oblDealerPQ != null)
                                {
                                    uint insuranceAmount = 0;
                                    foreach (var price in oblDealerPQ.PriceList)
                                    {
                                        pqOnRoad.IsInsuranceFree = Bikewale.Utility.DealerOfferHelper.HasFreeInsurance(objPQOutput.DealerId.ToString(), "", price.CategoryName, price.Price, ref insuranceAmount);
                                    }
                                    pqOnRoad.IsInsuranceFree = true;
                                    pqOnRoad.DPQOutput = oblDealerPQ;
                                    pqOnRoad.InsuranceAmount = insuranceAmount;
                                }
                            }

                            catch (Exception ex)
                            {
                                ErrorClass.LogError(ex, "PQByCityArea: " + "GetOnRoadPrice");

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "PQByCityArea: " + "GetOnRoadPrice");

            }
            return pqOnRoad;
        }

        /// <summary>
        /// Author          :   Sangram Nandkhile
        /// Created Date    :   24 Nov 2015
        /// Description     :   Gets City Details by ModelId from cache
        /// </summary>
        /// <param name="modelId">Model Id</param>
        public IEnumerable<CityEntityBase> FetchCityByModelId(int modelId)
        {
            IEnumerable<CityEntityBase> cityList = null;
            try
            {

                cityList = objcity.GetPriceQuoteCities(Convert.ToUInt16(modelId));
                return cityList;
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "ModelPageController" + "FetchCityByModelId");

            }
            return cityList;
        }

        /// <summary>
        /// Author          :   Sangram Nandkhile
        /// Created Date    :   24 Nov 2015
        /// Description     :   Get List of Area depending on City and Model Id from cache
        /// </summary>
        public IEnumerable<Bikewale.Entities.Location.AreaEntityBase> GetAreaForCityAndModel(int modelId, int cityId)
        {
            IEnumerable<Bikewale.Entities.Location.AreaEntityBase> areaList = null;
            try
            {


                areaList = objArea.GetAreaList(Convert.ToUInt32(modelId), Convert.ToUInt32(cityId));
                return areaList;
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "ModelPageController" + "GetAreaForCityAndModel");

            }

            return areaList;
        }

        /// <summary>
        /// Created by: Sangram Nandkhile on 15 Apr 2016
        /// Summary:To map versionList Object for V3 model entity
        /// </summary>
        /// <param name="objModelPage"></param>
        /// <returns></returns>
        public PQByCityAreaEntity GetVersionList(int modelID, IEnumerable<BikeVersionMinSpecs> modelVersions, int? cityId, int? areaId, ushort? sourceId, string UTMA = null, string UTMZ = null, string DeviceId = null, string clientIP = null)
        {
            PQByCityAreaEntity pqEntity = new PQByCityAreaEntity();
            try
            {
                PQOnRoadPrice pqOnRoad = null;
                if (cityId > 0)
                {
                    IEnumerable<CityEntityBase> cityList = FetchCityByModelId(modelID);
                    CityEntityBase selectedCity = null;
                    if (cityList != null)
                    {
                        selectedCity = cityList.FirstOrDefault(p => p.CityId == cityId);
                    }
                    pqEntity.IsCityExists = selectedCity != null;
                    if (pqEntity.IsCityExists && cityList != null)
                    {
                        pqEntity.City = selectedCity;
                        var areaList = GetAreaForCityAndModel(modelID, Convert.ToInt16(cityId));
                        pqEntity.IsAreaExists = (areaList != null && areaList.Any());
                        // If area is provided, check if area exists in list
                        if (areaId > 0 && pqEntity.IsAreaExists)
                        {
                            pqEntity.IsAreaSelected = areaList != null && areaList.Any(p => p.AreaId == areaId);
                            pqEntity.Area = areaList.FirstOrDefault(p => p.AreaId == areaId);
                        }
                        if (selectedCity.HasAreas && pqEntity.IsAreaSelected)
                        {
                            pqOnRoad = GetOnRoadPrice(modelID, cityId, areaId, null, sourceId, UTMA, UTMZ, DeviceId, clientIP, isManufacturerCampaignRequired: false);
                        }
                        else
                        {
                            pqOnRoad = GetOnRoadPrice(modelID, cityId, areaId, null, sourceId, UTMA, UTMZ, DeviceId, clientIP, isManufacturerCampaignRequired: false);
                        }
                        if (pqOnRoad != null)
                        {
                            pqEntity.PqId = pqOnRoad.PriceQuote.PQId;
                            pqEntity.DealerId = pqOnRoad.PriceQuote.DealerId;
                            pqEntity.IsExShowroomPrice = pqOnRoad.DPQOutput == null && pqOnRoad.BPQOutput == null;

                            // When City has areas and area is not selected then show ex-showrrom price so user can select it
                            bool isAreaExistAndSelected = ((pqEntity.IsAreaExists && pqEntity.IsAreaSelected) || !selectedCity.HasAreas);
                            // when DPQ OR Only city level pricing exists
                            if (isAreaExistAndSelected || (!pqEntity.IsAreaExists))
                            {
                                #region  Iterate over version to fetch Dealer PQ or BikeWalePQ

                                foreach (var version in modelVersions)
                                {
                                    if (pqOnRoad.DPQOutput != null)
                                    {
                                        var selected = pqOnRoad.DPQOutput.Varients.Where(p => p.objVersion.VersionId == version.VersionId).FirstOrDefault();
                                        if (selected != null)
                                        {
                                            version.Price = selected.OnRoadPrice;
                                            version.IsDealerPriceQuote = true;
                                        }
                                        else if (pqOnRoad.BPQOutput != null && pqOnRoad.BPQOutput.Varients != null)
                                        {
                                            var selectedBPQ = pqOnRoad.BPQOutput.Varients.Where(p => p.VersionId == version.VersionId).FirstOrDefault();
                                            if (selectedBPQ != null)
                                            {
                                                version.Price = selectedBPQ.OnRoadPrice;
                                                version.IsDealerPriceQuote = false;
                                            }
                                        }
                                    }
                                    else if (pqOnRoad.BPQOutput != null && pqOnRoad.BPQOutput.Varients != null)
                                    {
                                        var selectedBPQ = pqOnRoad.BPQOutput.Varients.Where(p => p.VersionId == version.VersionId).FirstOrDefault();
                                        if (selectedBPQ != null)
                                        {
                                            version.Price = selectedBPQ.OnRoadPrice;
                                            version.IsDealerPriceQuote = false;
                                        }
                                    }
                                }
                                #endregion
                            }
                            else
                            {
                                pqEntity.IsExShowroomPrice = true;
                            }
                        }
                        else
                        {
                            pqEntity.IsExShowroomPrice = true;
                        }
                    }
                    else // Show mumbai Ex showroom price
                    {
                        pqEntity.IsExShowroomPrice = true;
                    }

                }
                else if (cityId == null)
                {
                    pqEntity.IsExShowroomPrice = true;
                }
                // Check if bike has more than 1 version and send base version as the first version in VersionList
                if (modelVersions != null && modelVersions.Count() > 1 && pqOnRoad != null && pqOnRoad.BaseVersion > 0)
                {
                    modelVersions = SwapVersionList(modelVersions.ToList(), pqOnRoad.BaseVersion);
                }
                pqEntity.VersionList = modelVersions;
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Exception : PQByCityArea GetVersionList");
            }
            return pqEntity;
        }

        /// <summary>
        /// Created By  : Sushil Kumar on 1st August 2016
        /// Description : To generate pricequote dependent on modelid,cityid and areaId
        /// </summary>
        /// <returns></returns>
        public Bikewale.Entities.PriceQuote.v2.PQByCityAreaEntity GetPriceQuoteByCityArea(PriceQuoteParametersEntity pqInput, bool isReload)
        {
            Bikewale.Entities.PriceQuote.v2.PQByCityAreaEntity pqOutput = null;
            try
            {
                pqOutput = new Bikewale.Entities.PriceQuote.v2.PQByCityAreaEntity();

                IEnumerable<CityEntityBase> cityList = FetchCityByModelId(Convert.ToInt32(pqInput.ModelId));
                if (pqInput.CityId > 0)
                {
                    //check if valid city id from the list

                    CityEntityBase selectedCity = null;
                    if (cityList != null)
                    {
                        selectedCity = cityList.FirstOrDefault(p => p.CityId == pqInput.CityId);
                        if (selectedCity != null)
                        {
                            if (selectedCity.HasAreas)
                            {
                                IEnumerable<Bikewale.Entities.Location.AreaEntityBase> areaList = null;

                                if (!(pqInput.AreaId > 0))
                                    areaList = GetAreaForCityAndModel(Convert.ToInt32(pqInput.ModelId), Convert.ToInt32(pqInput.CityId));


                                if (pqInput.AreaId > 0)
                                {
                                    if (!isReload)
                                    {
                                        PQOutputEntity priceQuote = objDealer.ProcessPQ(pqInput);
                                        if (priceQuote != null)
                                        {
                                            pqOutput.PriceQuote = priceQuote;
                                        }
                                    }
                                }
                                else  //selected area is not in the list
                                {
                                    pqOutput.PQCitites = cityList;
                                    pqOutput.PQAreas = areaList;
                                }
                            }
                            else //when city exists and no areas for that city exists show bikewale pricequote (hasareas for that city is false)
                            {
                                if (!isReload)
                                {
                                    pqOutput.PriceQuote = objDealer.ProcessPQ(pqInput);
                                }
                            }

                        }
                        else // when selected city is not in the list show cities list 
                        {
                            pqOutput.PQCitites = cityList;
                        }
                    }
                }
                else //if (pqInput.ModelId > 0)  //when city is not selected (default behaviour)
                {
                    pqOutput.PQCitites = cityList;
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Exception : PQByCityArea GetPriceQuoteByCityArea");

            }
            return pqOutput;

        }


        /// <summary>
        /// Created By  : Kartik Rathod on 20 jun 2018,
		/// Description : created new version for price quote changes
        /// </summary>
        /// <returns></returns>
        public Bikewale.Entities.PriceQuote.v4.PQByCityAreaEntity GetPriceQuoteByCityAreaV2(Entities.PriceQuote.v2.PriceQuoteParametersEntity pqInput, bool isReload)
        {
            Bikewale.Entities.PriceQuote.v4.PQByCityAreaEntity pqOutput = null;
            try
            {
                pqOutput = new Bikewale.Entities.PriceQuote.v4.PQByCityAreaEntity();

                IEnumerable<CityEntityBase> cityList = FetchCityByModelId(Convert.ToInt32(pqInput.ModelId));
                if (pqInput.CityId > 0)
                {
                    //check if valid city id from the list

                    CityEntityBase selectedCity = null;
                    if (cityList != null)
                    {
                        selectedCity = cityList.FirstOrDefault(p => p.CityId == pqInput.CityId);
                        if (selectedCity != null)
                        {
                            if (selectedCity.HasAreas)
                            {
                                IEnumerable<Bikewale.Entities.Location.AreaEntityBase> areaList = null;

                                if (!(pqInput.AreaId > 0))
                                    areaList = GetAreaForCityAndModel(Convert.ToInt32(pqInput.ModelId), Convert.ToInt32(pqInput.CityId));
                                if (pqInput.AreaId > 0)
                                {
                                    if (!isReload)
                                    {
                                        Bikewale.Entities.BikeBooking.v2.PQOutputEntity priceQuote = objDealer.ProcessPQV2(pqInput);
                                        if (priceQuote != null)
                                        {
                                            pqOutput.PriceQuote = priceQuote;
                                        }
                                    }
                                }
                                else  //selected area is not in the list
                                {
                                    pqOutput.PQCitites = cityList;
                                    pqOutput.PQAreas = areaList;
                                }
                            }
                            else //when city exists and no areas for that city exists show bikewale pricequote (hasareas for that city is false)
                            {
                                if (!isReload)
                                {
                                    pqOutput.PriceQuote = objDealer.ProcessPQV2(pqInput);
                                }
                            }

                        }
                        else // when selected city is not in the list show cities list 
                        {
                            pqOutput.PQCitites = cityList;
                        }
                    }
                }
                else //if (pqInput.ModelId > 0)  //when city is not selected (default behaviour)
                {
                    pqOutput.PQCitites = cityList;
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Exception : PQByCityArea GetPriceQuoteByCityAreaV2");

            }
            return pqOutput;

        }

        /// <summary>
        /// Created by: Sangram Nandkhile on 14 Apr 2016
        /// Put BaseVersion in the list as Zero'th element
        /// </summary>
        /// <param name="objDTOModelPage"></param>
        /// <param name="pqOnRoad"></param>
        /// <returns></returns>
        public IEnumerable<BikeVersionMinSpecs> SwapVersionList(List<BikeVersionMinSpecs> modelVersions, uint baseVersion)
        {
            try
            {
                int baseVersionPos = modelVersions.FindIndex(p => p.VersionId == baseVersion);
                if (baseVersionPos != -1)
                {
                    var tempVersion = modelVersions[0];
                    modelVersions[0] = modelVersions[baseVersionPos];
                    modelVersions[baseVersionPos] = tempVersion;
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Exception : PQByCityArea .SwapVersionList");

            }
            return modelVersions;
        }

        /// <summary>
        /// Created by: Sangram Nandkhile on 01-Jul-2016
        /// Summary: Moving Autobiz dealerPQ API call to Code
        /// </summary>
        /// <param name="dealerId"></param>
        /// <param name="versionId"></param>
        /// <returns></returns>
        private PQ_QuotationEntity GetDealePQEntity(uint cityId, uint dealerId, uint versionId)
        {
            PQ_QuotationEntity objDealerPrice = default(PQ_QuotationEntity);

            PQParameterEntity objParam = new PQParameterEntity();
            objParam.CityId = cityId;
            objParam.DealerId = dealerId;
            objParam.VersionId = versionId;
            objDealerPrice = objPriceQuote.GetDealerPriceQuote(objParam);
            return objDealerPrice;
        }


        /// <summary>
        /// Created by: vivek gupta on 17 june 2016
        /// Summary:To map versionList Object for V3 model entity
        ///         and adding dealer packages , secondary dealers count, 
        /// Modified by :   Monika Korrapati on 27 Sept 2018
        /// Description :   versionID will be fetched from cache layer.
        /// Modifier    : Kartik Rathod on 8 nov 2018
        /// Desc        : in case of city not provided, call getonroadprice function, to fetch all all india level campaigns
        /// </summary>
        /// <param name="objModelPage"></param>
        /// <returns></returns>
        public PQByCityAreaEntity GetVersionListV2(int modelId, IEnumerable<BikeVersionMinSpecs> modelVersions,
            int? cityId, int? areaId, ushort? sourceId, string UTMA = null,
            string UTMZ = null, string DeviceId = null, string clientIP = null)
        {
            PQByCityAreaEntity pqEntity = new PQByCityAreaEntity();
            uint versionID = 0;
            bool isAreaExistAndSelected = false;

            DateTime dt1, dt2, dt3, dt4;
            dt1 = DateTime.Now;

            try
            {
                dt3 = DateTime.Now;
                PQOnRoadPrice pqOnRoad = null;
                if (cityId > 0)
                {
                    
                    IEnumerable<CityEntityBase> cityList = FetchCityByModelId(modelId);
                    CityEntityBase selectedCity = null;
                    if (cityList != null)
                    {
                        selectedCity = cityList.FirstOrDefault(p => p.CityId == cityId);
                    }
                    pqEntity.IsCityExists = selectedCity != null;
                    if (pqEntity.IsCityExists)
                    {
                        pqEntity.City = selectedCity;
                        var areaList = GetAreaForCityAndModel(modelId, Convert.ToInt16(cityId));
                        pqEntity.IsAreaExists = (areaList != null && areaList.Any()) && selectedCity.HasAreas;
                        // If area is provided, check if area exists in list
                        if (areaId > 0 && pqEntity.IsAreaExists)
                        {
                            pqEntity.Area = areaList.FirstOrDefault(p => p.AreaId == areaId);
                            pqEntity.IsAreaSelected = pqEntity.Area != null;
                        }
                        pqOnRoad = GetOnRoadPrice(modelId, cityId, areaId, null, sourceId, UTMA, UTMZ, DeviceId, clientIP, isManufacturerCampaignRequired: true);

                        if (pqOnRoad != null)
                        {
                            pqEntity.PqId = pqOnRoad.PriceQuote.PQId;
                            pqEntity.DealerId = pqOnRoad.PriceQuote.DealerId;
                            pqEntity.IsExShowroomPrice = pqOnRoad.DPQOutput == null && pqOnRoad.BPQOutput == null;

                            // When City has areas and area is not selected then show ex-showrrom price so user can select it
                            isAreaExistAndSelected = ((pqEntity.IsAreaExists && pqEntity.IsAreaSelected) || !selectedCity.HasAreas);
                            // when DPQ OR Only city level pricing exists
                            if (isAreaExistAndSelected || (!pqEntity.IsAreaExists))
                            {


                                if (pqOnRoad.DPQOutput != null && pqOnRoad.DPQOutput.Varients != null && pqOnRoad.DPQOutput.Varients.Any())
                                {
                                    var dpQOutputVarients = pqOnRoad.DPQOutput.Varients;

                                    #region  Iterate over version to fetch Dealer PQ or BikeWalePQ
                                    foreach (var version in modelVersions)
                                    {
                                        var selected = dpQOutputVarients.FirstOrDefault(p => p.objVersion.VersionId == version.VersionId);
                                        if (selected != null)
                                        {
                                            version.Price = selected.OnRoadPrice;
                                            version.IsDealerPriceQuote = true;
                                            versionID = Convert.ToUInt32(version.VersionId);
                                        }
                                        else if (pqOnRoad.BPQOutput != null && pqOnRoad.BPQOutput.Varients != null)
                                        {
                                            var selectedBPQ = pqOnRoad.BPQOutput.Varients.FirstOrDefault(p => p.VersionId == version.VersionId);
                                            if (selectedBPQ != null)
                                            {
                                                version.Price = selectedBPQ.OnRoadPrice;
                                                version.IsDealerPriceQuote = false;
                                            }
                                        }
                                    }
                                    #endregion
                                }
                                else if (pqOnRoad.BPQOutput != null && pqOnRoad.BPQOutput.Varients != null)
                                {
                                    #region  Iterate over version to fetch Dealer PQ or BikeWalePQ
                                    foreach (var version in modelVersions)
                                    {
                                        var selectedBPQ = pqOnRoad.BPQOutput.Varients.FirstOrDefault(p => p.VersionId == version.VersionId);
                                        if (selectedBPQ != null)
                                        {
                                            version.Price = selectedBPQ.OnRoadPrice;
                                            version.IsDealerPriceQuote = false;
                                        }
                                    }
                                    #endregion
                                }
                                
                            }
                            else
                            {
                                pqEntity.IsExShowroomPrice = true;
                            }
                        }
                        else
                        {
                            pqEntity.IsExShowroomPrice = true;
                        }
                    }
                    else // Show mumbai Ex showroom price
                    {
                        pqEntity.IsExShowroomPrice = true;
                    }

                }
                else if (cityId == null || cityId.Value == 0)
                {
                    pqEntity.IsExShowroomPrice = true;
                    pqOnRoad = GetOnRoadPrice(modelId, cityId, areaId, null, sourceId, UTMA, UTMZ, DeviceId, clientIP, isManufacturerCampaignRequired: true);
                }

                dt4 = DateTime.Now;
                ThreadContext.Properties["GetVersionListV2_1"] = (dt4 - dt3).TotalMilliseconds;
                dt3 = DateTime.Now;
                // Check if bike has more than 1 version and send base version as the first version in VersionList
                if (modelVersions != null && modelVersions.Any() && pqOnRoad != null && pqOnRoad.BaseVersion > 0)
                {
                    modelVersions = SwapVersionList(modelVersions.ToList(), pqOnRoad.BaseVersion);
                }
                pqEntity.VersionList = modelVersions;

                //below block added to get dealers pakage and offers and seondary dealers count

                if (versionID <= 0)
                {
                    if (areaId.HasValue && areaId.Value > 0)
                        versionID = objDealer.GetDefaultPriceQuoteVersion((uint)modelId, (uint)cityId.Value, (uint)areaId.Value);
                    else
                    {
                        versionID = _objPQCache.GetDefaultPriceQuoteVersion((uint)modelId, Convert.ToUInt32(cityId));
                    }       
                }

                dt4 = DateTime.Now;
                ThreadContext.Properties["GetVersionListV2_2"] = (dt4 - dt3).TotalMilliseconds;
                dt3 = DateTime.Now;

                if (cityId > 0 && versionID > 0 && pqOnRoad != null)
                {
                    Bikewale.Entities.PriceQuote.v2.DetailedDealerQuotationEntity detailedDealer = null;
                    detailedDealer = objIPQ.GetDealerQuotationV2(Convert.ToUInt32(cityId), versionID, pqEntity.DealerId, areaId.HasValue && areaId.Value > 0 ? (uint)areaId.Value : 0);
                    pqEntity.DealerEntity = detailedDealer;
                    if (isAreaExistAndSelected || (!pqEntity.IsAreaExists))
                    {
                        pqEntity.PrimaryDealer = detailedDealer.PrimaryDealer != null && detailedDealer.PrimaryDealer.DealerDetails != null ? detailedDealer.PrimaryDealer : null;
                        pqEntity.SecondaryDealerCount = detailedDealer.SecondaryDealerCount;
                        pqEntity.IsPremium = pqEntity.PrimaryDealer != null && detailedDealer.PrimaryDealer.IsPremiumDealer;
                    }
                }

                dt4 = DateTime.Now;
                ThreadContext.Properties["GetVersionListV2_3"] = (dt4 - dt3).TotalMilliseconds;
                dt3 = DateTime.Now;

                // Fetch ES only when Primary dealer is absent for given model
                // ES campaign should be shown even if the secondary dealers are found
                // Updated by: Sangram Nandkhile on 13 Oct 2017
                // modified by : Monika Korrapati on 21 Sept 2018
                // Description : Changed LeadsButtonTextMobile value to "Get Best Offers" in default case.
                if (pqEntity.PrimaryDealer == null && _objManufacturerCampaign != null)
                {
                    pqEntity.ManufacturerCampaign = pqOnRoad != null && pqOnRoad.PriceQuote != null ? pqOnRoad.PriceQuote.ManufacturerCampaign : null;

                    if ((cityId == null || cityId.Value == 0)
                        && pqEntity.ManufacturerCampaign != null
                        && pqEntity.ManufacturerCampaign.LeadCampaign != null
                        && !pqEntity.ManufacturerCampaign.LeadCampaign.ShowOnExshowroom)
                    {
                        pqEntity.ManufacturerCampaign = null;
                    }

                    if(pqEntity.ManufacturerCampaign != null && pqEntity.ManufacturerCampaign.LeadCampaign != null && pqEntity.ManufacturerCampaign.LeadCampaign.LeadsButtonTextMobile.ToLower() == "request callback")
                    {
                        pqEntity.ManufacturerCampaign.LeadCampaign.LeadsButtonTextMobile = "Get Best Offers";
                    }

                    if (pqEntity.PqId == 0 && cityId > 0)
                    {
                        PriceQuoteParametersEntity objPQEntity = new PriceQuoteParametersEntity();
                        objPQEntity.CityId = Convert.ToUInt16(cityId);
                        objPQEntity.AreaId = Convert.ToUInt32(areaId);
                        objPQEntity.ClientIP = "";
                        objPQEntity.SourceId = Convert.ToUInt16(sourceId ?? 0);
                        objPQEntity.ModelId = (uint)modelId;
                        objPQEntity.VersionId = versionID;
                        objPQEntity.PQLeadId = (int)PQSourceEnum.Mobile_ModelPage;
                        objPQEntity.UTMA = UTMA;
                        objPQEntity.UTMZ = UTMZ;
                        objPQEntity.DeviceId = DeviceId;
                        pqEntity.PqId = (uint)objPq.RegisterPriceQuote(objPQEntity);


                    }
                }
                dt4 = DateTime.Now;
                ThreadContext.Properties["GetVersionListV2_4"] = (dt4 - dt3).TotalMilliseconds;
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Exception : PQByCityArea GetVersionList");
            }
            finally
            {
                dt2 = DateTime.Now;
                ThreadContext.Properties["GetVersionListV2_Total"] = (dt2 - dt1).TotalMilliseconds;                
                _logger.Info("GetVersionListV2");                
            }
            return pqEntity;
        }

        /// <summary>
        /// Created by  : Pratibha Verma on 12 October 2018
        /// Description : new version from GetVersionListV2 for PQId releated changes
        /// </summary>
        /// <param name="modelId"></param>
        /// <param name="modelVersions"></param>
        /// <param name="cityId"></param>
        /// <param name="areaId"></param>
        /// <param name="sourceId"></param>
        /// <param name="UTMA"></param>
        /// <param name="UTMZ"></param>
        /// <param name="DeviceId"></param>
        /// <param name="clientIP"></param>
        /// <returns></returns>
        public Bikewale.Entities.PriceQuote.v3.PQByCityAreaEntity GetVersionListV3(int modelId, IEnumerable<BikeVersionMinSpecs> modelVersions,
            int? cityId, int? areaId, ushort? sourceId, string UTMA = null,
            string UTMZ = null, string DeviceId = null, string clientIP = null)
        {
            Bikewale.Entities.PriceQuote.v3.PQByCityAreaEntity pqEntity = new Bikewale.Entities.PriceQuote.v3.PQByCityAreaEntity();
            uint versionID = 0;
            bool isAreaExistAndSelected = false;

            DateTime dt1, dt2, dt3, dt4;
            dt1 = DateTime.Now;

            try
            {
                dt3 = DateTime.Now;
                Bikewale.Entities.PriceQuote.v2.PQOnRoadPrice pqOnRoad = null;
                if (cityId > 0)
                {

                    IEnumerable<CityEntityBase> cityList = FetchCityByModelId(modelId);
                    CityEntityBase selectedCity = null;
                    if (cityList != null)
                    {
                        selectedCity = cityList.FirstOrDefault(p => p.CityId == cityId);
                    }
                    pqEntity.IsCityExists = selectedCity != null;
                    if (pqEntity.IsCityExists)
                    {
                        pqEntity.City = selectedCity;
                        var areaList = GetAreaForCityAndModel(modelId, Convert.ToInt16(cityId));
                        pqEntity.IsAreaExists = (areaList != null && areaList.Any()) && selectedCity.HasAreas;
                        // If area is provided, check if area exists in list
                        if (areaId > 0 && pqEntity.IsAreaExists)
                        {
                            pqEntity.Area = areaList.FirstOrDefault(p => p.AreaId == areaId);
                            pqEntity.IsAreaSelected = pqEntity.Area != null;
                        }
                        pqOnRoad = GetOnRoadPriceV2(modelId, cityId, areaId, null, sourceId, UTMA, UTMZ, DeviceId, clientIP, isManufacturerCampaignRequired: true);

                        if (pqOnRoad != null)
                        {
                            pqEntity.PqId = pqOnRoad.PriceQuote.PQId;
                            pqEntity.DealerId = pqOnRoad.PriceQuote.DealerId;
                            pqEntity.IsExShowroomPrice = pqOnRoad.DPQOutput == null && pqOnRoad.BPQOutput == null;

                            // When City has areas and area is not selected then show ex-showrrom price so user can select it
                            isAreaExistAndSelected = ((pqEntity.IsAreaExists && pqEntity.IsAreaSelected) || !selectedCity.HasAreas);
                            // when DPQ OR Only city level pricing exists
                            if (isAreaExistAndSelected || (!pqEntity.IsAreaExists))
                            {


                                if (pqOnRoad.DPQOutput != null && pqOnRoad.DPQOutput.Varients != null && pqOnRoad.DPQOutput.Varients.Any())
                                {
                                    var dpQOutputVarients = pqOnRoad.DPQOutput.Varients;

                                    #region  Iterate over version to fetch Dealer PQ or BikeWalePQ
                                    foreach (var version in modelVersions)
                                    {
                                        var selected = dpQOutputVarients.FirstOrDefault(p => p.objVersion.VersionId == version.VersionId);
                                        if (selected != null)
                                        {
                                            version.Price = selected.OnRoadPrice;
                                            version.IsDealerPriceQuote = true;
                                            versionID = Convert.ToUInt32(version.VersionId);
                                        }
                                        else if (pqOnRoad.BPQOutput != null && pqOnRoad.BPQOutput.Varients != null)
                                        {
                                            var selectedBPQ = pqOnRoad.BPQOutput.Varients.FirstOrDefault(p => p.VersionId == version.VersionId);
                                            if (selectedBPQ != null)
                                            {
                                                version.Price = selectedBPQ.OnRoadPrice;
                                                version.IsDealerPriceQuote = false;
                                            }
                                        }
                                    }
                                    #endregion
                                }
                                else if (pqOnRoad.BPQOutput != null && pqOnRoad.BPQOutput.Varients != null)
                                {
                                    #region  Iterate over version to fetch Dealer PQ or BikeWalePQ
                                    foreach (var version in modelVersions)
                                    {
                                        var selectedBPQ = pqOnRoad.BPQOutput.Varients.FirstOrDefault(p => p.VersionId == version.VersionId);
                                        if (selectedBPQ != null)
                                        {
                                            version.Price = selectedBPQ.OnRoadPrice;
                                            version.IsDealerPriceQuote = false;
                                        }
                                    }
                                    #endregion
                                }

                            }
                            else
                            {
                                pqEntity.IsExShowroomPrice = true;
                            }
                        }
                        else
                        {
                            pqEntity.IsExShowroomPrice = true;
                        }
                    }
                    else // Show mumbai Ex showroom price
                    {
                        pqEntity.IsExShowroomPrice = true;
                    }

                }
                else if (cityId == null || cityId.Value == 0)
                {
                    pqEntity.IsExShowroomPrice = true;
                    pqOnRoad = GetOnRoadPriceV2(modelId, cityId, areaId, null, sourceId, UTMA, UTMZ, DeviceId, clientIP, isManufacturerCampaignRequired: true);
                }

                dt4 = DateTime.Now;
                ThreadContext.Properties["GetVersionListV3_1"] = (dt4 - dt3).TotalMilliseconds;
                dt3 = DateTime.Now;
                // Check if bike has more than 1 version and send base version as the first version in VersionList
                if (modelVersions != null && modelVersions.Any() && pqOnRoad != null && pqOnRoad.BaseVersion > 0)
                {
                    modelVersions = SwapVersionList(modelVersions.ToList(), pqOnRoad.BaseVersion);
                }
                pqEntity.VersionList = modelVersions;

                //below block added to get dealers pakage and offers and seondary dealers count

                if (versionID <= 0)
                {
                    if (areaId.HasValue && areaId.Value > 0)
                        versionID = objDealer.GetDefaultPriceQuoteVersion((uint)modelId, (uint)cityId.Value, (uint)areaId.Value);
                    else
                    {
                        versionID = _objPQCache.GetDefaultPriceQuoteVersion((uint)modelId, Convert.ToUInt32(cityId));
                    }
                }

                dt4 = DateTime.Now;
                ThreadContext.Properties["GetVersionListV3_2"] = (dt4 - dt3).TotalMilliseconds;
                dt3 = DateTime.Now;

                if (cityId > 0 && versionID > 0 && pqOnRoad != null)
                {
                    Bikewale.Entities.PriceQuote.v2.DetailedDealerQuotationEntity detailedDealer = null;
                    detailedDealer = objIPQ.GetDealerQuotationV2(Convert.ToUInt32(cityId), versionID, pqEntity.DealerId, areaId.HasValue && areaId.Value > 0 ? (uint)areaId.Value : 0);
                    pqEntity.DealerEntity = detailedDealer;
                    if (isAreaExistAndSelected || (!pqEntity.IsAreaExists))
                    {
                        pqEntity.PrimaryDealer = detailedDealer.PrimaryDealer != null && detailedDealer.PrimaryDealer.DealerDetails != null ? detailedDealer.PrimaryDealer : null;
                        pqEntity.SecondaryDealerCount = detailedDealer.SecondaryDealerCount;
                        pqEntity.IsPremium = pqEntity.PrimaryDealer != null && detailedDealer.PrimaryDealer.IsPremiumDealer;
                    }
                }

                dt4 = DateTime.Now;
                ThreadContext.Properties["GetVersionListV3_3"] = (dt4 - dt3).TotalMilliseconds;
                dt3 = DateTime.Now;

                // Fetch ES only when Primary dealer is absent for given model
                // ES campaign should be shown even if the secondary dealers are found
                // Updated by: Sangram Nandkhile on 13 Oct 2017
                // modified by : Monika Korrapati on 21 Sept 2018
                // Description : Changed LeadsButtonTextMobile value to "Get Best Offers" in default case.
                if (pqEntity.PrimaryDealer == null && _objManufacturerCampaign != null)
                {
                    pqEntity.ManufacturerCampaign = pqOnRoad != null && pqOnRoad.PriceQuote != null ? pqOnRoad.PriceQuote.ManufacturerCampaign : null;

                    if ((cityId == null || cityId.Value == 0)
                        && pqEntity.ManufacturerCampaign != null
                        && pqEntity.ManufacturerCampaign.LeadCampaign != null
                        && !pqEntity.ManufacturerCampaign.LeadCampaign.ShowOnExshowroom)
                    {
                        pqEntity.ManufacturerCampaign = null;
                    }

                    if (pqEntity.ManufacturerCampaign != null && pqEntity.ManufacturerCampaign.LeadCampaign != null && pqEntity.ManufacturerCampaign.LeadCampaign.LeadsButtonTextMobile.ToLower() == "request callback")
                    {
                        pqEntity.ManufacturerCampaign.LeadCampaign.LeadsButtonTextMobile = "Get Best Offers";
                    }

                    if (string.IsNullOrEmpty(pqEntity.PqId) && cityId > 0)
                    {
                        Bikewale.Entities.PriceQuote.v2.PriceQuoteParametersEntity objPQEntity = new Bikewale.Entities.PriceQuote.v2.PriceQuoteParametersEntity();
                        objPQEntity.CityId = Convert.ToUInt16(cityId);
                        objPQEntity.AreaId = Convert.ToUInt32(areaId);
                        objPQEntity.ClientIP = "";
                        objPQEntity.SourceId = Convert.ToUInt16(sourceId ?? 0);
                        objPQEntity.ModelId = (uint)modelId;
                        objPQEntity.VersionId = versionID;
                        objPQEntity.PQLeadId = (int)PQSourceEnum.Mobile_ModelPage;
                        objPQEntity.UTMA = UTMA;
                        objPQEntity.UTMZ = UTMZ;
                        objPQEntity.DeviceId = DeviceId;
                        pqEntity.PqId = objPq.RegisterPriceQuoteV2(objPQEntity);


                    }
                }
                dt4 = DateTime.Now;
                ThreadContext.Properties["GetVersionListV3_4"] = (dt4 - dt3).TotalMilliseconds;
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Exception : PQByCityArea GetVersionListV3");
            }
            finally
            {
                dt2 = DateTime.Now;
                ThreadContext.Properties["GetVersionListV3_Total"] = (dt2 - dt1).TotalMilliseconds;
                _logger.Info("GetVersionListV3");
            }
            return pqEntity;
        }
    }
}
