using Carwale.Notifications.Logs;
using System;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Threading;

namespace Carwale.DAL.CoreDAL.MySql
{
    public static class MySqlDatabase
    {
        static readonly int _retryCount = Convert.ToInt32(ConfigurationManager.AppSettings["RetryCount"]);
        static readonly int _retryWaitLatency = Convert.ToInt32(ConfigurationManager.AppSettings["RetryWaitLatency"]);

        #region selectQuery
        public static IDataReader SelectQuery(string strSql, string connString)
        {
            return SelectQuery(strSql, null, connString);
        }

        public static IDataReader SelectQuery(string strSql, DbParameter[] commandParameters, string connString)
        {
            using (DbCommand cmd = DbFactory.GetDBCommand(strSql))
            {
                if (commandParameters != null)
                {
                    foreach (DbParameter p in commandParameters)
                    {
                        if ((p.Direction == ParameterDirection.InputOutput) && (p.Value == null))
                            p.Value = DBNull.Value;

                        cmd.Parameters.Add(p);
                    }
                }

                return SelectQuery(cmd, connString);

            }
        }

        public static IDataReader SelectQuery(DbCommand cmd, string connString)
        {
            return SelectQuery(cmd, connString, _retryCount);
        }

        public static IDataReader SelectQuery(DbCommand cmd, string connString, int retryCount)
        {
            IDataReader dataReader;
            LogLiveSps.LogMySqlSpInGrayLog(cmd);
            DbConnection con = DbFactory.GetDBConnection(connString);
            try
            {
                cmd.Connection = con;
                con.Open();
                dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                return dataReader;
            }
            catch (EndOfStreamException ex)
            {
                con.Close();
                
                retryCount--;
                if (retryCount > 0)
                {
                    Thread.Sleep(_retryWaitLatency);
                    return SelectQuery(cmd, connString, retryCount);
                }
                else
                {
                    LogErrorMessageOnException(ex.Message + " retryCount= " + retryCount);
                    throw ex;
                }
            }
            catch (Exception ex)
            {
                con.Close();
                LogErrorMessageOnException(ex.Message);
                throw ex;
            }
        }

        #endregion

        #region select adapter query

        public static DataSet SelectAdapterQuery(string strSql, string connString)
        {
            return SelectAdapterQuery(strSql, null, connString);
        }

        public static DataSet SelectAdapterQuery(string strSql, DbParameter[] commandParameters, string connString)
        {

            using (DbCommand cmd = DbFactory.GetDBCommand(strSql))
            {
                if (commandParameters != null)
                {
                    foreach (DbParameter p in commandParameters)
                    {
                        if ((p.Direction == ParameterDirection.InputOutput) && (p.Value == null))
                            p.Value = DBNull.Value;

                        cmd.Parameters.Add(p);
                    }
                }

                return SelectAdapterQuery(cmd, connString);
            }
        }

        public static DataSet SelectAdapterQuery(DbCommand cmd, string connString)
        {
            return SelectAdapterQuery(cmd, connString, _retryCount);
        }

        public static DataSet SelectAdapterQuery(DbCommand cmd, string connString, int retryCount)
        {
            DataSet dataSet = new DataSet();
            LogLiveSps.LogMySqlSpInGrayLog(cmd);
            using (DbConnection con = DbFactory.GetDBConnection(connString))
            {
                DbDataAdapter adapter = DbFactory.GetDBDataAdaptor();

                try
                {
                    cmd.Connection = con;
                    adapter.SelectCommand = cmd;
                    adapter.Fill(dataSet);
                    con.Close();
                }
                catch (EndOfStreamException ex)
                {
                    con.Close();

                    retryCount--;
                    if (retryCount > 0)
                    {
                        Thread.Sleep(_retryWaitLatency);
                        return SelectAdapterQuery(cmd, connString, retryCount);
                    }
                    else
                    {
                        LogErrorMessageOnException(ex.Message + " retryCount= " + retryCount);
                        throw ex;
                    }
                }
                catch (Exception ex)
                {
                    con.Close();
                    LogErrorMessageOnException(ex.Message);
                    throw ex;
                }
            }
            return dataSet;
        }
        #endregion

        #region InsertQuery
        public static bool InsertQuery(string strSql, string connString)
        {
            return InsertQuery(strSql, null, connString);
        }

