using Bikewale.Common;
using Bikewale.DAL.UserReviews;
using Bikewale.Interfaces.UserReviews;
using Microsoft.Practices.Unity;
using System;
using System.Web;

/// <summary>
/// Summary description for AjaxUserReviews
/// </summary>
namespace Bikewale.Ajax
{
    public class AjaxUserReviews
    {
        // write all the Forum Ajax functions here
        // used for writing the debug messages
        private HttpContext objTrace = HttpContext.Current;

        //this function is used to approve the review
        //it accepts reviewId, customerId of reviewer, title of the review, and review about which bike
        [AjaxPro.AjaxMethod()]
        public bool ApproveReview(string reviewId, string customerId, string title, string bike)
        {

            //string sql;
            //string threadId = "", messageText, titleText;
            //bool ret = false;
            //Database db = new Database();

            try
            {
                throw new Exception("ApproveReview(string reviewId, string customerId, string title, string bike) : Method not used/commented");
                //    //the IsVerified status is set to 1 to approve the review
                //    sql = " UPDATE CustomerReviews SET IsVerified=1 WHERE ID = @reviewId ";
                //    SqlParameter[] param1 = { new SqlParameter("@reviewId", reviewId) };
                //    db.UpdateQry(sql, param1);

                //    UpdateReviewRateCount(reviewId);

                //    messageText = "Dear Friends,"
                //                + "<br/><br/>I have written a review on " + bike + " and look forward to your comments on it."
                //                + "<br/><br/>Review Link: <a href=\"https://www.bikewale.com/content/userreviews/reviewdetails.aspx?rid=" + reviewId + "\"><strong>" + title + "</strong></a>&nbsp;"
                //                + "<br/><br/>Thanks.";

                //    titleText = title + " - " + bike + " User Review";

                //    //to create a new thread of the approved review
                //    threadId = ForumsCommon.CreateNewThread(customerId, messageText, 1, "25", titleText);

                //    db = new Database();
                //    sql = " INSERT INTO Forum_ArticleAssociation"
                //        + " (ArticleType,ThreadId,ArticleId,CreateDate)"
                //        + " VALUES(3, @threadId , @reviewId , GETDATE())";

                //    SqlParameter[] param2 = { new SqlParameter("@threadId", threadId), new SqlParameter("@reviewId", reviewId) };
                //    db.InsertQry(sql, param2);
                //    ret = true;

            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "AjaxUserReviews.ApproveReview");
                
                return false;
            }
            //finally
            //{
            //    db.CloseConnection();
            //}
            //return ret;
        }

        //this function is used to approve the review which is being updated by the users
        [AjaxPro.AjaxMethod()]
        public bool ApproveUpdatedReview(string reviewId, string customerId)
        {
            ErrorClass.LogError(new Exception("Method not used/commented"), "AjaxUserReviews.ApproveUpdatedReview");
            
            return false;
            //bool ret = false;
            //try
            //{
            //    UpdateReplicaAsVerified(reviewId);
            //    UpdateChangesToCustomerReviews(reviewId);
            //    UpdateReviewRateCount(reviewId);
            //    AddReplyInForums(reviewId, customerId);
            //    ret = true;
            //}
            //catch (Exception ex)
            //{
            //    ErrorClass.LogError(ex, "AjaxUserReviews.ApproveUpdatedReview");
            //    
            //}
            //return ret;
        }


