using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.DTOs.CarData
{
    public class VersionDetailsDTO_AndroidV1
    {
        [JsonProperty("callSlugNumber")]
        public string CallSlugNumber { get; set; }

        [JsonProperty("colors")]
        public VersionSpecsFeaturesTab Colors { get; set; }

        [JsonProperty("exShowroomCity")]
        public string ExShowroomCity { get; set; }

        [JsonProperty("exShowroomPrice")]
        public string ExShowroomPrice { get; set; }

        [JsonProperty("features")]
        public VersionSpecsFeaturesTab Features { get; set; }

        [JsonProperty("hostUrl")]
        public string HostUrl { get; set; }

        [JsonProperty("largePicUrl")]
        public string LargePicUrl { get; set; }

        [JsonProperty("makeId")]
        public string MakeId { get; set; }

        [JsonProperty("makeName")]
        public string MakeName { get; set; }

        [JsonProperty("modelId")]
        public string ModelId { get; set; }

        [JsonProperty("modelName")]
        public string ModelName { get; set; }

        [JsonProperty("newCarGalleryUrl")]
        public string NewCarGalleryUrl { get; set; }

        [JsonProperty("newCarPhotoUrl")]
        public string NewCarPhotoUrl { get; set; }

        [JsonProperty("offerExists")]
        public bool OfferExists { get; set; }

        [JsonProperty("onRoadPriceVersionCityUrl")]
        public string OnRoadPriceVersionCityUrl { get; set; }

        [JsonProperty("originalImgPath")]
        public string OriginalImgPath { get; set; }

        [JsonProperty("otherVersions")]
        public List<CarVersionDTO_V2> OtherVersions { get; set; }

        [JsonProperty("overview")]
        public VersionSpecsFeaturesTab Overview { get; set; }

        [JsonProperty("reviewCount")]
        public string ReviewCount { get; set; }

        [JsonProperty("reviewRate")]
        public string ReviewRate { get; set; }

        [JsonProperty("reviewUrl")]
        public string ReviewUrl { get; set; }

        [JsonProperty("shareUrl")]
        public string ShareUrl { get; set; }

        [JsonProperty("smallPicUrl")]
        public string SmallPicUrl { get; set; }

        [JsonProperty("specifications")]
        public VersionSpecsFeaturesTab Specifications { get; set; }

        [JsonProperty("tinyShareUrl")]
        public string TinyShareUrl { get; set; }

        [JsonProperty("versionId")]
        public string VersionId { get; set; }

        [JsonProperty("versionName")]
        public string VersionName { get; set; }

    }
}
