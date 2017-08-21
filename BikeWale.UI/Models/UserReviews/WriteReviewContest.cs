using Bikewale.Interfaces.BikeData;
using Bikewale.Entities.UserReviews;
using Bikewale.Interfaces.UserReviews;

namespace Bikewale.Models.UserReviews
{
    /// <summary>
    /// Created by: Sangram Nandkhile on 05 Jun 2017
    /// Summary: Write review contest
    /// Modified by: Vivek Singh Tomar on 12th Aug 2017
    /// Summary: Added IUserReviewsCache to fetch list of winners of user reviews contest
    /// </summary>
    public class WriteReviewContest
    {
        private bool _isMobile = false;
        private readonly IBikeMakesCacheRepository<int> _makeRepository = null;
        private readonly IUserReviewsCache _userReviewCache = null;
        public WriteReviewContest(bool IsMobile, IBikeMakesCacheRepository<int> makeRepository, IUserReviewsCache userReviewCache)
        {
            _makeRepository = makeRepository;
            _isMobile = IsMobile;
            _userReviewCache = userReviewCache;
        }

        public int csrc { get; set; }
        /// <summary>
        /// Summary: To Create Write review contest GET data
        /// Modified by: Vivek Singh Tomar On 12th Aug 2017
        /// Summary: Added functionality to get winners of user reviews contest
        /// </summary>
        /// <returns></returns>
        public WriteReviewContestVM GetData()
        {
            WriteReviewContestVM viewModel = new WriteReviewContestVM();
            viewModel.Makes = _makeRepository.GetMakesByType(Entities.BikeData.EnumBikeType.UserReviews);
            if(csrc > 0)
                viewModel.QueryString = Utils.Utils.EncryptTripleDES(string.Format("sourceid={0}", csrc));
            else
                viewModel.QueryString = Utils.Utils.EncryptTripleDES(string.Format("sourceid={0}", _isMobile ? (int)UserReviewPageSourceEnum.Mobile_UserReviewContestPage : (int)UserReviewPageSourceEnum.Desktop_UserReviewContestPage));
            viewModel.UserReviewsWinners = new UserReviewWinnerWidget(_userReviewCache).GetData();
            BindPageMetas(viewModel);
            return viewModel;
        }

        /// <summary>
        /// Summary: Bind meta tags
        /// </summary>
        /// <param name="objData"></param>
        private void BindPageMetas(WriteReviewContestVM objData)
        {
            objData.PageMetaTags.Title = "Bike Review Contest | Participate & Win- BikeWale";
            objData.PageMetaTags.Description = "Write a fair review about your bike and help others in making the right choice. Share your experience with other prospective buyers.";
            objData.PageMetaTags.Keywords = "Review contest, bike reviews, bike user reviews, user review contest, bikewale user review, bikewale contest";
            objData.PageMetaTags.CanonicalUrl = "https://www.bikewale.com/bike-review-contest/";
            objData.PageMetaTags.AlternateUrl = "https://www.bikewale.com/m/bike-review-contest/";

        }
    }
}