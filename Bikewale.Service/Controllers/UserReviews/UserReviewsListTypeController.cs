using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AutoMapper;
using Bikewale.DAL.UserReviews;
using Bikewale.DTO.Make;
using Bikewale.DTO.Model;
using Bikewale.DTO.Version;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.DTO;
using Bikewale.Entities.UserReviews;
using Bikewale.Interfaces.UserReviews;
using Microsoft.Practices.Unity;
using System.Web.Http.Description;
using Bikewale.Service.AutoMappers.UserReviews;
using Bikewale.Notifications;

namespace Bikewale.Service.Controllers.UserReviews
{
    /// <summary>
    /// User Reviews List based on differnt Categories
    /// Author : Sushil Kumar
    /// Created On : 24th August 2015
    /// </summary>
    public class UserReviewsListTypeController : ApiController
    {
        
        private readonly IUserReviews _userReviewsRepo = null;
        public UserReviewsListTypeController(IUserReviews userReviewsRepo)
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

                    return Ok(objUserReview);
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
