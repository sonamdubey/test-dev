using Bikewale.Entities.BikeData;
using Bikewale.Entities.UserReviews;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.UserReviews;
using Bikewale.Notifications;
using System;
using System.Collections.Generic;
using Bikewale.Utility;

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
        private readonly IBikeMakesCacheRepository _makeRepository = null;
        private readonly IUserReviewsCache _userReviewCache = null;
        private readonly uint? _makeId;
        private readonly uint? _modelId;
        private readonly string _makeName;
        private readonly string _modelName;
        private readonly string _makeMasking;
        private readonly string _modelMasking;

        public WriteReviewContest(bool IsMobile, IBikeMakesCacheRepository makeRepository, IUserReviewsCache userReviewCache, uint? makeId,
            uint? modelId, string makeName, string modelName, string makeMaskingName, string modelMaskingName)
        {
            _makeRepository = makeRepository;
            _isMobile = IsMobile;
            _userReviewCache = userReviewCache;
            _makeId = makeId;
            _modelId = modelId;
            _makeName = makeName;
            _modelName = modelName;
            _makeMasking = makeMaskingName;
            _modelMasking = modelMaskingName;
        }

        public int csrc { get; set; }
        public bool IsMobile { get; internal set; }

        /// <summary>
        /// Modified by: Vivek Singh Tomar On 12th Aug 2017
        /// Summary: To Create Write review contest GET data
        /// Modified By :   Vishnu Teja Yalakuntla on 22nd Aug 2017
        /// Summary :   Populated MakeModelPopupVM and added try catch
        /// Summary: Added functionality to get winners of user reviews contest
        /// </summary>
        /// <returns></returns>
        public WriteReviewContestVM GetData()
        {
            WriteReviewContestVM viewModel = new WriteReviewContestVM();
            viewModel.UserReviewPopup = new Make.UserReviewPopupVM();

            try
            {
                viewModel.MakeId = _makeId;
                viewModel.ModelId = _modelId;
                viewModel.MakeName = _makeName;
                viewModel.ModelName = _modelName;
                viewModel.MakeMaskingName = _makeMasking;
                viewModel.ModelMaskingName = _modelMasking;
                IEnumerable<BikeMakeEntityBase> makesList = _makeRepository.GetMakesByType(Entities.BikeData.EnumBikeType.UserReviews);
                viewModel.Makes = makesList;

                viewModel.UserReviewPopup = new Make.UserReviewPopupVM();
                UserReviewPopupModel userReviewPopupModel = new UserReviewPopupModel(_makeRepository, makesList);

                viewModel.UserReviewPopup.Makes = userReviewPopupModel.GetMakesList();
                viewModel.UserReviewPopup.MakeId = _makeId;
                viewModel.UserReviewPopup.ModelId = _modelId;
                viewModel.UserReviewPopup.ModelName = _modelName;
                viewModel.UserReviewPopup.MakeName = _makeName;

                if (csrc > 0)
                    viewModel.QueryString = Bikewale.Utility.TripleDES.EncryptTripleDES(string.Format("sourceid={0}", csrc));
                else
                    viewModel.QueryString = Bikewale.Utility.TripleDES.EncryptTripleDES(string.Format("sourceid={0}", _isMobile ? (int)UserReviewPageSourceEnum.Mobile_UserReviewContestPage : (int)UserReviewPageSourceEnum.Desktop_UserReviewContestPage));

                viewModel.UserReviewsWinners = new UserReviewWinnerWidget(_userReviewCache).GetData();
                BindPageMetas(viewModel);
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "GetData");
            }

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