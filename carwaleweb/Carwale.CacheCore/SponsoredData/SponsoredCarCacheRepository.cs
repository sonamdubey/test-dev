using AEPLCore.Cache;
using AEPLCore.Cache.Interfaces;
using Carwale.Entity.CarData;
using Carwale.Entity.Sponsored;
using Carwale.Interfaces;
using Carwale.Interfaces.SponsoredCar;
using Carwale.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Cache.SponsoredData
{
    public class SponsoredCarCacheRepository : ISponsoredCarCache
    {
        private readonly ISponsoredCar _sponsoredCar;
        private readonly ICacheManager _cacheProvider;

        public SponsoredCarCacheRepository(ISponsoredCar sponsoredCarRepo, ICacheManager cacheProvider)
        {
            _sponsoredCar = sponsoredCarRepo;
            _cacheProvider = cacheProvider;
        }

        /// <summary>
        /// Returns the list of Sponsored Campaigns based on categoryId and platformId passed from Cache 
        /// </summary>
        /// <param name="categoryId">CampaignCategoryId</param>
        /// <param name="platformId">PlatformId</param>
        /// <param name="categorySection">categorySection</param>
        /// <param name="param">can be modelid/versionid comma seperated values</param>
        /// <returns></returns>
        public List<Sponsored_Car> GetSponsoredCampaigns(int categoryId, int platformId, int categorySection, string param, int applicationId)
        {
            //Fetch sponsored campaign from cache
            string key = "Sponsored_Campaign_v3_" + applicationId + "_" + categoryId + (categorySection > 0 ? "_" + categorySection : "") + "_" + platformId + (string.IsNullOrWhiteSpace(param) ? string.Empty : "_" + param);
            var CacheObj = _cacheProvider.GetFromCache<List<Sponsored_Car>>(key);

            try
            {
                if (CacheObj == null)// if sponsored campaign does not exists in cache
                {
                    DateTime nextCampaignStartDate;
                    CacheObj = _sponsoredCar.GetSponsoredCampaigns(categoryId, platformId, categorySection, out nextCampaignStartDate, param, applicationId); //fetch sponsored campaign from database

                    if (CacheObj != null)//if campaign exists either sponsored or default
                    {
                        TimeSpan cacheDuration = CacheRefreshTime.DefaultRefreshTime();

                        if (CacheObj.Count < 1 || CacheObj.FirstOrDefault() == default(Sponsored_Car) || CacheObj.FirstOrDefault().EndDate == default(DateTime) || CacheObj.FirstOrDefault().StartDate == default(DateTime) || CacheObj[0].IsDafault)
                        {
                            //set cache duration for default campaign
                            cacheDuration = (TimeSpan)(nextCampaignStartDate - DateTime.Now);
                        }
                        else if (CacheObj[0].EndDate != null && CacheObj[0].StartDate != null)
                        {
                            //Set cache duration for sponsored campaign
                            cacheDuration = (TimeSpan)(CacheObj[0].EndDate - DateTime.Now /*CacheObj[0].StartDate //Was StartDate before -Rohan.s */ );
                        }

                        return _cacheProvider.GetFromCache<List<Sponsored_Car>>(key,
                              cacheDuration,
                                  () => CacheObj);
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "SponsoredCarCacheRepository.GetSponsoredCampaigns()");
                objErr.LogException();
            }

            return CacheObj;
        }

        public int GetFeaturedCar(string versionIds, int categoryId, int platformId)
        {
            try
            {
                return _cacheProvider.GetFromCache<int>(string.Format("GetFeatureCar_{0}_{1}_{2}", versionIds,categoryId,platformId),
                              new TimeSpan(1, 0, 0, 0, 0),//1day
                                  () => _sponsoredCar.GetFeaturedCar(versionIds,categoryId,platformId));
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "SponsoredCarCacheRepository.GetMiscellaneousScript()");
                objErr.LogException();
            }
            return -1;
        }

        public List<SponsoredHistoryModels> GetAllSponsoredHistoryModels(int platformId)
        {
            return _cacheProvider.GetFromCache<List<SponsoredHistoryModels>>("GlobalSearchHistoryAds_" + platformId,
                CacheRefreshTime.OneDayExpire(),
                () => _sponsoredCar.GetAllSponsoredHistoryModels(platformId));
        }

        public List<GlobalSearchSponsoredModelEntity> GetAllSponsoredTrendingCars(int platformId)
        {
            try
            {
                return _cacheProvider.GetFromCache<List<GlobalSearchSponsoredModelEntity>>(string.Format("GlobalSearchTrendingAds_{0}", platformId),
                              new TimeSpan(1, 0, 0, 0, 0),//1day
                                  () => _sponsoredCar.GetAllSponsoredTrendingCars(platformId));
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "SponsoredCarCacheRepository.GetAllSponsoredTrendingCars()");
                objErr.LogException();
            }
            return null;
        }
    }
}
