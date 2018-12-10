using AutoMapper;
using Carwale.DTOs.UserReviews;
using Carwale.Entity.UserReviews;

namespace Carwale.Service.Mappers
{
    public static class UserReviewMapper
    {
        public static void CreateMaps()
        {
            Mapper.CreateMap<RatingDTO, Rating>();
            Mapper.CreateMap<CarDetailsDto, CarDetails>();
            Mapper.CreateMap<RatingQuestionsDto, RatingQuestions>();
            Mapper.CreateMap<UserDetailsDto, UserDetails>();
            Mapper.CreateMap<RatingDetailsDTO, RatingDetails>();
        }
    }
}
