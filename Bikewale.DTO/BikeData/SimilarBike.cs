using Bikewale.DTO.Make;
using Bikewale.DTO.Model;
using Bikewale.DTO.Version;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Bikewale.DTO.BikeData
{
    public class SimilarBike
    {
        [JsonProperty("minPrice")]
        public int MinPrice { get; set; }

        [JsonProperty("maxPrice")]
        public int MaxPrice { get; set; }

        [JsonProperty("price")]
        public int VersionPrice { get; set; }

        [JsonProperty("hostUrl")]
        public string HostUrl { get; set; }

        [JsonProperty("imagePath")]
        public string OriginalImagePath { get; set; }

        [JsonProperty("makeBase")]
        public MakeBase MakeBase { get; set; }

        [JsonProperty("modelBase")]
        public ModelBase ModelBase { get; set; }

        [JsonProperty("versionBase")]
        public VersionBase VersionBase { get; set; }
    }
}
