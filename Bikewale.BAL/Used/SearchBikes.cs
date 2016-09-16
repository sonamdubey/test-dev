using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bikewale.Entities.Used.Search;
using Bikewale.Interfaces.Used.Search;
using Bikewale.Notifications;

namespace Bikewale.BAL.Used.Search
{
    /// <summary>
    /// Created By : Ashish G. Kamble on 11 Sept 2016
    /// Summary : class have all business logic to get the used bikes search
    /// </summary>
    public class SearchBikes : ISearch
    {
        ISearchFilters _searchFilters = null;
        ISearchQuery _searchQuery = null;
        ISearchRepository _searchRepo = null;

        /// <summary>
        /// Pass all dependencies from constructor
        /// </summary>
        /// <param name="searchQuery"></param>
        /// <param name="searchFilters"></param>
        /// <param name="searchRepo"></param>
        public SearchBikes(ISearchFilters searchFilters, ISearchQuery searchQuery, ISearchRepository searchRepo)
        {
            _searchFilters = searchFilters;
            _searchQuery = searchQuery;
            _searchRepo = searchRepo;
        }

        /// <summary>
        /// Function to get the used bikes seach. This encapsulates all the business logic to get the search result.
        /// </summary>
        /// <param name="inputFilters">All input filters from the user</param>
        /// <returns></returns>
        public SearchResult GetUsedBikesList(InputFilters inputFilters)
        {
            SearchResult objResult = null;

            try
            {
                // Process all filters and get the search query
                string searchQuery = _searchQuery.GetSearchResultQuery(inputFilters);

                // Get search result from database
                if (!string.IsNullOrEmpty(searchQuery))
                    objResult = _searchRepo.GetUsedBikesList(searchQuery);

                // Get pager entity and populate it in search result
            }
            catch (Exception ex)
            {
                ErrorClass objError = new ErrorClass(ex, "Bikewale.BAL.Used.GetUsedBikesList");
                objError.SendMail();
            }

            return objResult;
        }
    }
}
