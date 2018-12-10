using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.DTOs.CMS.Articles
{
    /// <summary>
    /// written by Natesh kumar on 25/9/14
    /// </summary>
    public class CMSContent
    {
        public IList<ArticleSummary> Articles { get; set; }
        public uint RecordCount { get; set; }
        public string NextPageUrl { get; set; }
        [JsonProperty("newsRecordCount")]
        public int NewsRecordCount { get; set; }
        [JsonProperty("expertReviewsRecordCount")]
        public int ExpertReviewsRecordCount { get; set; }
        [JsonProperty("featuresRecordCount")]
        public int FeaturesRecordCount { get; set; }
    }
}
