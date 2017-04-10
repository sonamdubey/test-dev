
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
        /// Modified By: Subodh Jain 09 March 2017
        /// Summary :- added body style filter
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
            }
            return filterExpression.Compile();
        }


        /// <summary>
        /// Binds the makes.
        /// </summary>
        /// <param name="upcomingBikes">The upcoming bikes.</param>
        /// <param name="topCount">The top count.</param>
        /// <returns>
        /// Created by : Sangram Nandkhile on 10-Apr-2017 
        /// </returns>
        public BrandWidgetVM BindUpcomingMakes(uint topCount)
        {
            BrandWidgetVM brands = null;
            try
            {
                brands = new BrandWidgetVM();
                IEnumerable<UpcomingBikeEntity> upcomingBikes = _upcomingRepo.GetUpcomingModels();
                ICollection<BikeMakeEntityBase> topBrands = new List<BikeMakeEntityBase>();
                ICollection<BikeMakeEntityBase> otherBrands = new List<BikeMakeEntityBase>();
                var makes = upcomingBikes.GroupBy(x => x.MakeBase.MakeId)
                    .Select(x => x.First().MakeBase).ToList();

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
                            Title = String.Format("Upcoming {0} bikes/", make.MaskingName)
                        });
                    }
                    else
                    {
                        otherBrands.Add(new BikeMakeEntityBase()
                        {
                            MakeId = make.MakeId,
                            MakeName = make.MakeName,
                            Href = String.Format("/{0}-bikes/upcoming/", make.MaskingName),
                            Title = String.Format("Upcoming {0} bikes/", make.MaskingName)
                        });
                    }
                    i++;
                }
                brands.TopBrands = topBrands;
                brands.OtherBrands = otherBrands;
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Bikewale.BAL.BikeData.Upcoming.BindMakes()");
            }
            return brands;
        }
    }   // class
}   // namespace
