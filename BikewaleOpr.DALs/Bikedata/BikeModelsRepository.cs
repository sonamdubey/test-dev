
using Bikewale.DAL.CoreDAL;
using Bikewale.Notifications;
using Bikewale.Utility;
using BikewaleOpr.Entities.BikeData;
using BikewaleOpr.Entity.BikeData;
using BikewaleOpr.Interface.BikeData;
using Dapper;
using MySql.CoreDAL;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Common;

namespace BikewaleOpr.DALs.Bikedata
{
    public class BikeModelsRepository : IBikeModelsRepository
    {
        /// <summary>
        /// Created By : Sushil Kumar on  25th Oct 2016
        /// Description :  Getting Models only by providing MakeId and request type
        /// </summary>
        /// <param name="makeId"></param>
        /// <param name="requestType">Pass value as New or Used or Upcoming or PriceQuote</param>
        /// <returns></returns>
        public IEnumerable<BikeModelEntityBase> GetModels(uint makeId, string requestType)
        {
            IList<BikeModelEntityBase> _objBikeModels = null;
            try
            {

                using (DbCommand cmd = DbFactory.GetDBCommand("getbikemodels"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_requesttype", DbType.String, 20, requestType));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_makeid", DbType.Int32, makeId));


                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            _objBikeModels = new List<BikeModelEntityBase>();
                            while (dr.Read())
                            {
                                BikeModelEntityBase _objModel = new BikeModelEntityBase();
                                _objModel.ModelName = Convert.ToString(dr["Text"]);
                                _objModel.ModelId = SqlReaderConvertor.ToInt32(dr["Value"]);
                                _objBikeModels.Add(_objModel);
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("BikewaleOpr.DALs.Bikedata.GetModels_Make_{0}_RequestType_{1}", makeId, requestType));
                
            }
            return _objBikeModels;
        }

        /// <summary>
        /// Created by Sajal Gupta on 22-12-2016
        /// Des : Save model unit sold data in db
        /// </summary>
        /// <returns></returns>
        public void SaveModelUnitSold(string list, DateTime date)
        {
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandText = "savemodelunitsold";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_modelunitsoldList", DbType.String, list));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_monthyear", DbType.DateTime, date));

