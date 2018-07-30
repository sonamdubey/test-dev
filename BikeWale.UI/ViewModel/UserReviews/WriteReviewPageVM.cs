using Bikewale.Entities.BikeData;
using Bikewale.Entities.UserReviews;
using Bikewale.Models.UserReviews;
using System.Collections.Generic;

namespace Bikewale.Models
{
    /// <summary>
    /// Created by Sajal Gupta on 10-04-2017
    /// Descrioption : This model is for write review page.
    /// Modified by : Aditi Srivastava on 5 July 2017
    /// Summary     : Added contest src for user review contest page
    /// </summary>
    public class WriteReviewPageVM : ModelBase
    {
        public BikeMakeEntityBase Make { get; set; }
        public BikeModelEntityBase Model { get; set; }
        public UserReviewOverallRating Rating { get; set; }
        public uint ReviewId { get; set; }
        public IEnumerable<UserReviewQuestion> QuestionsList { get; set; }
        public string Tips { get; set; }
        public bool HasReview { get; set; }
        public string JsonQuestionList { get; set; }
        public string HostUrl { get; set; }
        public string OriginalImagePath { get; set; }
        public ulong CustomerId { get; set; }
        public string PreviousPageUrl { get; set; }
        public string EncodedWriteUrl { get; set; }
        public string EmailId { get; set; }
        public string UserName { get; set; }
        public WriteReviewPageSubmitResponse SubmitResponse { get; set; }
        public string JsonReviewSummary { get; set; }
        public uint PageSourceId { get; set; }
        public int ContestSrc { get; set; }
    }
}
