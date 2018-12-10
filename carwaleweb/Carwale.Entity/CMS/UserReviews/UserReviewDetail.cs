using Newtonsoft.Json;
using System;

namespace Carwale.Entity.CMS
{
    [Serializable]
    public class UserReviewDetail : MakeEntity
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("author")]
        public string Author { get; set; }

        [JsonProperty("reviewDate")]
        public string ReviewDate { get; set; }

        [JsonProperty("reviewRate")]
        public string ReviewRate { get; set; }

        [JsonProperty("goods")]
        public string Pros { get; set; }

        [JsonProperty("bads")]
        public string Cons { get; set; }

        [JsonProperty("comment")]
        public string Comments { get; set; }

        [JsonProperty("purchasedAs ")]
        public string PurchasedAs { get; set; }

        [JsonProperty("fuelEconomy")]
        public string FuelEconomy { get; set; }

        [JsonProperty("familarity")]
        public string Familiarity { get; set; }

        [JsonProperty("shareUrl")]
        public string ShareUrl { get; set; }

        [JsonProperty("makeName")]
        public string Make { get; set; }

        [JsonProperty("modelId")]
        public string ModelId { get; set; }

        [JsonProperty("modelName")]
        public string Model { get; set; }

        [JsonProperty("versionId")]
        public string VersionId { get; set; }
        
        [JsonProperty("versionName")]
        public string Version { get; set; }

        [JsonProperty("startPrice")]
        public string StartPrice { get; set; }

        [JsonProperty("reviewId")]
        public string ReviewId { get; set; }

        [JsonProperty("reviewCommentsUrl")]
        public string ReviewCommentsUrl { get; set; }

        [JsonProperty("commentCount")]
        public string CommentsCount { get; set; }

        [JsonProperty("originalImgPath")]
        public string OriginalImgPath { get; set; }

        [JsonProperty("hostUrl")]
        public string HostUrl { get; set; }

        [JsonProperty("platformId")]
        public int PlatformId { get; set; }

        [JsonIgnore]
        public string MinPrice { get; set; }

        [JsonIgnore]
        public string MaskingName { get; set; }

        [JsonIgnore]
        public string HandleName { get; set; }

        [JsonIgnore]
        public string CustomerName { get; set; }

        [JsonIgnore]
        public string CustomerEmail { get; set; }

        [JsonIgnore]
        public string IsOwned { get; set; }

        [JsonIgnore]
        public string IsNewlyPurchased { get; set; }

        [JsonIgnore]
        public float Mileage { get; set; }

        [JsonIgnore]
        public DateTime EntryDateTime { get; set; }

        [JsonIgnore]
        public float OverallR { get; set; }
        public int StyleR { get; set; }
        public int PerformanceR { get; set; }
        public int ComfortR { get; set; }
        public int FuelEconomyR { get; set; }
        public int ValueR { get; set; }
        public string ReviewQuestions { get; set; }
        public string CustomerId { get; set; }
        public int ImageCount { get; set; }
        public int VideoCount { get; set; }
		public string MakeMaskingName { get; set; }
		public string VersionMaskingName { get; set; }
	}
}
