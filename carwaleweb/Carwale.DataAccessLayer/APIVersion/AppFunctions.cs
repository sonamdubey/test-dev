using AEPLCore.Logging;
using Carwale.DAL.CoreDAL;
using Carwale.DAL.CoreDAL.MySql;
using Carwale.Notifications;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Carwale.DAL
{
    public static class AppFunctions
    {
		private static Logger Logger = LoggerFactory.GetLogger();
		public static void CheckVersionStatus(int appVersion, int sourceId, out bool isSupported , out bool isLatest)
        {
            isSupported = false; isLatest = false;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("CheckVersionStatusForApp_v16_11_7"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_AppVersionId", DbType.Int16, appVersion));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_SourceId",DbType.Int16,sourceId));
                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, DbConnections.CarDataMySqlReadConnection))
                    {
                        while (dr.Read())
                        {
                            isSupported = Convert.ToBoolean(dr["IsSupported"].ToString());
                            isLatest = Convert.ToBoolean(dr["IsLatest"].ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
				Logger.LogException(ex);
			}
        }
    }
}
