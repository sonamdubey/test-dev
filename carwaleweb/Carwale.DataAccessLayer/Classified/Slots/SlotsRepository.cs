using Carwale.Entity.Classified;
using Carwale.Interfaces.Classified.Slots;
using Dapper;
using System.Collections.Generic;
using System.Data;

namespace Carwale.DAL.Classified.Slots
{
    public class SlotsRepository : RepositoryBase, ISlotsRepository
    {
        public IEnumerable<Slot> GetSlotsByCityId(int cityId)
        {
            var sql = @" select s.Id, s.FranchiseeProbability, s.DiamondProbability, s.PlatinumProbability
                         from slots s inner join cityslotsmapping csm on s.id = csm.slotId
                         where csm.cityId = @v_cityid order by rank;";

            using (var con = ClassifiedMySqlReadConnection)
            {
                return con.Query<Slot>(sql, new { v_cityid = cityId }, commandType: CommandType.Text);
            }
        }
    }
}
