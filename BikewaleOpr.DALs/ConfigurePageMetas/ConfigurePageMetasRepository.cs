using Bikewale.DAL.CoreDAL;
using Bikewale.Notifications;
using BikewaleOpr.Entity.ConfigurePageMetas;
using BikewaleOpr.Interface.ConfigurePageMetas;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikewaleOpr.DALs.ConfigurePageMetas
{
    public class ConfigurePageMetasRepository : IConfigurePageMetasRepository
    {
        public IEnumerable<PageEntity> GetPagesList()
        {
            IEnumerable<PageEntity> objPageList = null;
            try
            {
                using (IDbConnection connection = DatabaseHelper.GetMasterConnection())
                {
                    connection.Open();

                    var param = new DynamicParameters();
                    objPageList = connection.Query<PageEntity>("getbwpagemaster", param: param, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, string.Format("ConfigurePageMetasRepository.GetPagesList"));
            }
            return objPageList;
        }

        /// <summary>
        /// Created by : Ashutosh Sharma on 14-Aug-2017
        /// Description : Method to get active or inactive or both page metas list.
        /// </summary>
        /// <param name="pageMetaStatus">0 for inactive, 1 for active and 2 for both.</param>
        /// <returns></returns>
        public IEnumerable<PageMetaEntity> GetPageMetas(uint pageMetaStatus)
        {
            IEnumerable<PageMetaEntity> objPageMetasList = null;
            try
            {
                using (IDbConnection connection = DatabaseHelper.GetMasterConnection())
                {
                    var param = new DynamicParameters();
                    param.Add("par_status", pageMetaStatus);
                    objPageMetasList = connection.Query<PageMetaEntity>("getpagemetas", param: param, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, string.Format("ConfigurePageMetasRepository.GetPageMetas_pageMetaStatus : {0}", pageMetaStatus));
            }
            return objPageMetasList;
        }
    }
}
