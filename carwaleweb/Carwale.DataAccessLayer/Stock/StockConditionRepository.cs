using Carwale.Entity.Stock;
using Carwale.Interfaces.Stock;
using Carwale.Notifications.Logs;
using Dapper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.DAL.Stock
{
    public class StockConditionRepository : RepositoryBase, IStockConditionRepository
    {
        public bool AddStockCondition(int inquiryId, StockCondition carCondition)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("v_stockid", inquiryId);
                param.Add("v_condition", JsonConvert.SerializeObject(carCondition));
                param.Add("v_carConditionId", dbType: DbType.Int32, direction: ParameterDirection.Output);

                using (var con = ClassifiedMySqlMasterConnection)
                {
                    con.Execute("addstockcondition", param, commandType: CommandType.StoredProcedure);
                    return true;
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return false;
        }

        public List<StockConditionItems> GetCarConditionParts()
        {
            List<StockConditionItems> carConditionParts = null;
            try
            {
                using (var con = ClassifiedMySqlReadConnection)
                {
                    carConditionParts = con.Query<StockConditionItems>("getstockconditionitems", commandType: CommandType.StoredProcedure).ToList();
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex, "Error in fetching car condition parts from database");
            }
            return carConditionParts;
        }

        public StockCondition GetCarConditionResponses(int inquiryId)
        {
            StockCondition stockCondition = null;
            try
            {
                using (var con = ClassifiedMySqlReadConnection)
                {
                    var param = new DynamicParameters();
                    param.Add("v_inquiryId", inquiryId);
                    string jsonString = con.Query<string>("getcarconditionreponses", param, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    stockCondition = JsonConvert.DeserializeObject<StockCondition>(jsonString);
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex, "Error in fetching car condition responses from database");
            }
            return stockCondition;
        }
    }
}
