using Bikewale.Entities.NewBikeSearch;
using Bikewale.Entities.UserReviews;
using Bikewale.Entities.UserReviews.Search;
using Bikewale.Interfaces.Pager;
using Bikewale.Interfaces.UserReviews;
using Bikewale.Interfaces.UserReviews.Search;
using Bikewale.Notifications;
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

                if (objResult != null && objResult.Result != null && objResult.Result.Count() > skipTopCount)
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
                ErrorClass.LogError(ex, "GetUserReviewsList");
            }
            return objResult;
        }

        /// <summary>
        /// Modified By : Deepak Israni on 2 May 2018
        /// Description : Sanitized description and added to sanitized description field when it turns up null.
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public SearchResult GetUserReviewsList(ReviewDataCombinedFilter filter)
        {
            SearchResult objResult = null;
            try
            {
                objResult = GetUserReviewsList(filter.InputFilter);

                if (objResult != null && objResult.Result != null && objResult.TotalCount > 0 && filter.ReviewFilter != null)
                    foreach (var review in objResult.Result)
                    {
                        if (review.Questions != null && !filter.ReviewFilter.RatingQuestion)
                        {
                            review.Questions = review.Questions.Where(x => x.Type != UserReviewQuestionType.Rating);
                            review.RatingQuestionsCount = 0;
                        }

                        if (review.Questions != null && !filter.ReviewFilter.ReviewQuestion)
                        {
                            review.Questions = review.Questions.Where(x => x.Type != UserReviewQuestionType.Review);

                        }

                        if (!filter.ReviewFilter.BasicDetails)
                        {
                            review.OverallRating = null;
                            review.Title = null;
                            review.ReviewAge = null;
                            review.Make = null;
                            review.Model = null;
                            review.OriginalImagePath = null;
                            review.HostUrl = null;
                        }

                        if (string.IsNullOrEmpty(review.SanitizedDescription) && !string.IsNullOrEmpty(review.Description))
                        {
                            review.SanitizedDescription = System.StringHtmlHelpers.RemoveHtmlWithSpaces(review.Description);
                        }

                        if (filter.ReviewFilter.SantizeHtml)
                        {
                            review.SanitizedDescription = (review.SanitizedDescription != null) && (review.SanitizedDescription.Length > filter.ReviewFilter.SanitizedReviewLength) ? review.SanitizedDescription.Substring(0, (int)filter.ReviewFilter.SanitizedReviewLength) : review.SanitizedDescription;
                        }
                        else
                        {
                            review.SanitizedDescription = null;
                        }
                        if (!filter.ReviewFilter.IsDescriptionRequired)
                        {
                            review.Description = null;
                        }

                    }

            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "GetUserReviewsList");
            }
            return objResult;
        }

        /// <summary>
        /// Modified By : Deepak Israni on 2 May 2018
        /// Description : Sanitized description and added to sanitized description field when it turns up null.
        /// </summary>
        /// <param name="inputFilters"></param>
        /// <returns></returns>
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

                            if (string.IsNullOrEmpty(review.SanitizedDescription) && !string.IsNullOrEmpty(review.Description))
                            {
                                review.SanitizedDescription = System.StringHtmlHelpers.RemoveHtmlWithSpaces(review.Description);
                            }

                        }
                    }

                    objResult.Result = objReviewSummaryList;
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "GetUserReviewsList");
            }
            return objResult;
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
                totalPageCount = _pager.GetTotalPages(totalRecordCount, Convert.ToInt32(objFilters.PS));
                if (totalPageCount > 0)
                {
                    string controllerurl = "/api/user-reviews/search/?";

                    currentPageNo = (objFilters.PN == 0) ? 1 : objFilters.PN;
                    if (currentPageNo == totalPageCount)
                        objPager.NextPageUrl = string.Empty;
                    else
                    {
                        string apiUrlStrforNext = GetApiUrl(objFilters, 1);
                        objPager.NextPageUrl = controllerurl + apiUrlStrforNext;
                    }

                    if (objFilters.PN == 1 || objFilters.PN == 0)
                        objPager.PrevPageUrl = string.Empty;
                    else
                    {
                        string apiUrlStrforPrev = GetApiUrl(objFilters, -1);
                        objPager.PrevPageUrl = controllerurl + apiUrlStrforPrev;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.BAL.UserReviewsSearch.GetPrevNextUrl");

            }
            return objPager;
        }
        /// <summary>
        /// Created By: Aditi Srivastava on 16 Sep 2016
        /// Description: Set api url parameters according to filters for UserReviewsSearch bikes
        /// </summary>
        /// <param name="objFilters"></param>
        /// <returns></returns>
        private string GetApiUrl(InputFilters objFilters, short PageValue)
        {
            string apiUrlstr = string.Empty;
            try
            {

                if (!String.IsNullOrEmpty(objFilters.Make))
                    apiUrlstr += "&make=" + objFilters.Make.Replace(" ", "+");
                if (!String.IsNullOrEmpty(objFilters.Model))
                    apiUrlstr += "&model=" + objFilters.Model.Replace(" ", "+");
                apiUrlstr += "&ps=" + objFilters.PS;
                apiUrlstr += "&pn=" + (objFilters.PN + PageValue);

                if (!String.IsNullOrEmpty(objFilters.CAT))
                    apiUrlstr += "&cat=" + objFilters.CAT;

                apiUrlstr += "&so=" + objFilters.SO;

            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.BAL.UserReviewsSearch.GetApiUrl");

            }
            return apiUrlstr;
        }

    }   // Class

}   // namespace



