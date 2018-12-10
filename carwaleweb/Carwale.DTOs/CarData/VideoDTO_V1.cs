using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.DTOs.CarData
{
   public  class VideoDTO_V1
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

        
        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("basicId")]
        public int BasicId { get; set; } //basicId of editcms

       
        [JsonProperty("tags")]
        public string Tags { get; set; }


        [JsonProperty("duration")]
        public int Duration { get; set; }

       
        [JsonProperty("makeName")]
        public string MakeName { get; set; }

      
        [JsonProperty("modelName")]
        public string ModelName { get; set; }

    
        [JsonProperty("maskingName")]
        public string MaskingName { get; set; }

       
        [JsonProperty("subCatId")]
        public string SubCatId { get; set; }

        [JsonProperty("subCatName")]
        public string SubCatName { get; set; }

        [JsonProperty("videoTitleUrl")]
        public string VideoTitleUrl { get; set; }


        [JsonProperty("imgHost")]
        public string ImgHost { get; set; }

        
        [JsonProperty("thumbnailPath")]
        public string ThumbnailPath { get; set; }


        [JsonProperty("imagePath")]
        public string ImagePath { get; set; }


        [JsonProperty("publishedDate")]
        public DateTime DisplayDate { get; set; }

        [JsonProperty("categoryId")]
        public int CategoryId { get; set; }
    }
}
