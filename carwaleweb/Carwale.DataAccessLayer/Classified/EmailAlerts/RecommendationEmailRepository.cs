using Carwale.Entity.Classified.EmailAlerts;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;

namespace Carwale.DAL.Classified.EmailAlerts
{
    public class RecommendationEmailRepository : RepositoryBase
    {
        public List<RecommendationEmailData> GetRecommendationEmailData()
        {
            List<RecommendationEmailData> custList = null;
            using (var con = ClassifiedMySqlReadConnection)
            {
                custList = con.Query<RecommendationEmailData>("GetRecommendationEmailData", commandType: CommandType.StoredProcedure).ToList();
            }
            return custList;
        }
    }
}
