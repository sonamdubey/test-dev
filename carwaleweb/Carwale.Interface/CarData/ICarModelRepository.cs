using System;
using System.Collections.Generic;
using Carwale.Entity.CarData;
using Carwale.Entity.Common;
using System.Collections;
using Carwale.Entity.Campaigns;
using System.Data;
using Carwale.Entity.Enum;
using Carwale.Entity;

namespace Carwale.Interfaces
{
    public interface ICarModelRepository
    {
        List<CarMakeModelEntityBase> GetAllModels(string type,bool isCriticalRead = false);
        List<CarModelSummary> GetAllModelsSummary(int makeId, bool isCriticalRead = false);
        List<CarModelEntityBase> GetCarModelsByType(string type, int? makeId, int? year, int threeSixtyType, bool isCriticalRead = false);
        List<ValuationModel> GetValuationModels(int Year, int MakeId, bool isCriticalRead = false);
        List<CarModelSummary> GetModelsByMake(int makeId, bool isCriticalRead = false);
        List<UpcomingCarModel> GetUpcomingCarModelsByMake(int makeId, int count = 100, bool isCriticalRead = false);
        List<ModelColors> GetModelColorsByModel(int modelId, bool isCriticalRead = false);
        CarModelDetails GetModelDetailsById(int modelId, bool isCriticalRead = false);
        List<ModelPriceInOtherCities> GetPricesInOtherCities(int modelId, int cityId, bool isCriticalRead = false);
        UpcomingCarModel GetUpcomingCarDetails(int modelId, bool isCriticalRead = false);
        PageMetaTags GetModelPageMetaTags(int modelId, int pageId, bool isCriticalRead = false);
        List<CarModelDetails> GetNewCarSearchResult(CarModelURI uri, bool isCriticalRead = false);
        List<LaunchedCarModel> GetLaunchedCarModels(int topCount, bool isCriticalRead = false);
        List<LaunchedCarModel> GetLaunchedCarModelsV1(bool isCriticalRead = false);
        List<TopSellingCarModel> GetTopSellingCarModels(int topCount, bool isCriticalRead = false);
        List<CampaignInput> GetUserRecentLeadModels(string mobileNo, bool isCriticalRead = false);
        List<CarModelDetails> GetModelsByBodyStyle(int bodyStyle, bool similarBodyStyle, bool isCriticalRead = false);
        List<ModelDetails> GetModelSpecs(bool isCriticalRead = false);
        DataSet GetUpcomingCarsForOldVersionApp(int id, int makeId, bool isCriticalRead = false);
        List<CarMakeModelAdEntityBase> GetTrendingModels(int count, bool isCriticalRead = false);
        Dictionary<CarBodyStyle, Tuple<int[], string>> GetCarRanksByBodyType(string bodytypes, int count, bool isCriticalRead = false);
        UpcomingCarModel GetSimilarUpcomingCarModel(int modelId, bool isCriticalRead = false);
        UpcomingCarModel GetUpgradedModel(int modelId, bool isCriticalRead = false);
        CarModelMaskingResponse GetModelByMaskingName(string maskingName, bool isCriticalRead = false);
        List<ModelSummary> GetActiveModelsByMake(int makeId, bool isCriticalRead = false);
        string GetExpectedLaunchDate(DateTime launchDate, bool isHighConfidence);
    }
}
