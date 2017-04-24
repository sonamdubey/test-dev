﻿using AutoMapper;
using Bikewale.DTO.Make;
using Bikewale.DTO.Model;
using Bikewale.DTO.UserReviews;
using Bikewale.DTO.Version;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.DTO;
using Bikewale.Entities.UserReviews;
using System.Collections.Generic;

namespace Bikewale.Service.AutoMappers.UserReviews
{
    public class UserReviewsMapper
    {
        public static ReviewDetails Convert(ReviewDetailsEntity entity)
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

        internal static ReviewRating Convert(ReviewRatingEntity objURRating)
        {
            Mapper.CreateMap<ReviewRatingEntity, ReviewRating>();
            return Mapper.Map<ReviewRatingEntity, ReviewRating>(objURRating);
        }

        internal static List<ReviewTaggedBike> Convert(List<ReviewTaggedBikeEntity> objUserReview)
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

        internal static IEnumerable<Review> Convert(IEnumerable<ReviewEntity> objUserReview)
        {
            Mapper.CreateMap<BikeModelEntityBase, ModelBase>();
            Mapper.CreateMap<BikeMakeEntityBase, MakeBase>();
            Mapper.CreateMap<BikeVersionEntityBase, VersionBase>();
            Mapper.CreateMap<ReviewTaggedBikeEntity, ReviewTaggedBike>();
            Mapper.CreateMap<ReviewEntity, Review>();
            Mapper.CreateMap<ReviewRatingEntity, ReviewRating>();
            Mapper.CreateMap<ReviewRatingEntityBase, ReviewRatingBase>();
            Mapper.CreateMap<ReviewEntityBase, ReviewBase>();
            return Mapper.Map<IEnumerable<ReviewEntity>, IEnumerable<Review>>(objUserReview);
        }

        internal static List<ReviewsList> Convert(List<ReviewsListEntity> objUserReview)
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

        internal static DTO.UserReviews.UserReviewSummaryDto Convert(UserReviewSummary objUserReview)
        {
            Mapper.CreateMap<BikeModelEntityBase, ModelBase>();
            Mapper.CreateMap<BikeMakeEntityBase, MakeBase>();
            Mapper.CreateMap<UserReviewRating, UserReviewRatingDto>();
            Mapper.CreateMap<UserReviewSummary, UserReviewSummaryDto>();
            Mapper.CreateMap<UserReviewQuestion, UserReviewQuestionDto>();
            Mapper.CreateMap<UserReviewOverallRating, UserReviewOverallRatingDto>();
            return Mapper.Map<UserReviewSummary, UserReviewSummaryDto>(objUserReview);
        }
    }
}