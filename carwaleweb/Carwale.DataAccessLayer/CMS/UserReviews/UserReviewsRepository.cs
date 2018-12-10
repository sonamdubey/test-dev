using Carwale.DAL.CoreDAL.MySql;
using Carwale.Entity.CMS;
using Carwale.Entity.CMS.UserReviews;
using Carwale.Entity.Enum;
using Carwale.Entity.UserReviews;
using Carwale.Interfaces.CMS.UserReviews;
using Carwale.Notifications;
using Carwale.Notifications.Logs;
using Carwale.Utility;
using Dapper;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Linq;

namespace Carwale.DAL.CMS.UserReviews
{
    public class UserReviewsRepository : RepositoryBase, IUserReviewsRepository
    {
        public UserReviewDetail GetUserReviewDetailById(int reviewId, CMSAppId applicationId)
        {
            UserReviewDetail userReviewDetail;
            try
            {
                var param = new DynamicParameters();
                if (reviewId > 0)
                {
                    param.Add("v_ReviewId", reviewId);
                }

                using (var con = CarDataMySqlReadConnection)
                {
                    userReviewDetail = con.Query<UserReviewDetail>("GetUserReviewDetailsById_v16_11_7", param, commandType: CommandType.StoredProcedure).Single();
                }
                if (userReviewDetail != null)
                {
                    var param1 = new DynamicParameters();
                    param1.Add("v_ReviewId", reviewId);
                    using (var con1 = ForumsMySqlReadConnection)
                    {
                        userReviewDetail.CommentsCount = con1.Query<Int32>("GetCommentsCount_v16_11_7", param1, commandType: CommandType.StoredProcedure).Single().ToString();
                    }
                    if (userReviewDetail.MinPrice != null)
                    {
                        userReviewDetail.StartPrice = Format.GetPrice(userReviewDetail.MinPrice.ToString());
                    }
                    userReviewDetail.ShareUrl = ManageCarUrl.CreateUserReviewDetailsUrl(Format.FormatSpecial(userReviewDetail.Make), userReviewDetail.MaskingName, reviewId, isAbsoluteUrl: true);
                    userReviewDetail.Author = !string.IsNullOrEmpty(userReviewDetail.HandleName) ? userReviewDetail.HandleName : userReviewDetail.CustomerName;

                    userReviewDetail.ReviewDate = Format.GetDisplayTimeSpan(userReviewDetail.EntryDateTime.ToString());
                    userReviewDetail.ReviewRate = Format.GetAbsReviewRate(Convert.ToDouble(userReviewDetail.OverallR.ToString()));
                    userReviewDetail.PurchasedAs = Format.PurchasedAs(userReviewDetail.IsOwned, userReviewDetail.IsNewlyPurchased);
                    userReviewDetail.FuelEconomy = Format.GetFuelEconomy(userReviewDetail.Mileage.ToString());
                    userReviewDetail.ReviewCommentsUrl = "http://" + (ConfigurationManager.AppSettings["HostUrl"] ?? "") + "/api/ReviewsComments?reviewId=" + userReviewDetail.ReviewId + "&pageNo=1&pageSize=10";
                }
            }
            catch (Exception err)
            {
                ExceptionHandler objErr = new ExceptionHandler(err, "GetUserReviewDetailById DAL in Exception- GetUserReviewDetailById()");
                objErr.LogException();
                throw;
            }
            return userReviewDetail;
        }

