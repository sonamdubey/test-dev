using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Entities.Models
{
    /// <summary>
    /// Created By : Ashutosh Sharma on 27 Oct 2017
    /// Description : Provide properties to include only required JS on AMP page.
    /// Modifier    : Kartik Rathod on 1 jun 2018 added IsYoutube 
    /// </summary>
    public class AmpJsTags
    {
        public bool IsAnalytics { get; set; }
        public bool IsSidebar { get; set; }
        public bool IsAccordion { get; set; }
        public bool IsIframe { get; set; }
        public bool IsAd { get; set; }
        public bool IsSocialShare { get; set; }
        public bool IsCarousel { get; set; }
        public bool IsSelector { get; set; }
        public bool IsBind{ get; set; }
        public bool IsYoutube { get; set; }
        public bool IsPositionObserver { get; set; }
    }
}
