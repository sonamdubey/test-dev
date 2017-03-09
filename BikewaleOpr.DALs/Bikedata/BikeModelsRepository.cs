
using Bikewale.Notifications;
using Bikewale.Utility;
using BikewaleOpr.Entities.BikeData;
using BikewaleOpr.Entity.BikeData;
using BikewaleOpr.Interface.BikeData;
using MySql.CoreDAL;
using System;
using System.Collections.Generic;
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
                ErrorClass objErr = new ErrorClass(ex, string.Format("BikewaleOpr.DALs.Bikedata.GetModels_Make_{0}_RequestType_{1}", makeId, requestType));
                objErr.SendMail();
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
                ErrorClass objErr = new ErrorClass(ex, string.Format("BikewaleOpr.DALs.Bikedata.SaveModelUnitSold-{0}-{1}", list, date));
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
                ErrorClass objErr = new ErrorClass(ex, string.Format("BikewaleOpr.DALs.Bikedata.GetLastSoldUnitData"));
            }
            return dataObj;
        }
        /// <summary>
        /// Created by Sangram Nandkhile on 06 Mar 2017
        /// Desc : function to get the used bikes where no model image is found
        /// </summary>
        /// </summary>
        /// <returns></returns>
        public IEnumerable<UsedBikeImagesModel> GetPendingUsedBikesWithoutModelImage()
        {
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
                            _objBikeModels = new List<UsedBikeImagesModel>();
                            while (dr.Read())
                            {
                                UsedBikeImagesModel _objBike = new UsedBikeImagesModel();

                                _objBike.MakeId = SqlReaderConvertor.ToInt32(dr["makeid"]);
                                _objBike.MakeName = Convert.ToString(dr["makeName"]);
                                _objBike.ModelName = Convert.ToString(dr["modelname"]);

                                _objBikeModels.Add(_objBike);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "BikewaleOpr.DALs.Bikedata.GetPendingUsedBikesWithoutModelImage()");
            }
            return _objBikeModels;
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
                ErrorClass objErr = new ErrorClass(ex, "BikewaleOpr.DALs.Bikedata.BikeModelsRepository.FetchPhotoId");
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
                ErrorClass objErr = new ErrorClass(ex, "BikewaleOpr.DALs.Bikedata.BikeModelsRepository.FetchPhotoId");
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
                ErrorClass objErr = new ErrorClass(ex, string.Format("BikewaleOpr.DALs.Bikedata.BikeModelsRepository.GetUsedBikeModelImageByMake makeId {0}", makeId));
            }

            return objImageList;
        }

    }
}



