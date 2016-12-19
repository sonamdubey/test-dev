﻿using Bikewale.Entities.Dealer;
using Bikewale.Entities.DealerLocator;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.Dealer;
using Bikewale.Notifications;
using System;
using System.Collections.Generic;

namespace Bikewale.Cache.DealersLocator
{
    /// <summary>
    /// Created By : Lucky Rathore
    /// Created On : 21 March 2016
    /// </summary>
    public class DealerCacheRepository : IDealerCacheRepository
    {

        private readonly ICacheManager _cache;
        private readonly IDealer _objDealers;

        public DealerCacheRepository(ICacheManager cache, IDealer objDealers)
        {
            _cache = cache;
            _objDealers = objDealers;
        }

        /// <summary>
        /// Created By : Lucky Rathore on 21 March 2016
        /// Description : Cahing of Dealer detail By Make and City
        /// Modified By : Lucky Rathore on 30 March 2016
        /// Description : TIme reduced to 1/2 hour
        /// Modified By :   Sumit Kate on 19 Jun 2016
        /// Description :   Added Optional parameter(inherited from Interface) and create memcache key based on model id
        /// </summary>
        /// <param name="cityId">e.g. 1</param>
        /// <param name="makeId">e.g. 9</param>
        /// <returns>Dealers</returns>
        public DealersEntity GetDealerByMakeCity(uint cityId, uint makeId, uint modelId = 0)
        {
            Entities.DealerLocator.DealersEntity dealers = null;
            string key = modelId > 0 ? String.Format("BW_DealerList_Make_{0}_{1}_City_{2}", makeId, modelId, cityId) : String.Format("BW_DealerList_Make_{0}_City_{1}", makeId, cityId);
            try
            {
                dealers = _cache.GetFromCache<Entities.DealerLocator.DealersEntity>(key, new TimeSpan(0, 30, 0), () => _objDealers.GetDealerByMakeCity(cityId, makeId, modelId));
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "DealerCacheRepository.GetDealerByMakeCity");
                objErr.SendMail();
            }
            return dealers;
        }

        /// <summary>
        /// Created By : Lucky Rathore on 21 March 2016
        /// Description : Cahing of bike models for specific dealer
        /// Modified By  :Sushil Kumar on 22 March 2016
        /// Description : Changed Cacke key from BWDealerBikeModel_{0} to BW_DealerBikeModel_{0}
        /// Modified By : Lucky Rathore on 30 March 2016
        /// Description : TIme reduced to 1/2 hour.
        /// </summary>
        /// <param name="dealerId">e.g. 1</param>
        /// <returns>DealerBikesEntity</returns>
        public DealerBikesEntity GetDealerDetailsAndBikes(uint dealerId, uint campaignId)
        {
            DealerBikesEntity models = null;
            string key = String.Format("BW_DealerBikeModel_{0}", dealerId);
            try
            {
                models = _cache.GetFromCache<DealerBikesEntity>(key, new TimeSpan(0, 30, 0), () => _objDealers.GetDealerDetailsAndBikes(dealerId, campaignId));
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "DealerCacheRepository.GetDealerDetailsAndBikes");
                objErr.SendMail();
            }
            return models;
        }

        /// <summary>
        /// Created By : Sajal Gupta on 26/09/2016
        /// Description : Calls BAL method to get dealer's bikes and details on the basis of dealerId and makeId.
        /// </summary>
        public DealerBikesEntity GetDealerDetailsAndBikesByDealerAndMake(uint dealerId, int makeId)
        {
            DealerBikesEntity models = null;
            string key = String.Format("BW_DealerBikeModel_{0}_{1}", dealerId, makeId);
            try
            {
                models = _cache.GetFromCache<DealerBikesEntity>(key, new TimeSpan(0, 30, 0), () => _objDealers.GetDealerDetailsAndBikesByDealerAndMake(dealerId, makeId));
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "DealerCacheRepository.GetDealerDetailsAndBikes");
                objErr.SendMail();
            }
            return models;
        }

        /// <summary>
        /// Craeted by  :   Sumit Kate on 21 Jun 2016
        /// Description :   Get Cached Popular City Dealer Count
        /// </summary>
        /// <param name="makeId"></param>
        /// <returns></returns>
        public IEnumerable<PopularCityDealerEntity> GetPopularCityDealer(uint makeId, uint topCount)
        {
            IEnumerable<PopularCityDealerEntity> cityDealers = null;
            string key = String.Format("BW_MakePopularCity_Dealers_{0}_Cnt_{1}", makeId, topCount);
            try
            {
                cityDealers = _cache.GetFromCache<IEnumerable<PopularCityDealerEntity>>(key, new TimeSpan(0, 30, 0), () => _objDealers.GetPopularCityDealer(makeId, topCount));
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "DealerCacheRepository.GetPopularCityDealer");
                objErr.SendMail();
            }
            return cityDealers;
        }


        public IEnumerable<NewBikeDealersMakeEntity> GetDealersMakesList()
        {
            IEnumerable<NewBikeDealersMakeEntity> dealersMakes = null;
            string key = String.Format("BW_DealerMakes_List");
            try
            {
                dealersMakes = _cache.GetFromCache<IEnumerable<NewBikeDealersMakeEntity>>(key, new TimeSpan(1, 0, 0), () => _objDealers.GetDealersMakesList());
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "DealerCacheRepository.GetPopularCityDealer");
                objErr.SendMail();
            }
            return dealersMakes;
        }

        public IEnumerable<DealerBrandEntity> GetDealerByBrandList()
        {

            IEnumerable<DealerBrandEntity> dealersMakes = null;
            string key = String.Format("BW_DealerByBrand_List");
            try
            {
                dealersMakes = _cache.GetFromCache<IEnumerable<DealerBrandEntity>>(key, new TimeSpan(1, 0, 0), () => _objDealers.GetDealerByBrandList());
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "DealerCacheRepository.GetDealerByBrandList");
                objErr.SendMail();
            }
            return dealersMakes;

        }
    }
}
