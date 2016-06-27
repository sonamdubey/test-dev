using Bikewale.Notifications;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;

//SQL Syntex For Parameterised Query
//UPDATE TableName SET value1=@value1, value2=@value2 WHERE Id=@Id

//Parameter Syntex
//SqlParameter[] param = new SqlParameter[2];
//param[1] = new SqlParameter("@value2", val2.ToString());
//param[0] = new SqlParameter("@value1", val1.ToString());
//param[2] = new SqlParameter("@Id", id.ToString());

namespace Bikewale.CoreDAL
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

        //constructor which assigns the connection string, as passed in the argument
        public Database()
        {
            this.strConn = ConfigurationManager.AppSettings["bwconnectionString"];
        }

        //constructor which connects it with available live data
        public Database(bool isTesting)
        {
            if (isTesting)
                this.strConn = ConfigurationManager.AppSettings["connectionStringTest"];
            else
                this.strConn = ConfigurationManager.AppSettings["bwconnectionString"];
        }

        public Database(string strConn)
        {
            this.strConn = strConn;
        }

        //return the connection string
        public string GetConString()
        {
            return strConn;
        }

        public SqlDataReader SelectQry(string strSql)
        {
            SqlDataReader dataReader = null;
            con = new SqlConnection(strConn);

            try
            {
                con.Open();
                cmd = new SqlCommand(strSql, con);
                dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            }

            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"] + String.Format(" strSql : {0}", strSql));
                objErr.SendMail();
            }

            finally
            {
                CloseConnection();
            }

            return dataReader;
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
            }

            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"] + String.Format(" commandParameters : {0}", Convert.ToString(commandParameters)));
                objErr.SendMail();
            }

            finally
            {
                cmd.Parameters.Clear();
                CloseConnection();
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
                HttpContext.Current.Trace.Warn(err.Message + err.Source);
                ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"] + String.Format(" cmdParam : {0}", cmdParam));
                objErr.SendMail();
            }

            finally
            {
                cmdParam.Parameters.Clear();
                CloseConnection();
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
            catch (Exception err)
            {
                CloseConnection();
                HttpContext.Current.Trace.Warn(err.Message + err.Source);
                ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
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

            try
            {
                HttpContext.Current.Trace.Warn("checkstart");
                SqlDataAdapter adapter = new SqlDataAdapter();
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
            catch (Exception err)
            {
                CloseConnection();
                HttpContext.Current.Trace.Warn(err.Message + err.Source);
                ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();

            }
            finally
            {
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
                con.Open();
                adapter.SelectCommand = cmdParam;

                adapter.Fill(dataSet);
            }
            catch (Exception err)
            {
                HttpContext.Current.Trace.Warn(err.Message + err.Source);
                ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            finally
            {
                cmdParam.Parameters.Clear();
                adapter.SelectCommand.Parameters.Clear();
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
                    objTrace.Trace.Warn("database:CloseConnection:Connection is closed successfully.");
                    con.Close();
                }
            }
            catch (Exception)
            {
                //do nothing
                //HttpContext.Current.Trace.Warn(err.Message + err.Source);
            }

        }

        public bool InsertQry(string strSql)
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

                cmd.Parameters.Clear();
            }
            catch (Exception err)
            {
                HttpContext.Current.Trace.Warn(err.Message + err.Source);
                ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();

            }
            finally
            {
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
                HttpContext.Current.Trace.Warn(err.Message + err.Source);
                ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
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

                cmd.Parameters.Clear();
            }
            catch (Exception err)
            {
                HttpContext.Current.Trace.Warn(err.Message + err.Source);
                ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            finally
            {
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
                HttpContext.Current.Trace.Warn(err.Message + err.Source);
                ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
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
                HttpContext.Current.Trace.Warn(err.Message + err.Source);
                ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
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
                cmd.Parameters.Clear();
            }
            catch (Exception err)
            {
                HttpContext.Current.Trace.Warn(err.Message + err.Source);
                ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            finally
            {
                CloseConnection();
            }
            return intRetRows;
        }

        public bool DeleteQry(string strSql)
        {
            bool result;
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
                else
                {
                    result = false;
                }
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

                cmd.Parameters.Clear();
            }
            catch (Exception err)
            {

                HttpContext.Current.Trace.Warn(err.Message + err.Source);
                ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();

            }
            finally
            {
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
                HttpContext.Current.Trace.Warn(err.Message + err.Source);
                ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
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

            Database db = new Database();
            string conStr = db.GetConString();

            con = new SqlConnection(conStr);

            try
            {
                con.Open();
                cmd = new SqlCommand(strSql, con);
                val = Convert.ToString(cmd.ExecuteScalar());
            }
            catch (Exception err)
            {
                HttpContext.Current.Trace.Warn(err.Message + err.Source);
                ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
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
                cmd.Parameters.Clear();
            }
            catch (Exception err)
            {
                HttpContext.Current.Trace.Warn(err.Message + err.Source);
                ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();

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

                HttpContext.Current.Trace.Warn("GetCommandValue: " + err.Message + err.Source + ":GetCommandValue");
                ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
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
                }

                HttpContext.Current.Trace.Warn(commandParameters.Length.ToString());
            }
            catch (Exception err)
            {

                HttpContext.Current.Trace.Warn("GetInClauseValue: " + err.Message + err.Source + ":GetInClauseValue");
                ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();

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

        public int ExecuteScalarVal(string SP, SqlParameter[] commandParameters)
        {
            int val = 0;
            con = new SqlConnection(strConn);

            try
            {
                cmd = new SqlCommand(SP, con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                cmd.Parameters.AddRange(commandParameters);
                val = Convert.ToInt32(cmd.ExecuteScalar());

            }
            catch (Exception err)
            {
                HttpContext.Current.Trace.Warn(err.Message + err.Source);
                ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();

            }
            finally
            {
                cmd.Parameters.Clear();
                CloseConnection();
            }
            return val;
        }


    }//class
}//namespace
