using Carwale.Entity.CarData;
using Carwale.Entity.Common;
using Carwale.Interfaces.CarData;
using Carwale.Notifications;
using Carwale.Notifications.Logs;
using Carwale.Utility;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Carwale.DAL.CarData
{
    public class CarModelRootsRepository : RepositoryBase,IModelRootsRepository
    {
        public List<RootBase> GetRootsByMake(int makeId, bool isCriticalRead = false)
        {
            var rootList = new List<RootBase>();
            try
            {
                using (var con = isCriticalRead ? CarDataMySqlMasterConnection : CarDataMySqlReadConnection)
                {
                    var param = new DynamicParameters();
                    param.Add("v_MakeId", makeId);
                    return con.Query<RootBase>("GetRootsByMake_v16_11_7",param, commandType: CommandType.StoredProcedure).ToList();                    
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "CarModelRootsRepository.GetRootsByMake()");
                objErr.LogException();
                throw;
            }
        }

        public RootBase GetRootByModel(int modelId, bool isCriticalRead = false)
        {
            try
            {
                using (var con = isCriticalRead ? CarDataMySqlMasterConnection : CarDataMySqlReadConnection)
                {
                    var param = new DynamicParameters();
                    param.Add("v_ModelId", modelId);
                    return con.Query<RootBase>("GetRootByModel_v16_11_7", param, commandType: CommandType.StoredProcedure).First();
                }                
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "CarModelRootsRepository.GetRootByModel()");
                objErr.LogException();
                throw;
            }
        }

        public List<ModelsByRootAndYear> GetModelsByRootAndYear(int rootId, int year, bool isCriticalRead = false)
        {
            try
            {
                using (var con = isCriticalRead ? AccessoriesMySqlMasterConnection : AccessoriesMySqlReadConnection)
                {
                    var param = new DynamicParameters();
                    param.Add("v_RootId", rootId);
                    param.Add("v_ManufacturingYear", year);
                    var data = con.Query<ModelsByRootAndYear>("getAllModelsByRootAndYear", param, commandType: CommandType.StoredProcedure).ToList();

                    if (data.Count > 0)
                        return data;
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return null;
        }

        public List<CarLaunchDiscontinueYear> GetYearsByRootId(int rootId, bool isCriticalRead = false)
        {            
            try
            {
                var param = new DynamicParameters();
                param.Add("v_RootId", rootId);

                using (var con = isCriticalRead ? AccessoriesMySqlMasterConnection : AccessoriesMySqlReadConnection)
                {
                    return con.Query<CarLaunchDiscontinueYear>("GetModelYearsByRoot_v17_8_6", param, commandType: CommandType.StoredProcedure).AsList();                    
                }
            }
            catch (Exception err)
            {
                Logger.LogException(err);
                return null;
            }           
        }

        public IEnumerable<RootBase> GetRoots(string rootIds, bool isCriticalRead = false)
        {
            IEnumerable<RootBase> rootsDetails = null;
            try
            {
                var param = new DynamicParameters();
                param.Add("v_RootIds", rootIds);

                using (var con = isCriticalRead ? ClassifiedMySqlMasterConnection : ClassifiedMySqlReadConnection)
                {
                    rootsDetails = con.Query<RootBase>("cwmasterdb.GetRootsDetails", param, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return rootsDetails;
        }
    }
}
