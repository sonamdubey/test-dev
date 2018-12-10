using Carwale.Entity.CarData;
using System;
using System.Collections.Generic;
using Carwale.Entity.Classification;
using Carwale.Entity;
using Carwale.DTOs.CarData;
using Newtonsoft.Json;
using Carwale.DTOs.PriceQuote;
using Carwale.DTOs.Classification;

namespace Carwale.DTOs.NewCars
{
    [Serializable]
    public class TopSellingCarModel
    {
        [JsonProperty("review")]
        public CarReviewBase Review { get; set; }
        [JsonProperty("price")]
        public string Price { get; set; }
        [JsonProperty("image")]
        public CarImageBase Image { get; set; }
        [JsonProperty("make")]
        public MakeEntity Make { get; set; }
        [JsonProperty("model")]
        public ModelEntity Model { get; set; }
        [JsonProperty("city")]
        public City City { get; set; }
    }

    public class TopSellingCarModelV2
    {
        [JsonProperty("makeId")]
        public int MakeId { get; set; }
        [JsonProperty("makeName")]
        public string MakeName { get; set; }

        [JsonProperty("modelId")]
        public int ModelId { get; set; }
        [JsonProperty("modelName")]
        public string ModelName { get; set; }
        [JsonProperty("maskingName")]
        public string MaskingName { get; set; }

        [JsonProperty("overallRating")]
        public float OverallRating { get; set; }
        [JsonProperty("reviewCount")]
        public ushort ReviewCount { get; set; }

        [JsonProperty("hostUrl")]
        public string HostUrl { get; set; }
        [JsonProperty("originalImgPath")]
        public string OriginalImgPath { get; set; }

        [JsonProperty("priceOverview")]
        public PriceOverviewDTO Price { get; set; }
    }

    [Serializable]
    public class LaunchedCarModel
    {
        [JsonProperty("make")]
        public MakeEntity Make { get; set; }
        [JsonProperty("model")]
        public ModelEntity Model { get; set; }
        [JsonProperty("launchedDate")]
        public string LaunchedDate { get; set; }
        [JsonProperty("price")]
        public string Price { get; set; }
        [JsonProperty("image")]
        public CarImageBase Image { get; set; }
        [JsonProperty("review")]
        public CarReviewBase Review { get; set; }
        [JsonProperty("city")]
        public City City { get; set; }

        [JsonIgnore]
        public VersionEntity Version { get; set; }
    }

    public class LaunchedCarModelV2
    {

        [JsonProperty("makeId")]
        public int MakeId { get; set; }
        [JsonProperty("makeName")]
        public string MakeName { get; set; }

        [JsonProperty("modelId")]
        public int ModelId { get; set; }
        [JsonProperty("modelName")]
        public string ModelName { get; set; }
        [JsonProperty("maskingName")]
        public string MaskingName { get; set; }

        [JsonProperty("launchedDate")]
        public string LaunchedDate { get; set; }

        [JsonProperty("overallRating")]
        public float OverallRating { get; set; }
        [JsonProperty("reviewCount")]
        public ushort ReviewCount { get; set; }

        [JsonProperty("hostUrl")]
        public string HostUrl { get; set; }
        [JsonProperty("originalImgPath")]
        public string OriginalImgPath { get; set; }

        [JsonProperty("priceOverview")]
        public PriceOverviewDTO Price { get; set; }

        [JsonIgnore]
        public VersionEntity Version { get; set; }

    }


    [Serializable]
    public class UpcomingModel
    {
        [JsonProperty("make")]
        public MakeEntity Make { get; set; }
        [JsonProperty("model")]
        public ModelEntity Model { get; set; }
        [JsonProperty("expectedLaunch")]
        public string ExpectedLaunch { get; set; }
        [JsonProperty("price")]
        public string Price { get; set; }
        [JsonProperty("image")]
        public CarImageBase Image { get; set; }
        [JsonProperty("expectedLaunchId")]
        public int ExpectedLaunchId { get; set; }
    }

    public class UpcomingModelV2
    {
        [JsonProperty("makeName")]
        public string MakeName { get; set; }
        [JsonProperty("makeId")]
        public string MakeId { get; set; }
        [JsonProperty("modelName")]
        public string ModelName { get; set; }
        [JsonProperty("modelId")]
        public int ModelId { get; set; }
        [JsonProperty("expectedLaunch")]
        public string ExpectedLaunch { get; set; }
        [JsonProperty("launchLabel")]
        public string LaunchLabel { get; set; }
        [JsonProperty("priceOverview")]
        public PriceOverviewDTO Price { get; set; }

        [JsonProperty("hostUrl")]
        public string HostUrl { get; set; }
        [JsonProperty("originalImgPath")]
        public string OriginalImgPath { get; set; }

