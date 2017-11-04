﻿using BikewaleOpr.Entity.UserReviews;
using BikewaleOpr.Interface.BikeData;
using BikewaleOpr.Interface.UserReviews;
using BikeWaleOpr.Common;
using System;
using System.Collections.Generic;

namespace BikewaleOpr.Models.UserReviews
{
    /// <summary>
    /// 
    /// </summary>
    public class ManageUserReviewsPageModel
    {
        private readonly IUserReviewsRepository _reviewsRepo = null;
        private readonly IBikeMakesRepository _makesRepo = null;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reviewsRepo"></param>
        /// <param name="makesRepo"></param>
        public ManageUserReviewsPageModel(IUserReviewsRepository reviewsRepo, IBikeMakesRepository makesRepo)
        {
            _reviewsRepo = reviewsRepo;
            _makesRepo = makesRepo;
        }

        /// <summary>
        /// Modified by Sajal Gupta on 01-08-2017
        /// Description: Call different function if search review id and emailid are passed
        /// </summary>
        /// <param name="filters"></param>
        /// <returns></returns>
        public ManageUserReviewsPageVM GetData(ReviewsInputFilters filters)
        {
            ManageUserReviewsPageVM objPageModel = new ManageUserReviewsPageVM();

            try
            {
                if ((ushort)filters.ReviewStatus == 0)
                    filters.ReviewStatus = ReviewsStatus.Pending;

                objPageModel.selectedFilters = filters;

                objPageModel.Makes = _makesRepo.GetMakes(8);

                if (!string.IsNullOrEmpty(filters.SearchEmailId) && filters.SearchReviewId > 0)
                {
                    var reviewList = new List<ReviewBase>();
                    ReviewBase objReview = _reviewsRepo.GetUserReviewWithEmailIdReviewId(filters.SearchReviewId, filters.SearchEmailId);

                    if (objReview != null)
                        reviewList.Add(_reviewsRepo.GetUserReviewWithEmailIdReviewId(filters.SearchReviewId, filters.SearchEmailId));

                    objPageModel.Reviews = reviewList;
                }
                else
                    objPageModel.Reviews = _reviewsRepo.GetReviewsList(filters);

                objPageModel.currentUserId = Convert.ToInt32(CurrentUser.Id);
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "BikewaleOpr.Models.UserReviews.ManageUserReviewsPageModel");
            }
            return objPageModel;
        }

    }   // Class
}   // namespace