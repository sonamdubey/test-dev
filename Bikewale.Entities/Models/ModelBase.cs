using Bikewale.Entities.Models;
using Bikewale.Entities.Pages;
using Bikewale.Entities.Schema;
using System.Collections.Generic;

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
    /// Modified by: Vivek Singh Tomar on 23 Aug 2017
    /// Summary: Added page property to hold page id and name for GA
    /// Modified By : Ashutosh Sharma on 27 Oct 2017
    /// Description : Added AmpJsTags.
    /// Modified By : Deepak Israni on 20 March 2018
    /// Description : Added AdSlots list.
    /// Modified by : Sanskar Gupta on 23 March 2018
    /// Description : Changed type of `AdSlots` to dictionary
    /// Modified by : Sanskar Gupta on 18 April 2018
    /// Description : Added property named `PageName`
    /// Modified by : Ashutosh Sharma on 08 Jun 2018
    /// Description : Added CssClassed.
    /// </summary>
    public class ModelBase
    {
        public PageMetaTags PageMetaTags { get; private set; }
        public AdTags AdTags { get; set; }
        public BreadcrumbList BreadcrumbList { get; private set; }
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

        public string[] Page_JS_Paths { get; set; }
		    public string[] Page_BTF_CSS_Paths { get; set; }

        public ModelBase()
        {
            this.PageMetaTags = new PageMetaTags();
            this.AdTags = new AdTags();
            this.BreadcrumbList = new BreadcrumbList();
            CssClasses = new Dictionary<string, string>();
        }

        public bool ExcludeContestSlug { get; set; }
        public GAPages Page { get; set; }
        public AmpJsTags AmpJsTags { get; set; }
        public string Amp_Page_CSS { get; set; }

        public IDictionary<string, AdSlotModel> AdSlots { get; set; }

        public string PageName { get; set; }
        /// <summary>
        /// Dictionary will hold conditional css classed that need to be used in common Layout.cshtml file.
        /// (string, string) => (key, className), if key exist then apply class name.
        /// </summary>
        public IDictionary<string, string> CssClasses { get; set; }
    }
}
