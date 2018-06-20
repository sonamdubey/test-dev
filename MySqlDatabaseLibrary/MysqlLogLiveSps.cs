using log4net;
using System;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Text;

namespace MySql.CoreDAL.Logging
{
    public static class MysqlLogLiveSps
    {
        static readonly ILog log = LogManager.GetLogger(typeof(MysqlLogLiveSps));
        static bool _enableLiveCallLogs;

        static MysqlLogLiveSps()
        {
            _enableLiveCallLogs = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableLiveCallLogs"]);
        }

        public static void LogSpInGrayLog(DbCommand cmd)
        {
            if (_enableLiveCallLogs && cmd != null)
            {
         
                StringBuilder sb = new StringBuilder();
                sb.AppendLine(cmd.CommandText);

                foreach (DbParameter item in cmd.Parameters)
                {
                    switch (item.DbType)
                    {
                        case DbType.String:
                        case DbType.DateTime:
                        case DbType.Date:
                        case DbType.Time:
                            sb.AppendLine(string.Format("{0} = '{1}'", item.ParameterName, item.Value));
                            break;
                        default:
                            sb.AppendLine(string.Format("{0} = {1}", item.ParameterName, item.Value));
                            break;
                    }

                }
                sb.AppendLine(";");

                log.Error(sb.ToString());
            }
        }

    }
}