        public List<UserReviewEntity> GetUserReviewsList(int makeId, int modelId, int versionId, int start, int end, int sortCriteria)
        {
            List<UserReviewEntity> userReviewList = new List<UserReviewEntity>();
            try
            {
                var param = new DynamicParameters();
                param.Add("v_ModelId", modelId);
                param.Add("v_MakeId", makeId);
                param.Add("v_StartIndex", start);
                param.Add("v_EndIndex", end);
                param.Add("v_SortCriteria", sortCriteria);
                param.Add("v_VersionId", versionId > 0 ? versionId : 0);

                using (var con = CarDataMySqlReadConnection)
                {
                    userReviewList = con.Query<UserReviewEntity>("GetUserReviews_v16_11_7", param, commandType: CommandType.StoredProcedure).AsList();
                }
                if (userReviewList != null)
                {
                    foreach (var review in userReviewList)
                    {
                        var param1 = new DynamicParameters();
                        DataSet ds = new DataSet();
                        param1.Add("v_ReviewId", review.ReviewId);
                        using (var con1 = ForumsMySqlReadConnection)
                        {
                            ds = con1.Query<DataSet>("GetThreadAndCommentByReviewId_v16_11_7", param1, commandType: CommandType.StoredProcedure).FirstOrDefault();
                        }
                        if (ds != null && ds.Tables.Count > 0)
                        {
                            int threadId, comments;
                            int.TryParse(ds.Tables[0].Rows[0]["ThreadId"].ToString(), out threadId);
                            int.TryParse(ds.Tables[0].Rows[0]["Comments"].ToString(), out comments);
                            review.ThreadId = threadId;
                            review.Comments = comments;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "UserReviewsRepository.GetUserReviewsList()\n Exception : " + ex.Message);
                objErr.LogException();
                throw;
            }
            return userReviewList;
        }

        public string GetUserReviewedIdsByModel(int modelId)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("v_ModelId", modelId);

                using (var con = CarDataMySqlReadConnection)
                {
                    return con.Query<string>("GetCommaSeperatedReviewIdByModel_v16_11_7", param, commandType: CommandType.StoredProcedure).First();
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "UserReviewsRepository.GetUserReviewedIdsByModel()\n Exception : " + ex.Message);
                objErr.LogException();
                throw;
            }
        }
        public List<CarReviewBaseEntity> GetMostReviewedCars()
        {
            try
            {
                var param = new DynamicParameters();
                using (var con = CarDataMySqlReadConnection)
                {
                    return con.Query<CarReviewBaseEntity>("GetMostReviewedCars_v16_11_7", param, commandType: CommandType.StoredProcedure).ToList();
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "UserReviewsRepository.GetMostReviewedCars()\n Exception : " + ex.Message);
                objErr.LogException();
                throw;
            }
        }