        [JsonProperty("expectedLaunchId")]
        public int ExpectedLaunchId { get; set; }
    }

    [Serializable]
    public class HotCarComparison
    {
        [JsonProperty("hotCars")]
        public List<ComparisonCarModel> HotCars { get; set; }
        [JsonProperty("image")]
        public CarImageBase Image { get; set; }
    }

    public class HotCarComparisonV2
    {
        [JsonProperty("hotCars")]
        public List<ComparisonCarModelV2> HotCars { get; set; }
        [JsonProperty("image")]
        public CarImageBaseDTO Image { get; set; }
        [JsonProperty("isSponsored")]
        public bool IsSponsored { get; set; }
    }

    public class ComparisonCarModelV2
    {


        [JsonProperty("makeId")]
        public int MakeId { get; set; }
        [JsonProperty("makeName")]
        public string MakeName { get; set; }
        [JsonProperty("modelId")]
        public int ModelId { get; set; }
        [JsonProperty("modelName")]
        public string ModelName { get; set; }
        [JsonProperty("maskingName")]
        public string MaskingName { get; set; }
        [JsonProperty("overallRating")]
        public float OverallRating { get; set; }
        [JsonProperty("reviewCount")]
        public ushort ReviewCount { get; set; }
        [JsonProperty("versionId")]
        public int VersionId { get; set; }
        [JsonProperty("priceOverview")]
        public PriceOverviewDTO PriceOverview { get; set; }

    }

    [Serializable]
    public class ComparisonCarModel
    {
        [JsonProperty("make")]
        public MakeEntity Make { get; set; }
        [JsonProperty("model")]
        public ModelEntity Model { get; set; }
        [JsonProperty("review")]
        public CarReviewBase Review { get; set; }
        [JsonProperty("versionId")]
        public int VersionId { get; set; }
        [JsonProperty("price")]
        public string Price { get; set; }
    }

    [Serializable]
    public class CarMakeLogo
    {
        [JsonProperty("image")]
        public CarImageBase Image { get; set; }
        [JsonProperty("make")]
        public MakeEntity Make { get; set; }
    }

    public class CarMakeLogoV2
    {
        [JsonProperty("makeId")]
        public int MakeId { get; set; }
        [JsonProperty("makeName")]
        public string MakeName { get; set; }
        [JsonProperty("hostUrl")]
        public string HostURL { get; set; }
        [JsonProperty("imagePath")]
        public string OriginalImgPath { get; set; }
    }

    public class NewCarHome
    {
        [JsonProperty("topSellingModels")]
        public List<TopSellingCarModel> TopSellingModels { get; set; }
        [JsonProperty("recentLaunches")]
        public List<LaunchedCarModel> RecentLaunches { get; set; }
        [JsonProperty("upcomingModels")]
        public List<UpcomingModel> UpcomingModels { get; set; }
        [JsonProperty("bodyType")]
        public List<BodyType> BodyType { get; set; }
        [JsonProperty("carMakeLogo")]
        public List<CarMakeLogo> Makes { get; set; }
        [JsonProperty("topCarCompare")]
        public List<HotCarComparison> TopCarCompare { get; set; }
    }


    public class NewCarHomeV2
    {
        [JsonProperty("topSellingModels")]
        public List<TopSellingCarModelV2> TopSellingModels { get; set; }
        [JsonProperty("recentLaunches")]
        public List<LaunchedCarModelV2> RecentLaunches { get; set; }
        [JsonProperty("upcomingModels")]
        public List<UpcomingModelV2> UpcomingModels { get; set; }
        [JsonProperty("bodyType")]
        public List<BodyTypeDTO> BodyType { get; set; }
        [JsonProperty("carMakeLogo")]
        public List<CarMakeLogoV2> Makes { get; set; }
        [JsonProperty("topCarCompare")]
        public List<HotCarComparisonV2> TopCarCompare { get; set; }
    }
    public class NewCarHomeV3
    {
        [JsonProperty("topSellingModels")]
        public List<TopSellingCarModelV2> TopSellingModels { get; set; }
        [JsonProperty("recentLaunches")]
        public List<LaunchedCarModelV2> RecentLaunches { get; set; }
        [JsonProperty("upcomingModels")]
        public List<UpcomingModelV2> UpcomingModels { get; set; }
        [JsonProperty("bodyType")]
        public List<BodyTypeDTO> BodyType { get; set; }
        [JsonProperty("carMakeLogo")]
        public List<CarMakeLogoV2> Makes { get; set; }
        [JsonProperty("topCarCompare")]
        public List<HotCarComparisonV2> TopCarCompare { get; set; }
        [JsonProperty("orpText")]
        public string OrpText { get; set; }
    }
}
