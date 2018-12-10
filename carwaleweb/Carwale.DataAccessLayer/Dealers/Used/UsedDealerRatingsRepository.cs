using Carwale.Entity.Dealers;
using Carwale.Interfaces.Dealers.Used;
using Dapper;
using System.Data;
using System.Linq;

namespace Carwale.DAL.Dealers.Used
{
    public class UsedDealerRatingsRepository : RepositoryBase, IUsedDealerRatingsRepository
    {
        public UsedCarDealersRating GetRating(int dealerId)
        {
            UsedCarDealersRating ratingText = null;
            var param = new DynamicParameters();
            param.Add("v_dealerId", dealerId, DbType.Int32);
            using (var con = ClassifiedMySqlReadConnection)
            {
                ratingText = con.Query<UsedCarDealersRating>("GetDealerRating", param, commandType: CommandType.StoredProcedure).FirstOrDefault();
            }
            return ratingText;
        }
        public void SaveRating(int dealerId, string ratingText)
        {
            var param = new DynamicParameters();
            param.Add("v_dealer_id", dealerId, DbType.Int32);
            param.Add("v_rating_text", ratingText, DbType.String);
            using (var con = ClassifiedMySqlMasterConnection)
            {
                con.Execute("SaveDealerRating", param, commandType: CommandType.StoredProcedure);
            }
        }
    }
}
