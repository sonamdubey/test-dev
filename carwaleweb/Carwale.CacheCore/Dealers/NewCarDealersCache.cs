using AEPLCore.Cache;
using AEPLCore.Cache.Interfaces;
using Carwale.Entity;
using Carwale.Entity.Campaigns;
using Carwale.Entity.CarData;
using Carwale.Entity.Dealers;
using Carwale.Interfaces;
using Carwale.Interfaces.Dealers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Cache.Dealers
{
    public class NewCarDealersCache : INewCarDealersCache
    {
        protected readonly INewCarDealersRepository _dealersRepo;
        protected readonly ICacheManager _cacheProvider;

        public NewCarDealersCache(INewCarDealersRepository dealersRepo, ICacheManager cacheProvider)
        {
            _dealersRepo = dealersRepo;
            _cacheProvider = cacheProvider;
        }

        /// <summary>
        /// Gets the list of makes from memcache based on cityId passed. If list not available in cache gets the list from database
        /// Written By : Supriya on 29/5/2014
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public List<CarMakeEntityBase> GetMakesByCity(int cityId)
        {
            return _cacheProvider.GetFromCache<List<CarMakeEntityBase>>("NewCarDealersMakesByCity_" + cityId,
                        CacheRefreshTime.GetInMinutes("DefaultCacheRefreshTime"),
                            () => _dealersRepo.GetMakesByCity(cityId));
        }

        /// <summary>
        /// Gets the list of cities from memcache based on makeId passed. If list not available in cache gets the list from database.
        /// Written By : Supriya on 8/9/2014
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public List<DealerStateEntity> GetCitiesByMake(int makeId)
        {
            return _cacheProvider.GetFromCache<List<DealerStateEntity>>($"NCDCitiesByMake_{makeId}_v1",
                        CacheRefreshTime.GetInMinutes("DefaultCacheRefreshTime"),
                            () => _dealersRepo.GetCitiesByMake(makeId));
        }

        /// <summary>
        /// Gets the list of populracities from memcache based on makeId passed. If list not available in cache gets the list from database.
        /// Written By : Supriya on 8/9/2014
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public List<PopularCitiesEntity> GetPopularCitiesByMake(int makeId)
        {
            return _cacheProvider.GetFromCache<List<PopularCitiesEntity>>("NCDPopularCitiesByMake_" + makeId,
                        CacheRefreshTime.GetInMinutes("DefaultCacheRefreshTime"),
                            () => _dealersRepo.GetPopularCitiesByMake(makeId));
        }

        public List<NewCarDealerCountByMake> NewCarDealerCountMake(string type)
        {
            return _cacheProvider.GetFromCache<List<NewCarDealerCountByMake>>("DealerMakeCount_" + type.ToLower(),
                        CacheRefreshTime.GetInMinutes("DefaultCacheRefreshTime"),
                            () => _dealersRepo.GetCarCountByMakesAndType(type));
        }

        public List<MakeModelEntity> GetDealerModels(int dealerId)
        {
            return _cacheProvider.GetFromCache<List<MakeModelEntity>>("Dealer_" + dealerId + "_Models_v1", CacheRefreshTime.NeverExpire(),
                () => _dealersRepo.GetDealerModels(dealerId));
        }

        public IEnumerable<NewCarDealersList> GetNCSDealers(int modelId, int cityId)
        {
            return _cacheProvider.GetFromCache<IEnumerable<NewCarDealersList>>(String.Format("NCSDealers-v2-{0}-{1}", modelId, cityId), CacheRefreshTime.OneDayExpire(),
                () => _dealersRepo.GetNCSDealers(modelId,cityId));
        }

        public void StoreDealersList(string cacheKey,NewCarDealerEntiy newCarDealers)
        {
            _cacheProvider.StoreToCache<NewCarDealerEntiy>(cacheKey, CacheRefreshTime.OneHourWithEOD(),newCarDealers);
        }

        public void StoreDealerDetails(string cacheKey, DealerDetails dealerDetails)
        {
            _cacheProvider.StoreToCache<DealerDetails>(cacheKey, CacheRefreshTime.GetInMinutes("DefaultCacheRefreshTime"), dealerDetails);
        }

        public List<ClientCampaignMapping> GetClientCampaignMapping()
        {
            return _cacheProvider.GetFromCache<List<ClientCampaignMapping>>("ClientCampaignMapping", CacheRefreshTime.EODExpire(),
                () => _dealersRepo.GetClientCampaignMapping());
        }
        public IEnumerable<NewCarDealer> GetDealerListByCityMake(int makeId, int cityId)
        {
            return _cacheProvider.GetFromCache<IEnumerable<NewCarDealer>>(string.Format("NewCarDealers_MakeId{0}_CityId{1}" , makeId , cityId),
                        CacheRefreshTime.GetInMinutes("DefaultCacheRefreshTime"),
                            () => _dealersRepo.GetDealerListByCityMake(makeId, cityId));

        }
    }
}
