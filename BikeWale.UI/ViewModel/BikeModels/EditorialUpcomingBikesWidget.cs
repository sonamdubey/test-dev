using Bikewale.Entities.BikeData;
using Bikewale.Models.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Models.BikeModels
{
    public class EditorialUpcomingBikesWidget : EditorialWidgetInfo
    {
        public EditorialUpcomingBikesWidget()
        {
            this.WidgetType = EditorialWidgetType.Upcoming;
        }
        public override EditorialWidgetType WidgetType
        {
            get
            {
                return base.WidgetType;
            }

            protected set
            {
                base.WidgetType = value;
            }
        }
        public IEnumerable<UpcomingBikeEntity> UpcomingBikeList { get; set; }
    }
}
