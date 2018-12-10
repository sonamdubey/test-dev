using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.DTOs.CarData
{

    public class CompareCarsDetailsDTO
    {
        [JsonProperty("compareCars")]
        public List<CompareCarOverviewDTO> CompareCars { get; set; }
        [JsonProperty("nextPageUrl")]
        public string NextPageUrl { get; set; }
    }

    public class CompareCarOverviewDTO
    {
        [JsonProperty("car1")]
        public CompareCarVersionInfoDTO Car1 { get; set; }
        [JsonProperty("car2")]
        public CompareCarVersionInfoDTO Car2 { get; set; }
        [JsonProperty("hostUrl")]
        public string HostUrl { get; set; }
        [JsonProperty("originalImgPath")]
        public string OriginalImgPath { get; set; }
        [JsonProperty("detailsUrl")]
        public string DetailsUrl { get; set; }
        [JsonProperty("isSponsored")]
        public bool IsSponsored { get; set; }
    }

    public class CompareCarVersionInfoDTO 
    {
        [JsonProperty("versionId")]
        public int VersionId { get; set; }
        [JsonProperty("carName")]
        public string CarName { get; set; }
        [JsonProperty("modelId")]
        public int ModelId { get; set; }
    }
}
