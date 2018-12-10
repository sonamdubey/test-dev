using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using Carwale.Notifications;
using System.Data.SqlClient;
using Carwale.Interfaces.Forums;
using Carwale.DAL.CoreDAL;
using Dapper;
using System.Linq;
using System.Data.Common;
using Carwale.DAL.CoreDAL.MySql;
using MySql.Data.MySqlClient;


/// <summary>
/// Implements IForum interface methods.
/// </summary>
/// 
namespace Carwale.DAL.Forums
{
    public class ForumsDAL : RepositoryBase,IForum
    {
        #region Get Forum Details
        public DataSet GetForumDetails(int forumId, int startIndex, int endIndex)
        {    
            DataSet ds = new DataSet();
            try
            {
                using(DbCommand cmd = DbFactory.GetDBCommand("GetForumSubCategoryDetails_v16_11_7"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_forumId", DbType.Int64, forumId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_startindex", DbType.Int32, startIndex));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_endindex", DbType.Int32, endIndex));
                    ds = MySqlDatabase.SelectAdapterQuery(cmd, DbConnections.ForumsMySqlReadConnection);
                }          
            }
            catch (MySqlException err)
            {
                ErrorClass objErr = new ErrorClass(err, "Forum DAL-get forum details method - MYSQL exception");
                objErr.SendMail();
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, "Forum DAL - get forum details method.");
                objErr.SendMail();
            }
            
            return ds;
        }
        #endregion

        #region Get All Forums Details
        /// <summary>
        /// gets detaRepositoryBase,ils of all the forums for the default page repeater.
        /// </summary>
        /// <returns></returns>
        public DataSet GetAllForums()
        {      
            DataSet ds = new DataSet();
            try
            {
                using(DbCommand cmd = DbFactory.GetDBCommand("GetAboutForumDetails_v16_11_7"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    ds = MySqlDatabase.SelectAdapterQuery(cmd, DbConnections.ForumsMySqlReadConnection);
                }          
            }
            catch (MySqlException err)
            {
                ErrorClass objErr = new ErrorClass(err, "Forum DAL-get all forums method - SQL Exception");
                objErr.SendMail();
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, "Forum DAL - get all forums method.");
                objErr.SendMail();
            }
            return ds;
        }
        #endregion
        #region Get all Active Member
        /// <summary>
        /// get all active member of forums
        /// </summary>
        public List<string> GetActiveMember()
        {
            try
            {
                using(var con = ForumsMySqlReadConnection)
                {
                    return con.Query<string>("GetActiveForumMembers_v16_11_7",commandType: CommandType.StoredProcedure).ToList();
                } 
            }
            catch (SqlException err)
            {
                ErrorClass objErr = new ErrorClass(err, "Forum DAL-get all Active member method - SQL Exception");
                objErr.SendMail();
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, "Forum DAL -get all Active member method.");
                objErr.SendMail();
            }
            return null;
        }
        #endregion
        #region Get Forums posts
        public List<string> GetForumsPostSummary()
        {
            try
            {
                using(var con = ForumsMySqlReadConnection)
                {
                    return con.Query<string>("GetForumsPosts_v16_11_7",commandType: CommandType.StoredProcedure).ToList();
                } 
            }
            catch (SqlException err)
            {
                ErrorClass objErr = new ErrorClass(err, "Forum DAL- Get Forums posts method - SQL Exception");
                objErr.SendMail();
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, "Forum DAL - Get Forums posts method.");
                objErr.SendMail();
            }
            return null;
        }     
        #endregion

       public DataSet GetForumReviewCount(int ReviewId,int StartIndex,int EndIndex)
        {
            DataSet ds = new DataSet();
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("WA_GetReviewComments_v16_11_7"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_ReviewId",DbType.Int32,Convert.ToInt32(ReviewId)));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_StartIndex",DbType.Int32,Convert.ToInt32(StartIndex)));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_EndIndex",DbType.Int32,Convert.ToInt32(EndIndex)));
                    ds = MySqlDatabase.SelectAdapterQuery(cmd,DbConnections.ForumsMySqlReadConnection);
                }
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            return ds;
        }

    }//class
}//namespace