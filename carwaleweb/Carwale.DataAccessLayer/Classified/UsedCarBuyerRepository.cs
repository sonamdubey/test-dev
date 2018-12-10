using Carwale.Entity.Classified.Leads;
using Carwale.Interfaces.Classified;
using Dapper;
using System.Data;
using System.Linq;

namespace Carwale.DAL.Classified
{
    public class UsedCarBuyerRepository : RepositoryBase, IUsedCarBuyerRepository
    {
        public BuyerInfo GetBuyerInfo(string userId)
        {
            BuyerInfo buyerInfo = null;
            using (var con = ClassifiedMySqlReadConnection)
            {
                buyerInfo = con.Query<BuyerInfo>($"select mobile, userId, accessToken, isChatLeadGiven from buyercredentials where userId = @v_userId",
                                                new
                                                {
                                                    v_userId = userId
                                                }, commandType: CommandType.Text).FirstOrDefault();
            }
            return buyerInfo;
        }
    }
}
