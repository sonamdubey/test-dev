
using Bikewale.Entities.BikeData;
using Bikewale.Entities.Dealer;
using Bikewale.Entities.DealerLocator;
using Bikewale.Entities.Location;

using System.Collections.Generic;
namespace Bikewale.Models
{
    public class DealerShowroomIndiaPageVM : ModelBase
    {
        public IEnumerable<DealerBrandEntity> AllDealers;
        public UpcomingBikesWidgetVM objUpcomingBikes { get; set; }
        public UsedBikeModelsWidgetVM UsedBikeModel;
        public BikeMakeEntityBase Make;
        public CityEntityBase CityDetails;
        public uint DealerCount;
        public uint CitiesCount;
        public DealerLocatorList States;
        public NewLaunchedWidgetVM NewLaunchedBikes;
    }
}
