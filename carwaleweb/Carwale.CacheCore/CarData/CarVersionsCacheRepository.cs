using AEPLCore.Cache;
using Carwale.Interfaces;
using Carwale.Interfaces.CarData;
using System.Collections.Generic;
using Carwale.Entity.CarData;
using System;
using Carwale.Entity;
using Carwale.Entity.CompareCars;
using AEPLCore.Cache.Interfaces;
using Carwale.Entity.CentralizedCacheRefresh;
using Carwale.Entity.Enum;
using Carwale.Notifications.Logs;

namespace Carwale.Cache.CarData
{
    public class CarVersionsCacheRepository : ICarVersionCacheRepository
    {
        //Cache key convention to be followed 
        //Cachekey = CacheKeyPrefix_CacheKeySuffix
        //CacheKeyPrefix = "CW_VERSION_{PASCAL LETTER OF FUNCTION NAME}"  //EXAMPLE:FOR GetVersionSummaryByModel() IT WOULD BE "GVSBM"
        //cacheKeySuffix = _{0}_{1}_{2}_{3}_{4}_{5}_{6}_{7}
        //THINK BEFORE CACHE FUNCTION NAME (DUPLICATE PREFIX ISSUE)
        private static readonly Dictionary<string, string> _cacheKeyPrefix = new Dictionary<string, string>
         {
            {"GetVersionSummaryByModel","CW_VERSION_GVSBM"},
            {"GetCarVersionsByType","CW_VERSION_GCVBT"},
            {"GetVersionDetailsById","CW_VERSION_GVDBI_V1"},
            {"GetOtherCarVersionsOfModel","CW_VERSION_GOCVOM"},
            {"GetDefaultVersionId","CW_VERSION_GDVI"},
            {"GetVersionInfoFromMaskingName","CW_VERSION_GVIFMN"},
            {"GetVersionColors","CW_VERSION_GVC"},
            { "GetVersionCountByModel","CW_VERSION_GVCBM"}
        };

        private readonly ICarVersionRepository _carVersionsRepo;
        private readonly ICacheManager _cacheProvider;
        private static readonly int _suffixArrCount = typeof(VersionAttribute).GetProperties().Length;
        public CarVersionsCacheRepository(ICarVersionRepository carVersionsRepo, ICacheManager cacheProvider)
        {
            _carVersionsRepo = carVersionsRepo;
            _cacheProvider = cacheProvider;
        }

        /// <summary>
        /// Gets the list of CarVersions based on the modelId passed
        /// Written By : Shalini on 14/07/14
        /// </summary>
        /// <param name="modelId"></param>
        /// <returns></returns>
        public List<CarVersions> GetVersionSummaryByModel(int modelId, Status status)
        {
            string[] cacheKeySuffixArr = new string[_suffixArrCount];
            cacheKeySuffixArr[(int)VersionParameter.ModelId] = modelId.ToString();
            cacheKeySuffixArr[(int)VersionParameter.Status] = status.ToString().ToLower();
            return _cacheProvider.GetFromCache<List<CarVersions>>(_cacheProvider.GenerateCacheKey(_cacheKeyPrefix["GetVersionSummaryByModel"], cacheKeySuffixArr),
                CacheRefreshTime.NeverExpire(), () => _carVersionsRepo.GetVersionSummaryByModel(modelId, status), () => _carVersionsRepo.GetVersionSummaryByModel(modelId, status, true));
        }

        public List<CarVersionEntity> GetCarVersionsByType(string type, int modelId, UInt16? year = null)
        {
            string[] cacheKeySuffixArr = new string[_suffixArrCount];
            cacheKeySuffixArr[(int)VersionParameter.ModelId] = modelId.ToString();
            cacheKeySuffixArr[(int)VersionParameter.Type] = type.ToLower();
            cacheKeySuffixArr[(int)VersionParameter.Year] = year.ToString();
            return _cacheProvider.GetFromCache<List<CarVersionEntity>>(_cacheProvider.GenerateCacheKey(_cacheKeyPrefix["GetCarVersionsByType"], cacheKeySuffixArr),
                CacheRefreshTime.DefaultRefreshTime(), () => _carVersionsRepo.GetCarVersionsByType(type, modelId, year), () => _carVersionsRepo.GetCarVersionsByType(type, modelId, year, true));
        }

        /// <summary>
        /// Gets the list of Car details base on car versionid
        /// Written By : Ashish Verma on 29/09/14
        /// </summary>
        /// <param name="modelId"></param>
        /// <returns></returns>
        public CarVersionDetails GetVersionDetailsById(int versionId)
        {
            string[] cacheKeySuffixArr = new string[_suffixArrCount];
            cacheKeySuffixArr[(int)VersionParameter.Id] = versionId.ToString();
            return _cacheProvider.GetFromCache<CarVersionDetails>(_cacheProvider.GenerateCacheKey(_cacheKeyPrefix["GetVersionDetailsById"], cacheKeySuffixArr),
              CacheRefreshTime.NeverExpire(), () => _carVersionsRepo.GetVersionDetailsById(versionId), () => _carVersionsRepo.GetVersionDetailsById(versionId, true));
        }
        /// <summary>
        /// Get List Of versions
        /// </summary>
        /// <param name="versionList"></param>
        /// <returns></returns>
        public Dictionary<int, CarVersionDetails> MultiGetVersionDetails(List<int> versionList)
        {
            var versionsDetails = new Dictionary<int, CarVersionDetails>();
            foreach (var version in versionList)
            {
                if (!versionsDetails.ContainsKey(version))
                {
                    versionsDetails.Add(version, GetVersionDetailsById(version));
                }
            }
            return versionsDetails;
        }
        /// <summary>
        /// Gets the List of Other Car Versions of a Model from cache,if available or from Database
        /// Written By : Shalini Nair on 29/12/14
        /// </summary>
        /// <param name="modelId"></param>
        /// <param name="versionId"></param>
        /// <returns>List of Other Car Versions of a Model</returns>
        public List<CarVersions> GetOtherCarVersionsOfModel(int versionId)
        {
            string[] cacheKeySuffixArr = new string[_suffixArrCount];
            cacheKeySuffixArr[(int)VersionParameter.Id] = versionId.ToString();
            return _cacheProvider.GetFromCache<List<CarVersions>>(_cacheProvider.GenerateCacheKey(_cacheKeyPrefix["GetOtherCarVersionsOfModel"], cacheKeySuffixArr),
                CacheRefreshTime.NeverExpire(), () => _carVersionsRepo.GetOtherCarVersionsOfModel(versionId), () => _carVersionsRepo.GetOtherCarVersionsOfModel(versionId, true));
        }

