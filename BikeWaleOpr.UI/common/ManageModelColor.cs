using BikewaleOpr.Entities;
using BikeWaleOpr.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using BikeWaleOpr.Entities;
using System.Data.Common;
using BikeWaleOPR.Utilities;
using MySql.CoreDAL;

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
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_modelid", DbParamTypeMapper.GetInstance[SqlDbType.Int], modelId));


                    using (IDataReader reader = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (reader != null )
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

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_versionid", DbParamTypeMapper.GetInstance[SqlDbType.Int], versionId));

                    using (IDataReader reader = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (reader != null )
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
                    cmd.Parameters.Add(DbFactory.GetDbParam("@modelid", DbParamTypeMapper.GetInstance[SqlDbType.Int], modelId));

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
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_modelid", DbParamTypeMapper.GetInstance[SqlDbType.Int], modelId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_userid", DbParamTypeMapper.GetInstance[SqlDbType.VarChar], 100, userId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_colorname", DbParamTypeMapper.GetInstance[SqlDbType.VarChar], 100, colorName));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_hexcodes", DbParamTypeMapper.GetInstance[SqlDbType.VarChar], 500, hexCodes));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_companycolorname", DbParamTypeMapper.GetInstance[SqlDbType.VarChar], 100, Convert.DBNull));

                    isSaved = MySqlDatabase.InsertQuery(cmd, ConnectionType.MasterDatabase);
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
        /// </summary>
        /// <param name="versionColor">Version Color Entity</param>
        /// <param name="userId">User Id</param>
        /// <returns>Success/Failure</returns>
        public bool SaveVersionColor(VersionColorBase versionColor, int versionId ,int userId)
        {
            bool isSaved = false;
            
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("saveversioncolor"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_modelcolorid", DbParamTypeMapper.GetInstance[SqlDbType.Int], Convert.ToInt32(versionColor.ModelColorID)));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_versionid", DbParamTypeMapper.GetInstance[SqlDbType.Int], versionId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_userid", DbParamTypeMapper.GetInstance[SqlDbType.VarChar], 100, userId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_isactive", DbParamTypeMapper.GetInstance[SqlDbType.Bit], versionColor.IsActive));


                    isSaved = MySqlDatabase.InsertQuery(cmd, ConnectionType.MasterDatabase);
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
        /// </summary>
        /// <param name="colorId"></param>
        /// <param name="hexCode"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public bool UpdateColorCode(int colorId, string hexCode, int userId,bool isActive)
        {
            bool isUpdated = false;
            
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("updatemodelcolor"))
                {
                    
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_colorid", DbParamTypeMapper.GetInstance[SqlDbType.Int], colorId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_hexcode", DbParamTypeMapper.GetInstance[SqlDbType.VarChar], 6, hexCode));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_userid", DbParamTypeMapper.GetInstance[SqlDbType.VarChar], 100, userId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_isactive", DbParamTypeMapper.GetInstance[SqlDbType.Bit], isActive));

                    isUpdated = MySqlDatabase.UpdateQuery(cmd, ConnectionType.MasterDatabase);
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
        /// </summary>
        /// <param name="modelColorId">Model Color Id</param>
        /// <param name="hexCode">Color Hex Color</param>
        /// <param name="userId">User Id</param>
        /// <param name="isActive">Active/Inactive(true by default)</param>
        /// <returns></returns>
        public bool AddColorCode(int modelColorId,string hexCode,int userId,bool isActive = true)
        {
            bool isSaved = false;
            
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("addmodelcolorhexcode"))
                {
                    
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_modelcolorid", DbParamTypeMapper.GetInstance[SqlDbType.Int], modelColorId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_hexcode", DbParamTypeMapper.GetInstance[SqlDbType.VarChar],6, hexCode));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_userid", DbParamTypeMapper.GetInstance[SqlDbType.VarChar], 100, userId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_isactive", DbParamTypeMapper.GetInstance[SqlDbType.Bit], isActive));

                    isSaved = MySqlDatabase.UpdateQuery(cmd, ConnectionType.MasterDatabase);
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
        /// </summary>
        /// <param name="modelColorId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public bool DeleteModelColor(int modelColorId,int userId)
        {
            bool isDeleted = false;
            
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("deletemodelcolor"))
                {
                    
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_modelcolorid", DbParamTypeMapper.GetInstance[SqlDbType.Int], modelColorId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_userid", DbParamTypeMapper.GetInstance[SqlDbType.Int], userId));


                    isDeleted = MySqlDatabase.UpdateQuery(cmd, ConnectionType.MasterDatabase);
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "ManageModelColor.DeleteModelColor");
                objErr.SendMail();
            }
            
            return isDeleted;
        }

    }
}