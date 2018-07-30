using Bikewale.Entities.CMS.Photos;
using System.Collections.Generic;

namespace Bikewale.Models
{
    /// <summary>
    /// Created by : Aditi Srivastava on 30 Mar 2017
    /// Summary    : View model for edit cms photo gallery
    /// </summary>
    public class EditCMSPhotoGalleryVM
    {
        public IEnumerable<ModelImage> Images { get; set; }
        public int ImageCount {get;set;}
        public string BikeName { get; set; }
    }
}
