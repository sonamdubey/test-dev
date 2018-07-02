using System;
using System.Collections.Generic;
using BikewaleOpr.Entity.BikePricing;
using BikewaleOpr.Interface.BikePricing;
using System.Data;
using System.Data.Common;
using MySql.CoreDAL;
using Bikewale.Utility;
using Bikewale.Notifications;
using System.Collections.ObjectModel;

namespace BikewaleOpr.DALs.BikePricing
{
    /// <summary>
    /// Created By : Prabhu Puredla on 18 May 2018
    /// Description : Repository for Bulk Price Upload
    /// </summary>
    public class BulkPriceRepository : IBulkPriceRepository
    {        
        /// <summary>
        /// Created By : Prabhu Puredla on 18 May 2018
        /// Description : Get Mapped Bikes based on a Make
        /// </summary>
        /// <param name="makeId"></param>
        /// <returns></returns>
        public IEnumerable<MappedBikesEntity> GetMappedBikesData(uint makeId)
        {
            ICollection<MappedBikesEntity> mappedBikes = null;

            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("getmappedbikesdata"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_makeId", DbType.Int32, 20, makeId));

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            mappedBikes = new Collection<MappedBikesEntity>();

                            while (dr.Read())
                            {
                                MappedBikesEntity mappedBikesEntity = new MappedBikesEntity
                                {
                                    Id = SqlReaderConvertor.ToUInt32(dr["Id"]),
                                    OemBikeName = Convert.ToString(dr["OEMBikeName"]),
                                    MakeId = SqlReaderConvertor.ToUInt32(dr["MakeId"]),
                                    MakeName = Convert.ToString(dr["MakeName"]),
                                    ModelName = Convert.ToString(dr["ModelName"]),
                                    VersionName = Convert.ToString(dr["VersionName"]),
                                    VersionId = SqlReaderConvertor.ToUInt32(dr["Id"]),
                                    LastUpdatedBy = Convert.ToString(dr["LastUpdatedBy"]),
                                    LastUpdatedDate = Convert.ToString(dr["LastUpdatedDate"])
                                };
                                mappedBikes.Add(mappedBikesEntity);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "BikewaleOpr.DALs.BikePricing.GetMappedBikesData");
            }
            return mappedBikes;
        }

        /// <summary>
        /// Created By : Prabhu Puredla on 18 May 2018
        /// Description : Delete Mapping of a Bike in the database
        /// </summary>
        /// <param name="mappingId"></param>
        /// <param name="updatedBy"></param>
        public void DeleteMappedBike(uint mappingId, uint updatedBy)
        {
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("deletemappedbike"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_id", DbType.Int32, mappingId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_updatedby", DbType.Int32, updatedBy));

