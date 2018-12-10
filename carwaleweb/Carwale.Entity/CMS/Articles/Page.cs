using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.CMS.Articles
{
    /// <summary>
    /// Created By : Ashish G. Kamble on 13 Aug 2014
    /// </summary>
    [Serializable]
    public class Page
    {
        public ulong pageId { get; set; }
        public ushort Priority { get; set; }
        public string PageName { get; set; }
        public string Content { get; set; }
    }
}
