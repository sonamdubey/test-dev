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
        public override EditorialWidgetType WidgetType
        {
            get
            {
                return base.WidgetType;
            }

            protected set
            {
                base.WidgetType = EditorialWidgetType.OtherBrands;
            }
        }

        public IEnumerable<BikeMakeEntityBase> OtherBrandsList { get; set; }
    }
}