        [AjaxPro.AjaxMethod()]
        public bool ApproveAbusedReview(string reviewId)
        {
            bool ret = false;
            try
            {
                UpdateAbuseAsVerified(reviewId);
                ret = true;
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "AjaxUserReviews.ApproveUpdatedReview");
                
            }
            return ret;
        }

        //this function deletes the review
        //according to the review id passed and the retsurn value is true or false
        [AjaxPro.AjaxMethod()]
        public bool DeleteReview(string reviewId)
        {
            ErrorClass.LogError(new Exception("Method not used/commented"), "AjaxUserReviews.GetThreadIdForReview");
            
            return false;
            //string sql;
            //bool ret = false;
            //Database db = new Database();

            //try
            //{
            //    //the IsActive status is set to 0 to approve the review
            //    sql = " UPDATE CustomerReviews SET IsActive=0 WHERE ID = @reviewId";
            //    SqlParameter[] param = { new SqlParameter("@reviewId", reviewId) };
            //    db.UpdateQry(sql, param);
            //    ret = true;
            //}
            //catch (Exception ex)
            //{
            //    ErrorClass.LogError(ex, "AjaxUserReviews.ApproveReview");
            //    
            //}
            //finally
            //{
            //    db.CloseConnection();
            //}
            //return ret;
        }


        [AjaxPro.AjaxMethod()]
        public bool DeleteUpdatedReview(string reviewId)
        {
            bool ret = false;
            try
            {
                UpdateReplicaAsVerified(reviewId);
                ret = true;
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "AjaxUserReviews.ApproveUpdatedReview");
                
            }
            return ret;
        }

        [AjaxPro.AjaxMethod()]
        public bool DeleteAbusedReview(string reviewId)
        {
            ErrorClass.LogError(new Exception("Method not used/commented"), "AjaxUserReviews.GetThreadIdForReview");
            
            return false;

            //string sql = "";
            //Database db = new Database();
            //bool ret = false;
            //try
            //{
            //    UpdateAbuseAsVerified(reviewId);
            //    sql = " UPDATE CustomerReviews SET IsActive=0 WHERE ID = @reviewId";
            //    SqlParameter[] param = { new SqlParameter("@reviewId", reviewId) };
            //    db.UpdateQry(sql, param);
            //    UpdateReviewRateCount(reviewId);
            //    ret = true;
            //}
            //catch (Exception ex)
            //{
            //    ErrorClass.LogError(ex, "AjaxUserReviews.ApproveUpdatedReview");
            //    
            //}
            //return ret;
        }

        private void UpdateReplicaAsVerified(string reviewId)
        {

            ErrorClass.LogError(new Exception("Method not used/commented"), "AjaxUserReviews.GetThreadIdForReview");
            
            //string sql;
            //Database db = new Database();

            //try
            //{
            //    sql = " UPDATE CustomerReviewsReplica SET IsVerified=1 WHERE ReviewId = @reviewId";
            //    SqlParameter[] param = { new SqlParameter("@reviewId", reviewId) };
            //    db.UpdateQry(sql, param);
            //}
            //catch (Exception ex)
            //{
            //    ErrorClass.LogError(ex, "AjaxUserReviews.UpdateReplicaAsVerified");
            //    
            //}
            //finally
            //{
            //    db.CloseConnection();
            //}
        }

        private void UpdateAbuseAsVerified(string reviewId)
        {
            ErrorClass.LogError(new Exception("Method not used/commented"), "AjaxUserReviews.GetThreadIdForReview");
            

            //string sql;
            //Database db = new Database();

            //try
            //{
            //    sql = " UPDATE ReviewAbusedDetails SET IsVerified=1 WHERE CustomerReviewId = @reviewId";
            //    SqlParameter[] param = { new SqlParameter("@reviewId", reviewId) };
            //    db.UpdateQry(sql, param);
            //}
            //catch (Exception ex)
            //{
            //    ErrorClass.LogError(ex, "AjaxUserReviews.UpdateAbuseAsVerified");
            //    
            //}
            //finally
            //{
            //    db.CloseConnection();
            //}
        }

        private void UpdateChangesToCustomerReviews(string reviewId)
        {
            ErrorClass.LogError(new Exception("Method not used/commented"), "AjaxUserReviews.GetThreadIdForReview");
            

            //string sql = "";
            //Database db = new Database();
            //try
            //{
            //    sql = " UPDATE CustomerReviews"
            //        + " SET     "
            //        + " StyleR = CustomerReviewsReplica.StyleR,"
            //        + " ComfortR = CustomerReviewsReplica.ComfortR,"
            //        + " PerformanceR = CustomerReviewsReplica.PerformanceR,"
            //        + " ValueR = CustomerReviewsReplica.ValueR,"
            //        + " FuelEconomyR = CustomerReviewsReplica.FuelEconomyR,"
            //        + " OverallR = CustomerReviewsReplica.OverallR,"
            //        + " Pros = CustomerReviewsReplica.Pros,"
            //        + " Cons = CustomerReviewsReplica.Cons,"
            //        + " Title = CustomerReviewsReplica.Title,"
            //        + " Comments = CustomerReviewsReplica.Comments,"
            //        + " IsOwned = CustomerReviewsReplica.IsOwned,"
            //        + " IsNewlyPurchased = CustomerReviewsReplica.IsNewlyPurchased,"
            //        + " Familiarity = CustomerReviewsReplica.Familiarity,"
            //        + " Mileage = CustomerReviewsReplica.Mileage,"
            //        + " LastUpdatedBy = CustomerReviews.CustomerId,"
            //        + " LastUpdatedOn = CustomerReviewsReplica.LastUpdatedOn"
            //        + " FROM "
            //        + " CustomerReviews, CustomerReviewsReplica"
            //        + " WHERE "
            //        + " CustomerReviews.Id = CustomerReviewsReplica.ReviewId"
            //        + " AND CustomerReviewsReplica.ReviewId = @reviewId";

            //    SqlParameter[] param = { new SqlParameter("@reviewId", reviewId) };
            //    db.UpdateQry(sql, param);
            //}
            //catch (Exception ex)
            //{
            //    ErrorClass.LogError(ex, "AjaxUserReviews.UpdateChangesToCustomerReviews");
            //    
            //}
            //finally
            //{
            //    db.CloseConnection();
            //}
        }

        private void AddReplyInForums(string reviewIds, string customerIds)
        {
            ErrorClass.LogError(new Exception("Method not used/commented"), "AjaxUserReviews.GetThreadIdForReview");
            

            //string threadId = "";
            //threadId = GetThreadIdForReview(reviewIds);
            //if (threadId != "-1")
            //{
            //    ForumsCommon.SavePost(customerIds, "This review is updated", 1, threadId);
            //}
        }

        private string GetThreadIdForReview(string reviewId)
        {
            ErrorClass.LogError(new Exception("Method not used/commented"), "AjaxUserReviews.GetThreadIdForReview");
            
            return string.Empty;

            //string returnVal = "-1";
            //string sql = "SELECT ThreadId FROM Forum_ArticleAssociation With(NoLock) WHERE ArticleType = 3 AND ArticleId = @reviewId";

            //SqlDataReader dr = null;
            //Database db = new Database();
            //try
            //{
            //    SqlParameter[] param = { new SqlParameter("@reviewId", reviewId) };
            //    dr = db.SelectQry(sql, param);
            //    if (dr.Read())
            //    {
            //        returnVal = dr[0].ToString();
            //    }
            //}
            //catch (Exception err)
            //{
            //    ErrorClass.LogError(err, "AjaxUserReviews.GetThreadIdForReview");
            //    
            //    returnVal = "-1";
            //}
            //finally
            //{
            //    if(dr != null)
            //        dr.Close();

            //    db.CloseConnection();
            //}

            //return returnVal;
        }


        private void UpdateReviewRateCount(string reviewId)
        {
            ErrorClass.LogError(new Exception("Method not used/commented"), "AjaxUserReviews.UpdateReviewRateCount");
            

        
        }

        //this function updates the liked and the disliked Budget of the customer reviews table
        //according to the review id passed and the helpful value which is either true or false
        [AjaxPro.AjaxMethod()]
        public bool UpdateReviewHelpful(string reviewId, string helpful)
        {

            //check whether this review has already been viewed
            string viewedList = CookiesUserReviews.URHelpful;
            bool viewed = false;
            uint _reviewId;
            bool returnVal = false;


            bool isHelpFull = helpful == "1" ? true : false;

            if (viewedList != "")
            {
                string[] lists = viewedList.Split(',');
                for (int i = 0; i < lists.Length; i++)
                {
                    if (reviewId == lists[i])
                    {
                        viewed = true;
                        break;
                    }
                }
            }

            if (viewed == false)
            {
                CommonOpn op = new CommonOpn();

                try
                {

                    if (uint.TryParse(reviewId, out _reviewId) && _reviewId > 0)
                    {
                        using (IUnityContainer container = new UnityContainer())
                        {
                            container.RegisterType<IUserReviewsRepository, UserReviewsRepository>()
                            .RegisterType<IUserReviewsRepository, UserReviewsRepository>();
                            IUserReviewsRepository objReviews = container.Resolve<IUserReviewsRepository>();

                            returnVal = objReviews.UpdateReviewUseful(_reviewId, isHelpFull);
                        }
                    }

                }
                catch (Exception err)
                {
                    HttpContext.Current.Trace.Warn("Ajaxfunctions : UpdateReviewHelpful : " + err.Message);
                    ErrorClass.LogError(err, "Ajaxfunctions.UpdateReviewHelpful");
                    
                    returnVal = false;
                } // catch Exception
                finally
                {
                    //add this id to helpful
                    CookiesUserReviews.URHelpful += reviewId + ",";
                }
            }

            returnVal = !viewed;

            return returnVal;
        }

        //this function updates the liked and the disliked Budget of the customer reviews table
        //according to the review id passed and the helpful value which is either true or false
        [AjaxPro.AjaxMethod()]
        public bool UpdateReviewAbuse(string reviewId, string comments)
        {
            bool returnVal = false;
            uint _reviewId;

            try
            {
                uint.TryParse(reviewId, out _reviewId);

                //using (DbCommand cmd = DbFactory.GetDBCommand("updatecustomerreviewsabuse"))
                //{
                //    cmd.CommandType = CommandType.StoredProcedure;
                //    cmd.Parameters.Add(DbFactory.GetDbParam("par_reviewid", DbType.Int64, reviewId));
                //    cmd.Parameters.Add(DbFactory.GetDbParam("par_comments", DbType.String, 500, comments));
                //    cmd.Parameters.Add(DbFactory.GetDbParam("par_reportedby", DbType.Int64, CurrentUser.Id));
                //    MySqlDatabase.ExecuteNonQuery(cmd,ConnectionType.ReadOnly);
                //    // Bikewale.Notifications.// LogLiveSps.LogSpInGrayLog(cmd);

                //    returnVal = true;
                //}

                if (_reviewId > 0)
                {
                    using (IUnityContainer container = new UnityContainer())
                    {
                        container.RegisterType<IUserReviewsRepository, UserReviewsRepository>()
                        .RegisterType<IUserReviewsRepository, UserReviewsRepository>();
                        IUserReviewsRepository objReviews = container.Resolve<IUserReviewsRepository>();

                        returnVal = objReviews.AbuseReview(_reviewId, comments, CurrentUser.Id.ToString());
                    }
                }


            }
            catch (Exception err)
            {
                HttpContext.Current.Trace.Warn("Ajaxfunctions : UpdateReviewAbuse : " + err.Message);
                ErrorClass.LogError(err, "Ajaxfunctions.UpdateReviewAbuse");
                
                returnVal = false;
            } // catch Exception

            return returnVal;
        }

        [AjaxPro.AjaxMethod()]
        public string GetModels(string makeId)
        {
            ErrorClass.LogError(new Exception("Method not used/commented"), "AjaxUserReviews.GetModels");
            
            return string.Empty;
            //DataSet ds = new DataSet();

            //if (makeId == "" || CommonOpn.CheckId(makeId) == false)
            //    return "";

            //Database db = new Database();
            //string sql = "";

            //sql = " SELECT ID AS Value, Name AS Text FROM BikeModels With(NoLock) WHERE IsDeleted = 0 AND Futuristic = 0 AND "
            //    + " BikeMakeId =@makeId ORDER BY Text ";

            //try
            //{
            //    SqlParameter[] param = { new SqlParameter("@makeId", makeId) };
            //    ds = db.SelectAdaptQry(sql, param);
            //}
            //catch (Exception err)
            //{
            //    ErrorClass.LogError(err, "AjaxUserReviews.GetModels");
            //    
            //}

            //string jsonString = ds.Tables.Count > 0 ? JSON.GetJSONString(ds.Tables[0]) : "";

            //return jsonString;
        }
    }
}