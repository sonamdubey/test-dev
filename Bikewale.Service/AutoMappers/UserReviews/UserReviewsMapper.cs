using AutoMapper;
using Bikewale.DTO.Make;
using Bikewale.DTO.Model;
using Bikewale.DTO.Series;
using Bikewale.DTO.UserReviews;
using Bikewale.DTO.Version;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.DTO;
using Bikewale.Entities.NewBikeSearch;
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

        internal static Bikewale.DTO.UserReviews.Search.SearchResult Convert(Entities.UserReviews.Search.SearchResult objUserReviews)
        {
            Mapper.CreateMap<BikeModelEntityBase, ModelBase>();
            Mapper.CreateMap<BikeMakeEntityBase, MakeBase>();
            Mapper.CreateMap<BikeVersionEntityBase, VersionBase>();
            Mapper.CreateMap<ReviewTaggedBikeEntity, ReviewTaggedBike>();
            Mapper.CreateMap<ReviewEntity, Bikewale.DTO.UserReviews.v2.Review>();
            Mapper.CreateMap<ReviewRatingEntity, ReviewRating>();
            Mapper.CreateMap<ReviewRatingEntityBase, Bikewale.DTO.UserReviews.v2.ReviewRatingBase>();
            Mapper.CreateMap<PagingUrl, Bikewale.DTO.BikeBooking.PagingUrl>();
            Mapper.CreateMap<ReviewEntityBase, ReviewBase>();
            Mapper.CreateMap<UserReviewRating, UserReviewRatingDto>();
            Mapper.CreateMap<UserReviewSummary, UserReviewSummaryDto>();
            Mapper.CreateMap<UserReviewQuestion, UserReviewQuestionDto>();
            Mapper.CreateMap<UserReviewOverallRating, UserReviewOverallRatingDto>();
            Mapper.CreateMap<Entities.UserReviews.Search.SearchResult, Bikewale.DTO.UserReviews.Search.SearchResult>();
            return Mapper.Map<Entities.UserReviews.Search.SearchResult, Bikewale.DTO.UserReviews.Search.SearchResult>(objUserReviews);
        }


        /// <summary>
        /// Created by : Snehal Dange on 1st Sep 2017
        /// Summary     : Mapper for Rate Bike api 
        /// </summary>
        public static RateBikeDetails Convert(UserReviewRatingData reviewRatingDataEntity)
        {
            Mapper.CreateMap<BikeModelEntity, ModelDetails>();
            Mapper.CreateMap<BikeModelEntityBase, ModelBase>();
            Mapper.CreateMap<BikeMakeEntityBase, MakeBase>();
            Mapper.CreateMap<BikeSeriesEntityBase, SeriesBase>();
            return Mapper.Map<UserReviewRatingData, RateBikeDetails>(reviewRatingDataEntity);
        }

        /// <summary>
        /// Created By  :   Vishnu Teja Yalakuntla on 06 Sep 2017
        /// Summary     :   Mapper for SaveUserReviewDetails API action method
        /// </summary>
        /// <param name="writeReviewInput"></param>
        /// <returns></returns>
        public static ReviewSubmitData Convert(WriteReviewInput writeReviewInput)
        {
            Mapper.CreateMap<WriteReviewInput, ReviewSubmitData>();
            return Mapper.Map<WriteReviewInput, ReviewSubmitData>(writeReviewInput);
        }
    }
}