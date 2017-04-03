
namespace Bikewale.Models
{
    /// <summary>
    /// Created By : Ashish G. Kamble on 20 Mar 2017
    /// Summary : Class have all properties for view models of the mvc pages. All view mdoels for mvc pages should be inherited from this class.
    /// Modified by :   Sumit Kate on 24 Mar 2017
    /// Description :   Initialize Page meta and Ad tag in constructor
    /// Modified by :   Sumit Kate on 28 Mar 2017
    /// Description :   Added PageH1 property of the page for binding
    /// Modified by Sajal Gupta on 01-04-2017
    /// Description : Added IsHeaderRequired, IsAppBannerNeeded
    /// </summary>
    public class ModelBase
    {
        public PageMetaTags PageMetaTags { get; private set; }
        public AdTags AdTags { get; private set; }

        public bool IsTransparentHeader { get; set; }
        public bool IsHomePage { get; set; }
        public bool IsHeaderFix { get; set; }

        private bool _IsHeaderRequired = true;
        public bool IsHeaderRequired
        {
            get { return _IsHeaderRequired; }
            set { _IsHeaderRequired = value; }
        }

        public bool IsPageTypeLanding { get; set; }

        public string Page_ATF_CSS { get; set; }
        public string Page_BTF_CSS_Path { get; set; }
        public string Page_JS_Path { get; set; }

        public string Page_H1 { get; set; }

        public ModelBase()
        {
            this.PageMetaTags = new PageMetaTags();
            this.AdTags = new AdTags();
        }

        private bool _IsAppBannerNeeded = true;
        public bool IsAppBannerNeeded
        {
            get { return _IsAppBannerNeeded; }
            set { _IsAppBannerNeeded = value; }
        }
    }
}
