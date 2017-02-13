using Bikewale.Entities.BikeData.NewLaunched;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.BikeData.NewLaunched;
using Bikewale.Notifications;
using Bikewale.Utility.LinqHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
namespace Bikewale.BAL.BikeData.NewLaunched
{
    /// <summary>
    /// Created by  :   Sumit Kate on 10 Feb 2017
    /// Description :   NewBikeLaunches Business Layer
    /// </summary>
    public class NewBikeLaunchesBL : INewBikeLaunchesBL
    {
        private readonly IBikeModelsCacheRepository<int> _modelCache = null;

        /// <summary>
        /// Type Initializer
        /// </summary>
        /// <param name="modelCache"></param>
        public NewBikeLaunchesBL(IBikeModelsCacheRepository<int> modelCache)
        {
            _modelCache = modelCache;
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 10 Feb 2017
        /// Description :   Returns New Launched MakeList with Bike count
        /// </summary>
        /// <returns></returns>
        public IEnumerable<BikesCountByMakeEntityBase> GetMakeList()
        {
            IEnumerable<BikesCountByMakeEntityBase> makeList = null;
            try
            {
                var bikes = _modelCache.GetNewLaunchedBikesList();
                if (bikes != null)
                {
                    makeList = from bike in bikes
                               group bike by bike.Make.MakeId into grpMake
                               select new BikesCountByMakeEntityBase()
                               {
                                   BikeCount = grpMake.Count(),
                                   Make = grpMake.FirstOrDefault(m => m.Make.MakeId == grpMake.Key).Make
                               };
                    makeList = makeList.OrderBy(make => make.Make.PopularityIndex);
                }
            }
            catch (Exception ex)
            {
                ErrorClass err = new ErrorClass(ex, "NewBikeLaunchesBL.NewLaunchedMakeList");
            }
            return makeList;
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 10 Feb 2017
        /// Description :   Returns New Launched Years list with count
        /// </summary>
        /// <returns></returns>
        public IEnumerable<BikesCountByYearEntityBase> YearList()
        {
            IEnumerable<BikesCountByYearEntityBase> years = null;
            try
            {
                var bikes = _modelCache.GetNewLaunchedBikesList();
                if (bikes != null)
                {
                    years = bikes
                                .GroupBy(m => m.LaunchedOn.Year)
                                .Select(g => new BikesCountByYearEntityBase { Year = g.Key, BikeCount = g.Count() })
                                .OrderByDescending(m => m.Year);
                }
            }
            catch (Exception ex)
            {
                ErrorClass err = new ErrorClass(ex, "NewBikeLaunchesBL.NewLaunchedYearList");
            }
            return years;
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 10 Feb 2017
        /// Description :   Get New Launched Bikes with filters
        /// </summary>
        /// <param name="filters"></param>
        /// <returns></returns>
        public NewLaunchedBikeResult GetBikes(InputFilter filters)
        {
            NewLaunchedBikeResult result = null;

            try
            {
                var bikes = _modelCache.GetNewLaunchedBikesList();
                if (bikes != null && bikes.Count() > 0)
                {
                    result = new NewLaunchedBikeResult();

                    var filteredBikes = bikes.Where(ProcessInputFilter(filters));
                    if (filteredBikes != null && filteredBikes.Count() > 0)
                    {
                        result.Bikes = filteredBikes.Page(filters.PageNo, filters.PageSize);
                        result.TotalCount = (uint)filteredBikes.Count();
                        result.Filter = filters;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass err = new ErrorClass(ex, String.Format("NewBikeLaunchesBL.GetBikes({0})", Newtonsoft.Json.JsonConvert.SerializeObject(filters)));
            }

            return result;
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 10 Feb 2017
        /// Description :   Returns New Launched MakeList with Bike count skip mentioned make id
        /// </summary>
        /// <param name="skipMakeId"></param>
        /// <returns></returns>
        public IEnumerable<BikesCountByMakeEntityBase> GetMakeList(uint skipMakeId)
        {
            IEnumerable<BikesCountByMakeEntityBase> makes = null;
            try
            {
                makes = GetMakeList();
                if (makes != null && makes.Count() > 0)
                {
                    return makes.Where(PredicateSkipMakeId(skipMakeId));
                }
            }
            catch (Exception ex)
            {
                ErrorClass err = new ErrorClass(ex, String.Format("NewBikeLaunchesBL.NewLaunchedMakeList({0})", skipMakeId));
            }
            return makes;
        }

        #region Linq Predicates
        /// <summary>
        /// Created by  :   Sumit Kate on 10 Feb 2017
        /// Description :   Process Input Filters
        /// </summary>
        /// <param name="filters"></param>
        /// <returns></returns>
        private Func<NewLaunchedBikeEntityBase, bool> ProcessInputFilter(InputFilter filters)
        {
            Expression<Func<NewLaunchedBikeEntityBase, bool>> filterExpression = PredicateBuilder.True<NewLaunchedBikeEntityBase>();
            if (filters != null)
            {
                if (filters.Make > 0)
                {
                    filterExpression = filterExpression.And(PredicateMakeId(filters.Make));
                }
                if (filters.YearLaunch > 0)
                {
                    filterExpression = filterExpression.And(PredicateYear(filters.YearLaunch));
                }
            }
            return filterExpression.Compile();
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 10 Feb 2017
        /// Description :   Predicate for Skip MakeId
        /// </summary>
        /// <param name="skipMakeId"></param>
        /// <returns></returns>
        private Func<BikesCountByMakeEntityBase, bool> PredicateSkipMakeId(uint skipMakeId)
        {
            return m => m.Make.MakeId != skipMakeId;
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 10 Feb 2017
        /// Description :   Predicate to match makeid
        /// </summary>
        /// <param name="makeId"></param>
        /// <returns></returns>
        private Expression<Func<NewLaunchedBikeEntityBase, bool>> PredicateMakeId(uint makeId)
        {
            return m => m.Make.MakeId == makeId;
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 10 Feb 2017
        /// Description :   Predicate to match year
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        private Expression<Func<NewLaunchedBikeEntityBase, bool>> PredicateYear(uint year)
        {
            return m => m.LaunchedOn.Year == year;
        }
        #endregion
    }
}
