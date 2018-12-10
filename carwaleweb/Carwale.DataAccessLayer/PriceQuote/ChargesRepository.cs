using Carwale.Entity.Price;
using Carwale.Interfaces.PriceQuote;
using Carwale.Notifications.Logs;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Carwale.DAL.PriceQuote
{
    public class ChargesRepository : RepositoryBase, IChargesRepository
    {
        public Dictionary<int,Charge> GetCharges()
        {
            try
            {
                using (var con = NewCarMySqlReadConnection)
                {
                    return con.Query<Charge>("GetAllCharges", commandType: CommandType.StoredProcedure).ToDictionary(key => key.Id, val => val);
                }
            }
            catch (Exception e)
            {
                Logger.LogException(e);
                throw;
            }
        }

        public List<int> GetComponents(int chargeId)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("v_ChargeId", chargeId, dbType: DbType.Int16);

                using (var con = NewCarMySqlReadConnection)
                {
                    return con.Query<int>("GetChargeComponents", param, commandType: CommandType.StoredProcedure).ToList();
                }
            }
            catch (Exception e)
            {
                Logger.LogException(e);
                throw;
            }
        }
    }
}
