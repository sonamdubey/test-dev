
using Bikewale.Cache.Core;
using Bikewale.Cache.Helper.PriceQuote;
using Bikewale.Cache.PriceQuote;
using Bikewale.DAL.BikeBooking;
using Bikewale.DAL.PriceQuote;
using Bikewale.DTO.PriceQuote.Version;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.PriceQuote;
using Bikewale.Interfaces.BikeBooking;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.PriceQuote;
using Bikewale.Notifications;
using Bikewale.Utility;
using BikeWale.Entities.AutoBiz;
using log4net;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web.Hosting;

namespace Bikewale.BAL.PriceQuote
{
    /// <summary>
    /// Created By: Ashish G. Kamble
    /// Summary : Class have functions for the price quote business layer.
    /// Modified By :   Sumit Kate
    /// Date        :   16 Oct 2015
    /// Description :   Implemented newly added method of IPriceQuote interface
    /// </summary>
    public class PriceQuote : IPriceQuote
    {
        private readonly IPriceQuote objPQ;
        private readonly IPriceQuoteCache objPQCache;
        private readonly IDealerPriceQuoteCache _dealerPQCache;
        static ILog _logger = LogManager.GetLogger("PriceQuoteLogger");

        private readonly PQGenerate _pqGenerate;
        public PriceQuote()
        {
            using (IUnityContainer objPQCont = new UnityContainer())
            {
                objPQCont.RegisterType<IPriceQuoteCache, PriceQuoteCache>();
                objPQCont.RegisterType<IPriceQuoteCacheHelper, PriceQuoteCacheHelper>();
                objPQCont.RegisterType<IPriceQuote, PriceQuoteRepository>();
                objPQCont.RegisterType<IDealerPriceQuote, DealerPriceQuoteRepository>();
                objPQCont.RegisterType<IDealerPriceQuoteCache, DealerPriceQuoteCache>();
                objPQCont.RegisterType<ICacheManager, MemcacheManager>();
                objPQCont.RegisterType<Bikewale.Interfaces.AutoBiz.IDealerPriceQuote, Bikewale.DAL.AutoBiz.DealerPriceQuoteRepository>();
                objPQ = objPQCont.Resolve<IPriceQuote>();
                objPQCache = objPQCont.Resolve<IPriceQuoteCache>();
                _dealerPQCache = objPQCont.Resolve<IDealerPriceQuoteCache>();
                _pqGenerate = new PQGenerate(objPQ);
            }
        }

        /// <summary>
        /// Function to save the price quote
        /// </summary>
        /// <param name="pqParams">All parameters necessory to save the price quote.</param>
        /// <returns>Returns price quote id</returns>
        public ulong RegisterPriceQuote(PriceQuoteParametersEntity pqParams)
        {
            ulong pqId = 0;

            pqId = objPQ.RegisterPriceQuote(pqParams);

            return pqId;
        }

        /// <summary>
        /// Aithor  : Kartik rathod on 20 jun 2018 price quote changes return guid
        /// </summary>
        /// <param name="pqParams"></param>
        /// <returns></returns>
        public string RegisterPriceQuoteV2(Bikewale.Entities.PriceQuote.v2.PriceQuoteParametersEntity pqParams)
        {
            string pqGUId = string.Empty;
            try
            {
                using (BenchMark benchMark = new BenchMark(_logger, "RegisterPriceQuoteV2"))
                {
                    pqGUId = RandomNoGenerator.GenerateUniqueId();
                    string clientIp = CurrentUser.GetClientIP();
                    PushToQueue(pqParams, pqGUId, clientIp);
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("Bikewale.BAL.PriceQuote.PriceQuote.RegisterPriceQuoteV2()--> PQId = {0}", pqGUId));
            }
            return pqGUId;
        }

