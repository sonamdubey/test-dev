using Bikewale.DTO.CMS.Articles;
using Bikewale.DTO.Videos;
using Bikewale.Entities.DTO;
using Newtonsoft.Json;
using System.Collections.Generic;


namespace Bikewale.DTO.Model
{

    /// <summary>
    /// Author : Vivek Gupta on 5-5-2016
    /// Desc : wrapping in a single api 4 apis for android
    /// - User Reviews, - News, - Expert Reviews, - Videos
    /// </summary>
    public class BikeModelContentDTO
    {
        [JsonProperty("reviewDetails")]
        public IEnumerable<Review> ReviewDetails { get; set; }

        [JsonProperty("news")]
        public IEnumerable<CMSArticleSummary> News { get; set; }

        [JsonProperty("expertReviews")]
        public IEnumerable<CMSArticleSummary> ExpertReviews { get; set; }

        [JsonProperty("videos")]
        public IEnumerable<VideoBase> Videos { get; set; }
    }
}
