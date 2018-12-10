using Carwale.Entity.Stock;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Carwale.Notifications.Logs;
using Carwale.Interfaces.Stock;

namespace Carwale.DAL.Stock
{
    public class StockFinanceRepository : RepositoryBase, IStockFinanceRepository
    {
        public FinanceEligibility GetFinanceEligibilityCriteria()
        {
            try
            {
                using (var con = ClassifiedMySqlReadConnection)
                {
                    var financeCriterias = con.QueryMultiple("GetFinanceEligibilityCriteria", commandType: CommandType.StoredProcedure);
                    FinanceEligibility stockFinanceRepository = new FinanceEligibility();
                    stockFinanceRepository.CityIds = financeCriterias.Read<int>().ToList();
                    stockFinanceRepository.ModelIds = financeCriterias.Read<int>().ToList();
                    stockFinanceRepository.ExcludedDealerIds = financeCriterias.Read<int>().ToList();
                    stockFinanceRepository.ThresholdValue = financeCriterias.Read<int>().ToList();
                    return stockFinanceRepository;
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return null;
        }
    }
}
