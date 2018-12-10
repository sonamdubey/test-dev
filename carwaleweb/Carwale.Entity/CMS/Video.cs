using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.CMS
{   
    /// <summary>
    /// Modified by     :   Sumit Kate on 19 Feb 2016
    /// Description     :   Added IsFeatured flag
    /// </summary>
    [Serializable, JsonObject]
    public class Video
    {
        [JsonProperty]
        public string VideoTitle { get; set; }

        [JsonProperty]
        public string VideoUrl { get; set; }

        [JsonProperty]
        public string VideoId { get; set; }

        [JsonProperty]
        public int Views { get; set; }

        [JsonProperty]
        public int Likes { get; set; }

        [JsonProperty]
        public string Description { get; set; }

        [JsonProperty]
        public int BasicId { get; set; } //basicId of editcms

        [JsonProperty]
        public string Tags { get; set; }

        [JsonProperty]
        public int Duration { get; set; }

        [JsonProperty]
        public string MakeName { get; set; }

        [JsonProperty]
        public string ModelName { get; set; }

        [JsonProperty]
        public string MaskingName { get; set; }

        [JsonProperty]
        public string SubCatId { get; set; }

        [JsonProperty]
        public string SubCatName { get; set; }

        [JsonProperty]
        public string VideoTitleUrl { get; set; }

        [JsonProperty]
        public string ImgHost { get; set; }

        [JsonProperty]
        public string ThumbnailPath { get; set; }

        [JsonProperty]
        public string ImagePath { get; set; }

        [JsonProperty]
        public DateTime DisplayDate { get; set; }

        [JsonProperty]
        public bool IsFeatured { get; set; }

        [JsonProperty]
        public int CategoryId { get; set; }
    }
}
