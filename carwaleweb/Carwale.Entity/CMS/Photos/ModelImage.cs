using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Carwale.Entity.CarData;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Carwale.Entity.CMS.Photos
{
    /// <summary>
    /// Created By : Ashish G. Kamble on 13 Aug 2014
    /// </summary>
    [Serializable,JsonObject]
    public class ModelImage
    {
        [JsonProperty]
        public uint ImageId { get; set; }

        [JsonProperty]
        public string HostUrl { get; set; }

        [JsonProperty]
        public string ImagePathThumbnail { get; set; }

        [JsonProperty]
        public string ImagePathLarge { get; set; }

        [JsonProperty]
        public string OriginalImgPath { get; set; }

        [JsonProperty]
        public short MainImgCategoryId { get; set; }

        [JsonProperty]
        public short SubImgCategoryId { get; set; }

        [JsonProperty]
        public string ImageCategory { get; set; }

        [JsonProperty]
        public string Caption { get; set; }

        [JsonProperty]
        public string ImageName { get; set; }

        [JsonProperty]
        public string AltImageName { get; set; }

        [JsonProperty]
        public string ImageTitle { get; set; }

        [JsonProperty]
        public string ImageDescription { get; set; }

        [JsonProperty]
        public CarMakeEntityBase MakeBase { get; set; }

        [JsonProperty]
        public CarModelEntityBase ModelBase { get; set; }
    }
}