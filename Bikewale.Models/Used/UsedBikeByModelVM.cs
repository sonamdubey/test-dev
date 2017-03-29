using Bikewale.Entities.Location;
using Bikewale.Entities.UsedBikes;
using System.Collections.Generic;

namespace Bikewale.Models
{
    public class UsedBikeModelsVM
    {
        public IEnumerable<MostRecentBikes> UsedBikeModelList;
        public CityEntityBase CityDetails;
    }
}
