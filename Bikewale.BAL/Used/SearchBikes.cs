using Bikewale.Entities.NewBikeSearch;
using Bikewale.Entities.Used.Search;
using Bikewale.Interfaces.Pager;
using Bikewale.Interfaces.Used.Search;
using Bikewale.Notifications;
using Microsoft.Practices.Unity;
using System;

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
        /// Modified by: Aditi Srivastava on 16 Sep 2016
        /// Description: Set the paging url and current page no for api response
        /// Modified by: Sushil Kumar on 22 Sep 2016
        /// Description: Add null check for objResult
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
                objResult = _searchRepo.GetUsedBikesList(searchQuery);

                if (objResult != null)
                {
                    // Set paging url and current page numbers
                    objResult.PageUrl = GetPrevNextUrl(inputFilters, objResult.TotalCount);
                    objResult.CurrentPageNo = inputFilters.PN;
                    if (!String.IsNullOrEmpty(objResult.PageUrl.PrevPageUrl))
                    {
                        objResult.PageUrl.PrevPageUrl = objResult.PageUrl.PrevPageUrl.Replace("+", "%2b");
                    }
                    if (!String.IsNullOrEmpty(objResult.PageUrl.NextPageUrl))
                    {
                        objResult.PageUrl.NextPageUrl = objResult.PageUrl.NextPageUrl.Replace("+", "%2b");
                    }
                }

            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.BAL.Used.GetUsedBikesList");
                
            }

            return objResult;
        }
        /// <summary>
        /// Created By: Aditi Srivastava on 16 Sep 2016
        /// Description: Set previous and next page urls for api result of used bikes
        /// </summary>
        /// <param name="filterInputs"></param>
        /// <param name="totalRecordCount"></param>
        /// <param name="fetchedCount"></param>
        /// <returns></returns>
        private PagingUrl GetPrevNextUrl(InputFilters objFilters, int totalRecordCount)
        {
            IPager Pager = GetPager();
            PagingUrl objPager = null;
            int totalPageCount = 0;
            int currentPageNo = 0;
            try
            {
                objPager = new PagingUrl();
                string apiUrlStr = GetApiUrl(objFilters);
                totalPageCount = Pager.GetTotalPages(totalRecordCount, Convert.ToInt32(objFilters.PS));

                if (totalPageCount > 0)
                {
                    string controllerurl = "/api/used/search/?";

                    currentPageNo = (objFilters.PN == 0) ? 1 : objFilters.PN;
                    if (currentPageNo == totalPageCount)
                        objPager.NextPageUrl = string.Empty;
                    else
                    {
                        objPager.NextPageUrl = controllerurl + apiUrlStr;
                    }

                    if (objFilters.PN == 1 || objFilters.PN == 0)
                        objPager.PrevPageUrl = string.Empty;
                    else
                        objPager.PrevPageUrl = controllerurl + apiUrlStr;
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.BAL.Used.GetPrevNextUrl");
                
            }
            return objPager;
        }
        /// <summary>
        /// Created By: Aditi Srivastava on 16 Sep 2016
        /// Description: Set api url parameters according to filters for used bikes
        /// </summary>
        /// <param name="objFilters"></param>
        /// <returns></returns>
        private string GetApiUrl(InputFilters objFilters)
        {
            string apiUrlstr = string.Empty;
            try
            {

                if (!String.IsNullOrEmpty(objFilters.Make))
                    apiUrlstr += "&make=" + objFilters.Make.Replace(" ", "+");
                if (!String.IsNullOrEmpty(objFilters.Model))
                    apiUrlstr += "&model=" + objFilters.Model.Replace(" ", "+");
                if (!String.IsNullOrEmpty(objFilters.Budget))
                    apiUrlstr += "&budget=" + objFilters.Budget.Replace(" ", "+");
                if (!String.IsNullOrEmpty(objFilters.Kms))
                    apiUrlstr += "&kms=" + objFilters.Kms.Replace(" ", "+");
                if (!String.IsNullOrEmpty(objFilters.Age))
                    apiUrlstr += "&age=" + objFilters.Age.Replace(" ", "+");
                if (!String.IsNullOrEmpty(objFilters.Owner))
                    apiUrlstr += "&owner=" + objFilters.Owner.Replace(" ", "+");
                apiUrlstr += "&ps=" + objFilters.PS;
                apiUrlstr += "&pn=" + objFilters.PN;

                if (!String.IsNullOrEmpty(objFilters.ST))
                    apiUrlstr += "&st=" + objFilters.ST;

                apiUrlstr += "&so=" + objFilters.SO;

            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.BAL.Used.GetApiUrl");
                
            }
            return apiUrlstr;
        }
        private IPager GetPager()
        {
            IPager _objPager = null;
            using (IUnityContainer container = new UnityContainer())
            {
                container.RegisterType<IPager, Bikewale.BAL.Pager.Pager>();
                _objPager = container.Resolve<IPager>();
            }
            return _objPager;
        }
    }
}
