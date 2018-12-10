using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using Carwale.UI.Common;
using AjaxPro;
using Carwale.DAL.CoreDAL;
using Carwale.Notifications;
using Carwale.DAL.Forums;
using Carwale.Notifications.Logs;
using System.Data.Common;
using Carwale.DAL.CoreDAL.MySql;
using Microsoft.Practices.Unity;
using Carwale.Interfaces.UesrReview;
using Carwale.Entity.Enum;
using Carwale.DAL.UserReview;

namespace CarwaleAjax
{
    public class AjaxUserReviews
    {
        // write all the Forum Ajax functions here
        // used for writing the debug messages
        private HttpContext objTrace = HttpContext.Current;
        private readonly IUserReviewRepository _userReveiwRepo;
        public AjaxUserReviews()
        {
            using (IUnityContainer container = new UnityContainer())
            {
                container.RegisterType<IUserReviewRepository, UserReviewRepository>();
                _userReveiwRepo = container.Resolve<IUserReviewRepository>();
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.None)]
        public bool ApproveReview(string reviewId, string customerId, string title, string car)
        {      
            string threadId = "", messageText, titleText;
            bool ret = false;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("ApproveReview_v16_11_7"))
                 {
                     cmd.CommandType = CommandType.StoredProcedure;
                     cmd.Parameters.Add(DbFactory.GetDbParam("v_ReviewId", DbType.Int64, reviewId));
                     MySqlDatabase.UpdateQuery(cmd,DbConnections.CarDataMySqlMasterConnection);
                 }
                messageText = "Dear Friends,"
                            + "<br/><br/>I have written a review on " + car + " and look forward to your comments on it."
                            + "<br/><br/>Review Link: <a href=\"https://www.carwale.com/research/userreviews/reviewdetails.aspx?rid=" + reviewId + "\"><strong>" + title + "</strong></a>&nbsp;"
                            + "<br/><br/>Thanks.";

