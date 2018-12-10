using Carwale.Entity.Accessories.Tyres;
using Carwale.Interfaces.Accessories.Tyres;
using Carwale.Notifications.Logs;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Carwale.DAL.Accessories.Tyres
{
    public class TyresRepository : RepositoryBase, ITyresRepository
    {
        public VersionTyres GetTyresByVersionId(int carVersionId)
        {
            try
            {
                var versionTyres = new VersionTyres();
                versionTyres.Tyres = new List<TyreSummary>();

                var param = new DynamicParameters();
                param.Add("v_VersionId", carVersionId > 0 ? carVersionId : Convert.DBNull, dbType: DbType.Int32, direction: ParameterDirection.Input);

                using (var conn = AccessoriesMySqlReadConnection)
                {
                    var tyresList = conn.QueryMultiple("GetTyresByVersionId_v17_9_1", param, commandType: CommandType.StoredProcedure);
                    versionTyres.Tyres = tyresList.Read<TyreSummary>().ToList();
                    versionTyres.VersionTyreSize = tyresList.Read<string>().FirstOrDefault();

                    return versionTyres;
                }
            }
            catch (Exception err)
            {
                Logger.LogException(err);
                return null;
            }
        }

        public List<TyreSummary> GetTyresByModels(string carModelIds)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("v_ModelIds", carModelIds, dbType: DbType.String, direction: ParameterDirection.Input);

                using (var conn = AccessoriesMySqlReadConnection)
                {
                    return conn.Query<TyreSummary>("GetTyresByModels_v17_9_1", param, commandType: CommandType.StoredProcedure).AsList();
                }
            }
            catch (Exception err)
            {
                Logger.LogException(err);
            }
            return null;
        }
        public int GetBrandIdFromMaskingName(string maskingName)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("v_MaskingName", maskingName, dbType: DbType.String, direction: ParameterDirection.Input);

                using (var conn = AccessoriesMySqlReadConnection)
                {
                    return conn.Query<int>("GetBrandIdFromMaskingName", param, commandType: CommandType.StoredProcedure).FirstOrDefault();
                }
            }
            catch (Exception err)
            {
                Logger.LogException(err);
            }
            return -1;
        }

        public List<TyreSummary> GetTyresByBrandId(int brandId)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("v_BrandId", brandId, dbType: DbType.String, direction: ParameterDirection.Input);

                using (var conn = AccessoriesMySqlReadConnection)
                {
                    return conn.Query<TyreSummary>("GetTyresByBrandId_v17_9_1", param, commandType: CommandType.StoredProcedure).AsList();
                }
            }
            catch (Exception err)
            {
                Logger.LogException(err);
            }
            return null;
        }
    }
}
