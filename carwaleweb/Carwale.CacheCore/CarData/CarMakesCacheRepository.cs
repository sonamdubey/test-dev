using AEPLCore.Cache;
using AEPLCore.Cache.Interfaces;
using Carwale.Cache.Core;
using Carwale.Entity;
using Carwale.Entity.CarData;
using Carwale.Entity.CentralizedCacheRefresh;
using Carwale.Entity.Common;
using Carwale.Entity.Enum;
using Carwale.Entity.Geolocation;
using Carwale.Interfaces;
using Carwale.Notifications.Logs;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Carwale.Cache.CarData
{
	public class CarMakesCacheRepository : ICarMakesCacheRepository
    {
        //convention to be followed 
        //cachekey = CacheKeyPrefix_cacheKeySuffix
        //CacheKeyPrefix = "CW_VERSION_{PASCAL LETTER OF FUNCTION NAME}"  //EXAMPLE:FOR GetVersionSummaryByModel() IT WOULD BE "GVSBM"
        //cacheKeySuffix = _{0}_{1}_{2}_{3}_{4}_{5}_{6}_{7}
        //THINK BEFORE CACHE FUNCTION NAME (DUPLICATE PREFIX ISSUE)
		private static readonly Dictionary<string, string> _cacheKeyPrefix = new Dictionary<string, string>
		   {
			{"GetCarMakesFromLocalCache","CW_MAKE_GCMFLC"},
			{"GetCarMakesByType","CW_MAKE_GCMBT"},
			{"GetCarMakeDescription","CW_MAKE_GCMD_D0"}, //DUPLICATE PREFIX CONVENTION
			{"GetMakePageMetaTags","CW_MAKE_GMPMT"},
			{"GetCarMakeDetails","CW_MAKE_GCMD_D1"},  //DUPLICATE PREFIX CONVENTION
			{"GetMakes","CW_MAKE_GM"},
			{"GetSummary","CW_MAKE_GS"},
			{"GetCarMakesWithLogo","CW_MAKE_GCMWL"},
			{"GetMakeDetailsByName","CW_MAKE_GMDBN"},
			{"GetMakeListWithDealerAvailable","CW_MAKE_GMLWDA"},
			{"GetAllCitiesHavingDealerByMake","CW_MAKE_GACHDBM"},
			{"GetDealerAvailabilityForMakeCity","CW_MAKE_GDAFMC"}
		};

		protected readonly ICarMakesRepository _makesRepo;
        protected readonly ICacheManager _cacheProvider;
        private static readonly int _suffixArrCount = typeof(MakeAttribute).GetProperties().Length;
		public CarMakesCacheRepository(ICarMakesRepository makesRepo, ICacheManager cacheProvider)
        {
            _makesRepo = makesRepo;
            _cacheProvider = cacheProvider;
        }

        /// <summary>
        /// Get car makes from local cache if available otherwise fetch it from database
        /// </summary>
        /// <returns></returns>
        public List<CarMakeEntityBase> GetCarMakesFromLocalCache()
        {
            string[] cacheKeySuffixArr = new string[_suffixArrCount];
            cacheKeySuffixArr[(int)MakeParameter.Type] = "all";
            return _cacheProvider.GetFromCache<List<CarMakeEntityBase>>(_cacheProvider.GenerateCacheKey(_cacheKeyPrefix["GetCarMakesFromLocalCache"], cacheKeySuffixArr),
                CacheRefreshTime.NeverExpire(), () => _makesRepo.GetCarMakesByType("all"), () => _makesRepo.GetCarMakesByType("all", null, null, 0, true));
        }

        /// <summary>
        /// Get car makes from memcache if available otherwise fetch it from database
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public List<CarMakeEntityBase> GetCarMakesByType(string type, Modules? module = null, bool? isPopular = null, int filter = 0)
        {
			string[] cacheKeySuffixArr = new string[_suffixArrCount];
			cacheKeySuffixArr[(int)MakeParameter.Type] = type.ToLower();
			cacheKeySuffixArr[(int)MakeParameter.Module] = module?.ToString().ToLower();
			cacheKeySuffixArr[(int)MakeParameter.IsPopular] = isPopular?.ToString().ToLower();
			cacheKeySuffixArr[(int)MakeParameter.Filter] = (filter == 0)? null : filter.ToString();
			return _cacheProvider.GetFromCache<List<CarMakeEntityBase>>(_cacheProvider.GenerateCacheKey(_cacheKeyPrefix["GetCarMakesByType"], cacheKeySuffixArr),
            CacheRefreshTime.DefaultRefreshTime(),() => _makesRepo.GetCarMakesByType(type, module, isPopular, filter),() => _makesRepo.GetCarMakesByType(type, module, isPopular, filter,true));
        }
        /// <summary>
        /// Gets the Car Make Description based on makeId passed 
        /// Written By : Shalini on 15/07/14
        /// </summary>
        /// <param name="makeId"></param>
        /// <returns></returns>
        public CarMakeDescription GetCarMakeDescription(int makeId)
        {
            string[] cacheKeySuffixArr = new string[_suffixArrCount];
			cacheKeySuffixArr[(int)MakeParameter.Id] = makeId.ToString();
            return _cacheProvider.GetFromCache<CarMakeDescription>(_cacheProvider.GenerateCacheKey(_cacheKeyPrefix["GetCarMakeDescription"], cacheKeySuffixArr),
                CacheRefreshTime.NeverExpire(),() => _makesRepo.GetCarMakeDescription(makeId), () => _makesRepo.GetCarMakeDescription(makeId,true));
        }

        public List<Entity.Common.ValuationMake> GetValuationMakes(int Year)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns the MakePage Metatags
        /// Written By : Shalini on 24/11/14
        /// </summary>
        /// <param name="makeId"></param>
        /// <param name="pageId"></param>
        /// <returns></returns>
        public PageMetaTags GetMakePageMetaTags(int makeId, int pageId)
        {
            string[] cacheKeySuffixArr = new string[_suffixArrCount];
			cacheKeySuffixArr[(int)MakeParameter.Id] = makeId.ToString();
            return _cacheProvider.GetFromCache<PageMetaTags>(_cacheProvider.GenerateCacheKey(_cacheKeyPrefix["GetMakePageMetaTags"], cacheKeySuffixArr),
               CacheRefreshTime.DefaultRefreshTime(),() => _makesRepo.GetMakePageMetaTags(makeId, pageId), () => _makesRepo.GetMakePageMetaTags(makeId, pageId,true));
        }

        /// <summary>
        /// Returns the Make Details from Cache if available or from Database
        /// Written By : Shalini on 24/11/14
        /// </summary>
        /// <param name="makeId"></param>
        /// <returns></returns>
        public CarMakeEntityBase GetCarMakeDetails(int makeId)
        {
            string[] cacheKeySuffixArr = new string[_suffixArrCount];
			cacheKeySuffixArr[(int)MakeParameter.Id] = makeId.ToString();
            return _cacheProvider.GetFromCache<CarMakeEntityBase>(_cacheProvider.GenerateCacheKey(_cacheKeyPrefix["GetCarMakeDetails"], cacheKeySuffixArr),
                CacheRefreshTime.NeverExpire(),() => _makesRepo.GetCarMakeDetails(makeId), () => _makesRepo.GetCarMakeDetails(makeId,true));
        }

        public IEnumerable<CarMakeEntityBase> GetMakes(string makeIds="", Modules module = Modules.Default)
        {
            string[] cacheKeySuffixArr = new string[_suffixArrCount];
			cacheKeySuffixArr[(int)MakeParameter.Ids] = makeIds??string.Empty;
			cacheKeySuffixArr[(int)MakeParameter.Module] = module.ToString().ToLower();
            return _cacheProvider.GetFromCache<IEnumerable<CarMakeEntityBase>>(_cacheProvider.GenerateCacheKey(_cacheKeyPrefix["GetMakes"], cacheKeySuffixArr),
              CacheRefreshTime.DefaultRefreshTime(),() => _makesRepo.GetMakes(module, makeIds), () => _makesRepo.GetMakes(module, makeIds,true));
        }

        /// <summary>
        /// Returns the Summary for Make page
        /// Written By : Shalini on 26/11/14
        /// </summary>
        /// <param name="makeId"></param>
        /// <returns></returns>
        public List<CarMakeDescription> GetSummary(int makeId)
        {
            string[] cacheKeySuffixArr = new string[_suffixArrCount];
			cacheKeySuffixArr[(int)MakeParameter.Id] = makeId.ToString();
            return _cacheProvider.GetFromCache<List<CarMakeDescription>>(_cacheProvider.GenerateCacheKey(_cacheKeyPrefix["GetSummary"], cacheKeySuffixArr),
                CacheRefreshTime.NeverExpire(), () => _makesRepo.GetSummary(makeId), () => _makesRepo.GetSummary(makeId,true));
        }

        /// <summary>
        /// Returns the Summary for Make page
        /// Written By : Shalini on 26/11/14
        /// </summary>
        /// <param name="makeId"></param>
        /// <returns></returns>
        public List<MakeLogoEntity> GetCarMakesWithLogo(string type)
        {
            string[] cacheKeySuffixArr = new string[_suffixArrCount];
			cacheKeySuffixArr[(int)MakeParameter.Type] = type.ToLower();
            return _cacheProvider.GetFromCache<List<MakeLogoEntity>>(_cacheProvider.GenerateCacheKey(_cacheKeyPrefix["GetCarMakesWithLogo"], cacheKeySuffixArr),
                CacheRefreshTime.NeverExpire(), () => _makesRepo.GetCarMakesWithLogo(type), () => _makesRepo.GetCarMakesWithLogo(type,true));
        }

        public CarMakesEntity GetMakeDetailsByName(string carMake)
        {
            string[] cacheKeySuffixArr = new string[_suffixArrCount];
			cacheKeySuffixArr[(int)MakeParameter.Name] = carMake;
            return _cacheProvider.GetFromCache<CarMakesEntity>(_cacheProvider.GenerateCacheKey(_cacheKeyPrefix["GetMakeDetailsByName"], cacheKeySuffixArr),
                CacheRefreshTime.NeverExpire(), () => _makesRepo.GetMakeDetailsByName(carMake), () => _makesRepo.GetMakeDetailsByName(carMake,true));
        }        

       public List<CarMakeEntityBase> GetMakeListWithDealerAvailable()
        {
            string[] cacheKeySuffixArr = new string[_suffixArrCount];
            return _cacheProvider.GetFromCache<List<CarMakeEntityBase>>(_cacheProvider.GenerateCacheKey(_cacheKeyPrefix["GetMakeListWithDealerAvailable"], cacheKeySuffixArr),
              CacheRefreshTime.NeverExpire(), () => _makesRepo.GetMakeListWithAvailableDealer(), () => _makesRepo.GetMakeListWithAvailableDealer(true));
        }
        public List<Cities> GetAllCitiesHavingDealerByMake(int makeId)
        {
            string[] cacheKeySuffixArr = new string[_suffixArrCount];
			cacheKeySuffixArr[(int)MakeParameter.Id] = makeId.ToString();
            return _cacheProvider.GetFromCache<List<Cities>>(_cacheProvider.GenerateCacheKey(_cacheKeyPrefix["GetAllCitiesHavingDealerByMake"], cacheKeySuffixArr),
              CacheRefreshTime.NeverExpire(), () => _makesRepo.GetAllCitiesHavingDealerByMake(makeId), () => _makesRepo.GetAllCitiesHavingDealerByMake(makeId,true));
        }
        public bool GetDealerAvailabilityForMakeCity(int make, int city)
        {
            string[] cacheKeySuffixArr = new string[_suffixArrCount];
			cacheKeySuffixArr[(int)MakeParameter.Id] = make.ToString();
			cacheKeySuffixArr[(int)MakeParameter.CityId] = city.ToString();
            return _cacheProvider.GetFromCache<bool>(_cacheProvider.GenerateCacheKey(_cacheKeyPrefix["GetDealerAvailabilityForMakeCity"], cacheKeySuffixArr),
              CacheRefreshTime.DefaultRefreshTime(), () => _makesRepo.GetDealerAvailabilityForMakeCity(make, city), () => _makesRepo.GetDealerAvailabilityForMakeCity(make, city,true));
        }
        public bool RefreshCarMakeCache(List<MakeAttribute> makeAttributes)
        {
            try
            {
                bool isRefreshed = false;
                string[] suffixArr = new string[_suffixArrCount];
                foreach (var make in makeAttributes)
                {
                    suffixArr[(int)MakeParameter.Id] = make.Id;
                    suffixArr[(int)MakeParameter.Type] = make.Type;
                    suffixArr[(int)MakeParameter.Module] = make.Module;
                    suffixArr[(int)MakeParameter.IsPopular] = make.IsPopular;
                    suffixArr[(int)MakeParameter.Filter] = make.Filter;
                    suffixArr[(int)MakeParameter.Ids] = make.Ids;
                    suffixArr[(int)MakeParameter.Name] = make.Name;
                    suffixArr[(int)MakeParameter.CityId] = make.CityId;
                    foreach (var key in CarMakesCacheRepository._cacheKeyPrefix.Values)
                    {
                        string finalKey = _cacheProvider.GenerateCacheKey(key, suffixArr);
                        isRefreshed = _cacheProvider.ExpireCacheWithCriticalRead(finalKey);
                        if (!isRefreshed)
                        {
                            Logger.LogException(null, "MemCache key name = " + finalKey + " is not refreshed.");
                            isRefreshed = true;
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return false;
            }
        }
	}
}