        public static bool InsertQuery(string strSql, DbParameter[] commandParameters, string connString)
        {
            using (DbCommand cmd = DbFactory.GetDBCommand(strSql))
            {
                if (commandParameters != null)
                {
                    foreach (DbParameter p in commandParameters)
                    {
                        if ((p.Direction == ParameterDirection.InputOutput) && (p.Value == null))
                            p.Value = DBNull.Value;

                        cmd.Parameters.Add(p);
                    }
                }
                return InsertQuery(cmd, connString);
            }
        }

        public static bool InsertQuery(DbCommand cmdParam, string connString)
        {
            return InsertQuery(cmdParam, connString, _retryCount);

        }

        public static bool InsertQuery(DbCommand cmdParam, string connString, int retryCount)
        {
            LogLiveSps.LogMySqlSpInGrayLog(cmdParam);
            using (DbConnection con = DbFactory.GetDBConnection(connString))
            {
                try
                {
                    cmdParam.Connection = con;
                    con.Open();
                    int retval = cmdParam.ExecuteNonQuery();
                    con.Close();
                    return (retval > 0);
                }
                catch (EndOfStreamException ex)
                {
                    con.Close();

                    retryCount--;
                    if (retryCount > 0)
                    {
                        Thread.Sleep(_retryWaitLatency);
                        return InsertQuery(cmdParam, connString, retryCount);
                    }
                    else
                    {
                        LogErrorMessageOnException(ex.Message + " retryCount= " + retryCount);
                        throw ex;
                    }
                }
                catch (Exception ex)
                {
                    con.Close();
                    LogErrorMessageOnException(ex.Message);
                    throw ex;
                }
            }

        }


        public static int InsertQueryViaAdaptor(DbCommand cmdParam, DataTable dt,string connString)
        {
            return InsertQueryViaAdaptor(cmdParam, dt, connString, _retryCount);
        }

        public static int InsertQueryViaAdaptor(DbCommand cmdParam, DataTable dt, string connString, int retryCount)
        {
            if (cmdParam != null)
            {
                LogLiveSps.LogMySqlSpInGrayLog(cmdParam);
                using (DbConnection con = DbFactory.GetDBConnection(connString))
                {
                    try
                    {
                        cmdParam.Connection = con;
                        DbDataAdapter adpt = DbFactory.GetDBDataAdaptor();
                        adpt.InsertCommand = cmdParam;
                        adpt.UpdateBatchSize = dt.Rows.Count;

                        con.Open();

                        return adpt.Update(dt);

                    }
                    catch (EndOfStreamException ex)
                    {
                        con.Close();

                        retryCount--;
                        if (retryCount > 0)
                        {
                            Thread.Sleep(_retryWaitLatency);
                            return InsertQueryViaAdaptor(cmdParam, dt, connString, retryCount);
                        }
                        else
                        {
                            LogErrorMessageOnException(ex.Message + " retryCount= " + retryCount);
                            throw ex;
                        }
                    }
                    catch (Exception ex)
                    {
                        con.Close();
                        LogErrorMessageOnException(ex.Message);
                        throw ex;
                    }
                }
            }
            return 0;
        }

        #endregion

        #region UpdateQuery

        public static bool UpdateQuery(string strSql, string connString)
        {
            return UpdateQuery(strSql, null, connString);
        }

        public static bool UpdateQuery(string strSql, DbParameter[] commandParameters, string connString)
        {
            using (DbCommand cmd = DbFactory.GetDBCommand(strSql))
            {
                if (commandParameters!=null)
                {
                    foreach (DbParameter p in commandParameters)
                    {
                        if ((p.Direction == ParameterDirection.InputOutput) && (p.Value == null))
                            p.Value = DBNull.Value;

                        cmd.Parameters.Add(p);
                    } 
                }

                return UpdateQuery(cmd, connString);
            }
        }

        public static bool UpdateQuery(DbCommand cmdParam, string connString)
        {
            return UpdateQuery(cmdParam, connString, _retryCount);
        }

        public static bool UpdateQuery(DbCommand cmdParam, string connString, int retryCount)
        {
            LogLiveSps.LogMySqlSpInGrayLog(cmdParam);
            using (DbConnection conn = DbFactory.GetDBConnection(connString))
            {
                try
                {
                    cmdParam.Connection = conn;
                    conn.Open();

                    int retval = cmdParam.ExecuteNonQuery();
                    conn.Close();
                    return (retval > 0);

                }
                catch (EndOfStreamException ex)
                {
                    conn.Close();

                    retryCount--;
                    if (retryCount > 0)
                    {
                        Thread.Sleep(_retryWaitLatency);
                        return UpdateQuery(cmdParam, connString, retryCount);
                    }
                    else
                    {
                        LogErrorMessageOnException(ex.Message + " retryCount= " + retryCount);
                        throw ex;
                    }
                }
                catch (Exception ex)
                {
                    conn.Close();
                    LogErrorMessageOnException(ex.Message);
                    throw ex;
                }
            }
        }

