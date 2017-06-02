using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Bikewale.Interfaces.BikeData;
using Bikewale.Entities.UserReviews;

namespace Bikewale.Models.UserReviews
{
    public class WriteReviewContest
    {
        private readonly IBikeMakesCacheRepository<int> _makeRepository = null;
        public WriteReviewContest(IBikeMakesCacheRepository<int> makeRepository)
        {
            _makeRepository = makeRepository;
        }

        public WriteReviewContestVM GetData()
        {
            WriteReviewContestVM viewModel = new WriteReviewContestVM();
            viewModel.Makes = _makeRepository.GetMakesByType(Entities.BikeData.EnumBikeType.UserReviews);
            viewModel.QueryString = Utils.Utils.EncryptTripleDES(string.Format("sourceid={0}", (int)UserReviewPageSourceEnum.Mobile_ModelPage));
            BindPageMetas(viewModel);
            return viewModel;
        }

        private void BindPageMetas(WriteReviewContestVM objData)
        {
            objData.PageMetaTags.Title = "Bike review Contest";
            objData.PageMetaTags.Description = "Write a fair review about your bike and help others in making the right choice. Share your experience with other prospective buyers.";
            objData.PageMetaTags.Keywords = "Review contest, bike reviews, bike user reviews, user review contest, bikewale user review, bikewale contest";
        }
    }
}