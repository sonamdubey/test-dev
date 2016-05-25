using System;
using System.Data;
using System.Web;
using System.Data.Common;
using BikeWaleOpr.Common;

namespace BikeWaleOPR.DAL.CoreDAL
{
	public static class MySqlDatabase
    {
        #region selectQuery
        public static IDataReader SelectQuery(string strSql)
		{
            return SelectQuery(strSql, null);
		}

        public static IDataReader SelectQuery(string strSql, DbParameter[] commandParameters)
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

                return SelectQuery(cmd);

            }
        }

        public static IDataReader SelectQuery(DbCommand cmd)
        {
            IDataReader dataReader;
            DbConnection con = DbFactory.GetDBConnection();
            try
            {
                cmd.Connection = con;
                con.Open();
                dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                return dataReader;
            }
            catch (Exception ex)
            {
                con.Close();
                SendErrorMessageOnException(ex);
                throw ex;
            }

        }
				
        #endregion

        #region select adapter query

        public static DataSet SelectAdapterQuery(string strSql)
        {
            return SelectAdapterQuery(strSql, null);
        }

        public static DataSet SelectAdapterQuery(string strSql, DbParameter[] commandParameters)
        {

            using(DbCommand cmd = DbFactory.GetDBCommand(strSql))
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

                return SelectAdapterQuery(cmd);
            }            
        }
		
		public static DataSet SelectAdapterQuery(DbCommand cmd) 
		{
            DataSet dataSet = new DataSet();
            using (DbConnection con = DbFactory.GetDBConnection())
            {             
                DbDataAdapter adapter = DbFactory.GetDBDataAdaptor();

                try
                {
                    cmd.Connection = con;
                    adapter.SelectCommand = cmd;
                    adapter.Fill(dataSet);
                    con.Close();
                }
                catch (Exception ex)
                {
                    SendErrorMessageOnException(ex);
                    con.Close();
                    throw ex;
                }
            }
			return dataSet;
		}		
		
        #endregion

        #region InsertQuery
        public static bool InsertQuery(string strSql)
		{
            return InsertQuery(strSql, null);
		}

        public static bool InsertQuery(string strSql, DbParameter[] commandParameters)
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
                return InsertQuery(cmd);
            }
        }

        public static bool InsertQuery(DbCommand cmdParam) 
		{

            using (DbConnection con = DbFactory.GetDBConnection())
            {
                try
                {
                    cmdParam.Connection = con;
                    con.Open();
                    int retval = cmdParam.ExecuteNonQuery();
                    con.Close();
                    return (retval > 0);
                }
                catch (Exception ex)
                {
                    SendErrorMessageOnException(ex);
                    con.Close();
                    throw ex;
                }
            }
		   	
		}

        #endregion

        #region UpdateQuery

        public static bool UpdateQuery(string strSql)
		{
            return UpdateQuery(strSql, null);
		}

        public static bool UpdateQuery(string strSql, DbParameter[] commandParameters) 
		{
            using (DbCommand cmd = DbFactory.GetDBCommand(strSql))
            {
                if (commandParameters !=null)
                {
                    foreach (DbParameter p in commandParameters)
                    {
                        if ((p.Direction == ParameterDirection.InputOutput) && (p.Value == null))
                            p.Value = DBNull.Value;

                        cmd.Parameters.Add(p);
                    } 
                }

                return UpdateQuery(cmd);
            }			
		}

        public static bool UpdateQuery(DbCommand cmdParam) 
		{
            using (DbConnection conn = DbFactory.GetDBConnection())
            {
                try
                {
                    cmdParam.Connection = conn;
                    conn.Open();

                    int retval = cmdParam.ExecuteNonQuery();
                    conn.Close();
                    return (retval > 0);

                }
                catch (Exception ex)
                {
                    SendErrorMessageOnException(ex);
                    conn.Close();
                    throw ex;
                }
            }
		}

        #endregion

        #region UpdateQueryReturnRowCount
        public static int UpdateQueryReturnRowCount(string strSql)
		{
            return UpdateQueryReturnRowCount(strSql, null);
		}

        public static int UpdateQueryReturnRowCount(string strSql, DbParameter[] commandParameters)
        {
            int retCount = 0;
            using (DbConnection con = DbFactory.GetDBConnection())
            {

                try
                {
                    using (DbCommand cmd = DbFactory.GetDBCommand(strSql))
                    {
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
                catch (Exception ex)
                {
                    SendErrorMessageOnException(ex);
                    con.Close();
                    throw ex;

                }
            }
            return retCount;
        }
        #endregion

        #region ExecuteScalar

        public static string ExecuteScalar(string strSql) 
		{
            return ExecuteScalar(strSql, null);
		}

        public static string ExecuteScalar(string strSql, DbParameter[] commandParameters)
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
                return ExecuteScalar(cmd);
            }
        }

        public static string ExecuteScalar(DbCommand cmd)
        {
            string retVal = string.Empty;
            using (DbConnection con = DbFactory.GetDBConnection())
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
                catch (Exception ex)
                {
                    SendErrorMessageOnException(ex);
                    con.Close();
                    throw ex;
                }
            }
            return retVal;
        }

        #endregion

        #region ExecuteNonQuery
        public static int ExecuteNonQuery(string strSql)
        {
           return ExecuteNonQuery(strSql, null);
        }

        public static int ExecuteNonQuery(string strSql, DbParameter[] commandParameters)
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

               return ExecuteNonQuery(cmd);

            }
        }

        public static int ExecuteNonQuery(DbCommand cmd)
        {
            using (DbConnection con = DbFactory.GetDBConnection())
            {
                try
                {
                    cmd.Connection = con;
                    con.Open();
                    int retVal = cmd.ExecuteNonQuery();
                    con.Close();
                    return retVal;

                }
                catch (Exception ex)
                {
                    con.Close();
                    SendErrorMessageOnException(ex);
                    throw ex;
                }
            }
        }

        #endregion

        private static void SendErrorMessageOnException(Exception err)
        {
            ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
            objErr.SendMail();
        }

    }//class
}//namespace
