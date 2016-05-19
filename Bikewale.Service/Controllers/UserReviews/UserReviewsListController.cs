using Bikewale.Entities.DTO;
using Bikewale.Entities.UserReviews;
using Bikewale.Interfaces.UserReviews;
using Bikewale.Notifications;
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

        private readonly IUserReviews _userReviewsRepo = null;
        public UserReviewsListController(IUserReviews userReviewsRepo)
        {
            _userReviewsRepo = userReviewsRepo;
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
                    objDTOUserReview = new List<ReviewTaggedBike>();
                    objDTOUserReview = UserReviewsMapper.Convert(objUserReview);

                    objUserReview.Clear();
                    objUserReview = null;

                    return Ok(objDTOUserReview);
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.Service.UserReviews.UserReviewsListController");
                objErr.SendMail();
                return InternalServerError();
            }

            return NotFound();
        }   // Get 
        #endregion

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
                    // Auto map the properties
                    objDTOUserReview = new List<ReviewTaggedBike>();
                    objDTOUserReview = UserReviewsMapper.Convert(objUserReview);

                    objUserReview.Clear();
                    objUserReview = null;

                    return Ok(objDTOUserReview);
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.Service.UserReviews.UserReviewsListController");
                objErr.SendMail();
                return InternalServerError();
            }

            return NotFound();
        }   // Get 
        #endregion

        #region Get Bike Reviews List wth Paging
        /// <summary>
        /// To get review bike list with pagination
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
            List<ReviewEntity> objUserReview = null;
            List<Review> objDTOUserReview = null;
            try
            {
                objUserReview = _userReviewsRepo.GetBikeReviewsList(startIndex, endIndex, modelId, versionId, filter, out totalRecords);

                if (objUserReview != null)
                {
                    // Auto map the properties
                    objDTOUserReview = new List<Review>();
                    objDTOUserReview = UserReviewsMapper.Convert(objUserReview);

                    objUserReview.Clear();
                    objUserReview = null;

                    return Ok(objDTOUserReview);
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.Service.UserReviews.UserReviewsListController");
                objErr.SendMail();
                return InternalServerError();
            }

            return NotFound();
        }   // Get 
        #endregion
    }
}
