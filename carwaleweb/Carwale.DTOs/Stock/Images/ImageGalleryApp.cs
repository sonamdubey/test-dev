using System.Collections.Generic;

namespace Carwale.DTOs.Stock.Images
{
    public class ImageGalleryApp
    {
        public string HostUrl { get; set; }
        public IList<string> OriginalImgPaths { get; set; }
        public bool IsChatAvailable { get; set; }
    }
}