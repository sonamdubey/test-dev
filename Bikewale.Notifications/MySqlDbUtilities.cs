using Bikewale.Notifications.CoreDAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Bikewale.Notifications.MySqlUtility
{
    public class MySqlDbUtilities
    {
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

        public string GetInClauseValue(string input, string fieldName, DbCommand cmd)
        {
            string[] inputArr = input.Split(',');
            string[] parameters = new string[inputArr.Length];
            try
            {
                for (int i = 0; i < inputArr.Length; i++)
                {
                    cmd.Parameters.Add(DbFactory.GetDbParam("@" + fieldName + i, DbType.String, inputArr[i].Length, inputArr[i].ToString()));
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
    }
}
