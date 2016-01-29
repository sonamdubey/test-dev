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
        /// To get the Manufacture's active campaigns
        /// </summary>
        /// <param name="dealerId"></param>
        /// <returns></returns>
        public IEnumerable<ManufacturerCampaignEntity> GetManufacturerCampaigns(uint dealerId)
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
    }
}