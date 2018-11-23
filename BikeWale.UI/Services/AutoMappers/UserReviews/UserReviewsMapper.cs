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
        
            return Mapper.Map<ReviewDetailsEntity, ReviewDetails>(entity);
        }

        internal static ReviewRating Convert(ReviewRatingEntity objURRating)
        {
         
            return Mapper.Map<ReviewRatingEntity, ReviewRating>(objURRating);
        }

        internal static List<ReviewTaggedBike> Convert(List<ReviewTaggedBikeEntity> objUserReview)
        {
          
            return Mapper.Map<List<ReviewTaggedBikeEntity>, List<ReviewTaggedBike>>(objUserReview);
        }

        internal static IEnumerable<Review> Convert(IEnumerable<ReviewEntity> objUserReview)
        {
           
            return Mapper.Map<IEnumerable<ReviewEntity>, IEnumerable<Review>>(objUserReview);
        }

        internal static List<ReviewsList> Convert(List<ReviewsListEntity> objUserReview)
        {
         
            return Mapper.Map<List<ReviewsListEntity>, List<ReviewsList>>(objUserReview);
        }

        internal static DTO.UserReviews.UserReviewSummaryDto Convert(UserReviewSummary objUserReview)
        {
           
            return Mapper.Map<UserReviewSummary, UserReviewSummaryDto>(objUserReview);
        }

        internal static IEnumerable<DTO.UserReviews.UserReviewSummaryDto> Convert(IEnumerable<UserReviewSummary> objUserReview)
        {
         
            return Mapper.Map<IEnumerable<UserReviewSummary>, IEnumerable<UserReviewSummaryDto>>(objUserReview);
        }

        internal static Bikewale.DTO.UserReviews.Search.SearchResult Convert(Entities.UserReviews.Search.SearchResult objUserReviews)
        {
       
            return Mapper.Map<Entities.UserReviews.Search.SearchResult, Bikewale.DTO.UserReviews.Search.SearchResult>(objUserReviews);
        }

        /// <summary>
        /// Created By : Sushil Kumar on 7th Sep 2017
        /// Description : Mapper to resolve bikeratingsreview entity to its relevant dto
        /// </summary>
        /// <param name="objUserReview"></param>
        /// <returns></returns>
        internal static DTO.UserReviews.BikeModelUserReviews Convert(BikeRatingsReviewsInfo objUserReview)
        {
         
            return Mapper.Map<BikeRatingsReviewsInfo, BikeModelUserReviews>(objUserReview);
        }

        /// <summary>
        /// Created by : Snehal Dange on 1st Sep 2017
        /// Summary     : Mapper for Rate Bike api 
        /// </summary>
        public static Bikewale.DTO.UserReviews.RateBikeDetails Convert(Bikewale.Entities.UserReviews.UserReviewRatingData reviewRatingDataEntity)
        {
          
            return Mapper.Map<Bikewale.Entities.UserReviews.UserReviewRatingData, Bikewale.DTO.UserReviews.RateBikeDetails>(reviewRatingDataEntity);
        }

        /// <summary>
        /// Created by : Snehal Dange on 7st Sep 2017
        /// Summary     : Map input rating DTO to Entity
        /// </summary>
        public static InputRatingSaveEntity Convert(InputRatingSave objSaveInputRating) 
        {
          
            return Mapper.Map<InputRatingSave, InputRatingSaveEntity>(objSaveInputRating);
        }

        internal static IEnumerable<UserReviewQuestionDto> Convert(IEnumerable<UserReviewQuestion> objUserReview)
        {
           
            return Mapper.Map<IEnumerable<UserReviewQuestion>, IEnumerable<UserReviewQuestionDto>>(objUserReview);
        }

        /// <summary>
        /// Created by : Snehal Dange on 7st Sep 2017
        /// Summary     : Map a entity and dto to DTO (middle method to save all data)
        /// </summary>
        public static RatingReviewInput Convert(UserReviewRatingObject objRating, InputRatingSave objSaveInputRating)
        {
          
            var objRatingDTO = Mapper.Map<InputRatingSave, RatingReviewInput>(objSaveInputRating);
            return Mapper.Map<UserReviewRatingObject, RatingReviewInput>(objRating, objRatingDTO);
        }

        /// <summary>
        /// Created By  :   Vishnu Teja Yalakuntla on 06 Sep 2017
        /// Summary     :   Mapper for SaveUserReviewDetails API action method
        /// </summary>
        /// <param name="writeReviewInput"></param>
        /// <returns></returns>
        public static ReviewSubmitData Convert(WriteReviewInput writeReviewInput)
        {
            return Mapper.Map<WriteReviewInput, ReviewSubmitData>(writeReviewInput);
        }
    }
}