        private void PushToQueue(Bikewale.Entities.PriceQuote.v2.PriceQuoteParametersEntity pqParams, string pqGUId, string clientIp)
        {
            try
            {
                NameValueCollection objNVC = new NameValueCollection();
                objNVC.Add("GUID", pqGUId);
                objNVC.Add("versionId", Convert.ToString(pqParams.VersionId));
                objNVC.Add("cityId", Convert.ToString(pqParams.CityId));
                objNVC.Add("areaId", Convert.ToString(pqParams.AreaId));
                objNVC.Add("buyingPreference", Convert.ToString(pqParams.BuyingPreference));
                objNVC.Add("customerId", Convert.ToString(pqParams.CustomerId));
                objNVC.Add("customerName", pqParams.CustomerName);
                objNVC.Add("customerEmail", pqParams.CustomerEmail);
                objNVC.Add("customerMobile", pqParams.CustomerMobile);
                objNVC.Add("clientIP", clientIp);
                objNVC.Add("sourceId", Convert.ToString(pqParams.SourceId));
                objNVC.Add("dealerId", Convert.ToString(pqParams.DealerId));
                objNVC.Add("deviceId", pqParams.DeviceId);
                objNVC.Add("UTMA", pqParams.UTMA);
                objNVC.Add("UTMZ", pqParams.UTMZ);
                objNVC.Add("pqSourceId", Convert.ToString(pqParams.PQLeadId));
                objNVC.Add("refGUID", pqParams.RefPQId);

                HostingEnvironment.QueueBackgroundWorkItem(f => PushToPQConsumerQueue(objNVC));
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("Bikewale.BAL.PriceQuote.PriceQuote.PushToQueue()--> PQId = {0}", pqGUId));
            }
        }

        private void PushToPQConsumerQueue(NameValueCollection objNVC)
        {
            _pqGenerate.RabbitMQExecution(objNVC);
        }

        /// <summary>
        /// Created By  : Pratibha Verma on 19 June 2018
        /// Description : removed PQId dependency
        /// </summary>
        /// <param name="cityId"></param>
        /// <param name="versionId"></param>
        /// <returns></returns>
        public Bikewale.Entities.PriceQuote.v2.BikeQuotationEntity GetPriceQuote(uint cityId, uint versionId)
        {
            Bikewale.Entities.PriceQuote.v2.BikeQuotationEntity objQuotation = null;

            objQuotation = objPQ.GetPriceQuote(cityId, versionId);

            return objQuotation;
        }

        public IEnumerable<OtherVersionInfoEntity> GetOtherVersionsPrices(uint modelId, uint cityId)
        {
            return objPQ.GetOtherVersionsPrices(modelId, cityId);
        }

