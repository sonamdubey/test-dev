using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.DTOs.CMS.Articles
{
    /// <summary>
    /// Created By : Ashish G. Kamble on 13 Aug 2014
    /// </summary>
    public class ArticleDetails : ArticleSummary
    {
        public string Content { get; set; }
        public List<string> TagsList { get; set; }
        public List<VehicleTag> VehiclTagsList { get; set; }

        public ArticleBase NextArticle { get; set; }
        public ArticleBase PrevArticle { get; set; }

        public string MainImgCaption { get; set; }
        public bool IsMainImageSet { get; set; }
        public string ShareUrl { get; set; }
    }


    public class ArticleDetails_V1 : ArticleSummaryDTOV2
    {
        [JsonProperty("content")]
        public string Content { get; set; }
        [JsonProperty("tagsList")]
        public List<string> TagsList { get; set; }
        [JsonProperty("vehiclTagsList")]
        public List<VehicleTag_V1> VehiclTagsList { get; set; }

        [JsonProperty("nextArticle")]
        public ArticleBaseDTOV2 NextArticle { get; set; }
        [JsonProperty("prevArticle")]
        public ArticleBaseDTOV2 PrevArticle { get; set; }
        [JsonProperty("mainImgCaption")]
        public string MainImgCaption { get; set; }
        [JsonProperty("isMainImageSet")]
        public bool IsMainImageSet { get; set; }
        [JsonProperty("shareUrl")]
        public string ShareUrl { get; set; }
    }
}
