using Bikewale.Entities.NewBikeSearch;
using Bikewale.Entities.UserReviews;
using Bikewale.Entities.UserReviews.Search;
using Bikewale.Interfaces.Pager;
using Bikewale.Interfaces.UserReviews;
using Bikewale.Interfaces.UserReviews.Search;
using Bikewale.Notifications;
using Bikewale.Utility;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bikewale.BAL.UserReviews.Search
{
    /// <summary>
    /// Created By : Sushil Kumar on 5th May 2017
    /// summary : Class have functions to process the raw UserReviews.Search bikes search filters and copy them into usable prcessedinputfilters class.
    /// </summary>
    public class UserReviewsSearch : IUserReviewsSearch
    {
        ProcessedInputFilters filterInputs = null;
        private readonly IPager _pager = null;
        private readonly IUserReviewsCache _userReviewsCache = null;

        /// <summary>
        /// Pass all dependencies from constructor
        /// </summary>
        /// <param name="searchQuery"></param>
        /// <param name="searchFilters"></param>
        /// <param name="searchRepo"></param>
        public UserReviewsSearch(IUserReviewsCache userReviewsCache, IPager pager)
        {
            _userReviewsCache = userReviewsCache;
            _pager = pager;
        }

        /// <summary>
        /// Created by Sajal Gupta on 11-09-2017
        /// Description : Function to skip top reviews from result set.
        /// </summary>
        /// <param name="inputFilters"></param>
        /// <param name="skipTopCount"></param>
        /// <returns></returns>
        public SearchResult GetUserReviewsList(InputFilters inputFilters, uint skipTopCount)
        {
            SearchResult objResult = null;
            try
            {
                objResult = GetUserReviewsList(inputFilters);

                if(objResult != null && objResult.Result != null && objResult.Result.Count() > skipTopCount)
                {
                    objResult.Result = objResult.Result.Skip((int)skipTopCount);
                    objResult.TotalCount = objResult.TotalCount - (int)skipTopCount;
                }
                else
                {
                    objResult = null;
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "GetUserReviewsList");
            }
            return objResult;
        }

        public SearchResult GetUserReviewsList(InputFilters inputFilters)
        {
            SearchResult objResult = null;
            try
            {
                objResult = new SearchResult();

                IEnumerable<uint> reviewIdList = null;

                ushort SortOrder = inputFilters.SO;

                uint modelId;
                uint.TryParse(inputFilters.Model, out modelId);

                BikeReviewIdListByCategory reviewIdLists = _userReviewsCache.GetReviewsIdListByModel(modelId);

                if (reviewIdLists != null)
                {
                    switch (SortOrder)
                    {
                        case (ushort)SortOrderEnum.RecentReviews:
                            {
                                reviewIdList = reviewIdLists.RecentReviews;
                                break;
                            }
                        case (ushort)SortOrderEnum.HelpfulReviews:
                            {
                                reviewIdList = reviewIdLists.HelpfulReviews;
                                break;
                            }
                        case (ushort)SortOrderEnum.PositiveReviews:
                            {
                                reviewIdList = reviewIdLists.PositiveReviews;
                                break;
                            }
                        case (ushort)SortOrderEnum.NegativeReviews:
                            {
                                reviewIdList = reviewIdLists.NegativeReviews;
                                break;
                            }
                        case (ushort)SortOrderEnum.NeutralReviews:
                            {
                                reviewIdList = reviewIdLists.NeutralReviews;
                                break;
                            }
                        default:
                            reviewIdList = reviewIdLists.RecentReviews;
                            break;
                    }

                    if (reviewIdList != null)
                    {
                        if (inputFilters.SkipReviewId > 0)
                        {
                            reviewIdList = reviewIdList.Where(x => x != inputFilters.SkipReviewId);
                        }

                        objResult.TotalCount = reviewIdList.Count();

                        int startingIndex = (inputFilters.PN - 1) * (inputFilters.PS) + 1;
                        int endingIndex = (inputFilters.PN) * (inputFilters.PS) < reviewIdList.Count() ? (inputFilters.PN) * (inputFilters.PS) : reviewIdList.Count();
                        int numberOfResults = endingIndex - startingIndex + 1;

                        reviewIdList = reviewIdList.Skip(startingIndex - 1).Take(numberOfResults);
                    }

                    IEnumerable<UserReviewSummary> objReviewSummaryList = _userReviewsCache.GetUserReviewSummaryList(reviewIdList);

                    if (objReviewSummaryList != null)
                    {
                        objReviewSummaryList = objReviewSummaryList.OrderBy(d => reviewIdList.ToList().FindIndex(m => m == d.ReviewId));
                    }

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

                    if (objReviewSummaryList != null)
                    {
                        foreach (var review in objReviewSummaryList)
                        {
                            if (review.Questions != null)
                            {
                                foreach (var ques in review.Questions)
                                {
                                    if (ques.Type == UserReviewQuestionType.Rating)
                                        review.RatingQuestionsCount++;
                                }
                            }

                        }
                    }

                    objResult.Result = objReviewSummaryList;
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "GetUserReviewsList");
            }
            return objResult;
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



