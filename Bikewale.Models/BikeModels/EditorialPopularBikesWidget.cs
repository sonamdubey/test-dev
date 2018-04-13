using Bikewale.Entities.BikeData;
using Bikewale.Entities.PriceQuote;
using Bikewale.Models.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Models.BikeModels
{
    public class EditorialPopularBikesWidget : EditorialWidgetInfo
    {
        public override EditorialWidgetType WidgetType
        {
            get
            {
                return base.WidgetType;
            }

            protected set
            {
                base.WidgetType = EditorialWidgetType.Popular;
            }
        }


        public IEnumerable<MostPopularBikesBase> MostPopularBikeList { get; set; }
        public uint CityId { get; set; }
        public string ReturnUrlForAmpPages { get; set; }
    }
}
