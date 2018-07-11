
using Bikewale.BAL.BikeBooking;
using Bikewale.Cache.Core;
using Bikewale.Cache.PriceQuote;
using Bikewale.DAL.PriceQuote;
using Bikewale.Entities.BikeBooking;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.PriceQuote;
using Bikewale.Interfaces.AutoBiz;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.PriceQuote;
using Bikewale.Utility;
using Bikewale.Notifications;
using BikeWale.Entities.AutoBiz;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using RabbitMqPublishing;
using System.Collections.Specialized;
using log4net;
using System.Threading.Tasks;

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
        private readonly IDealerPriceQuoteCache _dealerPQCache;
        static ILog _logger = LogManager.GetLogger("PriceQuoteLogger");
        public PriceQuote()
        {
            using (IUnityContainer objPQCont = new UnityContainer())
            {
                objPQCont.RegisterType<IPriceQuote, PriceQuoteRepository>();
                objPQCont.RegisterType<IDealerPriceQuoteCache, DealerPriceQuoteCache>();
                objPQCont.RegisterType<ICacheManager, MemcacheManager>();
                objPQCont.RegisterType<IDealerPriceQuote, Bikewale.DAL.AutoBiz.DealerPriceQuoteRepository>();
                objPQ = objPQCont.Resolve<IPriceQuote>();
                _dealerPQCache = objPQCont.Resolve<IDealerPriceQuoteCache>();
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
            double time=0;
            try
            {
                DateTime startTime = DateTime.Now;
                pqGUId = RandomNoGenerator.GenerateUniqueId();
                Task.Run(() => PushToQueue(pqParams, pqGUId, CurrentUser.GetClientIP()));
                DateTime endTime = DateTime.Now;
                time = (endTime - startTime).TotalMilliseconds;
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("Bikewale.BAL.PriceQuote.PriceQuote.RegisterPriceQuoteV2()--> PQId = {0}", pqGUId));
            }finally
            {
                ThreadContext.Properties["RegisterPriceQuoteV2"] = time;
                _logger.Error("Time");
                ThreadContext.Properties.Remove("RegisterPriceQuoteV2");
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

                RabbitMqPublish _RabbitMQPublishing = new RabbitMqPublish();
                _RabbitMQPublishing.PublishToQueue(BWConfiguration.Instance.PQConsumerQueue, objNVC);
            }catch(Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("Bikewale.BAL.PriceQuote.PriceQuote.PushToQueue()--> PQId = {0}", pqGUId));
            }
        }

        /// <summary>
        /// Function to get the price quote by price quote id.
        /// </summary>
        /// <param name="pqId">Price quote id. Only positive numbers are allowed.</param>
        /// <returns>Returns price quote information in the PriceQuoteEntity object.</returns>
        public BikeQuotationEntity GetPriceQuoteById(ulong pqId)
        {
            BikeQuotationEntity objQuotation = null;

            objQuotation = objPQ.GetPriceQuoteById(pqId);

            return objQuotation;
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

        /// <summary>
        /// Function to get the price quote by price quote id.
        /// </summary>
        /// <param name="pqId">Price quote id. Only positive numbers are allowed.</param>
        /// <returns>Returns price quote information in the PriceQuoteEntity object.</returns>
        public BikeQuotationEntity GetPriceQuoteById(ulong pqId, LeadSourceEnum page)
        {
            BikeQuotationEntity objQuotation = null;

            objQuotation = objPQ.GetPriceQuoteById(pqId, page);

            return objQuotation;
        }

        /// <summary>
        /// Craeted By  : Pratibha Verma on 20 June 2018
        /// Description : Removed PQId dependency
        /// </summary>
        /// <param name="cityId"></param>
        /// <param name="versionId"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public Bikewale.Entities.PriceQuote.v2.BikeQuotationEntity GetPriceQuote(uint cityId, uint versionId, LeadSourceEnum page)
        {
            Bikewale.Entities.PriceQuote.v2.BikeQuotationEntity objQuotation = null;

            objQuotation = objPQ.GetPriceQuote(cityId, versionId, page);

            return objQuotation;
        }

        /// <summary>
        /// Function to get the price quote by price quote information. Price quote will be registered automatically and returns price quote.
        /// </summary>
        /// <param name="pqParams">all parameters necessory to save the price quote.</param>
        /// <returns>Retunrs price quote in the PricQuoteEntity object.</returns>
        [Obsolete("Unused")]
        public BikeQuotationEntity GetPriceQuote(PriceQuoteParametersEntity pqParams)
        {
            BikeQuotationEntity objQuotation = null;

            objQuotation = objPQ.GetPriceQuote(pqParams);

            return objQuotation;
        }

        /// <summary>
        /// Function to get the other versions of the model whose price quote is taken.
        /// </summary>
        /// <param name="pqId">Price quote id</param>
        /// <returns>Returns list containing versions with on road prices.</returns>
        public List<OtherVersionInfoEntity> GetOtherVersionsPrices(ulong pqId)
        {
            List<OtherVersionInfoEntity> objVersionsList = null;

            objVersionsList = objPQ.GetOtherVersionsPrices(pqId);

            return objVersionsList;
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
        public IEnumerable<Entities.ManufacturerDealer> GetManufacturerDealers()
        {
            return objPQ.GetManufacturerDealers();
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
    }   // class
}   // namespace