        /// <summary>
        /// Author  :   Sumit Kate
        /// Created On  :   16 Oct 2015
        /// Description :   Updates the price quote data
        /// </summary>
        /// <param name="pqId">Price Quote Id is mandatory</param>
        /// <param name="pqParams">Price Quote data that needs to be updated</param>
        /// <returns></returns>
        public bool UpdatePriceQuote(UInt32 pqId, PriceQuoteParametersEntity pqParams)
        {
            if (pqId > 0)
            {
                return objPQ.UpdatePriceQuote(pqId, pqParams);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Created By  : Rajan Chauhan on 27 June 2018
        /// Description : Method for updating PQ details by leadId
        /// </summary>
        /// <param name="leadId"></param>
        /// <param name="pqParams"></param>
        /// <returns></returns>
        public bool UpdatePriceQuoteDetailsByLeadId(UInt32 leadId, PriceQuoteParametersEntity pqParams)
        {
            if (leadId > 0)
            {
                return objPQ.UpdatePriceQuoteDetailsByLeadId(leadId, pqParams);
            }
            return false;
        }


        public bool SaveBookingState(uint pqId, PriceQuoteStates state)
        {
            return objPQ.SaveBookingState(pqId, state);
        }

        public bool SaveBookingStateByLeadId(uint leadId, PriceQuoteStates state)
        {
            return objPQ.SaveBookingStateByLeadId(leadId, state);
        }

        public PriceQuoteParametersEntity FetchPriceQuoteDetailsById(ulong pqId)
        {
            return objPQ.FetchPriceQuoteDetailsById(pqId);
        }

        /// <summary>
        /// Created By : Vivek Gupta
        /// Date : 20-05-2016
        /// Desc : Fetch BW Pricequote of top cities by modelId
        /// </summary>
        /// <param name="modelId"></param>
        /// <param name="topCount"></param>
        /// <returns></returns>
        public IEnumerable<PriceQuoteOfTopCities> FetchPriceQuoteOfTopCities(uint modelId, uint topCount)
        {
            return objPQ.FetchPriceQuoteOfTopCities(modelId, topCount);
        }


        /// <summary>
        /// Written By : Ashish G. Kamble on 23 May 2016
        /// Summary : Function get the prices of the given model in the nearest cities of the given city. Function gets data from DAL.
        /// </summary>
        /// <param name="modelId"></param>
        /// <param name="cityId"></param>
        /// <param name="topCount"></param>
        /// <returns></returns>
        public IEnumerable<PriceQuoteOfTopCities> GetModelPriceInNearestCities(uint modelId, uint cityId, ushort topCount)
        {
            return objPQ.GetModelPriceInNearestCities(modelId, cityId, topCount);
        }

        public ModelTopVersionPrices GetTopVersionPriceInCities(uint modelId)
        {
            return objPQ.GetTopVersionPriceInCities(modelId);
        }


        /// <summary>
        /// Created By  : Rajan Chauhan on 28 September 2018
        /// Description : Created method to access VersionPrices via cache 
        /// </summary>
        /// <param name="modelId"></param>
        /// <param name="cityId"></param>
        /// <returns></returns>
        public IEnumerable<BikeQuotationEntity> GetVersionPricesByModelId(uint modelId, uint cityId)
        {
            return objPQCache.GetVersionPricesByModelId(modelId, cityId);
        }
        /// <summary>
        /// Created by  :   Sumit Kate on 28 Mar 2017
        /// Description :   Call DAL function
        /// </summary>
        /// <param name="modelId"></param>
        /// <param name="cityId"></param>
        /// <param name="hasArea"></param>
        /// <returns></returns>
        public IEnumerable<BikeQuotationEntity> GetVersionPricesByModelId(uint modelId, uint cityId, out bool hasArea)
        {
            return objPQ.GetVersionPricesByModelId(modelId, cityId, out hasArea);
        }


        /// <summary>
        /// Gets the manufacturer dealers.
        /// </summary>
        /// <returns>
        /// Created by : Sangram Nandkhile on 10-May-2017 
        /// </returns>
        public IDictionary<uint, List<Entities.ManufacturerDealer>> GetManufacturerDealers(uint dealerId)
        {
            return objPQ.GetManufacturerDealers(dealerId);
        }

        /// <summary>
        /// Created by  : Pratibha Verma on 8 June 2018
        /// Description : returns version price
        /// </summary>
        /// <param name="cityId"></param>
        /// <param name="modelId"></param>
        /// <param name="dealerId"></param>
        public void GetDealerVersionsPriceByModelCity(IEnumerable<BikeVersionMinSpecs> versionList, uint cityId, uint modelId, uint dealerId = 0)
        {
            IEnumerable<PQ_VersionPrice> objDealerPrice = null;
            IEnumerable<OtherVersionInfoEntity> objBWPrice = null;
            try
            {
                if (dealerId > 0)
                {
                    objDealerPrice = _dealerPQCache.GetDealerPriceQuotesByModelCity(cityId, modelId, dealerId);
                }
                objBWPrice = objPQ.GetOtherVersionsPrices(modelId, cityId);
                if (versionList != null && versionList.Any())
                {
                    foreach (var version in versionList)
                    {
                        var dealerPrice = objDealerPrice != null && objDealerPrice.FirstOrDefault(x => x.VersionId == version.VersionId) != null ? objDealerPrice.FirstOrDefault(x => x.VersionId == version.VersionId).Price : 0;
                        if (dealerPrice > 0)
                        {
                            version.Price = dealerPrice;
                        }
                        else
                        {
                            version.Price = objBWPrice != null && objBWPrice.FirstOrDefault(x => x.VersionId == version.VersionId) != null ? objBWPrice.FirstOrDefault(x => x.VersionId == version.VersionId).OnRoadPrice : 0;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.BAL.PriceQuote.GetDealerVersionPriceByModelCity");
            }
        }
        /// <summary>
        /// Created By : Prabhu Puredla on 18 july 2018
        /// Description : Get the status for make city combination in mla
        /// </summary>
        /// <param name="makeId"></param>
        /// <param name="cityId"></param>
        /// <returns></returns>
        public bool GetMLAStatus(int makeId, uint cityId)
        {
            try
            {
                if (makeId > 0 && cityId > 0)
                {
                    string key = string.Format("{0}_{1}", makeId, cityId);
                    IEnumerable<string> mlaMakeCities = _dealerPQCache.GetMLAMakeCities();
                    if (mlaMakeCities != null)
                    {
                        return mlaMakeCities.Contains(key);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("Bikewale.BAL.PriceQuote.GetMLAStatus for MakeId_{0} , CityId_{1}", makeId, cityId));
            }
            return false;
        }

        public VersionPrice GetVersionPriceByCityId(uint versionId, uint cityId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Created by  : Pratibha Verma on 29 August 2018
        /// Description : Get Version price list by cityid
        /// </summary>
        /// <param name="versionId"></param>
        /// <param name="cityId"></param>
        /// <returns></returns>
        public IList<PriceCategory> GetVersionPriceListByCityId(uint versionId, uint cityId)
        {
            IList<PriceCategory> versionPriceList = null;
            try
            {
                VersionPrice versionPrice = objPQ.GetVersionPriceByCityId(versionId, cityId);
                if (versionPrice != null)
                {
                    versionPriceList = new List<PriceCategory>();
                    if (versionPrice.Exshowroom > 0)
                    {
                        versionPriceList.Add(new PriceCategory() { Category = "Ex-Showroom Price", Price = versionPrice.Exshowroom });
                    }
                    if (versionPrice.RTO > 0)
                    {
                        versionPriceList.Add(new PriceCategory() { Category = "RTO", Price = versionPrice.RTO });
                    }
                    if (versionPrice.Insurance > 0)
                    {
                        versionPriceList.Add(new PriceCategory() { Category = "Insurance", Price = versionPrice.Insurance });
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("Bikewale.BAL.PriceQuote.PriceQuote.GetVersionPriceByCityId(versionId = {0}, cityId = {1})", versionId, cityId));
            }
            return versionPriceList;
        }

        public IEnumerable<string> GetManufacturerOffers(uint campaignId)
        {
            return objPQ.GetManufacturerOffers(campaignId);
        }


        /// <summary>
        /// Created  by : Rajan Chauhan on 9 Nov 2018
        /// Description : Method to return offer template based on platformId 
        /// </summary>
        /// <param name="platformId"></param>
        /// <returns></returns>
        public string GetManufactuerDefaultCampaignOfferTemplate(ushort platformId)
        {
            return objPQ.GetManufactuerDefaultCampaignOfferTemplate(platformId);
        }
    }   // class


    internal class PQGenerate
    {
        private readonly IPriceQuote _objPQ;
        public PQGenerate(IPriceQuote objPQ)
        {
            _objPQ = objPQ;
        }
        public void RabbitMQExecution(NameValueCollection nvc)
        {
            try
            {
                // For unvalidated messages dropping them from queue by returning true from the Execution method
                if (validateMessage(nvc))
                {
                    Bikewale.Entities.PriceQuote.v2.PriceQuoteParametersEntity pqParams = new Bikewale.Entities.PriceQuote.v2.PriceQuoteParametersEntity()
                    {
                        GUID = nvc["GUID"],
                        VersionId = Convert.ToUInt32(String.IsNullOrEmpty(nvc["versionId"]) ? "0" : nvc["versionId"]),
                        CityId = Convert.ToUInt32(String.IsNullOrEmpty(nvc["cityId"]) ? "0" : nvc["cityId"]),
                        AreaId = Convert.ToUInt32(String.IsNullOrEmpty(nvc["areaId"]) ? "0" : nvc["areaId"]),
                        BuyingPreference = Convert.ToUInt16(String.IsNullOrEmpty(nvc["buyingPreference"]) ? "0" : nvc["buyingPreference"]),
                        CustomerId = Convert.ToUInt64(String.IsNullOrEmpty(nvc["customerId"]) ? "0" : nvc["customerId"]),
                        CustomerName = nvc["customerName"],
                        CustomerEmail = nvc["customerEmail"],
                        CustomerMobile = nvc["customerMobile"],
                        ClientIP = nvc["clientIP"],
                        SourceId = Convert.ToUInt16(String.IsNullOrEmpty(nvc["sourceId"]) ? "0" : nvc["sourceId"]),
                        DealerId = Convert.ToUInt32(String.IsNullOrEmpty(nvc["dealerId"]) ? "0" : nvc["dealerId"]),
                        DeviceId = nvc["deviceId"],
                        UTMA = nvc["UTMA"],
                        UTMZ = nvc["UTMZ"],
                        PQLeadId = Convert.ToUInt16(String.IsNullOrEmpty(nvc["pqSourceId"]) ? "0" : nvc["pqSourceId"]),
                        RefPQId = nvc["refGUID"]
                    };
                    _objPQ.RegisterPriceQuoteV2(pqParams);
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("PQConsumer.RabbitMQExecution: Error while performing operation for versionId = {0}, GUID = {1}, pqSourceId = {2}", nvc["versionId"], nvc["GUID"], nvc["pqSourceId"]));
            }
        }

        /// <summary>
        /// Method for validating queueMessage
        /// </summary>
        /// <param name="nvc"></param>
        /// <returns></returns>
        private bool validateMessage(NameValueCollection nvc)
        {
            try
            {
                return (!String.IsNullOrEmpty(nvc["GUID"]) && !String.IsNullOrEmpty(nvc["versionId"]) && Convert.ToUInt32(nvc["versionId"]) > 0);
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("PQConsumer.validateMessage : versionId = {0}, GUID = {1}", nvc["versionId"], nvc["GUID"]));
            }
            return false;
        }
    }

}   // namespace
