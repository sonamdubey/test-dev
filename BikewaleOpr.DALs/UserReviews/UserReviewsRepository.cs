using Bikewale.DAL.CoreDAL;
using Bikewale.Notifications;
using Bikewale.Utility;
using BikewaleOpr.Entities;
using BikewaleOpr.Entities.UserReviews;
using BikewaleOpr.Entity.UserReviews;
using BikewaleOpr.Interface.UserReviews;
using Dapper;
using MySql.CoreDAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Web;

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
        public uint UpdateUserReviewsStatus(uint reviewId, ReviewsStatus reviewStatus, uint moderatorId, ushort disapprovalReasonId, string review, string reviewTitle, string reviewTips)
        {
            uint oldTableReviewId = 0;

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
                    param.Add("par_oldTableReviewId", value: 0, dbType: DbType.UInt32, direction: ParameterDirection.Output);

                    connection.Open();

                    connection.Query("changeuserreviewstatus", param: param, commandType: CommandType.StoredProcedure);

                    oldTableReviewId = param.Get<uint>("par_oldTableReviewId");

                    if (connection.State == ConnectionState.Open)
                        connection.Close();
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "BikewaleOpr.DALs.UserReviews.UpdateUserReviewsStatus");
            }

            return oldTableReviewId;

        }   // End of UpdateUserReviewsStatus


        /// <summary>
        /// Created By : Sushil Kumar on 17th April 2017
        /// Description : Get user reviews summary for all pages
        /// </summary>
        /// <param name="reviewId"></param>
        /// <returns></returns>
        public UserReviewSummary GetUserReviewSummary(uint reviewId)
        {
            UserReviewSummary objUserReviewSummary = null;
            IList<UserReviewRating> objUserReviewrating = null;

            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("getUserReviewSummaryWithRating"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_reviewId", DbType.UInt32, reviewId));

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.MasterDatabase))
                    {
                        if (dr != null && dr.Read())
                        {
                            objUserReviewSummary = new UserReviewSummary()
                            {

                                CustomerEmail = Convert.ToString(dr["CustomerEmail"]),
                                CustomerName = Convert.ToString(dr["CustomerName"]),
                                Description = Convert.ToString(dr["Comments"]),
                                Title = Convert.ToString(dr["ReviewTitle"]),
                                Tips = Convert.ToString(dr["ReviewTips"]),
                                OverallRatingId = SqlReaderConvertor.ToUInt16(dr["overallratingId"]),
                                OverallRating = new UserReviewOverallRating()
                                {
                                    Id = SqlReaderConvertor.ToUInt16(dr["overallratingId"]),
                                    Value = SqlReaderConvertor.ToUInt16(dr["Rating"]),
                                    Heading = Convert.ToString(dr["Heading"]),
                                    Description = Convert.ToString(dr["Description"]),
                                },
                                Make = new BikeMakeEntityBase()
                                {
                                    MakeId = SqlReaderConvertor.ToInt32(dr["makeid"]),
                                    MaskingName = Convert.ToString(dr["makemasking"]),
                                    MakeName = Convert.ToString(dr["makeName"])
                                },
                                Model = new BikeModelEntityBase()
                                {
                                    ModelId = SqlReaderConvertor.ToInt32(dr["modelId"]),
                                    MaskingName = Convert.ToString(dr["modelmasking"]),
                                    ModelName = Convert.ToString(dr["modelName"])
                                },
                                OriginalImgPath = Convert.ToString(dr["OriginalImgPath"]),
                                HostUrl = Convert.ToString(dr["hostUrl"])
                            };
                        }

                        if (objUserReviewSummary != null && dr.NextResult())
                        {
                            var objQuestions = new List<UserReviewQuestion>();
                            while (dr.Read())
                            {
                                objQuestions.Add(new UserReviewQuestion()
                                {
                                    SelectedRatingId = SqlReaderConvertor.ToUInt32(dr["answerValue"]),
                                    Id = SqlReaderConvertor.ToUInt32(dr["QuestionId"]),
                                    Heading = Convert.ToString(dr["Heading"]),
                                    Description = Convert.ToString(dr["Description"]),
                                    DisplayType = (UserReviewQuestionDisplayType)Convert.ToInt32(dr["DisplayType"]),
                                    Type = (UserReviewQuestionType)Convert.ToInt32(dr["QuestionType"]),
                                    Order = SqlReaderConvertor.ToUInt16(dr["DisplayOrder"])
                                });
                            }
                            objUserReviewSummary.Questions = objQuestions;
                        }

                        if (objUserReviewSummary != null && dr.NextResult())
                        {
                            objUserReviewrating = new List<UserReviewRating>();
                            while (dr.Read())
                            {
                                objUserReviewrating.Add(
                                    new UserReviewRating()
                                    {
                                        Id = SqlReaderConvertor.ToUInt32(dr["RatingId"]),
                                        QuestionId = SqlReaderConvertor.ToUInt32(dr["QuestionId"]),
                                        Text = Convert.ToString(dr["RatingText"]),
                                        Value = Convert.ToString(dr["RatingValue"])
                                    });
                            }
                        }

                        dr.Close();
                    }
                }

                if (objUserReviewSummary != null)
                {

                    foreach (var question in objUserReviewSummary.Questions)
                    {
                        var objRating = objUserReviewrating.Where(q => q.QuestionId == question.Id && question.SelectedRatingId.ToString() == q.Value);
                        question.Rating = objRating;
                    }
                }
            }

            catch (Exception ex)
            {

                ErrorClass errObj = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);

            }

            return objUserReviewSummary;
        }
    }   // class
}   // namespace
