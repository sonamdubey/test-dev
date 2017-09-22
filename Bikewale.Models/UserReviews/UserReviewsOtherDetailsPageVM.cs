using Bikewale.Entities.BikeData;
using Bikewale.Entities.UserReviews;
using System.Collections.Generic;

namespace Bikewale.Models.UserReviews
{
    /// <summary>
    /// Created by sajal gupta on 31-08-2017
    /// Descritpion : Entity for user review other page
    /// </summary>
    public class UserReviewsOtherDetailsPageVM : ModelBase
    {
        public BikeMakeEntityBase Make { get; set; }
        public BikeModelEntityBase Model { get; set; }
        public UserReviewOverallRating Rating { get; set; }
        public uint ReviewId { get; set; }
        public IEnumerable<UserReviewQuestion> QuestionsList { get; set; }
        public uint PageSourceId { get; set; }
        public int ContestSrc { get; set; }
        public string EmailId { get; set; }
        public string UserName { get; set; }
        public ulong CustomerId { get; set; }
        public string PreviousPageUrl { get; set; }
        public string EncodedWriteUrl { get; set; }
        public string ReturnUrl { get; set; }
    }
}
