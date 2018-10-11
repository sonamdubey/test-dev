using Bikewale.Interfaces.AutoBiz;
using Bikewale.Interfaces.PriceQuote;
using Bikewale.Notifications;
using BikeWale.Entities.AutoBiz;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bikewale.BAL.AutoBiz
{
    public class Dealers : IDealer
    {
        IDealerPriceQuote _dealerPQRepository = null;
        IDealerPriceQuoteCache _dealerPqCache;
        private readonly Random _rand;

        public Dealers(IDealerPriceQuote dealerPQRepository,IDealerPriceQuoteCache dealerPqCache)
        {
            _dealerPQRepository = dealerPQRepository;
            _dealerPqCache = dealerPqCache;
            _rand = new Random();
        }
        /// <summary>
        /// Created by  :   Sumit Kate on 18 Jul 2017
        /// Description :   Returns primary Dealer allocation
        /// Modofier    :   Kartik Rathod on 28 sept 2018
        /// Desc        :   Modified called GetNearestDealer(modelId, cityId)
        /// </summary>
        /// <param name="modelId"></param>
        /// <param name="cityId"></param>
        /// <param name="areaId"></param>
        /// <returns></returns>
        public DealerInfo GetSubscriptionDealer(uint modelId, uint cityId, uint areaId)
        {
            DealerInfo dealer = null;
            try
            {
                if (modelId > 0 && cityId > 0)
                {
                    if (areaId > 0)
                    {
                        dealer = _dealerPQRepository.GetNearestDealer(modelId, cityId, areaId);
                    }

                    if (dealer == null || !dealer.IsDealerAvailable)
                    {
                        dealer = GetNearestDealer(modelId, cityId);
                    }
                }
                
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("GetSubscriptionDealer({0},{1},{2})", modelId, cityId, areaId));
            }
            return dealer;
        }

        /// <summary>
        /// Author  :   Kartik Rathod on 28 sept 2018
        /// Desc    :   Give nearest dealer id randomly based on data get from cache dealer list for that city and model
        /// </summary>
        /// <param name="modelId"></param>
        /// <param name="cityId"></param>
        /// <returns></returns>
        public DealerInfo GetNearestDealer(uint modelId, uint cityId)
        {
            DealerInfo dealer = null;
            try
            {
                IEnumerable<uint> objDealerList = null;
                objDealerList = _dealerPqCache.GetNearestDealer(modelId, cityId);
                if (objDealerList != null && objDealerList.Any())
                {
                    dealer = new DealerInfo();
                    dealer.DealerId = objDealerList.ElementAt(_rand.Next(0, objDealerList.Count()));
                    dealer.IsDealerAvailable = dealer.DealerId > 0;
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("Dealers : GetNearestDealer({0},{1}", modelId, cityId));
            }
            return dealer;
        }
    } // class
}   // namespace
