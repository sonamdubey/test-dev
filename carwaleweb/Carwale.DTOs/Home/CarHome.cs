using Carwale.Entity.CarData;
using System.Collections.Generic;
using Newtonsoft.Json;
using Carwale.DTOs.NewCars;
using Carwale.DTOs.Classified.PopularUC;
using Carwale.DTOs.CarData;

namespace Carwale.DTOs
{
    public class CarHome
    {
        [JsonProperty("topSellingModels")]
        public List<Carwale.DTOs.NewCars.TopSellingCarModel> TopSellingModels { get; set; }
        [JsonProperty("recentLaunches")]
        public List<Carwale.DTOs.NewCars.LaunchedCarModel> RecentLaunches { get; set; }
        [JsonProperty("upcomingModels")]
        public List<UpcomingModel> UpcomingModels { get; set; }
        [JsonProperty("videos")]
        public List<YouTubeVideo> Videos { get; set; }
        [JsonProperty("expertReviews")]
        public List<Article> ExpertReviews { get; set; }
        [JsonProperty("news")]
        public List<Article> News { get; set; }
        [JsonProperty("popularUsedCars")]
        public List<PopularUCModelApp> PopularUsedCar { get; set; }
        [JsonProperty("response")]
        public AppResponse Response { get; set; }
        [JsonProperty("insuranceClientId")]
        public int InsuranceClientId { get; set; }
        [JsonProperty("showInsurance")]
        public bool ShowInsurance { get; set; }
    }

    public class Article
    {
        [JsonProperty("image")]
        public CarImageBase Image { get; set; }
        [JsonProperty("desc")]
        public string Description { get; set; }
        [JsonProperty("author")]
        public Author Author { get; set; }
        [JsonProperty("publishedDate")]
        public string PublishedDate { get; set; }
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("url")]
        public string Url { get; set; }
        [JsonProperty("basicId")]
        public ulong BasicId { get; set; }
    }


    public class ArticleV2
    {
        [JsonProperty("image")]
        public CarImageBaseDTO Image { get; set; }
        [JsonProperty("desc")]
        public string Description { get; set; }
        [JsonProperty("author")]
        public Author Author { get; set; }
        [JsonProperty("publishedDate")]
        public string PublishedDate { get; set; }
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("url")]
        public string Url { get; set; }
        [JsonProperty("basicId")]
        public ulong BasicId { get; set; }
    }
    public class Author
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("maskingName")]
        public string MaskingName { get; set; }
    }

    public class YouTubeVideo
    {
        [JsonProperty("image")]
        public CarImageBase Image { get; set; }
        [JsonProperty("url")]
        public string Url { get; set; }
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("publishedDate")]
        public string PublishedDate { get; set; }
        [JsonProperty("views")]
        public int Views { get; set; }
        [JsonProperty("likes")]
        public int Likes { get; set; }
        [JsonProperty("videoId")]
        public string VideoId { get; set; }
    }

    public class YouTubeVideoV2
    {
        [JsonProperty("hostUrl")]
        public string HostUrl { get; set; }
        [JsonProperty("originalImgPath")]
        public string OriginalImgPath { get; set; }
        [JsonProperty("url")]
        public string Url { get; set; }
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("publishedDate")]
        public string PublishedDate { get; set; }
        [JsonProperty("views")]
        public int Views { get; set; }
        [JsonProperty("likes")]
        public int Likes { get; set; }
        [JsonProperty("videoId")]
        public string VideoId { get; set; }
    }

    public class City
    {
        [JsonProperty("cityId")]
        public int CityId { get; set; }
        [JsonProperty("cityName")]
        public string CityName { get; set; }
    }
}
