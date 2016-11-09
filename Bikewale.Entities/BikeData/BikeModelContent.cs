﻿using Bikewale.Entities.CMS.Articles;
using Bikewale.Entities.UserReviews;
using Bikewale.Entities.Videos;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Bikewale.Entities.BikeData
{
    /// <summary>
    /// Modified By:- Subodh Jain 08 Nov 2016
    /// Description:- Added TipsAndAdvices
    /// </summary>
    public class BikeModelContent
    {
        [JsonProperty("reviewDetails")]
        public IEnumerable<ReviewEntity> ReviewDetails { get; set; }

        [JsonProperty("news")]
        public IEnumerable<ArticleSummary> News { get; set; }

        [JsonProperty("expertReviews")]
        public IEnumerable<ArticleSummary> ExpertReviews { get; set; }

        [JsonProperty("videos")]
        public IEnumerable<BikeVideoEntity> Videos { get; set; }

        [JsonProperty("TipsAndAdvices")]
        public IEnumerable<ArticleSummary> TipsAndAdvices { get; set; }
    }
}
