using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Carwale.DTOs
{
    /// <summary>
    /// Created by : Supriya K on 12/6/2014 
    /// Desc : Class for entity required in news list api of android
    /// </summary>
    public class NewsItemDTOEntity
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("author")]
        public string AuthorName { get; set; }

        [JsonProperty("pubDate")]
        public string DisplayDate { get; set; }

        [JsonProperty("smallPicUrl")]
        public string SmallImageUrl { get; set; }

        [JsonProperty("detailUrl")]
        public string DetailUrl { get; set; }

        [JsonProperty("largepPicUrl")]
        public string LargeImageUrl { get; set; }

        [JsonProperty("mediumPicUrl")]
        public string ThumbNailImageUrl { get; set; }

        [JsonProperty("cwNewsDetailUrl")]
        public string CWNewsDetailUrl { get; set; }

        [JsonProperty("views")]
        public string Views { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("hostUrl")]
        public string HostUrl { get; set; }

        [JsonProperty("originalPathImg")]
        public string OriginalImgPath { get; set; }

        [JsonProperty("categoryId")]
        public int CategoryId { get; set; }

        [JsonIgnore]
        [JsonProperty("makeName")]
        public string MakeName;

        [JsonIgnore]
        [JsonProperty("modelMaskingName")]
        public string ModelMaskingName { get; set; }

        [JsonProperty("categoryMaskingName")]
        public string CategoryMaskingName { get; set; }
                
    }
}
