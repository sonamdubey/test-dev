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
    /// <summary>
    /// Created By  : Rajan Chauhan on 30 Jan 2018
    /// Description : View Model for make photos page
    /// Modified by : Ashutosh Sharma on 6th Feb 2018
    /// Description : Added 'Videos' for videos widget.
    /// </summary>
    public class MakePhotosPageVM : ModelBase
    {
        public IEnumerable<ModelImages> BikeModelsPhotos { get; set; }
        public OtherMakesVM OtherPopularMakes { get; set; }
        public IDictionary<EnumBikeBodyStyles, IEnumerable<uint>> ModelBodyStyleArray { get; set; }
        public BikeMakeEntityBase Make { get; set; }
        public string ImagesSynopsis { get; set; }
        public RecentVideosVM Videos { get; set; }
    }
}
