
using Bikewale.DAL.CoreDAL;
using Bikewale.Notifications;
using Bikewale.Utility;
using BikewaleOpr.Entity;
using BikewaleOpr.Interface;
using Dapper;
using MySql.CoreDAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
namespace BikewaleOpr.DALs.AdOperation
{
    /// <summary>
    /// Created by : Snehal Dange on 2nd Jan 2018
    /// Descripion: AdOperations repository
    /// </summary>
    public class AdOperation : IAdOperation
    {

        public IEnumerable<PromotedBike> GetPromotedBikes()
        {
            IList<PromotedBike> _objPromotedBikeList = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("getadpromotedbikes"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_modelid", DbType.Int32, 0));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_makeid", DbType.Int16, 0));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_adoperationid", DbType.UInt16, 0));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_isactive", DbType.Boolean, 1));

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            _objPromotedBikeList = new List<PromotedBike>();
                            while (dr.Read())
                            {
                                PromotedBike _objBike = new PromotedBike();
                                _objBike.Make = new Entities.BikeData.BikeMakeEntityBase();
                                _objBike.Model = new Entities.BikeData.BikeModelEntityBase();
                                _objBike.PromotedBikeId = SqlReaderConvertor.ToUInt32(dr["adpromotionid"]);
                                _objBike.Make.MakeId = SqlReaderConvertor.ToInt32(dr["makeid"]);
                                _objBike.Model.ModelId = SqlReaderConvertor.ToInt32(dr["modelid"]);
                                _objBike.Make.MakeName = Convert.ToString(dr["makename"]);
                                _objBike.Model.ModelName = Convert.ToString(dr["modelname"]);
                                _objBike.AdOperationType = Convert.ToByte(dr["adoperationid"]);
                                _objBike.StartTime = SqlReaderConvertor.ToDateTime(dr["startdatetime"]);
                                _objBike.EndTime = SqlReaderConvertor.ToDateTime(dr["enddatetime"]);
                                _objBike.LastUpdatedBy = Convert.ToString(dr["username"]);
                                _objBike.UserId = SqlReaderConvertor.ToUInt32(dr["opruserid"]);
                                _objBike.ContractStatus = (ContractStatusEnum)Enum.Parse(typeof(ContractStatusEnum), Convert.ToString(dr["id"]));
                                _objPromotedBikeList.Add(_objBike);
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "BikewaleOpr.DALs.AdOperation.GetPromotedBikes");
            }
            return _objPromotedBikeList;
        }

        /// <summary>
        /// Created by : Snehal Dange on 2nd Jan 2018
        /// Description: Method to add promoted bike 
        /// </summary>
        /// <returns></returns>
        public bool SavePromotedBike(PromotedBike objPromotedBike)
        {
            bool status = false;
            try
            {
                using (IDbConnection connection = DatabaseHelper.GetMasterConnection())
                {
                    connection.Open();

                    var param = new DynamicParameters();
                    param.Add("par_adoperationid", objPromotedBike.AdOperationType);
                    param.Add("par_makeid", objPromotedBike.Make.MakeId);
                    param.Add("par_modelid", objPromotedBike.Model.ModelId);
                    param.Add("par_startdatetime", objPromotedBike.StartTime);
                    param.Add("par_enddatetime", objPromotedBike.EndTime);
                    param.Add("par_opruserid", objPromotedBike.UserId);
                    status = connection.Execute("insertadpromotedbike", param: param, commandType: CommandType.StoredProcedure) > 0;
                }


            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "BikewaleOpr.DALs.AdOperation.SavePromotedBike()");
            }
            return status;
        }

        /// <summary>
        /// Created by : Snehal Dange on 4th Jan 2018
        /// Description: Method to delete promoted bike( change status of promoted bike to 'inactive')
        /// </summary>
        /// <param name="objPromotedBike"></param>
        /// <returns></returns>
        public bool UpdatePromotedBike(PromotedBike objPromotedBike)
        {
            bool status = false;
            try
            {
                using (IDbConnection connection = DatabaseHelper.GetMasterConnection())
                {
                    connection.Open();

                    var param = new DynamicParameters();
                    param.Add("par_adpromotedbikeid", objPromotedBike.PromotedBikeId);
                    param.Add("par_startdatetime", objPromotedBike.StartTime);
                    param.Add("par_enddatetime", objPromotedBike.EndTime);
                    param.Add("par_opruserid", objPromotedBike.UserId);
                    param.Add("par_status", objPromotedBike.ContractStatus);
                    param.Add("par_adoperationid", objPromotedBike.AdOperationType);


                    status = connection.Execute("updateadpromotedbike", param: param, commandType: CommandType.StoredProcedure) > 0;
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("BikewaleOpr.DALs.AdOperation.UpdatePromotedBike():Id:{0}"
                    , objPromotedBike.PromotedBikeId));
            }
            return status;
        }
    }
}
