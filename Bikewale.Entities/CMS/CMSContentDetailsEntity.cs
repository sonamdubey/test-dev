﻿namespace Bikewale.Entities.CMS
{
    public class CMSContentDetailsEntity : CMSContentListEntity
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
        public bool IsMainImageSet { get; set; }
    }
}
