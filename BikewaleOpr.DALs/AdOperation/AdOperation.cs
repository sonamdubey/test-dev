
using Bikewale.Notifications;
using Bikewale.Utility;
using BikewaleOpr.Entity;
using BikewaleOpr.Entity.AdOperations;
using BikewaleOpr.Interface;
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
        /// <summary>
        /// Created by : Snehal Dange on 2nd Jan 2018
        /// Description: Method created to get all latest launched bikes list
        /// </summary>
        /// <returns></returns>
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
                                _objBike.AdOperationType = (AdOperationEnum)Convert.ToByte(dr["adoperationid"]);
                                _objBike.StartTime = SqlReaderConvertor.ToDateTime(dr["startdatetime"]);
                                _objBike.EndTime = SqlReaderConvertor.ToDateTime(dr["enddatetime"]);
                                _objBike.LastUpdatedBy = Convert.ToString(dr["username"]);
                                _objBike.LastUpdatedById = SqlReaderConvertor.ToUInt32(dr["opruserid"]);
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
                using (DbCommand cmd = DbFactory.GetDBCommand("insertadpromotedbike"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_adoperationid", DbType.UInt16, objPromotedBike.AdOperationType));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_makeid", DbType.UInt16, objPromotedBike.Make.MakeId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_modelid", DbType.UInt32, objPromotedBike.Model.ModelId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_startdatetime", DbType.DateTime, objPromotedBike.StartTime));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_enddatetime", DbType.DateTime, objPromotedBike.EndTime));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_opruserid", DbType.UInt16, objPromotedBike.LastUpdatedById));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_status", DbType.UInt16, objPromotedBike.ContractStatus));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_success", DbType.Byte, ParameterDirection.InputOutput));

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            if (dr.Read())
                            {

                                status = Convert.ToBoolean(cmd.Parameters["par_success"].Value);
                            }
                            dr.Close();
                        }
                    }
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
                using (DbCommand cmd = DbFactory.GetDBCommand("updateadpromotedbike"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_adpromotedbikeid", DbType.UInt16, objPromotedBike.PromotedBikeId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_opruserid", DbType.UInt16, objPromotedBike.LastUpdatedById));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_success", DbType.Byte, ParameterDirection.InputOutput));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_adoperationid", DbType.UInt16, objPromotedBike.AdOperationType));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_status", DbType.UInt16, objPromotedBike.ContractStatus));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_startdatetime", DbType.DateTime, objPromotedBike.StartTime));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_enddatetime", DbType.DateTime, objPromotedBike.EndTime));

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            if (dr.Read())
                            {
                                status = Convert.ToBoolean(cmd.Parameters["par_success"].Value);
                            }
                            dr.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("BikewaleOpr.DALs.AdOperation.DeletePromotedBike():Id:{0}"
                    , objPromotedBike.PromotedBikeId));
            }
            return status;
        }
    }
}
