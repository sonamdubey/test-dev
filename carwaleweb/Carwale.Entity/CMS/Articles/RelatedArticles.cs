using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.CMS.Articles
{
    [Serializable]
    public class RelatedArticles
    {
       public int CategoryId { get; set; }
       public string CategoryMaskingName { get; set; }
       public int BasicId { get; set; }
       public int ParentCatId { get; set; }
    }
}
