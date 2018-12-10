using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.CMS
{

    [Serializable]
    public class PhotoGallery
    {
        public string ImagePathMedium { get; set; }
        public string ImagePathThumbNail { get; set; }
        public string ImagePathLarge { get; set; }
        public int ImageId { get; set; }
        public string HostUrl { get; set; }
        public string ImageCaption { get; set; }
        public string OriginalImgPath { get; set; }
    }
}
