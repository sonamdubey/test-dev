using AutoMapper;
using Bikewale.DAL.UserReviews;
using Bikewale.DTO.Make;
using Bikewale.DTO.Model;
using Bikewale.DTO.Version;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.DTO;
using Bikewale.Entities.UserReviews;
using Bikewale.Interfaces.UserReviews;
using Bikewale.Service.AutoMappers.UserReviews;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace Bikewale.Service.Controllers.UserReviews
{
    public class UserReviewsController : ApiController
    {
        #region User Reviews Details
        /// <summary>
        /// 
        /// </summary>
        /// <param name="reviewId"></param>
        /// <returns></returns>
        [ResponseType(typeof(ReviewDetails))]
        public HttpResponseMessage Get(uint reviewId)
        {
            ReviewDetailsEntity objUserReview = null;
            ReviewDetails objDTOUserReview = null;
            using (IUnityContainer container = new UnityContainer())
            {
                IUserReviews userReviewsRepo= null;

                container.RegisterType<IUserReviews, UserReviewsRepository>();
                userReviewsRepo = container.Resolve<IUserReviews>();

                objUserReview = userReviewsRepo.GetReviewDetails(reviewId);

                if (objUserReview != null)
                {
                    // Auto map the properties
                    objDTOUserReview = new ReviewDetails();

                    //Mapper.CreateMap<BikeModelEntityBase, ModelBase>();
                    //Mapper.CreateMap<BikeMakeEntityBase, MakeBase>();
                    //Mapper.CreateMap<BikeVersionEntityBase, VersionBase>();
                    //Mapper.CreateMap<ReviewTaggedBikeEntity, ReviewTaggedBike>();
                    //Mapper.CreateMap<ReviewEntity, Review>();
                    //Mapper.CreateMap<ReviewRatingEntity, ReviewRating>();
                    //Mapper.CreateMap<ReviewRatingEntityBase, ReviewRatingBase>();
                    //Mapper.CreateMap<ReviewEntityBase, ReviewBase>();
                    //Mapper.CreateMap<ReviewDetailsEntity, ReviewDetails>();
                    //objDTOUserReview = Mapper.Map<ReviewDetailsEntity, ReviewDetails>(objUserReview);

                    objDTOUserReview = UserReviewsEntityToDTO.ConvertReviewDetailsEntity(objUserReview);

                    return Request.CreateResponse(HttpStatusCode.OK, objUserReview);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NoContent, "No Data Found");
                }
            }
        }   // Get 
        #endregion

        #region Get User Review Ratings
        /// <summary>
        /// 
        /// </summary>
        /// <param name="reviewId"></param>
        /// <returns></returns>
        [ResponseType(typeof(ReviewRating))]
        public HttpResponseMessage Get(uint modelId,bool?review)
        {
            ReviewRatingEntity objURRating = null;
            ReviewRating objDTOURRating = null;
            using (IUnityContainer container = new UnityContainer())
            {
                IUserReviews userReviewsRepo = null;

                container.RegisterType<IUserReviews, UserReviewsRepository>();
                userReviewsRepo = container.Resolve<IUserReviews>();

                objURRating = userReviewsRepo.GetBikeRatings(modelId);

                if (objURRating != null)
                {
                    // Auto map the properties
                    objDTOURRating = new ReviewRating();
                    //Mapper.CreateMap<ReviewRatingEntity, ReviewRating>();
                    //objDTOURRating = Mapper.Map<ReviewRatingEntity, ReviewRating>(objURRating);

                    objDTOURRating = UserReviewsEntityToDTO.ConvertReviewRatingEntity(objURRating);

                    return Request.CreateResponse(HttpStatusCode.OK, objURRating);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NoContent, "No Data Found");
                }
            }
        }   // Get 
        #endregion

        #region Update Reviews View Count
        /// <summary>
        /// 
        /// </summary>
        /// <param name="reviewId"></param>
        /// <returns></returns>
        [ResponseType(typeof(Boolean))]
        public HttpResponseMessage Put(uint reviewId)
        {
            bool objURRating = false;
            using (IUnityContainer container = new UnityContainer())
            {
                IUserReviews userReviewsRepo = null;

                container.RegisterType<IUserReviews, UserReviewsRepository>();
                userReviewsRepo = container.Resolve<IUserReviews>();

                objURRating = userReviewsRepo.UpdateViews(reviewId);

                if (objURRating)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, objURRating);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotModified, "Oops ! Something Went Wrong");
                }
            }
        }   // Get 
        #endregion

        #region Is Helpful  User Review 
        /// <summary>
        /// 
        /// </summary>
        /// <param name="reviewId"></param>
        /// <returns></returns>
        [ResponseType(typeof(Boolean))]
        public HttpResponseMessage Put(uint reviewId,bool isHelpful)
        {
            bool objURHelpful = false;
            using (IUnityContainer container = new UnityContainer())
            {
                IUserReviews userReviewsRepo = null;

                container.RegisterType<IUserReviews, UserReviewsRepository>();
                userReviewsRepo = container.Resolve<IUserReviews>();

                objURHelpful = userReviewsRepo.UpdateReviewUseful(reviewId, isHelpful);

                if (objURHelpful)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, objURHelpful);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotModified, "Oops ! Something Went Wrong");
                }
            }
        }   // Get 
        #endregion

        #region Abuse User Review
        /// <summary>
        /// 
        /// </summary>
        /// <param name="reviewId"></param>
        /// <returns></returns>
        [ResponseType(typeof(Boolean))]
        public HttpResponseMessage Put(uint reviewId, string comment,string userId)
        {
            bool objURAbuse = false;
            using (IUnityContainer container = new UnityContainer())
            {
                IUserReviews userReviewsRepo = null;

                container.RegisterType<IUserReviews, UserReviewsRepository>();
                userReviewsRepo = container.Resolve<IUserReviews>();

                objURAbuse = userReviewsRepo.AbuseReview(reviewId, comment, userId);

                if (objURAbuse)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, objURAbuse);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotModified, "Oops ! Something Went Wrong");
                }
            }
        }   // Get 
        #endregion
    }
}
