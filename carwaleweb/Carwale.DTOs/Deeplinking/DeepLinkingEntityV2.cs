using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.DTO.DeepLinking
{
    public class DeepLinkingDTO
    {
        [JsonProperty("screenId")]
        public int ScreenId;
        [JsonProperty("params")]
        public IBase Params;
    }

    public abstract class IBase { };

    public class ModelDetailsDTO : IBase
    {
        [JsonProperty("makeId", NullValueHandling = NullValueHandling.Ignore )]
        public int? MakeId;
        [JsonProperty("modelId",NullValueHandling = NullValueHandling.Ignore)]
        public int? ModelId;
        [JsonProperty("versionId", NullValueHandling = NullValueHandling.Ignore)]
        public int? VersionId;
        [JsonProperty("cityId", NullValueHandling = NullValueHandling.Ignore)]
        public int? CityId;
    }
    public class SearchResultDTO : IBase
    {        
        [JsonProperty("queryString", NullValueHandling = NullValueHandling.Ignore)]
        public string QueryString;
        [JsonProperty("makeId", NullValueHandling = NullValueHandling.Ignore)]
        public int? MakeId;
        [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
        public string Type;
    }
    public class CarCompareDetailsDTO : IBase
    {
        [JsonProperty("modelId1")]
        public int ModelId1;
        [JsonProperty("modelId2")]
        public int ModelId2;
        [JsonProperty("versionId1")]
        public int VersionId1;
        [JsonProperty("versionId2")]
        public int VersionId2;
    }
    public class UsedCarProfileDTO : IBase
    {
        [JsonProperty("profileId")]
        public string ProfileId;
    }
    
    public class NewsDetailDTO : IBase
    {
        [JsonProperty("basicId", NullValueHandling = NullValueHandling.Ignore)]
        public int? BasicId;
        [JsonProperty("categoryId", NullValueHandling = NullValueHandling.Ignore)]
        public int? CategoryId;
        [JsonProperty("makeId", NullValueHandling = NullValueHandling.Ignore)]
        public int? MakeId;
        [JsonProperty("modelId", NullValueHandling = NullValueHandling.Ignore)]
        public int? ModelId;
        [JsonProperty("makeName", NullValueHandling = NullValueHandling.Ignore)]
        public string MakeName;
        [JsonProperty("modelName", NullValueHandling = NullValueHandling.Ignore)]
        public string ModelName;
    }   
}