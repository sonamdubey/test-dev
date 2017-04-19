using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bikewale.DAL.CoreDAL;
using Bikewale.Notifications;
using BikewaleOpr.Entity.UserReviews;
using BikewaleOpr.Interface.UserReviews;
using Dapper;

namespace BikewaleOpr.DALs.UserReviews
{
    /// <summary>
    /// Created By : Ashish G. Kamble on 17 Apr 2017
    /// Summary : Class have functions related to user reviews
    /// </summary>
    public class UserReviewsRepository : IUserReviewsRepository
    {
        /// <summary>
        /// Written By : Ashish G. Kamble on 15 Apr 207
        /// Summary : Function to get the user reviews list.
        /// </summary>
        /// <param name="filter">Filters to get specific reviews.</param>
        /// <returns></returns>
        public IEnumerable<ReviewBase> GetReviewsList(ReviewsInputFilters filter)
        {
            IEnumerable<ReviewBase> objReviews = null;

            try
            {
                using (IDbConnection connection = DatabaseHelper.GetMasterConnection())
                {
                    connection.Open();

                    var param = new DynamicParameters();

                    param.Add("par_statusid", (ushort)filter.ReviewStatus);
                    param.Add("par_makeid", filter.MakeId > 0 ? filter.MakeId : (uint?)null);
                    param.Add("par_modelid", filter.ModelId > 0 ? filter.ModelId : (uint?)null);
                    param.Add("par_reviewdate", filter.ReviewDate != default(DateTime) ? filter.ReviewDate : (DateTime?)null);

                    objReviews = connection.Query<ReviewBase>("GetUserReviewsList", param: param, commandType: CommandType.StoredProcedure);

                    if (connection.State == ConnectionState.Open)
                        connection.Close();
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "BikewaleOpr.DALs.UserReviews.GetReviewsList");
            }

            return objReviews;
        }   // End of GetReviewsList


        /// <summary>
        /// Written By : Ashish G. Kamble on 18 Apr 2017
        /// Summary : Function to get the user reviews discard/ rejection reasons
        /// Summary : 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<DiscardReasons> GetUserReviewsDiscardReasons()
        {
            IEnumerable<DiscardReasons> objReasons = null;

            try
            {
                using (IDbConnection connection = DatabaseHelper.GetReadonlyConnection())
                {
                    connection.Open();

                    objReasons = connection.Query<DiscardReasons>("GetUserReviewsDiscardReasons", commandType: CommandType.StoredProcedure);

                    if (connection.State == ConnectionState.Open)
                        connection.Close();
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "BikewaleOpr.DALs.UserReviews.GetUserReviewsDiscardReasons");
            }

            return objReasons;

        }   // End of GetUserReviewsDiscardReasons

        /// <summary>
        /// Written By : Ashish G. Kamble on 18 Apr 2017
        /// Summary : Function to approve or discard the user review
        /// </summary>
        /// <param name="reviewId"></param>
        /// <param name="status"></param>
        /// <param name="disapprovalReasonId"></param>
        public void UpdateUserReviewsStatus(uint reviewId, ReviewsStatus reviewStatus, uint moderatorId, ushort disapprovalReasonId, string review, string reviewTitle, string reviewTips)
        {
            try
            {
                using (IDbConnection connection = DatabaseHelper.GetMasterConnection())
                {
                    var param = new DynamicParameters();

                    param.Add("par_reviewId", reviewId);
                    param.Add("par_moderatorId", moderatorId);
                    param.Add("par_status", (ushort)reviewStatus);
                    param.Add("par_disapproveId", disapprovalReasonId > 0 ? disapprovalReasonId : (ushort?)null);
                    param.Add("par_review", String.IsNullOrEmpty(review) ? null : review);
                    param.Add("par_title", String.IsNullOrEmpty(reviewTitle) ? null : reviewTitle);
                    param.Add("par_tips", String.IsNullOrEmpty(reviewTips) ? null : reviewTips);

                    connection.Open();

                    connection.Query("changeuserreviewstatus", param: param, commandType: CommandType.StoredProcedure);

                    if (connection.State == ConnectionState.Open)
                        connection.Close();
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "BikewaleOpr.DALs.UserReviews.UpdateUserReviewsStatus");
            }
        }   // End of UpdateUserReviewsStatus

    }   // class
}   // namespace
