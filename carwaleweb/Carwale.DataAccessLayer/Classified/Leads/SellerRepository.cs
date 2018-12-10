using Carwale.Entity.Classified.Leads;
using Carwale.Interfaces.Classified.Leads;
using Dapper;
using System.Data;
using System.Linq;

namespace Carwale.DAL.Classified.Leads
{
    public class SellerRepository : RepositoryBase, ISellerRepository
    {
        public Seller GetDealerSeller(int inquiryId)
        {
            var param = new DynamicParameters();
            param.Add("v_inquiryid", inquiryId, DbType.Int32);
            using (var con = ClassifiedMySqlReadConnection)
            {
                return con.Query<Seller>("GetDealerSeller_v18_6_1", param, commandType: CommandType.StoredProcedure).FirstOrDefault();
            }
        }

        public Seller GetIndividualSeller(int inquiryId)
        {
            var param = new DynamicParameters();
            param.Add("v_inquiryid", inquiryId, DbType.Int32);

            using (var con = ClassifiedMySqlReadConnection)
            {
                return con.Query<Seller>("GetIndividualSeller", param, commandType: CommandType.StoredProcedure).FirstOrDefault();
            }
        }
    }
}
