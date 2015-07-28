using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Entities.CMS
{
    /// <summary>
    /// Created By : Ashish G. Kamble on 15 May 2014
    /// Summary : Class have properties to show the featured content.
    /// </summary>
    public class CMSFeaturedArticlesEntity
    {
        public int ContentId { get; set; }
        public ushort CategoryId { get; set; }
        public string Title { get; set; }
        public string ContentUrl { get; set; }
        public string HostUrl { get; set; }
        public string LargePicUrl { get; set; }
        public string SmallPicUrl { get; set; }        
    }
}
