using Bikewale.Entities.GenericBikes;
using Bikewale.Entities.NewBikeSearch;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.GenericBikes;
using Bikewale.Interfaces.NewBikeSearch;
using Bikewale.Notifications;
using System;
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
        private readonly IGenericBikeRepository _genericBike = null;

        /// <summary>
        /// Created by  :   Sumit Kate on 26 Dec 2016
        /// Description :   Constructor to intialize the member variables
        /// Modified By : Sushil Kumar on 2nd Jan 2016
        /// Description : Addded new interface input parameter for generic bike info
        /// </summary>
        /// <param name="cache"></param>
        /// <param name="searchResult"></param>
        /// <param name="processFilter"></param>
        public BestBikesCacheRepository(ICacheManager cache, ISearchResult searchResult, IProcessFilter processFilter, IGenericBikeRepository genericBike)
        {
            _searchResult = searchResult;
            _processFilter = processFilter;
            _cache = cache;
            _genericBike = genericBike;
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 26 Dec 2016
        /// Description :   Calls DAL via Cache layer
        /// </summary>
        /// <param name="bodyStyle"></param>
        /// <param name="filterInputs"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        public SearchOutputEntity BestBikesByType(EnumBikeBodyStyles bodyStyle, FilterInput filterInputs, InputBaseEntity input)
        {
            string key = string.Format("BW_GenericBikes_{0}", (int)bodyStyle);
            SearchOutputEntity objSearchList = null;
            try
            {
                objSearchList = _cache.GetFromCache<SearchOutputEntity>(key, new TimeSpan(0, 30, 0), () => _searchResult.GetSearchResult(filterInputs, input));

            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, string.Format("BestBikesCacheRepository.BestBikesByType_BodyStyle_{0}", bodyStyle));
            }
            return objSearchList;
        }

        /// <summary>
        /// Created by  :   Sushil Kumar on 2nd Jan 2016
        /// Description :   Calls DAL via Cache layer for generic bike info
        /// </summary>
        /// <param name="modelId"></param>
        /// <returns></returns>
        public GenericBikeInfo GetGenericBikeInfo(uint modelId)
        {
            string key = string.Format("BW_GenericBikeInfo_MO_{0}", modelId);
            GenericBikeInfo objSearchList = null;
            try
            {
                objSearchList = _cache.GetFromCache<GenericBikeInfo>(key, new TimeSpan(0, 30, 0), () => _genericBike.GetGenericBikeInfo(modelId));

            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, string.Format("BestBikesCacheRepository.GetGenericBikeInfo_{0}", modelId));
            }
            return objSearchList;
        }
    }
}
