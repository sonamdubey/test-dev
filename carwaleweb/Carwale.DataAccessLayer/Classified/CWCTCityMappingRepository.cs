using Carwale.Notifications;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Carwale.Interfaces.Classified;
using Carwale.Notifications.Logs;

namespace Carwale.DAL.Classified
{
    public class CWCTCityMappingRepository : RepositoryBase, ICWCTCityMappingRepositiory
    {
        public bool IsCarTradeCity(int cityId){
            short isCartadeCity = -1;
            try
            { 
                var param = new DynamicParameters();
                param.Add("v_CWCityId", cityId);
                param.Add("v_IsCartradeCity", dbType: DbType.Int16, direction: ParameterDirection.Output);
                using (var con = ClassifiedMySqlReadConnection)
                {
                    con.Execute("IsCartradeCity", param, commandType: CommandType.StoredProcedure);
                    LogLiveSps.LogSpInGrayLog("IsCartradeCity");
                }
                isCartadeCity = param.Get<short>("v_IsCartradeCity");
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            return (isCartadeCity > 0)?true:false;
        }
    }
}
