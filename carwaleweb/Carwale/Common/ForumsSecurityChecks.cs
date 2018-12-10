using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using Carwale.UI.Common;
using Carwale.DAL.CoreDAL;
using Carwale.Notifications;
using Carwale.Notifications.Logs;
using System.Data.Common;
using Carwale.DAL.CoreDAL.MySql;
/// <summary>
/// Summary description for ForumsSecurityChecks
/// </summary>
/// 
namespace Carwale.UI.Common
{
    public class ForumsSecurityChecks
    {
        private int PostCount;
        private int LastPost;
        private int ActivePostCount;
        private int IsFake = 0;

        //By default assumed that the customer is a valid customer
        private int IsValidCustomerCheck = 1;


        /* CheckForumPostEligibility Return Values : 
         * 0 -> The customer is not a valid customer.
         * 1 -> The customer is valid, but the 5 minute rule applies to him as his number of posts are less than 50.
         * 2 -> The customer is valid, and  the 5 minute rule is not applicable to him/her.
         */
        public int CheckForumPostEligibility(string customerId)
        {
            int ForumCustomerCheck = -1;
            FetchCustomerData(customerId);
            if (PostCount < 50)
            {
                if (IsValidCustomerCheck == 0 && IsFake == 0)
                {
                    ForumCustomerCheck = 0;
                }
                else if (LastPost < 5 && IsFake == 0)
                {
                    ForumCustomerCheck = 1;
                }
            }
            else if (IsFake == 1)
                ForumCustomerCheck = -2;

            else
                ForumCustomerCheck = 2;

            return ForumCustomerCheck;
        }

        public bool Captchacheck(string customerId)
        {
            FetchCustomerData(customerId);

            HttpContext.Current.Trace.Warn("Number of Posts = " + PostCount);
            if (PostCount < 50)
                return false;
            else
                return true;
        }

        public bool CheckForModeration(string customerId)
        {
            bool ToBeMod = false; // Indicates that the user is legit and no need for moderation of his/her posts.
            GetActivePostsCount(customerId);
            int PostsCountCheck = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["NumberOfPostsForModeration"]);
            if (ActivePostCount <= PostsCountCheck)
            {
                ToBeMod = true; // Indicates that Posts need to be moderated.
            }

            return ToBeMod;
        }



        /// <summary>
        /// This method checks if posts by this user are to be sent for moderation or not.
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        private void GetActivePostsCount(string customerId)
        {       
            try
            {
                using(DbCommand cmd = DbFactory.GetDBCommand("GetActivePostsCount_v16_11_7"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_CustomerId",DbType.Int64,customerId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_ActivePostsCount",DbType.Int32,ParameterDirection.Output));
                    LogLiveSps.LogSpInGrayLog(cmd);
                    MySqlDatabase.ExecuteNonQuery(cmd,DbConnections.ForumsMySqlReadConnection);
                    ActivePostCount = Convert.ToInt32(cmd.Parameters["v_ActivePostsCount"].Value.ToString());
                }
            }
            catch (Exception err)
            {
                HttpContext.Current.Trace.Warn("ForumSecurityCheck : " + err.Message);
            }
        }

        void FetchCustomerData(string customerId)
        {
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("ForumPostsCount_v16_11_7"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_CustomerId", DbType.Int64, customerId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_PostCount", DbType.Int32, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_LastPostTime", DbType.Int64, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_VerifyCustomer", DbType.Int32, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_IsFake", DbType.Int32, ParameterDirection.Output));
                    LogLiveSps.LogSpInGrayLog(cmd);
                    MySqlDatabase.ExecuteNonQuery(cmd, DbConnections.ForumsMySqlReadConnection);
                    PostCount = Convert.ToInt32(cmd.Parameters["v_PostCount"].Value.ToString());
                    if (PostCount == 0)
                        LastPost = 5;
                    else
                    LastPost = Convert.ToInt32(cmd.Parameters["v_LastPostTime"].Value.ToString());
                    IsValidCustomerCheck = Convert.ToInt32(cmd.Parameters["v_VerifyCustomer"].Value.ToString());
                    IsFake = Convert.ToInt32(cmd.Parameters["v_IsFake"].Value.ToString());
                }
            }
            catch (Exception err)
            {
                HttpContext.Current.Trace.Warn("ForumSecurityCheck : " + err.Message);
            } // catch Exception
        }
    }
}