                    MySqlDatabase.ExecuteNonQuery(cmd, ConnectionType.MasterDatabase);
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "BikewaleOpr.DALs.BikePricing.DeleteMappedBike");
            }
        }

        /// <summary>
        /// Created By : Prabhu Puredla on 22 May 2018
        /// Description : Get Mapped Cities for a specific State
        /// </summary>
        /// <param name="stateId"></param>
        /// <returns></returns>
        public IEnumerable<MappedCitiesEntity> GetMappedCitiesData(uint stateId)
        {
            ICollection<MappedCitiesEntity> mappedCities = null;

            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("getmappedcitiesdata"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_stateId", DbType.Int32, 20, stateId));

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            mappedCities = new Collection<MappedCitiesEntity>();

                            while (dr.Read())
                            {
                                MappedCitiesEntity mappedCitiesEntity = new MappedCitiesEntity
                                {
                                    Id = SqlReaderConvertor.ToUInt32(dr["Id"]),
                                    OemCityName = Convert.ToString(dr["OEMCityName"]),
                                    StateName = Convert.ToString(dr["StateName"]),
                                    CityName = Convert.ToString(dr["CityName"]),
                                    CityId = SqlReaderConvertor.ToUInt32(dr["CityId"]),
                                    LastUpdatedBy = Convert.ToString(dr["LastUpdatedBy"]),
                                    LastUpdatedDate = Convert.ToString(dr["LastUpdatedDate"])
                                };
                                mappedCities.Add(mappedCitiesEntity);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "BikewaleOpr.DALs.BikePricing.GetMappedBikesData");
            }
            return mappedCities;
        }

        /// <summary>
        /// Created By : Prabhu Puredla on 18 May 2018
        /// Description : Get all Mapped bikes for all Makes to compare with OemBikeNames
        /// </summary>
        /// <param name="makeId"></param>
        /// <returns></returns>
        public IEnumerable<MappedBikesEntity> GetAllMappedBikesData()
        {
            ICollection<MappedBikesEntity> mappedBikes = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("getallmappedbikesdata"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            mappedBikes = new Collection<MappedBikesEntity>();
                            MappedBikesEntity mappedBikesEntity = null;
                            while (dr.Read())
                            {
                                mappedBikesEntity = new MappedBikesEntity
                                {
                                    Id = SqlReaderConvertor.ToUInt32(dr["VersionId"]),
                                    OemBikeName = Convert.ToString(dr["OEMBikeName"]),
                                    ModelId = SqlReaderConvertor.ToUInt32(dr["ModelId"])
                                };
                                mappedBikes.Add(mappedBikesEntity);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "BikewaleOpr.DALs.BikePricing.GetAllMappedBikesData");
            }
            return mappedBikes;
        }

        /// <summary>
        /// Created By : Prabhu Puredla on 23 May 2018
        /// Description : Delete a Mapping of a city in the database
        /// </summary>
        /// <param name="mappingId"></param>
        /// <param name="updatedBy"></param>
        public void DeleteMappedCity(uint mappingId, uint updatedBy)
        {
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("deletemappedcity"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_id", DbType.Int32, mappingId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_updatedby", DbType.Int32, updatedBy));

                    MySqlDatabase.ExecuteNonQuery(cmd, ConnectionType.MasterDatabase);
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "BikewaleOpr.DALs.BikePricing.DeleteMappedCity");
            }
        }

        /// <summary>
        /// Created By : Prabhu Puredla on 5 june 2018
        /// Description : Get all Mapped Cities for all states to compare with OemCityNames
        /// </summary>
        /// <returns></returns>
        public IEnumerable<MappedCitiesEntity> GetAllMappedCitiesData()
        {
            ICollection<MappedCitiesEntity> mappedCities = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("getallmappedcitiesdata"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            mappedCities = new Collection<MappedCitiesEntity>();

                            while (dr.Read())
                            {
                                MappedCitiesEntity mappedCitiesEntity = new MappedCitiesEntity
                                {
                                    Id = SqlReaderConvertor.ToUInt32(dr["CityId"]),
                                    OemCityName = Convert.ToString(dr["OEMCityName"]),
                                    StateId = SqlReaderConvertor.ToUInt32(dr["StateId"])
                                };
                                mappedCities.Add(mappedCitiesEntity);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "BikewaleOpr.DALs.BikePricing.GetAllMappedCitiesData");
            }
            return mappedCities;
        }

        /// <summary>
        /// Created By : Prabhu Puredla on 5 june 2018
        /// Description : Map all Unmapped bike in the database
        /// </summary>
        /// <param name="oemBikeName"></param>
        /// <param name="versionId"></param>
        /// <param name="updatedBy"></param>
        /// <returns></returns>
        public bool MapTheUnmappedBike(string oemBikeName, uint versionId, uint updatedBy)
        {
            bool isInserted = false;

            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("insertbikemapping"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_oembikename", DbType.String, oemBikeName));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_versionid", DbType.Int32, versionId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_lastupdatedby", DbType.Int32, updatedBy));

                    isInserted = MySqlDatabase.ExecuteNonQuery(cmd, ConnectionType.MasterDatabase) > 0;
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "BikewaleOpr.DALs.BikePricing.MapTheUnmappedBikes");
            }
            return isInserted;
        }

        /// <summary>
        /// Created By : Prabhu Puredla on 5 june 2018
        /// Description : Map the Unmapped City in the database 
        /// </summary>
        /// <param name="cityId"></param>
        /// <param name="oemCityName"></param>
        /// <param name="updatedBy"></param>
        /// <returns></returns>
        public bool MapTheUnmappedCity(uint cityId, string oemCityName, uint updatedBy)
        {
            bool isInserted = false;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("insertcitymapping"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_oemcityname", DbType.String, oemCityName));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_cityid", DbType.Int32, cityId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_lastupdatedby", DbType.Int32, updatedBy));

                    isInserted = MySqlDatabase.ExecuteNonQuery(cmd, ConnectionType.MasterDatabase) > 0;
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "BikewaleOpr.DALs.BikePricing.MapTheUnmappedCities");
            }
            return isInserted;
        }

        /// <summary>
        /// Created By : Prabhu Puredla on 5 june 2018
        /// Description : Update Bulk Prices of a bike in different cities at a time 
        /// </summary>
        /// <param name="pricesToUpdate">Row and column seperated string</param>
        /// <param name="bikeId"></param>
        /// <param name="updatedBy"></param>
        public bool SavePrices(string prices, uint bikeId, uint updatedBy)
        {
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("insertbulkprices_06012018"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_prices", DbType.String, prices));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_bikeversionid", DbType.Int32, bikeId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_updatedby", DbType.Int32, updatedBy));

                    MySqlDatabase.ExecuteNonQuery(cmd, ConnectionType.MasterDatabase);
                    return true;
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "BikewaleOpr.DALs.BikePricing.SavePrices");
                return false;
            }
        }      
    }

}
