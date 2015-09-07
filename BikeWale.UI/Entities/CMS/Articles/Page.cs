using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Entity.CMS.Articles
{
    /// <summary>
    /// Created By : Ashwini Todkar on 17 Sept 2014
    /// </summary>
    public class Page
    {
        public ulong pageId { get; set; }
        public ushort Priority { get; set; }
        public string PageName { get; set; }
        public string Content { get; set; }
    }
}
