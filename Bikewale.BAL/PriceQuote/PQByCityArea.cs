using Bikewale.BAL.BikeBooking;
using Bikewale.BAL.BikeData;
using Bikewale.Cache.Core;
using Bikewale.Cache.Location;
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

        ICityCacheRepository objcity = null;
        IBikeModels<BikeModelEntity, int> objClient = null;
        IPriceQuote objPq = null;
        IDealerPriceQuote objDealer = null;
        Bikewale.Interfaces.AutoBiz.IDealerPriceQuote objPriceQuote = null;
        IAreaCacheRepository objArea = null;
        IDealerPriceQuoteDetail objIPQ = null;

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



                objcity = container.Resolve<ICityCacheRepository>();
                objClient = container.Resolve<IBikeModels<BikeModelEntity, int>>();
                objPq = container.Resolve<IPriceQuote>();
                objDealer = container.Resolve<IDealerPriceQuote>();
                objPriceQuote = container.Resolve<DealerPriceQuoteRepository>();
                objArea = container.Resolve<IAreaCacheRepository>();
                objIPQ = container.Resolve<IDealerPriceQuoteDetail>();
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
                                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, "PQByCityArea: " + "GetOnRoadPrice");
                                objErr.SendMail();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, "PQByCityArea: " + "GetOnRoadPrice");
                objErr.SendMail();
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
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, "ModelPageController" + "FetchCityByModelId");
                objErr.SendMail();
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
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, "ModelPageController" + "GetAreaForCityAndModel");
                objErr.SendMail();
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
                    pqEntity.IsCityExists = cityList != null && cityList.Any(p => p.CityId == cityId);
                    if (pqEntity.IsCityExists)
                    {
                        var areaList = GetAreaForCityAndModel(modelID, Convert.ToInt16(cityId));
                        pqEntity.IsAreaExists = (areaList != null && areaList.Count() > 0);
                        // If area is provided, check if area exists in list
                        if (areaId > 0)
                        {
                            pqEntity.IsAreaSelected = areaList != null && areaList.Any(p => p.AreaId == areaId);
                        }
                        pqOnRoad = GetOnRoadPrice(modelID, cityId, areaId, null, sourceId, UTMA, UTMZ, DeviceId, clientIP);
                        if (pqOnRoad != null)
                        {
                            pqEntity.PqId = pqOnRoad.PriceQuote.PQId;
                            pqEntity.DealerId = pqOnRoad.PriceQuote.DealerId;
                            pqEntity.IsExShowroomPrice = pqOnRoad.DPQOutput == null && pqOnRoad.BPQOutput == null;

                            // When City has areas and area is not selected then show ex-showrrom price so user can select it
                            bool isAreaExistAndSelected = pqEntity.IsAreaExists && pqEntity.IsAreaSelected;
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
                ErrorClass objErr = new ErrorClass(ex, "Exception : PQByCityArea GetVersionList");
                objErr.SendMail();
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
                        pqOutput.IsCityExists = selectedCity != null;

                        if (pqOutput.IsCityExists)
                        {
                            if (selectedCity.HasAreas)
                            {
                                pqOutput.IsAreaExists = pqInput.AreaId > 0;
                                IEnumerable<Bikewale.Entities.Location.AreaEntityBase> areaList = null;

                                if (!pqOutput.IsAreaExists)
                                    areaList = GetAreaForCityAndModel(Convert.ToInt32(pqInput.ModelId), Convert.ToInt32(pqInput.CityId));


                                if (pqOutput.IsAreaExists)
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
                ErrorClass objErr = new ErrorClass(ex, "Exception : PQByCityArea GetPriceQuoteByCityArea");
                objErr.SendMail();
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
                ErrorClass objErr = new ErrorClass(ex, "Exception : PQByCityArea .SwapVersionList");
                objErr.SendMail();
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
        public PQByCityAreaEntity GetVersionListV2(int modelID, IEnumerable<BikeVersionMinSpecs> modelVersions, int? cityId, int? areaId, ushort? sourceId, string UTMA = null, string UTMZ = null, string DeviceId = null, string clientIP = null)
        {
            PQByCityAreaEntity pqEntity = new PQByCityAreaEntity();
            uint versionID = 0;
            bool isAreaExistAndSelected = false;

            try
            {
                PQOnRoadPrice pqOnRoad = null;
                if (cityId > 0)
                {
                    IEnumerable<CityEntityBase> cityList = FetchCityByModelId(modelID);
                    pqEntity.IsCityExists = cityList != null && cityList.Any(p => p.CityId == cityId);
                    if (pqEntity.IsCityExists)
                    {
                        var areaList = GetAreaForCityAndModel(modelID, Convert.ToInt16(cityId));
                        pqEntity.IsAreaExists = (areaList != null && areaList.Count() > 0);
                        // If area is provided, check if area exists in list
                        if (areaId > 0)
                        {
                            pqEntity.IsAreaSelected = areaList != null && areaList.Any(p => p.AreaId == areaId);
                        }

                        pqOnRoad = GetOnRoadPrice(modelID, cityId, areaId, null, sourceId, UTMA, UTMZ, DeviceId, clientIP);
                        if (pqOnRoad != null)
                        {
                            pqEntity.PqId = pqOnRoad.PriceQuote.PQId;
                            pqEntity.DealerId = pqOnRoad.PriceQuote.DealerId;
                            pqEntity.IsExShowroomPrice = pqOnRoad.DPQOutput == null && pqOnRoad.BPQOutput == null;

                            // When City has areas and area is not selected then show ex-showrrom price so user can select it
                            isAreaExistAndSelected = pqEntity.IsAreaExists && pqEntity.IsAreaSelected;
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

                //below block added to get dealers pakage and offers and seondary dealers count

                if (versionID <= 0)
                {

                    versionID = objDealer.GetDefaultPriceQuoteVersion(Convert.ToUInt32(modelID), Convert.ToUInt32(cityId));
                }

                if (cityId > 0 && versionID > 0)
                {
                    DetailedDealerQuotationEntity detailedDealer = null;

                    detailedDealer = objIPQ.GetDealerQuotation(Convert.ToUInt32(cityId), versionID, pqEntity.DealerId);


                    if (isAreaExistAndSelected || (!pqEntity.IsAreaExists))
                    {
                        pqEntity.PrimaryDealer = detailedDealer.PrimaryDealer != null && detailedDealer.PrimaryDealer.DealerDetails != null ? detailedDealer.PrimaryDealer : null;

                        pqEntity.SecondaryDealerCount = detailedDealer.SecondaryDealerCount;

                        if (detailedDealer.PrimaryDealer != null && detailedDealer.PrimaryDealer.DealerDetails != null)
                        {
                            pqEntity.IsPremium = detailedDealer.PrimaryDealer.DealerDetails.DealerPackageType == DealerPackageTypes.Premium ? true : false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Exception : PQByCityArea GetVersionList");
                objErr.SendMail();
            }
            return pqEntity;
        }

    }
}
