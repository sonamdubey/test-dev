using Carwale.Entity;
using Carwale.Entity.CMS.Articles;
using Carwale.Entity.SEO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.DTOs.CMS
{
    public class ContentListDTO
    {
        public List<ArticleSummary> Articles { get; set; }
        public MetaTags SeoTags  { get; set; }
        public PageListDTO CMSPages { get; set; }
    }
}
