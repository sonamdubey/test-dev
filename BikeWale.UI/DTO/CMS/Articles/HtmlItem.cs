using System.Collections.Generic;

namespace Bikewale.DTO.CMS.Articles
{
    /// <summary>
    /// Written By : Ashish G. Kamble on 24 Sept 2015
    /// </summary>
    public class HtmlItem
    {
        public string Content { get; set; }

        public string Type { get; set; }

        public bool SetMargin { get; set; }

        public List<string> ContentList { get; set; }
    }
}
