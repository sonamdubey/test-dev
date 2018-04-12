using Bikewale.Entities;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.BikeData.NewLaunched;
using Bikewale.Entities.CMS.Photos;
using Bikewale.Entities.GenericBikes;
using Bikewale.Entities.Location;
using Bikewale.Entities.SEO;
using Bikewale.Entities.UserReviews;
using Bikewale.Interfaces.BikeData;
using Bikewale.Notifications;
using Bikewale.Utility;
using MySql.CoreDAL;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Bikewale.DAL.BikeData
{
    /// <summary>
    /// Created By : Ashish G. Kamble
    /// Modified By : Sushil Kumar on 5th Jan 2016
    /// Description : To get similar bikes with photos count
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
                ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);

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


        /// <summary>
        /// Modified By : Sushil Kumar on 21st jan 2016
        /// Description : Moved Model color logic to BAL to process multitone colors with linq
        /// Modified By : Lucky Rathore on 18th Apr 2016
        /// Description : validation modelPage.ModelDetails and modelPage.ModelDesc added. 
        /// Modified by : Aditi Srivastava on 31 May 2017
        /// Summary     : Moved GetModelColors function outside ModelVersions condition
        /// Modified by : Ashutosh Sharma on 26-Sep-2017
        /// Description : Added condition to get futuristic version min specs for upcoming bikes.
        /// </summary>
        /// <param name="modelId"></param>
        /// <returns></returns>
        public BikeModelPageEntity GetModelPage(U modelId, int versionId)
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
                // If bike is upcoming Bike get the upcoming bike data and version min specs for futuristic bike
                if (modelPage.ModelDetails.Futuristic)
                {
                    modelPage.UpcomingBike = GetUpcomingBikeDetails(modelId);
                    modelPage.ModelVersions = GetFuturisticVersions(modelId).ToList();
                }
                else
                {
                    // Get model min specs
                    modelPage.ModelVersions = GetVersions(modelId, modelPage.ModelDetails.New);
                }
                modelPage.ModelColors = GetModelColor(modelId);
                modelPage.colorPhotos = GetModelColorPhotos(modelId);
                
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);
            }
            return modelPage;
        }

        /// <summary>
        /// Modified By : Sajal Gupta on 01-03-2017
        /// Description : Changed sp to get modelcolor image id .
        /// </summary>
        /// <param name="modelId"></param>
        /// <returns></returns>
        public IEnumerable<NewBikeModelColor> GetModelColor(U modelId)
        {
            List<BikeModelColor> colors = null;
            List<NewBikeModelColor> objMultiToneColor = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("getmodelcolor_01032017"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

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
                                        ColorImageId = Convert.ToUInt32(dr["colorimageid"]),
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

                        ICollection<string> HexCodeList = new List<string>();
                        foreach (var colorList in color)
                        {
                            tempColor.ColorName = colorList.ColorName;
                            tempColor.ModelId = colorList.ModelId;
                            tempColor.ColorImageId = colorList.ColorImageId;
                            HexCodeList.Add(colorList.HexCode.Trim());
                        }

                        tempColor.HexCodes = HexCodeList;
                        objMultiToneColor.Add(tempColor);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);
            }
            return objMultiToneColor;
        }

        /// <summary>
        /// Versions list with Min specs
        /// Modified by : Ashutosh Sharma on 30 Aug 2017 
        /// Description : Changed SP from 'getversions_23082017' to 'getversions_30082017', removed IsGstPrice flag
        /// Modified by : Ashutosh Sharma on 29 Sep 2017 
        /// Description : Changed SP from 'getversions_30082017' to 'getversions_29092017', to get avg price.
        /// Modified by : Ashutosh Sharma on 05 Apr 2018.
        /// Description : Changed sp from 'getversions_08112017' to 'getversions_05042018' to remove min specs.
        /// </summary>
        /// <param name="modelId">model id</param>
        /// <param name="isNew">is new</param>
        /// <returns></returns>
        public List<BikeVersionMinSpecs> GetVersions(U modelId, bool isNew)
        {

            List<BikeVersionMinSpecs> objMinSpecs = new List<BikeVersionMinSpecs>();
            try
            {

                using (DbCommand cmd = DbFactory.GetDBCommand("getversions_05042018"))
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
                                    VersionId = SqlReaderConvertor.ToInt32(dr["ID"]),
                                    VersionName = Convert.ToString(dr["Version"]),
                                    ModelName = Convert.ToString(dr["Model"]),
                                    Price = SqlReaderConvertor.ToUInt64(dr["VersionPrice"]),
                                    AverageExShowroom = SqlReaderConvertor.ToUInt32(dr["AverageExShowroom"]),
                                    BodyStyle = (EnumBikeBodyStyles)SqlReaderConvertor.ToUInt16(dr["BodyStyleId"]),
                                    HostUrl = Convert.ToString(dr["HostURL"]),
                                    OriginalImagePath = Convert.ToString(dr["OriginalImagePath"]),
                                    FuelType = Convert.ToString(dr["BikeFuelType"])
                                });
                            }
                            dr.Close();
                        }

                    }
                }

            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);

            }


            return objMinSpecs;
        }

        /// <summary>
        /// Created by : Ashutosh Sharma on 26-Sep-2017
        /// Description : DAL method to get futuristice version of bike model
        /// Modified by : Rajan Chauhan on 26 Mar 2018
        /// Description : Removed MinSpecs mapping from BikeVersionMinSpecs
        /// </summary>
        /// <param name="modelId"></param>
        /// <returns></returns>
        public IEnumerable<BikeVersionMinSpecs> GetFuturisticVersions(U modelId)
        {

            ICollection<BikeVersionMinSpecs> objMinSpecs = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("getfuturisticversions_05042018"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_modelid", DbType.Int32, modelId));
                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            objMinSpecs = new Collection<BikeVersionMinSpecs>();
                            BikeVersionMinSpecs objBikeVersion;
                            while (dr.Read())
                            {
                                objBikeVersion = new BikeVersionMinSpecs
                                {
                                    VersionId = SqlReaderConvertor.ToInt32(dr["VersionId"]),
                                    VersionName = Convert.ToString(dr["VersionName"]),
                                    ModelName = Convert.ToString(dr["ModelName"]),
                                    Price = SqlReaderConvertor.ToUInt32(dr["Price"]),
                                    HostUrl = Convert.ToString(dr["HostURL"]),
                                    OriginalImagePath = Convert.ToString(dr["OriginalImagePath"]),
                                    BodyStyle = (EnumBikeBodyStyles)SqlReaderConvertor.ToInt32(dr["BodyStyle"])
                                };
                                objMinSpecs.Add(objBikeVersion);
                            }
                            dr.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("Bikewale.DAL.BikeData.BikeModelsRepository.GetFuturisticVersionMinSpecs_{0}", modelId));
            }
            return objMinSpecs;
        }
        [Obsolete("Use Specification and Features Micro Service to get all specs.", true)]
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
        /// Summary: Used a different sp(earlier getmodeldetails_new) to retrieve model details using data reader'
        /// Modified by sajal gupta on 19/05/2017
        /// Description : Added ratings count
        /// Modified by : Ashutosh Sharma on 30 Aug 2017 
        /// Description : Changed SP from 'getmodeldetails_new_28062017' to 'getmodeldetails_new_30082017', removed IsGstPrice flag
        /// Modified By :Snehal Dange on 12 Oct 2017
        /// Description : Changed Sp from 'getmodeldetails_new_30082017' to 'getmodeldetails_new_12102017' added IsScooterOnly flag
        /// Modified by : Ashutosh Sharma on 23 Oct 2017 
        /// Description : Changed SP from 'getmodeldetails_new_12102017' to 'getmodeldetails_new_23102017'
        /// Modified by : Rajan Chauhan on 06 Feb 2018 
        /// Description : Changed SP from 'getmodeldetails_new_23102017' to 'getmodeldetails_new_06022018' added Field NewsCount
        /// </summary>
        /// <param name="id">Model Id should be a positive number.</param>
        /// <returns>Returns object containing the particular model's all details.</returns>
        public T GetById(U id)
        {
            T t = default(T);
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("getmodeldetails_new_06022018"))
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
                                t.MakeBase.IsScooterOnly = SqlReaderConvertor.ToBoolean(dr["IsScooterOnly"]);
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
                                t.ModelSeries.SeriesId = SqlReaderConvertor.ToUInt32(dr["bikeseriesid"]);
                                t.ModelSeries.SeriesName = Convert.ToString(dr["bikeseriesname"]);
                                t.ModelSeries.MaskingName = Convert.ToString(dr["bikeseriesmaskingname"]);
                                t.ModelSeries.IsSeriesPageUrl = SqlReaderConvertor.ToBoolean(dr["IsSeriesPageUrl"]);
                                t.ReviewCount = Convert.ToInt32(dr["ReviewCount"]);
                                t.RatingCount = SqlReaderConvertor.ToInt32(dr["RatingsCount"]);
                                t.ReviewRate = Convert.ToDouble(dr["ReviewRate"]);
                                t.ReviewUIRating = string.Format("{0:0.0}", t.ReviewRate);
                                t.OriginalImagePath = Convert.ToString(dr["OriginalImagePath"]);
                                t.PhotosCount = Convert.ToInt32(dr["PhotosCount"]);
                                t.VideosCount = Convert.ToInt32(dr["VideosCount"]);
                                t.UsedListingsCnt = Convert.ToUInt32(dr["UsedListingsCnt"]);
                                t.ExpertReviewsCount = SqlReaderConvertor.ToUInt32(dr["ExpertReviewsCount"]);
                                t.NewsCount = SqlReaderConvertor.ToUInt32(dr["NewsCount"]);
                            }

                            if (dr.NextResult())
                            {
                                var metas = new List<CustomPageMetas>();
                                while (dr.Read())
                                {
                                    var meta = new CustomPageMetas();
                                    meta.PageId = SqlReaderConvertor.ToUInt32(dr["pageid"]);
                                    meta.Title = Convert.ToString(dr["title"]);
                                    meta.Description = Convert.ToString(dr["description"]);
                                    meta.Keywords = Convert.ToString(dr["keywords"]);
                                    meta.Heading = Convert.ToString(dr["heading"]);
                                    meta.Summary = Convert.ToString(dr["summary"]);
                                    meta.ModelId = (uint)t.ModelId;
                                    metas.Add(meta);
                                }
                                t.Metas = metas;
                            }

                            dr.Close();
                        }
                    }
                }
            }
            catch (Exception err)
            {

                ErrorClass.LogError(err, HttpContext.Current.Request.ServerVariables["URL"]);

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

            catch (Exception ex)
            {

                ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);

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
                    cmd.CommandType = CommandType.StoredProcedure;
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
                ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);

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
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_modelid", DbType.Int32, modelId));

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null && dr.Read())
                        {
                            objModel = new UpcomingBikeEntity()
                            {
                                ExpectedLaunchId = Convert.ToUInt16(dr["id"]),
                                ExpectedLaunchDate = !String.IsNullOrEmpty(Convert.ToString(dr["ExpectedLaunch"])) ? Convert.ToDateTime(dr["ExpectedLaunch"]).ToString("MMMM yyyy") : "",
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
                ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);

            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("GetUpcomingBikeDetails ex : " + ex.Message + ex.Source);
                ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);

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

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_startindex", DbType.Int32, inputParams.PageNo));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_endindex", DbType.Int32, inputParams.PageSize));
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
                                objModel.ExpectedLaunchDate = !String.IsNullOrEmpty(Convert.ToString(dr["ExpectedLaunch"])) ? Convert.ToDateTime(dr["ExpectedLaunch"]).ToString("MMMM yyyy") : "";
                                objModel.ExpectedLaunchedDate = Convert.ToDateTime(dr["ExpectedLaunch"]);
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
                ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);

            }

            return objModelList;
        }

        /// <summary>
        /// Created By : Sadhana Upadhyay on 4 June 2014
        /// Summary : to get all recently launched bikes
        /// Modified By : Rajan Chauhan on 3 Apr 2014
        /// Description : Added VersionId in NewLaunchedBikeEntity objModel
        /// </summary>
        /// <param name="startIndex"></param>
        /// <param name="endIndex"></param>
        /// <param name="recordCount"></param>
        /// <returns></returns>
        public NewLaunchedBikesBase GetNewLaunchedBikesList(int startIndex, int endIndex)
        {
            NewLaunchedBikesBase newLaunchedBikes = new NewLaunchedBikesBase();
            int recordCount = 0;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("getnewlaunchedbikes_10042018"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_startindex", DbType.Int32, startIndex));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_endindex", DbType.Int32, endIndex));

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            ICollection<NewLaunchedBikeEntity> objModelList = new Collection<NewLaunchedBikeEntity>();

                            while (dr.Read())
                            {
                                NewLaunchedBikeEntity objModels = new NewLaunchedBikeEntity();
                                objModels.BikeLaunchId = SqlReaderConvertor.ToUInt16(dr["BikeLaunchId"]);
                                objModels.MakeBase.MakeId = SqlReaderConvertor.ToInt32(dr["BikeMakeId"]);
                                objModels.MakeBase.MakeName = Convert.ToString(dr["Make"]);
                                objModels.MakeBase.MaskingName = Convert.ToString(dr["MakeMaskingName"]);
                                objModels.ModelId = SqlReaderConvertor.ToInt32(dr["ModelId"]);
                                objModels.ModelName = Convert.ToString(dr["Model"]);
                                objModels.MaskingName = Convert.ToString(dr["ModelMaskingName"]);
                                objModels.HostUrl = Convert.ToString(dr["HostURL"]);
                                objModels.LargePicUrl = Convert.ToString(dr["LargePic"]);
                                objModels.SmallPicUrl = Convert.ToString(dr["SmallPic"]);
                                objModels.ReviewCount = SqlReaderConvertor.ToInt16(dr["ReviewCount"]);
                                objModels.ReviewRate = SqlReaderConvertor.ParseToDouble(dr["ReviewRate"]);
                                objModels.ReviewUIRating = string.Format("{0:0.0}", objModels.ReviewRate);
                                objModels.MinPrice = SqlReaderConvertor.ToInt64(dr["MinPrice"]);
                                objModels.MaxPrice = SqlReaderConvertor.ToInt64(dr["MaxPrice"]);
                                objModels.LaunchDate = SqlReaderConvertor.ToDateTime(dr["LaunchDate"]);
                                objModels.OriginalImagePath = Convert.ToString(dr["OriginalImagePath"]);
                                objModels.VersionId = SqlReaderConvertor.ToInt32(dr["TopVersionId"]);
                                objModelList.Add(objModels);

                            }
                            if (dr.NextResult() && dr.Read())
                            {
                                recordCount = Convert.ToInt32(dr["RecordCount"]);
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
                ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);

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
                using (DbCommand cmd = DbFactory.GetDBCommand("getnewlaunchedbikesbymake_04042018"))
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
                                objModels.BikeLaunchId = SqlReaderConvertor.ToUInt16(dr["BikeLaunchId"]);
                                objModels.MakeBase.MakeId = SqlReaderConvertor.ToInt32(dr["BikeMakeId"]);
                                objModels.MakeBase.MakeName = Convert.ToString(dr["Make"]);
                                objModels.MakeBase.MaskingName = Convert.ToString(dr["MakeMaskingName"]);
                                objModels.ModelId = SqlReaderConvertor.ToInt32(dr["ModelId"]);
                                objModels.ModelName = Convert.ToString(dr["Model"]);
                                objModels.MaskingName = Convert.ToString(dr["ModelMaskingName"]);
                                objModels.HostUrl = Convert.ToString(dr["HostURL"]);
                                objModels.LargePicUrl = Convert.ToString(dr["LargePic"]);
                                objModels.SmallPicUrl = Convert.ToString(dr["SmallPic"]);
                                objModels.ReviewCount = SqlReaderConvertor.ToInt16(dr["ReviewCount"]);
                                objModels.ReviewRate = SqlReaderConvertor.ParseToDouble(dr["ReviewRate"]);
                                objModels.ReviewUIRating = string.Format("{0:0.0}", objModels.ReviewRate);
                                objModels.MinPrice = SqlReaderConvertor.ToInt64(dr["MinPrice"]);
                                objModels.MaxPrice = SqlReaderConvertor.ToInt64(dr["MaxPrice"]);
                                objModels.LaunchDate = SqlReaderConvertor.ToDateTime(dr["LaunchDate"]);
                                objModels.OriginalImagePath = Convert.ToString(dr["OriginalImagePath"]);
                                objModels.VersionId = SqlReaderConvertor.ToInt32(dr["VersionId"]);
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
                ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);

            }
            return newLaunchedBikes;
        }

        /// <summary>
        /// Modified by : Ashutosh Sharma on 07 Apr 2018.
        /// Description : Changed sp from 'getmostpopularbikes' to 'getmostpopularbikes_07042018' to remove min specs.
        /// </summary>
        /// <param name="topCount"></param>
        /// <param name="makeId"></param>
        /// <returns></returns>
        public IEnumerable<MostPopularBikesBase> GetMostPopularBikes(int? topCount = null, int? makeId = null)
        {
            ICollection<MostPopularBikesBase> objList = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("getmostpopularbikes_07042018"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_topcount", DbType.Int16, topCount));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_makeid", DbType.Int32, makeId.HasValue && makeId.Value > 0 ? makeId : Convert.DBNull));


                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            objList = new Collection<MostPopularBikesBase>();
                            MostPopularBikesBase objData = null;
                            while (dr.Read())
                            {
                                objData = new MostPopularBikesBase();
                                objData.objMake = new BikeMakeEntityBase();
                                objData.objModel = new BikeModelEntityBase();
                                objData.objVersion = new BikeVersionsListEntity();
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
                ErrorClass.LogError(err, HttpContext.Current.Request.ServerVariables["URL"]);
            }
            return objList;
        }

        /// <summary>
        /// Modified by : Ashutosh Sharma on 07 Apr 2018.
        /// Description : Changed sp from 'getmostpopularbikesbymake_04082016' to 'getmostpopularbikesbymake_07042018' to remove min specs.
        /// </summary>
        /// <param name="makeId"></param>
        /// <returns></returns>
        public IEnumerable<MostPopularBikesBase> GetMostPopularBikesByMake(int makeId)
        {
            ICollection<MostPopularBikesBase> objList = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("getmostpopularbikesbymake_07042018"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_makeid", DbType.Int32, makeId));

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            objList = new Collection<MostPopularBikesBase>();
                            MostPopularBikesBase objData = null;
                            while (dr.Read())
                            {
                                objData = new MostPopularBikesBase();
                                objData.objMake = new BikeMakeEntityBase();
                                objData.objModel = new BikeModelEntityBase();
                                objData.objVersion = new BikeVersionsListEntity();
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
                                objData.BikePopularityIndex = Convert.ToUInt16(dr["PopularityIndex"]);
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
                ErrorClass.LogError(err, HttpContext.Current.Request.ServerVariables["URL"]);

            }
            return objList;
        }

        /// <summary>
        /// Created by : Ashutosh Sharma on 29 Sep 2017 
        /// Description : DAL method to get most popular bikes by make with city price when city is selected.
        /// Modified by : Snehal Dange on 6th Feb 2018 
        /// Description : Added parameter for on-road price and modified sp from 'getmostpopularbikesbymakewithcityprice' to 'getmostpopularbikesbymakewithcityprice_06022018'
        /// Description : Added parameter for 'cityname' and modified sp from 'getmostpopularbikesbymakewithcityprice_06022018' to 'getmostpopularbikesbymakewithcityprice_01032018'.
        /// Modified by : Ashutosh Sharma on 05 Apr 2018.
        /// Description : Changed sp from 'getmostpopularbikesbymakewithcityprice_01032018' to 'getmostpopularbikesbymakewithcityprice_05042018' to remove min specs.
        /// </summary>
        /// <param name="makeId"></param>
        /// <param name="cityId"></param>
        /// <returns></returns>
        public IEnumerable<MostPopularBikesBase> GetMostPopularBikesByMakeWithCityPrice(int makeId, uint cityId)
        {
            ICollection<MostPopularBikesBase> objList = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("getmostpopularbikesbymakewithcityprice_05042018"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_makeid", DbType.Int32, makeId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_cityid", DbType.Int32, cityId));

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            objList = new Collection<MostPopularBikesBase>();
                            MostPopularBikesBase objData = null;
                            while (dr.Read())
                            {
                                objData = new MostPopularBikesBase();
                                objData.objMake = new BikeMakeEntityBase();
                                objData.objModel = new BikeModelEntityBase();
                                objData.objVersion = new BikeVersionsListEntity();
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
                                objData.AvgPrice = SqlReaderConvertor.ToInt64(dr["AvgVersionPrice"]);
                                objData.ExShowroomPrice = SqlReaderConvertor.ToInt64(dr["ExShowroomPrice"]);
                                objData.BikePopularityIndex = Convert.ToUInt16(dr["PopularityIndex"]);
                                objData.OnRoadPrice = SqlReaderConvertor.ToInt64(dr["OnRoad"]);
                                objData.OnRoadPriceMumbai = SqlReaderConvertor.ToInt64(dr["OnRoadMumbai"]);
                                objData.CityName = Convert.ToString(dr["cityname"]);
                                objList.Add(objData);
                            }
                            dr.Close();
                        }
                    }
                }
            }
            catch (Exception err)
            {
                ErrorClass.LogError(err, string.Format("Bikewale.DAL.BikeData.BikeModelsRepository_GetMostPopularBikesByMakeWithCityPrice_{0}_{1}", makeId, cityId));
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
                using (DbCommand cmd = DbFactory.GetDBCommand("sp_getmodelmappingnames_08122017"))
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

            catch (Exception ex)
            {

                ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);

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
                    cmd.CommandText = "getoldmaskingnameslist_08122017";
                    cmd.CommandType = CommandType.StoredProcedure;

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            ht = new Hashtable();


                            while (dr.Read())
                            {
                                if (!ht.ContainsKey(dr["OldMaskingName"]))
                                    ht.Add(dr["OldMaskingName"], dr["NewMaskingName"]);
                            }

                            dr.Close();
                        }
                    }
                }
            }


            catch (Exception ex)
            {

                ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);

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
                ErrorClass.LogError(ex, "Sql Exception : Bikewale.DAL.BikeModelRepository.GetFeaturedBikes");

            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Exception : Bikewale.DAL.BikeModelRepository.GetFeaturedBikes");

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
                ErrorClass.LogError(ex, "Exception : Bikewale.DAL.BikeModelRepository.GetFeaturedBikes");

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
        public ModelHostImagePath GetModelPhotoInfo(U modelId)
        {
            ModelHostImagePath modelPhotos = null;
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
                            if (dr.Read())
                            {
                                modelPhotos = new ModelHostImagePath();
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
                ErrorClass.LogError(err, HttpContext.Current.Request.ServerVariables["URL"]);
            }
            return modelPhotos;
        }
        /// <summary>
        /// Created by Subodh Jain on 22 sep 2016
        /// Des:- To fetch most popular bikes on make and city
        /// Modified By : Sushil Kumar on 10th Nov 2016
        /// Description : Get cityname with other info
        /// Modified by : Ashutosh Sharma on 04 Apr 2018.
        /// Description : Changed sp from 'getmostpopularbikesbymakecity' to 'getmostpopularbikesbymakecity_04042018'.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<MostPopularBikesBase> GetMostPopularBikesbyMakeCity(uint topCount, uint makeId, uint cityId)
        {
            IList<MostPopularBikesBase> objList = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("getmostpopularbikesbymakecity_04042018"))
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
                            MostPopularBikesBase objData = null;
                            while (dr.Read())
                            {
                                objData = new MostPopularBikesBase();
                                objData.objMake = new BikeMakeEntityBase();
                                objData.objModel = new BikeModelEntityBase();
                                objData.objVersion = new BikeVersionsListEntity();
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
                                objData.CityName = Convert.ToString(dr["cityname"]);
                                objData.CityMaskingName = Convert.ToString(dr["citymasking"]);
                                objList.Add(objData);
                            }
                            dr.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "BikeModelsRepository.GetMostPopularBikesbymakecity");
            }
            return objList;
        }
        /// <summary>
        /// Created By : Sangram Nandkhile on 01 Dec 2016
        /// Summary : Function to get all specifications of all the versions of ModelId
        /// Modified by : Aditi Srivastava on 18 May 2017
        /// Summary     : Changed data types of specs and features from bool to nullable bool used a versioned sp
        /// </summary>
        public IEnumerable<BikeSpecificationEntity> GetModelSpecifications(U modelId)
        {
            List<BikeSpecificationEntity> objMinspecs = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("getbikemodelspecifications_18052017"))
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
                                    Colors = dr["colors"].ToString()
                                });
                            dr.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("BikeVersionsRepository<T, U>.GetModelSpecifications()=> modelId: {0}", modelId));

            }
            return objMinspecs;
        }

        /// <summary>
        /// Created By:Snehal Dange on 8th Sep 2017
        /// Description :Method which calls proper Sp based on availibility of City
        /// </summary>
        /// <param name="modelId"></param>
        /// <param name="totalRecords"></param>
        /// <param name="cityId"></param>
        /// <param name="sP"></param>
        /// <returns></returns>

        private IEnumerable<SimilarBikesWithPhotos> GetSimilarBikesWithPhotosCount(U modelId, ushort totalRecords, uint cityId, string sP)
        {
            IList<SimilarBikesWithPhotos> SimilarBikeInfoList = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = sP;
                    if (cityId > 0)
                    {
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_cityid", DbType.Int32, cityId));
                    }

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_modelid", DbType.Int32, modelId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_topcount", DbType.Int16, totalRecords));
                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            SimilarBikeInfoList = new List<SimilarBikesWithPhotos>();

                            while (dr.Read())
                            {
                                var bikeInfo = new SimilarBikesWithPhotos();
                                bikeInfo.Make = new Entities.BikeData.BikeMakeEntityBase();
                                bikeInfo.Model = new BikeModelEntityBase();
                                bikeInfo.OriginalImagePath = Convert.ToString(dr["originalimagepath"]);
                                bikeInfo.HostUrl = Convert.ToString(dr["hosturl"]);
                                bikeInfo.PhotosCount = SqlReaderConvertor.ToUInt32(dr["photoscount"]);
                                bikeInfo.Make.MakeName = Convert.ToString(dr["makename"]);
                                bikeInfo.Make.MaskingName = Convert.ToString(dr["makemaskingname"]);
                                bikeInfo.Model.ModelName = Convert.ToString(dr["modelname"]);
                                bikeInfo.Model.MaskingName = Convert.ToString(dr["modelmaskingname"]);
                                bikeInfo.ExShowroomPriceMumbai = SqlReaderConvertor.ToUInt32(dr["exshowroompricemumbai"]);
                                bikeInfo.BodyStyle = (sbyte)SqlReaderConvertor.ToUInt16(dr["bodystyleid"]);
                                if (cityId > 0)
                                {
                                    bikeInfo.OnRoadPriceInCity = SqlReaderConvertor.ToUInt32(dr["onroadpriceincity"]);
                                }
                                SimilarBikeInfoList.Add(bikeInfo);

                            }
                            dr.Close();
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.DAL.BikeData.GetSimilarBikesWithPhotosCount_model" + modelId);
            }
            return SimilarBikeInfoList;

        }

        /// <summary>
        /// Modified By : Sushil Kumar on 5th Jan 2016
        /// Description : To get similar bikes with photos count
        /// Modified By : Snehal Dange on 6th Sep 2017
        /// Description : Added ExShowroomPrice
        /// </summary>
        /// <param name="modelId"></param>
        /// <returns></returns>
        public IEnumerable<SimilarBikesWithPhotos> GetAlternativeBikesWithPhotos(U modelId, ushort totalRecords)
        {
            IEnumerable<SimilarBikesWithPhotos> SimilarBikeInfoList = null;
            try
            {
                SimilarBikeInfoList = GetSimilarBikesWithPhotosCount(modelId, totalRecords, 0, "getsimilarbikeswithphotoscount_06092017");

            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.DAL.BikeData.GetAlternativeBikesWithPhotos_Model" + modelId);
            }
            return SimilarBikeInfoList;
        }


        /// <summary>
        /// Created By : Snehal Dange on 6th September 2017
        /// Description : To get similar bikes with photos count in city
        /// </summary>
        /// <param name="modelId"></param>
        /// <returns></returns>
        public IEnumerable<SimilarBikesWithPhotos> GetAlternativeBikesWithPhotosInCity(U modelId, ushort totalRecords, uint cityId)
        {
            {
                IEnumerable<SimilarBikesWithPhotos> SimilarBikeInfoList = null;
                try
                {
                    SimilarBikeInfoList = GetSimilarBikesWithPhotosCount(modelId, totalRecords, cityId, "getsimilarbikeswithphotoscountbycity");
                }
                catch (Exception ex)
                {
                    ErrorClass.LogError(ex, string.Format("Bikewale.DAL.BikeData.GetAlternativeBikesWithPhotosInCity_Model_{0}_City_{1}", modelId, cityId));
                }
                return SimilarBikeInfoList;
            }
        }

        /// <summary>
        /// Modified By : Suresh Prajapati on 22 Aug 2014
        /// Summary : to retrieve isnew and isused flag
        /// Modified By :- Subodh Jain 17 Jan 2017
        /// Summary :- shift function to dal layer
        /// Modified by : Ashutosh Sharma on 07 Apr 2018.
        /// Description : Changed sp from 'getmodeldescription_24042017' to 'getmodeldescription_07042018' to remove min specs.
        /// </summary>
        /// <param name="modelId"></param>
        public ReviewDetailsEntity GetDetailsByModel(U modelId, uint cityId)
        {
            ReviewDetailsEntity objReview = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("getmodeldescription_07042018"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_modelid", DbType.Int32, modelId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_cityid", DbType.Int32, cityId));

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null && dr.Read())
                        {
                            objReview = new ReviewDetailsEntity();
                            objReview.BikeEntity.MakeEntity.MakeId = SqlReaderConvertor.ToInt32(dr["makeid"]);
                            objReview.BikeEntity.MakeEntity.MakeName = Convert.ToString(dr["MakeName"]);
                            objReview.BikeEntity.ModelEntity.ModelName = Convert.ToString(dr["ModelName"]);
                            objReview.BikeEntity.ModelEntity.MaskingName = Convert.ToString(dr["ModelMaskingName"]);
                            objReview.BikeEntity.MakeEntity.MaskingName = Convert.ToString(dr["MakeMaskingName"]);
                            objReview.HostUrl = Convert.ToString(dr["HostURL"]);
                            objReview.LargePicUrl = Convert.ToString(dr["LargePic"]);
                            objReview.ReviewRatingEntity.ModelRatingLooks = SqlReaderConvertor.ToFloat(dr["Looks"]);
                            objReview.ReviewRatingEntity.PerformanceRating = SqlReaderConvertor.ToFloat(dr["Performance"]);
                            objReview.ReviewRatingEntity.ComfortRating = SqlReaderConvertor.ToFloat(dr["Comfort"]);
                            objReview.ReviewRatingEntity.ValueRating = SqlReaderConvertor.ToFloat(dr["ValueForMoney"]);
                            objReview.ReviewRatingEntity.FuelEconomyRating = SqlReaderConvertor.ToFloat(dr["FuelEconomy"]);
                            objReview.ReviewRatingEntity.OverAllRating = SqlReaderConvertor.ToFloat(dr["ReviewRate"]);
                            objReview.BikeEntity.ReviewsCount = Convert.ToUInt32(dr["ReviewCount"]);

                            objReview.IsFuturistic = Convert.ToBoolean(dr["Futuristic"]);
                            objReview.New = Convert.ToBoolean(dr["New"]);
                            objReview.Used = Convert.ToBoolean(dr["Used"]);
                            objReview.ReviewRatingEntity.IsReviewAvailable = Convert.ToBoolean(dr["isReviewAvailable"]);
                            objReview.ModelBasePrice = Convert.ToString(dr["MinPrice"]);
                            objReview.ModelHighendPrice = Convert.ToString(dr["MaxPrice"]);
                            objReview.OriginalImagePath = Convert.ToString(dr["OriginalImagePath"]);
                            dr.Close();
                        }
                    }
                }

            }
            catch (Exception err)
            {
                ErrorClass.LogError(err, string.Format("ModelVersionDescription.GetDetailsByModel_modelid_{0}", modelId));
            }
            return objReview;
        }
        /// <summary>
        /// Modified By :- Subodh Jain on 17 Jan 2017
        /// Summary :- shifted function to dal layer
        /// Modified by : Ashutosh Sharma on 07 Apr 2018.
        /// Description : Changed sp from 'getversiondescription_13012017' to 'getversiondescription_13012017' to remove min specs.
        /// </summary>
        /// <param name="versionId"></param>
        /// <param name="cityId"></param>
        /// <returns></returns>
        public ReviewDetailsEntity GetDetailsByVersion(U versionId, uint cityId)
        {
            ReviewDetailsEntity objReview = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("getversiondescription_13012017"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_modelid", DbType.Int32, versionId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_cityid", DbType.Int32, cityId));

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null && dr.Read())
                        {
                            objReview = new ReviewDetailsEntity();
                            objReview.BikeEntity.MakeEntity.MakeId = SqlReaderConvertor.ToInt32(dr["makeid"]);
                            objReview.BikeEntity.MakeEntity.MakeName = Convert.ToString(dr["MakeName"]);
                            objReview.BikeEntity.ModelEntity.ModelName = Convert.ToString(dr["ModelName"]);
                            objReview.BikeEntity.ModelEntity.MaskingName = Convert.ToString(dr["ModelMaskingName"]);
                            objReview.BikeEntity.MakeEntity.MaskingName = Convert.ToString(dr["MakeMaskingName"]);
                            objReview.HostUrl = Convert.ToString(dr["HostURL"]);
                            objReview.LargePicUrl = Convert.ToString(dr["LargePic"]);
                            objReview.ReviewRatingEntity.ModelRatingLooks = SqlReaderConvertor.ToFloat(dr["Looks"]);
                            objReview.ReviewRatingEntity.PerformanceRating = SqlReaderConvertor.ToFloat(dr["Performance"]);
                            objReview.ReviewRatingEntity.ComfortRating = SqlReaderConvertor.ToFloat(dr["Comfort"]);
                            objReview.ReviewRatingEntity.ValueRating = SqlReaderConvertor.ToFloat(dr["ValueForMoney"]);
                            objReview.ReviewRatingEntity.FuelEconomyRating = SqlReaderConvertor.ToFloat(dr["FuelEconomy"]);
                            objReview.ReviewRatingEntity.OverAllRating = SqlReaderConvertor.ToFloat(dr["ReviewRate"]);
                            objReview.BikeEntity.ReviewsCount = Convert.ToUInt32(dr["ReviewCount"]);
                            objReview.IsFuturistic = Convert.ToBoolean(dr["Futuristic"]);
                            objReview.New = Convert.ToBoolean(dr["New"]);
                            objReview.Used = Convert.ToBoolean(dr["Used"]);
                            objReview.ModelBasePrice = Convert.ToString(dr["MinPrice"]);
                            objReview.ModelHighendPrice = Convert.ToString(dr["MaxPrice"]);
                            objReview.OriginalImagePath = Convert.ToString(dr["OriginalImagePath"]);
                            dr.Close();
                        }
                    }
                }

            }
            catch (Exception err)
            {
                ErrorClass.LogError(err, string.Format("ModelVersionDescription.GetDetailsByModel_modelid_{0}", versionId));
            }
            return objReview;
        }
        /// <summary>
        /// Created by Subodh Jain 17 jan 2017
        /// Desc Get User Review Similar Bike
        /// </summary>
        /// <param name="modelId"></param>
        /// <param name="topCount"></param>
        /// <returns></returns>
        public IEnumerable<BikeUserReviewRating> GetUserReviewSimilarBike(uint modelId, uint topCount)
        {
            IList<BikeUserReviewRating> objUserreview = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("similarbikebyuserreviews"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_modelid", DbType.Int32, modelId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_topcount", DbType.Int32, topCount));

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.MasterDatabase))
                    {
                        if (dr != null)
                        {
                            objUserreview = new List<BikeUserReviewRating>();

                            while (dr.Read())
                            {
                                objUserreview.Add(new BikeUserReviewRating()
                                {
                                    OverAllRating = SqlReaderConvertor.ParseToDouble(dr["ReviewRate"]),
                                    ModelMaskingName = Convert.ToString(dr["modelmaskingname"]),
                                    MakeMaksingName = Convert.ToString(dr["makemaskingname"]),
                                    OriginalImagePath = Convert.ToString(dr["originalimagepath"]),
                                    HostUrl = Convert.ToString(dr["hosturl"]),
                                    ReviewCounting = SqlReaderConvertor.ParseToDouble(dr["ReviewCount"]),
                                    ModelName = Convert.ToString(dr["modelname"]),
                                    MakeName = Convert.ToString(dr["makename"])
                                });

                            }
                            dr.Close();
                        }
                    }
                }
            }

            catch (Exception err)
            {

                ErrorClass.LogError(err, string.Format(" ModelVersionDescription.GetUserReviewSimilarBike_modelid_{0}_topcount_{1}", modelId, topCount));

            }
            return objUserreview;
        }
        /// <summary>
        /// Modified By :- Subodh Jain on 17 Jan 2017
        /// Summary :- shifted function to dal layer       
        /// Modified by : Ashutosh Sharma on 07 Apr 2018.
        /// Description : Changed sp from 'getcustomerreviewinfo_12052017' to 'getcustomerreviewinfo_07042018' to remove min specs.
        /// </summary>
        /// <param name="reviewId"></param>
        /// <param name="isAlreadyViewed"></param>
        /// <returns></returns>
        public ReviewDetailsEntity GetDetails(string reviewId, bool isAlreadyViewed)
        {
            string sql = "";
            ReviewDetailsEntity objReview = null;
            try
            {
                if (!string.IsNullOrEmpty(reviewId))
                {
                    using (DbCommand cmd = DbFactory.GetDBCommand())
                    {
                        DbCommand cmd1 = cmd;
                        if (isAlreadyViewed)
                        {
                            //update the viewd count by 1
                            sql = " update customerreviews set viewed = ifnull(viewed, 0) + 1 where id = @v_reviewid";
                            cmd.CommandText = sql;


                            cmd.Parameters.Add(DbFactory.GetDbParam("@v_reviewid", DbType.Int64, reviewId));

                            MySqlDatabase.UpdateQuery(cmd, ConnectionType.MasterDatabase);

                        }

                        cmd1.CommandType = CommandType.StoredProcedure;
                        cmd1.CommandText = "getcustomerreviewinfo_07042018";
                        cmd1.Parameters.Add(DbFactory.GetDbParam("par_reviewid", DbType.Int64, reviewId));


                        using (IDataReader dr = MySqlDatabase.SelectQuery(cmd1, ConnectionType.ReadOnly))
                        {
                            if (dr != null && dr.Read())
                            {
                                objReview = new ReviewDetailsEntity();

                                objReview.BikeEntity.VersionEntity.VersionName = Convert.ToString(dr["Version"]);
                                objReview.BikeEntity.VersionEntity.VersionId = SqlReaderConvertor.ToInt32(dr["versionid"]);
                                objReview.ReviewEntity.Comments = Convert.ToString(dr["Comments"]);
                                objReview.ReviewEntity.Cons = Convert.ToString(dr["Cons"]);
                                objReview.ReviewEntity.Disliked = SqlReaderConvertor.ToUInt16(dr["Disliked"]);
                                objReview.ReviewEntity.Liked = SqlReaderConvertor.ToUInt16(dr["Liked"]);
                                objReview.ReviewEntity.Pros = Convert.ToString(dr["Pros"]);
                                objReview.ReviewEntity.ReviewDate = Convert.ToDateTime(dr["EntryDateTime"]);
                                objReview.ReviewEntity.NewReviewId = SqlReaderConvertor.ToUInt32(dr["NewReviewId"]);
                                objReview.ReviewEntity.ReviewTitle = Convert.ToString(dr["Title"]);
                                objReview.ReviewEntity.WrittenBy = Convert.ToString(dr["CustomerName"]);
                                objReview.ReviewEntity.Viewed = Convert.ToUInt32(dr["viewed"]);
                                objReview.BikeEntity.MakeEntity.MakeId = SqlReaderConvertor.ToInt32(dr["makeid"]);
                                objReview.BikeEntity.MakeEntity.MakeName = Convert.ToString(dr["Make"]);
                                objReview.BikeEntity.ModelEntity.ModelName = Convert.ToString(dr["Model"]);
                                objReview.BikeEntity.ModelEntity.ModelId = SqlReaderConvertor.ToInt32(dr["modelid"]);
                                objReview.BikeEntity.ModelEntity.MaskingName = Convert.ToString(dr["modelmaskingname"]);
                                objReview.BikeEntity.MakeEntity.MaskingName = Convert.ToString(dr["makemaskingname"]);

                                objReview.ReviewRatingEntity.ModelRatingLooks = SqlReaderConvertor.ToFloat(dr["Looks"]);
                                objReview.ReviewRatingEntity.PerformanceRating = SqlReaderConvertor.ToFloat(dr["Performance"]);
                                objReview.ReviewRatingEntity.ComfortRating = SqlReaderConvertor.ToFloat(dr["Comfort"]);
                                objReview.ReviewRatingEntity.ValueRating = SqlReaderConvertor.ToFloat(dr["ValueForMoney"]);
                                objReview.ReviewRatingEntity.FuelEconomyRating = SqlReaderConvertor.ToFloat(dr["FuelEconomy"]);
                                objReview.ReviewRatingEntity.OverAllRating = SqlReaderConvertor.ToFloat(dr["ReviewRate"]);
                                objReview.ModelBasePrice = Convert.ToString(dr["MinPrice"]);
                                objReview.ModelHighendPrice = Convert.ToString(dr["MaxPrice"]);
                                objReview.OriginalImagePath = Convert.ToString(dr["OriginalImagePath"]);
                                objReview.IsFuturistic = Convert.ToBoolean(dr["Futuristic"]);
                                objReview.New = Convert.ToBoolean(dr["new"]);
                                objReview.Used = Convert.ToBoolean(dr["used"]);
                                objReview.HostUrl = Convert.ToString(dr["HostURL"]);
                                dr.Close();
                            }
                        }


                    }
                }

            }
            catch (Exception err)
            {

                ErrorClass.LogError(err, string.Format(" ModelVersionDescription.GetDetails_ReviewId_{0}", reviewId));

            } // catch Exception
            return objReview;
        }

        /// <summary>
        /// Created by : Aditi Srivastava on 25 Jan 2017
        /// summary    : Get details bodystype of a bike model
        /// </summary>
        /// <param name="modelId"></param>
        /// <returns></returns>
        public EnumBikeBodyStyles GetBikeBodyType(uint modelId)
        {
            EnumBikeBodyStyles out_param, bodyStyle = EnumBikeBodyStyles.AllBikes;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "getbikerankingbymodel";
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_modelid", DbType.Int32, modelId));
                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            if (dr.Read())
                            {
                                Enum.TryParse(Convert.ToString(dr["CategoryId"]), out out_param);
                                bodyStyle = out_param;
                            }
                            dr.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format(" GetBikeBodyType_ModelId: {0}", modelId));
            }
            return bodyStyle;
        }


        /// <summary>
        /// Retrieves the Model Colors and Images
        /// Created by: Sangram Nandkhile on 30th Jan 2017
        /// Modified by : Rajan Chauhan on 13 Mar 2017
        /// Description : changed sp from getbikemodelcolor_09012017 to getbikemodelcolor_12032018
        ///               having extra check of isActive on bikemodelcolorPhotos
        /// </summary>
        /// <param name="modelId">Bike Model Id</param>
        /// <returns>Model colorwise Image List</returns>
        public IEnumerable<ModelColorImage> GetModelColorPhotos(U modelId)
        {
            List<ModelColorImage> modelColors = null;

            IList<ColorCodeBase> colorCodes = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("getbikemodelcolor_12032018"))
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
                                    Id = SqlReaderConvertor.ToUInt32(reader["modelColorId"]),
                                    Name = Convert.ToString(reader["ColorName"]),
                                    Host = Convert.ToString(reader["host"]),
                                    OriginalImagePath = Convert.ToString(reader["OriginalImagePath"]),
                                    IsImageExists = SqlReaderConvertor.ToBoolean(reader["IsImageExists"]),
                                    BikeModelColorId = SqlReaderConvertor.ParseToUInt32(reader["BikeModelColorId"]),
                                    ImageCategory = Convert.ToString(reader["ColorName"])
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
                                            Id = SqlReaderConvertor.ToUInt32(reader["ColorId"]),
                                            ModelColorId = SqlReaderConvertor.ToUInt32(reader["modelColorId"]),
                                            IsActive = SqlReaderConvertor.ToBoolean(reader["IsActive"])
                                        });
                                }
                                modelColors.ForEach(
                                    modelColor => modelColor.ColorCodes =
                                        (from colorCode in colorCodes
                                         where colorCode.ModelColorId == modelColor.Id
                                         select colorCode).ToList()
                                    );
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("ManageModelColor.GetModelColorPhotos ==> ModelId {0}", modelId));

            }

            return modelColors;
        }
        /// <summary>
        /// Created by : Aditi Srivastava on 25 Jan 2017
        /// summary    : Get details of top bikes by bodystyle and pricing by city
        /// Modified by : Ashutosh Sharma on 29 Sep 2017 
        /// Description : Changed SP from 'getmostpopularbikesbybodystyle_02022017' to 'getmostpopularbikesbybodystyle_03102017', to get avg price.
        /// </summary>
        /// <param name="bodyStyleId"></param>
        /// <param name="topCount"></param>
        /// <returns></returns>
        public ICollection<MostPopularBikesBase> GetPopularBikesByModelBodyStyle(int modelId, int topCount, uint cityId)
        {
            ICollection<MostPopularBikesBase> popularBikesList = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "getmostpopularbikesbybodystyle_03102017";
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_modelid", DbType.Int32, modelId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_topcount", DbType.Int32, topCount));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_cityid", DbType.Int32, cityId > 0 ? cityId : Convert.DBNull));
                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.MasterDatabase))
                    {
                        if (dr != null)
                        {
                            popularBikesList = new Collection<MostPopularBikesBase>();
                            EnumBikeBodyStyles bodyStyle;
                            while (dr.Read())
                            {
                                MostPopularBikesBase popularObj = new MostPopularBikesBase();
                                popularObj.objModel = new BikeModelEntityBase();
                                popularObj.objMake = new BikeMakeEntityBase();
                                popularObj.objMake.MakeId = SqlReaderConvertor.ToInt32(dr["MakeId"]);
                                popularObj.objMake.MakeName = Convert.ToString(dr["MakeName"]);
                                popularObj.objMake.MaskingName = Convert.ToString(dr["MakeMaskingName"]);
                                popularObj.MakeId = SqlReaderConvertor.ToInt32(dr["MakeId"]);
                                popularObj.MakeName = Convert.ToString(dr["MakeName"]);
                                popularObj.MakeMaskingName = Convert.ToString(dr["MakeMaskingName"]);
                                popularObj.objModel.ModelId = SqlReaderConvertor.ToInt32(dr["ModelId"]);
                                popularObj.objModel.ModelName = Convert.ToString(dr["ModelName"]);
                                popularObj.objModel.MaskingName = Convert.ToString(dr["ModelMaskingName"]);
                                popularObj.OriginalImagePath = Convert.ToString(dr["OriginalImagePath"]);
                                popularObj.HostURL = Convert.ToString(dr["HostURL"]);
                                popularObj.VersionPrice = SqlReaderConvertor.ToInt64(dr["VersionPrice"]);
                                popularObj.AvgPrice = SqlReaderConvertor.ToInt64(dr["AvgPrice"]);
                                popularObj.CityName = Convert.ToString(dr["CityName"]);
                                popularObj.CityMaskingName = Convert.ToString(dr["CityMaskingName"]);
                                popularObj.BikePopularityIndex = SqlReaderConvertor.ToUInt16(dr["Rank"]);
                                Enum.TryParse(Convert.ToString(dr["CategoryId"]), out bodyStyle);
                                popularObj.BodyStyle = bodyStyle;
                                popularBikesList.Add(popularObj);
                            }
                            dr.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format(" GetPopularBikesByBodyStyle_ModelId: {0}, topCount: {1}", modelId, topCount));
            }
            return popularBikesList;
        }
        /// <summary>
        /// Created By : Sushil Kumar on 2nd Jan 2016
        /// Summary :  To get generic bike info by modelid
        /// Modified By : Sushil Kumar on 5th Jan 2016
        /// Description : To get generic bike info with min specs
        /// Modified by : Aditi Srivastava on 23 Jan 2017
        /// Summary     : Added new,ued and futuristic flags and Estimated min and max price(for upcoming)
        /// Modifed by : Snehal Dange on 24th Jan 2018
        /// Summary : changed sp name from "getbikeinfo_18092017" to "getbikeinfo_24012018"
        /// Modified by : Pratibha Verma on 2 April 2018
        /// Description : Replaced sp "getbikeinfo_24012018" with "getbikeinfo_02042018"
        /// </summary>
        /// <returns></returns>
        public GenericBikeInfo GetBikeInfo(uint modelId)
        {
            GenericBikeInfo genericBikeInfo = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "getbikeinfo_02042018";
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_modelid", DbType.Int32, modelId));
                    genericBikeInfo = PopulateGenericBikeInfoEntity(genericBikeInfo, cmd);
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("GenericBikeRepository.GetBikeInfo: ModelId:{0}", modelId));
            }
            return genericBikeInfo;
        }
        /// <summary>
        /// Modified  By :- subodh Jain 10 Feb 2017
        /// Summary :- BikeInfo Slug details
        /// Modified by : Pratibha Verma on 2 April 2018
        /// Description : Removed MinSpecs mapping removed isSpecsAvailable
        /// </summary>
        private GenericBikeInfo PopulateGenericBikeInfoEntity(GenericBikeInfo genericBikeInfo, DbCommand cmd)
        {
            try
            {
                using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                {
                    if (dr != null)
                    {
                        if (dr.Read())
                        {
                            genericBikeInfo = new GenericBikeInfo();
                            genericBikeInfo.Make = new BikeMakeEntityBase();
                            genericBikeInfo.Model = new BikeModelEntityBase();
                            genericBikeInfo.OriginalImagePath = Convert.ToString(dr["originalimagepath"]);
                            genericBikeInfo.HostUrl = Convert.ToString(dr["hosturl"]);
                            genericBikeInfo.VideosCount = SqlReaderConvertor.ToUInt32(dr["videoscount"]);
                            genericBikeInfo.NewsCount = SqlReaderConvertor.ToUInt32(dr["newscount"]);
                            genericBikeInfo.PhotosCount = SqlReaderConvertor.ToUInt32(dr["photoscount"]);
                            genericBikeInfo.ExpertReviewsCount = SqlReaderConvertor.ToUInt32(dr["expertreviewscount"]);
                            genericBikeInfo.FeaturesCount = SqlReaderConvertor.ToUInt32(dr["featurescount"]);
                            genericBikeInfo.BikePrice = SqlReaderConvertor.ToUInt32(dr["price"]);
                            genericBikeInfo.EstimatedPriceMin = SqlReaderConvertor.ToUInt32(dr["EstimatedPriceMin"]);
                            genericBikeInfo.EstimatedPriceMax = SqlReaderConvertor.ToUInt32(dr["EstimatedPriceMax"]);
                            genericBikeInfo.Make.MakeName = Convert.ToString(dr["makename"]);
                            genericBikeInfo.Make.MaskingName = Convert.ToString(dr["makemaskingname"]);
                            genericBikeInfo.Model.ModelName = Convert.ToString(dr["modelname"]);
                            genericBikeInfo.Model.MaskingName = Convert.ToString(dr["modelmaskingname"]);
                            genericBikeInfo.IsUsed = SqlReaderConvertor.ToBoolean(dr["Used"]);
                            genericBikeInfo.IsNew = SqlReaderConvertor.ToBoolean(dr["New"]);
                            genericBikeInfo.IsFuturistic = SqlReaderConvertor.ToBoolean(dr["Futuristic"]);
                            genericBikeInfo.UsedBikeCount = SqlReaderConvertor.ToUInt32(dr["AvailableBikes"]);
                            genericBikeInfo.UsedBikeMinPrice = SqlReaderConvertor.ToUInt32(dr["UsedBikeMinPrice"]);
                            genericBikeInfo.UserReview = SqlReaderConvertor.ToUInt32(dr["ReviewCount"]);
                            genericBikeInfo.DealersCount = SqlReaderConvertor.ToUInt32(dr["dealeravailable"]);
                            genericBikeInfo.PriceInCity = SqlReaderConvertor.ToUInt32(dr["priceincity"]);
                            genericBikeInfo.Rating = Convert.ToSingle(dr["ReviewRate"]);
                            genericBikeInfo.RatingCount = SqlReaderConvertor.ToUInt16(dr["RatingsCount"]);
                            genericBikeInfo.UserReviewCount = SqlReaderConvertor.ToUInt16(dr["ReviewCount"]);
                            genericBikeInfo.BodyStyleId = SqlReaderConvertor.ToInt16(dr["BodyStyleId"]);
                            genericBikeInfo.VersionId = SqlReaderConvertor.ToInt32(dr["TopVersionId"]);
                        }
                        dr.Close();
                    }

                }
                return genericBikeInfo;
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "GenericBikeRepository.PopulateGenericBikeInfoEntity");
            }
            return null;
        }

        /// <summary>
        /// Created By : Sushil Kumar on 2nd Jan 2016
        /// Summary :  To get generic bike info by modelid
        /// Modified By : Sushil Kumar on 5th Jan 2016
        /// Description : To get generic bike info with min specs
        /// Modified by : Aditi Srivastava on 23 Jan 2017
        /// Summary     : Added new,ued and futuristic flags and Estimated min and max price(for upcoming)
        /// Modified By :- subodh jain 9 feb 2017 
        /// summary :- added city id as parameter
        /// Modified By:Snehal Dange on 15th Nov 2017
        /// Summary : Changed sp from getbikeinfobycity_05072017 to getbikeinfobycity_15112017. Changed logic for onRoadPrice
        /// Modified by : Snehal Dange on 24th Jan 2018
        /// Summary : Modified sp from "getbikeinfobycity_15112017" to "getbikeinfobycity_24012018". Added fueltype parameter.
        /// Modified by : Pratibha Verma on 2 April 2018
        /// Description : Replaced sp "getbikeinfobycity_24012018" with "getbikeinfobycity_02042018"
        /// </summary>
        /// <returns></returns>
        public GenericBikeInfo GetBikeInfo(uint modelId, uint cityId)
        {
            GenericBikeInfo genericBikeInfo = null;
            try
            {

                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "getbikeinfobycity_02042018";
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_modelid", DbType.Int32, modelId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_cityid", DbType.Int32, cityId));
                    genericBikeInfo = PopulateGenericBikeInfoEntity(genericBikeInfo, cmd);
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("GenericBikeRepository.GetBikeInfo: ModelId:{0}", modelId));

            }
            return genericBikeInfo;
        }
        /// <summary>
        /// Created By : Aditi Srivastava on 12 Jan 2017
        /// Description : To get bike rankings by category
        /// </summary>
        /// <param name="modelId"></param>
        /// <returns></returns>
        public BikeRankingEntity GetBikeRankingByCategory(uint modelId)
        {
            BikeRankingEntity bikeRankObj = null;
            EnumBikeBodyStyles bodyStyle;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "getbikerankingbymodel_02112017";
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_modelid", DbType.Int32, modelId));
                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            if (dr.Read())
                            {
                                bikeRankObj = new BikeRankingEntity();
                                bikeRankObj.Rank = SqlReaderConvertor.ToInt32(dr["Rank"]);
                                Enum.TryParse(Convert.ToString(dr["CategoryId"]), out bodyStyle);
                                bikeRankObj.BodyStyle = bodyStyle;
                            }
                            dr.Close();
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("GenericBikeRepository.GetBikeRankingByCategory: ModelId:{0}", modelId));
            }
            return bikeRankObj;
        }
        /// <summary>
        /// Created By : Aditi Srivastava on 17 Jan 2017
        /// Description : To get top 10 bikes of a given body style
        /// Modified by : Sajal Gupta on 02-02-2017
        /// Description : Passed cityid to get used bikes count.     
        /// Modified by : Ashutosh Sharma on 29 Sep 2017 
        /// Description : Changed SP from 'getgenericbikelisting_03042017' to 'getgenericbikelisting_02102017', to get avg price.
        /// Modified by : Pratibha Verma on 27 Mar 2018
        /// Description : Removed MinSpecs code
        /// Modified by : Ashutosh Sharma on 05 Apr 2018.
        /// Description : Changed sp from 'getgenericbikelisting_02102017' to 'getgenericbikelisting_05042018' to remove min specs.
        /// </summary>
        /// <param name="bodyStyle"></param>
        /// <returns></returns>
        public ICollection<BestBikeEntityBase> GetBestBikesByCategory(EnumBikeBodyStyles bodyStyle, uint? cityId = null)
        {
            ICollection<BestBikeEntityBase> bestBikesList = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "getgenericbikelisting_05042018";
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_bodystyleid", DbType.Int32, bodyStyle));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_cityid", DbType.Int32, (cityId.HasValue && cityId.Value > 0) ? cityId.Value : Convert.DBNull));

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            bestBikesList = new Collection<BestBikeEntityBase>();
                            while (dr.Read())
                            {
                                BestBikeEntityBase bestBikeObj = new BestBikeEntityBase();
                                bestBikeObj.BikeName = Convert.ToString(dr["BikeName"]);
                                bestBikeObj.HostUrl = Convert.ToString(dr["HostURL"]);
                                bestBikeObj.OriginalImagePath = Convert.ToString(dr["ImagePath"]);
                                bestBikeObj.Make = new BikeMakeEntityBase();
                                bestBikeObj.Model = new BikeModelEntityBase();
                                bestBikeObj.Model.ModelId = SqlReaderConvertor.ToInt32(dr["ModelId"]);
                                bestBikeObj.Model.ModelName = Convert.ToString(dr["ModelName"]);
                                bestBikeObj.Model.MaskingName = Convert.ToString(dr["ModelMaskingName"]);
                                bestBikeObj.Make.MakeId = SqlReaderConvertor.ToInt32(dr["MakeId"]);
                                bestBikeObj.Make.MakeName = Convert.ToString(dr["MakeName"]);
                                bestBikeObj.Make.MaskingName = Convert.ToString(dr["MakeMaskingName"]);
                                bestBikeObj.Price = SqlReaderConvertor.ToUInt32(dr["MinPrice"]);
                                bestBikeObj.AvgPrice = SqlReaderConvertor.ToUInt32(dr["AvgPrice"]);
                                bestBikeObj.SmallModelDescription = Convert.ToString(dr["SmallDescription"]);
                                bestBikeObj.FullModelDescription = Convert.ToString(dr["FullDescription"]);
                                bestBikeObj.UnitsSold = SqlReaderConvertor.ToUInt32(dr["UnitsSold"]);
                                bestBikeObj.LaunchDate = SqlReaderConvertor.ToDateTime(dr["LaunchDate"]);
                                bestBikeObj.PhotosCount = SqlReaderConvertor.ToUInt32(dr["PhotosCount"]);
                                bestBikeObj.VideosCount = SqlReaderConvertor.ToUInt32(dr["VideosCount"]);
                                bestBikeObj.ExpertReviewsCount = SqlReaderConvertor.ToUInt32(dr["ExpertReviewsCount"]);
                                bestBikeObj.NewsCount = SqlReaderConvertor.ToUInt32(dr["NewsCount"]);
                                bestBikeObj.TotalVersions = SqlReaderConvertor.ToUInt32(dr["VersionCount"]);
                                bestBikeObj.TotalModelColors = SqlReaderConvertor.ToUInt32(dr["ColorCount"]);
                                bestBikeObj.LastUpdatedModelSold = SqlReaderConvertor.ToDateTime(dr["UnitSoldDate"]);
                                bestBikeObj.UsedBikesCount = SqlReaderConvertor.ToUInt32(dr["AvailableBikes"]);
                                bestBikeObj.UsedCity = new CityEntityBase();
                                bestBikeObj.UsedCity.CityId = SqlReaderConvertor.ToUInt32(dr["CityId"]);
                                bestBikeObj.UsedCity.CityMaskingName = Convert.ToString(dr["CityMaskingName"]);
                                bestBikeObj.UsedCity.CityName = Convert.ToString(dr["CityName"]);
                                bestBikeObj.PriceInCity = Convert.ToString(dr["PriceInCity"]);
                                bestBikeObj.VersionId = Convert.ToInt32(dr["TopVersionId"]);
                                bestBikesList.Add(bestBikeObj);
                            }
                            dr.Close();
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("GenericBikeRepository.GetBestBikesByCategory: BodyStyleId:{0}", bodyStyle));
            }
            return bestBikesList;
        }

        /// <summary>
        /// Created By  :   Vishnu Teja Yalakuntla on 11 Sep 2017
        /// Description :   Fetches best bikes for particular model in its make
        /// Modified by : Pratibha Verma on 27 Mar 2018
        /// Description : Removed MinSpecs code
        /// Modified by : Ashutosh Sharma on 05 Apr 2018.
        /// Description : Changed sp from 'getsimilarbikemodelswithinmake' to 'getsimilarbikemodelswithinmake_05042018' to remove min specs.
        /// </summary>
        /// <param name="modelId"></param>
        /// <param name="cityId"></param>
        /// <returns></returns>
        public IEnumerable<BestBikeEntityBase> GetBestBikesByModelInMake(uint modelId)
        {
            ICollection<BestBikeEntityBase> bestBikesList = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "getsimilarbikemodelswithinmake_05042018";
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_modelid", DbType.Int32, modelId));

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            bestBikesList = new Collection<BestBikeEntityBase>();
                            BestBikeEntityBase bestBikeObj;
                            while (dr.Read())
                            {
                                bestBikeObj = new BestBikeEntityBase();
                                bestBikeObj.BikeName = Convert.ToString(dr["BikeName"]);
                                bestBikeObj.HostUrl = Convert.ToString(dr["HostURL"]);
                                bestBikeObj.OriginalImagePath = Convert.ToString(dr["OriginalImagePath"]);
                                bestBikeObj.Make = new BikeMakeEntityBase();
                                bestBikeObj.Model = new BikeModelEntityBase();
                                bestBikeObj.Model.ModelId = SqlReaderConvertor.ToInt32(dr["ModelId"]);
                                bestBikeObj.Model.ModelName = Convert.ToString(dr["ModelName"]);
                                bestBikeObj.Model.MaskingName = Convert.ToString(dr["ModelMaskingName"]);
                                bestBikeObj.Make.MakeId = SqlReaderConvertor.ToInt32(dr["MakeId"]);
                                bestBikeObj.Make.MakeName = Convert.ToString(dr["MakeName"]);
                                bestBikeObj.Make.MaskingName = Convert.ToString(dr["MakeMaskingName"]);
                                bestBikeObj.Price = SqlReaderConvertor.ToUInt32(dr["VersionPrice"]);
                                bestBikeObj.UsedCity = new CityEntityBase();
                                bestBikesList.Add(bestBikeObj);
                            }
                            dr.Close();
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("GenericBikeRepository.GetBestBikesByCategory: ModelId:{0}", modelId));
            }
            return bestBikesList;
        }

        /// <summary>
        /// Created By  :   Vishnu Teja Yalakuntla on 11 Sep 2017
        /// Description :   Fetches best bikes for particular model in its make with on road price in given city
        /// Modified by : Pratibha Verma on 27 Mar 2018
        /// Description : Removed MinSpecs code
        /// Modified by : Ashutosh Sharma on 05 Apr 2018.
        /// Description : Changed sp from 'getsimilarbikemodelswithinmakebycity' to 'getsimilarbikemodelswithinmakebycity_05042018' to remove min specs.
        /// </summary>
        /// <param name="modelId"></param>
        /// <param name="cityId"></param>
        /// <returns></returns>
        public ICollection<BestBikeEntityBase> GetBestBikesByModelInMake(uint modelId, uint cityId)
        {
            ICollection<BestBikeEntityBase> bestBikesList = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "getsimilarbikemodelswithinmakebycity_05042018";
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_modelid", DbType.Int32, modelId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_cityid", DbType.Int32, cityId));

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            bestBikesList = new Collection<BestBikeEntityBase>();
                            while (dr.Read())
                            {
                                BestBikeEntityBase bestBikeObj = new BestBikeEntityBase();
                                bestBikeObj.BikeName = Convert.ToString(dr["BikeName"]);
                                bestBikeObj.HostUrl = Convert.ToString(dr["HostURL"]);
                                bestBikeObj.OriginalImagePath = Convert.ToString(dr["OriginalImagePath"]);
                                bestBikeObj.Make = new BikeMakeEntityBase();
                                bestBikeObj.Model = new BikeModelEntityBase();
                                bestBikeObj.Model.ModelId = SqlReaderConvertor.ToInt32(dr["ModelId"]);
                                bestBikeObj.Model.ModelName = Convert.ToString(dr["ModelName"]);
                                bestBikeObj.Model.MaskingName = Convert.ToString(dr["ModelMaskingName"]);
                                bestBikeObj.Make.MakeId = SqlReaderConvertor.ToInt32(dr["MakeId"]);
                                bestBikeObj.Make.MakeName = Convert.ToString(dr["MakeName"]);
                                bestBikeObj.Make.MaskingName = Convert.ToString(dr["MakeMaskingName"]);
                                bestBikeObj.Price = SqlReaderConvertor.ToUInt32(dr["VersionPrice"]);
                                bestBikeObj.AvgPrice = SqlReaderConvertor.ToUInt32(dr["AvgPrice"]);
                                bestBikeObj.UsedCity = new CityEntityBase();
                                bestBikeObj.OnRoadPriceInCity = SqlReaderConvertor.ToUInt32(dr["OnRoadPriceInCity"]);
                                bestBikeObj.UsedCity.CityId = SqlReaderConvertor.ToUInt32(dr["CityId"]);
                                bestBikeObj.UsedCity.CityMaskingName = Convert.ToString(dr["CityMaskingName"]);
                                bestBikeObj.UsedCity.CityName = Convert.ToString(dr["CityName"]);
                                bestBikesList.Add(bestBikeObj);
                            }
                            dr.Close();
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("GenericBikeRepository.GetBestBikesByCategory: ModelId:{0} Cityid:{1}", modelId, cityId));
            }
            return bestBikesList;
        }

        /// <summary>
        /// Modified By :- Subodh Jain on 17 Jan 2017
        /// Summary :- get makedetails if videos is present
        /// </summary>
        public IEnumerable<BikeMakeEntityBase> GetMakeIfVideo()
        {
            IList<BikeMakeEntityBase> objVideoMake = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "getmakebyvideo";
                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {

                            objVideoMake = new List<BikeMakeEntityBase>();

                            while (dr.Read())
                            {
                                objVideoMake.Add(new BikeMakeEntityBase()
                                {
                                    PopularityIndex = SqlReaderConvertor.ToUInt16(dr["PopularityIndex"]),
                                    MakeId = SqlReaderConvertor.ToUInt16(dr["MakeId"]),
                                    MaskingName = Convert.ToString(dr["MaskingName"]),
                                    MakeName = Convert.ToString(dr["MakeName"])
                                });

                            }
                        }
                    }


                }

            }
            catch (Exception err)
            {

                ErrorClass.LogError(err, " Bikewale.DAL.BikeData.GetMakeIfVideo");

            } // catch Exception
            return objVideoMake;
        }
        /// <summary>
        /// Created by :- Subodh Jain 3 feb 2017
        /// Summary :- Bind Video details for similar bikes
        /// </summary>
        /// <param name="modelId"></param>
        /// <param name="totalRecords"></param>
        /// <returns></returns>
        public IEnumerable<SimilarBikeWithVideo> GetSimilarBikesVideos(uint modelId, uint totalRecords, uint cityid)
        {
            IList<SimilarBikeWithVideo> SimilarBikeInfoList = null;
            try
            {

                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "getalternativebikeswithvideoscount_27102017";
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_modelid", DbType.Int32, modelId));

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_cityid", DbType.Int32, cityid));


                    cmd.Parameters.Add(DbFactory.GetDbParam("par_topcount", DbType.Int16, totalRecords));
                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            SimilarBikeInfoList = new List<SimilarBikeWithVideo>();

                            while (dr.Read())
                            {
                                var bikeInfo = new SimilarBikeWithVideo();
                                bikeInfo.Make = new BikeMakeEntityBase();
                                bikeInfo.Model = new BikeModelEntityBase();
                                bikeInfo.OriginalImagePath = Convert.ToString(dr["originalimagepath"]);
                                bikeInfo.HostUrl = Convert.ToString(dr["hosturl"]);
                                bikeInfo.VideoCount = SqlReaderConvertor.ToUInt32(dr["VideosCount"]);
                                bikeInfo.Make.MakeName = Convert.ToString(dr["makename"]);
                                bikeInfo.Make.MaskingName = Convert.ToString(dr["makemaskingname"]);
                                bikeInfo.Model.ModelName = Convert.ToString(dr["modelname"]);
                                bikeInfo.Model.MaskingName = Convert.ToString(dr["modelmaskingname"]);
                                bikeInfo.ExShowRoomPriceMumbai = SqlReaderConvertor.ToUInt32(dr["exshowroompricemumbai"]);
                                bikeInfo.OnRoadPriceInCity = SqlReaderConvertor.ToUInt32(dr["onroadpriceincity"]);
                                SimilarBikeInfoList.Add(bikeInfo);

                            }
                            dr.Close();
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("Bikewale.DAL.BikeData.GetSimilarBikesVideos_Model: {0}", modelId));
            }
            return SimilarBikeInfoList;
        }

        /// <summary>
        /// Created by :- Sajal Gupta on 08-05-2017
        /// Summary :- Bind User review count for similar bikes
        /// Modified By :   Vishnu Teja Yalakuntla on 09 Sep 2017
        /// Description :   Getting the ex-showroom price in mumbai in addition. Changed the method name. 
        /// </summary>
        /// <param name="modelId"></param>
        /// <param name="totalRecords"></param>
        /// <returns></returns>
        public IEnumerable<SimilarBikeUserReview> GetSimilarBikesUserReviewsWithPrice(uint modelId, uint totalRecords)
        {
            IList<SimilarBikeUserReview> SimilarBikeInfoList = null;
            try
            {

                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "getalternativebikeswithreviewcountandprice";
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_modelid", DbType.Int32, modelId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_topcount", DbType.Int16, totalRecords));
                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            SimilarBikeInfoList = new List<SimilarBikeUserReview>();

                            while (dr.Read())
                            {
                                var bikeInfo = new SimilarBikeUserReview();
                                bikeInfo.Make = new Entities.BikeData.BikeMakeEntityBase();
                                bikeInfo.Model = new BikeModelEntityBase();
                                bikeInfo.OriginalImagePath = Convert.ToString(dr["originalimagepath"]);
                                bikeInfo.HostUrl = Convert.ToString(dr["hosturl"]);
                                bikeInfo.OverAllRating = SqlReaderConvertor.ParseToDouble(dr["overallrating"]);
                                bikeInfo.Make.MakeName = Convert.ToString(dr["makename"]);
                                bikeInfo.Make.MaskingName = Convert.ToString(dr["makemaskingname"]);
                                bikeInfo.Model.ModelName = Convert.ToString(dr["modelname"]);
                                bikeInfo.Model.MaskingName = Convert.ToString(dr["modelmaskingname"]);
                                bikeInfo.NumberOfRating = SqlReaderConvertor.ToUInt32(dr["numberOfRatings"]);
                                bikeInfo.NumberOfReviews = SqlReaderConvertor.ToUInt32(dr["numberOfReviews"]);
                                bikeInfo.ExShowroomPriceMumbai = SqlReaderConvertor.ToUInt32(dr["ExShowroomPriceMumbai"]);
                                SimilarBikeInfoList.Add(bikeInfo);
                            }
                            dr.Close();
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("Bikewale.DAL.BikeData.GetSimilarBikesUserReviews_Model: {0}", modelId));
            }
            return SimilarBikeInfoList;
        }

        /// <summary>
        /// Modified By :   Vishnu Teja Yalakuntla on 09 Sep 2017
        /// Description :   Getting Similar Bikes UserReviews with on-road price in city.
        /// </summary>
        /// <param name="modelId"></param>
        /// <param name="cityId"></param>
        /// <param name="totalRecords"></param>
        /// <returns></returns>
        public IEnumerable<SimilarBikeUserReview> GetSimilarBikesUserReviewsWithPriceInCity(uint modelId, uint cityId, uint totalRecords)
        {
            IList<SimilarBikeUserReview> SimilarBikeInfoList = null;
            try
            {

                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "getalternativebikeswithreviewcountandpriceincity";
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_modelid", DbType.Int32, modelId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_cityid", DbType.Int32, cityId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_topcount", DbType.Int16, totalRecords));
                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            SimilarBikeInfoList = new List<SimilarBikeUserReview>();

                            while (dr.Read())
                            {
                                var bikeInfo = new SimilarBikeUserReview();
                                bikeInfo.Make = new Entities.BikeData.BikeMakeEntityBase();
                                bikeInfo.Model = new BikeModelEntityBase();
                                bikeInfo.OriginalImagePath = Convert.ToString(dr["originalimagepath"]);
                                bikeInfo.HostUrl = Convert.ToString(dr["hosturl"]);
                                bikeInfo.OverAllRating = SqlReaderConvertor.ParseToDouble(dr["overallrating"]);
                                bikeInfo.Make.MakeName = Convert.ToString(dr["makename"]);
                                bikeInfo.Make.MaskingName = Convert.ToString(dr["makemaskingname"]);
                                bikeInfo.Model.ModelName = Convert.ToString(dr["modelname"]);
                                bikeInfo.Model.MaskingName = Convert.ToString(dr["modelmaskingname"]);
                                bikeInfo.NumberOfRating = SqlReaderConvertor.ToUInt32(dr["numberOfRatings"]);
                                bikeInfo.NumberOfReviews = SqlReaderConvertor.ToUInt32(dr["numberOfReviews"]);
                                bikeInfo.ExShowroomPriceMumbai = SqlReaderConvertor.ToUInt32(dr["ExShowroomPriceMumbai"]);
                                bikeInfo.OnRoadPriceInCity = SqlReaderConvertor.ToUInt32(dr["OnRoadPriceInCity"]);
                                SimilarBikeInfoList.Add(bikeInfo);
                            }
                            dr.Close();
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("Bikewale.DAL.BikeData.GetSimilarBikesUserReviews_Model: {0}", modelId));
            }
            return SimilarBikeInfoList;
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 10 Feb 2017
        /// Description :   Returns New bikes launched
        /// Modified by:- Subodh jain 09 march 2017
        ///summary :-  Added body type filter
        /// Modified by : Pratibha Verma on 28 Mar 2018
        /// Description : Replaced sp with `getnewlaunchedbikes_28032018`
        /// </summary>
        /// <returns></returns>
        public IEnumerable<NewLaunchedBikeEntityBase> GetNewLaunchedBikesList()
        {
            ICollection<NewLaunchedBikeEntityBase> newLaunchedBikes = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "getnewlaunchedbikes_28032018";
                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            newLaunchedBikes = new Collection<NewLaunchedBikeEntityBase>();

                            while (dr.Read())
                            {
                                newLaunchedBikes.Add(
                                    new NewLaunchedBikeEntityBase()
                                    {
                                        Make = new BikeMakeEntityBase()
                                        {
                                            MakeId = SqlReaderConvertor.ToInt32(dr["bikemakeid"]),
                                            MakeName = Convert.ToString(dr["make"]),
                                            MaskingName = Convert.ToString(dr["makemaskingname"]),
                                            PopularityIndex = SqlReaderConvertor.ToUInt16(dr["makePopularityIndex"])
                                        },
                                        Model = new BikeModelEntityBase()
                                        {
                                            ModelId = SqlReaderConvertor.ToInt32(dr["modelid"]),
                                            ModelName = Convert.ToString(dr["model"]),
                                            MaskingName = Convert.ToString(dr["modelmaskingname"])
                                        },

                                        HostUrl = Convert.ToString(dr["hosturl"]),
                                        OriginalImagePath = Convert.ToString(dr["OriginalImagePath"]),
                                        ReviewCount = SqlReaderConvertor.ToUInt32(dr["reviewcount"]),
                                        ReviewRate = SqlReaderConvertor.ParseToDouble(dr["reviewrate"]),
                                        MinPrice = SqlReaderConvertor.ToUInt32(dr["minprice"]),
                                        MaxPrice = SqlReaderConvertor.ToUInt32(dr["maxprice"]),
                                        LaunchedOn = SqlReaderConvertor.ToDateTime(dr["LaunchDate"]),
                                        Price = SqlReaderConvertor.ToUInt32(dr["Price"]),
                                        BodyStyleId = SqlReaderConvertor.ToUInt32(dr["BodyStyleId"]),
                                        VersionId = SqlReaderConvertor.ToInt32(dr["TopVersionId"])
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
                ErrorClass.LogError(ex, "Bikewale.DAL.BikeData.GetNewLaunchedBikesList");
            }
            return newLaunchedBikes;
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 13 Feb 2017
        /// Description :   Returns New Bikes launched with Exshowroom of given city
        /// Modified by:- Subodh jain 09 march 2017
        /// Summary :-  Added body type filter
        /// Modified by : Sanskar Gupta on 12 Feb 2018
        /// Description : Changed SP to `getnewlaunchedbikesbycity_12022018`
        /// Modified by : Pratibha Verma on 28 Mar 2018
        /// Description : Replaced sp with `getnewlaunchedbikesbycity_28032018`
        /// </summary>
        /// <param name="cityId"></param>
        /// <returns></returns>
        public IEnumerable<NewLaunchedBikeEntityBase> GetNewLaunchedBikesList(uint cityId)
        {
            ICollection<NewLaunchedBikeEntityBase> newLaunchedBikes = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "getnewlaunchedbikesbycity_28032018";
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_cityid", DbType.Int16, cityId));
                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            newLaunchedBikes = new Collection<NewLaunchedBikeEntityBase>();

                            while (dr.Read())
                            {
                                newLaunchedBikes.Add(
                                    new NewLaunchedBikeEntityBase()
                                    {
                                        Make = new BikeMakeEntityBase()
                                        {
                                            MakeId = SqlReaderConvertor.ToInt32(dr["bikemakeid"]),
                                            MakeName = Convert.ToString(dr["make"]),
                                            MaskingName = Convert.ToString(dr["makemaskingname"]),
                                            PopularityIndex = SqlReaderConvertor.ToUInt16(dr["makePopularityIndex"])
                                        },
                                        Model = new BikeModelEntityBase()
                                        {
                                            ModelId = SqlReaderConvertor.ToInt32(dr["modelid"]),
                                            ModelName = Convert.ToString(dr["model"]),
                                            MaskingName = Convert.ToString(dr["modelmaskingname"])
                                        },

                                        HostUrl = Convert.ToString(dr["hosturl"]),
                                        OriginalImagePath = Convert.ToString(dr["OriginalImagePath"]),
                                        ReviewCount = SqlReaderConvertor.ToUInt32(dr["reviewcount"]),
                                        ReviewRate = SqlReaderConvertor.ParseToDouble(dr["reviewrate"]),
                                        MinPrice = SqlReaderConvertor.ToUInt32(dr["minprice"]),
                                        MaxPrice = SqlReaderConvertor.ToUInt32(dr["maxprice"]),
                                        LaunchedOn = SqlReaderConvertor.ToDateTime(dr["LaunchDate"]),
                                        City = (!String.IsNullOrEmpty(Convert.ToString(dr["cityname"])) ? new CityEntityBase()
                                        {
                                            CityId = SqlReaderConvertor.ToUInt32(dr["cityid"]),
                                            CityName = Convert.ToString(dr["cityname"]),
                                            CityMaskingName = Convert.ToString(dr["citymaskingname"])
                                        } : null),
                                        Price = SqlReaderConvertor.ToUInt32(dr["price"]),
                                        AvgPrice = SqlReaderConvertor.ToUInt32(dr["AvgPrice"]),
                                        ExshowroomPrice = SqlReaderConvertor.ToUInt32(dr["ExshowroomPrice"]),
                                        VersionPrice = SqlReaderConvertor.ToUInt32(dr["VersionPrice"]),
                                        BodyStyleId = SqlReaderConvertor.ToUInt32(dr["BodyStyleId"]),
                                        VersionId = SqlReaderConvertor.ToInt32(dr["TopVersionId"])
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
                ErrorClass.LogError(ex, "Bikewale.DAL.BikeData.GetNewLaunchedBikesList");
            }
            return newLaunchedBikes;
        }
        /// <summary>
        /// Created By : Aditi Srivastava on 9 Mar 2017
        /// Summary    : Return list of popular scooters
        /// Modified by : Ashutosh Sharma on 03 Apr 2018
        /// Description : Assigning version object in bike models.
        /// Modified  by : Ashutosh Sharma on 05 Apr 2018
        /// Description : Changed sp 'getmostpopularscootersbymakecity' from to 'getmostpopularscootersbymakecity_05042018' to remove min specs.
        /// </summary>
        public IEnumerable<MostPopularBikesBase> GetMostPopularScooters(uint topCount, uint? cityId)
        {
            ICollection<MostPopularBikesBase> popularScooters = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "getmostpopularscootersbymakecity_05042018";
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_topcount", DbType.Int32, topCount));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_cityid", DbType.Int32, (cityId.HasValue && cityId.Value > 0) ? cityId.Value : Convert.DBNull));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_makeid", DbType.Int32, Convert.DBNull));
                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            popularScooters = new Collection<MostPopularBikesBase>();

                            while (dr.Read())
                            {
                                popularScooters.Add(
                                   new MostPopularBikesBase()
                                   {
                                       objMake = new BikeMakeEntityBase()
                                       {
                                           MakeId = SqlReaderConvertor.ToInt32(dr["makeid"]),
                                           MakeName = Convert.ToString(dr["make"]),
                                           MaskingName = Convert.ToString(dr["makemaskingname"]),
                                       },
                                       objModel = new BikeModelEntityBase()
                                       {
                                           ModelId = SqlReaderConvertor.ToInt32(dr["modelid"]),
                                           ModelName = Convert.ToString(dr["model"]),
                                           MaskingName = Convert.ToString(dr["modelmaskingname"]),
                                       },
                                       objVersion = new BikeVersionsListEntity
                                       {
                                           VersionId = SqlReaderConvertor.ToInt32(dr["versionid"])
                                       },
                                       BikeName = Convert.ToString(dr["bikename"]),
                                       MakeId = SqlReaderConvertor.ToInt32(dr["makeid"]),
                                       MakeName = Convert.ToString(dr["make"]),
                                       MakeMaskingName = Convert.ToString(dr["makemaskingname"]),
                                       HostURL = Convert.ToString(dr["hosturl"]),
                                       OriginalImagePath = Convert.ToString(dr["originalimagepath"]),
                                       VersionPrice = SqlReaderConvertor.ToUInt32(dr["VersionPrice"]),
                                       ReviewCount = SqlReaderConvertor.ToInt32(dr["reviewcount"]),
                                       CityName = Convert.ToString(dr["cityname"]),
                                       CityMaskingName = Convert.ToString(dr["citymasking"])
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
                ErrorClass.LogError(ex, "Bikewale.DAL.BikeData.GetMostPopularScooters");
            }
            return popularScooters;
        }
        /// <summary>
        /// Created by:- Subodh Jain 10 March 2017
        /// Summary :- Get comparision list of popular bike 
        /// Modified by : Ashutosh Sharma on 05 Apr 2018
        /// Description : Changed sp 'getmostpopularscootersbymake' from to 'getmostpopularscootersbymake_04042018' to remove min specs.
        /// </summary>
        /// <param name="makeId"></param>
        /// <returns></returns>
        public IEnumerable<MostPopularBikesBase> GetMostPopularScooters(uint makeId)
        {
            ICollection<MostPopularBikesBase> objList = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("getmostpopularscootersbymake_04042018"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_makeid", DbType.Int32, makeId));

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            objList = new Collection<MostPopularBikesBase>();
                            MostPopularBikesBase objData = null;
                            while (dr.Read())
                            {
                                objData = new MostPopularBikesBase();
                                objData.objMake = new BikeMakeEntityBase();
                                objData.objModel = new BikeModelEntityBase();
                                objData.objVersion = new BikeVersionsListEntity();
                                objData.objMake.MakeName = Convert.ToString(dr["Make"]);
                                objData.objModel.ModelName = Convert.ToString(dr["Model"]);
                                objData.objMake.MakeId = Convert.ToInt32(dr["MakeId"]);
                                objData.objModel.ModelId = Convert.ToInt32(dr["ModelId"]);
                                objData.objMake.MaskingName = Convert.ToString(dr["MakeMaskingName"]);
                                objData.objModel.MaskingName = Convert.ToString(dr["ModelMaskingName"]);
                                objData.ModelRating = Convert.ToDouble(dr["ReviewRate"]);
                                objData.ReviewCount = Convert.ToUInt16(dr["ReviewCount"]);
                                objData.BikeName = Convert.ToString(dr["BikeName"]);
                                objData.HostURL = Convert.ToString(dr["HostUrl"]);
                                objData.OriginalImagePath = Convert.ToString(dr["OriginalImagePath"]);
                                objData.VersionPrice = SqlReaderConvertor.ToInt64(dr["VersionPrice"]);
                                objData.BikePopularityIndex = Convert.ToUInt16(dr["PopularityIndex"]);
                                objData.objVersion.VersionId = SqlReaderConvertor.ToInt32(dr["VersionId"]);
                                objList.Add(objData);
                            }
                            dr.Close();
                        }
                    }
                }
            }
            catch (Exception err)
            {
                ErrorClass.LogError(err, string.Format("Bikewale.DAL.BikeData.GetMostPopularScooters MakeId:{0}", makeId));
            }
            return objList;
        }
        /// <summary>
        /// Created By :- Subodh Jain 07-12-2017
        /// Summary :- Method to GetElectricBikes 
        /// Modified by : Ashutosh Sharma on 07 Apr 2018.
        /// Description : Changed sp from 'getelectricbikes' to 'getelectricbikes_07042018' to remove min specs.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<MostPopularBikesBase> GetElectricBikes()
        {
            ICollection<MostPopularBikesBase> objList = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("getelectricbikes_07042018"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            objList = new Collection<MostPopularBikesBase>();
                            MostPopularBikesBase objData = null;
                            while (dr.Read())
                            {
                                objData = new MostPopularBikesBase();
                                objData.objMake = new BikeMakeEntityBase();
                                objData.objModel = new BikeModelEntityBase();
                                objData.objVersion = new BikeVersionsListEntity();
                                objData.objMake.MakeName = Convert.ToString(dr["Make"]);
                                objData.objModel.ModelName = Convert.ToString(dr["Model"]);
                                objData.objMake.MakeId = Convert.ToInt32(dr["MakeId"]);
                                objData.objModel.ModelId = Convert.ToInt32(dr["ModelId"]);
                                objData.objMake.MaskingName = Convert.ToString(dr["MakeMaskingName"]);
                                objData.objModel.MaskingName = Convert.ToString(dr["ModelMaskingName"]);
                                objData.ModelRating = Convert.ToDouble(dr["ReviewRate"]);
                                objData.ReviewCount = Convert.ToUInt16(dr["ReviewCount"]);
                                objData.BikeName = Convert.ToString(dr["BikeName"]);
                                objData.HostURL = Convert.ToString(dr["HostUrl"]);
                                objData.OriginalImagePath = Convert.ToString(dr["OriginalImagePath"]);
                                objData.VersionPrice = SqlReaderConvertor.ToInt64(dr["VersionPrice"]);
                                objData.objVersion.VersionId = SqlReaderConvertor.ToInt32(dr["VersionId"]);
                                objList.Add(objData);
                            }
                            dr.Close();
                        }
                    }
                }
            }
            catch (Exception err)
            {
                ErrorClass.LogError(err, string.Format("Bikewale.DAL.BikeData.GetElectricBikes"));
            }
            return objList;
        }


        /// <summary>
        /// Created By :- Subodh Jain 07-12-2017
        /// Summary :- Method to GetElectricBikes 
        /// Modified by : Ashutosh Sharma on 07 Apr 2018.
        /// Description : Changed sp from 'getelectricbikesbycity' to 'getelectricbikesbycity_07042018' to remove min specs.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<MostPopularBikesBase> GetElectricBikes(uint cityId)
        {
            ICollection<MostPopularBikesBase> objList = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("getelectricbikesbycity_07042018"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_cityid", DbType.Int32, cityId));

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            objList = new Collection<MostPopularBikesBase>();
                            MostPopularBikesBase objData = null;
                            while (dr.Read())
                            {
                                objData = new MostPopularBikesBase();
                                objData.objMake = new BikeMakeEntityBase();
                                objData.objModel = new BikeModelEntityBase();
                                objData.objVersion = new BikeVersionsListEntity();
                                objData.objMake.MakeName = Convert.ToString(dr["Make"]);
                                objData.objModel.ModelName = Convert.ToString(dr["Model"]);
                                objData.objMake.MakeId = Convert.ToInt32(dr["MakeId"]);
                                objData.objModel.ModelId = Convert.ToInt32(dr["ModelId"]);
                                objData.objMake.MaskingName = Convert.ToString(dr["MakeMaskingName"]);
                                objData.objModel.MaskingName = Convert.ToString(dr["ModelMaskingName"]);
                                objData.ModelRating = Convert.ToDouble(dr["ReviewRate"]);
                                objData.ReviewCount = Convert.ToUInt16(dr["ReviewCount"]);
                                objData.BikeName = Convert.ToString(dr["BikeName"]);
                                objData.HostURL = Convert.ToString(dr["HostUrl"]);
                                objData.OriginalImagePath = Convert.ToString(dr["OriginalImagePath"]);
                                objData.VersionPrice = SqlReaderConvertor.ToInt64(dr["VersionPrice"]);
                                objData.objVersion.VersionId = SqlReaderConvertor.ToInt32(dr["VersionId"]);
                                objData.CityName = Convert.ToString(dr["cityname"]);
                                objData.CityMaskingName = Convert.ToString(dr["citymaskingname"]);

                                objList.Add(objData);
                            }
                            dr.Close();
                        }
                    }
                }
            }
            catch (Exception err)
            {
                ErrorClass.LogError(err, string.Format("Bikewale.DAL.BikeData.GetElectricBikes"));
            }
            return objList;
        }

        /// <summary>
        /// Created by  : Sumit Kate on 24 Mar 2017
        /// Description : Returns GetMostPopularScooters by make and city
        /// Modified by : Ashutosh Sharma on 05 Apr 2018
        /// Description : Changed sp 'getmostpopularscootersbymakecity' from to 'getmostpopularscootersbymakecity_05042018' to remove min specs.
        /// </summary>
        /// <param name="topCount"></param>
        /// <param name="makeId"></param>
        /// <param name="cityId"></param>
        /// <returns></returns>
        public IEnumerable<MostPopularBikesBase> GetMostPopularScooters(uint topCount, uint makeId, uint cityId)
        {
            ICollection<MostPopularBikesBase> popularScooters = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "getmostpopularscootersbymakecity_05042018";
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_topcount", DbType.Int32, topCount));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_cityid", DbType.Int32, cityId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_makeid", DbType.Int32, makeId));
                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            popularScooters = new Collection<MostPopularBikesBase>();

                            while (dr.Read())
                            {
                                popularScooters.Add(
                                   new MostPopularBikesBase()
                                   {
                                       objMake = new BikeMakeEntityBase()
                                       {
                                           MakeId = SqlReaderConvertor.ToInt32(dr["makeid"]),
                                           MakeName = Convert.ToString(dr["make"]),
                                           MaskingName = Convert.ToString(dr["makemaskingname"]),
                                       }
                                       ,
                                       objModel = new BikeModelEntityBase()
                                       {
                                           ModelId = SqlReaderConvertor.ToInt32(dr["modelid"]),
                                           ModelName = Convert.ToString(dr["model"]),
                                           MaskingName = Convert.ToString(dr["modelmaskingname"]),
                                       },
                                       BikeName = Convert.ToString(dr["bikename"]),
                                       MakeId = SqlReaderConvertor.ToInt32(dr["makeid"]),
                                       MakeName = Convert.ToString(dr["make"]),
                                       MakeMaskingName = Convert.ToString(dr["makemaskingname"]),
                                       HostURL = Convert.ToString(dr["hosturl"]),
                                       OriginalImagePath = Convert.ToString(dr["originalimagepath"]),
                                       VersionPrice = SqlReaderConvertor.ToUInt32(dr["VersionPrice"]),
                                       ReviewCount = SqlReaderConvertor.ToInt32(dr["reviewcount"]),
                                       CityName = Convert.ToString(dr["cityname"]),
                                       CityMaskingName = Convert.ToString(dr["citymasking"])
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
                ErrorClass.LogError(ex, "Bikewale.DAL.BikeData.GetMostPopularScooters");
            }
            return popularScooters;
        }

        /// <summary>
        /// Created by : Ashutosh Sharma on 18-Aug-2017
        /// Description : DAL method to get popular bikes by body style.
        /// </summary>
        /// <param name="bodyStyleId"></param>
        /// <param name="topCount"></param>
        /// <param name="cityId"></param>
        /// <returns></returns>
        public IEnumerable<MostPopularBikesBase> GetPopularBikesByBodyStyle(ushort bodyStyleId, uint topCount, uint cityId)
        {

            ICollection<MostPopularBikesBase> popularBikesList = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "getmostpopularbikesbybodystyle";
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_bodystyleid", DbType.Int32, bodyStyleId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_topcount", DbType.Int32, topCount));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_cityid", DbType.Int32, cityId > 0 ? cityId : 0));
                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            popularBikesList = new Collection<MostPopularBikesBase>();
                            EnumBikeBodyStyles bodyStyle;
                            while (dr.Read())
                            {
                                MostPopularBikesBase popularObj = new MostPopularBikesBase();
                                popularObj.objModel = new BikeModelEntityBase();
                                popularObj.MakeId = SqlReaderConvertor.ToInt32(dr["MakeId"]);
                                popularObj.MakeName = Convert.ToString(dr["MakeName"]);
                                popularObj.MakeMaskingName = Convert.ToString(dr["MakeMaskingName"]);
                                popularObj.objModel.ModelId = SqlReaderConvertor.ToInt32(dr["ModelId"]);
                                popularObj.objModel.ModelName = Convert.ToString(dr["ModelName"]);
                                popularObj.objModel.MaskingName = Convert.ToString(dr["ModelMaskingName"]);
                                popularObj.OriginalImagePath = Convert.ToString(dr["OriginalImagePath"]);
                                popularObj.HostURL = Convert.ToString(dr["HostURL"]);
                                popularObj.VersionPrice = SqlReaderConvertor.ToInt64(dr["VersionPrice"]);
                                popularObj.CityName = Convert.ToString(dr["CityName"]);
                                popularObj.CityMaskingName = Convert.ToString(dr["CityMaskingName"]);
                                popularObj.BikePopularityIndex = SqlReaderConvertor.ToUInt16(dr["Rank"]);
                                Enum.TryParse(Convert.ToString(bodyStyleId), out bodyStyle);
                                popularObj.BodyStyle = bodyStyle;
                                popularBikesList.Add(popularObj);
                            }
                            dr.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format(" GetPopularBikesByBodyStyle_bodyStyleId: {0}, topCount: {1}, cityId: {2}", bodyStyleId, topCount, cityId));
            }
            return popularBikesList;

        }

        /// <summary>
        /// Created By:Snehal Dange on 3rd Nov 2017]
        /// Description: Dal Method to get mileage details for a model
        /// Modified by : Pratibha Verma on 12 April 2018
        /// Description : change sp 'getbikesdatawithmileage' to 'getbikesdatawithmileage_11042018'
        /// </summary>
        /// <param name="modelId"></param>
        /// <returns></returns>
        public BikeMileageEntity GetMileageForModel()
        {
            BikeMileageEntity mileageDetails = new BikeMileageEntity();
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("getbikesdatawithmileage_11042018"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            ICollection<BikeWithMileageInfo> bikes = new List<BikeWithMileageInfo>();
                            ICollection<MileageInfoByBodyStyle> bodyStyleMileage = new List<MileageInfoByBodyStyle>();
                            while (dr.Read())
                            {
                                bikes.Add(
                               new BikeWithMileageInfo()
                               {
                                   Make = new BikeMakeEntityBase()
                                   {
                                       MakeId = Convert.ToInt32(dr["makeid"]),
                                       MakeName = Convert.ToString(dr["make"]),
                                       MaskingName = Convert.ToString(dr["MakeMaskingName"]),
                                   },
                                   Model = new BikeModelEntityBase()
                                   {
                                       ModelId = Convert.ToInt32(dr["ModelId"]),
                                       ModelName = Convert.ToString(dr["Model"]),
                                       MaskingName = Convert.ToString(dr["ModelMaskingName"]),
                                   },
                                   HostUrl = Convert.ToString(dr["HostUrl"]),
                                   OriginalImagePath = Convert.ToString(dr["OriginalImagePath"]),
                                   BodyStyleId = Convert.ToUInt16(dr["BodyStyleId"]),
                                   MileageByUserReviews = SqlReaderConvertor.ToFloat(dr["mileagebyuserreview"]),
                                   Rank = Convert.ToUInt16(dr["rank"]),
                                   Percentile = SqlReaderConvertor.ToFloat(dr["percentilescore"])
                               });

                            }

                            if (dr.NextResult())
                            {
                                while (dr.Read())
                                {
                                    MileageInfoByBodyStyle bodyStyleMileageobj = new MileageInfoByBodyStyle();
                                    bodyStyleMileageobj.BodyStyleId = Convert.ToUInt16(dr["bodystyleid"]);
                                    bodyStyleMileageobj.TotalBikesInBodyStyle = Convert.ToUInt16(dr["totalBikes"]);
                                    bodyStyleMileageobj.AvgBodyStyleMileageByUserReviews = SqlReaderConvertor.ToFloat(dr["avgmileagebyuserreview"]);

                                    bodyStyleMileage.Add(bodyStyleMileageobj);
                                }

                            }
                            if (bikes.Any() && bodyStyleMileage.Any())
                            {
                                mileageDetails.Bikes = bikes;
                                mileageDetails.BodyStyleMileage = bodyStyleMileage;
                            }

                            dr.Close();
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass.LogError(ex, "Bikewale.DAL.BikeData.GetMileageForModel");
            }
            return mileageDetails;
        }

        /// <summary>
        /// Created by  : Vivek Singh Tomar on 28th Nov 2017
        /// Description : Get Series Entity by modelid
        /// </summary>
        /// <param name="modelId"></param>
        /// <returns></returns>
        public BikeSeriesEntityBase GetSeriesByModelId(uint modelId)
        {
            BikeSeriesEntityBase objSeries = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("getseriesbymodelid"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_modelid", DbType.UInt32, modelId));
                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null && dr.Read())
                        {
                            objSeries = new BikeSeriesEntityBase
                            {
                                SeriesId = SqlReaderConvertor.ToUInt32(dr["id"]),
                                SeriesName = Convert.ToString(dr["name"]),
                                MaskingName = Convert.ToString(dr["maskingname"]),
                                IsSeriesPageUrl = SqlReaderConvertor.ToBoolean(dr["isseriespageurl"])
                            };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("Bikewale.DAL.Bikedata.GetSeriesByModelId modelId = {0}", modelId));
            }
            return objSeries;
        }
        /// <summary>
        /// Modified by : Ashutosh Sharma on 05 Apr 2018.
        /// Description : Changed sp from 'getadpromotedbikesdetailsbycity' to 'getadpromotedbikesdetailsbycity_05042018' to remove min specs.
        /// </summary>
        /// <param name="bikeFilters"></param>
        /// <returns></returns>
        public IEnumerable<MostPopularBikesBase> GetAdPromotedBike(BikeFilters bikeFilters)
        {
            ICollection<MostPopularBikesBase> objList = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("getadpromotedbikesdetailsbycity_05042018"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;


                    cmd.Parameters.Add(DbFactory.GetDbParam("par_cityid", DbType.Int32, bikeFilters.CityId));


                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            objList = new Collection<MostPopularBikesBase>();
                            MostPopularBikesBase objData = null;
                            while (dr.Read())
                            {
                                objData = new MostPopularBikesBase();
                                objData.objMake = new BikeMakeEntityBase();
                                objData.objModel = new BikeModelEntityBase();
                                objData.objVersion = new BikeVersionsListEntity();
                                objData.objMake.MakeName = Convert.ToString(dr["Make"]);
                                objData.objModel.ModelName = Convert.ToString(dr["Model"]);
                                objData.objMake.MakeId = SqlReaderConvertor.ToInt32(dr["MakeId"]);
                                objData.objModel.ModelId = SqlReaderConvertor.ToInt32(dr["ModelId"]);
                                objData.objMake.MaskingName = Convert.ToString(dr["MakeMaskingName"]);
                                objData.objModel.MaskingName = Convert.ToString(dr["ModelMaskingName"]);
                                objData.objVersion.VersionId = SqlReaderConvertor.ToInt32(dr["VersionId"]);
                                objData.ModelRating = SqlReaderConvertor.ParseToDouble(dr["ReviewRate"]);
                                objData.ReviewCount = SqlReaderConvertor.ToUInt16(dr["ReviewCount"]);
                                objData.HostURL = Convert.ToString(dr["HostUrl"]);
                                objData.OriginalImagePath = Convert.ToString(dr["OriginalImagePath"]);
                                objData.VersionPrice = SqlReaderConvertor.ToInt64(dr["VersionPrice"]);
                                objData.CityName = Convert.ToString(dr["cityname"]);
                                objData.CityMaskingName = Convert.ToString(dr["citymasking"]);
                                objData.StartDate = SqlReaderConvertor.ToDateTime(dr["startdatetime"]);
                                objData.EndDate = SqlReaderConvertor.ToDateTime(dr["enddatetime"]);
                                objData.IsAdPromoted = true;
                                objList.Add(objData);
                            }
                            dr.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "BikeModelsRepository.GetAdPromotedBikesDetails");

            }
            return objList;
        }

        /// <summary>
        /// Modified by : Ashutosh Sharma on 05 Apr 2018
        /// Description : Changed sp 'getadpromotedbikesdetails' from to 'getadpromotedbikesdetails_05042018' to remove min specs.
        /// </summary>
        /// <param name="bikeFilters"></param>
        /// <returns></returns>
        public IEnumerable<MostPopularBikesBase> GetAdPromotedBikeWithOutCity(BikeFilters bikeFilters)
        {
            ICollection<MostPopularBikesBase> objList = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("getadpromotedbikesdetails_05042018"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            objList = new Collection<MostPopularBikesBase>();
                            MostPopularBikesBase objData = null;
                            while (dr.Read())
                            {
                                objData = new MostPopularBikesBase();
                                objData.objMake = new BikeMakeEntityBase();
                                objData.objModel = new BikeModelEntityBase();
                                objData.objVersion = new BikeVersionsListEntity();
                                objData.objMake.MakeName = Convert.ToString(dr["Make"]);
                                objData.objModel.ModelName = Convert.ToString(dr["Model"]);
                                objData.objMake.MakeId = SqlReaderConvertor.ToInt32(dr["MakeId"]);
                                objData.objModel.ModelId = SqlReaderConvertor.ToInt32(dr["ModelId"]);
                                objData.objMake.MaskingName = Convert.ToString(dr["MakeMaskingName"]);
                                objData.objModel.MaskingName = Convert.ToString(dr["ModelMaskingName"]);
                                objData.objVersion.VersionId = SqlReaderConvertor.ToInt32(dr["VersionId"]);
                                objData.ModelRating = SqlReaderConvertor.ParseToDouble(dr["ReviewRate"]);
                                objData.ReviewCount = SqlReaderConvertor.ToUInt16(dr["ReviewCount"]);
                                objData.HostURL = Convert.ToString(dr["HostUrl"]);
                                objData.OriginalImagePath = Convert.ToString(dr["OriginalImagePath"]);
                                objData.VersionPrice = SqlReaderConvertor.ToInt64(dr["VersionPrice"]);
                                objData.CityName = Convert.ToString(dr["cityname"]);
                                objData.CityMaskingName = Convert.ToString(dr["citymasking"]);
                                objData.StartDate = SqlReaderConvertor.ToDateTime(dr["startdatetime"]);
                                objData.EndDate = SqlReaderConvertor.ToDateTime(dr["enddatetime"]);
                                objData.IsAdPromoted = true;
                                objList.Add(objData);
                            }
                            dr.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "BikeModelsRepository.GetAdPromotedBikesDetails");

            }
            return objList;
        }

        /// <summary>
        /// Created by  : Vivek Singh Tomar on 11th Jan 2018
        /// Description : Get Model Ids with body style
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ModelIdWithBodyStyle> GetModelIdsForImages()
        {
            List<ModelIdWithBodyStyle> modelIdsWithBodyStyle = null;

            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("getmodelidsforimages"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            modelIdsWithBodyStyle = new List<ModelIdWithBodyStyle>();
                            while (dr.Read())
                            {
                                modelIdsWithBodyStyle.Add(new ModelIdWithBodyStyle
                                {
                                    MakeId = SqlReaderConvertor.ToUInt32(dr["MakeId"]),
                                    ModelId = SqlReaderConvertor.ToUInt32(dr["ModelId"]),
                                    BodyStyle = (EnumBikeBodyStyles)Enum.Parse(typeof(EnumBikeBodyStyles), Convert.ToString(dr["BodyStyleId"]))
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.DAL.BikeData.BikeModelsRepository.GetModelIdsForImages");
            }

            return modelIdsWithBodyStyle;
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 15 Jan 2018
        /// Description :   GetModelImages contains, model image and color images
        /// </summary>
        /// <param name="modelIds"></param>
        /// <returns></returns>
        public ICollection<BikeModelColorImageEntity> GetModelImages(string modelIds)
        {
            ICollection<BikeModelColorImageEntity> images = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("getmodelsphotos"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_modelids", DbType.String, modelIds));
                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            images = new List<BikeModelColorImageEntity>();
                            while (dr.Read())
                            {
                                images.Add(new BikeModelColorImageEntity
                                {
                                    Make = new BikeMakeBase()
                                    {
                                        MakeId = Utility.SqlReaderConvertor.ToInt32(dr["MakeId"]),
                                        MakeMaskingName = Convert.ToString(dr["MakeMaskingName"]),
                                        MakeName = Convert.ToString(dr["MakeName"])

                                    },
                                    Model = new BikeModelEntityBase()
                                    {
                                        ModelId = Utility.SqlReaderConvertor.ToInt32(dr["modelid"]),
                                        ModelName = Convert.ToString(dr["ModelName"]),
                                        MaskingName = Convert.ToString(dr["MaskingName"])
                                    },
                                    HostUrl = Convert.ToString(dr["hostUrl"]),
                                    OriginalImagePath = Convert.ToString(dr["OriginalImagepath"]),
                                    PhotosCount = Utility.SqlReaderConvertor.ToUInt16(dr["photoscount"])
                                });
                            }
                            if (dr.NextResult())
                            {
                                while (dr.Read())
                                {
                                    images.Add(new BikeModelColorImageEntity
                                    {
                                        Make = new BikeMakeBase()
                                        {
                                            MakeId = Utility.SqlReaderConvertor.ToInt32(dr["MakeId"]),
                                            MakeMaskingName = Convert.ToString(dr["MakeMaskingName"]),
                                            MakeName = Convert.ToString(dr["MakeName"])

                                        },
                                        Model = new BikeModelEntityBase()
                                        {
                                            ModelId = Utility.SqlReaderConvertor.ToInt32(dr["modelid"]),
                                            ModelName = Convert.ToString(dr["ModelName"]),
                                            MaskingName = Convert.ToString(dr["MaskingName"])
                                        },
                                        HostUrl = Convert.ToString(dr["hostUrl"]),
                                        OriginalImagePath = Convert.ToString(dr["OriginalImagepath"]),
                                        ColorId = Utility.SqlReaderConvertor.ToUInt32(dr["modelcolorid"]),
                                        PhotoId = Utility.SqlReaderConvertor.ToUInt32(dr["PhotoId"])
                                    });
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("Bikewale.DAL.BikeData.BikeModelsRepository.GetModelImages", modelIds));
            }
            return images;
        }
    }   // class
}   // namespace
