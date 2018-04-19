using Bikewale.Entities.BikeData;
using Bikewale.Entities.GenericBikes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Entities.EditorialWidgets
{

    /// <summary>
    /// Created By  : Sanskar Gupta on 18 April 2018
    /// Description : Entity to hold the variables to be passed from `EditorialPage` to `EditorialBasePage.SetAdditionalData()`
    /// </summary>
    public class EditorialWidgetEntity
    {
        public bool IsMobile { get; set; }
        public bool IsMakeLive { get; set; }
        public bool IsModelTagged { get; set; }
        public bool IsSeriesAvailable { get; set; }
        public bool IsScooterOnlyMake { get; set; }
        public EnumBikeBodyStyles BodyStyle { get; set; }
        public uint CityId { get; set; }
        public BikeMakeEntityBase Make { get; set; }
        public BikeSeriesEntityBase Series { get; set; }
    }
}
