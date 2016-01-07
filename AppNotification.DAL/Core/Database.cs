using AppNotification.Notifications;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Configuration;

namespace AppNotification.DAL.Core
{
    public class Database
    {
        private SqlConnection con;
        private SqlCommand cmd;
        private String strConn;

        private HttpContext objTrace = HttpContext.Current;

        public SqlConnection Conn
        {
            get { return con; }
            set { con = value; }
        }

        public Database(string connectionString)
        {
            strConn = connectionString;
        }

        /// <summary>
        /// Overload for not providing connetion String.
        /// By default it will take connection string from web config file.
        /// </summary>
        public Database()
        {
            this.strConn = ConfigurationManager.AppSettings["connectionString"];
        }

        //return the connection string
        public string GetConString()
        {
            return strConn;
        }

        public SqlDataReader SelectQry(string strSql)
        {
            SqlDataReader dataReader;
            con = new SqlConnection(strConn);

            try
            {
                con.Open();
                cmd = new SqlCommand(strSql, con);
                dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                return dataReader;
            }
            catch (Exception)
            {
                CloseConnection();
                throw;
            }
        }

        public SqlDataReader SelectQry(string sqlStr, SqlParameter[] commandParameters)
        {
            SqlDataReader dataReader = null;
            con = new SqlConnection(strConn);

            try
            {
                con.Open();
                cmd = new SqlCommand(sqlStr, con);
                foreach (SqlParameter p in commandParameters)
                {
                    if ((p.Direction == ParameterDirection.InputOutput) && (p.Value == null))
                        p.Value = DBNull.Value;

                    cmd.Parameters.Add(p);
                }
                dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                cmd.Parameters.Clear();
            }
            catch (Exception err)
            {
                var objErr = new ExceptionHandler(err, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.LogException();
            }
            finally
            {
                cmd.Parameters.Clear();
                //CloseConnection();
            }
            return dataReader;
        }

        public SqlDataReader SelectQry(SqlCommand cmdParam)
        {
            con = new SqlConnection(strConn);
            SqlDataReader dataReader = null;

            try
            {
                cmdParam.Connection = con;
                con.Open();

                dataReader = cmdParam.ExecuteReader(CommandBehavior.CloseConnection);
                cmdParam.Parameters.Clear();
            }
            catch (Exception err)
            {
                cmdParam.Parameters.Clear();
                CloseConnection();

                var objErr = new ExceptionHandler(err, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.LogException();
            }
            return dataReader;
        }

        public DataSet SelectAdaptQry(string strSql)
        {
            DataSet dataSet = new DataSet();
            con = new SqlConnection(strConn);

            try
            {
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = new SqlCommand(strSql, con);
                adapter.Fill(dataSet);
            }
            catch (SqlException exSql)
            {
                dataSet = null;

                var objErr = new ExceptionHandler(exSql, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.LogException();
            }
            catch (Exception err)
            {
                var objErr = new ExceptionHandler(err, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.LogException();
            }
            finally
            {
                CloseConnection();
            }

            return dataSet;
        }

        public DataSet SelectAdaptQry(string sqlStr, SqlParameter[] commandParameters)
        {
            DataSet dataSet = new DataSet();

            con = new SqlConnection(strConn);
            SqlDataAdapter adapter = new SqlDataAdapter();
            try
            {
                adapter.SelectCommand = new SqlCommand(sqlStr, con);

                foreach (SqlParameter p in commandParameters)
                {
                    if ((p.Direction == ParameterDirection.InputOutput) && (p.Value == null))
                        p.Value = DBNull.Value;

                    adapter.SelectCommand.Parameters.Add(p);
                }
                adapter.Fill(dataSet);
                adapter.SelectCommand.Parameters.Clear();
            }
            catch (SqlException exSql)
            {
                dataSet = null;

                var objErr = new ExceptionHandler(exSql, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.LogException();
            }
            catch (Exception err)
            {
                var objErr = new ExceptionHandler(err, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.LogException();
            }
            finally
            {
                adapter.SelectCommand.Parameters.Clear();
                CloseConnection();
            }
            return dataSet;
        }

        public DataSet SelectAdaptQry(SqlCommand cmdParam)
        {
            con = new SqlConnection(strConn);
            DataSet dataSet = new DataSet();
            SqlDataAdapter adapter = new SqlDataAdapter();

            try
            {
                cmdParam.Connection = con;
                //con.Open();
                adapter.SelectCommand = cmdParam;
                adapter.Fill(dataSet);
            }
            catch (SqlException exSql)
            {
                dataSet = null;

                var objErr = new ExceptionHandler(exSql, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.LogException();
            }
            catch (Exception err)
            {
                var objErr = new ExceptionHandler(err, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.LogException();
            }
            finally
            {
                CloseConnection();
            }
            return dataSet;
        }

        public void CloseConnection()
        {
            try
            {
                if (con != null && con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
            catch (Exception err)
            {
                var objErr = new ExceptionHandler(err, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.LogException();
            }
        }

        public bool InsertQry(string strSql)
        {
            int intRetRows;
            con = new SqlConnection(strConn);
            bool result = false;
            try
            {
                cmd = new SqlCommand(strSql, con);
                con.Open();

                intRetRows = cmd.ExecuteNonQuery();
                if (intRetRows > 0)
                    result = true;
                else
                    result = false;
            }
            catch (Exception ex)
            {
                var objErr = new ExceptionHandler(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.LogException();
            }
            finally
            {
                CloseConnection();
            }
            return result;
        }

        public bool InsertQry(string sqlStr, SqlParameter[] commandParameters)
        {
            con = new SqlConnection(strConn);
            bool result = false;

            try
            {
                cmd = new SqlCommand(sqlStr, con);
                con.Open();

                foreach (SqlParameter p in commandParameters)
                {
                    if ((p.Direction == ParameterDirection.InputOutput) && (p.Value == null))
                        p.Value = DBNull.Value;

                    cmd.Parameters.Add(p);
                }

                int retval = cmd.ExecuteNonQuery();

                if (retval > 0)
                    result = true;
                else
                    result = false;
            }
            catch (Exception err)
            {
                var objErr = new ExceptionHandler(err, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.LogException();
            }
            finally
            {
                cmd.Parameters.Clear();
                CloseConnection();
            }
            return result;
        }

        public bool InsertQry(SqlCommand cmdParam)
        {
            con = new SqlConnection(strConn);
            bool result = false;

            try
            {
                cmdParam.Connection = con;
                con.Open();

                int retval = cmdParam.ExecuteNonQuery();

                if (retval > 0)
                    result = true;
                else
                    result = false;
            }
            catch (Exception err)
            {
                var objErr = new ExceptionHandler(err, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.LogException();
            }
            finally
            {
                cmdParam.Parameters.Clear();
                CloseConnection();
            }
            return result;
        }

        public bool UpdateQry(string strSql)
        {
            int intRetRows;
            con = new SqlConnection(strConn);
            bool result;
            try
            {
                cmd = new SqlCommand(strSql, con);
                con.Open();
                intRetRows = cmd.ExecuteNonQuery();

                if (intRetRows > 0)
                    result = true;
                else
                    result = false;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                CloseConnection();
            }
            return result;
        }

        public bool UpdateQry(string sqlStr, SqlParameter[] commandParameters)
        {
            con = new SqlConnection(strConn);
            bool result = false;

            try
            {
                cmd = new SqlCommand(sqlStr, con);
                con.Open();

                foreach (SqlParameter p in commandParameters)
                {
                    if ((p.Direction == ParameterDirection.InputOutput) && (p.Value == null))
                        p.Value = DBNull.Value;

                    cmd.Parameters.Add(p);
                }

                int retval = cmd.ExecuteNonQuery();

                if (retval > 0)
                    result = true;
                else
                    result = false;


            }
            catch (Exception err)
            {
                var objErr = new ExceptionHandler(err, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.LogException();
            }
            finally
            {
                cmd.Parameters.Clear();
                CloseConnection();
            }
            return result;
        }

        public bool UpdateQry(SqlCommand cmdParam)
        {
            con = new SqlConnection(strConn);
            bool result = false;

            try
            {
                cmdParam.Connection = con;
                con.Open();

                int retval = cmdParam.ExecuteNonQuery();

                if (retval > 0)
                    result = true;
                else
                    result = false;
            }
            catch (Exception err)
            {
                var objErr = new ExceptionHandler(err, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.LogException();
            }
            finally
            {
                cmdParam.Parameters.Clear();
                CloseConnection();
            }
            return result;
        }

        public int UpdateQryRetRows(string strSql)
        {
            int intRetRows = 0;
            con = new SqlConnection(strConn);

            try
            {
                cmd = new SqlCommand(strSql, con);
                con.Open();

                intRetRows = cmd.ExecuteNonQuery();
            }
            catch (Exception err)
            {
                var objErr = new ExceptionHandler(err, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.LogException();
            }
            finally
            {
                CloseConnection();
            }
            return intRetRows;
        }

        public int UpdateQryRetRows(string sqlStr, SqlParameter[] commandParameters)
        {
            con = new SqlConnection(strConn);
            int intRetRows = 0;

            try
            {
                cmd = new SqlCommand(sqlStr, con);
                con.Open();

                foreach (SqlParameter p in commandParameters)
                {
                    if ((p.Direction == ParameterDirection.InputOutput) && (p.Value == null))
                        p.Value = DBNull.Value;

                    cmd.Parameters.Add(p);
                }

                intRetRows = cmd.ExecuteNonQuery();

            }
            catch (Exception err)
            {
                var objErr = new ExceptionHandler(err, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.LogException();
            }
            finally
            {
                cmd.Parameters.Clear();
                CloseConnection();
            }
            return intRetRows;
        }

        public bool DeleteQry(string strSql)
        {
            bool result = false;
            int intRetRows;
            con = new SqlConnection(strConn);

            try
            {
                cmd = new SqlCommand(strSql, con);
                con.Open();

                intRetRows = cmd.ExecuteNonQuery();
                if (intRetRows > 0)
                {
                    result = true;
                }
            }
            catch (Exception err)
            {
                var objErr = new ExceptionHandler(err, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.LogException();
            }
            finally
            {
                CloseConnection();
            }

            return result;

        }

        public bool DeleteQry(string sqlStr, SqlParameter[] commandParameters)
        {
            con = new SqlConnection(strConn);
            bool result = false;

            try
            {
                cmd = new SqlCommand(sqlStr, con);
                con.Open();

                foreach (SqlParameter p in commandParameters)
                {
                    if ((p.Direction == ParameterDirection.InputOutput) && (p.Value == null))
                        p.Value = DBNull.Value;

                    cmd.Parameters.Add(p);
                }

                int retval = cmd.ExecuteNonQuery();

                if (retval > 0)
                    result = true;
                else
                    result = false;


            }
            catch (Exception err)
            {
                var objErr = new ExceptionHandler(err, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.LogException();

            }
            finally
            {
                cmd.Parameters.Clear();
                CloseConnection();
            }
            return result;
        }

        public bool DeleteQry(SqlCommand cmdParam)
        {
            con = new SqlConnection(strConn);
            bool result = false;

            try
            {
                cmdParam.Connection = con;
                con.Open();

                int retval = cmdParam.ExecuteNonQuery();

                if (retval > 0)
                    result = true;
                else
                    result = false;
            }
            catch (Exception err)
            {
                var objErr = new ExceptionHandler(err, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.LogException();
            }
            finally
            {
                cmdParam.Parameters.Clear();
                CloseConnection();
            }
            return result;
        }

        public string ExecuteScalar(string strSql)
        {
            string val = "";

            SqlConnection con;
            SqlCommand cmd;

            string conStr = GetConString();

            con = new SqlConnection(conStr);

            try
            {
                con.Open();
                cmd = new SqlCommand(strSql, con);
                val = Convert.ToString(cmd.ExecuteScalar());
            }
            catch (Exception err)
            {
                var objErr = new ExceptionHandler(err, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.LogException();
            }
            finally
            {
                CloseConnection();
            }
            return val;
        }

        public string ExecuteScalar(string strSql, SqlParameter[] commandParameters)
        {
            string val = "";
            con = new SqlConnection(strConn);

            try
            {
                cmd = new SqlCommand(strSql, con);
                con.Open();

                foreach (SqlParameter p in commandParameters)
                {
                    if ((p.Direction == ParameterDirection.InputOutput) && (p.Value == null))
                        p.Value = DBNull.Value;

                    cmd.Parameters.Add(p);
                }
                val = Convert.ToString(cmd.ExecuteScalar());

            }
            catch (Exception err)
            {
                var objErr = new ExceptionHandler(err, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.LogException();
            }
            finally
            {
                cmd.Parameters.Clear();
                CloseConnection();
            }
            return val;
        }

        /// <summary>
        /// Execute this function when passing sqlcomand 
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns>returns first rows & first column value</returns>
        public string ExecuteScalar(SqlCommand cmd)
        {
            string val = "";

            string conStr = GetConString();
            con = new SqlConnection(conStr);
            try
            {
                con.Open();
                cmd.Connection = con;
                val = Convert.ToString(cmd.ExecuteScalar());
            }
            catch (Exception err)
            {
                var objErr = new ExceptionHandler(err, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.LogException();
            }
            finally
            {
                CloseConnection();
            }
            return val;
        }

        //using sqlCommand
        public string GetInClauseValue(string input, string fieldName, SqlCommand cmd)
        {
            string[] inputArr = input.Split(',');
            string[] parameters = new string[inputArr.Length];
            try
            {
                for (int i = 0; i < inputArr.Length; i++)
                {
                    cmd.Parameters.Add("@" + fieldName + i, SqlDbType.VarChar, inputArr[i].Length).Value = inputArr[i].ToString();
                    parameters[i] = "@" + fieldName + i;
                }
            }
            catch (Exception err)
            {
                var objErr = new ExceptionHandler(err, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.LogException();
            }
            return string.Join(",", parameters);
        }

        public string GetInClauseValue(string input, string fieldName, out SqlParameter[] commandParameters)
        {
            string[] inputArr = input.Split(',');
            string[] parameters = new string[inputArr.Length];
            commandParameters = null;
            try
            {
                commandParameters = new SqlParameter[inputArr.Length];

                for (int i = 0; i < inputArr.Length; i++)
                {
                    parameters[i] = "@" + fieldName + i;
                    commandParameters[i] = new SqlParameter(parameters[i], inputArr[i]);
                    HttpContext.Current.Trace.Warn(parameters[i].ToString() + " : " + inputArr[i].ToString());
                }

                HttpContext.Current.Trace.Warn(commandParameters.Length.ToString());
            }
            catch (Exception err)
            {
                var objErr = new ExceptionHandler(err, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.LogException();
            }

            return string.Join(",", parameters);
        }

        public SqlParameter[] ConcatenateParams(SqlParameter[] param1, SqlParameter[] param2)
        {
            SqlParameter[] param = null;

            if (param1 != null && param2 != null)
            {
                param = new SqlParameter[param1.Length + param2.Length];

                for (int i = 0; i < param1.Length; i++)
                {
                    param[i] = param1[i];
                }

                for (int i = param1.Length; i < param.Length; i++)
                {
                    param[i] = param2[i - param1.Length];
                }

                for (int i = 0; i < param.Length; i++)
                {
                    HttpContext.Current.Trace.Warn("Param : " + i.ToString() + " : " + param[i]);
                }
            }
            else if (param1 != null)
                param = param1;
            else if (param2 != null)
                param = param2;

            return param;
        }

        public SqlParameter[] ConcatenateParams(SqlParameter[] param1, SqlParameter[] param2, SqlParameter[] param3)
        {
            //first join the first 2 params
            SqlParameter[] paramRes1 = ConcatenateParams(param1, param2);
            return ConcatenateParams(paramRes1, param3);
        }

        public SqlParameter[] ConcatenateParams(SqlParameter[] param1, SqlParameter[] param2, SqlParameter[] param3,
                                                                    SqlParameter[] param4)
        {
            //first join the first 2 params
            SqlParameter[] paramRes1 = ConcatenateParams(param1, param2, param3);
            return ConcatenateParams(paramRes1, param4);
        }
        /// <summary>
        /// similar to method SelectAdaptQry(SqlCommand cmdParam) but the parameters of the input SqlCommand remain intact
        /// </summary>
        public DataSet SelectAdaptQryParamNC(SqlCommand cmdParam)
        {
            con = new SqlConnection(strConn);
            DataSet dataSet = new DataSet();
            SqlDataAdapter adapter = new SqlDataAdapter();

            try
            {
                cmdParam.Connection = con;
                //con.Open();
                adapter.SelectCommand = cmdParam;
                adapter.Fill(dataSet);
            }
            catch (SqlException exSql)
            {
                dataSet = null;
                var objErr = new ExceptionHandler(exSql, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.LogException();
            }
            catch (Exception err)
            {
                HttpContext.Current.Trace.Warn(err.Message + err.Source);
                var objErr = new ExceptionHandler(err, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.LogException();
            }
            finally
            {
                CloseConnection();
            }
            return dataSet;
        }
    }
}
