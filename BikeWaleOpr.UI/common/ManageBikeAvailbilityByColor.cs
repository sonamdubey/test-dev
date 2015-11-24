using BikewaleOpr.Entities;
using BikeWaleOpr.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace BikewaleOpr.Common
{
    /// <summary>
    /// Manage Availability By Color Operations
    /// Author  :   Sushil Kumar Kanojia
    ///  Created On : 20th Nov 2015
    /// </summary>
    public class ManageBikeAvailbilityByColor
    {
        /// <summary>
        ///  To get ModelId based on selected VersionId
        /// </summary>
        /// <param name="versionId"></param>
        /// <returns>ModelId</returns>
        public int GetModelIdForVersion(int versionId)
        {
            Database db = null;
            int modelId = 0;
            try
            {
                using (SqlCommand cmd = new SqlCommand("SELECT BikeModelId AS ModelId  FROM BikeVersions WHERE ID = @versionId AND IsDeleted = 0"))
                {
                    db = new Database();
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@versionId", versionId);

                    using (SqlDataReader reader = db.SelectQry(cmd))
                    {
                        if (reader != null && reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                modelId = Convert.ToInt32(reader["ModelId"]);
                                break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "ManageBikeAvailbilityByColor.GetModelIdForVersion");
                objErr.SendMail();
            }
            finally
            {
                if (db != null)
                    db.CloseConnection();
                db = null; db = null;
            }
            return modelId;
        }


        /// <summary>
        /// Retrieves the Version colors list
        /// </summary>
        /// <param name="versionId">Model Id</param>
        /// <returns>Version Color List</returns>
        public IEnumerable<VersionColorWithAvailability> FetchVersionColorsWithAvailability(int versionId,int dealerId)
        {
            IList<VersionColorWithAvailability> versionColors = null;
            Database db = null;
            try
            {
                using (SqlCommand cmd = new SqlCommand("GetBikeAvailabilityByColor"))
                {
                    db = new Database();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@VersionId", versionId);
                    cmd.Parameters.AddWithValue("@DealerId", dealerId);

                    using (SqlDataReader reader = db.SelectQry(cmd))
                    {
                        if (reader != null && reader.HasRows)
                        {
                            versionColors = new List<VersionColorWithAvailability>();
                            while (reader.Read())
                            {
                                versionColors.Add(new VersionColorWithAvailability()
                                {
                                    ModelColorID = Convert.ToUInt32(reader["ID"]),
                                    IsActive = Convert.ToBoolean(reader["IsActive"]),
                                    ModelColorName = Convert.ToString(reader["ColorName"]),
                                    Hexcode = Convert.ToString(reader["HexCode"]),
                                    NoOfDays = Convert.ToString(reader["NoOfDays"])
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "ManageBikeAvailbilityByColor.FetchVersionColorsWithAvailability");
                objErr.SendMail();
            }
            finally
            {
                if (db != null)
                    db.CloseConnection();
                db = null; db = null;
            }
            return versionColors;
        }

        /// <summary>
        /// Saves Version Color
        /// </summary>
        /// <param name="versionColor">Version Color Entity</param>
        /// <param name="userId">User Id</param>
        /// <returns>Success/Failure</returns>
        public bool UpdateBikeAvailabilityByColor(VersionColorWithAvailability versionColor, int versionId, int userId,int dealerId)
        {
            bool isSaved = false;
            Database db = null;
            try
            {
                using (SqlCommand cmd = new SqlCommand("UpdateBikeAvailabilityByColor"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@NoOfDays", versionColor.NoOfDays);
                    cmd.Parameters.AddWithValue("@VersionId", versionId);
                    cmd.Parameters.AddWithValue("@ColorId", Convert.ToInt32(versionColor.ModelColorID));
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    cmd.Parameters.AddWithValue("@DealerId", dealerId);

                    db = new Database();
                    isSaved = db.UpdateQry(cmd);
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "ManageBikeAvailbilityByColor.UpdateBikeAvailabilityByColor");
                objErr.SendMail();
            }
            finally
            {
                if (db != null)
                    db.CloseConnection();
                db = null; db = null;
            }
            return isSaved;
        }
    }


}