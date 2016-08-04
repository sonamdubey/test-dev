using BikewaleOpr.Entities;
using Consumer;
using MySql.CoreDAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace Bikewale.DealerAreaCommuteDistance
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
        public IEnumerable<DealerEntity> FetchActiveContractDealer()
        {
            ICollection<DealerEntity> dealers = null;
            try
            {
                using (DbCommand command = DbFactory.GetDBCommand())
                {
                    command.CommandText = "getactivecontractdealer";
                    command.CommandType = CommandType.StoredProcedure;

                    using (IDataReader objReader = MySqlDatabase.SelectQuery(command, ConnectionType.MasterDatabase))
                    {
                        if (objReader != null)
                        {
                            dealers = new List<DealerEntity>();
                            while (objReader.Read())
                            {
                                dealers.Add(new DealerEntity()
                                {
                                    Id = !Convert.IsDBNull(objReader["dealerid"]) ? Convert.ToUInt16(objReader["dealerid"]) : default(UInt16),
                                    LeadServingRadius = !Convert.IsDBNull(objReader["servingradius"]) ? Convert.ToUInt16(objReader["servingradius"]) : default(UInt16),
                                });
                            }
                            Logs.WriteInfoLog(String.Format("Total Records fetched : {0}", dealers.Count));
                        }
                        else
                        {
                            Logs.WriteInfoLog("No Records fetched");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logs.WriteErrorLog("GetDealer " + ex.Message);
            }

            return dealers;
        }

        /// <summary>
        /// Created By : Lucky Rathore On 12 Apr 2016
        /// Description : To Get Area of city from which specfied dealer Belong.
        /// </summary>
        /// <param name="dealerId">e.g. 4</param>
        /// <returns>List of Area</returns>
        public IEnumerable<GeoLocationEntity> GetAreaByDealer(UInt16 dealerId, UInt16 leadServingDistance, out GeoLocationEntity dealerLocation)
        {
            GeoLocationEntity location = null;
            IList<GeoLocationEntity> areas = null;
            GeoLocationEntity dealerLocationEntity = null;
            try
            {

                using (DbCommand command = DbFactory.GetDBCommand())
                {
                    command.CommandText = "getareasbydealer_04082016";
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(DbFactory.GetDbParam("par_dealerid", DbType.Int32, dealerId));
                    command.Parameters.Add(DbFactory.GetDbParam("par_leadservingdistance", DbType.Byte, Convert.ToByte(leadServingDistance)));


                    using (IDataReader objReader = MySqlDatabase.SelectQuery(command, ConnectionType.MasterDatabase))
                    {
                        if (objReader != null)
                        {

                            if (objReader.Read())
                            {
                                dealerLocationEntity = new GeoLocationEntity();
                                dealerLocationEntity.Id = DBNull.Value == objReader["DealerId"] ? default(UInt16) : Convert.ToUInt16(objReader["DealerId"]);
                                dealerLocationEntity.Latitude = DBNull.Value == objReader["Lattitude"] ? 0 : Convert.ToDouble(objReader["Lattitude"]);
                                dealerLocationEntity.Longitude = DBNull.Value == objReader["Longitude"] ? 0 : Convert.ToDouble(objReader["Longitude"]);
                            }


                            if (objReader.NextResult())
                            {
                                areas = new List<GeoLocationEntity>();
                                while (objReader.Read())
                                {
                                    location = new GeoLocationEntity();
                                    location.Id = DBNull.Value == objReader["AreaId"] ? default(UInt16) : Convert.ToUInt16(objReader["AreaId"]);
                                    location.Latitude = DBNull.Value == objReader["Lattitude"] ? 0 : Convert.ToDouble(objReader["Lattitude"]);
                                    location.Longitude = DBNull.Value == objReader["Longitude"] ? 0 : Convert.ToDouble(objReader["Longitude"]);
                                    areas.Add(location);
                                }
                                Logs.WriteInfoLog(String.Format("Total Records fetched : {0}", areas.Count));
                            }
                        }
                        else
                        {
                            Logs.WriteInfoLog("No Records fetched");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logs.WriteErrorLog("GetAreaByDealer " + ex.Message);
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
        /// <param name="areaDistance">e.g. '18890:51,18249:89,17768:79' (areaId:CityId comma seprated pair)</param>
        /// <returns></returns>
        public bool UpdateArea(UInt16 dealerId, string areaDistance)
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

                    MySqlDatabase.ExecuteNonQuery(command, ConnectionType.MasterDatabase);
                    resp = 1;
                }
            }
            catch (Exception ex)
            {
                Logs.WriteErrorLog("UpdateArea " + ex.Message);
            }

            return resp > 0 ? true : false;
        }
    }
}
