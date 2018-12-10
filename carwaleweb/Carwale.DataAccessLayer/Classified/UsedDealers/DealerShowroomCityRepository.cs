using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Carwale.Entity.Classified.UsedDealers;
using Carwale.Notifications;
using Dapper;
using Carwale.Notifications.Logs;

namespace Carwale.DAL.Classified.UsedDealers
{
   public class DealerShowroomCityRepository : RepositoryBase
    {
        public List<DealerShowroomCity> GetDealerShowroomCity()
        {
            try
            {
                using (var con = ClassifiedMySqlReadConnection)
                {
                    LogLiveSps.LogSpInGrayLog("getcitieshavingdealershowroom_v16_9_8");
                    return con.Query<DealerShowroomCity>("getcitieshavingdealershowroom_v16_9_8", commandType: CommandType.StoredProcedure).AsList();
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, ex.Message);
                objErr.LogException();
                return new List<DealerShowroomCity>();
            }
        }

    }
}
