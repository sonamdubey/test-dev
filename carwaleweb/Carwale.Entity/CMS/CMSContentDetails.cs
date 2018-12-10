using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity
{
    /// <summary>
    /// Additional properties to CMSContentList that are necessary to display the content.
    /// </summary>
    /// 

    // This entity class is for content on the details page of News, AutoExpo, Pitstop.

    [Serializable]
    public class CMSContentDetails : CMSContentList
    {
        public string Tag { get; set; }
        public string Content { get; set; }
        public string MainImgCaption { get; set; }
        public string NextId { get; set; }
        public string NextUrl { get; set; }
        public string NextTitle { get; set; }
        public string PrevId { get; set; }
        public string PrevUrl { get; set; }
        public string PrevTitle { get; set; }
        public string Caption { get; set; }
    }
}
