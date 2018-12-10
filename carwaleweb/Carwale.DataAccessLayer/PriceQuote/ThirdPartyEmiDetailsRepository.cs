using Carwale.Entity.PriceQuote;
using Carwale.Interfaces.PriceQuote;
using Dapper;
using System.Linq;

namespace Carwale.DAL.PriceQuote
{
    public class ThirdPartyEmiDetailsRepository : RepositoryBase, IThirdPartyEmiDetailsRepository
    {
        public ThirdPartyEmiDetails Get(int carVersionId, bool isMetallic)
        {
            string cmd = @"SELECT EmiType, LoanAmount, Emi, LumpsumAmount, TenureInMonth, InterestRate from ThirdPartyEmiDetails WHERE CarVersionId = @CarVersionId and IsMetallic = @IsMetallic;";

            var param = new DynamicParameters();
            param.Add("@CarVersionId", carVersionId);
            param.Add("@IsMetallic", isMetallic);

            using (var con = PricesMySqlReadConnection)
            {
                return con.Query<ThirdPartyEmiDetails>(cmd, param).FirstOrDefault();
            }
        }
    }
}
