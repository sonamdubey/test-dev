using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bikewale.Entities.CMS.Photos;
using Bikewale.Entities.Videos;

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
