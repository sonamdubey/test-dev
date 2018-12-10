using Carwale.Entity.Stock;
using Carwale.Interfaces.Stock;
using Dapper;
using System.Linq;
using System.Data;

namespace Carwale.DAL.Stock
{
    public class StockPackagesRepository : RepositoryBase, IStockPackagesRepository
    {

        public StockBoostPackageDetails GetBoostPackage(int inquiryId)
        {
            string query = "select BoostPackageId, BoostLeadThreshold from sellinquiries where id = @v_inquiryId;";
            using (var con = ClassifiedMySqlMasterConnection)
            {
                return con.Query<StockBoostPackageDetails>(query, new { v_inquiryId = inquiryId }, commandType: CommandType.Text).FirstOrDefault();
            }
        }

        public BoostPackResponseStatus UpdateBoostPackage(int inquiryId, bool isDealer, int? boostPackageId)
        {
            BoostPackResponseStatus status;
            var param = new DynamicParameters();
            param.Add("v_inquiryId", inquiryId, DbType.Int32);
            param.Add("v_sellerType", isDealer ? 1 : 2, DbType.Int16);
            param.Add("v_boostPackageId", boostPackageId, DbType.Int32);
            param.Add("v_errorResponse", 0, DbType.Int32, direction: ParameterDirection.Output);

            using (var con = ClassifiedMySqlMasterConnection)
            {
                con.Execute("SaveBoostPackage", param, commandType: CommandType.StoredProcedure);
                status = (BoostPackResponseStatus)param.Get<int>("v_errorResponse");
            }
            return status;
        }
    }
}
