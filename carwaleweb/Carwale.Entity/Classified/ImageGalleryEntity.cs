using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.Classified
{
    public class ImageGalleryEntity
    {
        public List<string> ImgThumbURLs { get; set; }
        public List<string> ImgFullURLs { get; set; }
        public string YoutubeURL { get; set; }
    }
}