        /// <summary>
        /// For Getting Default Version Id on basis of Minimum value of PriceQuote
        /// Written By : Sanjay Soni
        /// </summary>
        /// <param name="PQId">cityid,modelId</param>
        /// <returns>Default Version Id</returns>
        public int GetDefaultVersionId(int cityId, int modelId)
        {
            string[] cacheKeySuffixArr = new string[_suffixArrCount];
            cacheKeySuffixArr[(int)VersionParameter.ModelId] = modelId.ToString();
            cacheKeySuffixArr[(int)VersionParameter.CityId] = cityId.ToString();
            return _cacheProvider.GetFromCache<int>(_cacheProvider.GenerateCacheKey(_cacheKeyPrefix["GetDefaultVersionId"], cacheKeySuffixArr),
                    CacheRefreshTime.DefaultRefreshTime(), () => _carVersionsRepo.GetDefaultVersionId(cityId, modelId), () => _carVersionsRepo.GetDefaultVersionId(cityId, modelId, true));
        }
        public VersionMaskingResponse GetVersionInfoFromMaskingName(string maskingName, int modelId, int versionId)
        {
            string[] cacheKeySuffixArr = new string[_suffixArrCount];
            cacheKeySuffixArr[(int)VersionParameter.Id] = versionId.ToString();
            cacheKeySuffixArr[(int)VersionParameter.MaskingName] = maskingName;
            cacheKeySuffixArr[(int)VersionParameter.ModelId] = modelId.ToString();
            return _cacheProvider.GetFromCache<VersionMaskingResponse>(_cacheProvider.GenerateCacheKey(_cacheKeyPrefix["GetVersionInfoFromMaskingName"], cacheKeySuffixArr),
                CacheRefreshTime.NeverExpire(), () => _carVersionsRepo.GetVersionInfoFromMaskingName(maskingName, modelId, versionId), () => _carVersionsRepo.GetVersionInfoFromMaskingName(maskingName, modelId, versionId, true)) ?? new VersionMaskingResponse();
        }
        public List<Color> GetVersionColors(int versionId)
        {
            string[] cacheKeySuffixArr = new string[_suffixArrCount];
            cacheKeySuffixArr[(int)VersionParameter.Id] = versionId.ToString();
            return _cacheProvider.GetFromCache<List<Color>>(_cacheProvider.GenerateCacheKey(_cacheKeyPrefix["GetVersionColors"], cacheKeySuffixArr),
                CacheRefreshTime.NeverExpire(), () => _carVersionsRepo.GetVersionColors(versionId), () => _carVersionsRepo.GetVersionColors(versionId, true));
        }

        public int GetVersionCountByModel(int modelId)
        {
            string[] cacheKeySuffixArr = new string[_suffixArrCount];
            cacheKeySuffixArr[(int)VersionParameter.ModelId] = modelId.ToString();
            return _cacheProvider.GetFromCache<int>(_cacheProvider.GenerateCacheKey(_cacheKeyPrefix["GetVersionCountByModel"], cacheKeySuffixArr),
                CacheRefreshTime.NeverExpire(), () => _carVersionsRepo.GetVersionCountByModel(modelId), () => _carVersionsRepo.GetVersionCountByModel(modelId, true));
        }
        public bool RefreshCarVersionCache(List<VersionAttribute> versionAttributes)
        {
            try
            {
                bool isRefreshed = true;
                string[] suffixArr = new string[_suffixArrCount];
                foreach (var version in versionAttributes)
                {
                    suffixArr[(int)VersionParameter.Id] = version.Id;
                    suffixArr[(int)VersionParameter.Name] = version.Name;
                    suffixArr[(int)VersionParameter.MaskingName] = version.MaskingName;
                    suffixArr[(int)VersionParameter.ModelId] = version.ModelId;
                    suffixArr[(int)VersionParameter.Status] = version.Status;
                    suffixArr[(int)VersionParameter.Type] = version.Type;
                    suffixArr[(int)VersionParameter.Year] = version.Year;
                    suffixArr[(int)VersionParameter.CityId] = version.CityId;
                    foreach (var key in _cacheKeyPrefix.Values)
                    {
                        string finalKey = _cacheProvider.GenerateCacheKey(key, suffixArr);
                        isRefreshed = _cacheProvider.ExpireCacheWithCriticalRead(finalKey) && isRefreshed;
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
