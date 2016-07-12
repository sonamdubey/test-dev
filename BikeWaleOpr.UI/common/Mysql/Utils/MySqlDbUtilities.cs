using BikeWaleOpr.Common;
using MySql.CoreDAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Web;

namespace BikeWaleOPR.Utilities
{
    public class MySqlDbUtilities
    {
        //using sqlCommand
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

        public string GetInClauseValue(string input, string fieldName, out DbParameter[] commandParameters)
        {
            string[] inputArr = input.Split(',');
            string[] parameters = new string[inputArr.Length];
            commandParameters = null;
            try
            {
                commandParameters = new DbParameter[inputArr.Length];

                for (int i = 0; i < inputArr.Length; i++)
                {

                    parameters[i] = "@" + fieldName + i;
                    commandParameters[i] = (DbFactory.GetDbParam(parameters[i], DbType.String, inputArr[i].Length, inputArr[i].ToString()));
                    
                }
            }
            catch (Exception err)
            {

                HttpContext.Current.Trace.Warn("GetInClauseValue: " + err.Message + err.Source + ":GetInClauseValue");
                ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();

            }

            return string.Join(",", parameters);
        }

        public DbParameter[] ConcatenateParams(DbParameter[] param1, DbParameter[] param2)
        {
            DbParameter[] param = null;

            if (param1 != null && param2 != null)
            {
                param = new DbParameter[param1.Length + param2.Length];

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

        public DbParameter[] ConcatenateParams(DbParameter[] param1, DbParameter[] param2, DbParameter[] param3)
        {
            //first join the first 2 params
            DbParameter[] paramRes1 = ConcatenateParams(param1, param2);
            return ConcatenateParams(paramRes1, param3);
        }

        public DbParameter[] ConcatenateParams(DbParameter[] param1, DbParameter[] param2, DbParameter[] param3,
                                                                    DbParameter[] param4)
        {
            //first join the first 2 params
            DbParameter[] paramRes1 = ConcatenateParams(param1, param2, param3);
            return ConcatenateParams(paramRes1, param4);
        }
    }
}