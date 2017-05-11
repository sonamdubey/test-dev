
using Bikewale.Entities.BikeData;
using Bikewale.Models;
using System.Collections.Generic;
namespace Bikewale.Interfaces.BikeData.UpComing
{
    /// <summary>
    /// Created By :- Subodh Jain 16 Feb 2017
    /// Summary :-  upcoming bikes interface
    /// </summary>
    public interface IUpcoming
    {
        IEnumerable<UpcomingBikeEntity> GetModels(UpcomingBikesListInputEntity inputParams, EnumUpcomingBikesFilter sortBy);
        BrandWidgetVM BindUpcomingMakes(uint topCount);
        IEnumerable<BikeMakeEntityBase> OtherMakes(uint makeId, int topCount);
        IEnumerable<int> GetYearList();
        IEnumerable<int> GetYearList(uint makeId);
        IEnumerable<BikeMakeEntityBase> GetMakeList();
        UpcomingBikeResult GetBikes(UpcomingBikesListInputEntity inputParams, EnumUpcomingBikesFilter sortBy);
    }
}
