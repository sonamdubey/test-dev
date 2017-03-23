using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Models
{
    /// <summary>
    /// Created By : Ashish G. Kamble on 20 Mar 2017
    /// Summary : Class have properties for the page meta
    /// </summary>
    public class PageMetaTags
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Keywords { get; set; }
        public string AlternateUrl { get; set; }
        public string CanonicalUrl { get; set; }
        public string ShareImage { get; set; }
        public string NextPageUrl { get; set; }
        public string PreviousPageUrl { get; set; }
        public string AmpUrl { get; set; }
        public string FBTitle { get; set; }
        public string FBImage { get; set; }
        public bool EnableOG { get; set; }
        public string OGImage { get; set; }
    }
}
