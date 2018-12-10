using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.CMS.Articles
{
    /// <summary>
    /// Created By : Ashish G. Kamble on 13 Aug 2014
    /// </summary>
    [Serializable]
    class Image
    {
        public int MakeId { get; set; }
        public int ModelId { get; set; }
        public string Caption { get; set; }
        public int Sequence { get; set; }
        public string ImageName { get; set; }
        public bool IsMainImage { get; set; }
        public string HostUrl { get; set; }
        public string ImagePathOriginal { get; set; }
        public string ImagePathThumbnail { get; set; }
        public string ImagePathLarge { get; set; }
        public string ImagePathCustom { get; set; }
        public string ImagePathCustom140 { get; set; }
        public string ImagePathCustom200 { get; set; }
        public string ImagePathCustom88 { get; set; }
    }
}
