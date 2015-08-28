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

namespace Bikewale.Service.Controllers.UserReviews
{
    public class UserReviewsListController : ApiController
    {

        #region Get Reviewed Bike List
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [ResponseType(typeof(IEnumerable<ReviewTaggedBike>))]
        public HttpResponseMessage Get()
        {
            List<ReviewTaggedBikeEntity> objUserReview = null;
            List<ReviewTaggedBike> objDTOUserReview = null;
            using (IUnityContainer container = new UnityContainer())
            {
                IUserReviews userReviewsRepo = null;

                container.RegisterType<IUserReviews, UserReviewsRepository>();
                userReviewsRepo = container.Resolve<IUserReviews>();

                objUserReview = userReviewsRepo.GetReviewedBikesList();

                if (objUserReview != null)
                {
                    // Auto map the properties
                    objDTOUserReview = new List<ReviewTaggedBike>();

                    Mapper.CreateMap<BikeModelEntityBase, ModelBase>();
                    Mapper.CreateMap<BikeMakeEntityBase, MakeBase>();
                    Mapper.CreateMap<BikeVersionEntityBase, VersionBase>();
                    Mapper.CreateMap<ReviewTaggedBikeEntity, ReviewTaggedBike>();
                    Mapper.CreateMap<ReviewEntity, Review>();
                    Mapper.CreateMap<ReviewRatingEntity, ReviewRating>();
                    Mapper.CreateMap<ReviewRatingEntityBase, ReviewRatingBase>();
                    Mapper.CreateMap<ReviewEntityBase, ReviewBase>();
                    Mapper.CreateMap<ReviewDetailsEntity, ReviewDetails>();
                    objDTOUserReview = Mapper.Map<List<ReviewTaggedBikeEntity>, List<ReviewTaggedBike>>(objUserReview);
                    return Request.CreateResponse(HttpStatusCode.OK, objUserReview);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NoContent, "No Data Found");
                }
            }
        }   // Get 
        #endregion

        #region Get Most Reviewed Bike List
        /// <summary>
        /// 
        /// </summary>
        /// <param name="totalRecords"></param>
        /// <returns></returns>
        [ResponseType(typeof(IEnumerable<ReviewTaggedBike>))]
        public HttpResponseMessage Get(ushort totalRecords)
        {
            List<ReviewTaggedBikeEntity> objUserReview = null;
            List<ReviewTaggedBike> objDTOUserReview = null;
            using (IUnityContainer container = new UnityContainer())
            {
                IUserReviews userReviewsRepo = null;

                container.RegisterType<IUserReviews, UserReviewsRepository>();
                userReviewsRepo = container.Resolve<IUserReviews>();

                objUserReview = userReviewsRepo.GetMostReviewedBikesList(totalRecords);

                if (objUserReview != null)
                {
                    // Auto map the properties
                    objDTOUserReview = new List<ReviewTaggedBike>();

                    //Mapper.CreateMap<BikeModelEntityBase, ModelBase>();
                    //Mapper.CreateMap<BikeMakeEntityBase, MakeBase>();
                    //Mapper.CreateMap<BikeVersionEntityBase, VersionBase>();
                    //Mapper.CreateMap<ReviewTaggedBikeEntity, ReviewTaggedBike>();
                    //Mapper.CreateMap<ReviewEntity, Review>();
                    //Mapper.CreateMap<ReviewRatingEntity, ReviewRating>();
                    //Mapper.CreateMap<ReviewRatingEntityBase, ReviewRatingBase>();
                    //Mapper.CreateMap<ReviewEntityBase, ReviewBase>();
                    //Mapper.CreateMap<ReviewDetailsEntity, ReviewDetails>();
                    //objDTOUserReview = Mapper.Map<List<ReviewTaggedBikeEntity>, List<ReviewTaggedBike>>(objUserReview);

                    objDTOUserReview = UserReviewsMapper.Convert(objUserReview);

                    return Request.CreateResponse(HttpStatusCode.OK, objUserReview);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NoContent, "No Data Found");
                }
            }
        }   // Get 
        #endregion

        #region Get Bike Reviews List wth Paging
        /// <summary>
        /// 
        /// </summary>
        /// <param name="totalRecords"></param>
        /// <returns></returns>
        [ResponseType(typeof(IEnumerable<Review>))]
        public HttpResponseMessage Get(uint startIndex, uint endIndex, uint modelId, uint versionId, FilterBy filter, out uint totalRecords)
        {
            List<ReviewEntity> objUserReview = null;
            List<Review> objDTOUserReview = null;
            using (IUnityContainer container = new UnityContainer())
            {
                IUserReviews userReviewsRepo = null;

                container.RegisterType<IUserReviews, UserReviewsRepository>();
                userReviewsRepo = container.Resolve<IUserReviews>();

                objUserReview = userReviewsRepo.GetBikeReviewsList(startIndex, endIndex, modelId, versionId, filter,out totalRecords);

                if (objUserReview != null)
                {
                    // Auto map the properties
                    objDTOUserReview = new List<Review>();

                    //Mapper.CreateMap<BikeModelEntityBase, ModelBase>();
                    //Mapper.CreateMap<BikeMakeEntityBase, MakeBase>();
                    //Mapper.CreateMap<BikeVersionEntityBase, VersionBase>();
                    //Mapper.CreateMap<ReviewTaggedBikeEntity, ReviewTaggedBike>();
                    //Mapper.CreateMap<ReviewEntity, Review>();
                    //Mapper.CreateMap<ReviewRatingEntity, ReviewRating>();
                    //Mapper.CreateMap<ReviewRatingEntityBase, ReviewRatingBase>();
                    //Mapper.CreateMap<ReviewEntityBase, ReviewBase>();
                    //objDTOUserReview = Mapper.Map<List<ReviewEntity>, List<Review>>(objUserReview);

                    objDTOUserReview = UserReviewsMapper.Convert(objUserReview);

                    return Request.CreateResponse(HttpStatusCode.OK, objUserReview);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NoContent, "No Data Found");
                }
            }
        }   // Get 
        #endregion
    }
}
