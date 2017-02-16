
using Bikewale.Entities.BikeData;
using Bikewale.Interfaces.BikeData.UpComing;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
namespace Bikewale.BAL.BikeData.UpComingBike
{
    /// <summary>
    /// Created By :- Subodh Jain 16 Feb 2017
    /// Summary :- upcoming bikes BL
    /// </summary>
    public class Upcoming : IUpcoming
    {
        private readonly ICacheManager _cache;
        private readonly IModelsCache _upcomingRepo = null;
        private readonly IModelsRepository _modelsRepo = null;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="upcomingRepo"></param>
        public Upcoming(ICacheManager cache, IModelsCache upcomingRepo, IModelsRepository modelsRepo)
        {
            _cache = cache;
            _upcomingRepo = upcomingRepo;
            _modelsRepo = modelsRepo;
        }

        /// <summary>
        /// Summary : Function to get the upcoming bikes as per filter criteria
        /// </summary>
        /// <param name="inputParams"></param>
        /// <param name="sortBy"></param>
        /// <param name="recordCount"></param>
        /// <returns></returns>
        public IEnumerable<UpcomingBikeEntity> GetModels(UpcomingBikesListInputEntity inputParams, EnumUpcomingBikesFilter sortBy)
        {
            IEnumerable<UpcomingBikeEntity> objUpcomingList = null;
            try
            {

                objUpcomingList = _upcomingRepo.GetUpcomingModels();
                if (inputParams.MakeId > 0)
                {
                    objUpcomingList = objUpcomingList.Where(m => m.MakeBase.MakeId == inputParams.MakeId);
                }

            }
            catch (Exception ex)
            {

                ErrorClass objErr = new ErrorClass(ex, "Upcoming.GetModels");
            }
            return objUpcomingList;
        }

    }   // class
}   // namespace
