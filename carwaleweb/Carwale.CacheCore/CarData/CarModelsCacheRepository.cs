using AEPLCore.Cache;
using AEPLCore.Cache.Interfaces;
using Carwale.Entity;
using Carwale.Entity.CarData;
using Carwale.Entity.CentralizedCacheRefresh;
using Carwale.Entity.Enum;
using Carwale.Interfaces;
using Carwale.Interfaces.CarData;
using Carwale.Notifications.Logs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Carwale.Cache.CarData
{
    public class CarModelsCacheRepository : ICarModelCacheRepository
    {
        //convention to be followed 
        //cachekey = _cacheKeyPrefix_cacheKeySuffix
        //_cacheKeyPrefix = "CW_VERSION_{PASCAL LETTER OF FUNCTION NAME}"  //EXAMPLE:FOR GetVersionSummaryByModel() IT WOULD BE "GVSBM"
        //cacheKeySuffix = _{0}_{1}_{2}_{3}_{4}_{5}_{6}_{7}_{8}_{9}_{10}_{11}
        //THINK BEFORE CACHE FUNCTION NAME (DUPLICATE PREFIX ISSUE)
        private static readonly Dictionary<string, string> _cacheKeyPrefix = new Dictionary<string, string>
           {
            {"GetCarModelsByType","CW_MODEL_GCMBT"},
            {"GetModelsByMake","CW_MODEL_GMBM"},
            {"GetAllModels","CW_MODEL_GAM"},
            {"GetAllModelsSummary","CW_MODEL_GAMS"},
            {"GetUpcomingCarModelsByMake","CW_MODEL_GUCMBMV1"},
            {"GetModelColorsByModel","CW_MODEL_GMCBM"},
            {"GetModelDetailsById","CW_MODEL_GMDBI"},
            {"GetPricesInOtherCities","CW_MODEL_GPIOC"},
            {"GetPricesForNearByCities","CW_MODEL_GPFNC"},
            {"GetUpcomingCarDetails","CW_MODEL_GUCDV1"},
            {"GetModelPageMetaTags","CW_MODEL_GMPMT"},
            {"GetTopSellingModels","CW_MODEL_GTSM"},
            {"GetLaunchedCarModels","CW_MODEL_GLCM"},
            {"GetLaunchedCarModelsV1","CW_MODEL_GLCMV"},
            {"GetModelsByBodyStyle","CW_MODEL_GMBBS"},
            {"GetTrendingModels","CW_MODEL_GTM"},
            {"GetCarRanksByBodyType","CW_MODEL_GCRBBT"},
            {"GetSimilarUpcomingCarModel","CW_MODEL_GSUCMV1"},
            {"GetUpgradedModel","CW_MODEL_GUM"},
            {"GetModelByMaskingName","CW_MODEL_GMBMN"},
            {"GetActiveModelsByMake","CW_MODEL_GAMBM"}
        };
        private readonly ICarModelRepository _carModelsRepo;
        private readonly ICacheManager _cacheProvider;
        private static readonly int _suffixArrCount = typeof(ModelAttribute).GetProperties().Length;


        public CarModelsCacheRepository(ICarModelRepository carModelsRepo, ICacheManager cacheProvider)
        {
            _carModelsRepo = carModelsRepo;
            _cacheProvider = cacheProvider;
        }

        public List<CarModelEntityBase> GetCarModelsByType(string type, int makeId, int? year = null, int threeSixtyType = 0)
        {
            string[] cacheKeySuffixArr = new string[_suffixArrCount];
            cacheKeySuffixArr[(int)ModelParameter.Type] = type.ToLower();
            cacheKeySuffixArr[(int)ModelParameter.MakeId] = makeId.ToString();
            cacheKeySuffixArr[(int)ModelParameter.Year] = year.ToString();
            cacheKeySuffixArr[(int)ModelParameter.ThreeSixtyType] = (threeSixtyType == 0) ? null : threeSixtyType.ToString();
            return _cacheProvider.GetFromCache<List<CarModelEntityBase>>(_cacheProvider.GenerateCacheKey(_cacheKeyPrefix["GetCarModelsByType"], cacheKeySuffixArr),
                        CacheRefreshTime.DefaultRefreshTime(), () => _carModelsRepo.GetCarModelsByType(type, makeId, year, threeSixtyType), () => _carModelsRepo.GetCarModelsByType(type, makeId, year, threeSixtyType, true));
        }
        /// <summary>
        /// Gets the list of Car Models based on makeId passed
        /// Written By : Shalini on 07/07/14
        /// </summary>
        /// <param name="makeId"></param>
        /// <returns></returns>
        public List<CarModelSummary> GetModelsByMake(int makeId, int dealerId = 0)
        {
            string[] cacheKeySuffixArr = new string[_suffixArrCount];
            cacheKeySuffixArr[(int)ModelParameter.MakeId] = makeId.ToString();
            cacheKeySuffixArr[(int)ModelParameter.DealerId] = (dealerId == 0) ? null : dealerId.ToString();
            return _cacheProvider.GetFromCache<List<CarModelSummary>>(_cacheProvider.GenerateCacheKey(_cacheKeyPrefix["GetModelsByMake"], cacheKeySuffixArr),
                        CacheRefreshTime.DefaultRefreshTime(), () => _carModelsRepo.GetModelsByMake(makeId), () => _carModelsRepo.GetModelsByMake(makeId, true));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public List<CarMakeModelEntityBase> GetAllModels(string type)
        {
            string[] cacheKeySuffixArr = new string[_suffixArrCount];
            cacheKeySuffixArr[(int)ModelParameter.Type] = type;
            return _cacheProvider.GetFromCache<List<CarMakeModelEntityBase>>(_cacheProvider.GenerateCacheKey(_cacheKeyPrefix["GetAllModels"], cacheKeySuffixArr),
                       CacheRefreshTime.NeverExpire(), () => _carModelsRepo.GetAllModels(type), () => _carModelsRepo.GetAllModels(type, true));
        }

        public List<CarModelSummary> GetAllModelsSummary(int makeId)
        {
            string[] cacheKeySuffixArr = new string[_suffixArrCount];
            cacheKeySuffixArr[(int)ModelParameter.MakeId] = makeId.ToString();
            return _cacheProvider.GetFromCache<List<CarModelSummary>>(_cacheProvider.GenerateCacheKey(_cacheKeyPrefix["GetAllModelsSummary"], cacheKeySuffixArr),
                       CacheRefreshTime.NeverExpire(), () => _carModelsRepo.GetAllModelsSummary(makeId), () => _carModelsRepo.GetAllModelsSummary(makeId, true));
        }
        /// <summary>
        /// Gets the list of upcoming car models based on makeId passed 
        /// Written By : Shalini on 11/07/14
        /// </summary>
        /// <param name="makeId"></param>
        /// <returns></returns>
        public List<UpcomingCarModel> GetUpcomingCarModelsByMake(int makeId, int topCount)
        {
            string[] cacheKeySuffixArr = new string[_suffixArrCount];
            cacheKeySuffixArr[(int)ModelParameter.MakeId] = makeId.ToString();
            cacheKeySuffixArr[(int)ModelParameter.Count] = topCount.ToString();
            return _cacheProvider.GetFromCache<List<UpcomingCarModel>>(_cacheProvider.GenerateCacheKey(_cacheKeyPrefix["GetUpcomingCarModelsByMake"], cacheKeySuffixArr),
                CacheRefreshTime.DefaultRefreshTime(), () => _carModelsRepo.GetUpcomingCarModelsByMake(makeId, topCount), () => _carModelsRepo.GetUpcomingCarModelsByMake(makeId, topCount, true));
        }

        /// <summary>
        /// Gets the list of model colors based on modelId passed 
        /// Written By : Shalini on 11/07/14
        /// </summary>
        /// <param name="modelId"></param>
        /// <returns></returns>
        public List<ModelColors> GetModelColorsByModel(int modelId)
        {
            string[] cacheKeySuffixArr = new string[_suffixArrCount];
            cacheKeySuffixArr[(int)ModelParameter.Id] = modelId.ToString();
            return _cacheProvider.GetFromCache<List<ModelColors>>(_cacheProvider.GenerateCacheKey(_cacheKeyPrefix["GetModelColorsByModel"], cacheKeySuffixArr),
                CacheRefreshTime.NeverExpire(), () => _carModelsRepo.GetModelColorsByModel(modelId), () => _carModelsRepo.GetModelColorsByModel(modelId, true));
        }

        /// <summary>
        /// Written By : Shalini on 27/08/14
        /// Returns the List of Car ModelDetails from cache,if present or from Database
        /// </summary>
        /// <param name="modelId"></param>
        /// <returns></returns>
        public CarModelDetails GetModelDetailsById(int modelId)
        {
            string[] cacheKeySuffixArr = new string[_suffixArrCount];
            cacheKeySuffixArr[(int)ModelParameter.Id] = modelId.ToString();
            return _cacheProvider.GetFromCache<CarModelDetails>(_cacheProvider.GenerateCacheKey(_cacheKeyPrefix["GetModelDetailsById"], cacheKeySuffixArr),
                CacheRefreshTime.NeverExpire(), () => _carModelsRepo.GetModelDetailsById(modelId), () => _carModelsRepo.GetModelDetailsById(modelId, true));
        }

        /// <summary>
        /// Written By : Shalini on 16/09/14
        /// Returns the List of Price in other cities from cache,if present or from Database 
        /// </summary>
        /// <param name="modelId"></param>
        /// <param name="cityId"></param>
        /// <returns>List of Price in other cities</returns>
        public List<ModelPriceInOtherCities> GetPricesInOtherCities(int modelId, int cityId)
        {
            string[] cacheKeySuffixArr = new string[_suffixArrCount];
            cacheKeySuffixArr[(int)ModelParameter.Id] = modelId.ToString();
            cacheKeySuffixArr[(int)ModelParameter.CityId] = cityId.ToString();
            return _cacheProvider.GetFromCache<List<ModelPriceInOtherCities>>(_cacheProvider.GenerateCacheKey(_cacheKeyPrefix["GetPricesInOtherCities"], cacheKeySuffixArr),
                 CacheRefreshTime.DefaultRefreshTime(), () => _carModelsRepo.GetPricesInOtherCities(modelId, cityId), () => _carModelsRepo.GetPricesInOtherCities(modelId, cityId, true));
        }

        public List<ModelPriceInOtherCities> GetPricesForNearByCities(int modelId, int cityId)
        {
            string[] cacheKeySuffixArr = new string[_suffixArrCount];
            cacheKeySuffixArr[(int)ModelParameter.Id] = modelId.ToString();
            cacheKeySuffixArr[(int)ModelParameter.CityId] = cityId.ToString();
            return _cacheProvider.GetFromCache<List<ModelPriceInOtherCities>>(_cacheProvider.GenerateCacheKey(_cacheKeyPrefix["GetPricesForNearByCities"], cacheKeySuffixArr));
        }

        public void SetPricesForNearByCities(int modelId, int cityId, List<ModelPriceInOtherCities> prices)
        {
            string[] cacheKeySuffixArr = new string[_suffixArrCount];
            cacheKeySuffixArr[(int)ModelParameter.Id] = modelId.ToString();
            cacheKeySuffixArr[(int)ModelParameter.CityId] = cityId.ToString();
            _cacheProvider.StoreToCache<List<ModelPriceInOtherCities>>(_cacheProvider.GenerateCacheKey(_cacheKeyPrefix["GetPricesForNearByCities"], cacheKeySuffixArr),
                 CacheRefreshTime.DefaultRefreshTime(), prices);
        }

        /// <summary>
        /// Returns the Upcoming CarDetail from cache, if present or from database
        /// Written By : Shalini on 29/09/14
        /// </summary>
        /// <param name="modelId"></param>
        /// <returns></returns>
        public UpcomingCarModel GetUpcomingCarDetails(int modelId)
        {
            string[] cacheKeySuffixArr = new string[_suffixArrCount];
            cacheKeySuffixArr[(int)ModelParameter.Id] = modelId.ToString();
            return _cacheProvider.GetFromCache<UpcomingCarModel>(_cacheProvider.GenerateCacheKey(_cacheKeyPrefix["GetUpcomingCarDetails"], cacheKeySuffixArr),
                CacheRefreshTime.NeverExpire(), () => _carModelsRepo.GetUpcomingCarDetails(modelId), () => _carModelsRepo.GetUpcomingCarDetails(modelId, true));
        }

        #region SEO details
        /// <summary>
        /// Returns the ModelPageMetaTags from cache,if present or from database
        /// Written By : Shalini on 30/09/14
        /// </summary>
        /// <param name="modelId"></param>
        /// <param name="pageId"></param>
        /// <returns></returns>
        public PageMetaTags GetModelPageMetaTags(int modelId, int pageId)
        {
            string[] cacheKeySuffixArr = new string[_suffixArrCount];
            cacheKeySuffixArr[(int)ModelParameter.Id] = modelId.ToString();
            string key = string.Format("{0}_page_{1}", _cacheProvider.GenerateCacheKey(_cacheKeyPrefix["GetModelPageMetaTags"], cacheKeySuffixArr), pageId);
            return _cacheProvider.GetFromCache<PageMetaTags>(key,
               CacheRefreshTime.DefaultRefreshTime(), () => _carModelsRepo.GetModelPageMetaTags(modelId, pageId), () => _carModelsRepo.GetModelPageMetaTags(modelId, pageId, true));
        }

        #region FRQ

        public List<TopSellingCarModel> GetTopSellingModels(int topCount)
        {
            string[] cacheKeySuffixArr = new string[_suffixArrCount];
            cacheKeySuffixArr[(int)ModelParameter.Count] = topCount.ToString();
            return _cacheProvider.GetFromCache<List<TopSellingCarModel>>(_cacheProvider.GenerateCacheKey(_cacheKeyPrefix["GetTopSellingModels"], cacheKeySuffixArr),
               CacheRefreshTime.NeverExpire(), () => _carModelsRepo.GetTopSellingCarModels(topCount), () => _carModelsRepo.GetTopSellingCarModels(topCount, true));
        }

        //call method from repository -- TopNewLaunches
        public List<LaunchedCarModel> GetLaunchedCarModels(int topCount)
        {
            string[] cacheKeySuffixArr = new string[_suffixArrCount];
            cacheKeySuffixArr[(int)ModelParameter.Count] = topCount.ToString();
            return _cacheProvider.GetFromCache<List<LaunchedCarModel>>(_cacheProvider.GenerateCacheKey(_cacheKeyPrefix["GetLaunchedCarModels"], cacheKeySuffixArr),
              CacheRefreshTime.NeverExpire(), () => _carModelsRepo.GetLaunchedCarModels(topCount), () => _carModelsRepo.GetLaunchedCarModels(topCount, true));
        }

        //call method from repository -- TopNewLaunches
        public List<LaunchedCarModel> GetLaunchedCarModelsV1()
        {
            string[] cacheKeySuffixArr = new string[_suffixArrCount];
            return _cacheProvider.GetFromCache<List<LaunchedCarModel>>(_cacheProvider.GenerateCacheKey(_cacheKeyPrefix["GetLaunchedCarModelsV1"], cacheKeySuffixArr),
                 CacheRefreshTime.NeverExpire(), () => _carModelsRepo.GetLaunchedCarModelsV1(), () => _carModelsRepo.GetLaunchedCarModelsV1(true));
        }

        #endregion

        /// <summary>
        /// Author:Chetan Thambad on <17/03/2016>
        /// Desc: Get all model details with or without bodystyle 
        /// </summary>
        public List<CarModelDetails> GetModelsByBodyStyle(int bodyStyle, bool similarBodyStyle)
        {
            string[] cacheKeySuffixArr = new string[_suffixArrCount];
            cacheKeySuffixArr[(int)ModelParameter.BodyStyleId] = bodyStyle.ToString();
            cacheKeySuffixArr[(int)ModelParameter.IsSimilarBodyStyle] = similarBodyStyle.ToString().ToLower();
            return _cacheProvider.GetFromCache<List<CarModelDetails>>(_cacheProvider.GenerateCacheKey(_cacheKeyPrefix["GetModelsByBodyStyle"], cacheKeySuffixArr),
                CacheRefreshTime.DefaultRefreshTime(), () => _carModelsRepo.GetModelsByBodyStyle(bodyStyle, similarBodyStyle), () => _carModelsRepo.GetModelsByBodyStyle(bodyStyle, similarBodyStyle, true));
        }

        /// <summary>
        /// Written By : Supreksha on <16/2/2017>
        /// Get top models based on popularity 
        /// </summary>
        public List<CarMakeModelAdEntityBase> GetTrendingModels(int count)
        {
            string[] cacheKeySuffixArr = new string[_suffixArrCount];
            cacheKeySuffixArr[(int)ModelParameter.Count] = count.ToString();
            return _cacheProvider.GetFromCache<List<CarMakeModelAdEntityBase>>(_cacheProvider.GenerateCacheKey(_cacheKeyPrefix["GetTrendingModels"], cacheKeySuffixArr),
                CacheRefreshTime.DefaultRefreshTime(), () => _carModelsRepo.GetTrendingModels(count), () => _carModelsRepo.GetTrendingModels(count, true));
        }

        /// <!-- written by Meet Shah on 19 July 2017-->
        /// <summary>
        /// This method returns a dictionary that contains mapping of 
        /// modelid to its rank in its specific body type. 
        /// </summary>
        /// <param name="bodytypes">Comma separated string of bodytypes 
        /// for which ranks are required.</param>
        /// <param name="count">Maximum count in each
        /// body type is defined by the count parameter which defaults to 10</param>
        /// <returns>Dictionary<CarBodyStyle, Tuple<int[], string>></returns>
        public Dictionary<CarBodyStyle, Tuple<int[], string>> GetCarRanksByBodyType(string bodytypes, int count)
        {
            string[] cacheKeySuffixArr = new string[_suffixArrCount];
            cacheKeySuffixArr[(int)ModelParameter.BodyTypes] = bodytypes;
            cacheKeySuffixArr[(int)ModelParameter.Count] = count.ToString();
            return _cacheProvider.GetFromCache(_cacheProvider.GenerateCacheKey(_cacheKeyPrefix["GetCarRanksByBodyType"], cacheKeySuffixArr),
                CacheRefreshTime.DefaultRefreshTime(), () => _carModelsRepo.GetCarRanksByBodyType(bodytypes, count), () => _carModelsRepo.GetCarRanksByBodyType(bodytypes, count, true));
        }

        /// <!-- written by Meet Shah and Ashutosh Udeniya on 6 Sep 2017-->
        /// <summary>
        /// This method returns a similar upcoming car for a given model
        /// </summary>
        public UpcomingCarModel GetSimilarUpcomingCarModel(int modelId)
        {
            string[] cacheKeySuffixArr = new string[_suffixArrCount];
            cacheKeySuffixArr[(int)ModelParameter.Id] = modelId.ToString();
            return _cacheProvider.GetFromCache(_cacheProvider.GenerateCacheKey(_cacheKeyPrefix["GetSimilarUpcomingCarModel"], cacheKeySuffixArr),
                CacheRefreshTime.NeverExpire(), () => _carModelsRepo.GetSimilarUpcomingCarModel(modelId), () => _carModelsRepo.GetSimilarUpcomingCarModel(modelId, true));
        }

        public List<CarModelDetails> MultiGetModelDetails(IEnumerable<int> modelIds)
        {
            List<CarModelDetails> carDetails = new List<CarModelDetails>();
            foreach (int modelId in modelIds)
            {
                string[] cacheKeySuffixArr = new string[_suffixArrCount];
                cacheKeySuffixArr[(int)ModelParameter.Id] = modelId.ToString();
                carDetails.Add(GetModelDetailsById(modelId));
            }
            return carDetails;
        }

        /// <summary>
        /// Returns the Upgraded Upcoming CarDetail from cache, if present or from database
        /// Written By : Meet Shah on 15/09/2017
        /// </summary>
        /// <param name="modelId"></param>
        /// <returns></returns>
        public UpcomingCarModel GetUpgradedModel(int modelId)
        {
            string[] cacheKeySuffixArr = new string[_suffixArrCount];
            cacheKeySuffixArr[(int)ModelParameter.Id] = modelId.ToString();
            return _cacheProvider.GetFromCache<UpcomingCarModel>(_cacheProvider.GenerateCacheKey(_cacheKeyPrefix["GetUpgradedModel"], cacheKeySuffixArr),
                CacheRefreshTime.NeverExpire(), () => _carModelsRepo.GetUpgradedModel(modelId), () => _carModelsRepo.GetUpgradedModel(modelId, true));
        }
        public CarModelMaskingResponse GetModelByMaskingName(string maskingName)
        {
            string[] cacheKeySuffixArr = new string[_suffixArrCount];
            cacheKeySuffixArr[(int)ModelParameter.MaskingName] = maskingName;
            return _cacheProvider.GetFromCache<CarModelMaskingResponse>(_cacheProvider.GenerateCacheKey(_cacheKeyPrefix["GetModelByMaskingName"], cacheKeySuffixArr),
                CacheRefreshTime.NeverExpire(), () => _carModelsRepo.GetModelByMaskingName(maskingName), () => _carModelsRepo.GetModelByMaskingName(maskingName, true)) ?? new CarModelMaskingResponse();

        }

        public List<ModelSummary> GetActiveModelsByMake(int makeId)
        {
            string[] cacheKeySuffixArr = new string[_suffixArrCount];
            cacheKeySuffixArr[(int)ModelParameter.Id] = makeId.ToString();
            return _cacheProvider.GetFromCache<List<ModelSummary>>(_cacheProvider.GenerateCacheKey(_cacheKeyPrefix["GetActiveModelsByMake"], cacheKeySuffixArr));
        }
        public void SetActiveModelsByMakeInCache(int makeId, List<ModelSummary> modelList)
        {
            string[] cacheKeySuffixArr = new string[_suffixArrCount];
            cacheKeySuffixArr[(int)ModelParameter.MakeId] = makeId.ToString();
            _cacheProvider.StoreToCache<List<ModelSummary>>(_cacheProvider.GenerateCacheKey(_cacheKeyPrefix["GetActiveModelsByMake"], cacheKeySuffixArr),
                CacheRefreshTime.NeverExpire(), modelList);
        }
        public bool RefreshCarModelCache(List<ModelAttribute> modelAttributes)
        {
            try
            {
                bool isRefreshed = true;
                string[] suffixArr = new string[_suffixArrCount];
                foreach (var model in modelAttributes)
                {
                    suffixArr[(int)ModelParameter.Id] = model.Id;
                    suffixArr[(int)ModelParameter.Type] = model.Type;
                    suffixArr[(int)ModelParameter.MakeId] = model.MakeId;
                    suffixArr[(int)ModelParameter.Year] = model.Year;
                    suffixArr[(int)ModelParameter.ThreeSixtyType] = model.ThreeSixtyType;
                    suffixArr[(int)ModelParameter.DealerId] = model.DealerId;
                    suffixArr[(int)ModelParameter.MaskingName] = model.MaskingName;
                    suffixArr[(int)ModelParameter.CityId] = model.CityId;
                    suffixArr[(int)ModelParameter.Count] = model.Count;
                    suffixArr[(int)ModelParameter.BodyStyleId] = model.BodyStyleId;
                    suffixArr[(int)ModelParameter.IsSimilarBodyStyle] = model.IsSimilarBodyStyle;
                    suffixArr[(int)ModelParameter.BodyTypes] = model.BodyTypes;
                    foreach (var key in CarModelsCacheRepository._cacheKeyPrefix.Values)
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

#endregion
