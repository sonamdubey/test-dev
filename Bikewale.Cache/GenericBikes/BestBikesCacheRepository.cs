﻿using Bikewale.Entities.GenericBikes;
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
                objErr.SendMail();
            }
            return objSearchList;
        }
    }
}
