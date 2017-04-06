using Bikewale.Entities.Location;
using Bikewale.Entities.UsedBikes;
using System.Collections.Generic;

namespace Bikewale.Models
{
    /// <author>
    /// Create by: Sangram Nandkhile on 27-Mar-2017
    /// Summary:  View Model for Used bike models. For ex. Used on Homepage and new page
    /// </author>
    public class UsedBikeModelsWidgetVM
    {
        public IEnumerable<MostRecentBikes> UsedBikeModelList;
        public CityEntityBase CityDetails;
    }
}
