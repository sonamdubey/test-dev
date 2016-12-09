﻿using Bikewale.Entities.BikeData;
using Bikewale.Entities.CMS.Photos;
using Bikewale.Interfaces.BikeData;
using Bikewale.Notifications;
using Bikewale.Utility;
using MySql.CoreDAL;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Bikewale.DAL.BikeData
{
    /// <summary>
    /// Created By : Ashish G. Kamble
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="U"></typeparam>
    public class BikeModelsRepository<T, U> : IBikeModelsRepository<T, U> where T : BikeModelEntity, new()
    {
        /// <summary>
        /// Summary : Function to get models list for a given make id.
        /// </summary>
        /// <param name="requestType">Bike data type</param>
        /// <param name="makeId"></param>
        /// <returns>Returns list containing objects of each model's basic data.</returns>
        public List<BikeModelEntityBase> GetModelsByType(EnumBikeType requestType, int makeId)
        {
            List<BikeModelEntityBase> objModelsList = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("getbikemodels_new"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_requesttype", DbType.String, 20, requestType.ToString()));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_makeid", DbType.Int32, makeId));

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            objModelsList = new List<BikeModelEntityBase>();

                            while (dr.Read())
                            {
                                objModelsList.Add(new BikeModelEntityBase()
                                {
                                    ModelId = Convert.ToInt32(dr["ID"]),
                                    ModelName = dr["NAME"].ToString(),
                                    MaskingName = dr["MaskingName"].ToString()
                                });
                            }
                            dr.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn(ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }

            return objModelsList;
        }

        public U Add(T t)
        {
            throw new NotImplementedException();
        }

        public bool Update(T t)
        {
            throw new NotImplementedException();
        }

        public bool Delete(U id)
        {
            throw new NotImplementedException();
        }

        public List<T> GetAll()
        {
            throw new NotImplementedException();
        }

        public BikeModelPageEntity GetModelPage(U modelId, bool isNew)
        {
            BikeModelPageEntity modelPage = new BikeModelPageEntity();

            try
            {

                modelPage.ModelDetails = GetById(modelId);
                modelPage.ModelDesc = GetModelSynopsis(modelId);
                modelPage.ModelVersions = GetVersionMinSpecs(modelId, isNew);
                modelPage.ModelVersionSpecs = MVSpecsFeatures(Convert.ToInt32(modelPage.ModelVersions[0].VersionId));
                modelPage.ModelVersionSpecsList = GetModelSpecifications(modelId);
                modelPage.ModelColors = GetModelColor(modelId);
            }
            catch (SqlException ex)
            {
                HttpContext.Current.Trace.Warn("GetModelDescription sql ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("GetModelDescription ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }

            return modelPage;

        }

        /// <summary>
        /// Modified By : Sushil Kumar on 21st jan 2016
        /// Description : Moved Model color logic to BAL to process multitone colors with linq
        /// Modified By : Lucky Rathore on 18th Apr 2016
        /// Description : validation modelPage.ModelDetails and modelPage.ModelDesc added. 
        /// </summary>
        /// <param name="modelId"></param>
        /// <returns></returns>
        public BikeModelPageEntity GetModelPage(U modelId)
        {
            BikeModelPageEntity modelPage = new BikeModelPageEntity();

            try
            {
                modelPage.ModelDetails = GetById(modelId);
                modelPage.ModelDesc = GetModelSynopsis(modelId);

                if (modelPage.ModelDetails == null || modelPage.ModelDesc == null)
                {
                    return null;
                }
                if (modelPage.ModelDetails != null)
                {
                    // If bike is upcoming Bike get the upcoming bike data
                    if (modelPage.ModelDetails.Futuristic)
                    {
                        modelPage.UpcomingBike = GetUpcomingBikeDetails(modelId);
                    }

                    // Get model min specs
                    modelPage.ModelVersions = GetVersionMinSpecs(modelId, modelPage.ModelDetails.New);

                    // Get version all specs
                    if (modelPage.ModelVersions != null && modelPage.ModelVersions.Count > 0)
                    {
                        modelPage.ModelVersionSpecs = MVSpecsFeatures(Convert.ToInt32(modelPage.ModelVersions[0].VersionId));
                        modelPage.ModelColors = GetModelColor(modelId);
                        modelPage.ModelVersionSpecsList = GetModelSpecifications(modelId);
                    }
                }
            }
            catch (SqlException ex)
            {
                HttpContext.Current.Trace.Warn("GetModelDescription sql ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("GetModelDescription ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }

            return modelPage;

        }

        public IEnumerable<NewBikeModelColor> GetModelColor(U modelId)
        {
            List<BikeModelColor> colors = null;
            List<NewBikeModelColor> objMultiToneColor = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("getmodelcolor_27012016"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    //cmd.CommandText = "getmodelcolor_27012016";

                    //cmd.Parameters.Add("@ModelId", SqlDbType.Int).Value = modelId;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_modelid", DbType.Int32, modelId));


                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            colors = new List<BikeModelColor>();

                            while (dr.Read())
                            {
                                colors.Add(
                                    new BikeModelColor
                                    {
                                        Id = Convert.ToUInt32(dr["ID"]),
                                        ColorName = Convert.ToString(dr["Color"]),
                                        HexCode = Convert.ToString(dr["HexCode"]),
                                        ModelId = Convert.ToUInt32(dr["BikeModelID"]),
                                    }
                                );
                            }
                            dr.Close();
                        }
                    }

                    objMultiToneColor = new List<NewBikeModelColor>();
                    var objColors = from color in colors
                                    group color by color.Id into newgroup
                                    orderby newgroup.Key
                                    select newgroup;

                    foreach (var color in objColors)
                    {
                        NewBikeModelColor tempColor = new NewBikeModelColor();
                        tempColor.Id = color.Key;

                        IList<string> HexCodeList = new List<string>();
                        foreach (var colorList in color)
                        {
                            tempColor.ColorName = colorList.ColorName;
                            tempColor.ModelId = colorList.ModelId;
                            HexCodeList.Add(colorList.HexCode);
                        }

                        tempColor.HexCodes = HexCodeList;
                        objMultiToneColor.Add(tempColor);
                    }
                }
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("GetModelColor ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }

            return objMultiToneColor;
        }

        /// <summary>
        /// Versions list with Min specs
        /// </summary>
        /// <param name="modelId">model id</param>
        /// <param name="isNew">is new</param>
        /// <returns></returns>
        public List<BikeVersionMinSpecs> GetVersionMinSpecs(U modelId, bool isNew)
        {

            List<BikeVersionMinSpecs> objMinSpecs = new List<BikeVersionMinSpecs>();
            try
            {

                using (DbCommand cmd = DbFactory.GetDBCommand("getversions"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_modelid", DbType.Int32, modelId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_new", DbType.Boolean, isNew));

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {

                        if (dr != null)
                        {
                            while (dr.Read())
                            {
                                objMinSpecs.Add(new BikeVersionMinSpecs()
                                {
                                    VersionId = Convert.ToInt32(dr["ID"]),
                                    VersionName = Convert.ToString(dr["Version"]),
                                    ModelName = Convert.ToString(dr["Model"]),
                                    Price = Convert.ToUInt64(dr["VersionPrice"]),
                                    BrakeType = !Convert.IsDBNull(dr["BrakeType"]) ? Convert.ToString(dr["BrakeType"]) : String.Empty,
                                    AlloyWheels = !Convert.IsDBNull(dr["AlloyWheels"]) ? Convert.ToBoolean(dr["AlloyWheels"]) : false,
                                    ElectricStart = !Convert.IsDBNull(dr["ElectricStart"]) ? Convert.ToBoolean(dr["ElectricStart"]) : false,
                                    AntilockBrakingSystem = !Convert.IsDBNull(dr["AntilockBrakingSystem"]) ? Convert.ToBoolean(dr["AntilockBrakingSystem"]) : false,
                                });
                            }
                            dr.Close();
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }


            return objMinSpecs;
        }

        public BikeSpecificationEntity MVSpecsFeatures(int versionId)
        {
            var mv = new BikeVersionsRepository<BikeVersionEntity, int>();
            return mv.GetSpecifications(versionId);
        }


        /// <summary>
        /// Summary : Function to get particular model's all details.
        /// Modified By : Sadhana Upadhyay on 20 Aug 2014
        /// Summary : To retrieve new and used flag
        /// Modified By: Aditi Srivastava on 25th Aug,2016
        /// Summary: Used a different sp(earlier getmodeldetails_new) to retrieve model details using data reader
        /// </summary>
        /// <param name="id">Model Id should be a positive number.</param>
        /// <returns>Returns object containing the particular model's all details.</returns>
        public T GetById(U id)
        {
            T t = default(T);
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("getmodeldetails_new_25082016"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_modelid", DbType.Int32, id));

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {

                            while (dr.Read())
                            {
                                t = new T();
                                t.ModelId = Convert.ToInt32(cmd.Parameters["par_modelid"].Value);
                                t.ModelName = Convert.ToString(dr["Name"]);
                                t.MakeBase.MakeId = Convert.ToInt32(dr["BikeMakeId"]);
                                t.MakeBase.MakeName = Convert.ToString(dr["MakeName"]);
                                t.Futuristic = Convert.ToBoolean(dr["Futuristic"]);
                                t.New = Convert.ToBoolean(dr["New"]);
                                t.Used = Convert.ToBoolean(dr["Used"]);
                                t.SmallPicUrl = Convert.ToString(dr["SmallPic"]);
                                t.LargePicUrl = Convert.ToString(dr["LargePic"]);
                                t.HostUrl = Convert.ToString(dr["HostURL"]);
                                t.MinPrice = Convert.ToInt64(dr["MinPrice"]);
                                t.MaxPrice = Convert.ToInt64(dr["MaxPrice"]);
                                t.MaskingName = Convert.ToString(dr["MaskingName"]);
                                t.MakeBase.MaskingName = Convert.ToString(dr["MakeMaskingname"]);
                                t.ModelSeries.SeriesId = 0;
                                t.ModelSeries.SeriesName = string.Empty;
                                t.ModelSeries.MaskingName = string.Empty;
                                t.ReviewCount = Convert.ToInt32(dr["ReviewCount"]);
                                t.ReviewRate = Convert.ToDouble(dr["ReviewRate"]);
                                t.ReviewUIRating = string.Format("{0:0.0}", t.ReviewRate);
                                t.OriginalImagePath = Convert.ToString(dr["OriginalImagePath"]);
                                t.PhotosCount = Convert.ToInt32(dr["PhotosCount"]);
                                t.VideosCount = Convert.ToInt32(dr["VideosCount"]);
                            }
                            dr.Close();
                        }
                    }
                }
            }
            catch (Exception err)
            {
                HttpContext.Current.Trace.Warn("Exception in GetById", err.Message);
                ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            return t;

        }


        /// <summary>
        /// Summary  : Function to get the bike versions list for the given model id
        /// Modified By : Sadhana on 20 Aug 2014 
        /// passed new parameter to sp to retrieve version list for new as well as discontinued bikes
        /// </summary>
        /// <param name="modelId">Only positive numbers are allowed.</param>
        /// <returns>Returns the list containg BikeVersionsList</returns>
        public List<BikeVersionsListEntity> GetVersionsList(U modelId, bool isNew)
        {
            List<BikeVersionsListEntity> objList = null;

            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "getversions_new";
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_modelid", DbType.Int32, modelId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_new", DbType.Boolean, isNew));

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            objList = new List<BikeVersionsListEntity>();

                            while (dr.Read())
                            {
                                BikeVersionsListEntity version = new BikeVersionsListEntity();
                                version.VersionId = Convert.ToInt32(dr["VersionId"]);
                                version.VersionName = Convert.ToString(dr["VersionName"]);
                                version.ModelName = Convert.ToString(dr["ModelName"]);
                                version.Price = Convert.ToUInt64(dr["Price"]);
                                objList.Add(version);
                            }
                            dr.Close();
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                HttpContext.Current.Trace.Warn("GetVersionsList sql ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("GetVersionsList ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }

            return objList;
        }

        /// <summary>
        /// Summary : function to get the model description.
        /// </summary>
        /// <param name="modelId">model id should be positive number.</param>
        /// <returns>Returns description in the object.</returns>
        public BikeDescriptionEntity GetModelSynopsis(U modelId)
        {
            BikeDescriptionEntity objModel = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("getmodelsynopsis"))
                {
                    // cmd.CommandText = "GetModelSynopsis";
                    cmd.CommandType = CommandType.StoredProcedure;

                    // cmd.Parameters.Add("@ModelId", SqlDbType.Int).Value = modelId;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_modelid", DbType.Int32, modelId));

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null && dr.Read())
                        {
                            objModel = new BikeDescriptionEntity()
                            {
                                SmallDescription = Convert.ToString(dr["SmallDescription"]),
                                FullDescription = Convert.ToString(dr["FullDescription"])
                            };
                            dr.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("GetModelDescription ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            return objModel;
        }   // End of GetModelDescription

        /// <summary>
        /// Summary : Function to get upcoming models information
        /// </summary>
        /// <param name="modelId">Only positive numbers are allowed.</param>
        /// <returns>Returns Upcoming bikes data in the UpcomingBikesEntity object.</returns>
        public UpcomingBikeEntity GetUpcomingBikeDetails(U modelId)
        {
            UpcomingBikeEntity objModel = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("getupcomingbikedetails"))
                {
                    //cmd.CommandText = "getupcomingbikedetails";
                    cmd.CommandType = CommandType.StoredProcedure;

                    //cmd.Parameters.Add("@ModelId", SqlDbType.Int).Value = modelId;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_modelid", DbType.Int32, modelId));

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null && dr.Read())
                        {
                            objModel = new UpcomingBikeEntity()
                            {
                                ExpectedLaunchId = Convert.ToUInt16(dr["id"]),
                                ExpectedLaunchDate = !String.IsNullOrEmpty(Convert.ToString(dr["ExpectedLaunch"])) ? Convert.ToDateTime(dr["ExpectedLaunch"]).ToString("MMM yyyy") : "",
                                EstimatedPriceMin = Convert.ToUInt64(dr["EstimatedPriceMin"]),
                                EstimatedPriceMax = Convert.ToUInt64(dr["EstimatedPriceMax"]),
                                HostUrl = Convert.ToString(dr["HostURL"]),
                                LargePicImagePath = Convert.ToString(dr["LargePicImagePath"]),
                                OriginalImagePath = Convert.ToString(dr["OriginalImagePath"])
                            };
                            dr.Close();
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                HttpContext.Current.Trace.Warn("GetUpcomingBikeDetails sql ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("GetUpcomingBikeDetails ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            return objModel;
        }   // End of GetUpcomingBikeDetails

        /// <summary>
        /// Modified By : Sadhana Upadhyay on 29 Oct 2015
        /// Summary : To fetch
        /// </summary>
        /// <param name="inputParams"></param>
        /// <param name="sortBy"></param>
        /// <param name="recordCount"></param>
        /// <returns></returns>
        public List<UpcomingBikeEntity> GetUpcomingBikesList(UpcomingBikesListInputEntity inputParams, EnumUpcomingBikesFilter sortBy, out int recordCount)
        {
            List<UpcomingBikeEntity> objModelList = null;

            recordCount = 0;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("getupcomingbikeslist_new"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_startindex", DbType.Int32, inputParams.StartIndex));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_endindex", DbType.Int32, inputParams.EndIndex));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_makeid", DbType.Int32, (inputParams.MakeId > 0) ? inputParams.MakeId : Convert.DBNull));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_modelid", DbType.Int32, (inputParams.ModelId > 0) ? inputParams.ModelId : Convert.DBNull));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_estimatedprice", DbType.Boolean, (sortBy != EnumUpcomingBikesFilter.Default) ? ((sortBy == EnumUpcomingBikesFilter.PriceHighToLow) ? true : false) : Convert.DBNull));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_launchdate", DbType.Boolean, (sortBy != EnumUpcomingBikesFilter.Default) ? ((sortBy == EnumUpcomingBikesFilter.LaunchDateLater) ? true : false) : Convert.DBNull));


                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            objModelList = new List<UpcomingBikeEntity>();

                            while (dr.Read())
                            {
                                UpcomingBikeEntity objModel = new UpcomingBikeEntity();

                                objModel.ExpectedLaunchId = Convert.ToUInt16(dr["ExpectedLaunchId"]);
                                objModel.ExpectedLaunchDate = !String.IsNullOrEmpty(Convert.ToString(dr["ExpectedLaunch"])) ? Convert.ToDateTime(dr["ExpectedLaunch"]).ToString("MMM yyyy") : "";
                                objModel.EstimatedPriceMin = Convert.ToUInt64(dr["EstimatedPriceMin"]);
                                objModel.EstimatedPriceMax = Convert.ToUInt64(dr["EstimatedPriceMax"]);
                                objModel.HostUrl = Convert.ToString(dr["HostURL"]);
                                objModel.LargePicImagePath = Convert.ToString(dr["LargePicImagePath"]);
                                objModel.BikeDescription.SmallDescription = Convert.ToString(dr["Description"]);
                                objModel.MakeBase.MakeId = Convert.ToInt32(dr["MakeId"]);
                                objModel.MakeBase.MakeName = Convert.ToString(dr["MakeName"]);
                                objModel.MakeBase.MaskingName = Convert.ToString(dr["MakeMaskingName"]);
                                objModel.ModelBase.ModelId = Convert.ToInt32(dr["ModelId"]);
                                objModel.ModelBase.ModelName = Convert.ToString(dr["ModelName"]);
                                objModel.ModelBase.MaskingName = Convert.ToString(dr["ModelMaskingName"]);
                                objModel.OriginalImagePath = Convert.ToString(dr["OriginalImagePath"]);
                                objModel.BikeName = string.Format("{0} {1}", objModel.MakeBase.MakeName, objModel.ModelBase.ModelName);
                                objModelList.Add(objModel);

                                recordCount = Convert.ToInt32(dr["RecordCount"]);
                            }
                            dr.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("GetUpcomingBikesList ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }

            return objModelList;
        }

        /// <summary>
        /// Created By : Sadhana Upadhyay on 4 June 2014
        /// Summary : to get all recently launched bikes
        /// </summary>
        /// <param name="startIndex"></param>
        /// <param name="endIndex"></param>
        /// <param name="recordCount"></param>
        /// <returns></returns>
        public NewLaunchedBikesBase GetNewLaunchedBikesList(int startIndex, int endIndex)
        {
            NewLaunchedBikesBase newLaunchedBikes = new NewLaunchedBikesBase();
            List<NewLaunchedBikeEntity> objModelList = null;
            int recordCount = 0;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("getnewlaunchedbikes_04082016"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_startindex", DbType.Int32, startIndex));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_endindex", DbType.Int32, endIndex));

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            objModelList = new List<NewLaunchedBikeEntity>();

                            while (dr.Read())
                            {
                                NewLaunchedBikeEntity objModels = new NewLaunchedBikeEntity();
                                objModels.Specs = new MinSpecsEntity();
                                objModels.BikeLaunchId = Convert.ToUInt16(dr["BikeLaunchId"]);
                                objModels.MakeBase.MakeId = Convert.ToInt32(dr["BikeMakeId"]);
                                objModels.MakeBase.MakeName = dr["Make"].ToString();
                                objModels.MakeBase.MaskingName = dr["MakeMaskingName"].ToString();
                                objModels.ModelId = Convert.ToInt32(dr["ModelId"]);
                                objModels.ModelName = dr["Model"].ToString();
                                objModels.MaskingName = dr["ModelMaskingName"].ToString();
                                objModels.HostUrl = dr["HostURL"].ToString();
                                objModels.LargePicUrl = dr["LargePic"].ToString();
                                objModels.SmallPicUrl = dr["SmallPic"].ToString();
                                objModels.ReviewCount = Convert.ToInt16(dr["ReviewCount"]);
                                objModels.ReviewRate = Convert.ToDouble(dr["ReviewRate"]);
                                objModels.ReviewUIRating = string.Format("{0:0.0}", objModels.ReviewRate);
                                objModels.MinPrice = Convert.ToInt64(dr["MinPrice"]);
                                objModels.MaxPrice = Convert.ToInt64(dr["MaxPrice"]);
                                objModels.LaunchDate = Convert.ToDateTime(dr["LaunchDate"]);
                                objModels.OriginalImagePath = Convert.ToString(dr["OriginalImagePath"]);
                                objModels.Specs.Displacement = SqlReaderConvertor.ToNullableFloat(dr["Displacement"]);
                                objModels.Specs.FuelEfficiencyOverall = SqlReaderConvertor.ToNullableUInt16(dr["FuelEfficiencyOverall"]);
                                objModels.Specs.MaximumTorque = SqlReaderConvertor.ToNullableFloat(dr["MaximumTorque"]);
                                objModels.Specs.KerbWeight = SqlReaderConvertor.ToNullableUInt16(dr["kerbweight"]);
                                objModels.Specs.MaxPower = SqlReaderConvertor.ToNullableFloat(dr["MaxPower"]);
                                objModelList.Add(objModels);

                            }
                            if (dr.NextResult())
                            {
                                if (dr.Read())
                                {
                                    recordCount = Convert.ToInt32(dr["RecordCount"]);
                                }
                            }
                            dr.Close();
                            newLaunchedBikes.Models = objModelList;
                            newLaunchedBikes.RecordCount = recordCount;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("GetNewLaunchedBikesList ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            return newLaunchedBikes;
        }
        public NewLaunchedBikesBase GetNewLaunchedBikesListByMake(int startIndex, int endIndex, int? makeid = null)
        {
            NewLaunchedBikesBase newLaunchedBikes = new NewLaunchedBikesBase();
            List<NewLaunchedBikeEntity> objModelList = null;
            int recordCount = 0;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("getnewlaunchedbikes_23092016"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_startindex", DbType.Int32, startIndex));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_endindex", DbType.Int32, endIndex));
                    if (makeid.HasValue && makeid > 0)
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_makeId", DbType.Int32, makeid));

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            objModelList = new List<NewLaunchedBikeEntity>();

                            while (dr.Read())
                            {
                                NewLaunchedBikeEntity objModels = new NewLaunchedBikeEntity();
                                objModels.Specs = new MinSpecsEntity();
                                objModels.BikeLaunchId = Convert.ToUInt16(dr["BikeLaunchId"]);
                                objModels.MakeBase.MakeId = Convert.ToInt32(dr["BikeMakeId"]);
                                objModels.MakeBase.MakeName = dr["Make"].ToString();
                                objModels.MakeBase.MaskingName = dr["MakeMaskingName"].ToString();
                                objModels.ModelId = Convert.ToInt32(dr["ModelId"]);
                                objModels.ModelName = dr["Model"].ToString();
                                objModels.MaskingName = dr["ModelMaskingName"].ToString();
                                objModels.HostUrl = dr["HostURL"].ToString();
                                objModels.LargePicUrl = dr["LargePic"].ToString();
                                objModels.SmallPicUrl = dr["SmallPic"].ToString();
                                objModels.ReviewCount = Convert.ToInt16(dr["ReviewCount"]);
                                objModels.ReviewRate = Convert.ToDouble(dr["ReviewRate"]);
                                objModels.ReviewUIRating = string.Format("{0:0.0}", objModels.ReviewRate);
                                objModels.MinPrice = Convert.ToInt64(dr["MinPrice"]);
                                objModels.MaxPrice = Convert.ToInt64(dr["MaxPrice"]);
                                objModels.LaunchDate = Convert.ToDateTime(dr["LaunchDate"]);
                                objModels.OriginalImagePath = Convert.ToString(dr["OriginalImagePath"]);
                                objModels.Specs.Displacement = SqlReaderConvertor.ToNullableFloat(dr["Displacement"]);
                                objModels.Specs.FuelEfficiencyOverall = SqlReaderConvertor.ToNullableUInt16(dr["FuelEfficiencyOverall"]);
                                objModels.Specs.MaximumTorque = SqlReaderConvertor.ToNullableFloat(dr["MaximumTorque"]);
                                objModels.Specs.KerbWeight = SqlReaderConvertor.ToNullableUInt16(dr["kerbweight"]);
                                objModels.Specs.MaxPower = SqlReaderConvertor.ToNullableFloat(dr["MaxPower"]);
                                objModelList.Add(objModels);

                            }
                            if (dr.NextResult())
                            {
                                if (dr.Read())
                                {
                                    recordCount = Convert.ToInt32(dr["RecordCount"]);
                                }
                            }
                            dr.Close();
                            newLaunchedBikes.Models = objModelList;
                            newLaunchedBikes.RecordCount = recordCount;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("GetNewLaunchedBikesList ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            return newLaunchedBikes;
        }


        public List<MostPopularBikesBase> GetMostPopularBikes(int? topCount = null, int? makeId = null)
        {
            List<MostPopularBikesBase> objList = null;
            MostPopularBikesBase objData = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("getmostpopularbikes"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_topcount", DbType.Int16, topCount));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_makeid", DbType.Int32, makeId.HasValue && makeId.Value > 0 ? makeId : Convert.DBNull));


                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            objList = new List<MostPopularBikesBase>();

                            while (dr.Read())
                            {
                                objData = new MostPopularBikesBase();
                                objData.objMake = new BikeMakeEntityBase();
                                objData.objModel = new BikeModelEntityBase();
                                objData.objVersion = new BikeVersionsListEntity();
                                objData.Specs = new MinSpecsEntity();
                                objData.objMake.MakeName = Convert.ToString(dr["Make"]);
                                objData.objModel.ModelName = Convert.ToString(dr["Model"]);
                                objData.objMake.MakeId = Convert.ToInt32(dr["MakeId"]);
                                objData.objModel.ModelId = Convert.ToInt32(dr["ModelId"]);
                                objData.objMake.MaskingName = Convert.ToString(dr["MakeMaskingName"]);
                                objData.objModel.MaskingName = Convert.ToString(dr["ModelMaskingName"]);
                                objData.objVersion.VersionId = Convert.ToInt32(dr["VersionId"]);
                                objData.ModelRating = Convert.ToDouble(dr["ReviewRate"]);
                                objData.ReviewCount = Convert.ToUInt16(dr["ReviewCount"]);
                                objData.BikeName = Convert.ToString(dr["BikeName"]);
                                objData.HostURL = Convert.ToString(dr["HostUrl"]);
                                objData.OriginalImagePath = Convert.ToString(dr["OriginalImagePath"]);
                                objData.VersionPrice = SqlReaderConvertor.ToInt64(dr["VersionPrice"]);
                                objData.Specs.Displacement = SqlReaderConvertor.ToNullableFloat(dr["Displacement"]);
                                objData.Specs.FuelEfficiencyOverall = SqlReaderConvertor.ToNullableUInt16(dr["FuelEfficiencyOverall"]);
                                objData.Specs.MaximumTorque = SqlReaderConvertor.ToNullableFloat(dr["MaximumTorque"]);
                                objData.Specs.MaxPower = SqlReaderConvertor.ToNullableFloat(dr["MaxPower"]);
                                objList.Add(objData);
                            }
                            dr.Close();
                        }
                    }
                }
            }
            catch (Exception err)
            {
                HttpContext.Current.Trace.Warn("Exception in GetModelsList", err.Message);
                ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }

            return objList;
        }


        public List<MostPopularBikesBase> GetMostPopularBikesByMake(int makeId)
        {
            List<MostPopularBikesBase> objList = null;
            MostPopularBikesBase objData = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("getmostpopularbikesbymake_04082016"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_makeid", DbType.Int32, makeId));

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            objList = new List<MostPopularBikesBase>();

                            while (dr.Read())
                            {
                                objData = new MostPopularBikesBase();
                                objData.objMake = new BikeMakeEntityBase();
                                objData.objModel = new BikeModelEntityBase();
                                objData.objVersion = new BikeVersionsListEntity();
                                objData.Specs = new MinSpecsEntity();
                                objData.objMake.MakeName = Convert.ToString(dr["Make"]);
                                objData.objModel.ModelName = Convert.ToString(dr["Model"]);
                                objData.objMake.MakeId = Convert.ToInt32(dr["MakeId"]);
                                objData.objModel.ModelId = Convert.ToInt32(dr["ModelId"]);
                                objData.objMake.MaskingName = Convert.ToString(dr["MakeMaskingName"]);
                                objData.objModel.MaskingName = Convert.ToString(dr["ModelMaskingName"]);
                                objData.objVersion.VersionId = Convert.ToInt32(dr["VersionId"]);
                                objData.ModelRating = Convert.ToDouble(dr["ReviewRate"]);
                                objData.ReviewCount = Convert.ToUInt16(dr["ReviewCount"]);
                                objData.BikeName = Convert.ToString(dr["BikeName"]);
                                objData.HostURL = Convert.ToString(dr["HostUrl"]);
                                objData.OriginalImagePath = Convert.ToString(dr["OriginalImagePath"]);
                                objData.VersionPrice = SqlReaderConvertor.ToInt64(dr["VersionPrice"]);
                                objData.Specs.Displacement = SqlReaderConvertor.ToNullableFloat(dr["Displacement"]);
                                objData.Specs.FuelEfficiencyOverall = SqlReaderConvertor.ToNullableUInt16(dr["FuelEfficiencyOverall"]);
                                objData.Specs.MaximumTorque = SqlReaderConvertor.ToNullableFloat(dr["MaximumTorque"]);
                                objData.Specs.MaxPower = SqlReaderConvertor.ToNullableFloat(dr["MaxPower"]);
                                objData.BikePopularityIndex = Convert.ToUInt16(dr["PopularityIndex"]);
                                objData.Specs.KerbWeight = SqlReaderConvertor.ToNullableUInt16(dr["KerbWeight"]);
                                objList.Add(objData);
                            }
                            dr.Close();
                        }
                    }
                }
            }
            catch (SqlException err)
            {
                HttpContext.Current.Trace.Warn("SQL Exception in GetModelsList", err.Message);
                ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            catch (Exception err)
            {
                HttpContext.Current.Trace.Warn("Exception in GetModelsList", err.Message);
                ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            return objList;
        }


        /// <summary>
        /// Created By : Suresh Prajapati on 24th Sep-14
        /// Summary : To create hash table for masking names
        /// </summary>
        /// <returns></returns>
        public Hashtable GetMaskingNames()
        {
            Hashtable ht = null;

            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("sp_getmodelmappingnames"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            ht = new Hashtable();

                            while (dr.Read())
                            {
                                if (!ht.ContainsKey(dr["MaskingName"]))
                                    ht.Add(dr["MaskingName"], dr["ID"]);
                            }
                            dr.Close();
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                HttpContext.Current.Trace.Warn("SP_GetModelMappingNames sql ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("SP_GetModelMappingNames ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            return ht;
        }


        /// <summary>
        /// Created By : Suresh Prajapati on 24th Sep-2014
        /// Summary : To create Hash table for old masking names
        /// </summary>
        /// <returns></returns>
        public Hashtable GetOldMaskingNames()
        {
            Hashtable ht = null;

            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandText = "getoldmaskingnameslist";
                    cmd.CommandType = CommandType.StoredProcedure;

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            ht = new Hashtable();

                            if (dr != null)
                            {
                                while (dr.Read())
                                {
                                    if (!ht.ContainsKey(dr["OldMaskingName"]))
                                        ht.Add(dr["OldMaskingName"], dr["NewMaskingName"]);
                                }
                            }
                            dr.Close();
                        }
                    }
                }
            }

            catch (SqlException ex)
            {
                HttpContext.Current.Trace.Warn("GetOldMaskingNamesList sql ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("GetOldMaskingNamesList ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }

            return ht;
        }

        #region GetFeaturedBikes Method
        /// <summary>
        /// Created By : Sadhana Upadhyay on 19 Aug 2015
        /// Summary : To get Featured Bikes
        /// </summary>
        /// <param name="topRecords"></param>
        /// <returns></returns>
        public List<FeaturedBikeEntity> GetFeaturedBikes(uint topRecords)
        {
            List<FeaturedBikeEntity> objFeatured = null;

            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandText = "getfeaturedbikemin_new";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_topcount", DbType.Byte, topRecords));

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            objFeatured = new List<FeaturedBikeEntity>();
                            while (dr.Read())
                            {
                                FeaturedBikeEntity featuredBike = new FeaturedBikeEntity();

                                featuredBike.BikeName = dr["BikeName"].ToString();
                                featuredBike.Discription = dr["Description"].ToString();
                                featuredBike.HostUrl = dr["HostUrl"].ToString();
                                featuredBike.OriginalImagePath = dr["OriginalImagePath"].ToString();
                                featuredBike.Priority = Convert.ToUInt16(dr["DisplayPriority"]);
                                featuredBike.MakeBase = new BikeMakeEntityBase()
                                {
                                    MakeId = Convert.ToInt32(dr["MakeId"]),
                                    MakeName = dr["MakeName"].ToString(),
                                    MaskingName = dr["MakeMaskingName"].ToString()
                                };

                                featuredBike.ModelBase = new BikeModelEntityBase()
                                {
                                    ModelId = Convert.ToInt32(dr["ModelId"]),
                                    ModelName = dr["ModelName"].ToString(),
                                    MaskingName = dr["ModelMaskingName"].ToString()
                                };

                                objFeatured.Add(featuredBike);
                            }
                            dr.Close();
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Sql Exception : Bikewale.DAL.BikeModelRepository.GetFeaturedBikes");
                objErr.SendMail();
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.DAL.BikeModelRepository.GetFeaturedBikes");
                objErr.SendMail();
            }

            return objFeatured;
        }
        #endregion

        #region GetAllModels Method
        /// <summary>
        /// Created By : Sadhana Upadhyay on 9 Nov 2015
        /// Summary : To Get ModelList According to request type
        /// </summary>
        /// <param name="requestType"></param>
        /// <returns></returns>
        public IEnumerable<BikeMakeModelEntity> GetAllModels(EnumBikeType requestType)
        {
            IList<BikeMakeModelEntity> objList = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandText = "getallmodels";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_requesttype", DbType.Byte, (int)requestType));

                    using (IDataReader reader = MySqlDatabase.SelectQuery(cmd, ConnectionType.MasterDatabase))
                    {
                        if (reader != null)
                        {
                            objList = new List<BikeMakeModelEntity>();
                            while (reader.Read())
                            {
                                objList.Add(new BikeMakeModelEntity()
                                {
                                    MakeBase = new BikeMakeEntityBase()
                                    {
                                        MakeId = Convert.ToInt32(reader["MakeId"]),
                                        MakeName = reader["MakeName"].ToString(),
                                        MaskingName = reader["MakeMaskingName"].ToString()
                                    },
                                    ModelBase = new BikeModelEntityBase()
                                    {
                                        ModelId = Convert.ToInt32(reader["ModelId"]),
                                        ModelName = reader["ModelName"].ToString(),
                                        MaskingName = reader["ModelMaskingName"].ToString()
                                    }
                                });
                            }
                            reader.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.DAL.BikeModelRepository.GetFeaturedBikes");
                objErr.SendMail();
            }
            return objList;
        }   //End of GetAllModels Method
        #endregion


        /// <summary>
        /// Author : Vivek Gupta on 9-5-2016
        /// Desc : this method has not been implemented in dal
        /// </summary>
        /// <param name="modelId"></param>
        /// <returns></returns>
        public BikeModelContent GetRecentModelArticles(U modelId)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Created By: Aditi Srivastava on 17th Aug, 2016
        /// Description: Fetches model image original image path and host url of model image
        /// </summary>
        /// <param name="modelId"></param>
        /// <returns></returns>
        public ModelPhotos GetModelPhotoInfo(U modelId)
        {
            ModelPhotos modelPhotos = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("getmodelphotos"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_modelid", DbType.Int32, modelId));

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            while (dr.Read())
                            {
                                modelPhotos = new ModelPhotos();
                                modelPhotos.HostURL = Convert.ToString(dr["HostUrl"]);
                                modelPhotos.OriginalImgPath = Convert.ToString(dr["OriginalImagePath"]);
                                modelPhotos.ModelName = Convert.ToString(dr["Name"]);
                                modelPhotos.MakeName = Convert.ToString(dr["MakeName"]);
                            }
                            dr.Close();
                        }
                    }
                }
            }
            catch (Exception err)
            {
                HttpContext.Current.Trace.Warn("Exception in GetModelPhotoInfo", err.Message);
                ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            return modelPhotos;
        }
        /// <summary>
        /// Created by Subodh Jain on 22 sep 2016
        /// Des:- To fetch most popular bikes on make and city
        /// Modified By : Sushil Kumar on 10th Nov 2016
        /// Description : Get cityname with other info
        /// </summary>
        /// <param name="topCount"></param>
        /// <param name="makeId"></param>
        /// <param name="cityId"></param>
        /// <returns></returns>
        public IEnumerable<MostPopularBikesBase> GetMostPopularBikesbyMakeCity(uint topCount, uint makeId, uint cityId)
        {
            List<MostPopularBikesBase> objList = null;
            MostPopularBikesBase objData = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("getmostpopularbikesbymakecity"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_topcount", DbType.Int16, topCount));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_makeid", DbType.Int32, makeId > 0 ? makeId : Convert.DBNull));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_cityid", DbType.Int32, cityId));


                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            objList = new List<MostPopularBikesBase>();

                            while (dr.Read())
                            {
                                objData = new MostPopularBikesBase();
                                objData.objMake = new BikeMakeEntityBase();
                                objData.objModel = new BikeModelEntityBase();
                                objData.objVersion = new BikeVersionsListEntity();
                                objData.Specs = new MinSpecsEntity();
                                objData.objMake.MakeName = Convert.ToString(dr["Make"]);
                                objData.objModel.ModelName = Convert.ToString(dr["Model"]);
                                objData.objMake.MakeId = Convert.ToInt32(dr["MakeId"]);
                                objData.objModel.ModelId = Convert.ToInt32(dr["ModelId"]);
                                objData.objMake.MaskingName = Convert.ToString(dr["MakeMaskingName"]);
                                objData.objModel.MaskingName = Convert.ToString(dr["ModelMaskingName"]);
                                objData.objVersion.VersionId = Convert.ToInt32(dr["VersionId"]);
                                objData.ModelRating = Convert.ToDouble(dr["ReviewRate"]);
                                objData.ReviewCount = Convert.ToUInt16(dr["ReviewCount"]);
                                objData.BikeName = Convert.ToString(dr["BikeName"]);
                                objData.HostURL = Convert.ToString(dr["HostUrl"]);
                                objData.OriginalImagePath = Convert.ToString(dr["OriginalImagePath"]);
                                objData.VersionPrice = SqlReaderConvertor.ToInt64(dr["VersionPrice"]);
                                objData.Specs.Displacement = SqlReaderConvertor.ToNullableFloat(dr["Displacement"]);
                                objData.Specs.FuelEfficiencyOverall = SqlReaderConvertor.ToNullableUInt16(dr["FuelEfficiencyOverall"]);
                                objData.Specs.MaximumTorque = SqlReaderConvertor.ToNullableFloat(dr["MaximumTorque"]);
                                objData.Specs.MaxPower = SqlReaderConvertor.ToNullableFloat(dr["MaxPower"]);
                                objData.Specs.KerbWeight = SqlReaderConvertor.ToNullableUInt16(dr["KerbWeight"]);
                                objData.CityName = Convert.ToString(dr["cityname"]);
                                objList.Add(objData);
                            }
                            dr.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "BikeModelsRepository.GetMostPopularBikesbymakecity");
                objErr.SendMail();
            }
            return objList;
        }
        /// <summary>
        /// Created By : Sangram Nandkhile on 01 Dec 2016
        /// Summary : Function to get all specifications of all the versions of ModelId
        /// </summary>
        public IEnumerable<BikeSpecificationEntity> GetModelSpecifications(U modelId)
        {
            List<BikeSpecificationEntity> objMinspecs = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("getbikemodelspecifications"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_modelid", DbType.Int32, modelId));
                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        objMinspecs = new List<BikeSpecificationEntity>();
                        if (dr != null)
                        {
                            while (dr.Read())
                                objMinspecs.Add(new BikeSpecificationEntity()
                                {
                                    BikeVersionId = Convert.ToUInt32(dr["versionid"].ToString()),
                                    Displacement = Convert.ToSingle(dr["displacement"]),
                                    Cylinders = Convert.ToUInt16(dr["cylinders"]),
                                    MaxPower = Convert.ToSingle(dr["maxpower"]),
                                    MaximumTorque = Convert.ToSingle(dr["maximumtorque"]),
                                    Bore = Convert.ToSingle(dr["bore"]),
                                    Stroke = Convert.ToSingle(dr["stroke"]),
                                    ValvesPerCylinder = Convert.ToUInt16(dr["valvespercylinder"]),
                                    FuelDeliverySystem = dr["fueldeliverysystem"].ToString(),
                                    FuelType = dr["fueltype"].ToString(),
                                    Ignition = dr["ignition"].ToString(),
                                    SparkPlugsPerCylinder = dr["sparkplugspercylinder"].ToString(),
                                    CoolingSystem = dr["coolingsystem"].ToString(),
                                    GearboxType = dr["gearboxtype"].ToString(),
                                    NoOfGears = Convert.ToUInt16(dr["noofgears"].ToString()),
                                    TransmissionType = dr["transmissiontype"].ToString(),
                                    Clutch = dr["clutch"].ToString(),
                                    Performance_0_60_kmph = Convert.ToSingle(dr["performance_0_60_kmph"]),
                                    Performance_0_80_kmph = Convert.ToSingle(dr["performance_0_80_kmph"]),
                                    Performance_0_40_m = Convert.ToSingle(dr["performance_0_40_m"]),
                                    TopSpeed = Convert.ToUInt16(dr["topspeed"]),
                                    Performance_60_0_kmph = dr["performance_60_0_kmph"].ToString(),
                                    Performance_80_0_kmph = dr["performance_80_0_kmph"].ToString(),
                                    KerbWeight = Convert.ToUInt16(dr["kerbweight"]),
                                    OverallLength = Convert.ToUInt16(dr["overalllength"]),
                                    OverallWidth = Convert.ToUInt16(dr["overallwidth"]),
                                    OverallHeight = Convert.ToUInt16(dr["overallheight"]),
                                    Wheelbase = Convert.ToUInt16(dr["wheelbase"]),
                                    GroundClearance = Convert.ToUInt16(dr["groundclearance"]),
                                    SeatHeight = Convert.ToUInt16(dr["seatheight"]),
                                    FuelTankCapacity = Convert.ToSingle(dr["fueltankcapacity"]),
                                    ReserveFuelCapacity = Convert.ToSingle(dr["reservefuelcapacity"]),
                                    FuelEfficiencyOverall = Convert.ToUInt16(dr["fuelefficiencyoverall"]),
                                    FuelEfficiencyRange = Convert.ToUInt16(dr["fuelefficiencyrange"]),
                                    ChassisType = dr["chassistype"].ToString(),
                                    FrontSuspension = dr["frontsuspension"].ToString(),
                                    RearSuspension = dr["rearsuspension"].ToString(),
                                    BrakeType = dr["braketype"].ToString(),
                                    FrontDisc = Convert.ToBoolean(dr["frontdisc"]),
                                    FrontDisc_DrumSize = Convert.ToUInt16(dr["frontdisc_drumsize"]),
                                    RearDisc = Convert.ToBoolean(dr["reardisc"]),
                                    RearDisc_DrumSize = Convert.ToUInt16(dr["reardisc_drumsize"]),
                                    CalliperType = dr["callipertype"].ToString(),
                                    WheelSize = Convert.ToSingle(dr["wheelsize"]),
                                    FrontTyre = dr["fronttyre"].ToString(),
                                    RearTyre = dr["reartyre"].ToString(),
                                    TubelessTyres = Convert.ToBoolean(dr["tubelesstyres"]),
                                    RadialTyres = Convert.ToBoolean(dr["radialtyres"]),
                                    AlloyWheels = Convert.ToBoolean(dr["alloywheels"]),
                                    ElectricSystem = dr["electricsystem"].ToString(),
                                    Battery = dr["battery"].ToString(),
                                    HeadlightType = dr["headlighttype"].ToString(),
                                    HeadlightBulbType = dr["headlightbulbtype"].ToString(),
                                    Brake_Tail_Light = dr["brake_tail_light"].ToString(),
                                    TurnSignal = dr["turnsignal"].ToString(),
                                    PassLight = Convert.ToBoolean(dr["passlight"]),
                                    Speedometer = dr["speedometer"].ToString(),
                                    Tachometer = Convert.ToBoolean(dr["tachometer"]),
                                    TachometerType = dr["tachometertype"].ToString(),
                                    ShiftLight = Convert.ToBoolean(dr["shiftlight"]),
                                    ElectricStart = Convert.ToBoolean(dr["electricstart"]),
                                    Tripmeter = Convert.ToBoolean(dr["tripmeter"]),
                                    NoOfTripmeters = dr["nooftripmeters"].ToString(),
                                    TripmeterType = dr["tripmetertype"].ToString(),
                                    LowFuelIndicator = Convert.ToBoolean(dr["lowfuelindicator"]),
                                    LowOilIndicator = Convert.ToBoolean(dr["lowoilindicator"]),
                                    LowBatteryIndicator = Convert.ToBoolean(dr["lowbatteryindicator"]),
                                    FuelGauge = Convert.ToBoolean(dr["fuelgauge"]),
                                    DigitalFuelGauge = Convert.ToBoolean(dr["digitalfuelgauge"]),
                                    PillionSeat = Convert.ToBoolean(dr["pillionseat"]),
                                    PillionFootrest = Convert.ToBoolean(dr["pillionfootrest"]),
                                    PillionBackrest = Convert.ToBoolean(dr["pillionbackrest"]),
                                    PillionGrabrail = Convert.ToBoolean(dr["PillionGrabrail"]),
                                    StandAlarm = Convert.ToBoolean(dr["standalarm"]),
                                    SteppedSeat = Convert.ToBoolean(dr["SteppedSeat"]),
                                    AntilockBrakingSystem = Convert.ToBoolean(dr["antilockbrakingsystem"]),
                                    Killswitch = Convert.ToBoolean(dr["killswitch"]),
                                    Clock = Convert.ToBoolean(dr["clock"]),
                                    MaxPowerRPM = Convert.ToSingle(dr["maxpowerrpm"]),
                                    MaximumTorqueRPM = Convert.ToSingle(dr["maximumtorquerpm"]),
                                    Colors = dr["colors"].ToString()
                                });
                            dr.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, string.Format("BikeVersionsRepository<T, U>.GetModelSpecifications()=> modelId: {0}", modelId));
                objErr.SendMail();
            }
            return objMinspecs;
        }

    }   // class
}   // namespace
