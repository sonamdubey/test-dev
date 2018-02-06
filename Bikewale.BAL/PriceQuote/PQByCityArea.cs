using Bikewale.BAL.BikeBooking;
using Bikewale.BAL.BikeData;
using Bikewale.Cache.Core;
using Bikewale.Cache.Location;
using Bikewale.Cache.PriceQuote;
using Bikewale.DAL.AutoBiz;
using Bikewale.DAL.Location;
using Bikewale.Entities.BikeBooking;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.Location;
using Bikewale.Entities.PriceQuote;
using Bikewale.Interfaces.BikeBooking;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.Location;
using Bikewale.Interfaces.PriceQuote;
using Bikewale.ManufacturerCampaign.DAL;
using Bikewale.ManufacturerCampaign.Interface;
using Bikewale.Notifications;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
namespace Bikewale.BAL.PriceQuote
{
    /// <summary>
    /// Created By: Sangram Nandkhile on 13 Apr 2016
    /// summary   : Class to have a common logic to get pq by Model, city and area
    /// </summary>
    public class PQByCityArea
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


        /// <summary>
        /// 
        /// </summary>
        public PQByCityArea()
        {
            using (IUnityContainer container = new UnityContainer())
            {
                container.RegisterType<IBikeModels<BikeModelEntity, int>, BikeModels<BikeModelEntity, int>>();
                container.RegisterType<IDealerPriceQuote, DealerPriceQuote>();
                container.RegisterType<IPriceQuote, BAL.PriceQuote.PriceQuote>();
                container.RegisterType<ICity, CityRepository>();
                container.RegisterType<ICacheManager, MemcacheManager>();
                container.RegisterType<ICityCacheRepository, CityCacheRepository>();
                container.RegisterType<IAreaCacheRepository, AreaCacheRepository>();
                container.RegisterType<Bikewale.Interfaces.AutoBiz.IDealerPriceQuote, DealerPriceQuoteRepository>();
                container.RegisterType<IDealerPriceQuoteDetail, DealerPriceQuoteDetail>();
                container.RegisterType<IManufacturerCampaignRepository, ManufacturerCampaignRepository>();
                container.RegisterType<IManufacturerCampaign, Bikewale.ManufacturerCampaign.BAL.ManufacturerCampaign>();
                container.RegisterType<IPriceQuoteCache, PriceQuoteCache>();

                objcity = container.Resolve<ICityCacheRepository>();
                objClient = container.Resolve<IBikeModels<BikeModelEntity, int>>();
                objPq = container.Resolve<IPriceQuote>();
                objDealer = container.Resolve<IDealerPriceQuote>();
                objPriceQuote = container.Resolve<DealerPriceQuoteRepository>();
                objArea = container.Resolve<IAreaCacheRepository>();
                objIPQ = container.Resolve<IDealerPriceQuoteDetail>();
                _objPQCache = container.Resolve<IPriceQuoteCache>();
                _objManufacturerCampaign = container.Resolve<IManufacturerCampaign>();
            }
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
        public PQOnRoadPrice GetOnRoadPrice(int modelId, int? cityId = null, int? areaId = null, int? versionId = null, ushort? sourceId = null, string UTMA = null, string UTMZ = null, string DeviceId = null, string clientIP = null, string PQLeadId = null)
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
                PQOutputEntity objPQOutput = objDealer.ProcessPQ(objPQEntity);
                if (objPQOutput != null)
                {
                    versionId = Convert.ToInt32(objPQOutput.VersionId);
                    pqOnRoad = new PQOnRoadPrice();
                    pqOnRoad.BaseVersion = objPQOutput.VersionId;
                    pqOnRoad.PriceQuote = objPQOutput;
                    BikeModelEntity bikemodelEnt = objClient.GetById(Convert.ToInt32(modelId));
                    pqOnRoad.BikeDetails = bikemodelEnt;
                    if (objPQOutput != null && objPQOutput.PQId > 0)
                    {
                        bpqOutput = objPq.GetPriceQuoteById(objPQOutput.PQId);
                        bpqOutput.Varients = objPq.GetOtherVersionsPrices(objPQOutput.PQId);
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
                            pqOnRoad = GetOnRoadPrice(modelID, cityId, areaId, null, sourceId, UTMA, UTMZ, DeviceId, clientIP);
                        }
                        else
                        {
                            pqOnRoad = GetOnRoadPrice(modelID, cityId, areaId, null, sourceId, UTMA, UTMZ, DeviceId, clientIP);
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

            try
            {
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
                            pqEntity.IsAreaSelected = areaList != null && areaList.Any(p => p.AreaId == areaId);
                            pqEntity.Area = areaList.FirstOrDefault(p => p.AreaId == areaId);
                        }
                        if (selectedCity != null && selectedCity.HasAreas)
                        {
                            if (pqEntity.IsAreaSelected)
                                pqOnRoad = GetOnRoadPrice(modelId, cityId, areaId, null, sourceId, UTMA, UTMZ, DeviceId, clientIP);
                        }
                        else
                        {
                            pqOnRoad = GetOnRoadPrice(modelId, cityId, areaId, null, sourceId, UTMA, UTMZ, DeviceId, clientIP);
                        }
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
                                            versionID = Convert.ToUInt32(version.VersionId);
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
                else if (cityId == null || cityId.Value == 0)
                {
                    pqEntity.IsExShowroomPrice = true;
                }
                // Check if bike has more than 1 version and send base version as the first version in VersionList
                if (modelVersions != null && modelVersions.Count() > 1 && pqOnRoad != null && pqOnRoad.BaseVersion > 0)
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
                        versionID = objDealer.GetDefaultPriceQuoteVersion((uint)modelId, Convert.ToUInt32(cityId));
                }

                if (cityId > 0 && versionID > 0 && pqOnRoad != null)
                {
                    Bikewale.Entities.PriceQuote.v2.DetailedDealerQuotationEntity detailedDealer = null;
                    detailedDealer = objIPQ.GetDealerQuotationV2(Convert.ToUInt32(cityId), versionID, pqEntity.DealerId, areaId.HasValue && areaId.Value > 0 ? (uint)areaId.Value : 0);
                    pqEntity.DealerEntity = detailedDealer;
                    if (isAreaExistAndSelected || (!pqEntity.IsAreaExists))
                    {
                        pqEntity.PrimaryDealer = detailedDealer.PrimaryDealer != null && detailedDealer.PrimaryDealer.DealerDetails != null ? detailedDealer.PrimaryDealer : null;
                        pqEntity.SecondaryDealerCount = detailedDealer.SecondaryDealerCount;

                        if (detailedDealer.PrimaryDealer != null && detailedDealer.PrimaryDealer.DealerDetails != null)
                        {
                            pqEntity.IsPremium = detailedDealer.PrimaryDealer.IsPremiumDealer;
                        }
                    }
                }

                // Fetch ES only when Primary dealer is absent for given model
                // ES campaign should be shown even if the secondary dealers are found
                // Updated by: Sangram Nandkhile on 13 Oct 2017
                if (pqEntity.PrimaryDealer == null && _objManufacturerCampaign != null)
                {
                    pqEntity.ManufacturerCampaign = _objManufacturerCampaign.GetCampaigns((uint)modelId, (uint)cityId, Bikewale.ManufacturerCampaign.Entities.ManufacturerCampaignServingPages.Mobile_Model_Page);

                    if ((cityId == null || cityId.Value == 0)
                        && pqEntity.ManufacturerCampaign != null
                        && pqEntity.ManufacturerCampaign.LeadCampaign != null
                        && !pqEntity.ManufacturerCampaign.LeadCampaign.ShowOnExshowroom)
                    {
                        pqEntity.ManufacturerCampaign = null;
                    }

                    if (pqEntity.PqId == 0 && cityId > 0)
                    {
                        PriceQuoteParametersEntity objPQEntity = new PriceQuoteParametersEntity();
                        objPQEntity.CityId = Convert.ToUInt16(cityId);
                        objPQEntity.AreaId = Convert.ToUInt32(areaId);
                        objPQEntity.ClientIP = "";
                        objPQEntity.SourceId = Convert.ToUInt16(sourceId ?? 0);
                        objPQEntity.ModelId = (uint)modelId;
                        objPQEntity.VersionId = pqOnRoad != null ? pqOnRoad.BaseVersion : (uint)pqEntity.VersionList.FirstOrDefault().VersionId;
                        objPQEntity.PQLeadId = (int)PQSourceEnum.Mobile_ModelPage;
                        objPQEntity.UTMA = UTMA;
                        objPQEntity.UTMZ = UTMZ;
                        objPQEntity.DeviceId = DeviceId;
                        pqEntity.PqId = (uint)objPq.RegisterPriceQuote(objPQEntity);


                    }
                    if (pqEntity.ManufacturerCampaign != null && pqEntity.ManufacturerCampaign.LeadCampaign != null)
                    {
                        _objManufacturerCampaign.SaveManufacturerIdInPricequotes((uint)pqEntity.PqId, pqEntity.ManufacturerCampaign.LeadCampaign.DealerId);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Exception : PQByCityArea GetVersionList");
            }
            return pqEntity;
        }
    }
}
