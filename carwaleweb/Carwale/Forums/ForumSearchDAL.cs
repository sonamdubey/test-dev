using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Carwale.Notifications;
using Carwale.DAL.CoreDAL;
using System.Data.Common;
using Carwale.DAL.CoreDAL.MySql;
using MySql.Data.MySqlClient;

/// <summary>
/// Summary description for SearchDAL
/// </summary>
/// 
namespace Carwale.Lucene.Forums
{
    public class ForumSearchDAL
    {
        /// <summary>
        /// Search forums method calls a particular method based on the search type.
        /// </summary>
        /// <param name="searchTerm"></param>
        /// <param name="searchType"></param>
        /// <param name="pageNo"></param>
        /// <param name="resultsPerPage"></param>
        /// <param name="totalResults"></param>
        /// <returns></returns>
        public DataTable SearchForums(string searchTerm, string searchType, int pageNo, int resultsPerPage,string sortBy,out int totalResults)
        {
            DataTable dtResults = null;
            DataSet dsResults = null;
            int total = -1;
            if (searchType == "Keyword")
            {
                dtResults = SearchKeyWord(searchTerm,sortBy, pageNo, resultsPerPage, out total);
            }
            else if (searchType == "PostsBy")
            {
                dsResults = PostsByStoredProcSearch(searchTerm, pageNo, resultsPerPage);
                dtResults = dsResults.Tables[1];
                total = Convert.ToInt32(dsResults.Tables[0].Rows[0].ItemArray[0]);
            }
            else if (searchType == "ByDate")
            {
                dsResults = ByDateStoredProcSearch(searchTerm, pageNo, resultsPerPage);
                dtResults = dsResults.Tables[1];
                total = Convert.ToInt32(dsResults.Tables[0].Rows[0].ItemArray[0]);
            }
            else if (searchType == "ThreadsBy")
            {
                dsResults = ThreadsByStoredProcSearch(searchTerm, pageNo, resultsPerPage);
                dtResults = dsResults.Tables[1];
                total = Convert.ToInt32(dsResults.Tables[0].Rows[0].ItemArray[0]);
            }
            totalResults = total;
            return dtResults;
        }

        #region Lucene key word search
        /// <summary>
        /// Lucene Search 
        /// </summary>
        /// <param name="searchTerm"></param>
        /// <param name="pageNo"></param>
        /// <param name="resultsPerPage"></param>
        /// <param name="totalResults"></param>
        /// <returns></returns>
        /// 
        public DataTable SearchKeyWord(string searchTerm,string sortBy, int pageNo, int resultsPerPage, out int totalResults)
        {
            totalResults = 0;
            DataSet dsDetails = null;
            dsDetails = GetKeyWordSearchFromDB(searchTerm, pageNo, resultsPerPage);
            return dsDetails.Tables[0];
        }
        #endregion

