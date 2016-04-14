using Bikewale.BAL.BikeBooking;
using Bikewale.BAL.BikeData;
using Bikewale.Entities.BikeBooking;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.PriceQuote;
using Bikewale.Interfaces.BikeBooking;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.PriceQuote;
using Microsoft.Practices.Unity;
using System;

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
        public PQOnRoadPrice GetOnRoadPrice(int modelId, int? cityId=null, int? areaId= null, int? versionId = null, ushort? sourceId = null, string UTMA = null, string UTMZ = null, string DeviceId = null, string clientIP = null)
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
    }
}
