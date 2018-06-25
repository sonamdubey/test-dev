using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Data;
using System.Data.Common;

namespace MySql.CoreDAL
{
    public static class DbFactory
    {

        static string _readOnlyConnectionString = ConfigurationManager.ConnectionStrings["ReadOnlyConnectionString"].ConnectionString;
        static string _masterConnectionString = ConfigurationManager.ConnectionStrings["MasterConnectionString"].ConnectionString;


        internal static DbConnection GetDBConnection(ConnectionType curConnType)
        {
            switch (curConnType)
            {
                case ConnectionType.MasterDatabase:
                default:
                    return string.IsNullOrEmpty(_masterConnectionString) ? new MySqlConnection() : new MySqlConnection(_masterConnectionString);

                case ConnectionType.ReadOnly:
                    return string.IsNullOrEmpty(_readOnlyConnectionString) ? new MySqlConnection() : new MySqlConnection(_readOnlyConnectionString);
            }

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
                dbParameter.Size = size;
            if (!Convert.IsDBNull(value))
                dbParameter.Value = value;

            return dbParameter;
        }


        public static DbParameter GetDbParamWithColumnName(string parameterName, DbType dbType, int size, string sourceColName)
        {
            DbParameter dbParameter = new MySqlParameter();
            dbParameter.ParameterName = parameterName;
            dbParameter.Direction = ParameterDirection.Input;
            dbParameter.DbType = dbType;
            if (size != -1)
                dbParameter.Size = size;
            dbParameter.SourceColumn = sourceColName;

            return dbParameter;
        }     

        #endregion
    }

    public enum ConnectionType
    {
        MasterDatabase,
        ReadOnly
    }     

}