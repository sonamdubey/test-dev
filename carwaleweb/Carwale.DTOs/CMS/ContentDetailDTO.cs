using Carwale.Entity.CMS.Articles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.DTOs.CMS
{
    public class ContentDetailDTO
    {
        public ArticleDetails Article { get; set; }
        public List<Carwale.DTOs.CMS.Articles.RelatedArticlesDTO> RelatedArticles { get; set; }
        public string DescriptionText { get; set; }
        public string ArticleBodyText { get; set; }
    }

    public class ContentDetailAmpDTO
    {
        public ArticleDetails Article { get; set; }
        public List<ArticleSummary> RelatedArticles { get; set; }
    }
}
