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
        public IEnumerable<UpcomingBikeEntity> UpcomingBikeList { get; set; }
    }
}