        public static int UpdateQueryViaAdaptor(DbCommand cmdParam, DataTable dt, string connString)
        {
            return UpdateQueryViaAdaptor(cmdParam, dt, connString, _retryCount);

        }

        public static int UpdateQueryViaAdaptor(DbCommand cmdParam, DataTable dt, string connString, int retryCount)
        {
            LogLiveSps.LogMySqlSpInGrayLog(cmdParam);
            using (DbConnection con = DbFactory.GetDBConnection(connString))
            {
                try
                {
                    DbDataAdapter adpt = DbFactory.GetDBDataAdaptor();
                    adpt.UpdateCommand = cmdParam;
                    adpt.UpdateBatchSize = dt.Rows.Count;

                    con.Open();

                    return adpt.Update(dt);

                }
                catch (EndOfStreamException ex)
                {
                    con.Close();

                    retryCount--;
                    if (retryCount > 0)
                    {
                        Thread.Sleep(_retryWaitLatency);
                        return UpdateQueryViaAdaptor(cmdParam, dt, connString, retryCount);
                    }
                    else
                    {
                        LogErrorMessageOnException(ex.Message + " retryCount= " + retryCount);
                        throw ex;
                    }
                }
                catch (Exception ex)
                {
                    con.Close();
                    LogErrorMessageOnException(ex.Message);
                    throw ex;
                }
            }

        }

        #endregion

        #region UpdateQueryReturnRowCount
        public static int UpdateQueryReturnRowCount(string strSql, string connString)
        {
            return UpdateQueryReturnRowCount(strSql, null, connString);
        }

        public static int UpdateQueryReturnRowCount(string strSql, DbParameter[] commandParameters, string connString)
        {
            return UpdateQueryReturnRowCount(strSql, commandParameters, connString, _retryCount);     
        }

        public static int UpdateQueryReturnRowCount(string strSql, DbParameter[] commandParameters, string connString, int retryCount)
        {

            int retCount = 0;
            using (DbConnection con = DbFactory.GetDBConnection(connString))
            {
                try
                {
                    using (DbCommand cmd = DbFactory.GetDBCommand(strSql))
                    {
                        LogLiveSps.LogMySqlSpInGrayLog(cmd);
                        cmd.Connection = con;

                        if (commandParameters != null)
                        {
                            foreach (DbParameter p in commandParameters)
                            {
                                if ((p.Direction == ParameterDirection.InputOutput) && (p.Value == null))
                                    p.Value = DBNull.Value;

                                cmd.Parameters.Add(p);
                            }
                        }
                        con.Open();
                        retCount = cmd.ExecuteNonQuery();
                        con.Close();

                    }

                }
                catch (EndOfStreamException ex)
                {
                    con.Close();

                    retryCount--;
                    if (retryCount > 0)
                    {
                        Thread.Sleep(_retryWaitLatency);
                        return UpdateQueryReturnRowCount(strSql, commandParameters, connString, retryCount);
                    }
                    else
                    {
                        LogErrorMessageOnException(ex.Message + " retryCount= " + retryCount);
                        throw ex;
                    }
                }
                catch (Exception ex)
                {
                    con.Close();
                    LogErrorMessageOnException(ex.Message);
                    throw ex;
                }
            }
            return retCount;
        }

        #endregion

        #region ExecuteScalar

        public static string ExecuteScalar(string strSql, string connString)
        {
            return ExecuteScalar(strSql, null, connString);
        }

        public static string ExecuteScalar(string strSql, DbParameter[] commandParameters, string connString)
        {
            using (DbCommand cmd = DbFactory.GetDBCommand())
            {

                if (commandParameters != null)
                {
                    foreach (DbParameter p in commandParameters)
                    {
                        if ((p.Direction == ParameterDirection.InputOutput) && (p.Value == null))
                            p.Value = DBNull.Value;

                        cmd.Parameters.Add(p);
                    }
                }
                return ExecuteScalar(cmd, connString);
            }
        }

