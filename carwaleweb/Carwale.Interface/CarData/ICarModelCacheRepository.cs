using System;
using System.Collections.Generic;
using Carwale.Entity;
using Carwale.Entity.CarData;
using Carwale.Entity.Enum;
using Carwale.Entity.CentralizedCacheRefresh;

namespace Carwale.Interfaces.CarData
{
    public interface ICarModelCacheRepository
    {
        List<CarMakeModelEntityBase> GetAllModels(string type);
        List<CarModelSummary> GetAllModelsSummary(int makeId);
        List<CarModelEntityBase> GetCarModelsByType(string type, int makeId, int? year = null, int threeSixtyType = 0);
        List<CarModelSummary> GetModelsByMake(int makeId, int dealerId = 0);
        List<UpcomingCarModel> GetUpcomingCarModelsByMake(int makeId, int topCount = 100);
        List<ModelColors> GetModelColorsByModel(int modelId);
        CarModelDetails GetModelDetailsById(int modelId);
        List<ModelPriceInOtherCities> GetPricesInOtherCities(int modelId, int cityId);
        UpcomingCarModel GetUpcomingCarDetails(int modelId);
        PageMetaTags GetModelPageMetaTags(int modelId, int pageId);
        List<LaunchedCarModel> GetLaunchedCarModels(int topCount);
        List<LaunchedCarModel> GetLaunchedCarModelsV1();
        List<TopSellingCarModel> GetTopSellingModels(int topCount);
        List<CarModelDetails> GetModelsByBodyStyle(int bodyStyle, bool similarBodyStyle);
        List<CarMakeModelAdEntityBase> GetTrendingModels(int count);
        Dictionary<CarBodyStyle, Tuple<int[], string>> GetCarRanksByBodyType(string bodytypes, int count);
        UpcomingCarModel GetSimilarUpcomingCarModel(int modelId);
        UpcomingCarModel GetUpgradedModel(int modelId);
        CarModelMaskingResponse GetModelByMaskingName(string maskingName);
        List<CarModelDetails> MultiGetModelDetails(IEnumerable<int> modelIds);
        void SetPricesForNearByCities(int modelId, int cityId, List<ModelPriceInOtherCities> prices);
        List<ModelPriceInOtherCities> GetPricesForNearByCities(int modelId, int cityId);
        List<ModelSummary> GetActiveModelsByMake(int makeId);
        void SetActiveModelsByMakeInCache(int makeId, List<ModelSummary> modelList);
        bool RefreshCarModelCache(List<ModelAttribute> modelAttributes);
    }
}
