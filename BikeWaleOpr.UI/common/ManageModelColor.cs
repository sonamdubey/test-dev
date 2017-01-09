using Bikewale.Utility;
using BikewaleOpr.Entities;
using BikeWaleOpr.Common;
using BikeWaleOpr.Entities;
using MySql.CoreDAL;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Data.Common;
using System.Linq;

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

            IList<ColorCodeBase> colorCodes = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("getbikemodelcolor"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_modelid", DbType.Int32, modelId));


                    using (IDataReader reader = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (reader != null)
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
                                            IsActive = Convert.ToBoolean(reader["IsActive"])
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

            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("getbikeversioncolor"))
                {

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_versionid", DbType.Int32, versionId));

                    using (IDataReader reader = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (reader != null)
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
        public IEnumerable<VersionEntityBase> FetchBikeVersion(int modelId)
        {
            IList<VersionEntityBase> bikeVersions = null;

            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("SELECT ID AS VersionId,Name AS VersionName from bikeversions where bikemodelid = @modelid and isdeleted = 0"))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add(DbFactory.GetDbParam("@modelid", DbType.Int32, modelId));

                    using (IDataReader reader = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (reader != null)
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

            return bikeVersions;
        }

        /// <summary>
        /// Saves the model color with HexCode
        /// Modified By : Sushil Kumar on 17th Nov 2016
        /// Description : Added logic to push bike color to carwale db through datasync
        /// </summary>
        /// <param name="modelId">Model id</param>
        /// <param name="colorName">Color Name</param>
        /// <param name="userId">User Id</param>
        /// <param name="hexCodes">Hex Codes (E.g. fff000,ababab)</param>
        /// <returns></returns>
        public bool SaveModelColor(int modelId, string colorName, int userId, string hexCodes)
        {
            bool isSaved = false;

            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("savemodelcolor"))
                {

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_modelid", DbType.Int32, modelId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_userid", DbType.Int32, userId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_colorname", DbType.String, 100, colorName));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_hexcodes", DbType.String, 500, hexCodes));

                    MySqlDatabase.ExecuteNonQuery(cmd, ConnectionType.MasterDatabase);

                    isSaved = true;

                    if (modelId > 0)
                    {
                        NameValueCollection nvc = new NameValueCollection();
                        nvc.Add("modelId", modelId.ToString());
                        nvc.Add("colorName", colorName);
                        nvc.Add("userId", userId.ToString());
                        nvc.Add("hexCodes", hexCodes);
                        SyncBWData.PushToQueue("BW_SaveBikeModelColor", DataBaseName.CW, nvc);
                    }


                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "ManageModelColor.SaveModelColor");
                objErr.SendMail();
            }

            return isSaved;
        }

        /// <summary>
        /// Saves Version Color
        /// Modified By : Sushil Kumar on 17th Nov 2016
        /// Description : Added logic to push bike color to carwale db through datasync
        /// </summary>
        /// <param name="versionColor">Version Color Entity</param>
        /// <param name="userId">User Id</param>
        /// <returns>Success/Failure</returns>
        public bool SaveVersionColor(VersionColorBase versionColor, int versionId, int userId)
        {
            bool isSaved = false;

            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("saveversioncolor"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_modelcolorid", DbType.Int32, Convert.ToInt32(versionColor.ModelColorID)));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_versionid", DbType.Int32, versionId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_userid", DbType.String, 100, userId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_isactive", DbType.Boolean, versionColor.IsActive));

                    MySqlDatabase.ExecuteNonQuery(cmd, ConnectionType.MasterDatabase);

                    isSaved = true;

                    if (versionColor.ModelColorID > 0 && versionId > 0)
                    {
                        NameValueCollection nvc = new NameValueCollection();
                        nvc.Add("modelColorId", versionColor.ModelColorID.ToString());
                        nvc.Add("versionId", versionId.ToString());
                        nvc.Add("userId", userId.ToString());
                        nvc.Add("isActive", versionColor.IsActive ? "1" : "0");
                        SyncBWData.PushToQueue("BW_SaveBikeVersionColor", DataBaseName.CW, nvc);
                    }

                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "ManageModelColor.SaveVersionColor");
                objErr.SendMail();
            }

            return isSaved;
        }

        /// <summary>
        /// Updates the Color hex code
        /// Modified By : Sushil Kumar on 17th Nov 2016
        /// Description : Added logic to push bike color to carwale db through datasync
        /// </summary>
        /// <param name="colorId"></param>
        /// <param name="hexCode"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public bool UpdateColorCode(int colorId, string hexCode, int userId, bool isActive)
        {
            bool isUpdated = false;

            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("updatemodelcolor"))
                {

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_colorid", DbType.Int32, colorId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_hexcode", DbType.String, 6, hexCode));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_userid", DbType.String, 100, userId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_isactive", DbType.Boolean, isActive));

                    MySqlDatabase.ExecuteNonQuery(cmd, ConnectionType.MasterDatabase);

                    isUpdated = true;

                    if (colorId > 0)
                    {
                        NameValueCollection nvc = new NameValueCollection();
                        nvc.Add("colorId", colorId.ToString());
                        nvc.Add("hexCode", hexCode);
                        nvc.Add("userId", userId.ToString());
                        nvc.Add("isActive", isActive ? "1" : "0");
                        SyncBWData.PushToQueue("BW_UpdateBikeModelColor", DataBaseName.CW, nvc);
                    }


                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "ManageModelColor.UpdateColorCode");
                objErr.SendMail();
            }

            return isUpdated;
        }

        /// <summary>
        /// Inserts new HexCode to model color
        /// Modified By : Sushil Kumar on 17th Nov 2016
        /// Description : Added logic to push bike color to carwale db through datasync
        /// </summary>
        /// <param name="modelColorId">Model Color Id</param>
        /// <param name="hexCode">Color Hex Color</param>
        /// <param name="userId">User Id</param>
        /// <param name="isActive">Active/Inactive(true by default)</param>
        /// <returns></returns>
        public bool AddColorCode(int modelColorId, string hexCode, int userId, bool isActive = true)
        {
            bool isSaved = false;

            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("addmodelcolorhexcode"))
                {

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_modelcolorid", DbType.Int32, modelColorId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_hexcode", DbType.String, 6, hexCode));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_userid", DbType.String, 100, userId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_isactive", DbType.Boolean, isActive));

                    MySqlDatabase.ExecuteNonQuery(cmd, ConnectionType.MasterDatabase);

                    isSaved = true;

                    if (modelColorId > 0)
                    {
                        NameValueCollection nvc = new NameValueCollection();
                        nvc.Add("modelColorId", modelColorId.ToString());
                        nvc.Add("hexCode", hexCode);
                        nvc.Add("userId", userId.ToString());
                        nvc.Add("isActive", isActive ? "1" : "0");
                        SyncBWData.PushToQueue("BW_AddBikeModelColorHexCode", DataBaseName.CW, nvc);
                    }


                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "ManageModelColor.AddColorCode");
                objErr.SendMail();
            }

            return isSaved;
        }

        /// <summary>
        /// Deletes the Model Color
        /// Modified By : Sushil Kumar on 17th Nov 2016
        /// Description : Added logic to push bike color to carwale db through datasync
        /// </summary>
        /// <param name="modelColorId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public bool DeleteModelColor(int modelColorId, int userId)
        {
            bool isDeleted = false;

            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("deletemodelcolor"))
                {

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_modelcolorid", DbType.Int32, modelColorId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_userid", DbType.Int32, userId));

                    MySqlDatabase.ExecuteNonQuery(cmd, ConnectionType.MasterDatabase);

                    isDeleted = true;

                    if (modelColorId > 0)
                    {
                        NameValueCollection nvc = new NameValueCollection();
                        nvc.Add("modelColorId", modelColorId.ToString());
                        nvc.Add("userId", userId.ToString());
                        SyncBWData.PushToQueue("BW_DeleteBikeModelColor", DataBaseName.CW, nvc);
                    }


                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "ManageModelColor.DeleteModelColor");
                objErr.SendMail();
            }

            return isDeleted;
        }


        /// <summary>
        /// Retrieves the Model Colors and Images
        /// Created by: Sangram Nandkhile on 9 Jan 2017
        /// </summary>
        /// <param name="modelId">Bike Model Id</param>
        /// <returns>Model colorwise Image List</returns>
        public IEnumerable<ModelColorImage> FetchModelImagesByColors(int modelId)
        {
            List<ModelColorImage> modelColors = null;

            IList<ColorCodeBase> colorCodes = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("getbikemodelcolor_09012017"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_modelid", DbType.Int32, modelId));


                    using (IDataReader reader = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (reader != null)
                        {
                            modelColors = new List<ModelColorImage>();
                            while (reader.Read())
                            {
                                modelColors.Add(new ModelColorImage()
                                {
                                    Id = Convert.ToUInt32(reader["modelColorId"]),
                                    Name = Convert.ToString(reader["ColorName"]),
                                    Host = Convert.ToString(reader["Host"]),
                                    OriginalImagePath = Convert.ToString(reader["OriginalImagePath"]),
                                    IsImageExists = Convert.ToBoolean(reader["IsImageExists"]),
                                    BikeModelColorId = Convert.ToString(reader["BikeModelColorId"]),
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
                                            IsActive = Convert.ToBoolean(reader["IsActive"])
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
                ErrorClass objErr = new ErrorClass(ex, "ManageModelColor.FetchModelImagesByColors");
                objErr.SendMail();
            }

            return modelColors;
        }

    }
}