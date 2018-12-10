using Carwale.Entity.Price;
using Carwale.Entity.PriceQuote;
using Carwale.Interfaces.PriceQuote;
using Carwale.Notifications.Logs;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Carwale.DAL.PriceQuote
{
    public class ChargeGroupsRepository : RepositoryBase, IChargeGroupsRepository
    {
        public Dictionary<int, ChargeGroup> GetChargeGroups()
        {
            try
            {
                using (var con = NewCarMySqlReadConnection)
                {
                    return con.Query<ChargeGroup>("GetCategories_v17_11_1", commandType: CommandType.StoredProcedure).ToDictionary(key => key.Id, value => value);
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
