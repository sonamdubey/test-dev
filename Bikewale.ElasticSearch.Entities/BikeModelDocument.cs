using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.ElasticSearch.Entities
{
    /// <summary>
    /// Created by: Dhruv Joshi
    /// Dated: 20th Feb 2018
    /// Description: Document for a bike model
    /// </summary>
    public class BikeModelDocument: WeightedDocument
    {
        [JsonProperty("bikeMake")]
        public MakeEntity BikeMake { get; set; }
        [JsonProperty("bikeModel")]
        public ModelEntity BikeModel { get; set; }
        [JsonProperty("topVersion")]
        public VersionEntity TopVersion { get; set; }
        [JsonProperty("bikeName")]
        public string BikeName { get; set; }
        [JsonProperty("bodyStyleId")]
        public uint BodyStyleId { get; set; }
        [JsonProperty("bikeImage")]
        public ImageEntity BikeImage { get; set; }
        [JsonProperty("imageCount")]
        public uint ImageCount { get; set; }
        [JsonProperty("videosCount")]
        public uint VideosCount { get; set; }
        [JsonProperty("expertReviewsCount")]
        public uint ExpertReviewsCount { get; set; }
        [JsonProperty("userReviewsCount")]
        public uint UserReviewsCount { get; set; }
        [JsonProperty("reviewRatings")]
        public double ReviewRatings { get; set; }
        [JsonProperty("ratingsCount")]
        public uint RatingsCount { get; set; }
    }
}
