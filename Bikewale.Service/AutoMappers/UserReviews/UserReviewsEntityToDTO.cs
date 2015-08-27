using AutoMapper;
using Bikewale.DTO.Make;
using Bikewale.DTO.Model;
using Bikewale.DTO.Version;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.DTO;
using Bikewale.Entities.UserReviews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bikewale.Service.AutoMappers.UserReviews
{
    public class UserReviewsEntityToDTO
    {
        public static ReviewDetails ConvertReviewDetailsEntity(ReviewDetailsEntity entity)
        {
            Mapper.CreateMap<BikeModelEntityBase, ModelBase>();
            Mapper.CreateMap<BikeMakeEntityBase, MakeBase>();
            Mapper.CreateMap<BikeVersionEntityBase, VersionBase>();
            Mapper.CreateMap<ReviewTaggedBikeEntity, ReviewTaggedBike>();
            Mapper.CreateMap<ReviewEntity, Review>();
            Mapper.CreateMap<ReviewRatingEntity, ReviewRating>();
            Mapper.CreateMap<ReviewRatingEntityBase, ReviewRatingBase>();
            Mapper.CreateMap<ReviewEntityBase, ReviewBase>();
            Mapper.CreateMap<ReviewDetailsEntity, ReviewDetails>();
            return Mapper.Map<ReviewDetailsEntity, ReviewDetails>(entity);
        }

        internal static ReviewRating ConvertReviewRatingEntity(ReviewRatingEntity objURRating)
        {
            Mapper.CreateMap<ReviewRatingEntity, ReviewRating>();
            return Mapper.Map<ReviewRatingEntity, ReviewRating>(objURRating);
        }

        internal static List<ReviewTaggedBike> ConvertReviewTaggedBikeEntityList(List<ReviewTaggedBikeEntity> objUserReview)
        {
            Mapper.CreateMap<BikeModelEntityBase, ModelBase>();
            Mapper.CreateMap<BikeMakeEntityBase, MakeBase>();
            Mapper.CreateMap<BikeVersionEntityBase, VersionBase>();
            Mapper.CreateMap<ReviewTaggedBikeEntity, ReviewTaggedBike>();
            Mapper.CreateMap<ReviewEntity, Review>();
            Mapper.CreateMap<ReviewRatingEntity, ReviewRating>();
            Mapper.CreateMap<ReviewRatingEntityBase, ReviewRatingBase>();
            Mapper.CreateMap<ReviewEntityBase, ReviewBase>();
            Mapper.CreateMap<ReviewDetailsEntity, ReviewDetails>();
            return Mapper.Map<List<ReviewTaggedBikeEntity>, List<ReviewTaggedBike>>(objUserReview);
        }

        internal static List<Review> ConvertReviewEntity(List<ReviewEntity> objUserReview)
        {
            Mapper.CreateMap<BikeModelEntityBase, ModelBase>();
            Mapper.CreateMap<BikeMakeEntityBase, MakeBase>();
            Mapper.CreateMap<BikeVersionEntityBase, VersionBase>();
            Mapper.CreateMap<ReviewTaggedBikeEntity, ReviewTaggedBike>();
            Mapper.CreateMap<ReviewEntity, Review>();
            Mapper.CreateMap<ReviewRatingEntity, ReviewRating>();
            Mapper.CreateMap<ReviewRatingEntityBase, ReviewRatingBase>();
            Mapper.CreateMap<ReviewEntityBase, ReviewBase>();
            return Mapper.Map<List<ReviewEntity>, List<Review>>(objUserReview);
        }

        internal static List<ReviewsList> ConvertReviewsListEntity(List<ReviewsListEntity> objUserReview)
        {
            Mapper.CreateMap<BikeModelEntityBase, ModelBase>();
            Mapper.CreateMap<BikeMakeEntityBase, MakeBase>();
            Mapper.CreateMap<BikeVersionEntityBase, VersionBase>();
            Mapper.CreateMap<ReviewTaggedBikeEntity, ReviewTaggedBike>();
            Mapper.CreateMap<ReviewRatingEntityBase, ReviewRatingBase>();
            Mapper.CreateMap<ReviewEntityBase, ReviewBase>();
            Mapper.CreateMap<ReviewsListEntity, ReviewsList>();
            return Mapper.Map<List<ReviewsListEntity>, List<ReviewsList>>(objUserReview);
        }
    }
}