                titleText = title + " - " + car + " User Review";
                string remoteAddr = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"] == null ? null : HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                string clientIp = HttpContext.Current.Request.ServerVariables["HTTP_CLIENT_IP"] == null ? null : HttpContext.Current.Request.ServerVariables["HTTP_CLIENT_IP"];
                //to create a new thread of the approved review
                threadId = GetThreadIdForReview(reviewId);
                if (threadId == "-1")
                {
                    ThreadsDAL threadDetails = new ThreadsDAL();
                    threadId = threadDetails.CreateNewThread(customerId, messageText, 1, "25", titleText, 1, remoteAddr, clientIp);
                    using(DbCommand cmd = DbFactory.GetDBCommand("InsertArticleAssociation_v16_11_7"))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DbFactory.GetDbParam("v_ThreadId", DbType.Int64, threadId));
                        cmd.Parameters.Add(DbFactory.GetDbParam("v_ReviewId", DbType.Int64, reviewId));
                        cmd.Parameters.Add(DbFactory.GetDbParam("v_ArticleType", DbType.Int16, 3));
                        MySqlDatabase.InsertQuery(cmd, DbConnections.ForumsMySqlMasterConnection);
                    }            
                }
                ret = true;
                _userReveiwRepo.SendUserReviewEmail(Convert.ToInt32(reviewId), UserReviewStatus.Approved);
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "AjaxUserReviews.ApproveReview");
                objErr.SendMail();
            }
            return ret;
        }

        //this function is used to approve the review which is being updated by the users
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.None)]
        public bool ApproveUpdatedReview(string reviewId, string customerId)
        {
            bool ret = false;
            try
            {
                UpdateReplicaAsVerified(reviewId);
                UpdateChangesToCustomerReviews(reviewId);       
                AddReplyInForums(reviewId, customerId);
                ret = true;
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "AjaxUserReviews.ApproveUpdatedReview");
                objErr.SendMail();
            }
            return ret;
        }


        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.None)]
        public bool ApproveAbusedReview(string reviewId, string customerId, string title, string car)
        {
            bool ret = false;
            try
            {
                UpdateAbuseAsVerified(reviewId);
                ret = true;
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "AjaxUserReviews.ApproveUpdatedReview");
                objErr.SendMail();
            }
            return ret;
        }

        //this function deletes the review
        //according to the review id passed and the retsurn value is true or false
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.None)]
        public bool DeleteReview(string reviewId)
        {       
            bool ret = false;      
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("DeleteCustomerReview_v16_11_7"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_ReviewId", DbType.Int64, reviewId));
                    MySqlDatabase.UpdateQuery(cmd, DbConnections.CarDataMySqlMasterConnection);
                    ret = true;
                    _userReveiwRepo.SendUserReviewEmail(Convert.ToInt32(reviewId), UserReviewStatus.Rejected);
                }
               
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "AjaxUserReviews.ApproveReview");
                objErr.SendMail();
            }         
            return ret;
        }


        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.None)]
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
                ErrorClass objErr = new ErrorClass(ex, "AjaxUserReviews.ApproveUpdatedReview");
                objErr.SendMail();
            }
            return ret;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.None)]
        public bool DeleteAbusedReview(string reviewId)
        {         
            bool ret = false;
            try
            {
                UpdateAbuseAsVerified(reviewId);        
                using (DbCommand cmd = DbFactory.GetDBCommand("DeleteAbusedReview_v16_11_7"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_ReviewId", DbType.Int64, reviewId));
                    MySqlDatabase.UpdateQuery(cmd, DbConnections.CarDataMySqlMasterConnection);
                }
                ret = true;
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "AjaxUserReviews.ApproveUpdatedReview");
                objErr.SendMail();
            }
            return ret;
        }

        private void UpdateReplicaAsVerified(string reviewId)
        {   
            try
            {            
                using(DbCommand cmd= DbFactory.GetDBCommand("VerifyReviewReplica_v16_11_7"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_ReviewId", DbType.Int64, reviewId));
                    MySqlDatabase.UpdateQuery(cmd, DbConnections.CarDataMySqlMasterConnection);
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "AjaxUserReviews.UpdateReplicaAsVerified");
                objErr.SendMail();
            }     
        }

        private void UpdateAbuseAsVerified(string reviewId)
        {     
            try
            {        
                using(DbCommand cmd = DbFactory.GetDBCommand("VerifyAbusedReviewById_v16_11_7"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_ReviewId", DbType.Int64, reviewId));
                    MySqlDatabase.UpdateQuery(cmd, DbConnections.CarDataMySqlMasterConnection);
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "AjaxUserReviews.UpdateAbuseAsVerified");
                objErr.SendMail();
            }       
        }

        private void UpdateChangesToCustomerReviews(string reviewId)
        {        
            try
            {
               using(DbCommand cmd = DbFactory.GetDBCommand("UpdateChangesToCustomerReview_v16_11_7"))
               {
                   cmd.CommandType = CommandType.StoredProcedure;
                   cmd.Parameters.Add(DbFactory.GetDbParam("v_ReviewId", DbType.Int64,reviewId));
                   MySqlDatabase.UpdateQuery(cmd, DbConnections.CarDataMySqlMasterConnection);
               }          
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "AjaxUserReviews.UpdateChangesToCustomerReviews");
                objErr.SendMail();
            }     
        }

        private void AddReplyInForums(string reviewIds, string customerIds)
        {
            string threadId = "";
            threadId = GetThreadIdForReview(reviewIds);

            if (threadId != "-1")
            {
                string remoteAddr = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"] == null ? null : HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                string clientIp = HttpContext.Current.Request.ServerVariables["HTTP_CLIENT_IP"] == null ? null : HttpContext.Current.Request.ServerVariables["HTTP_CLIENT_IP"];
                PostDAL postDetails = new PostDAL();
                postDetails.SavePost(customerIds, "This review is updated", 1, threadId, 1, remoteAddr, clientIp);
            }
        }

        private string GetThreadIdForReview(string reviewId)
        {
            string returnVal = "-1";     
            try
            {         
                using (DbCommand cmd = DbFactory.GetDBCommand("GetThreadIdForReview_v16_11_7"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_ReviewId", DbType.Int64, reviewId));
                    using(IDataReader dr = MySqlDatabase.SelectQuery(cmd,DbConnections.ForumsMySqlReadConnection))
                    {
                        if (dr.Read())
                        {
                            returnVal = dr[0].ToString();
                        }
                    }
                }
                
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, "AjaxUserReviews.GetThreadIdForReview");
                objErr.SendMail();
                returnVal = "-1";
            }       
            return returnVal;
        }



        //this function updates the liked and the disliked field of the customer reviews table
        //according to the review id passed and the helpful value which is either true or false
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.None)]
        public bool UpdateReviewHelpful(string reviewId, string helpful)
        {
            bool returnVal = false;

            //check whether this review has already been viewed
            string viewedList = CookiesUserReviews.URHelpful;
            bool viewed = false;


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
                try
                {                 
                    using (DbCommand cmd = DbFactory.GetDBCommand("UpdateCustomerReviewsHelpful_v16_11_7"))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DbFactory.GetDbParam("v_ReviewId", DbType.Int64, reviewId));
                        cmd.Parameters.Add(DbFactory.GetDbParam("v_Helpful", DbType.Boolean, isHelpFull));
                        MySqlDatabase.ExecuteNonQuery(cmd, DbConnections.CarDataMySqlMasterConnection);
                    }
                    returnVal = true;

                }
                catch (Exception err)
                {
                    HttpContext.Current.Trace.Warn("Ajaxfunctions : UpdateReviewHelpful : " + err.Message);
                    ErrorClass objErr = new ErrorClass(err, "Ajaxfunctions.UpdateReviewHelpful");
                    objErr.SendMail();
                    returnVal = false;
                } // catch Exception
                finally
                {
                    CookiesUserReviews.URHelpful += reviewId + ",";
                }
            }
            returnVal = !viewed;
            return returnVal;
        }

        //this function updates the liked and the disliked field of the customer reviews table
        //according to the review id passed and the helpful value which is either true or false
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.None)]
        public bool UpdateReviewAbuse(string reviewId, string comments)
        {
            bool returnVal = false;       
            CommonOpn op = new CommonOpn();
            try
            {             
                using (DbCommand cmd = DbFactory.GetDBCommand("UpdateCustomerReviewsAbuse_v16_11_7"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_ReviewId",DbType.Int64,reviewId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_Comments",DbType.String,500,comments));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_ReportedBy",DbType.Int64,CurrentUser.Id));
                    MySqlDatabase.ExecuteNonQuery(cmd, DbConnections.CarDataMySqlMasterConnection);
                    returnVal = true;
                }
             
            }
            catch (Exception err)
            {
                HttpContext.Current.Trace.Warn("Ajaxfunctions : UpdateReviewAbuse : " + err.Message);
                ErrorClass objErr = new ErrorClass(err, "Ajaxfunctions.UpdateReviewAbuse");
                objErr.SendMail();
                returnVal = false;
            } // catch Exception
            return returnVal;
        }    
    }
}