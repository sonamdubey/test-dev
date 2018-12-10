using Carwale.Entity.AutoComplete;
using Carwale.Entity.CMS.Articles;
using System.Collections.Generic;

namespace Carwale.Entity.Accessories.Tyres
{
    public class TyreDetailSummary
    {
        public ItemSummary TyreSummary { get; set; }
        public string Size { get; set; }
        public List<LabelValue> Overview { get; set; }

        public IList<ArticleSummary> Articles { get; set; }
    }
}
