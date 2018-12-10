using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Data;
using System.Data.Common;

namespace Carwale.DAL.CoreDAL.MySql
{
    public static class DbFactory
    {

        internal static DbConnection GetDBConnection(string connString)
        {
            return string.IsNullOrEmpty(connString) ? new MySqlConnection() : new MySqlConnection(connString);
        }

        #region DBCommand
        public static DbCommand GetDBCommand()
        {
            return GetDBCommand(string.Empty);
        }

        public static DbCommand GetDBCommand(string sqlCommandText)
        {
            return string.IsNullOrEmpty(sqlCommandText) ? new MySqlCommand() : new MySqlCommand(sqlCommandText);
        }

        public static DbCommand CloneDBCommand(DbCommand cmd)
        {
            MySqlCommand newCmd;
            MySqlCommand typeCastedCmd=cmd as MySqlCommand;
            if (typeCastedCmd == null)
            {
                return null;
            }

            newCmd  = typeCastedCmd.Clone();            
            return newCmd;
        }
        #endregion

        #region DataAdaptor

        public static DbDataAdapter GetDBDataAdaptor()
        {
            return new MySqlDataAdapter();
        }

        #endregion

        #region DBParameters
        public static DbParameter[] GetDbParamArray(int size)
        {
            return new MySqlParameter[size];
        }

        public static DbParameter GetDbParam<T>(string parameterName, DbType dbType, T value)
        {
            return GetDbParam(parameterName, dbType, -1,ParameterDirection.Input, value);
        }

        public static DbParameter GetDbParam(string parameterName, DbType dbType, ParameterDirection direction)
        {
            return GetDbParam(parameterName, dbType, -1, direction, Convert.DBNull);
        }

        public static DbParameter GetDbParam<T>(string parameterName, DbType dbType, ParameterDirection direction, T value)
        {
            return GetDbParam(parameterName, dbType, -1, direction, value);
        }     

        public static DbParameter GetDbParam<T>(string parameterName, DbType dbType, int size, T value)
        {
            return GetDbParam(parameterName, dbType, size, ParameterDirection.Input, value);
        }

        public static DbParameter GetDbParam(string parameterName, DbType dbType, int size, ParameterDirection direction)
        {
            return GetDbParam(parameterName, dbType, size, direction, Convert.DBNull);
        }

        public static DbParameter GetDbParam<T>(string parameterName, DbType dbType, int size, ParameterDirection direction, T value)
        {
            DbParameter dbParameter = new MySqlParameter();

            dbParameter.ParameterName = parameterName;
            dbParameter.Direction = direction;
            dbParameter.DbType = dbType;
            if (size != -1)
            {
                dbParameter.Size = size;
            }
            if (!Convert.IsDBNull(value))
            {
                dbParameter.Value = value;
            }

            return dbParameter;
        }


        public static DbParameter GetDbParamWithColumnName(string parameterName, DbType dbType, int size, string sourceColName)
        {
            DbParameter dbParameter = new MySqlParameter();
            dbParameter.ParameterName = parameterName;
            dbParameter.Direction = ParameterDirection.Input;
            dbParameter.DbType = dbType;
            if (size != -1)
            {
                dbParameter.Size = size;
            }

            dbParameter.SourceColumn = sourceColName;

            return dbParameter;
        }     

        #endregion
    }   

}