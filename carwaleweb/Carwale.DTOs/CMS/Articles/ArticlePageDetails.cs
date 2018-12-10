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
    public class ArticlePageDetails : ArticleSummary
    {
        public List<Page> PageList { get; set; }
        public List<string> TagsList { get; set; }
        public List<VehicleTag> VehiclTagsList { get; set; }

        public ArticleBase NextArticle { get; set; }
        public ArticleBase PrevArticle { get; set; }

        public string MainImgCaption { get; set; }
        public bool IsMainImageSet { get; set; }
        public bool ShowGallery { get; set; }
    }

    public class ArticlePageDetails_V1 : ArticleSummaryDTOV2
    {
        [JsonProperty("pageList")]
        public List<Page_V1> PageList { get; set; }
        [JsonProperty("tagsList")]
        public List<string> TagsList { get; set; }
        [JsonProperty("vehicleTagsList")]
        public List<VehicleTag_V1> VehicleTagsList { get; set; }
        [JsonProperty("mainImgCaption")]
        public string MainImgCaption { get; set; }
        [JsonProperty("isMainImageSet")]
        public bool IsMainImageSet { get; set; }
        [JsonProperty("showGallery")]
        public bool ShowGallery { get; set; }
        [JsonProperty("shareUrl")]
        public string ShareUrl { get; set; }

    }
}
