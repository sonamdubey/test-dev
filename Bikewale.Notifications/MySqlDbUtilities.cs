using MySql.CoreDAL;
using System;
using System.Data;
using System.Data.Common;
using System.Web;

namespace Bikewale.Notifications.MySqlUtility
{
    public class MySqlDbUtilities
    {
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

                ErrorClass.LogError(err, HttpContext.Current.Request.ServerVariables["URL"]);
            }
            return string.Join(",", parameters);
        }
    }
}
