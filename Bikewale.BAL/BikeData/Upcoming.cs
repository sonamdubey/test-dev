
using Bikewale.Entities.BikeData;
using Bikewale.Interfaces.BikeData.UpComing;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Notifications;
using Bikewale.Utility.LinqHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
            IEnumerable<UpcomingBikeEntity> objUpcomingList = null; bool isAsc;
            try
            {

                objUpcomingList = _upcomingRepo.GetUpcomingModels();
                if (objUpcomingList != null && objUpcomingList.Count() > 0)
                {
                    objUpcomingList = objUpcomingList.Where(ProcessInputFilter(inputParams));
                    if (objUpcomingList != null && objUpcomingList.Count() > 0)
                    {
                        objUpcomingList = objUpcomingList.Sort(ProcessOrderBy(sortBy, out isAsc), isAsc);
                        objUpcomingList = objUpcomingList.Page(inputParams.StartIndex, inputParams.EndIndex);

                    }

                }

            }
            catch (Exception ex)
            {

                ErrorClass objErr = new ErrorClass(ex, "Upcoming.GetModels");
            }
            return objUpcomingList;
        }
        /// <summary>
        /// Created By:- Subodh Jain 17 Feb 2017
        /// Summary :- Process orderby filter
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="isAsc"></param>
        /// <returns></returns>
        private Func<UpcomingBikeEntity, ulong> ProcessOrderBy(EnumUpcomingBikesFilter filter, out bool isAsc)
        {
            isAsc = true;
            switch (filter)
            {
                case EnumUpcomingBikesFilter.Default:
                case EnumUpcomingBikesFilter.LaunchDateSooner:
                default:
                    return m => (ulong)Convert.ToDateTime(m.ExpectedLaunchDate).Ticks;
                case EnumUpcomingBikesFilter.PriceLowToHigh:
                    return m => m.EstimatedPriceMin;
                case EnumUpcomingBikesFilter.PriceHighToLow:
                    isAsc = false;
                    return m => m.EstimatedPriceMin;
                case EnumUpcomingBikesFilter.LaunchDateLater:
                    isAsc = false;
                    return m => (ulong)Convert.ToDateTime(m.ExpectedLaunchDate).Ticks;
            }
        }
        /// <summary>
        /// Created by  :   Subodh Jain on 10 Feb 2017
        /// Description :   Process Input Filters
        /// </summary>
        /// <param name="filters"></param>
        /// <returns></returns>
        private Func<UpcomingBikeEntity, bool> ProcessInputFilter(UpcomingBikesListInputEntity filters)
        {
            Expression<Func<UpcomingBikeEntity, bool>> filterExpression = PredicateBuilder.True<UpcomingBikeEntity>();
            if (filters != null)
            {
                if (filters.MakeId > 0)
                {
                    filterExpression = filterExpression.And(m => m.MakeBase.MakeId == filters.MakeId);
                }
                if (filters.ModelId > 0)
                {
                    filterExpression = filterExpression.And(m => m.ModelBase.ModelId == filters.ModelId);
                }
            }
            return filterExpression.Compile();
        }


    }   // class
}   // namespace
