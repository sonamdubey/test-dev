using Bikewale.Entities.CMS.Photos;
using Bikewale.Entities.Pager;
using System.Collections.Generic;

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/// <summary>
/// Created By  : Rajan Chauhan on 09 Jan 2017
/// Description : Added OtherPopularMakes
/// </summary>
namespace Bikewale.Models.Photos.v1
{
    /// <summary>
    /// Created by : Ashutosh Sharma on 11th Jan 2018
    /// </summary>
    public class PhotosPageVM : ModelBase
    {
        public IEnumerable<ModelImages> BikeModelsPhotos { get; set; }
        public OtherMakesVM OtherPopularMakes { get; set; }
        public IEnumerable<ModelImages> ModelsImages { get; set; }
        public PagerEntity Pager { get; set; }
    }
}
