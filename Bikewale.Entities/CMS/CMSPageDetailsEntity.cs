using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Entities.CMS
{
    public class CMSPageDetailsEntity
    {        
        public string Title { get; set; }
        public string AuthorName { get; set; }
        public string Url { get; set; }
        public string DisplayDate { get; set; }
        public string Category { get; set; }
        public string SubCategory { get; set; }
        public string FieldName { get; set; }
        public string ValueType { get; set; }
        public string OtherInfoValue { get; set; }
        public string Data { get; set; }        

        public IList<CMSPageEntity> PageList { get; set; }
        public IList<CMSBikeTagsEntity> TagsList { get; set; }
        public IList<CMSImageEntity> ImageList { get; set; }

        public string MainImgCaption { get; set; }
        public string NextId { get; set; }
        public string NextUrl { get; set; }
        public string NextTitle { get; set; }
        public string PrevId { get; set; }
        public string PrevUrl { get; set; }
        public string PrevTitle { get; set; }        
        public bool IsMainImageSet { get; set; }
        public uint Views { get; set; }
        public string HostUrl { get; set; }
        public string LargePicUrl { get; set; }
        public string SmallPicUrl { get; set; }
        public uint FacebookCommentCount { get; set; }
    }
}
