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
                    var param = new DynamicParameters();

                    param.Add("par_statusid", (ushort)filter.ReviewStatus);
                    param.Add("par_makeid", filter.MakeId > 0 ? filter.MakeId : (uint?)null);
                    param.Add("par_modelid", filter.ModelId > 0 ? filter.ModelId : (uint?)null);
                    param.Add("par_reviewdate", filter.ReviewDate != null && filter.ReviewDate != default(DateTime) ? filter.ReviewDate : (DateTime?)null);

                    objReviews = connection.Query<ReviewBase>("GetUserReviewsListHavingReview", param: param, commandType: CommandType.StoredProcedure);

                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "BikewaleOpr.DALs.UserReviews.GetReviewsList");
            }

            return objReviews;
        }   // End of GetReviewsList

        /// <summary>
        /// Created by Sajal Gupta on 16/06/2017
        /// Summary : Function to get the user reviews ratings list.
        /// </summary>       
        /// <returns></returns>
        public IEnumerable<ReviewBase> GetRatingsList()
        {
            IEnumerable<ReviewBase> objReviews = null;

            try
            {
                using (IDbConnection connection = DatabaseHelper.GetMasterConnection())
                {

                    var param = new DynamicParameters();                    

                    objReviews = connection.Query<ReviewBase>("GetUserReviewsListNotHavingReview", param: param, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "BikewaleOpr.DALs.UserReviews.GetRatingsList");
            }

            return objReviews;
        }


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
                    objReasons = connection.Query<DiscardReasons>("GetUserReviewsDiscardReasons", commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "BikewaleOpr.DALs.UserReviews.GetUserReviewsDiscardReasons");
            }

            return objReasons;

        }   // End of GetUserReviewsDiscardReasons

        /// <summary>
        /// Written By : Ashish G. Kamble on 18 Apr 2017
        /// Summary : Function to approve or discard the user review
        /// Modified by Sajal gupta on 17-05-2017
        /// Description : Added iShortListed to sp.
        /// </summary>
        /// <param name="reviewId"></param>
        /// <param name="status"></param>
        /// <param name="disapprovalReasonId"></param>
        public uint UpdateUserReviewsStatus(uint reviewId, ReviewsStatus reviewStatus, uint moderatorId, ushort disapprovalReasonId, string review, string reviewTitle, string reviewTips, bool iShortListed)
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
                    param.Add("par_isShortListed", iShortListed);
                    param.Add("par_oldTableReviewId", value: 0, dbType: DbType.UInt32, direction: ParameterDirection.Output);

                    connection.Query("updateuserreviewstatus_17052017", param: param, commandType: CommandType.StoredProcedure);

                    oldTableReviewId = param.Get<uint>("par_oldTableReviewId");


                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "BikewaleOpr.DALs.UserReviews.UpdateUserReviewsStatus");
            }

            return oldTableReviewId;

        }   // End of UpdateUserReviewsStatus


        /// <summary>
        /// Created By : Sushil Kumar on 17th April 2017
        /// Description : Get user reviews summary for all pages
        /// Modified by Sajal Gupta on 01-08-2017
        /// Descriptiopn : Added isWinner
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
                                HostUrl = Convert.ToString(dr["hostUrl"]),
                                IsShortListed = SqlReaderConvertor.ToBoolean(dr["isShortListed"]),
                                IsWinner = SqlReaderConvertor.ToBoolean(dr["isWinner"])
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

                ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);

            }

            return objUserReviewSummary;
        }

        /// <summary>
        /// Created by Sajal Gupta on 01-08-2017
        /// Description : Method to get user review summary based on reviewid and emailId
        /// </summary>
        /// <param name="reviewId"></param>
        /// <param name="emailId"></param>
        /// <returns></returns>
        public ReviewBase GetUserReviewWithEmailIdReviewId(uint reviewId, string emailId)
        {
            ReviewBase objUserReview = null;           
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("getUserReviewSummaryByEmailIdReviewId"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_reviewId", DbType.UInt32, reviewId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_emailId", DbType.String, emailId));


                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.MasterDatabase))
                    {
                        if (dr != null && dr.Read())
                        {
                            objUserReview = new ReviewBase()
                            {

                                CustomerEmail = Convert.ToString(dr["CustomerEmail"]),
                                WrittenBy = Convert.ToString(dr["WrittenBy"]),
                                Id = SqlReaderConvertor.ToUInt32(dr["Id"]),
                                MakeId = SqlReaderConvertor.ToUInt32(dr["MakeId"]),
                                ModelId = SqlReaderConvertor.ToUInt32(dr["ModelId"]),
                                MakeName = Convert.ToString(dr["MakeName"]),
                                ModelName = Convert.ToString(dr["ModelName"]),
                                EntryDate = Convert.ToString(dr["EntryDate"]),
                                ReviewStatus = SqlReaderConvertor.ToUInt16(dr["ReviewStatus"])
                            };
                        }                                               
                        dr.Close();
                    }
                }               
            }
            catch (Exception ex)
            {

                ErrorClass.LogError(ex, string.Format("BikewaleOpr.DALs.UserReviews.GetUserReviewSummary {0} {1}", reviewId, emailId));

            }

            return objUserReview;
        }

        /// <summary>
        /// Created by Sajal Gupta on 19-06-2017
        /// Description : Gets details from database for user review ids
        /// </summary>
        /// <param name="reviewIds"></param>
        /// <returns></returns>
        public IEnumerable<BikeRatingApproveEntity> GetUserReviewDetails(string reviewIds)
        {
            List<BikeRatingApproveEntity> objDetailsList = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("getuserreviewdetails"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_reviewids", DbType.String, reviewIds));

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        objDetailsList = new List<BikeRatingApproveEntity>();                       
                        if (dr != null)
                        {
                            while (dr.Read())
                            {
                                objDetailsList.Add(new BikeRatingApproveEntity()
                                {
                                    CustomerEmail = Convert.ToString(dr["customeremail"]),
                                    CustomerName = Convert.ToString(dr["customername"]),
                                    ReviewId = SqlReaderConvertor.ToUInt32(dr["reviewid"]),
                                    ModelId = SqlReaderConvertor.ToUInt32(dr["modelid"]),
                                    ModelMaskingName = Convert.ToString(dr["modelmaskingname"]),
                                    MakeMaskingName = Convert.ToString(dr["MakeMaskingName"]),
                                    BikeName = Convert.ToString(dr["bikename"])
                                });
                            }
                            dr.Close();
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                ErrorClass.LogError(ex, "BikewaleOpr.DALs.UserReviews.GetUserReviewDetails");
            }
            return objDetailsList;
        }

        public bool UpdateUserReviewRatingsStatus(string reviewIds, ReviewsStatus reviewStatus, uint moderatorId, ushort disapprovalReasonId)
        {            
            try
            {
                using (IDbConnection connection = DatabaseHelper.GetMasterConnection())
                {
                    var param = new DynamicParameters();

                    param.Add("par_reviewIds", reviewIds);
                    param.Add("par_moderatorId", moderatorId);
                    param.Add("par_status", (ushort)reviewStatus);
                    param.Add("par_disapproveId", disapprovalReasonId > 0 ? disapprovalReasonId : (ushort?)null);                                        


                    connection.Query("updateuserreviewratingsstatus_17072017", param: param, commandType: CommandType.StoredProcedure);                    

                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "BikewaleOpr.DALs.UserReviews.UpdateUserReviewRatingsStatus");
                return false;
            }
            return true;
        }

        /// <summary>
        /// Created by Sajal Gupta on 01-08-2017
        /// Descriptiopn : Function to save user review winner
        /// </summary>
        /// <param name="reviewId"></param>
        /// <param name="moderatorId"></param>
        /// <returns></returns>
        public bool SaveUserReviewWinner(uint reviewId, uint moderatorId)
        {
            try
            {
                using (IDbConnection connection = DatabaseHelper.GetMasterConnection())
                {
                    var param = new DynamicParameters();

                    param.Add("par_reviewid", reviewId);
                    param.Add("par_moderatorId", moderatorId);
                   
                    connection.Query("saveuserreviewswinner", param: param, commandType: CommandType.StoredProcedure);

                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("BikewaleOpr.DALs.UserReviews.SaveUserReviewWinner {0} {1}", reviewId, moderatorId));
                return false;
            }
            return true;
        }

    }   // class
}   // namespace
