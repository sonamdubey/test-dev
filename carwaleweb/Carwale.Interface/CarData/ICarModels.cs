using Carwale.DTOs.CarData;
using Carwale.Entity;
using Carwale.Entity.CarData;
using Carwale.Entity.CMS.Articles;
using Carwale.Entity.Enum;
using Carwale.Entity.PriceQuote;
using Carwale.Entity.ViewModels.CarData;
using Carwale.Interfaces.PriceQuote;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace Carwale.Interfaces.CarData
{
    public interface ICarModels
    {
        List<CarModelSummary> GetNewModelsByMake(int makeId, int dealerId = 0);
        List<CarModelSummary> GetDiscontinuedModelsByMake(int makeId);
        List<SimilarCarModels> GetSimilarCarsByModel(int modelId, string userIdentifier);
        SimilarCarsDTO GetSimilarCarVm(SimilarCarVmRequest similarCarVmRequest);
        List<CarModelSpecs> GetProcessedCarModelSpecs(IEnumerable<int> versionIds, int modelId);
        string Heading(string headingText, string makeName, string modelName);
        string Summary(string summaryText, string makeName, string modelName, int[] processVersionData,
        List<DTOs.NewCars.NewCarVersionsDTO> versions, bool isCityPage, string cityName);
        string Title(string titleText, string makeName, string modelName);
        string GetDescription(string makeName, string modelName, string modelPrice);
        List<TopSellingCarModel> GetTopSellingCarModels(Pagination page, int cityId = -1, bool orp = false);
        List<LaunchedCarModel> GetLaunchedCarModels(Pagination page, int cityId = -1, bool orp = false);
        List<LaunchedCarModel> GetLaunchedCarModelsV1(Pagination page, int cityId = -1, bool orp = false);
        List<UpcomingCarModel> GetUpcomingCarModels(Pagination page, int makeId = 0, int sort = 0);
        List<SimilarModelRecommendation> GetModelRecommendations(int modelId, int cityId, int noOfRecommendation);
        List<SimilarCarModelsDTO> GetSimilarCarsForApp(int modelId, string userIdentifier);
        SubNavigationDTO GetModelQuickMenu(CarModelDetails modelDetailsObj, ArticlePageDetails articlePageDetails, bool isModelPage, bool isExpertReviewAvial, string category, string label);
        List<CarMakeModelAdEntityBase> GetHistoryModelDetails(string modelIds, int platformId);
        TrendingCarDTO GetTrendingModelDetails(int count, int platformId, bool isAmp);
        ModelMaskingValidationEntity FetchModelIdFromMaskingName(string modelMaskingName, string modelId, string makeMaskingName = null, bool isMsite = false);

        List<CarModelSummaryDTO> GetModelDetails(int makeId, int cityId);
        List<ModelPriceInOtherCities> GetPricesInOtherCities(int modelId, int cityId = -1);
        List<UpcomingCarModel> GetSimilarUpcomingCars(int modelId, DateTime launchDate, double expectedPrice, int count = 5);
        SimilarCarModels GetReplacedModelDetails(short replacedModelId, CarModelDetails activeModelDetails, bool isMobile);

		List<SimilarCarModels> GetSimilarCarsWithPriceOverview(int modelId, int cityId, string userIdentifier);
		List<SimilarCarModels> GetSimilarCarsWithORP(int modelId, int cityId, string cwcCookie, int reqCount, string excludingModels = "");
        ModelDataSummary GetProcessedCarModelDataSummary(List<int> versionIds, int modelId);
        JObject CreateJsonLdJObject(ModelSchema modelSchema, List<SimilarCarModelsDTOV2> similarCarModels, bool isMileagePage = false);
    }
}
