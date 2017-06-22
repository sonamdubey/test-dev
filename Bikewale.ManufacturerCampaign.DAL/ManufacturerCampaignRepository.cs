using Bikewale.DAL.CoreDAL;
using Bikewale.ManufacturerCampaign.Entities;
using Bikewale.ManufacturerCampaign.Interface;
using Bikewale.Notifications;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.ManufacturerCampaign.DAL
{
    public class ManufacturerCampaignRepository : IManufacturerCampaignRepository
    {
        public ConfigureCampaignEntity getManufacturerCampaign(uint dealerId, uint campaignId)
        {
            ConfigureCampaignEntity objEntity = null;
            try
            {
                using (IDbConnection connection = DatabaseHelper.GetMasterConnection())
                {
                    connection.Open();

                    var param = new DynamicParameters();

                    param.Add("par_campaignId", campaignId);
                    param.Add("par_dealerId", dealerId);

                    objEntity = new ConfigureCampaignEntity();

                    using (var results = connection.QueryMultiple("getmanufacturercampaign", param: param, commandType: CommandType.StoredProcedure))
                    {
                        objEntity.DealerDetails = results.Read<ManufacturerCampaignDetails>().SingleOrDefault();
                        objEntity.CampaignPages = results.Read<ManufacturerCampaignPages>();
                    }

                    if (connection.State == ConnectionState.Open)
                        connection.Close();
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Bikewale.ManufacturerCampaign.DAL.getManufacturerCampaign");
            }
            return objEntity;
        }

    }
}
