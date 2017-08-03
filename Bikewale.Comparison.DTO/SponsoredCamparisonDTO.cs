using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Comparison.DTO
{
    /// <summary>
    /// Created by: Sangram Nandkhile on 31-Jul-2017
    /// Summary: DTO for Sponsored Camparison
    /// 
    /// </summary>
    public class SponsoredComparisonDTO
    {
        [JsonProperty("id")]
        public uint Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("startDate")]
        public DateTime StartDate { get; set; }

        [JsonProperty("endDate")]
        public DateTime EndDate { get; set; }

        [JsonProperty("linkText")]
        public string LinkText { get; set; }

        [JsonProperty("linkUrl")]
        public string LinkUrl { get; set; }

        [JsonProperty("nameImpressionUrl")]
        public string NameImpressionUrl { get; set; }

        [JsonProperty("imgImpressionUrl")]
        public string ImgImpressionUrl { get; set; }

        [JsonProperty("sponsoredComparisonStatus")]
        public SponsoredComparisonStatusDTO Status { get; set; }

        [JsonProperty("entryDate")]
        public DateTime EntryDate { get; set; }

        [JsonProperty("lastUpdated")]
        public DateTime LastUpdated { get; set; }

    }

    public enum SponsoredComparisonStatusDTO
    {
        Unstarted = 0,
        Active = 1,
        Paused = 2,
        Closed = 3,
        Aborted = 4
    }
}
