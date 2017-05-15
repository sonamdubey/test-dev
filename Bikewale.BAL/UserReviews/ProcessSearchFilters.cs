using Bikewale.Entities.NewBikeSearch;
using Bikewale.Entities.UserReviews.Search;
using Bikewale.Interfaces.Pager;
using Bikewale.Interfaces.UserReviews;
using Bikewale.Interfaces.UserReviews.Search;
using Bikewale.Notifications;
using Bikewale.Utility;
using Microsoft.Practices.Unity;
using System;

namespace Bikewale.BAL.UserReviews.Search
{
    /// <summary>
    /// Created By : Sushil Kumar on 5th May 2017
    /// summary : Class have functions to process the raw UserReviews.Search bikes search filters and copy them into usable prcessedinputfilters class.
    /// </summary>
    public class UserReviewsSearch : IUserReviewsSearch
    {
        ProcessedInputFilters filterInputs = null;
        private string whereClause = string.Empty;
        private uint _maxRecords = 24;

        private readonly IPager _pager = null;
        private readonly IUserReviewsRepository _objUserReviewrRepo = null;

        /// <summary>
        /// Pass all dependencies from constructor
        /// </summary>
        /// <param name="searchQuery"></param>
        /// <param name="searchFilters"></param>
        /// <param name="searchRepo"></param>
        public UserReviewsSearch(IPager pager, IUserReviewsRepository objUserReviewrRepo)
        {
            _pager = pager;
            _objUserReviewrRepo = objUserReviewrRepo;
        }


