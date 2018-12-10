using System.Collections.Generic;
using Newtonsoft.Json;
using Carwale.DTOs.NewCars;
using Carwale.DTOs.Classified.PopularUC;
using Carwale.DTOs.CMS.Articles;

namespace Carwale.DTOs
{
	public class CarHomeV3
	{
		[JsonProperty("topSellingModels")]
		public List<TopSellingCarModelV2> TopSellingModels { get; set; }
		[JsonProperty("recentLaunches")]
		public List<LaunchedCarModelV2> RecentLaunches { get; set; }
		[JsonProperty("upcomingModels")]
		public List<UpcomingModelV2> UpcomingModels { get; set; }
		[JsonProperty("videos")]
		public List<YouTubeVideoV2> Videos { get; set; }
		[JsonProperty("expertReviews")]
		public List<ArticleSummaryDTOV2> ExpertReviews { get; set; }
		[JsonProperty("news")]
		public List<ArticleSummaryDTOV2> News { get; set; }
		[JsonProperty("insuranceClientId")]
		public int InsuranceClientId { get; set; }
		[JsonProperty("showInsurance")]
		public bool ShowInsurance { get; set; }
		[JsonProperty("orpText")]
		public string OrpText { get; set; }
        [JsonProperty("popularUsedCars")]
        public List<PopularUCModelAppV2> PopularUsedCar { get; set; }
	}
}
