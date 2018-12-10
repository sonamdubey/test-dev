using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.ImageUpload
{
    public class ImageTokenDTO : Token
    {
        [JsonProperty("id")]//Required,
        public int? Id { get; set; }
        [JsonIgnore]
        public bool Status { get; set; }
        [JsonIgnore]
        public bool ServerError { get; set; }
        [JsonProperty("photoId")]
        public uint? PhotoId { get; set; }
        [JsonProperty("originalImagePath")]
        public string OriginalImgPath { get; set; }
        [JsonProperty("key")]
        public string Key { get; set; }
        [JsonProperty("imageType")]
        public int ImageType { get; set; }
    }
}
