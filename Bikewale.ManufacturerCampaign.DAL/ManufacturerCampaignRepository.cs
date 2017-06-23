
using Bikewale.ManufacturerCampaign.Entities;
using Bikewale.ManufacturerCampaign.Interface;
using Bikewale.Notifications;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;

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
using BikewaleOpr.Entity.ManufacturerCampaign;
using Bikewale.DAL.CoreDAL;

namespace Bikewale.ManufacturerCampaign.DAL
{

    public class ManufacturerCampaignRepository : IManufacturerCampaignRepository
    {

        public IEnumerable<ManufacturerEntity> GetManufacturersList()
        {
            IEnumerable<ManufacturerEntity> manufacturers = null;

            try
            {
                using (IDbConnection connection = DatabaseHelper.GetMasterConnection())
                {
                    connection.Open();


                    var param = new DynamicParameters();


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
        public uint saveManufacturerCampaign(ConfigureCampaignSave objCampaign)
        {
            uint campaignId = 0;
            try
            {
                using (IDbConnection connection = DatabaseHelper.GetMasterConnection())
                {
                    connection.Open();
                    var param = new DynamicParameters();
                    param.Add("par_userid", objCampaign.UserId);
                    param.Add("par_dealerid", objCampaign.DealerId);
                    param.Add("par_description", objCampaign.Description);
                    param.Add("par_maskingnumber", objCampaign.MaskingNumber);
                    param.Add("par_dailyleadlimit", objCampaign.DailyLeadLimit);
                    param.Add("par_totalleadlimit", objCampaign.TotalLeadLimit);
                    param.Add("par_campaignpages", objCampaign.CampaignPages);
                    param.Add("par_startDate", objCampaign.StartDate);
                    param.Add("par_endDate", objCampaign.EndDate);
                    param.Add("par_showonexshowroomprice", objCampaign.ShowOnExShowroomPrice);
                    param.Add("par_campaignid", objCampaign.CampaignId, dbType: DbType.Int32, direction: ParameterDirection.InputOutput);
                    connection.Query<dynamic>("savemanufacturercampaign_21062017", param: param, commandType: CommandType.StoredProcedure);
                    campaignId = (uint)param.Get<int>("par_campaignid");
                    if (connection.State == ConnectionState.Open)
                        connection.Close();
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Bikewale.ManufacturerCampaign.DAL.getManufacturerCampaign");
            }
            return campaignId;
        }
    }
}