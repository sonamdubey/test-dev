using Bikewale.Entities.BikeData;
using Bikewale.Entities.EditorialWidgets;
using Bikewale.Entities.PriceQuote;
using Bikewale.Models.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Models.BikeModels
{
    /// <summary>
    /// Modified By : Deepak Israni on 8 May 2018
    /// Description : Added properties for GAInfo and flag to show on road price button
    /// </summary>
    public class EditorialPopularBikesWidget : EditorialWidgetInfo
    {
        public EditorialPopularBikesWidget()
        {
            this.WidgetType = EditorialWidgetType.Popular;
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


        public IEnumerable<MostPopularBikesBase> MostPopularBikeList { get; set; }
        public bool ShowOnRoadPriceButton { get; set; }
        public EditorialGAEntity GAInfo { get; set; }
    }
}
