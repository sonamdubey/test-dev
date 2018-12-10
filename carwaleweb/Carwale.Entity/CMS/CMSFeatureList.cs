using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity
{
    public class CMSFeatureList
    {
        public int contentId { get; set; }
        public string authorName { get; set; }
        public string description { get; set; }
        public string displayDate { get; set; }
        public string views { get; set; }
        public string title { get; set; }
        public string url { get; set; }
        public string hostUrl { get; set; }
        public string imagePathThumbNail { get; set; }
        public string imagePathLarge { get; set; }
    }
}
