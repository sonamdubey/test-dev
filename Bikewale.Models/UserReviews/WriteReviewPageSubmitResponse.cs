
namespace Bikewale.Models.UserReviews
{
    /// <summary>
    /// Created by Sajal Gupta on 19-04-2017
    /// Descrioption : This entity will contain write review page submit response
    /// </summary>
    public class WriteReviewPageSubmitResponse
    {
        public string TitleErrorText { get; set; }
        public string ReviewErrorText { get; set; }
        public bool IsSuccess { get; set; }
    }
}
