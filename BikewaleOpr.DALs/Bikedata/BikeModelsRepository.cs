
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
    public class BikeModelsRepository : IBikeModels
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

                    MySqlDatabase.InsertQuery(cmd, ConnectionType.MasterDatabase);

                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, string.Format("BikewaleOpr.DALs.Bikedata.SaveModelUnitSold-{0}-{1}", list, date));
                objErr.SendMail();
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
                objErr.SendMail();
            }
            return dataObj;
        }
    }
}



