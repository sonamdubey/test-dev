using Bikewale.Entities.BikeData;
using Bikewale.Interfaces.BikeData.UpComing;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Models;
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
        private readonly IModelsCache _upcomingCacheRepo = null;
        private readonly IModelsRepository _modelsRepo = null;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="upcomingRepo"></param>
        public Upcoming(ICacheManager cache, IModelsCache upcomingCacheRepo, IModelsRepository modelsRepo)
        {
            _cache = cache;
            _upcomingCacheRepo = upcomingCacheRepo;
            _modelsRepo = modelsRepo;
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
                    return m => (ulong)(m.ExpectedLaunchedDate).Ticks;
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
        /// Modified By: Subodh Jain 09 March 2017
        /// Summary :- added body style filter
        /// Modified by : Ashutosh Sharma on 3 Nov 2017.
        /// Description : Add filter for price deviation.
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
                if (filters.BodyStyleId > 0)
                {
                    filterExpression = filterExpression.And(m => m.BodyStyleId == filters.BodyStyleId);
                }
                if (filters.Year > 0)
                {
                    filterExpression = filterExpression.And(m => m.ExpectedLaunchedDate.Year == filters.Year);
                }
            }
            return filterExpression.Compile();
        }


        /// <summary>
        /// Created by : Sangram Nandkhile on 10-Apr-2017 
        /// Binds the makes.
        /// </summary>
        public BrandWidgetVM BindUpcomingMakes(uint topCount)
        {
            BrandWidgetVM brands = null;
            try
            {
                brands = new BrandWidgetVM();
                IEnumerable<UpcomingBikeEntity> upcomingBikes = _upcomingCacheRepo.GetUpcomingModels();
                ICollection<BikeMakeEntityBase> topBrands = new List<BikeMakeEntityBase>();
                ICollection<BikeMakeEntityBase> otherBrands = new List<BikeMakeEntityBase>();
                var makes = upcomingBikes.GroupBy(x => x.MakeBase.MakeId)
                    .Select(x => x.First().MakeBase);

                int i = 0;
                foreach (var make in makes)
                {
                    if (i < topCount)
                    {
                        topBrands.Add(new BikeMakeEntityBase()
                        {
                            MakeId = make.MakeId,
                            MakeName = make.MakeName,
                            Href = String.Format("/{0}-bikes/upcoming/", make.MaskingName),
                            Title = String.Format("Upcoming {0} bikes", make.MaskingName)
                        });
                    }
                    else
                    {
                        otherBrands.Add(new BikeMakeEntityBase()
                        {
                            MakeId = make.MakeId,
                            MakeName = make.MakeName,
                            Href = String.Format("/{0}-bikes/upcoming/", make.MaskingName),
                            Title = String.Format("Upcoming {0} bikes", make.MaskingName)
                        });
                    }
                    i++;
                }
                brands.TopBrands = topBrands;
                brands.OtherBrands = otherBrands;
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.BAL.BikeData.Upcoming.BindUpcomingMakes()");
            }
            return brands;
        }
        /// <summary>
        /// Summary : Function to get the upcoming bikes as per filter criteria
        /// </summary>
        IEnumerable<UpcomingBikeEntity> IUpcoming.GetModels(UpcomingBikesListInputEntity inputParams, EnumUpcomingBikesFilter sortBy)
        {
            return GetModels(inputParams, sortBy);
        }

        /// <summary>
        /// Created by  :   Sajal Gupta on 10-04-2017
        /// Description :   Returns Upcoming Years list 
        /// </summary>
        /// <returns></returns>
        IEnumerable<int> IUpcoming.GetYearList()
        {
            IEnumerable<int> years = null;
            try
            {
                var bikes = _upcomingCacheRepo.GetUpcomingModels();
                if (bikes != null)
                {
                    years = bikes.Select(p => p.ExpectedLaunchedDate.Year).Distinct().OrderBy(m => m);
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.BAL.BikeData.Upcoming.GetYearList");
            }
            return years;
        }

        /// <summary>
        /// Created by  :   Sajal Gupta on 10-04-2017
        /// Description :   Returns Upcoming Years list make wise
        /// </summary>
        /// <returns></returns>
        IEnumerable<int> IUpcoming.GetYearList(uint makeId)
        {
            IEnumerable<int> years = null;
            try
            {
                var bikes = _upcomingCacheRepo.GetUpcomingModels();
                if (bikes != null)
                {
                    years = bikes.Where(m => m.MakeBase.MakeId == makeId).Select(p => p.ExpectedLaunchedDate.Year).Distinct().OrderBy(m => m);
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("Bikewale.BAL.BikeData.Upcoming.GetYearList {0}", makeId));
            }
            return years;
        }

        /// <summary>
        /// Created by  :   Sajal Gupta on 10-04-2017
        /// Description :   Returns Upcoming MakeBase list 
        /// </summary>
        /// <returns></returns>
        IEnumerable<BikeMakeEntityBase> IUpcoming.GetMakeList()
        {
            IEnumerable<BikeMakeEntityBase> makes = null;
            try
            {
                var bikes = _upcomingCacheRepo.GetUpcomingModels();
                if (bikes != null)
                {
                    makes = bikes.GroupBy(x => x.MakeBase.MakeId).Select(x => x.First().MakeBase).ToList();
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.BAL.BikeData.Upcoming.GetMakeList");
            }
            return makes;
        }

        /// <summary>
        /// Created by  :   Sajal Gupta on 10-04-2017
        /// Description :   Returns models according to filters
        /// </summary>
        /// <returns></returns>
        private IEnumerable<UpcomingBikeEntity> GetModels(UpcomingBikesListInputEntity inputParams, EnumUpcomingBikesFilter sortBy)
        {
            IEnumerable<UpcomingBikeEntity> objUpcomingList = null; bool isAsc;
            try
            {

                objUpcomingList = _upcomingCacheRepo.GetUpcomingModels();
                if (objUpcomingList != null && objUpcomingList.Any())
                {
                    objUpcomingList = objUpcomingList.Sort(ProcessOrderBy(sortBy, out isAsc), isAsc);
                    if (inputParams != null)
                    {
                        objUpcomingList = objUpcomingList.Where(ProcessInputFilter(inputParams));
                        if (objUpcomingList != null && objUpcomingList.Any())
                        {
                            objUpcomingList = objUpcomingList.Page(inputParams.PageNo, inputParams.PageSize);
                        }
                    }
                    
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.BAL.BikeData.Upcoming.GetModels");
            }
            return objUpcomingList;
        }

        /// <summary>
        /// Created by  :   Sajal Gupta on 10-04-2017
        /// Description :   Returns Wrapper of bikes , count  according to filters
        /// </summary>
        /// <returns></returns>
        UpcomingBikeResult IUpcoming.GetBikes(UpcomingBikesListInputEntity inputParams, EnumUpcomingBikesFilter sortBy)
        {
            UpcomingBikeResult objBikes = null;
            bool isAsc;
            try
            {
                objBikes = new UpcomingBikeResult();

                IEnumerable<UpcomingBikeEntity> objUpcomingList = _upcomingCacheRepo.GetUpcomingModels();

                if (objUpcomingList != null && objUpcomingList.Any())
                {
                    objUpcomingList = objUpcomingList.Sort(ProcessOrderBy(sortBy, out isAsc), isAsc);
                    objUpcomingList = objUpcomingList.Where(ProcessInputFilter(inputParams));

                    if (objUpcomingList != null && objUpcomingList.Any())
                    {
                        objBikes.TotalCount = (uint)objUpcomingList.Count();
                        objBikes.Bikes = objUpcomingList.Page(inputParams.PageNo, inputParams.PageSize);
                    }

                }
                objBikes.Filter = inputParams;
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.BAL.BikeData.Upcoming.GetBikes");
            }
            return objBikes;
        }


        /// <summary>
        /// Others the makes.
        /// </summary>
        /// <param name="makeId">The make identifier.</param>
        /// <returns>
        /// Created by : Sangram Nandkhile on 13-Apr-2017 
        /// </returns>
        public IEnumerable<BikeMakeEntityBase> OtherMakes(uint makeId, int topCount)
        {
            IEnumerable<BikeMakeEntityBase> makes = null;
            try
            {
                var bikes = _upcomingCacheRepo.GetUpcomingModels();
                if (bikes != null)
                {
                    makes = bikes.GroupBy(y => y.MakeBase.MakeId).
                        Select(group => new
                        BikeMakeEntityBase
                        {
                            MakeId = group.First().MakeBase.MakeId,
                            MakeName = group.First().MakeBase.MakeName,
                            Title = string.Format("Upcoming {0} bikes", group.First().MakeBase.MakeName),
                            Href = String.Format("/{0}-bikes/upcoming/", group.First().MakeBase.MaskingName),
                            Text = string.Format("{0} {1}", group.Count().ToString(), group.Count() == 1 ? "bike" : "bikes")
                        }).Where(x => x.MakeId != makeId).Take(topCount);

                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "NewBikeLaunchesBL.OtherMakes");
            }
            return makes;
        }
    }   // class
}   // namespace
