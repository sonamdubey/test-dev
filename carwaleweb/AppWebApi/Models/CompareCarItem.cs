using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace AppWebApi.Models
{
    public class CompareCarItem
    {
        [JsonProperty("firstCaName")]
        public string FirstCaName { get; set; }
        [JsonProperty("SecondCarName")]
        public string SecondCarName { get; set; }
        [JsonProperty("ImageUrl")]
        public string ImageUrl { get; set; }
        [JsonProperty("detailUrl")]
        public string DetailUrl { get; set; }
        [JsonProperty("firstCarVersionId")]
        public string FirstCarVersionId { get; set; }
        [JsonProperty("secondCarVersionId")]
        public string SecondCarVersionId { get; set; }
        [JsonProperty("hostUrl")]
        public string HostUrl { get; set; }
        [JsonProperty("originalImgPath")]
        public string OriginalImgPath { get; set; }
    }
}