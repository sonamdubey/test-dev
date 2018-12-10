using log4net;
using System;
using System.Configuration;
using System.Data.Common;
using System.Data.SqlClient;
using System.Text;

namespace Carwale.Notifications.Logs
{
    public static class LogLiveSps
    {
        static readonly ILog log = LogManager.GetLogger(typeof(LogLiveSps));
        static bool _logOnlySpCalls;
        static bool _enableLiveCallLogs;
        static LogLiveSps()
        {
            _logOnlySpCalls = Convert.ToBoolean(ConfigurationManager.AppSettings["LogOnlySpCalls"]);
            _enableLiveCallLogs = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableLiveCallLogs"]);
        }

        public static void LogSpInGrayLog(DbCommand cmd)
        {
            if (_enableLiveCallLogs && cmd != null)
            {
                if (_logOnlySpCalls && cmd.CommandType != System.Data.CommandType.StoredProcedure)
                    return;

                StringBuilder sb = new StringBuilder();
                sb.AppendLine(cmd.CommandText);

                foreach (DbParameter item in cmd.Parameters)
                {
                    switch (item.DbType)
                    {
                        case System.Data.DbType.String:
                        case System.Data.DbType.AnsiString:
                        case System.Data.DbType.DateTime:
                        case System.Data.DbType.Date:
                        case System.Data.DbType.Time:
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

        public static void LogMySqlSpInGrayLog(DbCommand cmd)
        {
            if (_enableLiveCallLogs && cmd != null)
            {
                if (_logOnlySpCalls && cmd.CommandType != System.Data.CommandType.StoredProcedure)
                    return;

                StringBuilder sb = new StringBuilder();
                sb.AppendLine(cmd.CommandText);

                foreach (DbParameter item in cmd.Parameters)
                {
                    switch (item.DbType)
                    {
                        case System.Data.DbType.String:
                        case System.Data.DbType.AnsiString:
                        case System.Data.DbType.DateTime:
                        case System.Data.DbType.DateTime2:
                        case System.Data.DbType.Date:
                        case System.Data.DbType.Time:
                        case System.Data.DbType.AnsiStringFixedLength:
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

        public static void LogSpInGrayLog(string errorMsg)
        {
            if (_enableLiveCallLogs)
            {
                log.Error(errorMsg);
            }
        }
    }
}
