using Bikewale.Interfaces.AutoBiz;
using Bikewale.Notifications;
using BikeWale.Entities.AutoBiz;
using System;

namespace Bikewale.BAL.AutoBiz
{
    public class Dealers : IDealer
    {
        IDealerPriceQuote _dealerPQRepository = null;

        public Dealers(IDealerPriceQuote dealerPQRepository)
        {
            _dealerPQRepository = dealerPQRepository;
        }
        /// <summary>
        /// Created by  :   Sumit Kate on 18 Jul 2017
        /// Description :   Returns primary Dealer allocation
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
                        if (dealer == null || !dealer.IsDealerAvailable)
                        {
                            dealer = _dealerPQRepository.GetNearestDealer(modelId, cityId);
                        }
                    }
                    else
                    {
                        dealer = _dealerPQRepository.GetNearestDealer(modelId, cityId);
                    }
                }

            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("GetSubscriptionDealer({0},{1},{2})", modelId, cityId, areaId));
            }
            return dealer;
        }
    } // class
}   // namespace
