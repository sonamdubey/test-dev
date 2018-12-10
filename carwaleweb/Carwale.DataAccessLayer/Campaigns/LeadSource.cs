using Carwale.Notifications;
using System;
using System.Collections.Generic;
using System.Data;
using Dapper;

namespace Carwale.DAL.Campaigns
{
    public class LeadSource : RepositoryBase
    {
        public IEnumerable<Entity.PriceQuote.LeadSource> GetAllLeadSources()
        {
            try
            {
                using (var con = NewCarMySqlReadConnection)
                {
                    return con.Query<Entity.PriceQuote.LeadSource>("GetAllLeadSources", null, commandType: CommandType.StoredProcedure).AsList();
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "PQRepository.GetAllLeadSources()");
                objErr.LogException();
                throw;
            }
        }
    }
}
