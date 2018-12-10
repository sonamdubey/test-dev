using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.DTOs.CMS.Articles
{
    public class ArticleSummaryDTOV2 : ArticleBaseDTOV2
    {
        [JsonProperty("categoryId")]
        public ushort CategoryId { get; set; }

        [JsonProperty("hostUrl")]
        public string HostUrl { get; set; }

        [JsonProperty("imagePath")]
        public string OriginalImgUrl { get; set; }

        [JsonProperty("desc")]
        public string Description { get; set; }

        [JsonProperty("authorName")]
        public string AuthorName { get; set; }

        [JsonProperty("publishedDate")]
        public string DisplayDate { get; set; }

        [JsonProperty("views")]
        public uint Views { get; set; }


        [JsonProperty("isSticky" ),JsonIgnore]
        public bool IsSticky { get; set; }

        [JsonProperty("facebookCommentCount")]
        public uint FacebookCommentCount { get; set; }

        [JsonProperty("makeName"),JsonIgnore]
        public string MakeName { get; set; }  //added by Shalini on 16/10/14

        [JsonProperty("maskingName"),JsonIgnore]
        public string MaskingName { get; set; }  //added by Shalini on 16/10/14

        [JsonProperty("subCategory"),JsonIgnore]
        public string SubCategory { get; set; } //added by Shalini on 03/11/14

        [JsonProperty("modelName"),JsonIgnore]
        public string ModelName { get; set; }

        [JsonProperty("formattedDisplayDate"),JsonIgnore]
        public string FormattedDisplayDate { get; set; } //added by Shalini on 18/11/14

        [JsonProperty("authorMaskingName")]
        public string AuthorMaskingName { get; set; } //added by natesh and removed from ArticleDetails

        [JsonProperty("isFeatured"),JsonIgnore]
        public bool IsFeatured { get; set; } //Added by Sumit Kate on 16 feb 2016

        [JsonProperty("categoryMaskingName"),JsonIgnore]
        public string CategoryMaskingName { get; set; } //Added by Ajay Singh on 27 april 2014 to show category name at news landing page
    }
}
