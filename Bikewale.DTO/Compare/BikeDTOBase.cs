using Newtonsoft.Json;
using System;

namespace Bikewale.DTO.Compare
{
    /// <summary>
    /// Created By  :   Sumit Kate on 22 Jan 2016
    /// Description :   Bike basic DTO
    /// </summary>
    public class BikeDTOBase
    {
        [JsonProperty("versionId")]
        public uint VersionId { get; set; }
        [JsonProperty("make")]
        public string Make { get; set; }
        [JsonProperty("model")]
        public string Model { get; set; }
        [JsonProperty("version")]
        public string Version { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("makeMaskingName")]
        public string MakeMaskingName { get; set; }
        [JsonProperty("modelMaskingName")]
        public string ModelMaskingName { get; set; }
        [JsonProperty("hostUrl")]
        public string HostUrl { get; set; }
        [JsonProperty("price")]
        public int Price { get; set; }
        [JsonProperty("imagePath")]
        public string ImagePath { get; set; }
        [JsonProperty("versionRating")]
        public UInt16 VersionRating { get; set; }
        [JsonProperty("modelRating")]
        public UInt16 ModelRating { get; set; }
    }
}
