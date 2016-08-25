using BikewaleOpr.Entities;
using Consumer;
using MySql.CoreDAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;

namespace Bikewale.DealerAreaCommuteDistance
{
    /// <summary>
    /// Created By : Sumit Kate on 12 Aug 2016
    /// Description : To Handle communitcation with DB.
    /// </summary>
    internal class CommuteDistanceRepository
    {
        /// <summary>
        /// Created by  :   Sumit Kate on 16 Aug 2016
        /// Description :   Get active campaign dealers of the city
        /// </summary>
        /// <param name="cityId"></param>
        /// <returns></returns>
        public IEnumerable<GeoLocationEntity> GetCampaignDealerByCity(int cityId)
        {
            IList<GeoLocationEntity> lstDealers = null;
            try
            {
                using (DbCommand command = DbFactory.GetDBCommand())
                {
                    command.CommandText = "bw_getcampaigndealersbycity";
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(DbFactory.GetDbParam("par_city", DbType.Int32, Convert.ToInt32(cityId)));
                    using (IDataReader objReader = MySqlDatabase.SelectQuery(command, ConnectionType.MasterDatabase))
                    {
                        if (objReader != null)
                        {
                            lstDealers = new List<GeoLocationEntity>();
                            while (objReader.Read())
                            {
                                lstDealers.Add(new GeoLocationEntity()
                                {
                                    Id = !Convert.IsDBNull(objReader["dealerid"]) ? Convert.ToUInt32(objReader["dealerid"]) : default(UInt16),
                                    Latitude = !Convert.IsDBNull(objReader["Lattitude"]) ? Convert.ToDouble(objReader["Lattitude"]) : default(double),
                                    Longitude = !Convert.IsDBNull(objReader["Longitude"]) ? Convert.ToDouble(objReader["Longitude"]) : default(double)
                                });
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                Logs.WriteErrorLog("GetCampaignDealerByCity " + ex.Message);
            }
            return lstDealers;
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 16 Aug 2016
        /// Description :   Updates the dealer-area distance mapping
        /// </summary>
        /// <param name="idType">if id value has dealerid, idType should be 1, if id value has areaid, idtype should be 2</param>
        /// <param name="id"></param>
        /// <param name="distanceMatrix"></param>
        /// <returns></returns>
        public bool UpdateDistances(UInt16 idType, UInt32 id, string distanceMatrix)
        {
            bool isSuccess = false;
            if (Enumerable.Range(1, 2).Contains(idType))
            {
                try
                {
                    using (DbCommand command = DbFactory.GetDBCommand())
                    {
                        command.CommandText = "bw_savecommutedistancebyid";
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add(DbFactory.GetDbParam("par_idtype", DbType.Int32, Convert.ToInt32(idType)));
                        command.Parameters.Add(DbFactory.GetDbParam("par_id", DbType.Int32, id));
                        command.Parameters.Add(DbFactory.GetDbParam("par_distancematrix", DbType.String, distanceMatrix));
                        MySqlDatabase.ExecuteNonQuery(command, ConnectionType.MasterDatabase);
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    Logs.WriteErrorLog("UpdateDistances " + ex.Message);
                }
            }
            return isSuccess;
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 16 Aug 2016
        /// Description :   Gets Already mapped dealers to area
        /// </summary>
        /// <param name="areaId"></param>
        /// <returns></returns>
        public IEnumerable<GeoLocationEntity> GetDealersByArea(UInt32 areaId)
        {
            IList<GeoLocationEntity> lstDealers = null;
            try
            {
                using (DbCommand command = DbFactory.GetDBCommand())
                {
                    command.CommandText = "bw_getdealerbyarea";
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(DbFactory.GetDbParam("par_areaid", DbType.Int32, Convert.ToInt32(areaId)));

                    using (IDataReader objReader = MySqlDatabase.SelectQuery(command, ConnectionType.MasterDatabase))
                    {
                        if (objReader != null)
                        {
                            lstDealers = new List<GeoLocationEntity>();
                            while (objReader.Read())
                            {
                                lstDealers.Add(new GeoLocationEntity()
                                {
                                    Id = !Convert.IsDBNull(objReader["dealerid"]) ? Convert.ToUInt32(objReader["dealerid"]) : default(UInt32),
                                    Latitude = !Convert.IsDBNull(objReader["Lattitude"]) ? Convert.ToDouble(objReader["Lattitude"]) : default(double),
                                    Longitude = !Convert.IsDBNull(objReader["Longitude"]) ? Convert.ToDouble(objReader["Longitude"]) : default(double)
                                });
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Logs.WriteErrorLog("GetDealersByArea " + areaId + ex.Message);
            }
            return lstDealers;
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 16 Aug 2016
        /// Description :   Gets mapped Areas by dealer
        /// </summary>
        /// <param name="dealerID"></param>
        /// <returns></returns>
        public IEnumerable<GeoLocationEntity> GetAreasByDealer(UInt32 dealerID)
        {
            IList<GeoLocationEntity> lstArea = null;
            try
            {
                using (DbCommand command = DbFactory.GetDBCommand())
                {
                    command.CommandText = "bw_getareasbydealer";
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(DbFactory.GetDbParam("par_dealerid", DbType.Int32, Convert.ToInt32(dealerID)));

                    using (IDataReader objReader = MySqlDatabase.SelectQuery(command, ConnectionType.MasterDatabase))
                    {
                        if (objReader != null)
                        {
                            lstArea = new List<GeoLocationEntity>();
                            while (objReader.Read())
                            {
                                lstArea.Add(new GeoLocationEntity()
                                {
                                    Id = !Convert.IsDBNull(objReader["ID"]) ? Convert.ToUInt32(objReader["ID"]) : default(UInt32),
                                    Latitude = !Convert.IsDBNull(objReader["Lattitude"]) ? Convert.ToDouble(objReader["Lattitude"]) : default(double),
                                    Longitude = !Convert.IsDBNull(objReader["Longitude"]) ? Convert.ToDouble(objReader["Longitude"]) : default(double)
                                });
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Logs.WriteErrorLog("GetAreasByDealer " + dealerID + ex.Message);
            }
            return lstArea;
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 16 Aug 2016
        /// Description :   Checks whether Area Lat-Lon is changed
        /// </summary>
        /// <param name="areaId"></param>
        /// <param name="lattitude"></param>
        /// <param name="longitude"></param>
        /// <returns></returns>
        public bool IsAreaGeoLocationChanged(UInt32 areaId, double lattitude, double longitude)
        {
            bool isChanged = false;
            try
            {
                using (DbCommand command = DbFactory.GetDBCommand())
                {
                    command.CommandText = "isAreaLatLonChanged";
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(DbFactory.GetDbParam("par_areaId", DbType.Int32, Convert.ToInt32(areaId)));
                    command.Parameters.Add(DbFactory.GetDbParam("par_lattitude", DbType.Double, lattitude));
                    command.Parameters.Add(DbFactory.GetDbParam("par_longitude", DbType.Double, longitude));

                    using (IDataReader objReader = MySqlDatabase.SelectQuery(command, ConnectionType.MasterDatabase))
                    {
                        if (objReader != null)
                        {
                            if (objReader.Read())
                            {
                                isChanged = !Convert.IsDBNull(objReader["IsChanged"]) ? Convert.ToBoolean(objReader["IsChanged"]) : false;
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Logs.WriteErrorLog(string.Format("IsAreaGeoLocationChanged({0},{1},{2}) - {3}", areaId, lattitude, longitude, ex.Message));
            }
            return isChanged;
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 16 Aug 2016
        /// Description :   Checks whether Dealer Lat-Lon is changed
        /// </summary>
        /// <param name="dealerId"></param>
        /// <param name="lattitude"></param>
        /// <param name="longitude"></param>
        /// <returns></returns>
        public bool IsDealerGeoLocationChanged(UInt32 dealerId, double lattitude, double longitude)
        {
            bool isChanged = false;
            try
            {
                using (DbCommand command = DbFactory.GetDBCommand())
                {
                    command.CommandText = "isDealerLatLonChanged";
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(DbFactory.GetDbParam("par_dealerId", DbType.Int32, Convert.ToInt32(dealerId)));
                    command.Parameters.Add(DbFactory.GetDbParam("par_lattitude", DbType.Double, lattitude));
                    command.Parameters.Add(DbFactory.GetDbParam("par_longitude", DbType.Double, longitude));

                    using (IDataReader objReader = MySqlDatabase.SelectQuery(command, ConnectionType.MasterDatabase))
                    {
                        if (objReader != null)
                        {
                            if (objReader.Read())
                            {
                                isChanged = !Convert.IsDBNull(objReader["IsChanged"]) ? Convert.ToBoolean(objReader["IsChanged"]) : false;
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Logs.WriteErrorLog("IsDealerGeoLocationChanged " + ex.Message);
            }
            return isChanged;
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 16 Aug 2016
        /// Description :   Checks whether Area exists
        /// </summary>
        /// <param name="areaId"></param>
        /// <returns></returns>
        public bool IsAreaExists(UInt32 areaId)
        {
            bool isExists = false;
            try
            {
                using (DbCommand command = DbFactory.GetDBCommand())
                {
                    command.CommandText = "select id from areas where id = " + areaId;
                    command.CommandType = CommandType.Text;

                    using (IDataReader objReader = MySqlDatabase.SelectQuery(command, ConnectionType.MasterDatabase))
                    {
                        if (objReader != null)
                        {
                            if (objReader.Read())
                            {
                                isExists = !Convert.IsDBNull(objReader["id"]) ? Convert.ToInt32(objReader["id"]) > 0 : false;
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Logs.WriteErrorLog("IsAreaExists " + ex.Message);
            }
            return isExists;
        }
    }
}
