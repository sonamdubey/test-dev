using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.Seo
{
    public class MetaTagDetails : MetaTags
    {
        public string NextPageUrl { get; set; }
        public string PrevPageUrl { get; set; }
    }
}
