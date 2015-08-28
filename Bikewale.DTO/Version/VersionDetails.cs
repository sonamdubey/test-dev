using Bikewale.DTO.Make;
using Bikewale.DTO.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.DTO.Version
{
    public class VersionDetails
    {
        [JsonProperty("new")]
        public bool New { get; set; }

        [JsonProperty("used")]
        public bool Used { get; set; }

        [JsonProperty("futuristic")]
        public bool Futuristic { get; set; }

        [JsonProperty("bikeName")]
        public string BikeName { get; set; }

        [JsonProperty("hostUrl")]
        public string HostUrl { get; set; }

        [JsonProperty("smallPic")]
        public string SmallPicUrl { get; set; }

        [JsonProperty("largePic")]
        public string LargePicUrl { get; set; }

        [JsonProperty("price")]
        public Int64 Price { get; set; }

        [JsonProperty("originalImagePath")]
        public string OriginalImagePath { get; set; }

        [JsonProperty("makeDetails")]
        private MakeBase objmakeBase { get; set; }

        [JsonProperty("modelDetails")]
        private ModelBase objmodelBase { get; set; }


    }
}