        public static string ExecuteScalar(DbCommand cmd, string connString)
        {
            return ExecuteScalar(cmd, connString, _retryCount);
        }

        public static string ExecuteScalar(DbCommand cmd, string connString,int retryCount)
        {
            string retVal = string.Empty;
            LogLiveSps.LogMySqlSpInGrayLog(cmd);
            using (DbConnection con = DbFactory.GetDBConnection(connString))
            {
                try
                {
                    cmd.Connection = con;
                    con.Open();
                    var data = cmd.ExecuteScalar();
                    con.Close();
                    if (data != null)
                        retVal = data.ToString();
                }
                catch (EndOfStreamException ex)
                {
                    con.Close();

                    retryCount--;
                    if (retryCount > 0)
                    {
                        Thread.Sleep(_retryWaitLatency);
                        return ExecuteScalar(cmd, connString, retryCount);
                    }
                    else
                    {
                        LogErrorMessageOnException(ex.Message + " retryCount= " + retryCount);
                        throw ex;
                    }
                }
                catch (Exception ex)
                {
                    con.Close();
                    LogErrorMessageOnException(ex.Message);
                    throw ex;
                }
            }
            return retVal;
        }

        #endregion

        #region ExecuteNonQuery
        public static int ExecuteNonQuery(string strSql, string connString)
        {
            return ExecuteNonQuery(strSql, null, connString);
        }

        public static int ExecuteNonQuery(string strSql, DbParameter[] commandParameters, string connString)
        {
            using (DbCommand cmd = DbFactory.GetDBCommand(strSql))
            {
                if (commandParameters != null)
                {
                    foreach (DbParameter p in commandParameters)
                    {
                        if ((p.Direction == ParameterDirection.InputOutput) && (p.Value == null))
                            p.Value = DBNull.Value;

                        cmd.Parameters.Add(p);
                    }
                }

                return ExecuteNonQuery(cmd, connString);

            }
        }

        public static int ExecuteNonQuery(DbCommand cmd, string connString)
        {
            return ExecuteNonQuery(cmd, connString, _retryCount);
        }

        public static int ExecuteNonQuery(DbCommand cmd, string connString, int retryCount)
        {
            LogLiveSps.LogMySqlSpInGrayLog(cmd);
            using (DbConnection con = DbFactory.GetDBConnection(connString))
            {
                try
                {
                    cmd.Connection = con;
                    con.Open();
                    int retVal = cmd.ExecuteNonQuery();
                    con.Close();
                    return retVal;

                }
                catch (EndOfStreamException ex)
                {
                    con.Close();

                    retryCount--;
                    if (retryCount > 0)
                    {
                        Thread.Sleep(_retryWaitLatency);
                        return ExecuteNonQuery(cmd, connString, retryCount);
                    }
                    else
                    {
                        LogErrorMessageOnException(ex.Message + " retryCount= " + retryCount);
                        throw ex;
                    }
                }
                catch (Exception ex)
                {
                    con.Close();
                    LogErrorMessageOnException(ex.Message);
                    throw ex;
                }
            }
        }

        #endregion


        private static void LogErrorMessageOnException(Exception err)
        {
            LogLiveSps.LogSpInGrayLog(err.Message);
        }

        private static void LogErrorMessageOnException(string error)
        {
            LogLiveSps.LogSpInGrayLog(error);
        }


        public static DbCommand GetCommandClone(DbCommand cmd)
        {
            if (cmd == null)
                return null;

            DbCommand cloneCmd = DbFactory.GetDBCommand(cmd.CommandText);
            cloneCmd.CommandType = cmd.CommandType;
            cloneCmd.CommandTimeout = cmd.CommandTimeout;
            //cloneCmd.Connection = cmd.Connection; we will not have connection 

            foreach (DbParameter param in cmd.Parameters)
            {
                cloneCmd.Parameters.Add(GetDbParameterClone(param));
            }

            return cloneCmd;
        }
        public static DbParameter GetDbParameterClone(DbParameter param)
        {
            if (param == null)
                return null;
            DbParameter cloneParam = DbFactory.GetDbParam(param.ParameterName, param.DbType, param.Value);
            cloneParam.Direction = param.Direction;
            cloneParam.Size = param.Size;
            cloneParam.SourceColumn = param.SourceColumn;
            cloneParam.SourceColumnNullMapping = param.SourceColumnNullMapping;
            return cloneParam;
        }


    }//class
}//namespace
