using Bikewale.Entities.BikeData;
using Bikewale.Entities.GenericBikes;
using Bikewale.Entities.PriceQuote;
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
    /// Modified By : Deepak Israni on 8 May 2018
    /// Description : Added flag for show on road price button and added GA Entity for related information.
    /// Modified by : Sanskar Gupta on 15 May 2018
    /// Description : Added property `ReturnUrlForAmpPages`
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
        public bool ShowOnRoadPriceButton { get; set; }
        public EditorialGAEntity GAInfo { get; set; }
        public String ReturnUrlForAmpPages { get; set; }
    }

    /// <summary>
    /// Created By : Deepak Israni on 8 May 2018
    /// Description: Entity to store information related to GA trigger fired on button click
    /// </summary>
    public class EditorialGAEntity
    {
        public EditorialGACategories CategoryId { get; set; }
        public PQSourceEnum PQSourceId { get; set; }
    }

    /// <summary>
    /// Created By : Deepak Israni on 8 May 2018
    /// Description: Enum to hold values of Editorial Page categories
    /// </summary>
    public enum EditorialGACategories
    {
        Editorial_List_Page = 7,
        Editorial_Details_Page = 8
    }
}
