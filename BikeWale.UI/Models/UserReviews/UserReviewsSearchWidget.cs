﻿using Bikewale.Entities.UserReviews;
using Bikewale.Entities.UserReviews.Search;
using Bikewale.Interfaces.UserReviews;
using System;

namespace Bikewale.Models.UserReviews
{
    /// <summary>
    /// Created By : Sushil Kumar on 7th May 2017
    /// Description : User reviews section to searching utility
    /// </summary>
    public class UserReviewsSearchWidget
    {

        private InputFilters _filters = null;
        private uint _modelId;
        private readonly IUserReviewsCache _userReviewsCacheRepo = null;
        public BikeReviewsInfo ReviewsInfo { get; set; }
        public uint? SkipReviewId { get; set; }
        public FilterBy ActiveReviewCateory { get; set; }
        public string WriteReviewLink { get; set; }


        /// <summary>
        /// Created By : Sushil Kumar on 7th May 2017
        /// Description : Constructor to resolve interfaces depedencies
        /// </summary>
        /// <param name="modelId"></param>
        /// <param name="filters"></param>
        /// <param name="userReviewsCacheRepo"></param>
        public UserReviewsSearchWidget(uint modelId, InputFilters filters, IUserReviewsCache userReviewsCacheRepo)
        {
            _modelId = modelId;
            _filters = filters;
            _userReviewsCacheRepo = userReviewsCacheRepo;
        }

        /// <summary>
        /// Created By : Sushil Kumar on 7th May 2017
        /// Description : Function to get user reviews data based on filters and bind related widgets
        /// </summary>
        /// <returns></returns>
        public UserReviewsSearchVM GetData()
        {
            UserReviewsSearchVM objData = new UserReviewsSearchVM();
            objData.ModelId = _modelId;
            objData.ReviewsInfo = ReviewsInfo;
            objData.ActiveReviewCategory = ActiveReviewCateory;

            objData.UserReviews = _userReviewsCacheRepo.GetUserReviewsList(_filters);

            if (objData.UserReviews != null)
            {
                if (objData.ReviewsInfo == null)
                {
                    objData.ReviewsInfo = _userReviewsCacheRepo.GetBikeReviewsInfo(objData.ModelId, SkipReviewId);
                }

                if (objData.ReviewsInfo != null && objData.ReviewsInfo.Make != null && objData.ReviewsInfo.Model != null)
                {
                    //set bike data and other properties
                    objData.BikeName = string.Format("{0} {1}", objData.ReviewsInfo.Make.MakeName, objData.ReviewsInfo.Model.ModelName);

                    objData.WriteReviewLink = Utils.Utils.EncryptTripleDES(string.Format("returnUrl=/{0}-bikes/{1}/", objData.ReviewsInfo.Make.MaskingName, objData.ReviewsInfo.Model.MaskingName));

                    objData.Pager = new Entities.Pager.PagerEntity()
                    {
                        PageNo = _filters.PN,
                        PageSize = _filters.PS,
                        PagerSlotSize = 5,
                        BaseUrl = String.Format("/m/{0}-bikes/{1}/reviews/", objData.ReviewsInfo.Make.MaskingName, objData.ReviewsInfo.Model.MaskingName),
                        PageUrlType = "page/",
                        TotalResults = objData.UserReviews.TotalCount
                    };
                }


            }


            return objData;
        }
    }
}