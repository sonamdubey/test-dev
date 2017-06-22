using Bikewale.DAL.CoreDAL;
using Bikewale.ManufacturerCampaign.Interface;
using Bikewale.Notifications;
using Bikewale.Utility;
using BikewaleOpr.Entities;
using Dapper;
using MySql.CoreDAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.ManufacturerCampaign.DAL
{
   public class ManufacturerCampaignRepository: IManufacturerCampaign
    {

        public IEnumerable<ManufacturerEntity> GetManufacturersList()
        {
            IEnumerable<ManufacturerEntity> manufacturers = null;
            try
            {
                using (IDbConnection connection = DatabaseHelper.GetMasterConnection())
                {
                    connection.Open();

                    var param = new Dapper.DynamicParameters();
                    manufacturers = connection.Query<ManufacturerEntity>("getdealerasmanufacturer", param: param, commandType: CommandType.StoredProcedure);

                    if (connection.State == ConnectionState.Open)
                        connection.Close();
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "BikewaleOpr.DALs.ManufactureCampaign.GetManufactureCampaigns");
            }
           

            return manufacturers;
        }
    }
}
