﻿using BikewaleOpr.Entities;
using BikeWaleOpr.Common;
using MySql.CoreDAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace BikewaleOpr.Common
{
    /// <summary>
    /// Created by  :   Sumit Kate on 29 Jan 2016
    /// Description :   Manage Manufacturer Campaign DAL
    /// </summary>
    public class ManageManufacturerCampaign
    {
        /// <summary>
        /// Created by  :   Sumit Kate on 29 Jan 2016
        /// To get the Manufacture's active campaigns
        /// </summary>
        /// <param name="dealerId"></param>
        /// <returns></returns>
        public IEnumerable<ManufacturerCampaignEntity> GetManufacturerCampaigns(int dealerId)
        {
            IList<ManufacturerCampaignEntity> lstManufacturerCampaign = null;

            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("getmanufacturercampaigns"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_dealerid", DbType.Int32, dealerId));

                    using (IDataReader reader = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (reader != null)
                        {
                            lstManufacturerCampaign = new List<ManufacturerCampaignEntity>();
                            while (reader.Read())
                            {
                                lstManufacturerCampaign.Add(
                                    new ManufacturerCampaignEntity()
                                    {
                                        CampaignId = Convert.ToUInt32(reader["Id"]),
                                        DealerId = Convert.ToUInt32(reader["DealerId"]),
                                        Description = Convert.ToString(reader["Description"]),
                                        EntryDate = (!Convert.IsDBNull(reader["EntryDate"])) ? Convert.ToDateTime(reader["EntryDate"]).ToString("d/M/yyyy") : "",
                                        IsActive = Convert.ToBoolean(reader["IsActive"]),
                                        ModelId = Convert.ToUInt32(reader["ModelId"]),
                                        ModelName = Convert.ToString(reader["ModelName"]),
                                        MakeName = Convert.ToString(reader["MakeName"])
                                    }
                                    );
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "ManageManufacturerCampaign.GetManufacturerCampaigns");
                
            }

            return lstManufacturerCampaign;
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 01 Feb 2016
        /// Save the manufacturer Campaigns
        /// </summary>
        /// <param name="dealerId">Manufacturer Id(Dealer Id)</param>
        /// <param name="modelIds">Model Ids (comma seperated value)</param>
        /// <param name="description">Campaign Description</param>
        /// <returns></returns>
        public bool SaveManufacturerCampaign(int dealerId, string modelIds, string description)
        {
            bool success = false;

            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("savemanufacturercampaign"))
                {

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_dealerid", DbType.Int32, dealerId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_modelids", DbType.String, 150, modelIds));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_description", DbType.String, 100, description));
                    MySqlDatabase.ExecuteNonQuery(cmd, ConnectionType.MasterDatabase);
                    success = true;
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "ManageManufacturerCampaign.SaveManufacturerCampaign");
                
            }

            return success;
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 01 Feb 2016
        /// Sets the manufacturer Campaigns as inactive
        /// </summary>
        /// <param name="dealerId">Manufacturer Id(Dealer Id)</param>
        /// <param name="campaignIds">campaign Ids (comma seperated value)</param>
        /// <returns></returns>
        public bool SetManufacturerCampaignInActive(int dealerId, string campaignIds)
        {
            bool success = false;

            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("setmfgcampaigninactive"))
                {

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_dealerid", DbType.Int32, dealerId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_campaignids", DbType.String, 150, campaignIds));

                    MySqlDatabase.ExecuteNonQuery(cmd, ConnectionType.MasterDatabase);
                    success = true;
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "ManageManufacturerCampaign.SetManufacturerCampaignInActive");
                
            }

            return success;
        }

        /// <summary>
        /// Created by  :   Sumit Kateon 01 Feb 2016
        /// Returns the Manufacturers list.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ManufacturerEntity> GetDealerAsManuFacturer()
        {
            IList<ManufacturerEntity> manufacturers = null;

            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("getdealerasmanufacturer"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    using (IDataReader reader = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (reader != null)
                        {
                            manufacturers = new List<ManufacturerEntity>();
                            while (reader.Read())
                            {
                                manufacturers.Add(
                                    new ManufacturerEntity()
                                    {
                                        Id = Convert.ToInt32(reader["Id"]),
                                        Name = Convert.ToString(reader["Name"]),
                                        Organization = Convert.ToString(reader["Organization"])
                                    }
                                    );
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "ManageManufacturerCampaign.GetDealerAsManuFacturer");
                
            }

            return manufacturers;
        }
    }
}