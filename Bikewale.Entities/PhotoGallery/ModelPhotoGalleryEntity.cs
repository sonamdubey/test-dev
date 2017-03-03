using Bikewale.Entities.BikeData;
using Bikewale.Entities.CMS.Photos;
using Bikewale.Entities.Videos;
using System.Collections.Generic;

namespace Bikewale.Entities.PhotoGallery
{
    /// <summary>
    /// Created by : Sajal Gupta on 24-02-2017
    /// Description : Entity to hold data of model photo gallery
    /// </summary>
    public class ModelPhotoGalleryEntity
    {
        public IEnumerable<BikeVideoEntity> VideosList { get; set; }
        public IEnumerable<ColorImageBaseEntity> ImageList { get; set; }
        public BikeModelEntity ObjModelEntity { get; set; }
    }
}
