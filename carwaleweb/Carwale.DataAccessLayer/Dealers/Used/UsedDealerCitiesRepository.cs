using Carwale.Entity.UsedCarsDealer;
using Carwale.Interfaces.Dealers.Used;
using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Carwale.DAL.Dealers.Used
{
    public class UsedDealerCitiesRepository : RepositoryBase, IUsedDealerCitiesRepository
    {
        public DealerCities GetCities(int dealerId)
        {
            DealerCities dealerCities = null;
            var param = new DynamicParameters();
            param.Add("v_dealerId", dealerId, DbType.Int32);
            using (var con = ClassifiedMySqlReadConnection)
            {
                dealerCities = con.Query<DealerCities>("GetDealerCities", param, commandType: CommandType.StoredProcedure).FirstOrDefault();
            }
            return dealerCities;
        }
        public bool SaveCities(int dealerId, IEnumerable<int> cityIds)
        {
            var param = new DynamicParameters();
            param.Add("v_dealerId", dealerId, DbType.Int32);
            param.Add("v_cityIds", cityIds == null ? null : string.Join(",", cityIds), DbType.String);
            param.Add("v_IsEveryCityValid", DbType.Int32, direction: ParameterDirection.Output);
            using (var con = ClassifiedMySqlMasterConnection)
            {
                con.Execute("SaveDealerCities", param, commandType: CommandType.StoredProcedure);
            }
            return param.Get<int>("v_IsEveryCityValid") == 1;
        }
    }
}
