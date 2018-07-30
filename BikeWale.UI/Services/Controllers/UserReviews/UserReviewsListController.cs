using Bikewale.DTO.UserReviews;
using Bikewale.Entities.DTO;
using Bikewale.Entities.UserReviews;
using Bikewale.Interfaces.UserReviews;
using Bikewale.Interfaces.UserReviews.Search;
using Bikewale.Notifications;
using Bikewale.Service.AutoMappers.Make;
using Bikewale.Service.AutoMappers.Model;
using Bikewale.Service.AutoMappers.UserReviews;
using Bikewale.Service.Utilities;
using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Description;

namespace Bikewale.Service.Controllers.UserReviews
{
    /// <summary>
    /// To List of User Reviews based on tagging and pagination
    /// Author : Sushil Kumar
    /// Created On : 24th August 2015
    /// Modified by :   Sumit Kate on 18 May 2016
    /// Description :   Extend from CompressionApiController instead of ApiController 
    /// </summary>
    public class UserReviewsListController : CompressionApiController//ApiController
    {

        private readonly IUserReviewsRepository _userReviewsRepo = null;
        private readonly IUserReviewsCache _userReviewsCacheRepo = null;
        private readonly IUserReviews _userReviews = null;
        private readonly IUserReviewsSearch _userReviewsSearch = null;
        public UserReviewsListController(IUserReviewsRepository userReviewsRepo, IUserReviewsCache userReviewsCacheRepo, IUserReviews userReviews, IUserReviewsSearch userReviewsSearch)
        {
            _userReviewsRepo = userReviewsRepo;
            _userReviewsCacheRepo = userReviewsCacheRepo;
            _userReviews = userReviews;
            _userReviewsSearch = userReviewsSearch;
        }

        #region Get Reviewed Bike List
        /// <summary>
        /// To get all reviewed Bikes List
        /// </summary>
        /// <returns></returns>
        [ResponseType(typeof(IEnumerable<ReviewTaggedBike>))]
        public IHttpActionResult Get()
        {
            List<ReviewTaggedBikeEntity> objUserReview = null;
            List<ReviewTaggedBike> objDTOUserReview = null;
            try
            {
                objUserReview = _userReviewsRepo.GetReviewedBikesList();

                if (objUserReview != null)
                {
                    // Auto map the properties
                    objDTOUserReview = UserReviewsMapper.Convert(objUserReview);

                    return Ok(objDTOUserReview);
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Exception : Bikewale.Service.UserReviews.UserReviewsListController");
                return InternalServerError();
            }

            return NotFound();
        }   // Get 
        #endregion
        

        /// <summary>
        /// To get user reviews
        /// </summary>
        /// <param name="filters"></param>
        /// <returns></returns>
        [Route("api/user-reviews/search/")]
        public IHttpActionResult GetUserReviewList([FromUri]Bikewale.Entities.UserReviews.Search.InputFilters filters)
        {
            Bikewale.Entities.UserReviews.Search.SearchResult objUserReviews = null;
            Bikewale.DTO.UserReviews.Search.SearchResult objDTOUserReview = null;
            try
            {
                if (filters != null && (!String.IsNullOrEmpty(filters.Model) || !String.IsNullOrEmpty(filters.Make)))
                {
                    objUserReviews = _userReviewsSearch.GetUserReviewsList(filters);
                    if (objUserReviews != null)
                    {
                        objDTOUserReview = UserReviewsMapper.Convert(objUserReviews);
                        return Ok(objDTOUserReview);
                    }
                }
                else
                {
                    return BadRequest();
                }

            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Exception : Bikewale.Service.UserReviews.GetUserReviewList");
                return InternalServerError();
            }

            return NotFound();
        }

        [Route("api/user-reviews/search/V2/")]
        public IHttpActionResult GetUserReviewListV2([FromUri]Bikewale.Entities.UserReviews.ReviewDataCombinedFilter filters)
        {
            Bikewale.Entities.UserReviews.Search.SearchResult objUserReviews = null;
            Bikewale.DTO.UserReviews.Search.SearchResult objDTOUserReview = null;
            try
            {
                if (filters != null && (!String.IsNullOrEmpty(filters.InputFilter.Model) || !String.IsNullOrEmpty(filters.InputFilter.Make)))
                {
                    filters.ReviewFilter.IsDescriptionRequired = true;
                    objUserReviews = _userReviewsSearch.GetUserReviewsList(filters);
                    if (objUserReviews != null)
                    {
                        objDTOUserReview = UserReviewsMapper.Convert(objUserReviews);
                        return Ok(objDTOUserReview);
                    }
                }
                else
                {
                    return BadRequest();
                }

            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Exception : Bikewale.Service.UserReviews.GetUserReviewList");
                return InternalServerError();
            }

            return NotFound();
        }


        /// <summary>
        /// Created by Sajal Gupta on 11-09-2017
        /// Description : Api to skip top count reviews
        /// </summary>
        /// <param name="filters"></param>
        /// <param name="skipTopCount"></param>
        /// <returns></returns>
        [Route("api/user-reviews/list/{skipTopCount}")]
        public IHttpActionResult GetUserReviewList([FromUri]Bikewale.Entities.UserReviews.Search.InputFilters filters, uint skipTopCount)
        {
            Bikewale.Entities.UserReviews.Search.SearchResult objUserReviews = null;
            Bikewale.DTO.UserReviews.Search.SearchResult objDTOUserReview = null;
            try
            {
                if (filters != null && (!String.IsNullOrEmpty(filters.Model) || !String.IsNullOrEmpty(filters.Make)))
                {
                    objUserReviews = _userReviewsSearch.GetUserReviewsList(filters, skipTopCount);
                    if (objUserReviews != null)
                    {
                        objDTOUserReview = UserReviewsMapper.Convert(objUserReviews);
                        return Ok(objDTOUserReview);
                    }
                }
                else
                {
                    return BadRequest();
                }

            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Exception : Bikewale.Service.UserReviews.GetUserReviewList");
                return InternalServerError();
            }

            return NotFound();
        }

        /// <summary>
        /// Created By : Sushil Kumar on 7th Sep 2017
        /// Description : To get user reviews by modelid
        /// </summary>
        /// <param name="modelId"></param>
        /// <param name="filters"></param>
        /// <returns></returns>
        [Route("api/user-reviews/model/{modelId}/")]
        public IHttpActionResult GetModelUserReviews(uint modelId,[FromUri]Bikewale.Entities.UserReviews.Search.InputFilters filters)
        {
            BikeModelUserReviews objUserReviews = null;
            try
            {
                if (modelId > 0 && filters!=null)
                {
                    BikeRatingsReviewsInfo objBikeRatingReview = _userReviewsCacheRepo.GetBikeRatingsReviewsInfo(modelId);
                    if(objBikeRatingReview!=null)
                    {
                        objUserReviews = UserReviewsMapper.Convert(objBikeRatingReview);
                        if(objUserReviews !=null)
                        {
                            filters.Model = modelId.ToString();
                            var result = _userReviewsSearch.GetUserReviewsList(filters);

                            if(result!=null)
                            {
                                objUserReviews.UserReviews = UserReviewsMapper.Convert(result);
                            }

                            if(objBikeRatingReview.RatingDetails !=null)
                            {
                                objUserReviews.Make = MakeListMapper.Convert(objBikeRatingReview.RatingDetails.Make);
                                objUserReviews.Model = ModelMapper.Convert(objBikeRatingReview.RatingDetails.Model);
                                objUserReviews.OriginalImagePath = objBikeRatingReview.RatingDetails.OriginalImagePath;
                                objUserReviews.HostUrl = objBikeRatingReview.RatingDetails.HostUrl;
                                objUserReviews.IsDiscontinued = objBikeRatingReview.RatingDetails.IsDiscontinued;
                                objUserReviews.IsUpcoming = objBikeRatingReview.RatingDetails.IsUpcoming;
                            }                           
                        }

                        return Ok(objUserReviews);

                    }
                    else
                    {
                        return NotFound();
                    }
                }
                else BadRequest();
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("Exception : Bikewale.Service.UserReviews.GetModelUserReviews({0})", modelId));
                return InternalServerError();
            }

            return NotFound();
        }

