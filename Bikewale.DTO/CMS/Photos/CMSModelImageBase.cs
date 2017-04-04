using Bikewale.DTO.Make;
using Bikewale.DTO.Model;
using Newtonsoft.Json;

namespace Bikewale.DTO.CMS.Photos
{
    public class CMSModelImageBase
    {
        [JsonIgnore]
        public uint ImageId { get; set; }
        [JsonProperty("hostUrl")]
        public string HostUrl { get; set; }
        [JsonIgnore]
        public string ImagePathThumbnail { get; set; }
        [JsonIgnore]
        public string ImagePathLarge { get; set; }
        [JsonIgnore]
        public short MainImgCategoryId { get; set; }
        [JsonIgnore]
        public string ImageCategory { get; set; }
        [JsonIgnore]
        public string Caption { get; set; }
        [JsonIgnore]
        public string ImageName { get; set; }
        [JsonIgnore]
        public string AltImageName { get; set; }
        [JsonIgnore]
        public string ImageTitle { get; set; }
        [JsonIgnore]
        public string ImageDescription { get; set; }
        [JsonIgnore]
        public MakeBase MakeBase { get; set; }
        [JsonIgnore]
        public ModelBase ModelBase { get; set; }
        [JsonProperty("originalImgPath")]
        public string OriginalImgPath { get; set; }
    }

}
