using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.CMS.Articles
{
    [Serializable]
    public class CMSSubCategoryV2
    {
        public int SubCategoryId { get; set; }
        public string SubCategoryName { get; set; }
        public int RecordCount { get; set; }
    }
}