        #region Get Most Reviewed Bike List
        /// <summary>
        ///  To get Most Reviewed Bikes 
        /// </summary>
        /// <param name="totalRecords">Number of records to be fetched</param>
        /// <returns>List of Bikes with Reviews</returns>
        [ResponseType(typeof(IEnumerable<ReviewTaggedBike>))]
        public IHttpActionResult Get(ushort totalRecords)
        {
            List<ReviewTaggedBikeEntity> objUserReview = null;
            List<ReviewTaggedBike> objDTOUserReview = null;
            try
            {
                objUserReview = _userReviewsRepo.GetMostReviewedBikesList(totalRecords);

                if (objUserReview != null)
                {
                    objDTOUserReview = UserReviewsMapper.Convert(objUserReview);

                    return Ok(objDTOUserReview);
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Exception : Bikewale.Service.UserReviews.UserReviewsListController");
                return InternalServerError();
            }

            return NotFound();
        }   // Get 
        #endregion

        #region Get Bike Reviews List wth Paging
        /// <summary>
        /// To get review bike list with pagination
        /// Modified by :   Sumit Kate on 26 Apr 2017
        /// Description :   Call new User Reviews BAL
        /// </summary>
        /// <param name="startIndex"></param>
        /// <param name="endIndex"></param>
        /// <param name="modelId"></param>
        /// <param name="versionId"></param>
        /// <param name="filter"></param>
        /// <param name="totalRecords"></param>
        /// <returns>List of Reviewed Bikes</returns>
        [ResponseType(typeof(IEnumerable<Review>))]
        public IHttpActionResult Get(uint startIndex, uint endIndex, uint modelId, uint versionId, FilterBy filter, uint totalRecords)
        {
            IEnumerable<ReviewEntity> objUserReview = null;
            IEnumerable<Review> objDTOUserReview = null;
            try
            {
                objUserReview = _userReviews.GetUserReviews(startIndex, endIndex, modelId, versionId, filter).ReviewList;
                if (objUserReview != null)
                {
                    objDTOUserReview = UserReviewsMapper.Convert(objUserReview);
                    return Ok(objDTOUserReview);
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Exception : Bikewale.Service.UserReviews.UserReviewsListController");
                return InternalServerError();
            }

            return NotFound();
        }   // Get 
        #endregion


    }
}
