using Bikewale.Entities.DTO;
using Bikewale.Entities.UserReviews;
using Bikewale.Interfaces.UserReviews;
using Bikewale.Notifications;
using Bikewale.Service.AutoMappers.UserReviews;
using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Description;

namespace Bikewale.Service.Controllers.UserReviews
{
    /// <summary>
    /// User Reviews List based on differnt Categories
    /// Author : Sushil Kumar
    /// Created On : 24th August 2015
    /// </summary>
    public class UserReviewsListTypeController : ApiController
    {
        
        private readonly IUserReviewsRepository _userReviewsRepo = null;
        public UserReviewsListTypeController(IUserReviewsRepository userReviewsRepo)
        {
            _userReviewsRepo = userReviewsRepo;
        }
        
        #region Get Most Read Reviews
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [ResponseType(typeof(IEnumerable<ReviewsList>))]
        public IHttpActionResult Get(FilterBy type, ushort totalRecords)
        {
            List<ReviewsListEntity> objUserReview = null;
            List<ReviewsList> objDTOUserReview = null;
            try
            {                
                //getRecords based on the review type
                switch (type)
                {
                    case FilterBy.MostRead:
                        objUserReview = _userReviewsRepo.GetMostReadReviews(totalRecords);
                        break;
                    case FilterBy.MostHelpful:
                        objUserReview = _userReviewsRepo.GetMostHelpfulReviews(totalRecords);
                        break;
                    case FilterBy.MostRecent:
                        objUserReview = _userReviewsRepo.GetMostRecentReviews(totalRecords);
                        break;
                    case FilterBy.MostRated:
                        objUserReview = _userReviewsRepo.GetMostRatedReviews(totalRecords);
                        break;
                    default:
                        break;
                }
                if (objUserReview != null)
                {
                    // Auto map the properties
                    objDTOUserReview = new List<ReviewsList>();
                    objDTOUserReview = UserReviewsMapper.Convert(objUserReview);

                    objUserReview.Clear();
                    objUserReview = null;

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