        public SearchResult GetUserReviewsList(InputFilters inputFilters)
        {
            SearchResult objResult = null;

            try
            {
                // Process all filters and get the search query
                string searchQuery = GetSearchResultQuery(inputFilters);

                // Get search result from database
                objResult = _objUserReviewrRepo.GetUserReviewsList(searchQuery);

                if (objResult != null)
                {
                    SetpaginationProperties(objResult, inputFilters);
                }

            }
            catch (Exception ex)
            {
                ErrorClass objError = new ErrorClass(ex, "Bikewale.BAL.UserReviewsSearch.GetUserReviewsSearchBikesList");

            }

            return objResult;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inputFilters"></param>
        /// <returns></returns>
        private string GetSearchResultQuery(InputFilters inputFilters)
        {
            string searchQuery = string.Empty;
            bool SkipRecordsLimit = (inputFilters.PN * inputFilters.PS) > _maxRecords;

            InitSearchQuery(inputFilters);

            try
            {
                searchQuery = string.Format(@" select sql_calc_found_rows {0}
                                               from {1} 
                                               where {2}
                                               order by {3} limit {4},{5};
                                            
                                               select found_rows() as RecordCount;"
                    , GetSelectClause(), GetFromClause(), GetWhereClause(), GetOrderByClause(), (SkipRecordsLimit) ? filterInputs.StartIndex - 1 : 0, (SkipRecordsLimit) ? filterInputs.PageSize : (int)_maxRecords);
            }
            catch (Exception ex)
            {
                ErrorClass objError = new ErrorClass(ex, "Bikewale.BAL.UserReviewsSearch.SearchQuery.GetSearchResultQuery");

            }

            return searchQuery;
        }


        private void SetpaginationProperties(SearchResult objResult, InputFilters inputFilters)
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


        /// <summary>
        /// Function to process all filters and populate the data into output entity
        /// </summary>
        /// <param name="objFilters">Input raw filters data</param>
        /// <returns>Returns processed filters into ProcessedInputFilters entity</returns>
        private ProcessedInputFilters ProcessFilters(InputFilters objFilters)
        {
            filterInputs = new ProcessedInputFilters();

            try
            {
                filterInputs.Reviews = objFilters.Reviews;
                filterInputs.Ratings = objFilters.Ratings;

                if (!string.IsNullOrEmpty(objFilters.Make))
                    ProcessMakes(objFilters.Make);

                if (!string.IsNullOrEmpty(objFilters.Model))
                    ProcessModels(objFilters.Model);

                if (!string.IsNullOrEmpty(objFilters.CAT))
                    ProcessCategories(objFilters.CAT);

                if (objFilters.SkipReviewId > 0)
                {
                    filterInputs.SkipReviewId = objFilters.SkipReviewId;
                }

                filterInputs.SortOrder = objFilters.SO;

                ProcessPaging(objFilters.PN, objFilters.PS);
            }
            catch (Exception ex)
            {
                ErrorClass objError = new ErrorClass(ex, "Bikewale.BAL.UserReviews.Search.ProcessFilters");

            }

            return filterInputs;
        }



        /// <summary>
        /// Function to filter the makes list and populate it into array
        /// </summary>
        private void ProcessMakes(string make)
        {
            try
            {
                filterInputs.Make = make.Split('+');
            }
            catch (Exception ex)
            {
                ErrorClass objError = new ErrorClass(ex, "Bikewale.BAL.UserReviews.Search.ProcessMakes");

            }
        }

        /// <summary>
        /// Function to filter the models list and populate it into array
        /// </summary>
        private void ProcessModels(string model)
        {
            try
            {
                filterInputs.Model = model.Split('+');
            }
            catch (Exception ex)
            {
                ErrorClass objError = new ErrorClass(ex, "Bikewale.BAL.UserReviews.Search.ProcessModels");

            }
        }

        /// <summary>
        /// Process type of sellers
        /// </summary>
        private void ProcessCategories(string category)
        {
            try
            {
                filterInputs.Category = category.Split('+');
            }
            catch (Exception ex)
            {
                ErrorClass objError = new ErrorClass(ex, "Bikewale.BAL.UserReviews.Search.ProcessCategories");

            }
        }

        /// <summary>
        /// Function to process the page filters
        /// </summary>
        private void ProcessPaging(int pgNo, int pgSize)
        {
            int startIndex = 0, endIndex = 0;
            try
            {
                // If page no is not valid, then consider this as a first page
                pgNo = pgNo <= 0 ? 1 : pgNo;

                // If page size is not passed then take the default page size
                pgSize = pgSize <= 0 ? Convert.ToInt32(Utility.BWConfiguration.Instance.PageSize) : pgSize;

                using (IUnityContainer container = new UnityContainer())
                {
                    Paging.GetStartEndIndex(pgSize, pgNo, out startIndex, out endIndex);

                    filterInputs.StartIndex = startIndex;
                    filterInputs.EndIndex = endIndex;
                    filterInputs.PageNo = pgNo;
                    filterInputs.PageSize = pgSize;
                }
            }
            catch (Exception ex)
            {
                ErrorClass objError = new ErrorClass(ex, "Bikewale.BAL.UserReviews.Search.ProcessPaging");
            }
        }


        /// <summary>
        /// Function to initialize the where clause and get the processed filters data to create a query
        /// </summary>
        /// <param name="inputFilters">raw input filters</param>
        private void InitSearchQuery(InputFilters inputFilters)
        {
            try
            {
                this.filterInputs = ProcessFilters(inputFilters);

                // Do not change the sequence
                ApplyUserSearchType();
                ApplyBikeFilter();
                ApplyReviewTypeFilter();
                SkipReviewId();
            }
            catch (Exception ex)
            {
                ErrorClass objError = new ErrorClass(ex, "Bikewale.BAL.UserReviewsSearch.SearchQuery.InitSearchQuery");

            }
        }

        private void SkipReviewId()
        {
            if (filterInputs.SkipReviewId > 0)
            {
                whereClause += string.Format(" and ur.id <> {0} ", filterInputs.SkipReviewId);
            }

        }

        private void ApplyUserSearchType()
        {
            if (!filterInputs.Ratings && filterInputs.Reviews)
            {
                whereClause += " and (title is not null or title <> '') ";
            }
        }


        /// <summary>
        /// Function to get the make  and model filter clause
        /// </summary>
        private void ApplyBikeFilter()
        {
            string makeList = string.Empty, modelList = string.Empty, makeFilter = string.Empty, modelFilter = string.Empty;

            try
            {
                if (filterInputs.Make != null && filterInputs.Make.Length > 0)
                {
                    foreach (string str in filterInputs.Make)
                    {
                        makeList += "," + str;
                    }

                    makeList = makeList.Substring(1);

                    makeFilter = string.Format(" ur.makeid in ({0}) ", makeList);
                }

                if (filterInputs.Model != null && filterInputs.Model.Length > 0)
                {
                    foreach (string str in filterInputs.Model)
                    {
                        modelList += "," + str;
                    }

                    modelList = modelList.Substring(1);

                    modelFilter = string.Format(" ur.modelid in ({0}) ", modelList);
                }

                if (!String.IsNullOrEmpty(makeFilter) && !String.IsNullOrEmpty(modelFilter))
                    whereClause += string.Format(" and ( {0} or {1} ) ", makeFilter, modelFilter);
                else if (!String.IsNullOrEmpty(makeFilter))
                    whereClause += string.Format(" and {0} ", makeFilter);
                else if (!String.IsNullOrEmpty(modelFilter))
                    whereClause += string.Format(" and {0} ", modelFilter);

            }
            catch (Exception ex)
            {
                ErrorClass objError = new ErrorClass(ex, "Bikewale.BAL.UserReviewsSearch.SearchQuery.ApplyBikeFilter");
            }
        }


        /// <summary>
        /// Function to get the seller types filter in the query
        /// </summary>
        private void ApplyReviewTypeFilter()
        {
            string reviewType = string.Empty;

            try
            {
                if (filterInputs.Category != null && filterInputs.Category.Length > 0)
                {
                    foreach (string str in filterInputs.Category)
                    {
                        reviewType += "," + str;
                    }

                    reviewType = reviewType.Substring(1);

                    whereClause += string.Format(" and ur.overallratingid in ({0}) ", reviewType);
                }
            }
            catch (Exception ex)
            {
                ErrorClass objError = new ErrorClass(ex, "Bikewale.BAL.UserReviewsSearch.SearchQuery.ApplySellerTypeFilter");
            }
        }

        /// <summary>
        /// Function to get the select clause for the search query
        /// Modified by :   Sumit Kate on 25 Oct 2016
        /// Description :   Added LastUpdated in select clause
        /// </summary>
        /// <returns></returns>
        private string GetSelectClause()
        {
            string selectClause = string.Empty;

            try
            {
                selectClause = @" id as reviewid, 
                customername as writtenby,
                title as reviewtitle,
                substring(striphtml(review),1,175) comments,
                ifnull(ur.entrydate,ur.lastmoderateddate) reviewdate,
                upvotes liked,
                downvotes disliked,
                views viewed,
                overallratingid overallrating,
                if(views >= 100,1,2) bucket,
                if(views >= 100,(upvotes - downvotes) / views ,upvotes) as helpfulness ";
            }
            catch (Exception ex)
            {
                ErrorClass objError = new ErrorClass(ex, "Bikewale.BAL.UserReviewsSearch.SearchQuery.GetSelectClause");
            }

            return selectClause;
        }

        /// <summary>
        /// Function to get the from clause
        /// </summary>
        /// <returns></returns>
        private string GetFromClause()
        {
            string fromClause = string.Empty;

            try
            {
                fromClause = @" userreviews as ur ";
            }
            catch (Exception ex)
            {
                ErrorClass objError = new ErrorClass(ex, "Bikewale.BAL.UserReviewsSearch.SearchQuery.GetFromClause");
            }

            return fromClause;
        }

        /// <summary>
        /// Function to get the where clause
        /// </summary>
        /// <returns></returns>
        private string GetWhereClause()
        {
            if (!string.IsNullOrEmpty(whereClause))
                whereClause = string.Format(" isactive=1 and status=2 {0} ", whereClause);

            return whereClause;
        }

        /// <summary>
        /// Function to get the order by clause
        /// Modified by :   Sumit Kate on 25 Oct 2016
        /// Description :   For Default sort, show listing with images of that day first followed by listing without image
        /// </summary>
        /// <returns></returns>
        private string GetOrderByClause()
        {
            string orderBy = string.Empty;

            try
            {
                switch (filterInputs.SortOrder)
                {
                    case 1:
                        orderBy = " ifnull(ur.entrydate,ur.lastmoderateddate) desc "; //most recent
                        break;
                    case 2:
                    case 5:
                    case 6:
                    case 7:
                        orderBy = " bucket, helpfulness desc "; //most helpful
                        break;
                    case 3:
                        orderBy = " ur.views desc "; //most read or viewed
                        break;
                    case 4:
                        orderBy = " ur.overallratingid desc "; //most rated 
                        break;
                    default:
                        orderBy = " ifnull(ur.entrydate,ur.lastmoderateddate) desc "; //most recent
                        break;
                }
            }
            catch (Exception ex)
            {
                ErrorClass objError = new ErrorClass(ex, "Bikewale.BAL.NewBikeSearch.SearchQuery.GetOrderByClause");
            }
            return orderBy;
        }



        /// <summary>
        /// Created By: Aditi Srivastava on 16 Sep 2016
        /// Description: Set previous and next page urls for api result of UserReviewsSearch bikes
        /// </summary>
        /// <param name="filterInputs"></param>
        /// <param name="totalRecordCount"></param>
        /// <param name="fetchedCount"></param>
        /// <returns></returns>
        private PagingUrl GetPrevNextUrl(InputFilters objFilters, int totalRecordCount)
        {

            PagingUrl objPager = null;
            int totalPageCount = 0;
            int currentPageNo = 0;
            try
            {
                objPager = new PagingUrl();
                string apiUrlStr = GetApiUrl(objFilters);
                totalPageCount = _pager.GetTotalPages(totalRecordCount, Convert.ToInt32(objFilters.PS));

                if (totalPageCount > 0)
                {
                    string controllerurl = "/api/user-reviews/search/?";

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
                ErrorClass objErr = new ErrorClass(ex, "Bikewale.BAL.UserReviewsSearch.GetPrevNextUrl");

            }
            return objPager;
        }
        /// <summary>
        /// Created By: Aditi Srivastava on 16 Sep 2016
        /// Description: Set api url parameters according to filters for UserReviewsSearch bikes
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
                apiUrlstr += "&ps=" + objFilters.PS;
                apiUrlstr += "&pn=" + objFilters.PN;

                if (!String.IsNullOrEmpty(objFilters.CAT))
                    apiUrlstr += "&cat=" + objFilters.CAT;

                apiUrlstr += "&so=" + objFilters.SO;

            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Bikewale.BAL.UserReviewsSearch.GetApiUrl");

            }
            return apiUrlstr;
        }

    }   // Class

}   // namespace



