using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Entities.CMS.Articles
{
    /// <summary>
    /// Written By : Ashish G. Kamble on 24 Sept 2015
    /// </summary>
    public class HtmlContent
    {
        public List<HtmlItem> HtmlItems { get; set; }

        public HtmlContent()
        {
            HtmlItems = new List<HtmlItem>();
        }
    }
}
