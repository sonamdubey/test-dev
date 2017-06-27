
using Dapper;
using Bikewale.ManufacturerCampaign.Entities;
using Bikewale.ManufacturerCampaign.Interface;
using Bikewale.Notifications;
using System;
using System.Collections.Generic;
using System.Data;
using BikewaleOpr.Entities;
using System.Linq;
using BikewaleOpr.Entity.ManufacturerCampaign;
using Bikewale.DAL.CoreDAL;
using Bikewaleopr.ManufacturerCampaign.Entities;

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

        public IEnumerable<BikeMakeEntity> GetBikeMakes()
        {
            IEnumerable<BikeMakeEntity> bikeMakes = null;
            try
            {
                using (IDbConnection connection = DatabaseHelper.GetMasterConnection())
                {
                    connection.Open();
                    var param = new DynamicParameters();
                    bikeMakes = connection.Query<BikeMakeEntity>("getmanufacturermakes", param: param, commandType: CommandType.StoredProcedure);

                    if (connection.State == ConnectionState.Open)
                        connection.Close();
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Bikewale.ManufacturerCampaign.DAL.GetBikeMakes");
            }
            return bikeMakes;
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

        public ManufacturerCampaignPopup getManufacturerCampaignPopup( uint campaignId)
        {
            ManufacturerCampaignPopup objEntity = null;
            try
            {
                using (IDbConnection connection = DatabaseHelper.GetMasterConnection())
                {
                    connection.Open();
                    var param = new DynamicParameters();
                    param.Add("par_campaignId", campaignId);
                    objEntity = new ManufacturerCampaignPopup();
                    objEntity = connection.Query<ManufacturerCampaignPopup>("getmanufacturercampaignpopup", param: param, commandType: CommandType.StoredProcedure).FirstOrDefault();
                 
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
        public IEnumerable<BikeModelEntity> GetBikeModels(uint makeId)
        {
            IEnumerable<BikeModelEntity> bikeModels = null;
            try
            {
                using (IDbConnection connection = DatabaseHelper.GetMasterConnection())
                {
                    connection.Open();
                    var param = new DynamicParameters();
                    param.Add("par_makeid", makeId);
                    bikeModels = connection.Query<BikeModelEntity>("getmanufacturermodels", param: param, commandType: CommandType.StoredProcedure);

                    if (connection.State == ConnectionState.Open)
                        connection.Close();
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, string.Format("Bikewale.ManufacturerCampaign.DAL.GetBikeModels. MakeId : {0}", makeId));
            }
            return bikeModels;
        }

        public IEnumerable<StateEntity> GetStates()
        {
            IEnumerable<StateEntity> states = null;
            try
            {
                using (IDbConnection connection = DatabaseHelper.GetMasterConnection())
                {
                    connection.Open();
                    var param = new DynamicParameters();
                    states = connection.Query<StateEntity>("getstates_20062017", param: param, commandType: CommandType.StoredProcedure);

                    if (connection.State == ConnectionState.Open)
                        connection.Close();
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Bikewale.ManufacturerCampaign.DAL.GetStates");
            }
            return states;
        }
        public IEnumerable<CityEntity> GetCitiesByState(uint stateId)
        {
            IEnumerable<CityEntity> cities = null;
            try
            {
                using (IDbConnection connection = DatabaseHelper.GetMasterConnection())
                {
                    connection.Open();
                    var param = new DynamicParameters();
                    param.Add("par_stateid", stateId);
                    cities = connection.Query<CityEntity>("getcitiesbystate", param: param, commandType: CommandType.StoredProcedure);

                    if (connection.State == ConnectionState.Open)
                        connection.Close();
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, string.Format("Bikewale.ManufacturerCampaign.DAL.GetCitiesByState. StateId : {0}", stateId));
            }
            return cities;
        }

        public IEnumerable<MfgRuleEntity> GetManufacturerCampaignRules(uint campaignId)
        {
            IEnumerable<MfgRuleEntity> mfgRules = null;
            try
            {
                using (IDbConnection connection = DatabaseHelper.GetMasterConnection())
                {
                    connection.Open();
                    var param = new DynamicParameters();
                    param.Add("par_campaignid", campaignId);
                    mfgRules = connection.Query<MfgRuleEntity>("getmanufacturercampaignrules", param: param, commandType: CommandType.StoredProcedure);

                    if (connection.State == ConnectionState.Open)
                        connection.Close();
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, string.Format("Bikewale.ManufacturerCampaign.DAL.GetManufacturerCampaignRules. CampaignId : {0}", campaignId));
            }
            return mfgRules;
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
                    param.Add("par_endDate", objCampaign.EndDate??null);
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
                ErrorClass objErr = new ErrorClass(ex, "Bikewale.ManufacturerCampaign.DAL.saveManufacturerCampaign");
            }
            return campaignId;
        }
        public void saveManufacturerCampaignPopup(ManufacturerCampaignPopup objCampaign)
        {
            
            try
            {
                using (IDbConnection connection = DatabaseHelper.GetMasterConnection())
                {
                    connection.Open();
                    var param = new DynamicParameters();
                    param.Add("par_campaignid",objCampaign.CampaignId);
                    param.Add("par_PopupHeading", objCampaign.PopupHeading);
                    param.Add("par_PopupDescription", objCampaign.PopupDescription);
                    param.Add("par_PopupSuccessMessage", objCampaign.PopupSuccessMessage);
                    param.Add("par_EmailRequired", objCampaign.EmailRequired);
                    param.Add("par_PincodeRequired", objCampaign.DealerRequired);
                    param.Add("par_DealerRequired", objCampaign.PinCodeRequired);
                    
                    connection.Query<dynamic>("savemanufacturercampaignpopup", param: param, commandType: CommandType.StoredProcedure);
                    
                    if (connection.State == ConnectionState.Open)
                        connection.Close();
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Bikewale.ManufacturerCampaign.DAL.saveManufacturerCampaignPopup");
            }
            
        }


        public bool SaveManufacturerCampaignRules(uint campaignId, string modelIds, string stateIds, string cityIds, bool isAllIndia, uint userId)
        {
            bool isSuccess = false;
            try
            {
                using (IDbConnection connection = DatabaseHelper.GetMasterConnection())
                {
                    connection.Open();
                    var param = new DynamicParameters();
                    param.Add("par_campaignid",campaignId);
                    param.Add("par_modelids", modelIds);
                    param.Add("par_stateids", stateIds);
                    param.Add("par_cityids", cityIds);
                    param.Add("par_enteredby", userId);
                    param.Add("par_isAllIndia", isAllIndia);
                    connection.Query<dynamic>("savemanufacturercampaignrules_20062017", param: param, commandType: CommandType.StoredProcedure);
                    isSuccess = true;
                    if (connection.State == ConnectionState.Open)
                        connection.Close();
                }

            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, string.Format("Bikewale.ManufacturerCampaign.DAL.SaveManufacturerCampaignRules. CampaignId : {0}, ModelIds : {1},StateIds : {2}, CityIds : {3}, IsAllIndia : {4}, UserId : {5}", campaignId, modelIds, stateIds, cityIds, isAllIndia, userId));
            }
            return isSuccess;
        }

        public bool DeleteManufacturerCampaignRules(uint campaignId, uint modelId, uint stateId, uint cityId, uint userId, bool isAllIndia)
        {
            bool isSuccess = false;
            try
            {

                using (IDbConnection connection = DatabaseHelper.GetMasterConnection())
                {
                    connection.Open();
                    var param = new DynamicParameters();
                    param.Add("par_campaignid", campaignId);
                    param.Add("par_modelid", modelId);
                    param.Add("par_stateid", stateId);
                    param.Add("par_cityid", cityId);
                    param.Add("par_enteredby", userId);
                    param.Add("par_isAllIndia", isAllIndia);
                    connection.Query<dynamic>("deletemanufacturercampaignrules_20062017", param: param, commandType: CommandType.StoredProcedure);
                    isSuccess = true;
                    if (connection.State == ConnectionState.Open)
                        connection.Close();
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, string.Format("Bikewale.ManufacturerCampaign.DAL.DeleteManufacturerCampaignRules. CampaignId : {0}, ModelId : {1},StateId : {2}, CityId : {3}, UserId : {4}", campaignId, modelId, stateId, cityId, userId));
            }
            return isSuccess;
        }
    }
}
