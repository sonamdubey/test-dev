using log4net;
using MySql.CoreDAL.Logging;
using System;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Threading;

namespace MySql.CoreDAL
{
    public static class MySqlDatabase
    {
        static readonly ILog _logger = LogManager.GetLogger(typeof(MySqlDatabase));
        static readonly ILog _connectionOpenTimeLogger = LogManager.GetLogger("ConnectionOpenTimeLogger");
        static readonly int _retryCount = Convert.ToInt32(ConfigurationManager.AppSettings["RetryCount"]);
        static readonly int _retryWaitLatency = Convert.ToInt32(ConfigurationManager.AppSettings["RetryWaitLatency"]);
        static readonly bool _logOpenTiming;

        static MySqlDatabase()
        {
            if (!Boolean.TryParse(ConfigurationManager.AppSettings["LogConnectionOpenTime"], out _logOpenTiming))
                _logOpenTiming = false;


        }

        #region selectQuery
        public static IDataReader SelectQuery(string strSql, ConnectionType connType)
        {
            return SelectQuery(strSql, null, connType);
        }

        public static IDataReader SelectQuery(string strSql, DbParameter[] commandParameters, ConnectionType connType)
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

                return SelectQuery(cmd, connType);

            }
        }

        public static IDataReader SelectQuery(DbCommand cmd, ConnectionType connType)
        {
            return SelectQuery(cmd, connType, _retryCount);
        }

        public static IDataReader SelectQuery(DbCommand cmd, ConnectionType connType, int retryCount)
        {
            DateTime start, end;
            start = end = DateTime.Now;
            IDataReader dataReader;
            MysqlLogLiveSps.LogSpInGrayLog(cmd);
            DbConnection con = DbFactory.GetDBConnection(connType);
            try
            {
                cmd.Connection = con;
                start = DateTime.Now; 
                con.Open(); 
                end = DateTime.Now;
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
                    return SelectQuery(cmd, connType, retryCount);
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
            finally
            {
                if (_logOpenTiming)
                {
                    ThreadContext.Properties["OpenTime"] = (end - start).TotalMilliseconds;
                    _connectionOpenTimeLogger.Error("ConnectionOpenTime");
                    ThreadContext.Properties.Remove("OpenTime");
                }
            }
        }

        #endregion

        #region select adapter query

        public static DataSet SelectAdapterQuery(string strSql, ConnectionType connType)
        {
            return SelectAdapterQuery(strSql, null, connType);
        }

        public static DataSet SelectAdapterQuery(string strSql, DbParameter[] commandParameters, ConnectionType connType)
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

                return SelectAdapterQuery(cmd, connType);
            }
        }

        public static DataSet SelectAdapterQuery(DbCommand cmd, ConnectionType connType)
        {
            return SelectAdapterQuery(cmd, connType, _retryCount);
        }

        public static DataSet SelectAdapterQuery(DbCommand cmd, ConnectionType connType, int retryCount)
        {
            DataSet dataSet = new DataSet();
            MysqlLogLiveSps.LogSpInGrayLog(cmd);
            using (DbConnection con = DbFactory.GetDBConnection(connType))
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
                        return SelectAdapterQuery(cmd, connType, retryCount);
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
        public static bool InsertQuery(string strSql, ConnectionType connType)
        {
            return InsertQuery(strSql, null, connType);
        }

        public static bool InsertQuery(string strSql, DbParameter[] commandParameters, ConnectionType connType)
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
                return InsertQuery(cmd, connType);
            }
        }

        public static bool InsertQuery(DbCommand cmdParam, ConnectionType connType)
        {
            return InsertQuery(cmdParam, connType, _retryCount);

        }

        public static bool InsertQuery(DbCommand cmdParam, ConnectionType connType, int retryCount)
        {
            DateTime start, end;
            start = end = DateTime.Now;
            MysqlLogLiveSps.LogSpInGrayLog(cmdParam);
            using (DbConnection con = DbFactory.GetDBConnection(connType))
            {
                try
                {
                    cmdParam.Connection = con;
                    start = DateTime.Now; 
                    con.Open(); 
                    end = DateTime.Now;
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
                        return InsertQuery(cmdParam, connType, retryCount);
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
                finally
                {
                    if (_logOpenTiming)
                    {
                        ThreadContext.Properties["OpenTime"] = (end - start).TotalMilliseconds;
                        _connectionOpenTimeLogger.Error("ConnectionOpenTime");
                        ThreadContext.Properties.Remove("OpenTime");
                    }
                }
            }

        }


        public static int InsertQueryViaAdaptor(DbCommand cmdParam, DataTable dt, ConnectionType connType)
        {
            return InsertQueryViaAdaptor(cmdParam, dt, connType, _retryCount);
        }

        public static int InsertQueryViaAdaptor(DbCommand cmdParam, DataTable dt, ConnectionType connType, int retryCount)
        {
            DateTime start, end;
            start = end = DateTime.Now;
            if (cmdParam != null)
            {

                MysqlLogLiveSps.LogSpInGrayLog(cmdParam);
                using (DbConnection con = DbFactory.GetDBConnection(connType))
                {
                    try
                    {
                        cmdParam.Connection = con;
                        DbDataAdapter adpt = DbFactory.GetDBDataAdaptor();
                        adpt.InsertCommand = cmdParam;
                        adpt.UpdateBatchSize = dt.Rows.Count;

                        start = DateTime.Now; 
                        con.Open(); 
                        end = DateTime.Now;

                        return adpt.Update(dt);

                    }
                    catch (EndOfStreamException ex)
                    {
                        con.Close();

                        retryCount--;
                        if (retryCount > 0)
                        {
                            Thread.Sleep(_retryWaitLatency);
                            return InsertQueryViaAdaptor(cmdParam, dt, connType, retryCount);
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
                    finally
                    {
                        if (_logOpenTiming)
                        {
                            ThreadContext.Properties["OpenTime"] = (end - start).TotalMilliseconds;
                            _connectionOpenTimeLogger.Error("ConnectionOpenTime");
                            ThreadContext.Properties.Remove("OpenTime");
                        }
                    }
                }
            }
            return 0;
        }

        #endregion

        #region UpdateQuery

        public static bool UpdateQuery(string strSql, ConnectionType connType)
        {
            return UpdateQuery(strSql, null, connType);
        }

        public static bool UpdateQuery(string strSql, DbParameter[] commandParameters, ConnectionType connType)
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

                return UpdateQuery(cmd, connType);
            }
        }

        public static bool UpdateQuery(DbCommand cmdParam, ConnectionType connType)
        {
            return UpdateQuery(cmdParam, connType, _retryCount);
        }

        public static bool UpdateQuery(DbCommand cmdParam, ConnectionType connType, int retryCount)
        {
            MysqlLogLiveSps.LogSpInGrayLog(cmdParam);
            using (DbConnection conn = DbFactory.GetDBConnection(connType))
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
                        return UpdateQuery(cmdParam, connType, retryCount);
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

        public static int UpdateQueryViaAdaptor(DbCommand cmdParam, DataTable dt, ConnectionType connType)
        {
            return UpdateQueryViaAdaptor(cmdParam, dt, connType, _retryCount);

        }

        public static int UpdateQueryViaAdaptor(DbCommand cmdParam, DataTable dt, ConnectionType connType, int retryCount)
        {
            DateTime start, end;
            start = end = DateTime.Now;
            MysqlLogLiveSps.LogSpInGrayLog(cmdParam);
            using (DbConnection con = DbFactory.GetDBConnection(connType))
            {
                try
                {
                    DbDataAdapter adpt = DbFactory.GetDBDataAdaptor();
                    adpt.UpdateCommand = cmdParam;
                    adpt.UpdateBatchSize = dt.Rows.Count;

                    start = DateTime.Now; 
                    con.Open(); 
                    end = DateTime.Now;

                    return adpt.Update(dt);

                }
                catch (EndOfStreamException ex)
                {
                    con.Close();

                    retryCount--;
                    if (retryCount > 0)
                    {
                        Thread.Sleep(_retryWaitLatency);
                        return UpdateQueryViaAdaptor(cmdParam, dt, connType, retryCount);
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
                finally
                {
                    if (_logOpenTiming)
                    {
                        ThreadContext.Properties["OpenTime"] = (end - start).TotalMilliseconds;
                        _connectionOpenTimeLogger.Error("ConnectionOpenTime");
                        ThreadContext.Properties.Remove("OpenTime");
                    }
                }
            }

        }

        #endregion

        #region UpdateQueryReturnRowCount
        public static int UpdateQueryReturnRowCount(string strSql, ConnectionType connType)
        {
            return UpdateQueryReturnRowCount(strSql, null, connType);
        }

        public static int UpdateQueryReturnRowCount(string strSql, DbParameter[] commandParameters, ConnectionType connType)
        {
            return UpdateQueryReturnRowCount(strSql, commandParameters, connType, _retryCount);
        }

        public static int UpdateQueryReturnRowCount(string strSql, DbParameter[] commandParameters, ConnectionType connType, int retryCount)
        {
            DateTime start, end;
            start = end = DateTime.Now;
            int retCount = 0;
            using (DbConnection con = DbFactory.GetDBConnection(connType))
            {
                try
                {
                    using (DbCommand cmd = DbFactory.GetDBCommand(strSql))
                    {
                        MysqlLogLiveSps.LogSpInGrayLog(cmd);
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
                        start = DateTime.Now; 
                        con.Open(); 
                        end = DateTime.Now;
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
                        return UpdateQueryReturnRowCount(strSql, commandParameters, connType, retryCount);
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
                finally
                {
                    if (_logOpenTiming)
                    {
                        ThreadContext.Properties["OpenTime"] = (end - start).TotalMilliseconds;
                        _connectionOpenTimeLogger.Error("ConnectionOpenTime");
                        ThreadContext.Properties.Remove("OpenTime");
                    }
                }
            }
            return retCount;
        }

        #endregion

        #region ExecuteScalar

        public static string ExecuteScalar(string strSql, ConnectionType connType)
        {
            return ExecuteScalar(strSql, null, connType);
        }

        public static string ExecuteScalar(string strSql, DbParameter[] commandParameters, ConnectionType connType)
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
                return ExecuteScalar(cmd, connType);
            }
        }

        public static string ExecuteScalar(DbCommand cmd, ConnectionType connType)
        {
            return ExecuteScalar(cmd, connType, _retryCount);
        }

        public static string ExecuteScalar(DbCommand cmd, ConnectionType connType, int retryCount)
        {
            DateTime start, end;
            start = end = DateTime.Now;
            string retVal = string.Empty;
            MysqlLogLiveSps.LogSpInGrayLog(cmd);
            using (DbConnection con = DbFactory.GetDBConnection(connType))
            {
                try
                {
                    cmd.Connection = con;
                    start = DateTime.Now; 
                    con.Open(); 
                    end = DateTime.Now;
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
                        return ExecuteScalar(cmd, connType, retryCount);
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
                finally
                {
                    if (_logOpenTiming)
                    {
                        ThreadContext.Properties["OpenTime"] = (end - start).TotalMilliseconds;
                        _connectionOpenTimeLogger.Error("ConnectionOpenTime");
                        ThreadContext.Properties.Remove("OpenTime");
                    }
                }
            }
            return retVal;
        }

        #endregion

        #region ExecuteNonQuery
        public static int ExecuteNonQuery(string strSql, ConnectionType connType)
        {
            return ExecuteNonQuery(strSql, null, connType);
        }

        public static int ExecuteNonQuery(string strSql, DbParameter[] commandParameters, ConnectionType connType)
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

                return ExecuteNonQuery(cmd, connType);

            }
        }

        public static int ExecuteNonQuery(DbCommand cmd, ConnectionType connType)
        {
            return ExecuteNonQuery(cmd, connType, _retryCount);
        }

        public static int ExecuteNonQuery(DbCommand cmd, ConnectionType connType, int retryCount)
        {
            DateTime start, end;
            start = end = DateTime.Now;
            MysqlLogLiveSps.LogSpInGrayLog(cmd);
            using (DbConnection con = DbFactory.GetDBConnection(connType))
            {
                try
                {
                    cmd.Connection = con;
                    start = DateTime.Now; 
                    con.Open(); 
                    end = DateTime.Now;
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
                        return ExecuteNonQuery(cmd, connType, retryCount);
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
                finally
                {
                    if (_logOpenTiming)
                    {
                        ThreadContext.Properties["OpenTime"] = (end - start).TotalMilliseconds;
                        _connectionOpenTimeLogger.Error("ConnectionOpenTime");
                        ThreadContext.Properties.Remove("OpenTime");
                    }
                }
            }
        }

        #endregion


        private static void LogErrorMessageOnException(Exception err)
        {
            _logger.Error(err.Message);
        }

        private static void LogErrorMessageOnException(string error)
        {
            _logger.Error(error);
        }

    }//class
}//namespace