                    MySqlDatabase.InsertQuery(cmd, ConnectionType.ReadOnly);

                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("BikewaleOpr.DALs.Bikedata.SaveModelUnitSold-{0}-{1}", list, date));
            }
        }

        /// <summary>
        /// Created by Sajal Gupta on 22-12-2016
        /// Des : function to get the last month's sold units data for all bikes
        /// </summary>
        /// <returns></returns>
        public SoldUnitData GetLastSoldUnitData()
        {
            SoldUnitData dataObj = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    dataObj = new SoldUnitData();
                    cmd.CommandText = "getlastsoldunitdate";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_lastUpdateDate", DbType.DateTime, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_isEmailToSend", DbType.Int16, ParameterDirection.Output));

                    MySqlDatabase.ExecuteNonQuery(cmd, ConnectionType.ReadOnly);

                    if (cmd.Parameters["par_lastUpdateDate"].Value != DBNull.Value)
                        dataObj.LastUpdateDate = Convert.ToDateTime(cmd.Parameters["par_lastUpdateDate"].Value);

                    if (!string.IsNullOrEmpty(Convert.ToString(cmd.Parameters["par_isEmailToSend"].Value)))
                        dataObj.IsEmailToSend = (Convert.ToInt16(cmd.Parameters["par_isEmailToSend"].Value) == 1) ? true : false;

                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("BikewaleOpr.DALs.Bikedata.GetLastSoldUnitData"));
            }
            return dataObj;
        }
        /// <summary>
        /// Created by Sangram Nandkhile on 06 Mar 2017
        /// Desc : function to get the used bikes where no model image is found
        /// Modified by : Sajal Gupta on 21-03-2017
        /// Description : Changed function to send notification once in a day
        /// </summary>
        /// </summary>
        /// <returns></returns>
        public UsedBikeImagesNotificationData GetPendingUsedBikesWithoutModelImage()
        {
            UsedBikeImagesNotificationData objImagesData = null;
            IList<UsedBikeImagesModel> _objBikeModels = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("getPendingUsedBikesWithoutModelImage"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            objImagesData = new UsedBikeImagesNotificationData();

                            _objBikeModels = new List<UsedBikeImagesModel>();
                            while (dr.Read())
                            {
                                UsedBikeImagesModel _objBike = new UsedBikeImagesModel();

                                _objBike.MakeId = SqlReaderConvertor.ToInt32(dr["makeid"]);
                                _objBike.MakeName = Convert.ToString(dr["makeName"]);
                                _objBike.ModelName = Convert.ToString(dr["modelname"]);

                                _objBikeModels.Add(_objBike);
                            }
                            objImagesData.Bikes = _objBikeModels;

                            if (dr.NextResult())
                            {
                                if (dr.Read())
                                {
                                    objImagesData.IsNotify = SqlReaderConvertor.ToBoolean(dr["sendNotification"]);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "BikewaleOpr.DALs.Bikedata.GetPendingUsedBikesWithoutModelImage()");
            }
            return objImagesData;
        }

        /// <summary>
        ///  Created by Sajal Gupta on 03-03-2017
        /// Des : function to save used bike model images to db and get saved photo id. 
        /// </summary>
        public uint FetchPhotoId(UsedBikeModelImageEntity objModelImageEntity)
        {
            uint photoId = 0;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("opr_saveUsedBikeImage"))
                {

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_modelid", DbType.Int32, objModelImageEntity.Modelid));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_userid", DbType.Int32, objModelImageEntity.UserId));

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.MasterDatabase))
                    {
                        if (dr != null)
                        {

                            while (dr.Read())
                            {
                                photoId = SqlReaderConvertor.ToUInt32(dr["Id"]);
                            }
                            dr.Close();
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "BikewaleOpr.DALs.Bikedata.BikeModelsRepository.FetchPhotoId");
            }

            return photoId;
        }

        /// <summary>
        ///  Created by Sajal Gupta on 03-03-2017
        /// Des : function to delete used bike model image .
        /// </summary>
        public bool DeleteUsedBikeModelImage(uint modelId)
        {
            bool isDeleted = false;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("opr_deleteUsedBikeImage"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_modelid", DbType.Int32, modelId));
                    MySqlDatabase.ExecuteNonQuery(cmd, ConnectionType.ReadOnly);
                    isDeleted = true;
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "BikewaleOpr.DALs.Bikedata.BikeModelsRepository.FetchPhotoId");
            }
            return isDeleted;
        }


        /// <summary>
        ///  Created by Sajal Gupta on 06-03-2017
        /// Des : function to get used bike model image data by make
        /// </summary>
        public IEnumerable<UsedBikeModelImageData> GetUsedBikeModelImageByMake(uint makeId)
        {
            IList<UsedBikeModelImageData> objImageList = null;

            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("getusedbikemodelimagebymake"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_makeid", DbType.Int32, makeId));

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.MasterDatabase))
                    {
                        if (dr != null)
                        {
                            objImageList = new List<UsedBikeModelImageData>();
                            while (dr.Read())
                            {
                                UsedBikeModelImageData objImage = new UsedBikeModelImageData();
                                objImage.ModelId = SqlReaderConvertor.ToUInt32(dr["modelid"]);
                                objImage.MakeId = SqlReaderConvertor.ToUInt32(dr["makeid"]);
                                objImage.ModelName = Convert.ToString(dr["modelname"]);
                                objImage.MakeName = Convert.ToString(dr["makename"]);
                                objImage.HostUrl = Convert.ToString(dr["HostURL"]);
                                objImage.OriginalImagePath = Convert.ToString(dr["OriginalImagePath"]);
                                objImageList.Add(objImage);
                            }
                            dr.Close();
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("BikewaleOpr.DALs.Bikedata.BikeModelsRepository.GetUsedBikeModelImageByMake makeId {0}", makeId));
            }

            return objImageList;
        }


        /// <summary>
        /// Written By : Ashish G. Kamble on 17 Apr 2017
        /// Summary : Function to get the list of bikemodels for the make
        /// </summary>
        /// <param name="makeId"></param>
        /// <param name="requestType"></param>
        /// <returns></returns>
        public IEnumerable<BikeModelEntityBase> GetModels(uint makeId, ushort requestType)
        {
            IEnumerable<BikeModelEntityBase> objModels = null;

            try
            {
                using (IDbConnection connection = DatabaseHelper.GetMasterConnection())
                {
                    connection.Open();

                    var param = new DynamicParameters();

                    param.Add("par_makeid", makeId);
                    param.Add("par_requesttype", requestType);

                    objModels = connection.Query<BikeModelEntityBase>("getbikemodels_new_1704442017", param: param, commandType: CommandType.StoredProcedure);

                    if (connection.State == ConnectionState.Open)
                        connection.Close();
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "BikewaleOpr.DALs.UserReviews.GetModels");
            }

            return objModels;
        }

        /// <summary>
        /// Created By : Deepak Israni on 14 March 2018
        /// Description: Returns all the models of a sepecific make.
        /// </summary>
        /// <param name="makeId"></param>
        /// <returns></returns>
        public IEnumerable<BikeModelEntityBase> GetModelsByMake(uint makeId)
        {
            IList<BikeModelEntityBase> models = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("getmodelsbymake"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_makeid", DbType.UInt32, makeId));

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            models = new List<BikeModelEntityBase>();
                            BikeModelEntityBase obj;
                            while (dr.Read())
                            {
                                obj = new BikeModelEntityBase();
                                obj.ModelId = SqlReaderConvertor.ToInt32(dr["ModelId"]);
                                obj.ModelName = Convert.ToString(dr["ModelName"]);
                                obj.MaskingName = Convert.ToString(dr["ModelMasking"]);
                                models.Add(obj);
                            }
                            dr.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("BikewaleOpr.DALs.GetModelsByMake : makeId {0}", makeId));
            }
            return models;
        }

        /// <summary>
        /// Created by : Aditi Srivastava on 23 May 2017
        /// Summary    : Get models information for a particular makeId
        /// </summary>
        public IEnumerable<BikeModelMailEntity> GetModelsByMake(uint makeId, string hostUrl, string oldMakeMasking, string newMakeMasking)
        {
            ICollection<BikeModelMailEntity> models = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("getmodelsbymake"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_makeid", DbType.UInt32, makeId));

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            models = new Collection<BikeModelMailEntity>();
                            BikeModelMailEntity obj = new BikeModelMailEntity();
                            obj.OldUrl = string.Format("{0}/{1}-bikes/", hostUrl, oldMakeMasking);
                            obj.NewUrl = string.Format("{0}/{1}-bikes/", hostUrl, newMakeMasking);
                            models.Add(obj);
                            while (dr.Read())
                            {
                                obj = new BikeModelMailEntity();
                                obj.ModelId = Convert.ToInt32(dr["ModelId"]);
                                obj.ModelName = Convert.ToString(dr["ModelName"]);
                                obj.MaskingName = Convert.ToString(dr["ModelMasking"]);
                                obj.OldUrl = string.Format("{0}/{1}-bikes/{2}/", hostUrl, oldMakeMasking, obj.MaskingName);
                                obj.NewUrl = string.Format("{0}/{1}-bikes/{2}/", hostUrl, newMakeMasking, obj.MaskingName);
                                models.Add(obj);
                            }
                            dr.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("BikewaleOpr.DALs.GetModelsByMake : makeId {0}", makeId));
            }
            return models;
        }

        #region GetModelsWithMissingColorImage function
        /// <summary>
        /// Created By : vivek singh tomar on 27/07/2017
        /// Summary : Function to fetch the list of models whose color images are not uploaded
        /// </summary>
        /// <returns></returns>
        public IEnumerable<BikeMakeModelData> GetModelsWithMissingColorImage()
        {
            IEnumerable<BikeMakeModelData> objBikeDataList = null;

            try
            {
                using (IDbConnection connection = DatabaseHelper.GetMasterConnection())
                {
                    connection.Open();

                    objBikeDataList = connection.Query<BikeModelEntityBase, BikeMakeEntityBase, BikeMakeModelData>
                        (
                            "getmodelswithmissingcolorimage",
                            (bikeModelEntityBase, bikeMakeEntityBase) =>
                            {
                                BikeMakeModelData bikeData = new BikeMakeModelData()
                                {
                                    BikeMake = bikeMakeEntityBase,
                                    BikeModel = bikeModelEntityBase
                                };
                                return bikeData;
                            }, splitOn: "MakeId", commandType: CommandType.StoredProcedure
                        );

                    if (connection != null && connection.State == ConnectionState.Open)
                    {
                        connection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("BikewaleOpr.DALs.Bikedata.BikeModelsRepository.GetModelsWithMissingColorImage"));
            }

            return objBikeDataList;
        }
        #endregion

        /// <summary>
        ///  Created By: Ashutosh Sharma on 27-07-2017
        /// Description: Update used bike as sold in 'classifiedindividualsellinquiries' table.
        /// </summary>
        /// <param name="inquiryId"></param>
        public bool UpdateInquiryAsSold(uint inquiryId)
        {
            int rowsAffected = 0;
            try
            {
                var param = new DynamicParameters();
                param.Add("par_inquiryId", inquiryId);

                using (IDbConnection connection = DatabaseHelper.GetMasterConnection())
                {
                    rowsAffected = connection.Execute("classified_updatelistingassold", param: param, commandType: CommandType.StoredProcedure);
                }

            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("BikewaleOpr.DALs.UpdateAsSoldInquiry : inquiryId {0}", inquiryId));
            }
            return rowsAffected > 0;
        }

        #region GetVersionsByModel function
        /// <summary>
        /// Created by : Vivek Singh Tomar on 7th Aug 2017
        /// Summary : Function to fetch list of versions respect to their model id
        /// </summary>
        /// <param name="modelId"></param>
        /// <returns></returns>
        public IEnumerable<BikeVersionEntityBase> GetVersionsByModel(EnumBikeType requestType, uint modelId)
        {
            IEnumerable<BikeVersionEntityBase> objBikeVersionEntityBaseList = null;
            try
            {
                using (IDbConnection connection = DatabaseHelper.GetMasterConnection())
                {
                    var param = new DynamicParameters();
                    param.Add("par_modelid", modelId);
                    param.Add("par_requesttype", requestType);
                    param.Add("par_cityid", 0);
                    connection.Open();
                    objBikeVersionEntityBaseList = connection.Query<BikeVersionEntityBase>("getbikeversions_new", param: param, commandType: CommandType.StoredProcedure);
                    if (connection != null && connection.State == ConnectionState.Open)
                    {
                        connection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "BikewaleOpr.DALs.BikeData.BikeModelsRepository.GetVersionsByMake");
            }
            return objBikeVersionEntityBaseList;
        }
        #endregion

        /// <summary>
        /// Created by  :   Sumit Kate on 21 Nov 2017
        /// Description :   Check if masking name exists in model table
        /// </summary>
        /// <param name="modelMaskingName"></param>
        /// <returns></returns>
        public bool IsModelMaskingNameExists(uint makeId, string modelMaskingName)
        {
            bool isExists = false;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandText = "ismodelmaskingnameexists";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_makeId", DbType.Int32, makeId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_maskingname", DbType.String, modelMaskingName));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_ismaskingexist", DbType.Int16, ParameterDirection.Output));

                    MySqlDatabase.ExecuteNonQuery(cmd, ConnectionType.MasterDatabase);
                    isExists = Bikewale.Utility.SqlReaderConvertor.ToBoolean(cmd.Parameters["par_ismaskingexist"].Value);
                }
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass.LogError(ex, string.Format("BikewaleOpr.DALs.IsModelMaskingNameExists({0})", modelMaskingName));
            }
            return isExists;
        }


        /// <summary>
        /// Created by  : Sanskar Gupta on 04 Jan 2018
        /// Description : This function will fetch a list of all the Newly Launched bikes eligible for getting promoted. It will use the SP "getbikesforpromotion" to get the entries from the database.
        /// </summary>
        public IEnumerable<BikeMakeModelData> GetPromotionBikes() {
            ICollection<BikeMakeModelData> newLaunchesToBePromoted = null;

            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("getbikesforpromotion"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            while (dr.Read())
                            {
                                if(newLaunchesToBePromoted == null)
                                    newLaunchesToBePromoted = new Collection<BikeMakeModelData>();

                                BikeMakeModelData obj = new BikeMakeModelData();
                                obj.BikeMake = new BikeMakeEntityBase();
                                obj.BikeModel = new BikeModelEntityBase();
                                obj.BikeMake.MakeId = Convert.ToInt32(dr["BikeMakeId"]);
                                obj.BikeModel.ModelId = Convert.ToInt32(dr["id"]);
                                obj.BikeModel.ModelName = Convert.ToString(dr["Name"]);
                                newLaunchesToBePromoted.Add(obj);
                            }
                            dr.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("BikewaleOpr.DALs.GetPromotionBikes"));
            }
            return newLaunchesToBePromoted;
        }

		#region SavePageViews method to update the models page views from google analytics into database
		/// <summary>		
		/// Written By : Ashish G. Kamble on 2 Feb 2018
		/// Summary : Function to update the models page views from google analytics into database		
		/// </summary>
		/// <param name="modelsList">value should be ModelId1:Views1, ModelId2:Views2 (22:352,686:1265) only</param>
		/// <returns>Number of rows affected</returns>
		public bool SavePageViews(string modelsList)
		{
			bool isSuccess = false;

			try
			{
				using (IDbConnection connection = DatabaseHelper.GetMasterConnection())
				{
					var param = new DynamicParameters();
					param.Add("par_modelsList", modelsList);

					connection.Execute("savemodelpageviews", param: param, commandType: CommandType.StoredProcedure);
					isSuccess = true;
				}
			}
			catch (Exception ex)
			{
				ErrorClass.LogError(ex, string.Format("BikewaleOpr.DALs.SavePageViews Input Data -> {0}", modelsList));
			}

			return isSuccess;
		} 
		#endregion


        /// <summary>
        /// Created By : Deepak Israni on 22 Feb 2018
        /// Description: To get the model id list respective to the version ids.
        /// </summary>
        /// <param name="versions"></param>
        /// <returns></returns>
        public string GetModelsByVersions(string versions)
        {
            String spName = "getmodelfromversion";

            string models = "";

            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand(spName))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_versionids", DbType.String, versions));

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.MasterDatabase))
                    {
                        if (dr != null)
                        {
                            while (dr.Read())
                            {
                                models += string.Format("{0},", SqlReaderConvertor.ToUInt32(dr["ModelId"]));
                            }

                            dr.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("BikewaleOpr.DAL.GetModelsByVersions: Versions- {0}", versions));
            }

            return models;
        }
    }
}



