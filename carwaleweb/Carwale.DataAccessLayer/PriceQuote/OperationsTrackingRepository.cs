using Carwale.Interfaces.PriceQuote;
using Dapper;
using System.Data;

namespace Carwale.DAL.PriceQuote
{
    public class OperationsTrackingRepository : RepositoryBase, IOperationsTrackingRepository
    {
        public void TrackOperations(int versionId, int cityId, bool isMetallic, int sourceCityId, int updatedBy)
        {
            var param = new DynamicParameters();
            param.Add("v_VersionId", versionId);
            param.Add("v_CityId", cityId);
            param.Add("v_IsMetallic", isMetallic);
            param.Add("v_CopiedFromCityId", sourceCityId);
            param.Add("v_CopiedBy", updatedBy);

            using (var con = NewCarMySqlMasterConnection)
            {
                con.Execute("TrackOperations", param, commandType: CommandType.StoredProcedure);
            }
        }
    }
}
