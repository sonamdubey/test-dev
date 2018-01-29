using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bikewale.Entities.CMS.Photos;
using Bikewale.Entities.Pager;
using Bikewale.Entities.GenericBikes;

namespace Bikewale.Models.Photos
{
    public class MakePhotosPageVM : ModelBase
    {
        public IEnumerable<ModelImages> BikeModelsPhotos { get; set; }
        public Dictionary<EnumBikeBodyStyles, IEnumerable<int>> ModelBodyStyleArray { get; set; }
        public string ImagesSynopsis { get; set; }
    }
}
