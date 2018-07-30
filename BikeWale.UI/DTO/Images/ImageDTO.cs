
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
namespace Bikewale.DTO.Images
{
    /// <summary>
    /// Created by  :   Sumit Kate on 15 Nov 2016
    /// Description :   Image DTO
    /// </summary>
    public class ImageDTO
    {
        [Required, JsonProperty("extension")]
        public string Extension { get; set; }

        [JsonProperty("id")]
        public uint? Id { get; set; }

        [Required, JsonProperty("categoryId")]
        public uint? CategoryId { get; set; }

        [Required, JsonProperty("itemId")]
        public uint? ItemId { get; set; }

        [JsonProperty("hostUrl")]
        public string HostUrl { get; set; }

        [JsonProperty("originalPath")]
        public string OriginalPath { get; set; }

        [JsonProperty("isReplicated")]
        public bool? IsReplicated { get; set; }

        [JsonProperty("replicatedId")]
        public uint? ReplicatedId { get; set; }

        [Required, JsonProperty("aspectRatio")]
        public decimal? AspectRatio { get; set; }

        [JsonProperty("isWaterMark")]
        public bool? IsWaterMark { get; set; }

        [JsonProperty("isMain")]
        public bool? IsMain { get; set; }

        [JsonProperty("isMaster")]
        public bool? IsMaster { get; set; }

        [JsonProperty("processedId")]
        public uint? ProcessedId { get; set; }
    }
}
