using Carwale.Interfaces.ProfanityFilter;
using Dapper;
using System.Collections.Generic;
using System.Data;

namespace Carwale.DAL.ProfanityFilter
{
    public class BadWordsRepository : RepositoryBase, IBadWordsRepository
    {
        public void InsertBadWords(IEnumerable<string> badWords)
        {
            var param = new DynamicParameters();
            param.Add("v_badWords", string.Join(",", badWords), DbType.String);

            using (var con = ClassifiedMySqlMasterConnection)
            {
                con.Execute("InsertBadWords", param, commandType: CommandType.StoredProcedure);
            }
        }

        public void DeleteBadWords(IEnumerable<string> badWords)
        {
            var param = new DynamicParameters();
            param.Add("v_badWords", string.Join(",", badWords), DbType.String);

            using (var con = ClassifiedMySqlMasterConnection)
            {
                con.Execute("DeleteBadWords", param, commandType: CommandType.StoredProcedure);
            }
        }
    }
}
