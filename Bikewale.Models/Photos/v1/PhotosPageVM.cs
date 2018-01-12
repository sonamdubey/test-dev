using System.Collections.Generic;
using Bikewale.Entities.CMS.Photos;

namespace Bikewale.Models.Photos.v1
{
    /// <summary>
    /// Created by : Ashutosh Sharma on 11th Jan 2018
    /// </summary>
    public class PhotosPageVM : ModelBase
    {
        public OtherMakesVM OtherPopularMakes { get; set; }
        public IEnumerable<ModelImages> ModelsImages { get; set; }
    }
}
