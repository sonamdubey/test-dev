using System;
using Bikewale.DTO.NewBikeSearch;
using Bikewale.Entities.NewBikeSearch;
using Bikewale.Interfaces.NewBikeSearch;
using Bikewale.Notifications;

namespace Bikewale.BAL.BikeSearch
{
    /// <summary>
    /// Created by : Vivek Singh Tomar on 16th Nov 2017
    /// Summary : BAL for Bike Search Result
    /// </summary>
    public class BikeSearchResult : IBikeSearchResult
    {
        private readonly ISearchResult _searchResult;
        public BikeSearchResult(ISearchResult searchResult)
        {
            _searchResult = searchResult;
        }

        /// <summary>
        /// Created by : Vivek Singh Tomar on 16th Nov 2017
        /// Summary : Fetch bikes for given input query
        /// </summary>
        /// <param name="filterInputs"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        public SearchOutput GetSearchResult(FilterInput filterInputs, InputBaseEntity input)
        {
            SearchOutput searchResult = null;
            try
            {
                SearchOutputEntity objSearchEntity = _searchResult.GetSearchResult(filterInputs, input);
                searchResult = SearchOutputMapper.Convert(objSearchEntity);

            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Bikewale.BAL.BikeSearchResult.GetSearchResult");
            }
            return searchResult;
        }
    }
}