        public List<UserReviewEntity> GetUserReviewsByType(UserReviewsSorting type)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("v_type", (int)type);
                using (var con = CarDataMySqlReadConnection)
                {
                    return con.Query<UserReviewEntity>("GetUserReviewByTypes_v16_11_7", param, commandType: CommandType.StoredProcedure).ToList();
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "UserReviewsRepository.GetUserReviewsByType()\n Exception : " + ex.Message);
                objErr.LogException();
                throw;
            }
        }

        public bool CheckVersionReview(string versionId, string email, string customerId, string modelId)
        {
            bool found = false;
            //string id = versionId == "" ? drpVersions.SelectedItem.Value : versionId;
            //string email = txtEmail.Text.Trim().Replace("'", "''");
            try
            {
                if (customerId != "-1" || !string.IsNullOrEmpty(email))
                {
                    using (DbCommand cmd = DbFactory.GetDBCommand("GetUserReviewCount_v17_6_1"))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DbFactory.GetDbParam("v_CustomerId", DbType.Int64, customerId != "-1" ? customerId : Convert.DBNull));
                        cmd.Parameters.Add(DbFactory.GetDbParam("v_VersionId", DbType.Int32, !string.IsNullOrWhiteSpace(versionId) ? versionId : Convert.DBNull));
                        cmd.Parameters.Add(DbFactory.GetDbParam("v_ModelId", DbType.Int32, !string.IsNullOrWhiteSpace(modelId) ? modelId : Convert.DBNull));
                        cmd.Parameters.Add(DbFactory.GetDbParam("v_Email", DbType.String, 100, !string.IsNullOrWhiteSpace(email) ? email : Convert.DBNull));
                        using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, DbConnections.CarDataMySqlReadConnection))
                        {
                            if (dr.Read())
                            {
                                if (Convert.ToInt32(dr[0]) > 0)
                                    found = true;
                                else
                                    found = false;
                            }
                        }
                    }
                }
            }
            catch (MySqlException err)
            {
                ErrorClass objErr = new ErrorClass(err, "DAL/CMS/UserReview/CheckVersionsReview");
                objErr.SendMail();
            } // catch Exceptio
            return found;
        }

        public string SaveDetails(UserReviewDetail userReviewDetail)
        {
            string recordId = string.Empty;
            try
            {

                using (DbCommand cmd = DbFactory.GetDBCommand("EntryCustomerReviews_v17_6_1"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_CustomerId", DbType.Int64, userReviewDetail.CustomerId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_MakeId", DbType.Int32, userReviewDetail.MakeId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_ModelId", DbType.Int32, userReviewDetail.ModelId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_VersionId", DbType.Int32, String.IsNullOrEmpty(userReviewDetail.VersionId) ? -1 : Convert.ToInt32(userReviewDetail.VersionId)));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_StyleR", DbType.Int16, userReviewDetail.StyleR));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_ComfortR", DbType.Int16, userReviewDetail.ComfortR));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_PerformanceR", DbType.Int16, userReviewDetail.PerformanceR));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_ValueR", DbType.Int16, userReviewDetail.ValueR));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_FuelEconomyR", DbType.Int16, userReviewDetail.FuelEconomyR));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_OverallR", DbType.Decimal, userReviewDetail.OverallR));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_Pros", DbType.String, 100, userReviewDetail.Pros));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_Cons", DbType.String, 100, userReviewDetail.Cons));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_Comments", DbType.String, 8000, userReviewDetail.Comments));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_Title", DbType.String, 100, userReviewDetail.Title));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_EntryDateTime", DbType.DateTime, DateTime.Now));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_ID", DbType.Int32, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_IsOwned", DbType.Boolean, userReviewDetail.IsOwned));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_IsNewlyPurchased", DbType.Boolean, userReviewDetail.IsNewlyPurchased));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_Familiarity", DbType.Int32, userReviewDetail.Familiarity));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_Mileage", DbType.Decimal, userReviewDetail.Mileage));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_PlatformId", DbType.Int32, userReviewDetail.PlatformId));
                    MySqlDatabase.InsertQuery(cmd, DbConnections.CarDataMySqlMasterConnection);
                    recordId = cmd.Parameters["v_ID"].Value.ToString();
                }
            }
            catch (MySqlException ex)
            {
                ErrorClass objErr = new ErrorClass(ex, string.Format("DAL/UserReviewRepository/SaveDetails, error - {0}", ex.ToString()));
                objErr.SendMail();
            } // catch SqlException
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, string.Format("DAL/UserReviewRepository/SaveDetails, error - {0}", ex.ToString()));
                objErr.SendMail();
            } // catch Exception
            return recordId;
        }

        public int SaveRating(RatingDetails ratingDetails)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("OverallR", ratingDetails.Rating.UserRating);
                param.Add("CustomerId", ratingDetails.CustomerId);
                param.Add("MakeId", ratingDetails.CarDetails.MakeId);
                param.Add("ModelId", ratingDetails.CarDetails.ModelId);
                param.Add("VersionId", ratingDetails.CarDetails.VersionId);
                param.Add("EntryDateTime", DateTime.Now);
                param.Add("IsOwned", ratingDetails.Rating.IsOwned);
                param.Add("IsNewlyPurchased", ratingDetails.Rating.IsNewlyPurchased);
                param.Add("Familiarity", ratingDetails.Rating.Familiarity);
                param.Add("PlatformId", ratingDetails.PlatformId);

                string cmd = @"Insert Into cwexperience.customerreviews 
                                        (CustomerId, MakeId, ModelId, VersionId, EntryDateTime, IsOwned, IsNewlyPurchased, Familiarity, PlatformId,OverallR) 
                                 Values (@CustomerId, @MakeId, @ModelId, @VersionId, @EntryDateTime, @IsOwned, @IsNewlyPurchased, @Familiarity, @PlatformId,@OverallR);
                                 SELECT LAST_INSERT_ID();";

                using (var con = CarDataMySqlMasterConnection)
                {
                    return con.Query<int>(cmd, param, commandType: CommandType.Text).FirstOrDefault();
                }
            }
            catch (MySqlException ex)
            {
                Logger.LogException(ex);
                return 0;
            } // catch SqlException            
        }

        public int GetReviewId(int customerId, int versionId)
        {
            try
            {
                string cmd = "Select Id From cwexperience.customerreviews where CustomerId = @CustomerId And VersionId = @VersionId and IsActive = 1;";
                var param = new DynamicParameters();
                param.Add("CustomerId", customerId);
                param.Add("VersionId", versionId);
                using (var con = CarDataMySqlMasterConnection)
                {
                    return con.Query<int>(cmd, param, commandType: CommandType.Text).FirstOrDefault();
                }
            }
            catch (MySqlException ex)
            {
                Logger.LogException(ex, "GetReviewId");
                return -1;
            }
        }

        public Tuple<int, bool> UpdateRating(RatingDetails ratingDetails)
        {
            int reviewId = 0;
            try
            {
                var param = new DynamicParameters();
                param.Add("OverallR", ratingDetails.Rating.UserRating);
                param.Add("CustomerId", ratingDetails.CustomerId);
                param.Add("ModelId", ratingDetails.CarDetails.ModelId);
                param.Add("VersionId", ratingDetails.CarDetails.VersionId);
                param.Add("EntryDateTime", DateTime.Now);
                param.Add("IsOwned", ratingDetails.Rating.IsOwned);
                param.Add("IsNewlyPurchased", ratingDetails.Rating.IsNewlyPurchased);
                param.Add("Familiarity", ratingDetails.Rating.Familiarity);
                param.Add("PlatformId", ratingDetails.PlatformId);
                param.Add("ReviewId", ratingDetails.ReviewId);

                string cmd = @"Insert Into cwexperience.customerreviewsreplica 
                                (ReviewId, CustomerId, ModelId, VersionId, EntryDateTime, IsOwned, IsNewlyPurchased, Familiarity, PlatformId,OverallR) 
                                Values (@ReviewId,@CustomerId, @ModelId, @VersionId, @EntryDateTime, @IsOwned, @IsNewlyPurchased, @Familiarity, @PlatformId,@OverallR);
                                Select LAST_INSERT_ID();";
                using (var con = CarDataMySqlMasterConnection)
                {
                    reviewId = con.Query<int>(cmd, param, commandType: CommandType.Text).FirstOrDefault();
                    if (reviewId > 0)
                    {
                        return Tuple.Create<int, bool>(reviewId, false);
                    }
                }
            }
            catch (MySqlException ex)
            {
                Logger.LogException(ex);
            } // catch SqlException
            catch (Exception ex)
            {
                Logger.LogException(ex);
            } // catch Exception
            return Tuple.Create<int, bool>(ratingDetails.ReviewId, false);
        }

        public int SaveCustomerDetails(string name, string email)
        {
            int customerId = 0;
            try
            {
                var param = new DynamicParameters();

                param.Add("Name", name);
                param.Add("Email", email);
                param.Add("IsEmailVerified", false);
                string cmd = @"Select id from cwexperience.userreviewcustomers where Email = @Email;";
                using (var con = CarDataMySqlMasterConnection)
                {
                    customerId = con.Query<int>(cmd, param, commandType: CommandType.Text).FirstOrDefault();
                    if (customerId <= 0)
                    {
                        cmd = @"Insert Into cwexperience.userreviewcustomers (Name, Email, IsEmailVerified, CreatedOn, UpdatedOn) Values(@Name, @Email, @IsEmailVerified, Now(), Now());
                                Select LAST_INSERT_ID();";
                        return con.Query<int>(cmd, param, commandType: CommandType.Text).FirstOrDefault();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return customerId;
            }
            return customerId;
        }

        public int ProcessEmailVerfication(bool isEmailVerified, int userId)
        {
            try
            {
                string query = @"UPDATE cwmasterdb.customers SET IsEmailVerified = @IsEmailVerified, LastEMailOn = now() WHERE Id = @UserId AND isEmailVerified=0 ;";
                using (var con = CarDataMySqlMasterConnection)
                {
                    int rowCount = con.Execute(query, new { IsEmailVerified = isEmailVerified, UserId = userId }, commandType: CommandType.Text);
                    return rowCount;
                }
            }
            catch (MySqlException ex)
            {
                Logger.LogException(ex);
                return 0;
            }
        }
        public UserReviewCustomerInfo GetUserReviewCustomerById(int userId)
        {
            try
            {
                string query = @"SELECT * FROM cwmasterdb.customers WHERE Id = @UserId;";
                using (var con = CarDataMySqlMasterConnection)
                {
                    UserReviewCustomerInfo customerInfo = con.Query<UserReviewCustomerInfo>(query, new { UserId = userId }, commandType: CommandType.Text).FirstOrDefault();
                    return customerInfo;
                }
            }
            catch (MySqlException ex)
            {
                Logger.LogException(ex);
                return null;
            }
        }
        public UserReviewReplicaEntity GetLatestUserReviewReplicaByReviewId(int reviewId)
        {
            try
            {
                string query = @"SELECT * FROM cwexperience.customerreviewsreplica 
                                 WHERE ReviewId = @ReviewId AND IsActive = 1 ORDER BY id DESC LIMIT 1;";
                using (var con = CarDataMySqlMasterConnection)
                {
                    return con.Query<UserReviewReplicaEntity>(query, new { ReviewId = reviewId }, commandType: CommandType.Text).FirstOrDefault();
                }
            }
            catch (MySqlException ex)
            {
                Logger.LogException(ex);
                return null;
            }
        }
        public Tuple<int, int> InvalidateUserReviewReplica(int replicaId)
        {
            try
            {
                string query = @"UPDATE cwexperience.customerreviewsreplica SET IsActive = 0 WHERE id = @ReplicaId;
                                 SELECT ReviewId,CustomerId FROM cwexperience.customerreviewsreplica  WHERE id = @ReplicaId;";
                using (var con = CarDataMySqlMasterConnection)
                {
                    var result = con.Query<dynamic>(query, new { ReplicaId = replicaId }, commandType: CommandType.Text).FirstOrDefault();
                    return (new Tuple<int, int>((int)(result?.ReviewId ?? 0), (int)(result?.CustomerId ?? 0)));
                }
            }
            catch (MySqlException ex)
            {
                Logger.LogException(ex);
                return (new Tuple<int, int>(0, 0));
            }
        }
        public int UpdateReviewWithLatestReplica(UserReviewReplicaEntity replicaDetails)
        {
            try
            {
                string query = @"UPDATE cwexperience.customerreviews SET StyleR = @StyleR,ComfortR = @ComfortR,PerformanceR = @PerformanceR,
                                ValueR = @ValueR, FuelEconomyR = @FuelEconomyR, OverallR = @OverallR, Pros = @Pros, Cons = @Cons, Comments = @Comments, 
                                Title = @Title, EntryDateTime = @EntryDateTime, IsVerified = 1, ReportAbused = @ReportAbused, Liked = @Liked, 
                                Disliked =@Disliked, Viewed = @Viewed, ModeratorRecommendedReview = @ModeratorRecommendedReview, IsActive = 1, 
                                LastUpdatedOn = @LastUpdatedOn, LastUpdatedBy = @LastUpdatedBy, IsOwned = @IsOwned, IsNewlyPurchased = @IsNewlyPurchased,
                                Familiarity = @Familiarity, Mileage = @Mileage, PlatformId = @PlatformId, SourceId = @SourceId, MovedToForums = @MovedToForums  
                                WHERE id = @ReviewId;";
                using (var con = CarDataMySqlMasterConnection)
                {
                    int rowCount = con.Execute(query, replicaDetails, commandType: CommandType.Text);
                    return rowCount;
                }
            }
            catch (MySqlException ex)
            {
                Logger.LogException(ex);
                return -1;
            }
        }
        public int DeleteUserReview(int reviewId)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("v_ReviewId", reviewId);
                using (var con = CarDataMySqlMasterConnection)
                {
                    return con.Execute("DeleteCustomerReview_v16_11_7", param, commandType: CommandType.StoredProcedure);
                }
            }
            catch (MySqlException ex)
            {
                Logger.LogException(ex);
                return -1;
            }
        }
        public bool IsVerifiedReviewReplica(int replicaId)
        {
            try
            {
                string query = @"SELECT IsVerified FROM cwexperience.customerreviewsreplica  WHERE id = @ReplicaId;";
                using (var con = CarDataMySqlMasterConnection)
                {
                    return con.Query<int>(query, new { ReplicaId = replicaId }, commandType: CommandType.Text).FirstOrDefault() > 0;
                }
            }
            catch (MySqlException ex)
            {
                Logger.LogException(ex);
                return true;
            }
        }
        public bool IsActiveReview(int reviewId)
        {
            try
            {
                string query = @"SELECT IsActive FROM cwexperience.customerreviews  WHERE id = @ReviewId;";
                using (var con = CarDataMySqlMasterConnection)
                {
                    return con.Query<int>(query, new { ReviewId = reviewId }, commandType: CommandType.Text).FirstOrDefault() > 0;
                }
            }
            catch (MySqlException ex)
            {
                Logger.LogException(ex);
                return true;
            }
        }
    }
}
