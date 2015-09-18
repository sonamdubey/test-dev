using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bikewale.DTO.Make;
using Bikewale.DTO.Model;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Bikewale.DTO.CMS.Photos
{
    public class CMSModelImageBase
    {
        [JsonProperty("imageId")]
        public uint ImageId { get; set; }
        [JsonProperty("hostUrl")]
        public string HostUrl { get; set; }
        [JsonProperty("imagePathThumbnail")]
        public string ImagePathThumbnail { get; set; }
        [JsonProperty("imagePathLarge")]
        public string ImagePathLarge { get; set; }
        [JsonProperty("mainImgCategoryId")]
        public short MainImgCategoryId { get; set; }
        [JsonProperty("imageCategory")]
        public string ImageCategory { get; set; }
        [JsonProperty("caption")]
        public string Caption { get; set; }
        [JsonProperty("imageName")]
        public string ImageName { get; set; }
        [JsonProperty("altImageName")]
        public string AltImageName { get; set; }
        [JsonProperty("imageTitle")]
        public string ImageTitle { get; set; }
        [JsonProperty("imageDescription")]
        public string ImageDescription { get; set; }
        [JsonProperty("makeBase")]
        public MakeBase MakeBase { get; set; }
        [JsonProperty("modelBase")]
        public ModelBase ModelBase { get; set; }
        [JsonProperty("originalImgPath")]
        public string OriginalImgPath{ get; set; }
    }

}
