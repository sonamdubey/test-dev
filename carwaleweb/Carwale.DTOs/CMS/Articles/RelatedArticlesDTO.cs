using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.DTOs.CMS.Articles
{
  public class RelatedArticlesDTO
    {
        [JsonProperty("categoryId")]
        public int CategoryId { get; set; }
        [JsonProperty("categoryMaskingName")]
        public string CategoryMaskingName { get; set; }
        [JsonProperty("basicId")]
        public int BasicId { get; set; }
    }

  public class RelatedArticlesDTOV2
  {
      [JsonProperty("header")]
      public string Header { get; set; }
      [JsonProperty("relatedArticles")]
      public List<ArticleSummaryDTOV4> RelatedArticles { get; set; }
      [JsonProperty("nextPageUrl")]
      public string NextPageUrl { get; set; }
  }
}
