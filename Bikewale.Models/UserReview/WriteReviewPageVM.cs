using Bikewale.Entities.BikeData;
using Bikewale.Entities.Customer;
using Bikewale.Entities.UserReviews;
using System.Collections.Generic;

namespace Bikewale.Models
{
    public class WriteReviewPageVM : ModelBase
    {
        public BikeMakeEntityBase Make { get; set; }
        public BikeModelEntityBase Model { get; set; }
        public UserReviewOverallRating Rating { get; set; }
        public uint ReviewId { get; set; }
        public string Review { get; set; }
        public string ReviewTitle { get; set; }
        public IEnumerable<UserReviewQuestion> QuestionsList { get; set; }
        public string Tips { get; set; }
        public CustomerEntityBase Customer { get; set; }
        public bool HasReview { get; set; }
        public string JsonQuestionList { get; set; }
        public string HostUrl { get; set; }
        public string OriginalImagePath { get; set; }
        public ulong CustomerId { get; set; }
        public string PreviousPageUrl { get; set; }
        public string EncodedWriteUrl { get; set; }
        public string EmailId { get; set; }
        public string UserName { get; set; }
    }
}
