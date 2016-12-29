
using System;
namespace Bikewale.Entities.HomePage
{
    /// <summary>
    /// Created by  :   Sumit Kate on 29 Dec 2016
    /// Description :   Home Page Banner Entity
    /// </summary>
    [Serializable]
    public class HomePageBannerEntity
    {
        public string MobileHtml { get; set; }
        public string DesktopHtml { get; set; }
        public string MobileCss { get; set; }
        public string DesktopCss { get; set; }
        public string MobileJS { get; set; }
        public string DesktopJS { get; set; }
    }
}
