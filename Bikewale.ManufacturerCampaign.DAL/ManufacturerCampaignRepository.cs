
using Dapper;
using Bikewale.ManufacturerCampaign.Entities;
using Bikewale.ManufacturerCampaign.Interface;
using Bikewale.Notifications;
using Bikewale.Utility;
using MySql.CoreDAL;
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

using System.Collections.ObjectModel;
using System.Linq;
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
    public class ManufacturerCampaignRepository : IManufacturerCampaign
    {
        public IEnumerable<BikeMakeEntity> GetBikeMakes()
        {
            ICollection<BikeMakeEntity> bikeMakes = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("getmanufacturermakes"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            bikeMakes = new Collection<BikeMakeEntity>();

                            while (dr.Read())
                            {
                                bikeMakes.Add(new BikeMakeEntity()
                                {
                                    MakeId = SqlReaderConvertor.ToUInt32(dr["MakeId"]),
                                    MakeName = Convert.ToString(dr["MakeName"])
                                });
                            }
                            dr.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Bikewale.ManufacturerCampaign.DAL.GetBikeMakes");
            }
            return bikeMakes;
        }

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

        public IEnumerable<BikeModelEntity> GetBikeModels(uint makeId)
        {
            ICollection<BikeModelEntity> bikeModels = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("getmanufacturermodels"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_makeid", DbType.Int32, makeId));

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            bikeModels = new Collection<BikeModelEntity>();
                            while (dr.Read())
                            {
                                bikeModels.Add(new BikeModelEntity()
                                {
                                    ModelId = SqlReaderConvertor.ToUInt32(dr["ModelId"]),
                                    ModelName = Convert.ToString(dr["ModelName"])
                                });
                            }
                            dr.Close();
                        }
                    }
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
            ICollection<StateEntity> states = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("getstates_20062017"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            states = new Collection<StateEntity>();

                            while (dr.Read())
                            {
                                states.Add(new StateEntity()
                                {
                                    StateId = SqlReaderConvertor.ToUInt32(dr["StateId"]),
                                    StateName = Convert.ToString(dr["StateName"])
                                });
                            }
                            dr.Close();
                        }
                    }
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
            ICollection<CityEntity> cities = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("getcitiesbystate"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_stateid", DbType.Int32, stateId));

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            cities = new Collection<CityEntity>();
                            while (dr.Read())
                            {
                                cities.Add(new CityEntity()
                                {
                                    CityId = SqlReaderConvertor.ToUInt32(dr["CityId"]),
                                    CityName = Convert.ToString(dr["CityName"])
                                });
                            }
                            dr.Close();
                        }
                    }
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
            ICollection<MfgRuleEntity> mfgRules = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("getmanufacturercampaignrules"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_campaignid", DbType.Int32, campaignId));

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            mfgRules = new Collection<MfgRuleEntity>();
                            while (dr.Read())
                            {
                                mfgRules.Add(
                                    new MfgRuleEntity
                                    {
                                        MakeId = SqlReaderConvertor.ToUInt32(dr["MakeId"]),
                                        MakeName = Convert.ToString(dr["MakeName"]),
                                        ModelId = SqlReaderConvertor.ToUInt32(dr["ModelId"]),
                                        ModelName = Convert.ToString(dr["ModelName"]),
                                        StateId = SqlReaderConvertor.ToUInt32(dr["StateId"]),
                                        StateName = Convert.ToString(dr["StateName"]),
                                        CityId = SqlReaderConvertor.ToUInt32(dr["CityId"]),
                                        CityName = Convert.ToString(dr["CityName"])
                                    }
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
                                    );
                            }
                            dr.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, string.Format("Bikewale.ManufacturerCampaign.DAL.GetManufacturerCampaignRules. CampaignId : {0}", campaignId));
            }
            return mfgRules;
        }

        public bool SaveManufacturerCampaignRules(uint campaignId, string modelIds, string stateIds, string cityIds, bool isAllIndia, uint userId)
        {
            bool isSuccess = false;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("savemanufacturercampaignrules_20062017"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_campaignid", DbType.Int32, campaignId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_modelids", DbType.String, modelIds));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_stateids", DbType.String, stateIds));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_cityids", DbType.String, cityIds));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_enteredby", DbType.Int32, userId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_isAllIndia", DbType.Boolean, isAllIndia));
                    MySqlDatabase.ExecuteNonQuery(cmd, ConnectionType.ReadOnly);
                    isSuccess = true;
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
                using (DbCommand cmd = DbFactory.GetDBCommand("deletemanufacturercampaignrules_20062017"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_campaignid", DbType.Int32, campaignId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_modelid", DbType.Int32, modelId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_stateid", DbType.Int32, stateId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_cityid", DbType.Int32, cityId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_enteredby", DbType.Int32, userId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_isAllIndia", DbType.Boolean, isAllIndia));
                    MySqlDatabase.ExecuteNonQuery(cmd, ConnectionType.ReadOnly);
                    isSuccess = true;
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