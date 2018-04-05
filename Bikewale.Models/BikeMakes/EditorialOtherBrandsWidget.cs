using Bikewale.Entities.BikeData;
using Bikewale.Models.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Models.BikeMakes
{
    public class EditorialOtherBrandsWidget : EditorialWidgetInfo
    {
        public IEnumerable<BikeMakeEntityBase> OtherBrandsList { get; set; }
    }
}
