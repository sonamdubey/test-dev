using System.Collections.Generic;

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
