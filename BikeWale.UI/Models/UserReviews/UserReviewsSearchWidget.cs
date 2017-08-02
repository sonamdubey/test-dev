﻿using Bikewale.Entities.UserReviews;
using Bikewale.Entities.UserReviews.Search;
using Bikewale.Interfaces.UserReviews;
using Bikewale.Interfaces.UserReviews.Search;
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
        private readonly IUserReviewsSearch _userReviewsSearch = null;
        public BikeReviewsInfo ReviewsInfo { get; set; }        
        public FilterBy ActiveReviewCateory { get; set; }
        public string WriteReviewLink { get; set; }
        public bool IsDesktop { get; set; }

        /// <summary>
        /// Created By : Sushil Kumar on 7th May 2017
        /// Description : Constructor to resolve interfaces depedencies
        /// </summary>
        /// <param name="modelId"></param>
        /// <param name="filters"></param>
        /// <param name="userReviewsCacheRepo"></param>
        public UserReviewsSearchWidget(uint modelId, InputFilters filters, IUserReviewsCache userReviewsCacheRepo, IUserReviewsSearch userReviewsSearch)
        {
            _modelId = modelId;
            _filters = filters;
            _userReviewsCacheRepo = userReviewsCacheRepo;
            _userReviewsSearch = userReviewsSearch;
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

            objData.UserReviews = _userReviewsSearch.GetUserReviewsList(_filters);

            if (objData.UserReviews != null)
            {
                if (objData.ReviewsInfo == null)
                {
                    objData.ReviewsInfo = _userReviewsCacheRepo.GetBikeReviewsInfo(objData.ModelId);
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

        public UserReviewsSearchVM GetDataDesktop()
        {
            UserReviewsSearchVM objData = new UserReviewsSearchVM();
            objData.ModelId = _modelId;
            objData.ReviewsInfo = ReviewsInfo;
            objData.ActiveReviewCategory = ActiveReviewCateory;

            objData.UserReviews = _userReviewsSearch.GetUserReviewsListDesktop(_filters);
            objData.ObjQuestionValue = _userReviewsCacheRepo.GetReviewQuestionValuesByModel(_modelId);

            if (objData.UserReviews != null)
            {
                if (objData.ReviewsInfo == null)
                {
                    objData.ReviewsInfo = _userReviewsCacheRepo.GetBikeReviewsInfo(objData.ModelId);
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
                        BaseUrl = String.Format("/{0}-bikes/{1}/reviews/", objData.ReviewsInfo.Make.MaskingName, objData.ReviewsInfo.Model.MaskingName),
                        PageUrlType = "page/",
                        TotalResults = objData.UserReviews.TotalCount
                    };
                }


            }


            return objData;
        }
    }
}