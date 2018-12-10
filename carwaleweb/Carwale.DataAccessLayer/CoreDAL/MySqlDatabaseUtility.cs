using Carwale.DAL.CoreDAL.MySql;
using Carwale.Notifications;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Carwale.DAL.CoreDAL
{
    public class MySqlDatabaseUtility
    {
        //using sqlCommand
        public static string GetInClauseValue(string input, string fieldName, DbCommand cmd)
        {
            string[] inputArr = input.Split(',');
            string[] parameters = new string[inputArr.Length];
            try
            {
                for (int i = 0; i < inputArr.Length; i++)
                {
                    cmd.Parameters.Add(DbFactory.GetDbParam("@" + fieldName.ToLower() + i, DbType.String, inputArr[i].Length, inputArr[i]));
                    parameters[i] = "@" + fieldName.ToLower() + i;
                }
            }
            catch (Exception err)
            {
                var objErr = new ExceptionHandler(err, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.LogException();
            }
            return string.Join(",", parameters);
        }

        public static string GetInClauseValue(string input, string fieldName, out DbParameter[] commandParameters)
        {
            string[] inputArr = input.Split(',');
            string[] parameters = new string[inputArr.Length];
            commandParameters = null;
            try
            {
                commandParameters = new DbParameter[inputArr.Length];

                for (int i = 0; i < inputArr.Length; i++)
                {
                    parameters[i] = "@" + fieldName.ToLower() + i;
                    commandParameters[i] = DbFactory.GetDbParam(parameters[i], DbType.String, inputArr[i].Length, inputArr[i]);
                    HttpContext.Current.Trace.Warn(parameters[i] + " : " + inputArr[i]);
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

        public static string ConvertDataTableToString(DataTable dt)
        {
            if (dt == null)
                return string.Empty;

            StringBuilder sb = new StringBuilder();

            for (int rowId = 0; rowId < dt.Rows.Count; rowId++)
            {

                for (int colId = 0; colId < dt.Columns.Count; colId++)
                {
                    sb.Append(dt.Rows[rowId][colId].ToString());
                    if (colId < dt.Columns.Count - 1)
                        sb.Append("#c0l#");
                }
                if (rowId < dt.Rows.Count - 1)
                    sb.Append("|r0w|");
            }
            return sb.ToString();
        }
    }
}
