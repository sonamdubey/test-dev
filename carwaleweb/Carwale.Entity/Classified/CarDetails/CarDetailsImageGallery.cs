using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.Classified.CarDetails
{
    /// <summary>
    /// Created By : Sadhana on 27 May 2015
    /// Summary : entity for gallery in details page
    /// </summary>
    [Serializable]
    public class CarDetailsImageGallery
    {
        public List<string> ImageUrls { get; set; }
        public List<ImageUrl> ImageUrlAttributes { get; set; }
        public List<string> VideoUrls { get; set; }
        public string MainImageUrl { get; set; }
        public int TotalPhotosUploaded { get; set; }

    }
}
