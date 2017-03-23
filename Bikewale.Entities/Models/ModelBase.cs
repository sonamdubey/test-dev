using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Models
{
    /// <summary>
    /// Created By : Ashish G. Kamble on 20 Mar 2017
    /// Summary : Class have all properties for view models of the mvc pages. All view mdoels for mvc pages should be inherited from this class.
    /// </summary>
    public class ModelBase
    {
        public PageMetaTags PageMetaTags { get; set; }
        public AdTags AdTags { get; set; }

        public bool IsTransparentHeader { get; set; }
        public bool IsHomePage { get; set; }
        public bool IsHeaderFix { get; set; }

        public string Page_ATF_CSS { get; set; }
        public string Page_BTF_CSS_Path { get; set; }
        public string Page_JS_Path { get; set; }
    }
}
