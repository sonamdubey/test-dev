﻿namespace Bikewale.Entities.CMS
{
    public class CMSImageEntity
    {
        public int MakeId { get; set; }
        public int ModelId { get; set; }
        public string Caption { get; set; }
        public short Sequence { get; set; }
        public string ImageName { get; set; }
        public bool IsMainImage { get; set; }
        public string HostUrl { get; set; }
        public string ImagePathCustom { get; set; }
        public string ImagePathCustom140 { get; set; }
        public string ImagePathCustom200 { get; set; }
        public string ImagePathCustom88 { get; set; }
        public string ImagePathLarge { get; set; }
        public string ImagePathOriginal { get; set; }
        public string ImagePathThumbnail { get; set; }
    }
}
