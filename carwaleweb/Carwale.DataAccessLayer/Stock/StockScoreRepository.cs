using Carwale.Entity.Stock;
using Carwale.Interfaces.Stock;
using Dapper;
using System.Data;
using System.Linq;

namespace Carwale.DAL.Stock
{
    public class StockScoreRepository : RepositoryBase, IStockScoreRepository
    {
        public StockSortScore GetStockScore(string profileId)
        {
            using (var con = ClassifiedMySqlReadConnection)
            {
                return con.Query<StockSortScore>("select SortScore, ExpiryDate from alteredsortscore where ProfileId = @v_profileId; ",
                    new
                    {
                        v_profileId = profileId
                    }, commandType: CommandType.Text).FirstOrDefault();
            }
        }

        public bool UpdateStockScore(string profileId, StockSortScore stockSortScore)
        {
            string query = @"update livelistings set sortscore = @v_sortscore where profileid = @v_profileId";
            int rowsAffected;
            using (var con = ClassifiedMySqlMasterConnection)
            {
                rowsAffected = con.Execute(query, new { v_profileId = profileId, v_sortScore = stockSortScore.SortScore }, commandType: CommandType.Text);
            }
            return rowsAffected > 0;
        }
    }
}
