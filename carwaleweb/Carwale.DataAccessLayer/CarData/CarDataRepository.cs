using Carwale.Interfaces.CarData;
using Dapper;
using System;
using System.Data;
using Carwale.Notifications;
using System.Web;
using Carwale.Entity;
using Carwale.Notifications.Logs;

namespace Carwale.DAL.CarData
{
    public class CarDataRepository : RepositoryBase, ICarDataRepository
    {
        public CarEntity GetVersionModel(int versionId)
        {
            try
            {
                CarEntity carMakeModel = new CarEntity();
                var param = new DynamicParameters();
                param.Add("v_VersionId", versionId, dbType: DbType.Int32, direction: ParameterDirection.Input);
                param.Add("v_MakeId", dbType: DbType.Int32, direction: ParameterDirection.Output);
                param.Add("v_ModelId", dbType: DbType.Int32, direction: ParameterDirection.Output);


                using (var conn = NewCarMySqlReadConnection)
                {
                    conn.Query("GetVersionModel_v16_11_7", param, commandType: CommandType.StoredProcedure);
                    LogLiveSps.LogSpInGrayLog("[dbo].[GetVersionModel]");
                }

                carMakeModel.MakeId = param.Get<int>("v_MakeId");
                carMakeModel.ModelId = param.Get<int>("v_ModelId");

                if (carMakeModel.MakeId <= 0 || carMakeModel.ModelId <= 0)
                    return null;

                return carMakeModel;
            }
            catch (Exception e)
            {
                var exception = new ExceptionHandler(e, HttpContext.Current.Request.ServerVariables["URL"]);
                exception.LogException();
                return null;
            }
        }
    }
}
