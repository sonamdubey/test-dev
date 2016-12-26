using Bikewale.Entities.GenericBikes;
using Bikewale.Entities.NewBikeSearch;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.GenericBikes;
using Bikewale.Interfaces.NewBikeSearch;
using Bikewale.Notifications;
using Bikewale.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
namespace Bikewale.Cache.GenericBikes
{
    /// <summary>
    /// Created by  :   Sumit Kate on 26 Dec 2016
    /// Description :   Top 10 Bikes Cache Repository
    /// </summary>
    public class BestBikesCacheRepository : IBestBikesCacheRepository
    {
        private readonly ICacheManager _cache;
        private readonly ISearchResult _searchResult = null;
        private readonly IProcessFilter _processFilter = null;

        /// <summary>
        /// Created by  :   Sumit Kate on 26 Dec 2016
        /// Description :   Constructor to intialize the member variables
        /// </summary>
        /// <param name="cache"></param>
        /// <param name="searchResult"></param>
        /// <param name="processFilter"></param>
        public BestBikesCacheRepository(ICacheManager cache, ISearchResult searchResult, IProcessFilter processFilter)
        {
            _searchResult = searchResult;
            _processFilter = processFilter;
            _cache = cache;
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 26 Dec 2016
        /// Description :   Calls DAL via Cache layer
        /// </summary>
        /// <param name="bodyStyle"></param>
        /// <param name="filterInputs"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        public IEnumerable<BestBikeEntityBase> BestBikesByType(EnumBikeBodyStyles bodyStyle, FilterInput filterInputs, InputBaseEntity input)
        {
            string key = string.Format("BW_GenericBikes_{0}", (int)bodyStyle);
            SearchOutputEntity objSearchList = null;
            IEnumerable<BestBikeEntityBase> bikes = null;
            try
            {
                objSearchList = _cache.GetFromCache<SearchOutputEntity>(key, new TimeSpan(0, 30, 0), () => _searchResult.GetSearchResult(filterInputs, input));
                if (objSearchList != null && objSearchList.TotalCount > 0)
                {
                    DateTime startOfTime = new DateTime();

                    var b = from bike in objSearchList.SearchResult
                            select new BestBikeEntityBase()
                            {
                                BikeName = bike.BikeName,
                                Model = bike.BikeModel,
                                Make = bike.BikeModel.MakeBase,
                                HostUrl = bike.BikeModel.HostUrl,
                                OriginalImagePath = bike.BikeModel.OriginalImagePath,
                                MinSpecs = new Entities.BikeData.MinSpecsEntity()
                                {
                                    Displacement = bike.Displacement,
                                    FuelEfficiencyOverall = bike.FuelEfficiency,
                                    KerbWeight = bike.KerbWeight,
                                    MaximumTorque = bike.MaximumTorque,
                                    MaxPower = SqlReaderConvertor.ToNullableFloat(bike.Power)
                                },
                                Price = SqlReaderConvertor.ParseToUInt32(bike.BikeModel.MinPrice),
                                SmallModelDescription = bike.SmallDescription,
                                FullModelDescription = bike.FullDescription,
                                LaunchDate = (!bike.LaunchedDate.Equals(startOfTime) ? bike.LaunchedDate : default(Nullable<DateTime>)),
                                PhotosCount = bike.PhotoCount,
                                VideosCount = bike.VideoCount,
                                UnitsSold = bike.UnitsSold,
                                TotalVersions = bike.VersionCount,
                                TotalModelColors = bike.ColorCount
                            };
                    bikes = b.ToList();
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "DealerCacheRepository.GetDealerByMakeCity");
            }
            return bikes;
        }
    }
}
