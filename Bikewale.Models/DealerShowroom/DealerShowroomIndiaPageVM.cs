
using Bikewale.Entities.BikeData;
using Bikewale.Entities.Dealer;
using Bikewale.Entities.DealerLocator;
using Bikewale.Entities.Location;
using Bikewale.Entities.UsedBikes;
using System.Collections.Generic;
namespace Bikewale.Models
{
    public class DealerShowroomIndiaPageVM : ModelBase
    {
        public IEnumerable<DealerBrandEntity> AllDealers;
        public IEnumerable<UpcomingBikeEntity> objUpcomingBikes;
        public UsedBikeModels UsedBikeModel;
        public BikeMakeEntityBase Make;
        public CityEntityBase CityDetails;
        public uint DealerCount;
        public uint CitiesCount;
        public DealerLocatorList States;
        public NewLaunchedWidgetVM NewLaunchedBikes;
    }
}
