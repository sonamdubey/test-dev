using Bikewale.BAL.ApiGateway.Adapters.BikeData;
using Bikewale.BAL.ApiGateway.ApiGatewayHelper;
using Bikewale.BAL.ApiGateway.Entities.BikeData;
using Bikewale.Entities.BikeData;
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
        private readonly IApiGatewayCaller _apiGatewayCaller;
        /// <summary>
        /// Type Initializer
        /// </summary>
        /// <param name="modelCache"></param>
        public NewBikeLaunchesBL(IBikeModelsCacheRepository<int> modelCache, IApiGatewayCaller apiGatewayCaller)
        {
            _modelCache = modelCache;
            _apiGatewayCaller = apiGatewayCaller;
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 10 Feb 2017
        /// Description :   Returns New Launched MakeList with Bike count
        /// Modified by :   Sumit Kate on 15 Feb 2017
        /// Description :   Replaced Linq query with Linq lambda syntax
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
                    var grpMakes = bikes
                        .GroupBy(m => m.Make.MakeId, m => new { Make = m.Make });

                    if (grpMakes != null)
                    {
                        makeList = grpMakes
                                        .Select(m => new BikesCountByMakeEntityBase()
                                        {
                                            BikeCount = m.Count(),
                                            Make = m.FirstOrDefault().Make
                                        })
                                        .OrderBy(make => make.Make.PopularityIndex);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "NewBikeLaunchesBL.NewLaunchedMakeList");
            }
            return makeList;
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 10 Feb 2017
        /// Description :   Returns New Launched Years list with count
        /// Modified by :   Sumit Kate on 15 Feb 2017
        /// Description :   Use group and iterate group to get final result
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
                    var grpYear = bikes
                                .GroupBy(m => m.LaunchedOn.Year, m => new { bikeName = String.Format("{0} {1}", m.Make.MakeName, m.Model.ModelName) });
                    if (grpYear != null)
                    {
                        years = grpYear
                                .Select(
                                m => new BikesCountByYearEntityBase()
                                {
                                    Year = m.Key,
                                    BikeCount = m.Count(),
                                    Bikes = m.Select(b => (b.bikeName)).Take(3)
                                })
                                .OrderByDescending(m => m.Year);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "NewBikeLaunchesBL.NewLaunchedYearList");
            }
            return years;
        }

        /// <summary>
        /// Created by  : Rajan Chauhan on 3 Apr 2018
        /// Description : Method to get NewLaunchedBikesList
        /// </summary>
        /// <param name="startIndex"></param>
        /// <param name="endIndex"></param>
        /// <param name="makeId"></param>
        /// <returns></returns>
        public NewLaunchedBikesBase GetNewLaunchedBikesList(int startIndex, int endIndex, int? makeId = null)
        {
            NewLaunchedBikesBase result = null;
            try
            {
                result = _modelCache.GetNewLaunchedBikesList(startIndex, endIndex, makeId);
                IEnumerable<NewLaunchedBikeEntity> newLaunchesList = result != null ? result.Models : null;
                if (newLaunchesList != null && newLaunchesList.Any())
                {
                    var specsItemList = new List<EnumSpecsFeaturesItems>
                    {
                        EnumSpecsFeaturesItems.Displacement,
                        EnumSpecsFeaturesItems.FuelEfficiencyOverall,
                        EnumSpecsFeaturesItems.MaxPowerBhp,
                        EnumSpecsFeaturesItems.MaximumTorqueNm,
                        EnumSpecsFeaturesItems.KerbWeight
                    };
                    BindMinSpecs(newLaunchesList, specsItemList);
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("NewBikeLaunchesBL.GetNewLaunchedBikesList({0}, {1}, {2})", startIndex, endIndex, makeId));
            }
            return result;
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 10 Feb 2017
        /// Description :   Get New Launched Bikes with filters
        /// Modified by:- Subodh jain 09 march 2017
        ///summary :-  Added body type filter
        /// Modified by : Rajan Chauhan on 04 Apr 2018
        /// Description : Binding logic for minSpecs from MS added
        /// </summary>
        /// <param name="filters"></param>
        /// <returns></returns>
        public NewLaunchedBikeResult GetBikes(InputFilter filters)
        {
            NewLaunchedBikeResult result = null;
            IEnumerable<NewLaunchedBikeEntityBase> bikes = null;
            try
            {
                if (filters.CityId > 0)
                    bikes = _modelCache.GetNewLaunchedBikesList(filters.CityId);
                else
                    bikes = _modelCache.GetNewLaunchedBikesList();

                if (bikes != null && bikes.Any())
                {
                    result = new NewLaunchedBikeResult();

                    var filteredBikes = bikes.Where(ProcessInputFilter(filters));
                    if (filteredBikes != null && filteredBikes.Any())
                    {
                        result.Bikes = filteredBikes.Page(filters.PageNo, filters.PageSize);
                        result.TotalCount = (uint)filteredBikes.Count();
                        result.Filter = filters;
                    }
                    IEnumerable<NewLaunchedBikeEntityBase> newLaunchesList = result.Bikes;
                    if (newLaunchesList != null && newLaunchesList.Any())
                    {
                        var specsItemList = new List<EnumSpecsFeaturesItems>
                        {
                            EnumSpecsFeaturesItems.Displacement,
                            EnumSpecsFeaturesItems.FuelEfficiencyOverall,
                            EnumSpecsFeaturesItems.MaxPowerBhp,
                            EnumSpecsFeaturesItems.KerbWeight
                        };
                        BindMinSpecs(newLaunchesList, specsItemList);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("NewBikeLaunchesBL.GetBikes({0})", Newtonsoft.Json.JsonConvert.SerializeObject(filters)));
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
                if (makes != null && makes.Any())
                {
                    return makes.Where(PredicateSkipMakeId(skipMakeId));
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("NewBikeLaunchesBL.NewLaunchedMakeList({0})", skipMakeId));
            }
            return makes;
        }
        /// <summary>
        /// Created by  : Sanskar Gupta on 01st Feb 2018
        /// Description : Get Newly Launched bike list for a make and within a span of particular number of days.
        /// </summary>
        /// <param name="filters"></param>
        /// <returns></returns>
        public IEnumerable<NewLaunchedBikeEntityBase> GetNewLaunchedBikesListByMakeAndDays(InputFilter filters)
        {
            IEnumerable<NewLaunchedBikeEntityBase> bikes = null;
            try
            {
                bikes = _modelCache.GetNewLaunchedBikesList(filters.CityId);
                if(bikes != null)
                {
                    bikes = bikes.Where(x => x.Make.MakeId == filters.Make && DateTime.Now >= x.LaunchedOn && DateTime.Now <= x.LaunchedOn.AddDays(10));
                    var specsItemList = new List<EnumSpecsFeaturesItems>
                    {
                        EnumSpecsFeaturesItems.Displacement,
                        EnumSpecsFeaturesItems.FuelEfficiencyOverall,
                        EnumSpecsFeaturesItems.MaxPowerBhp,
                        EnumSpecsFeaturesItems.KerbWeight
                    };
                    BindMinSpecs(bikes, specsItemList);
                }

            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("NewBikeLaunchesBL.GetNewLaunchedBikesListByMakeAndDays({0})", Newtonsoft.Json.JsonConvert.SerializeObject(filters)));
            }

            return bikes;
        }


        #region Linq Predicates
        /// <summary>
        /// Created by  :   Sumit Kate on 10 Feb 2017
        /// Description :   Process Input Filters
        /// Modified by:- Subodh jain 09 march 2017
        ///summary :-  Added body type filter
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
                if (filters.BodyStyle > 0)
                {
                    filterExpression = filterExpression.And(PredicateBodyStyle(filters.BodyStyle));
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
        /// <summary>
        /// Created by  :   Subodh Jain 09 March 2017
        /// Description :   Predicate to match BodyStyle
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        private Expression<Func<NewLaunchedBikeEntityBase, bool>> PredicateBodyStyle(uint BodyStyleId)
        {
            return m => m.BodyStyleId == BodyStyleId;
        }
        #endregion

        /// <summary>
        /// Created by : Ashutosh Sharma on 11 Apr 2018.
        /// Description : Method to call specs features service and bind specs features data in bikeList object.
        /// </summary>
        /// <param name="bikesList">List of bikes object in which specs binding has to be done.</param>
        /// <param name="specItemList">List of specs ids for which specs data has to be done.</param>
        private void BindMinSpecs(IEnumerable<NewLaunchedBikeEntity> bikesList, IEnumerable<EnumSpecsFeaturesItems> specItemList)
        {
            try
            {
                if (bikesList != null && bikesList.Any())
                {
                    GetVersionSpecsByItemIdAdapter adapt1 = new GetVersionSpecsByItemIdAdapter();
                    var specItemInput = new VersionsDataByItemIds_Input
                    {
                        Versions = bikesList.Select(m => m.VersionId),
                        Items = specItemList
                    };
                    adapt1.AddApiGatewayCall(_apiGatewayCaller, specItemInput);
                    _apiGatewayCaller.Call();

                    IEnumerable<VersionMinSpecsEntity> specsResponseList = adapt1.Output;
                    if (specsResponseList != null)
                    {
                        var specsEnumerator = specsResponseList.GetEnumerator();
                        var bikesEnumerator = bikesList.GetEnumerator();
                        while (bikesEnumerator.MoveNext() && specsEnumerator.MoveNext())
                        {
                            bikesEnumerator.Current.MinSpecsList = specsEnumerator.Current.MinSpecsList;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("Bikewale.BAL.BikeData.BikeModels.BindMinSpecs_bikesList_{0}_specItemList_{1}", bikesList, specItemList));
            }
        }
        /// <summary>
        /// Created by : Ashutosh Sharma on 11 Apr 2018.
        /// Description : Method to call specs features service and bind specs features data in bikeList object.
        /// </summary>
        /// <param name="bikesList">List of bikes object in which specs binding has to be done.</param>
        /// <param name="specItemList">List of specs ids for which specs data has to be done.</param>
        private void BindMinSpecs(IEnumerable<NewLaunchedBikeEntityBase> bikesList, IEnumerable<EnumSpecsFeaturesItems> specItemList)
        {
            try
            {
                if (bikesList != null && bikesList.Any())
                {
                    GetVersionSpecsByItemIdAdapter adapt1 = new GetVersionSpecsByItemIdAdapter();
                    VersionsDataByItemIds_Input specItemInput = new VersionsDataByItemIds_Input
                    {
                        Versions = bikesList.Select(m => m.VersionId),
                        Items = specItemList
                    };
                    adapt1.AddApiGatewayCall(_apiGatewayCaller, specItemInput);
                    _apiGatewayCaller.Call();

                    IEnumerable<VersionMinSpecsEntity> specsResponseList = adapt1.Output;
                    if (specsResponseList != null)
                    {
                        var specsEnumerator = specsResponseList.GetEnumerator();
                        var bikesEnumerator = bikesList.GetEnumerator();
                        while (bikesEnumerator.MoveNext() && specsEnumerator.MoveNext())
                        {
                            bikesEnumerator.Current.MinSpecsList = specsEnumerator.Current.MinSpecsList;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("Bikewale.BAL.BikeData.NewLaunched.NewBikeLaunchesBL.BindMinSpecs_bikesList_{0}_specItemList_{1}", bikesList, specItemList));
            }
        }
    }
}
