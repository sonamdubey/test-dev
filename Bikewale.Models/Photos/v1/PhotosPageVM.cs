using Bikewale.Entities.CMS.Photos;
using Bikewale.Entities.Pager;
using System.Collections.Generic;

namespace Bikewale.Models.Photos.v1
{
    /// <summary>
    /// Created by : Ashutosh Sharma on 11th Jan 2018
    /// </summary>
    public class PhotosPageVM : ModelBase
    {
        public IEnumerable<ModelImages> BikeModelsPhotos { get; set; }
        public OtherMakesVM OtherPopularMakes { get; set; }
        public PagerEntity Pager { get; set; }
    }
}
