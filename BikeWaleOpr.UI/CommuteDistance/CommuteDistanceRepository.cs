﻿using BikewaleOpr.Entities;
using BikeWaleOpr.Common;
using MySql.CoreDAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;

namespace BikewaleOpr.CommuteDistance
{
    /// <summary>
    /// Created By : Lucky Rathore On 12 Apr 2016
    /// Description : To Handle communitcation with DB.
    /// </summary>
    internal class CommuteDistanceRepository
    {

        /// <summary>
        /// Created By : Lucky Rathore On 12 Apr 2016
        /// Description : To Get List of Delaers  whoes area mapping needed to be done.
        /// </summary>
        /// <returns>List of Delaers</returns>
        public IEnumerable<GeoLocationEntity> GetDealer()
        {
            GeoLocationEntity location = null;
            IList<GeoLocationEntity> dealers = null;
            try
            {
                using (DbCommand command = DbFactory.GetDBCommand())
                {
                    command.CommandText = "getcommutedistancedealers";
                    command.CommandType = CommandType.StoredProcedure;

                    using (IDataReader objReader = MySqlDatabase.SelectQuery(command, ConnectionType.ReadOnly))
                    {
                        if (objReader != null)
                        {
                            dealers = new List<GeoLocationEntity>();
                            while (objReader.Read())
                            {
                                location = new GeoLocationEntity();
                                location.Id = DBNull.Value == objReader["DealerId"] ? default(UInt16) : Convert.ToUInt16(objReader["DealerId"]);
                                location.Latitude = DBNull.Value == objReader["Lattitude"] ? 0 : Convert.ToDouble(objReader["Lattitude"]);
                                location.Longitude = DBNull.Value == objReader["Longitude"] ? 0 : Convert.ToDouble(objReader["Longitude"]);
                                dealers.Add(location);
                            }
                            Debug.WriteLine(String.Format("Total Records fetched : {0}", dealers.Count));
                        }
                        else
                        {
                            Debug.WriteLine("No Records fetched");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "GetDealer");
                
            }

            return dealers;
        }

        /// <summary>
        /// Created By : Lucky Rathore On 12 Apr 2016
        /// Description : To Get Area of city from which specfied dealer Belong.
        /// </summary>
        /// <param name="dealerId">e.g. 4</param>
        /// <returns>List of Area</returns>
        public IEnumerable<GeoLocationEntity> GetAreaByDealer(uint dealerId, ushort leadServingDistance, out GeoLocationEntity dealerLocation)
        {
            GeoLocationEntity location = null;
            IList<GeoLocationEntity> areas = null;
            GeoLocationEntity dealerLocationEntity = null;
            try
            {

                using (DbCommand command = DbFactory.GetDBCommand())
                {
                    command.CommandText = "getareasbydealer";
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(DbFactory.GetDbParam("par_dealerid", DbType.Int32, dealerId));
                    command.Parameters.Add(DbFactory.GetDbParam("par_leadservingdistance", DbType.Int16, leadServingDistance));


                    using (IDataReader objReader = MySqlDatabase.SelectQuery(command, ConnectionType.ReadOnly))
                    {
                        if (objReader != null)
                        {

                            if (objReader.Read())
                            {
                                dealerLocationEntity = new GeoLocationEntity();
                                dealerLocationEntity.Id = DBNull.Value == objReader["DealerId"] ? default(UInt32) : Convert.ToUInt32(objReader["DealerId"]);
                                dealerLocationEntity.Latitude = DBNull.Value == objReader["Lattitude"] ? 0 : Convert.ToDouble(objReader["Lattitude"]);
                                dealerLocationEntity.Longitude = DBNull.Value == objReader["Longitude"] ? 0 : Convert.ToDouble(objReader["Longitude"]);
                            }


                            if (objReader.NextResult())
                            {
                                areas = new List<GeoLocationEntity>();
                                while (objReader.Read())
                                {
                                    location = new GeoLocationEntity();
                                    location.Id = DBNull.Value == objReader["AreaId"] ? default(UInt32) : Convert.ToUInt32(objReader["AreaId"]);
                                    location.Latitude = DBNull.Value == objReader["Lattitude"] ? 0 : Convert.ToDouble(objReader["Lattitude"]);
                                    location.Longitude = DBNull.Value == objReader["Longitude"] ? 0 : Convert.ToDouble(objReader["Longitude"]);
                                    areas.Add(location);
                                }
                                Debug.WriteLine(String.Format("Total Records fetched : {0}", areas.Count));
                            }
                        }
                        else
                        {
                            Debug.WriteLine("No Records fetched");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "GetAreaByDealer");
                
            }
            finally
            {
                dealerLocation = dealerLocationEntity;
            }
            return areas;
        }

        /// <summary>
        /// Created By : Lucky Rathore On 12 Apr 2016
        /// Description : To update distance.
        /// </summary>
        /// <param name="dealerId">e.g. 4</param>
        /// <param name="areaDistance">e.g. '18890:51,18249:89,17768:79' (areaId:Distance comma seprated pair)</param>
        /// <returns></returns>
        public bool UpdateArea(UInt32 dealerId, string areaDistance)
        {
            int resp = 0;
            try
            {

                using (DbCommand command = DbFactory.GetDBCommand())
                {

                    command.CommandText = "updatecommutedistance";
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(DbFactory.GetDbParam("par_dealerid", DbType.Int32, dealerId));
                    command.Parameters.Add(DbFactory.GetDbParam("par_areadistance", DbType.String, areaDistance));

                    resp = MySqlDatabase.ExecuteNonQuery(command, ConnectionType.MasterDatabase);
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "UpdateArea");
                
            }

            return resp > 0 ? true : false;
        }
    }
}