        #region Posts By User
        /// <summary>
        /// Search for Posts By User.
        /// </summary>
        /// <param name="searchTerm"></param>
        /// <param name="pageNo"></param>
        /// <param name="resultsPerPage"></param>
        /// <returns></returns>
        public DataSet PostsByStoredProcSearch(string searchTerm, int pageNo, int resultsPerPage)
        {
            int startIndex, endIndex;
            DataSet ds = new DataSet();    
            GetStartEndIndex(pageNo, resultsPerPage, out startIndex, out endIndex);
            try
            {
                using(DbCommand cmd = DbFactory.GetDBCommand("SearchForumsPostsBy_v16_11_7"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_CustomerId", DbType.Int64, searchTerm));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_StartIndex", DbType.Int32, startIndex));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_EndIndex", DbType.Int32, endIndex));
                    ds = MySqlDatabase.SelectAdapterQuery(cmd, DbConnections.ForumsMySqlReadConnection);
                }         
            }
            catch (MySqlException err)
            {
                ErrorClass objErr = new ErrorClass(err, "Forum DAL-Posts By Stored Proc Search- MYSQL Exception");
                objErr.SendMail();
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, "Forum DAL - Posts By Stored Proc Search - Exception.");
                objErr.SendMail();
            }
            return ds;
        }
        #endregion

        #region Search By Date
        /// <summary>
        /// Search By Date
        /// </summary>
        /// <param name="searchTerm"></param>
        /// <param name="pageNo"></param>
        /// <param name="resultsPerPage"></param>
        /// <returns></returns>
        public DataSet ByDateStoredProcSearch(string searchTerm, int pageNo, int resultsPerPage)
        {
            DataSet ds = new DataSet();    
            int startIndex, endIndex;
            DateTime dateValue = DateTime.Parse(searchTerm);
            searchTerm = dateValue.ToString("yyyy-MM-dd HH:mm:ss");
            GetStartEndIndex(pageNo, resultsPerPage, out startIndex, out endIndex);
            try
            {
                using(DbCommand cmd = DbFactory.GetDBCommand("SearchForumsByDate_v16_11_7"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_DateToCheck", DbType.DateTime, searchTerm));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_StartIndex", DbType.Int32, startIndex));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_EndIndex", DbType.Int32, endIndex));
                    ds = MySqlDatabase.SelectAdapterQuery(cmd, DbConnections.ForumsMySqlReadConnection);
                }         
            }
            catch (MySqlException err)
            {
                ErrorClass objErr = new ErrorClass(err, "Forum Search DAL-By Date Stored Proc Search - MYSQL Exception");
                objErr.SendMail();
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, "Forum DAL - By Date Stored Proc Search. - Exception");
                objErr.SendMail();
            }
            return ds;
        }
        #endregion

        #region Search Threads By User.
        /// <summary>
        /// Search Threads By User
        /// </summary>
        /// <param name="searchTerm"></param>
        /// <param name="pageNo"></param>
        /// <param name="resultsPerPage"></param>
        /// <returns></returns>
        public DataSet ThreadsByStoredProcSearch(string searchTerm, int pageNo, int resultsPerPage)
        {
            DataSet ds = new DataSet();   
            int startIndex, endIndex;
            GetStartEndIndex(pageNo, resultsPerPage, out startIndex, out endIndex);
            try
            {
                using(DbCommand cmd = DbFactory.GetDBCommand("SearchForumsThreadsBy_v16_11_7"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_CustomerId", DbType.Int64, searchTerm));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_StartIndex", DbType.Int32, startIndex));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_EndIndex", DbType.Int32, endIndex));
                    ds = MySqlDatabase.SelectAdapterQuery(cmd, DbConnections.ForumsMySqlReadConnection);
                }  
            }
            catch (MySqlException err)
            {
                ErrorClass objErr = new ErrorClass(err, "Forum DAL-Threads By Stored Proc Search - MYSQL Exception");
                objErr.SendMail();
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, "Forum DAL - Threads By Stored Proc Search - Exception");
                objErr.SendMail();
            }
            return ds;
        }
        #endregion

        #region Search For KeyWord In DataBase

        private DataSet GetKeyWordSearchFromDB(string searchTerm, int pageNo, int resultsPerPage)
        {
            int startIndex = -1 , endIndex = -1;
            DataSet ds = null;
            GetStartEndIndex(pageNo, resultsPerPage, out startIndex, out endIndex);
            try
            {         
                using(DbCommand cmd = DbFactory.GetDBCommand("SearchKeyWordFromDB_v16_11_7"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_SearchTerm", DbType.String, 500, searchTerm));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_StartIndex", DbType.Int32, startIndex));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_EndIndex", DbType.Int32, endIndex));
                    ds = MySqlDatabase.SelectAdapterQuery(cmd, DbConnections.ForumsMySqlReadConnection);
                }
            }
            catch (MySqlException err)
            {
                ErrorClass objErr = new ErrorClass(err, "Forum DAL-Threads By Stored Proc Search - SQL Exception");
                objErr.SendMail();
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, "Forum DAL - Threads By Stored Proc Search - Exception");
                objErr.SendMail();
            }
            return ds;
        }
        #endregion

        #region Get Forum Search Details

        public DataSet GetForumSearchDetails(DataTable dtSearchResults)
        {
            DataSet ds = new DataSet();    
            string stSearch = Utility.Format.ConvertDataTableToString(dtSearchResults);
            try
            {
                using(DbCommand cmd = DbFactory.GetDBCommand("GetForumThreadReads_v16_11_7"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_threadidlist", DbType.String, stSearch));
                    ds = MySqlDatabase.SelectAdapterQuery(cmd,DbConnections.ForumsMySqlReadConnection);
                }          
            }
            catch (MySqlException err)
            {
                ErrorClass objErr = new ErrorClass(err, "Forum DAL-get all forums method - MYSQL Exception");
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

        private void GetStartEndIndex(int pageNo, int resultsPerPage,out int startIndex,out int endIndex)
        {
            startIndex = 1;
            endIndex = resultsPerPage;
            if (pageNo > 1)
            {
                startIndex = ((pageNo - 1) * resultsPerPage) + 1;
                endIndex = pageNo * resultsPerPage;
            }

        }
    
    
    }
}