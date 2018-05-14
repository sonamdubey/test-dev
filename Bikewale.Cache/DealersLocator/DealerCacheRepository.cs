using Bikewale.Entities.Dealer;
using Bikewale.Entities.DealerLocator;
using Bikewale.Entities.Location;
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
        private readonly IDealerRepository _objDealersRepository;


        public DealerCacheRepository(ICacheManager cache, IDealerRepository objDealersRepository)
        {
            _cache = cache;
            _objDealersRepository = objDealersRepository;
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
            string key = modelId > 0 ? String.Format("BW_DealerList_Make_{0}_{1}_City_{2}_v1", makeId, modelId, cityId) : String.Format("BW_DealerList_Make_{0}_City_{1}_v1", makeId, cityId);
            try
            {
                TimeSpan cacheTime = new TimeSpan(1, 0, 0);
                if (modelId > 0)
                {
                    cacheTime = new TimeSpan(12, 0, 0);
                }
                dealers = _cache.GetFromCache<Entities.DealerLocator.DealersEntity>(key, cacheTime, () => _objDealersRepository.GetDealerByMakeCity(cityId, makeId, modelId));
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "DealerCacheRepository.GetDealerByMakeCity");

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
                models = _cache.GetFromCache(key, new TimeSpan(0, 30, 0), () => _objDealersRepository.GetDealerDetailsAndBikes(dealerId, campaignId));
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "DealerCacheRepository.GetDealerDetailsAndBikes");

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
            string key = String.Format("BW_DealerBikeModel_V_{0}_{1}", dealerId, makeId);
            try
            {
                models = _cache.GetFromCache<DealerBikesEntity>(key, new TimeSpan(0, 30, 0), () => _objDealersRepository.GetDealerDetailsAndBikesByDealerAndMake(dealerId, makeId));
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "DealerCacheRepository.GetDealerDetailsAndBikes");

            }
            return models;
        }

        /// <summary>
        /// Created by  : Vivek Singh Tomar on 21st dec 2017 
        /// </summary>
        /// <param name="dealerId"></param>
        /// <param name="makeId"></param>
        /// <returns></returns>
        public DealerBikeModelsEntity GetBikesByDealerAndMake(uint dealerId, uint makeId)
        {
            DealerBikeModelsEntity models = null;
            string key = String.Format("BW_BikeModelsByDealer_V1_{0}_{1}", dealerId, makeId);
            try
            {
                models = _cache.GetFromCache(key, new TimeSpan(1, 0, 0, 0), () => _objDealersRepository.GetBikesByDealerAndMake(dealerId, makeId));
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("DealerCacheRepository.GetBikesByDealerAndMake. dealerId = {0}, makeId = {1}", dealerId, makeId));

            }
            return models;
        }

        /// <summary>
        /// Craeted by  :   Sumit Kate on 21 Jun 2016
        /// Description :   Get Cached Popular City Dealer Count
        /// Modified by :  Subodh Jain on 21 Dec 2016
        /// Description :   Merge Dealer and service center for make and model page
        /// Modified by :  Snehal Dange on 12 Oct 2017
        /// Description :   Versioned cache key
        /// <param name="makeId"></param>
        /// <returns></returns>
        public PopularDealerServiceCenter GetPopularCityDealer(uint makeId, uint topCount)
        {
            PopularDealerServiceCenter cityDealers = null;
            string key = String.Format("BW_MakePopularCity_Dealers_{0}_Cnt_{1}_V2", makeId, topCount);
            try
            {
                cityDealers = _cache.GetFromCache<PopularDealerServiceCenter>(key, new TimeSpan(0, 30, 0), () => _objDealersRepository.GetPopularCityDealer(makeId, topCount));
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "DealerCacheRepository.GetPopularCityDealer");

            }
            return cityDealers;
        }


        public IEnumerable<NewBikeDealersMakeEntity> GetDealersMakesList()
        {
            IEnumerable<NewBikeDealersMakeEntity> dealersMakes = null;
            string key = String.Format("BW_DealerMakes_List");
            try
            {
                dealersMakes = _cache.GetFromCache<IEnumerable<NewBikeDealersMakeEntity>>(key, new TimeSpan(1, 0, 0), () => _objDealersRepository.GetDealersMakesList());
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "DealerCacheRepository.GetPopularCityDealer");

            }
            return dealersMakes;
        }
        /// <summary>
        /// Created By : Subodh Jain on 20 Dec 2016
        /// Summary    : To bind dealers data by brand
        /// </summary>
        public IEnumerable<DealerBrandEntity> GetDealerByBrandList()
        {

            IEnumerable<DealerBrandEntity> dealersMakes = null;
            string key = String.Format("BW_DealerByBrand_List");
            try
            {
                dealersMakes = _cache.GetFromCache<IEnumerable<DealerBrandEntity>>(key, new TimeSpan(1, 0, 0), () => _objDealersRepository.GetDealerByBrandList());
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "DealerCacheRepository.GetDealerByBrandList");

            }
            return dealersMakes;

        }

        /// <summary>
        /// Created by  :   Sajal Gupta on 19-12-2016
        /// Description :   Fetch dealers count for nearby city.
        /// </summary>
        /// <param name="makeId"></param>
        /// <returns></returns>
        public IEnumerable<NearByCityDealerCountEntity> FetchNearByCityDealersCount(uint makeId, uint cityId)
        {
            IEnumerable<NearByCityDealerCountEntity> objDealerCountList = null;
            string key = String.Format("BW_NearByCityDealers_{0}_{1}", makeId, cityId);
            try
            {
                objDealerCountList = _cache.GetFromCache<IEnumerable<NearByCityDealerCountEntity>>(key, new TimeSpan(1, 0, 0), () => _objDealersRepository.FetchNearByCityDealersCount(makeId, cityId));
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("exception in CAche layer for FetchNearByCityDealersCount {0}, {1}", makeId, cityId));

            }
            return objDealerCountList;
        }

        public IEnumerable<CityEntityBase> FetchDealerCitiesByMake(uint makeId)
        {
            IEnumerable<CityEntityBase> objDealerCitytList = null;
            string key = String.Format("BW_CityDealerCount_{0}", makeId);
            try
            {
                objDealerCitytList = _cache.GetFromCache<IEnumerable<CityEntityBase>>(key, new TimeSpan(1, 0, 0), () => _objDealersRepository.FetchDealerCitiesByMake(makeId));
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("exception in CAche layer for FetchDealerCitiesByMake {0}", makeId));
            }
            return objDealerCitytList;

        }

        /// <summary>
        /// Created by  :   Sumit Kate on 27 Dec 2017
        /// Description :   Returns the Bike version price components from cache
        /// </summary>
        /// <param name="dealerId"></param>
        /// <param name="versionId"></param>
        /// <returns></returns>
        public DealerVersionPrices GetBikeVersionPrice(uint dealerId, uint versionId)
        {
            DealerVersionPrices versionPrice = null;
            string key = String.Format("BW_Dealer_{0}_Version_{1}", dealerId, versionId);
            try
            {
                versionPrice = _cache.GetFromCache<DealerVersionPrices>(key, new TimeSpan(1, 0, 0, 0), () => _objDealersRepository.GetBikeVersionPrice(dealerId, versionId));
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("DealerCacheRepo.GetBikeVersionPrice({0},{1})", dealerId, versionId));
            }
            return versionPrice;
        }
    }
}
