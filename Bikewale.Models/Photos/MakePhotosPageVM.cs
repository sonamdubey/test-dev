using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.CMS.Photos;
using Bikewale.Entities.Pager;
using Bikewale.Entities.GenericBikes;

namespace Bikewale.Models.Photos
{
    public class MakePhotosPageVM : ModelBase
    {
        public IEnumerable<ModelImages> BikeModelsPhotos { get; set; }
        public IDictionary<EnumBikeBodyStyles, IEnumerable<uint>> ModelBodyStyleArray { get; set; }
        public BikeMakeEntityBase Make;
        public string ImagesSynopsis { get; set; }
    }
}
