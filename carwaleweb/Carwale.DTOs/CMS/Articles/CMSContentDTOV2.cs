using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.DTOs.CMS.Articles
{
    public class CMSContentDTOV2
    {
        public IList<ArticleSummaryDTOV3> Articles { get; set; }
        public uint RecordCount { get; set; }
        public string NextPageUrl { get; set; }
        [JsonProperty("newsRecordCount")]
        public int NewsRecordCount { get; set; }
        [JsonProperty("expertReviewsRecordCount")]
        public int ExpertReviewsRecordCount { get; set; }
        [JsonProperty("featuresRecordCount")]
        public int FeaturesRecordCount { get; set; }
    }

    public class CMSContentDTOV3
    {
        [JsonProperty("articles")]
        public List<ArticleSummaryDTOV4> Articles { get; set; }
        [JsonProperty("recordCount")]
        public uint RecordCount { get; set; }
        [JsonProperty("nextPageUrl")]
        public string NextPageUrl { get; set; }
        [JsonProperty("newsRecordCount")]
        public int NewsRecordCount { get; set; }
        [JsonProperty("expertReviewsRecordCount")]
        public int ExpertReviewsRecordCount { get; set; }
        [JsonProperty("featuresRecordCount")]
        public int FeaturesRecordCount { get; set; }
    }
}

