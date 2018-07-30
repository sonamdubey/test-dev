using Newtonsoft.Json;
using System.Collections.Generic;

namespace Bikewale.DTO.Model
{
    /// <summary>
    /// Created by:Sangram Nandkhile on 02 Feb 2017
    /// To return DTO of model images, color images and mainimage
    /// Modified by : Rajan Chauhan on 22 Jan 2017
    /// Description : imgPathLarge changed to originalImgPath Json Property
    /// </summary>    
    public class ImageBaseDTO
    {
        [JsonProperty("hostUrl")]
        public string HostUrl { get; set; }
        [JsonProperty("originalImgPath")]
        public string OriginalImgPath { get; set; }
        [JsonProperty("imageType")]
        public ImageBaseType ImageType { get; set; }
        [JsonProperty("imageTitle")]
        public string ImageTitle { get; set; }
    }

    public class ColorImageBaseDTO : ImageBaseDTO
    {
        [JsonProperty("colorId")]
        public uint ColorId { get; set; }
        [JsonProperty("colors")]
        public IEnumerable<string> Colors { get; set; }
    }
    public enum ImageBaseType
    {
        ModelImage = 1,
        ModelGallaryImage = 2,
        ModelColorImage = 3
    }
}
