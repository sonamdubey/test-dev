using AutoMapper;
using BikewaleOpr.DTO.BikeData;
using BikewaleOpr.DTO.Make;
using BikewaleOpr.DTO.UserReviews;
using BikewaleOpr.Entities;
using BikewaleOpr.Entities.UserReviews;

namespace BikewaleOpr.Service.AutoMappers.UserReviews
{
    public class UserReviewsMapper
    {
        internal static UserReviewSummaryDto Convert(UserReviewSummary objUserReview)
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