using Bikewale.BAL.BikeBooking;
using Bikewale.BAL.BikeData;
using Bikewale.Cache.Core;
using Bikewale.Cache.Location;
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
        /// <summary>
        /// Created By: Sangram Nandkhile on 13 Apr 2016
        /// summary   : Get On road price of all the version for modelId
        /// </summary>
        /// <param name="modelId">model Id</param>
        /// <param name="variantId">Model's version id</param>
        /// <param name="cityId">City id (optional)</param>
        /// <param name="areaId">Area id (optional)</param>
        /// <returns></returns>
        public PQOnRoadPrice GetOnRoadPrice(int modelId, int? cityId = null, int? areaId = null, int? versionId = null, ushort? sourceId = null, string UTMA = null, string UTMZ = null, string DeviceId = null, string clientIP = null)
        {
            PQOnRoadPrice pqOnRoad = null;
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IBikeModels<BikeModelEntity, int>, BikeModels<BikeModelEntity, int>>();
                    IBikeModels<BikeModelEntity, int> objClient = container.Resolve<IBikeModels<BikeModelEntity, int>>();
                    container.RegisterType<IPriceQuote, BAL.PriceQuote.PriceQuote>();
                    IPriceQuote objPq = container.Resolve<IPriceQuote>();
                    container.RegisterType<IDealerPriceQuote, DealerPriceQuote>();
                    IDealerPriceQuote objDealer = container.Resolve<IDealerPriceQuote>();
                    BikeQuotationEntity bpqOutput = null;
                    PriceQuoteParametersEntity objPQEntity = new PriceQuoteParametersEntity();
                    objPQEntity.CityId = Convert.ToUInt32(cityId);
                    objPQEntity.AreaId = Convert.ToUInt32(areaId);
                    objPQEntity.ClientIP = clientIP;
                    objPQEntity.SourceId = Convert.ToUInt16(sourceId);
                    objPQEntity.ModelId = Convert.ToUInt32(modelId);
                    objPQEntity.VersionId = Convert.ToUInt32(versionId);
                    objPQEntity.PQLeadId = Convert.ToUInt16(PQSourceEnum.Desktop_ModelPage);
                    objPQEntity.UTMA = UTMA;
                    objPQEntity.UTMZ = UTMZ;
                    objPQEntity.DeviceId = DeviceId;
                    PQOutputEntity objPQOutput = objDealer.ProcessPQ(objPQEntity);
                    if (objPQOutput != null)
                    {
                        versionId = Convert.ToInt32(objPQOutput.VersionId);
                    }
                    if (objPQOutput != null)
                    {
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
                                    string api = String.Format("/api/DealerPriceQuote/GetDealerPriceQuote/?cityid={0}&versionid={1}&dealerid={2}", cityId, versionId, objPQOutput.DealerId);
                                    using (Utility.BWHttpClient objDealerPqClient = new Utility.BWHttpClient())
                                    {
                                        oblDealerPQ = objDealerPqClient.GetApiResponseSync<PQ_QuotationEntity>(Utility.APIHost.AB, Utility.BWConfiguration.Instance.APIRequestTypeJSON, api, oblDealerPQ);
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
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<ICity, CityRepository>()
                                 .RegisterType<ICacheManager, MemcacheManager>()
                                 .RegisterType<ICityCacheRepository, CityCacheRepository>();
                    ICityCacheRepository objcity = container.Resolve<ICityCacheRepository>();
                    cityList = objcity.GetPriceQuoteCities(Convert.ToUInt16(modelId));
                    return cityList;
                }
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

                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IDealerPriceQuote, DealerPriceQuote>()
                        .RegisterType<ICacheManager, MemcacheManager>()
                        .RegisterType<IAreaCacheRepository, AreaCacheRepository>();

                    IAreaCacheRepository objArea = container.Resolve<IAreaCacheRepository>();
                    areaList = objArea.GetAreaList(Convert.ToUInt32(modelId), Convert.ToUInt32(cityId));
                    return areaList;
                }
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
        public PQByCityAreaEntity GetVersionList(int modelID, IEnumerable<BikeVersionMinSpecs> modelVersions, int? cityId, int? areaId)
        {
            PQByCityAreaEntity pqEntity = new PQByCityAreaEntity();
            try
            {
                PQOnRoadPrice pqOnRoad = GetOnRoadPrice(modelID, cityId, areaId);
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
                    }
                    pqOnRoad = GetOnRoadPrice(modelID, cityId, areaId);
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
                else if (cityId == null)
                {
                    pqEntity.IsExShowroomPrice = true;
                }
                // Check if bike has more than 1 version and send base version as the first version in VersionList
                if (modelVersions != null && modelVersions.Count() > 1 && pqOnRoad != null)
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
                var tempVersion = modelVersions[0];
                modelVersions[0] = modelVersions[baseVersionPos];
                modelVersions[baseVersionPos] = tempVersion;
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Exception : PQByCityArea .SwapVersionList");
                objErr.SendMail();
            }
            return modelVersions;
        }
    }
}
