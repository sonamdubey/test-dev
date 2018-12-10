using Carwale.Entity.CarData;
using Carwale.Entity.CMS.Articles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.Common
{
    [Serializable]
    public class NewsRightWidget
    {
        public List<ArticleSummary> PopularNews { get; set; }
        public List<ArticleSummary> RecentNews { get; set; }
    }
}
