using Carwale.Entity.PriceQuote;
using Carwale.Interfaces.PriceQuote;
using Dapper;
using System.Linq;

namespace Carwale.DAL.PriceQuote
{
    public class ModelSimilarPriceDetailsRepository : RepositoryBase, IModelSimilarPriceDetailsRepo
    {
        public bool Create(ModelSimilarPriceDetail modelSimilarPriceDetail)
        {
            string cmd = @"INSERT INTO ModelSimilarPriceDetails(ModelId, RefreshCount, IsPricesRefreshed)
                               VALUES(@ModelId, @RefreshCount, @IsPricesRefreshed);";

            using (var con = PricesMySqlMasterConnection)
            {
                return con.QueryAsync(cmd, modelSimilarPriceDetail).Id > 0;
            }
        }

        public bool Update(ModelSimilarPriceDetail modelSimilarPriceDetail)
        {
            string cmd = @"UPDATE ModelSimilarPriceDetails SET RefreshCount = @RefreshCount, AvailableOn = @AvailableOn WHERE ModelId = @ModelId;";

            using (var con = PricesMySqlMasterConnection)
            {
                return con.QueryAsync(cmd, modelSimilarPriceDetail).Id > 0;
            }
        }

        public ModelSimilarPriceDetail Get(int modelId)
        {
            string cmd = @"SELECT ModelId, RefreshCount, IsPricesRefreshed, AvailableOn from ModelSimilarPriceDetails WHERE ModelId = @ModelId;";

            var param = new DynamicParameters();
            param.Add("@ModelId", modelId);

            using (var con = PricesMySqlReadConnection)
            {
                return con.Query<ModelSimilarPriceDetail>(cmd, param).FirstOrDefault();
            }
        }
    }
}
