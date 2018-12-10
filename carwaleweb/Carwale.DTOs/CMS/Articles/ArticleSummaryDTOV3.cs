using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.DTOs.CMS.Articles
{
    public class ArticleSummaryDTOV3 : ArticleBase
    {
        public ushort CategoryId { get; set; }
        public string HostUrl { get; set; }
        public string LargePicUrl { get; set; }
        public string SmallPicUrl { get; set; }
        public string OriginalImgUrl { get; set; }
        public string Description { get; set; }
        public string AuthorName { get; set; }
        public string DisplayDate { get; set; }
        public uint Views { get; set; }
        public bool IsSticky { get; set; }
        public uint FacebookCommentCount { get; set; }
        public string MakeName { get; set; }
        public string MaskingName { get; set; }
        public string SubCategory { get; set; }
        public string ModelName { get; set; }
        public string FormattedDisplayDate { get; set; }
        public string AuthorMaskingName { get; set; }
        public bool IsFeatured { get; set; }
        public string CategoryMaskingName { get; set; }
        public string DetailPageUrl { get; set; }
        public string ArticleMaskingName { get; set; }
    }

    /*
     * "ArticleUrl": "/news/1957-jaguar-xkss-reborn-for-2017-25907/" //expecting it with a hostUrl
     * "AuthorMaskingName": "omkar-thakur", //expecting a authorUrl instead
     * 
     */
    public class ArticleSummaryDTOV4 : ArticleBaseV2
    {
        [JsonProperty("categoryId")]
        public ushort CategoryId { get; set; }
        [JsonProperty("hostUrl")]
        public string HostUrl { get; set; }
        [JsonProperty("originalImgUrl")]
        public string OriginalImgUrl { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
        [JsonProperty("authorName")]
        public string AuthorName { get; set; }
        [JsonProperty("displayDate")]
        public string DisplayDate { get; set; }
        [JsonProperty("views")]
        public uint Views { get; set; }
        [JsonProperty("subCategory")]
        public string SubCategory { get; set; }
        [JsonProperty("formattedDisplayDate")]
        public string FormattedDisplayDate { get; set; }
        [JsonProperty("authorUrl")]
        public string AuthorUrl { get; set; }
        [JsonProperty("categoryMaskingName")]
        public string CategoryMaskingName { get; set; }
        [JsonProperty("detailPageUrl")]
        public string DetailPageUrl { get; set; }
    }
}
