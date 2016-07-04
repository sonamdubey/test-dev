using log4net;
using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Text;

namespace Bikewale.Notifications
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

        public static void LogSpInGrayLog(SqlCommand cmd)
        {
            if (_enableLiveCallLogs && cmd != null)
            {
                if (_logOnlySpCalls && cmd.CommandType != System.Data.CommandType.StoredProcedure)
                    return;

                StringBuilder sb = new StringBuilder();
                sb.AppendLine(cmd.CommandText);

                foreach (SqlParameter item in cmd.Parameters)
                {
                    switch (item.SqlDbType)
                    {
                        case System.Data.SqlDbType.Text:
                        case System.Data.SqlDbType.VarChar:
                        case System.Data.SqlDbType.DateTime:
                        case System.Data.SqlDbType.Date:
                        case System.Data.SqlDbType.Time:
                        case System.Data.SqlDbType.Char:
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
