using BikewaleOpr.Entities;
using BikeWaleOpr.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

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
            Database db = null;
            try
            {
                using (SqlCommand cmd = new SqlCommand("GetManufacturerCampaigns"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@DealerId", dealerId);
                    db = new Database();
                    using (SqlDataReader reader = db.SelectQry(cmd))
                    {
                        if (reader != null && reader.HasRows)
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
                                        EntryDate = Convert.ToDateTime(reader["EntryDate"]),
                                        IsActive = Convert.ToBoolean(reader["IsActive"]),
                                        ModelId = Convert.ToUInt32(reader["ModelId"])
                                    }
                                    );
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "ManageManufacturerCampaign.GetManufacturerCampaigns");
                objErr.SendMail();
            }
            finally
            {
                if (db != null)
                    db.CloseConnection();
                db = null;
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
        public bool SaveManufacturerCampaign(int dealerId, string modelIds,string description)
        {
            bool success = false;
            Database db = null;
            try
            {
                using (SqlCommand cmd = new SqlCommand("SaveManufacturerCampaign"))
                {
                    db = new Database();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@DealerId", dealerId);
                    cmd.Parameters.AddWithValue("@ModelIds", modelIds);
                    cmd.Parameters.AddWithValue("@Description", description);
                    success = db.InsertQry(cmd);
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "ManageManufacturerCampaign.SaveManufacturerCampaign");
                objErr.SendMail();
            }
            finally
            {
                if (db != null)
                    db.CloseConnection();
                db = null;
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
            Database db = null;
            try
            {
                using (SqlCommand cmd = new SqlCommand("SetMfgCampaignInactive"))
                {
                    db = new Database();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@DealerId", dealerId);
                    cmd.Parameters.AddWithValue("@CampaignIds", campaignIds);
                    success = db.InsertQry(cmd);
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "ManageManufacturerCampaign.SetManufacturerCampaignInActive");
                objErr.SendMail();
            }
            finally
            {
                if (db != null)
                    db.CloseConnection();
                db = null;
            }
            return success;
        }
    }
}