using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.DTOs.CarData
{

    /// <summary>
    /// Created By : Ajay Singh on 1 march 2016
    /// </summary>
    public class VideoDTO
    {
        [JsonProperty("videoTitle")]
        public string VideoTitle { get; set; }

        [JsonProperty("videoUrl")]
        public string VideoUrl { get; set; }

        [JsonProperty("videoId")]
        public string VideoId { get; set; }

        [JsonProperty("views")]
        public int Views { get; set; }

        [JsonProperty("likes")]
        public int Likes { get; set; }

        [JsonIgnore]
        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("basicId")]
        public int BasicId { get; set; } //basicId of editcms

        [JsonIgnore]
        [JsonProperty("tags")]
        public string Tags { get; set; }

      
        [JsonProperty("duration")]
        public int Duration { get; set; }

        [JsonIgnore]
        [JsonProperty("makeName")]
        public string MakeName { get; set; }

        [JsonIgnore]
        [JsonProperty("modelName")]
        public string ModelName { get; set; }

        [JsonIgnore]
        [JsonProperty("maskingName")]
        public string MaskingName { get; set; }

        [JsonIgnore]
        [JsonProperty("subCatId")]
        public string SubCatId { get; set; }

        [JsonProperty("subCatName")]
        public string SubCatName { get; set; }

        [JsonIgnore]
        [JsonProperty("videoTitleUrl")]
        public string VideoTitleUrl { get; set; }

       
        [JsonProperty("imgHost")]
        public string ImgHost { get; set; }

        [JsonIgnore]
        [JsonProperty("thumbnailPath")]
        public string ThumbnailPath { get; set; }

       
        [JsonProperty("imagePath")]
        public string ImagePath { get; set; }


        [JsonProperty("publishedDate")]
        public DateTime DisplayDate { get; set; }
    }
}
