
using Bikewale.Common;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.UserReviews;
using Bikewale.Interfaces.UserReviews;
using System;
using System.Collections.Specialized;
using System.Linq;
using System.Web;

namespace Bikewale.Models.UserReviews
{
    public class WriteReviewPageModel
    {
        private readonly IUserReviews _userReviews = null;
        private uint _reviewId, _modelId, _makeId, _overAllRating;
        private string _decodedString;
        private ulong _customerId;

        public BikeMakeEntityBase Make { get; set; }
        public BikeModelEntityBase Model { get; set; }
        public ushort Rating { get; set; }


        /// <summary>
        /// Created By : Sushil Kumar on 17th April 2017
        /// Description : Added interfaces for bikeinfo and user reviews 
        /// </summary>
        /// <param name="userReviews"></param>
        public WriteReviewPageModel(IUserReviews userReviews, string encodedString)
        {
            _userReviews = userReviews;
            _decodedString = Utils.Utils.DecryptTripleDES(encodedString);
            ParseQueryString(_decodedString);
        }

        public void ParseQueryString(string decodedQueryString)
        {
            NameValueCollection queryCollection = HttpUtility.ParseQueryString(decodedQueryString);

            uint.TryParse(queryCollection["reviewid"], out _reviewId);
            uint.TryParse(queryCollection["modelid"], out _modelId);
            uint.TryParse(queryCollection["makeid"], out _makeId);
            uint.TryParse(queryCollection["overallrating"], out _overAllRating);
            ulong.TryParse(queryCollection["customerid"], out _customerId);

        }

        /// <summary>
        /// Created By : Sushil Kumar on 17th April 2017
        /// Description : Get data for the write reviews page
        /// </summary>
        /// <returns></returns>
        public WriteReviewPageVM GetData()
        {
            WriteReviewPageVM objPage = null;
            try
            {
                objPage = new WriteReviewPageVM();

                BikeModelEntity objModelEntity = null;

                if (_modelId > 0)
                    objModelEntity = new ModelHelper().GetModelDataById(_modelId);

                if (objModelEntity != null)
                {
                    objPage.Make = objModelEntity.MakeBase;
                    objPage.Model = new BikeModelEntityBase();
                    objPage.Model.ModelId = objModelEntity.ModelId;
                    objPage.Model.ModelName = objModelEntity.ModelName;
                    objPage.Model.MaskingName = objModelEntity.MaskingName;
                }
                

                GetUserRatings(objPage);


            }
            catch (Exception ex)
            {

            }
            return objPage;
        }

        /// <summary>
        /// Created By : Sushil Kumar on 17th April 2017
        /// Description : Function to get user ratings and overall ratings for write review page
        /// </summary>
        /// <param name="objUserVM"></param>
        private void GetUserRatings(WriteReviewPageVM objUserVM)
        {
            try
            {
                UserReviewsData objUserReviewData = _userReviews.GetUserReviewsData();
                if (objUserReviewData != null)
                {
                    if (objUserReviewData.OverallRating != null)
                    {
                        //objUserVM.Rating = objUserReviewData.OverallRating;
                    }

                    if (objUserReviewData.Questions != null)
                    {
                        UserReviewsInputEntity filter = new UserReviewsInputEntity()
                        {
                            Type = UserReviewQuestionType.Review,
                            PriceRangeId = 3
                        };
                        var objQuestions = _userReviews.GetUserReviewQuestions(filter, objUserReviewData);
                        if (objQuestions != null)
                        {
                            objQuestions.LastOrDefault().DisplayType = UserReviewQuestionDisplayType.Text;
                            objUserVM.JsonQuestionList = Newtonsoft.Json.JsonConvert.SerializeObject(objQuestions);
                        }
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

    }
}