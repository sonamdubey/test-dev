using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.ImageUpload
{

    public class ImageDTO
    {
        [JsonProperty("extension")]//Required, 
        public string Extension { get; set; }

        [JsonProperty("id")]
        public int? Id { get; set; }

        [JsonProperty("categoryId")]//Required, 
        public uint? CategoryId { get; set; }

        [JsonProperty("itemId")]//Required,
        public uint? ItemId { get; set; }

        [JsonProperty("hostUrl")]
        public string HostUrl { get; set; }

        [JsonProperty("originalPath")]
        public string OriginalImgPath { get; set; }

        [JsonProperty("isReplicated")]
        public bool? IsReplicated { get; set; }

        [JsonProperty("replicatedId")]
        public uint? ReplicatedId { get; set; }

        [JsonProperty("aspectRatio")]//Required,
        public decimal? AspectRatio { get; set; }

        [JsonProperty("isWaterMark")]
        public bool? IsWaterMark { get; set; }

        [JsonProperty("isMain")]
        public bool? IsMain { get; set; }

        [JsonProperty("isMaster")]
        public bool? IsMaster { get; set; }

        [JsonProperty("processedId")]
        public uint? ProcessedId { get; set; }

        [JsonProperty("sourceId")]
        public int? SourceId { get; set; }

        [JsonProperty("sellerType")]
        public int? SellerType { get; set; }

        [JsonProperty("stockId")]
        public int? StockId { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("versionId")]
        public int? VersionId { get; set; }

        [JsonProperty("imageType")]
        public int ImageType { get; set; }
    }
}
