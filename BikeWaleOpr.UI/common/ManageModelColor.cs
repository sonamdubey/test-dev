using BikewaleOpr.Entities;
using BikeWaleOpr.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using BikeWaleOpr.Entities;

namespace BikewaleOpr.Common
{
    /// <summary>
    /// Model and Version Color Operations
    /// Author  :   Sumit Kate created on 09 Nov 2015
    /// </summary>
    public class ManageModelColor
    {
        /// <summary>
        /// Retrieves the Model Colors list
        /// </summary>
        /// <param name="modelId">Bike Model Id</param>
        /// <returns>Model Color List</returns>
        public IEnumerable<ModelColorBase> FetchModelColors(int modelId)
        {
            List<ModelColorBase> modelColors = null;
            Database db = null;
            IList<ColorCodeBase> colorCodes = null;
            try
            {
                using (SqlCommand cmd = new SqlCommand("GetBikeModelColor"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@modelId", modelId);

                    db = new Database();
                    using (SqlDataReader reader = db.SelectQry(cmd))
                    {
                        if (reader != null && reader.HasRows)
                        {
                            modelColors = new List<ModelColorBase>();
                            while (reader.Read())
                            {
                                modelColors.Add(new ModelColorBase()
                                {
                                    Id = Convert.ToUInt32(reader["modelColorId"]),
                                    Name = Convert.ToString(reader["ColorName"]),
                                });
                            }
                            if (reader.NextResult())
                            {
                                colorCodes = new List<ColorCodeBase>();
                                while (reader.Read())
                                {
                                    colorCodes.Add(
                                        new ColorCodeBase()
                                        {
                                            HexCode = Convert.ToString(reader["HexCode"]),
                                            Id = Convert.ToUInt32(reader["ColorId"]),
                                            ModelColorId = Convert.ToUInt32(reader["modelColorId"]),
                                            IsActive    = Convert.ToBoolean(reader["IsActive"])
                                        });
                                }
                                modelColors.ForEach(
                                    modelColor => modelColor.ColorCodes =
                                        from colorCode in colorCodes
                                        where colorCode.ModelColorId == modelColor.Id
                                        select colorCode
                                    );
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "ManageModelColor.FetchModelColors");
                objErr.SendMail();
            }
            finally
            {
                db = null;
            }
            return modelColors;
        }
        
        /// <summary>
        /// Retrieves the Version colors list
        /// </summary>
        /// <param name="versionId">Model Id</param>
        /// <returns>Version Color List</returns>
        public IEnumerable<VersionColorBase> FetchVersionColors(int versionId)
        {
            IList<VersionColorBase> versionColors = null;
            Database db = null;
            try
            {
                using (SqlCommand cmd = new SqlCommand("GetBikeVersionColor"))
                {
                    db = new Database();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@versionId", versionId);

                    using (SqlDataReader reader = db.SelectQry(cmd))
                    {
                        if (reader != null && reader.HasRows)
                        {
                            versionColors = new List<VersionColorBase>();
                            while (reader.Read())
                            {
                                versionColors.Add(new VersionColorBase()
                                {
                                    ModelColorID = Convert.ToUInt32(reader["ID"]),
                                    IsActive = Convert.ToBoolean(reader["IsActive"]),
                                    ModelColorName = Convert.ToString(reader["ColorName"])
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "ManageModelColor.FetchVersionColors");
                objErr.SendMail();
            }
            finally
            {
                db = null;
            }
            return versionColors;
        }

        /// <summary>
        /// Fetches Bike Versions by Bike Model Id
        /// </summary>
        /// <param name="modelId">Bike Model Id</param>
        /// <param name="requestType">
        /// PriceQuote = 1,
        /// New = 2,
        /// Used = 3,
        /// Upcoming = 4,
        /// RoadTest = 5,
        /// ComparisonTest = 6,
        /// All = 7 (Default),
        /// UserReviews = 8,
        /// NewBikeSpecs = 9,
        /// UsedBikeSpecs = 10,
        /// NewBikeSpecification = 11</param>
        /// <returns></returns>
        public IEnumerable<VersionEntityBase> FetchBikeVersion(int modelId,int requestType = 7)
        {
            IList<VersionEntityBase> bikeVersions = null;
            Database db = null;
            try
            {
                using (SqlCommand cmd = new SqlCommand("GetBikeVersions_New"))
                {
                    db = new Database();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@RequestType", requestType);
                    cmd.Parameters.AddWithValue("@ModelId", modelId);

                    using (SqlDataReader reader = db.SelectQry(cmd))
                    {
                        if (reader != null && reader.HasRows)
                        {
                            bikeVersions = new List<VersionEntityBase>();
                            while (reader.Read())
                            {
                                bikeVersions.Add(new VersionEntityBase()
                                {
                                    VersionId = Convert.ToUInt32(reader["VersionId"]),
                                    VersionName = Convert.ToString(reader["VersionName"])
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "ManageModelColor.FetchBikeVersion");
                objErr.SendMail();
            }
            finally
            {
                db = null;
            }
            return bikeVersions;
        }

        /// <summary>
        /// Saves the model color with HexCode
        /// </summary>
        /// <param name="modelId">Model id</param>
        /// <param name="colorName">Color Name</param>
        /// <param name="userId">User Id</param>
        /// <param name="hexCodes">Hex Codes (E.g. fff000,ababab)</param>
        /// <returns></returns>
        public bool SaveModelColor(int modelId, string colorName, string userId, string hexCodes)
        {
            bool isSaved = false;
            Database db = null;
            try
            {
                using (SqlCommand cmd = new SqlCommand("SaveModelColor"))
                {
                    db = new Database();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@modelId", modelId);
                    cmd.Parameters.AddWithValue("@colorName", colorName);
                    cmd.Parameters.AddWithValue("@userId", userId);
                    cmd.Parameters.AddWithValue("@hexCodes", hexCodes);

                    isSaved = db.InsertQry(cmd);
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "ManageModelColor.SaveModelColor");
                objErr.SendMail();
            }
            finally
            {
                db = null;
            }
            return isSaved;
        }

        /// <summary>
        /// Saves Version Color
        /// </summary>
        /// <param name="versionColor">Version Color Entity</param>
        /// <param name="userId">User Id</param>
        /// <returns>Success/Failure</returns>
        public bool SaveVersionColor(VersionColorBase versionColor, int versionId ,string userId)
        {
            bool isSaved = false;
            Database db = null;
            try
            {
                using (SqlCommand cmd = new SqlCommand("SaveVersionColor"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@isActive", versionColor.IsActive);
                    cmd.Parameters.AddWithValue("@versionId", versionId);
                    cmd.Parameters.AddWithValue("@modelColorId", Convert.ToInt32(versionColor.ModelColorID));
                    cmd.Parameters.AddWithValue("@userId", userId);

                    db = new Database();
                    isSaved = db.InsertQry(cmd);
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "ManageModelColor.SaveVersionColor");
                objErr.SendMail();
            }
            finally
            {
                db = null;
            }
            return isSaved;
        }

        /// <summary>
        /// Updates the Color hex code
        /// </summary>
        /// <param name="colorId"></param>
        /// <param name="hexCode"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public bool UpdateColorCode(int colorId, string hexCode, string userId,bool isActive)
        {
            bool isUpdated = false;
            Database db = null;
            try
            {
                using (SqlCommand cmd = new SqlCommand("UpdateModelColor"))
                {
                    db = new Database();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@colorId", colorId);
                    cmd.Parameters.AddWithValue("@hexCode", hexCode);
                    cmd.Parameters.AddWithValue("@userId", userId);
                    cmd.Parameters.AddWithValue("@isActive", isActive);
                    isUpdated = db.UpdateQry(cmd);
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "ManageModelColor.UpdateColorCode");
                objErr.SendMail();
            }
            finally
            {
                db = null;
            }
            return isUpdated;
        }

        /// <summary>
        /// Inserts new HexCode to model color
        /// </summary>
        /// <param name="modelColorId">Model Color Id</param>
        /// <param name="hexCode">Color Hex Color</param>
        /// <param name="userId">User Id</param>
        /// <param name="isActive">Active/Inactive(true by default)</param>
        /// <returns></returns>
        public bool AddColorCode(int modelColorId,string hexCode,string userId,bool isActive = true)
        {
            bool isSaved = false;
            Database db = null;
            try
            {
                using (SqlCommand cmd = new SqlCommand("AddModelColorHexCode"))
                {
                    db = new Database();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@modelColorId", modelColorId);
                    cmd.Parameters.AddWithValue("@hexCode", hexCode);
                    cmd.Parameters.AddWithValue("@userId", userId);
                    cmd.Parameters.AddWithValue("@isActive", isActive);
                    isSaved = db.UpdateQry(cmd);
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "ManageModelColor.AddColorCode");
                objErr.SendMail();
            }
            finally
            {
                db = null;
            }
            return isSaved;
        }
    }
}