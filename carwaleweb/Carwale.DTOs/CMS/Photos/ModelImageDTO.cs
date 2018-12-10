using Carwale.Entity.CarData;
using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace Carwale.DTOs.CMS.Photos
{
    public  class ModelImageDTO
    {
        [DataMember]
        [JsonProperty("imageId")]
        public uint ImageId { get; set; }

        [DataMember]
        [JsonProperty("hostUrl")]
        public string HostUrl { get; set; }

        [DataMember]
        [JsonProperty("imagePathThumbnail")]
        public string ImagePathThumbnail { get; set; }

        [DataMember]
        [JsonProperty("imagePathLarge")]
        public string ImagePathLarge { get; set; }

        [DataMember]
        [JsonProperty("originalImgPath")]
        public string OriginalImgPath { get; set; }

        [DataMember]
        [JsonProperty("mainImgCategoryId")]
        public short MainImgCategoryId { get; set; }

        [DataMember]
        [JsonProperty("imageCategory")]
        public string ImageCategory { get; set; }

        [DataMember]
        [JsonProperty("caption")]
        public string Caption { get; set; }

        [DataMember]
        [JsonProperty("imageName")]
        public string ImageName { get; set; }

        [DataMember]
        [JsonProperty("altImageName")]
        public string AltImageName { get; set; }

        [DataMember]
        [JsonProperty("imageTitle")]
        public string ImageTitle { get; set; }

        [DataMember]
        [JsonProperty("imageDescription")]
        public string ImageDescription { get; set; }

        [DataMember]
        [JsonProperty("makeBase")]
        public CarMakeEntityBase MakeBase { get; set; }

        [DataMember]
        [JsonProperty("modelBase")]
        public CarModelEntityBase ModelBase { get; set; }
    }
}
