using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Entities.CMS
{
    public class CMSContentListEntity
    {
        public int ContentId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string AuthorName { get; set; }
        public string DisplayDate { get; set; }
        public uint Views { get; set; }
        public string ContentUrl { get; set; }
        public string HostUrl { get; set; }
        public string LargePicUrl { get; set; }
        public string SmallPicUrl { get; set; }        
        public bool IsSticky { get; set; }
        public uint FacebookCommentCount { get; set; }
        public int RowNumber { get; set; }
    }
}
