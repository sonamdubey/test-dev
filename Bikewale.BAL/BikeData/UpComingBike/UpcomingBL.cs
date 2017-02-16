
using Bikewale.Entities.BikeData;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.BikeData.UpComing;
using Bikewale.Notifications;
using System;
using System.Collections.Generic;
namespace Bikewale.BAL.BikeData.UpComingBike
{
    /// <summary>
    /// Created By :- Subodh Jain 16 Feb 2017
    /// Summary :- upcoming bikes BL
    /// </summary>
    public class UpcomingBL : IUpcomingBL
    {
        private readonly IBikeModelsCacheRepository<int> _modelCache = null;
        /// <summary>
        /// Type Initializer
        /// </summary>
        /// <param name="modelCache"></param>
        public UpcomingBL(IBikeModelsCacheRepository<int> modelCache)
        {
            _modelCache = modelCache;
        }
        public IEnumerable<UpcomingBikeEntity> GetUpComingBike(int? makeId)
        {
            IEnumerable<UpcomingBikeEntity> upcomingBikes = null;
            int TopCount = 9;
            try
            {
                if (makeId.HasValue && makeId.Value > 0)
                {
                    upcomingBikes = _modelCache.GetUpcomingBikesList(EnumUpcomingBikesFilter.Default, TopCount, makeId.Value);
                }
                else
                {
                    upcomingBikes = _modelCache.GetUpcomingBikesList(EnumUpcomingBikesFilter.Default, TopCount);
                }

            }
            catch (Exception ex)
            {
                ErrorClass err = new ErrorClass(ex, String.Format("UpcomingBL.GetUpComingBike:({0})", makeId.HasValue ? makeId.Value : 0));
            }
            return upcomingBikes;
        }
    }
}
