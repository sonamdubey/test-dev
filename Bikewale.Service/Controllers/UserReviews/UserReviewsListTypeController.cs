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
    public class UserReviewsListTypeController : ApiController
    {
        #region Get Most Read Reviews
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [ResponseType(typeof(IEnumerable<ReviewsList>))]
        public HttpResponseMessage Get(FilterBy type, ushort totalRecords)
        {
            List<ReviewsListEntity> objUserReview = null;
            List<ReviewsList> objDTOUserReview = null;
            using (IUnityContainer container = new UnityContainer())
            {
                IUserReviews userReviewsRepo = null;

                container.RegisterType<IUserReviews, UserReviewsRepository>();
                userReviewsRepo = container.Resolve<IUserReviews>();

                //getRecords based on the review type
                switch (type)
                {
                    case FilterBy.MostRead:
                        objUserReview = userReviewsRepo.GetMostReadReviews(totalRecords);
                        break;
                    case FilterBy.MostHelpful:
                        objUserReview = userReviewsRepo.GetMostHelpfulReviews(totalRecords);
                        break;
                    case FilterBy.MostRecent:
                        objUserReview = userReviewsRepo.GetMostRecentReviews(totalRecords);
                        break;
                    case FilterBy.MostRated:
                        objUserReview = userReviewsRepo.GetMostRatedReviews(totalRecords);
                        break;
                    default:
                        break;
                }
               

                if (objUserReview != null)
                {
                    // Auto map the properties
                    objDTOUserReview = new List<ReviewsList>();

                    //Mapper.CreateMap<BikeModelEntityBase, ModelBase>();
                    //Mapper.CreateMap<BikeMakeEntityBase, MakeBase>();
                    //Mapper.CreateMap<BikeVersionEntityBase, VersionBase>();
                    //Mapper.CreateMap<ReviewTaggedBikeEntity, ReviewTaggedBike>();
                    //Mapper.CreateMap<ReviewRatingEntityBase, ReviewRatingBase>();
                    //Mapper.CreateMap<ReviewEntityBase, ReviewBase>();  
                    //Mapper.CreateMap<ReviewsListEntity, ReviewsList>();
                    //objDTOUserReview = Mapper.Map<List<ReviewsListEntity>, List<ReviewsList>>(objUserReview);

                    objDTOUserReview = UserReviewsEntityToDTO.ConvertReviewsListEntity(objUserReview);

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
