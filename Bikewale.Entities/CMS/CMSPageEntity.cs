using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Entities.CMS
{
    public class CMSPageEntity
    {
        public int pageId { get; set; }
        public ushort Priority { get; set; }
        public string PageName { get; set; }
    }
}
