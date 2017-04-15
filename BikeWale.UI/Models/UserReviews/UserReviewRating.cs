
using Bikewale.Entities.UserReviews;
using Bikewale.Interfaces.BikeData;
using System.Collections.Generic;
namespace Bikewale.Models
{
    public class UserReviewRating
    {
        private readonly IBikeInfo _bikeInfo = null;
        private uint _modelId;
        public UserReviewRating(uint modelId, IBikeInfo bikeInfo)
        {
            _modelId = modelId;
            _bikeInfo = bikeInfo;
        }
        public UserReviewRatingVM GetData()
        {
            UserReviewRatingVM objUserVM = new UserReviewRatingVM();
            objUserVM.BikeInfo = _bikeInfo.GetBikeInfo(_modelId, 0);
            IList<UserReviewOverallRating> obj = new List<UserReviewOverallRating>();

            obj.Add(new UserReviewOverallRating
            {
                Id = 1,
                Heading = "Terrible!",
                Description = "I regret riding this bike."
            });
            obj.Add(new UserReviewOverallRating
            {
                Id = 1,
                Heading = "It's bad!",
                Description = "I know better bikes in the same price range."
            });
            obj.Add(new UserReviewOverallRating
            {
                Id = 1,
                Heading = "Ummm..!",
                Description = "It's okay. Could have been better."
            });
            obj.Add(new UserReviewOverallRating
            {
                Id = 1,
                Heading = "Superb!",
                Description = "It's good to ride, I love it."
            });
            obj.Add(new UserReviewOverallRating
            {
                Id = 1,
                Heading = "Amazing!",
                Description = "I love everything about the bike."
            });
            objUserVM.OverAllRatingText = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
            return objUserVM;
        }

    }
}