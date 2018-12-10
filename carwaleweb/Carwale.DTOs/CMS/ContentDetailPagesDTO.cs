using Carwale.DTOs.CarData;
using Carwale.Entity;
using Carwale.Entity.CMS;
using Carwale.Entity.CMS.Articles;
using Carwale.Entity.CMS.Photos;
using Carwale.Entity.Common;
using System.Collections.Generic;

namespace Carwale.DTOs.CMS
{

    public class ContentDetailPagesDTO
    {
        public ArticlePageDetails ArticlePages { get; set; }
        public List<ModelImage> ModelImages { get; set; }
        public SubNavigationDTO SubNavigation { get; set; }
        public List<ulong> RelatedArticleIds { get; set; }
        public List<BreadcrumbEntity> BreadcrumbEntitylist { get; set; }
        public CarRightWidget CarWidgetModel { get; set; }
        public List<VideoDTO> Videos { get; set; }
        public bool IsRedirect { get; set; }
    }

    public class ContentDetailPagesDTO_V1
    {
        public ArticlePageDetails ArticlePages { get; set; }
        public List<ModelImage> ModelImages { get; set; }
        public List<Video> Videos { get;set;}
        public SubNavigationDTO SubNavigation { get; set; }
        public List<Carwale.DTOs.CMS.Articles.RelatedArticlesDTO> RelatedArticles { get; set; }
        public bool IsRedirect { get; set; }
    }

    public class ContentDetailPagesDTO_V2
    {
        public ArticlePageDetails ArticlePages { get; set; }
        public List<ModelImage> ModelImages { get; set; }
        public SubNavigationDTO SubNavigation { get; set; }
        public List<ulong> RelatedArticleIds { get; set; }
        public NewsRightWidget NewsWidgetModel { get; set; }
        public CarRightWidget CarWidgetModel { get; set; }
        public PopularVideoWidget VideoWidgetModel { get; set; }
        public List<BreadcrumbEntity> BreadcrumbEntitylist { get; set; }
        public bool IsRedirect { get; set; }
        public string ArticleBodyText { get; set; }
    }
}
