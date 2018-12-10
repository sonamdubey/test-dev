using Carwale.DAL.CoreDAL.MySql;
using Carwale.Entity.Common;
using Carwale.Entity.Geolocation;
using Carwale.Entity.Geolocation.LatLongURI;
using Carwale.Interfaces.Geolocation;
using Carwale.Notifications;
using Carwale.Notifications.Logs;
using Carwale.Utility;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Web;

namespace Carwale.DAL.Geolocation
{
    public class GeoCitiesRepository : RepositoryBase, IGeoCitiesRepository
    {
        public IEnumerable<Cities> GetCities(Modules module = Modules.Default, bool? isPopular = null)
        {
            IEnumerable<Cities> cities = null;
            try
            {
                var param = new DynamicParameters();
                param.Add("v_moduleid", module, DbType.Int16);
                param.Add("v_ispopular", isPopular, DbType.Int16);

                using (var con = CarDataMySqlReadConnection)
                {
                    cities = con.Query<Cities>("GetCities", param, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return cities;
        }

        /// <summary>
        /// Get Price Quote Cities Based on modelId and stateId
        /// Written By : Ashish Verma on 2/6/2014
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public List<City> GetPQCitiesByStateIdAndModelId(int modelId, int stateId)
        {
            var cityList = new List<City>();
            using (DbCommand cmd = DbFactory.GetDBCommand("GetPqCitiesByModelIdAndStateId_v16_11_7"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(DbFactory.GetDbParam("v_ModelId", DbType.Int32, modelId));
                cmd.Parameters.Add(DbFactory.GetDbParam("v_StateId", DbType.Int32, stateId));
                using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, DbConnections.NewCarMySqlReadConnection))
                {
                    try
                    {
                        while (dr.Read())
                        {
                            cityList.Add(new City()
                            {
                                CityId = Convert.ToInt32(dr["CityId"]),
                                CityName = dr["CityName"].ToString()
                            });
                        }
                    }
                    catch (Exception err)
                    {
                        var exception = new ExceptionHandler(err, HttpContext.Current.Request.ServerVariables["URL"]);
                        exception.LogException();
                    }
                }
            }
            return cityList;
        }

        public List<City> GetCitiesByType(string type)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get Price Quote Cities Based on modelId 
        /// Written By : Ashish Verma on 2/6/2014
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public List<City> GetPQCitiesByModelId(int modelId)
        {
            var cityList = new List<City>();

            using (var cmd = DbFactory.GetDBCommand("GetPqCitiesByModelId_v16_11_7"))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(DbFactory.GetDbParam("v_ModelId", DbType.Int16, modelId));
                try
                {
                    using (var dr = MySqlDatabase.SelectQuery(cmd,DbConnections.NewCarMySqlReadConnection))
                    {
                        while (dr.Read())
                        {
                            cityList.Add(new City()
                            {
                                CityId = Convert.ToInt32(dr["CityId"]),
                                CityName = dr["CityName"].ToString(),
                                StateId = CustomParser.parseIntObject(dr["StateId"]),
                                StateName = dr["StateName"].ToString()
                            });
                        }
                    }
                }
                catch (Exception err)
                {
                    var exception = new ExceptionHandler(err, HttpContext.Current.Request.ServerVariables["URL"]);
                    exception.LogException();
                    throw;
                }
            }
            return cityList;
        }


        /// <summary>
        /// Get Price Quote States Based on modelId
        /// Written By : Ashish Verma on 2/6/2014
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public List<States> GetPQStatesByModelId(int modelId)
        {
            var stateList = new List<States>();
            using (DbCommand cmd = DbFactory.GetDBCommand("GetPqStateByModelId_v16_11_7"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(DbFactory.GetDbParam("v_ModelId", DbType.Int32, modelId));
                using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, DbConnections.NewCarMySqlReadConnection))
                {
                    try
                    {
                        while (dr.Read())
                        {
                            stateList.Add(new States()
                            {
                                StateId = Convert.ToInt32(dr["StateId"]),
                                StateName = dr["StateName"].ToString()
                            });
                        }
                    }
                    catch (Exception err)
                    {
                        var exception = new ExceptionHandler(err, HttpContext.Current.Request.ServerVariables["URL"]);
                        exception.LogException();
                    }
                }
            }
            return stateList;
        }

        /// <summary>
        /// Get Price Quote Zones Based on modelId and cityId
        /// Written By : Ashish Verma on 2/6/2014
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public List<Zones> GetPQCityZones(int cityId, int modelId)
        {
            var zoneList = new List<Zones>();
            using (DbCommand cmd = DbFactory.GetDBCommand("GetPqZones_V16_11_7"))
            {

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(DbFactory.GetDbParam("v_ModelId", DbType.Int32, modelId));
                cmd.Parameters.Add(DbFactory.GetDbParam("v_CityId", DbType.Int32, cityId));
                using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, DbConnections.NewCarMySqlReadConnection))
                {
                    try
                    {
                        while (dr.Read())
                        {
                            zoneList.Add(new Zones()
                            {
                                ZoneId = Convert.ToInt32(dr["ZoneId"]),
                                ZoneName = dr["ZoneName"].ToString(),
                                CityId = Convert.ToInt32(dr["CityId"]),
                                CityName = dr["CityName"].ToString()
                            });
                        }
                    }
                    catch (Exception err)
                    {
                        var exception = new ExceptionHandler(err, HttpContext.Current.Request.ServerVariables["URL"]);
                        exception.LogException();
                    }
                }
            }
            return zoneList;
        }

        /// <summary>
        /// Get Price Quote popular Cities Based on modelId 
        /// Written By : Ashish Verma on 2/6/2014
        /// Modified by Rohan Sapkal 20-03-2014 ,new SP,new Entity
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public List<PopularCity> GetPQPopularCities(int modelId)
        {
            var cityList = new List<PopularCity>();

            using (DbCommand cmd = DbFactory.GetDBCommand("GetPQPopularCitiesByModelId_v16_11_7"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(DbFactory.GetDbParam("v_ModelId", DbType.Int16, modelId));
                using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, DbConnections.NewCarMySqlReadConnection))
                {
                    try
                    {
                        while (dr.Read())
                        {
                            cityList.Add(new PopularCity()
                            {
                                CityId = Convert.ToInt32(dr["CityId"] != DBNull.Value ? dr["CityId"] : 0),
                                CityName = dr["CityName"] != DBNull.Value ? dr["CityName"].ToString() : "",
                                DisplayText = string.IsNullOrWhiteSpace(dr["DisplayText"].ToString()) ? "" : dr["DisplayText"].ToString()
                            });
                        }
                    }
                    catch (Exception err)
                    {
                        var exception = new ExceptionHandler(err, HttpContext.Current.Request.ServerVariables["URL"]);
                        exception.LogException();
                    }
                }
            }
            return cityList;
        }

        public string GetCityNameById(string cityId)
        {
            string CityName = "";

            if (!RegExValidations.IsNumeric(cityId))
                return "";

            try
            {
                var param = new DynamicParameters();
                param.Add("v_CityId", Convert.ToInt32(cityId) > 0 ? Convert.ToInt32(cityId) : 0);
                using (var con = CarDataMySqlReadConnection)
                {
                    CityName = con.Query<string>("GetCityNameByCityId_v16_11_7", param, commandType: CommandType.StoredProcedure).FirstOrDefault();
                }
            }
            catch (Exception err)
            {
                var exception = new ExceptionHandler(err, HttpContext.Current.Request.ServerVariables["URL"]);
                exception.LogException();
                throw;
            }
            return CityName;
        }

        /// <summary>
        /// to get list of all states
        /// </summary>
        /// <returns></returns>
        public List<States> GetStates()
        {
            var stateList = new List<States>();
            try
            {
                using (var Con = CarDataMySqlReadConnection)
                {
                    stateList = Con.Query<States>("cwmasterdb.GetAllStates_v16_11_7", null, commandType: CommandType.StoredProcedure).ToList();
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "GeoCitiesRepository.GetStates()");
                objErr.LogException();
            }

            return stateList;
        }


        public List<City> GetCitiesByStateId(int stateId)
        {
            var cityList = new List<City>();
            stateId = CustomParser.parseIntObject(stateId) <= 0 ? 0 : stateId;

            try
            {
                var param = new DynamicParameters();
                param.Add("v_StateId",stateId > 0 ? stateId : 0);
                using (var Con = CarDataMySqlReadConnection)
                {
                    cityList = Con.Query<City>("cwmasterdb.GetCitiesByStateId_v16_11_7 ", param, commandType: CommandType.StoredProcedure).ToList();
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "GeoCitiesRepository.GetCitiesByStateId()");
                objErr.LogException();
            }
            return cityList;
        }


        /// <summary>
        /// Created by  : Ashish Verma
        /// Date        : September 21, 2014
        /// Description : Gets the cityName,zoneName based on cityId and zoneId
        /// </summary>
        public CustLocation GetCustLocation(int cityId, string zoneId)
        {
            var custLocation = new CustLocation();
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("cwmasterdb.GetCustCityInfo_API_v16_11_7"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_CityId", DbType.Int16, cityId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_ZoneId", DbType.Int16, CustomParser.parseIntObject(zoneId)));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_CityName", DbType.String, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_ZoneName", DbType.String, ParameterDirection.Output));
                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, DbConnections.CarDataMySqlReadConnection))
                    {
                        if (dr.Read())
                        {
                            LogLiveSps.LogMySqlSpInGrayLog(cmd);

                            custLocation.CityId = cityId;
                            custLocation.CityName = dr["v_CityName"].ToString();
                            custLocation.ZoneId = string.IsNullOrEmpty(zoneId) ? "" : zoneId;
                            custLocation.ZoneName = dr["v_ZoneName"].ToString();
                        }
                    }
                }
            }
            catch (Exception err)
            {
                var exception = new ExceptionHandler(err, HttpContext.Current.Request.ServerVariables["URL"]);
                exception.LogException();
                throw;
            }
            return custLocation;
        }

        /// <summary>
        /// Returns all the PQ zones and groupcities together as zones for the modelId passed
        /// Written By: Shalini Nair on 14/04/2016
        /// </summary>
        /// <param name="modelId"></param>
        /// <returns></returns>
        [Obsolete("Please use GetPQZones and GetPQCityGroups instead")]
        public List<Zone> GetPQCityZonesList(int modelId)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("v_ModelId", modelId, dbType: DbType.Int32, direction: ParameterDirection.Input);
                using (var con = NewCarMySqlReadConnection)
                {
                    return con.Query<Zone>("GetPqCityZonesByModelId_V16_11_7", param, commandType: CommandType.StoredProcedure).AsList();
                }
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
                throw;
            }
        }

        /// <summary>
        /// Written By: Shalini Nair on 13/05/2015
        /// Returns the City Details based on Latitude and Longitude passed
        /// </summary>
        /// <param name="querystring"></param>
        /// <returns></returns>
        public City GetCityDetailsByLatLong(LatLongURI querystring)
        {
            var cityDetails = new City();
            try
            {
                var param = new DynamicParameters();
                param.Add("v_Latitude", querystring.Latitude);
                param.Add("v_Longitude", querystring.Longitude);
                using (var con = CarDataMySqlReadConnection)
                {
                    return con.Query<City>("cwmasterdb.GetGeoLocatedCity_v16_11_7", param, commandType: CommandType.StoredProcedure).First();
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "GeoCitiesRepository.GetCityDetailsByLatLong()");
                objErr.LogException();
            }

            return cityDetails;
        }

        /// <summary>
        /// Written By: Sachin Bharti on 10/10/2015
        /// Return the State details based on city
        /// </summary>
        /// <param name="querystring"></param>
        /// <returns></returns>
        public States GetStateByCityId(int cityId)
        {
            var state = new States();
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("cwmasterdb.GetCityDetails_v16_11_7"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_CityId", DbType.Int32, cityId));
                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, DbConnections.CarDataMySqlReadConnection))
                    {
                        if (dr.Read())
                        {
                            state.StateName = dr["State"].ToString();
                            state.StateId = CustomParser.parseIntObject(dr["StateId"]);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "GeoCitiesRepository.GetStateByCityId()");
                objErr.LogException();
            }

            return state;
        }

        /// <summary>
        /// Written By: Rohan Sapkal on 14/12/2015
        /// Return nearest Cities ordered by distance
        /// </summary>
        /// <param name="querystring"></param>
        /// <returns></returns>
        public List<City> GetNearestCities(int cityId, short count = 20)
        {
            var city = new List<City>();
            try
            {
                var param = new DynamicParameters();
                param.Add("v_cityid", cityId);
                param.Add("v_count", count);

                using (var con = NewCarMySqlReadConnection)
                {
                    return con.Query<City>("GetNearestCity_v16_11_7", param, commandType: CommandType.StoredProcedure).AsList();
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "GeoCitiesRepository.GetNearestCity()");
                objErr.LogException();
            }
            return city;
        }

        public StateAndAllCities GetStateAndAllCities(int cityId)
        {
            DataSet ds = new DataSet();
            StateAndAllCities stateAndAllCities = new StateAndAllCities();
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("cwmasterdb.GetStateAndAllCities_v16_11_7"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_cityid", DbType.Int32, cityId));
                    ds = MySqlDatabase.SelectAdapterQuery(cmd,DbConnections.CarDataMySqlReadConnection);
                }
                if (ds.Tables.Count == 2)
                {
                    stateAndAllCities.State = new States { StateId = int.Parse(ds.Tables[0].Rows[0]["Id"].ToString()), StateName = ds.Tables[0].Rows[0]["Name"].ToString() };
                    stateAndAllCities.Cities = new List<City>();
                    foreach (DataRow dr in ds.Tables[1].Rows)
                    {
                        stateAndAllCities.Cities.Add(new City { CityId = int.Parse(dr["Id"].ToString()), CityName = dr["Name"].ToString() });
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "GeoCitiesRepository.GetUsedPremiumSlotCount()");
                objErr.LogException();
            }
            return stateAndAllCities;
        }

        /// <summary>
        /// This function returns the zones where price for given modelId exists
        /// </summary>
        /// <param name="modelId">ModelId</param>
        /// <returns>List of Zones</returns>
        public List<Zone> GetPQZones(int modelId)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("v_ModelId", modelId, dbType: DbType.Int32, direction: ParameterDirection.Input);
                using (var con = NewCarMySqlReadConnection)
                {
                    return con.Query<Zone>("GetPqZonesByModelId_v16_11_7", param, commandType: CommandType.StoredProcedure).AsList();
                }
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
                return null;
            }
        }

        /// <summary>
        /// This function returns the cities which are  where price for modelId exists
        /// </summary>
        /// <param name="modelId"></param>
        /// <returns></returns>
        public List<City> GetPQGroupCities(int modelId)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("v_ModelId", modelId, dbType: DbType.Int32, direction: ParameterDirection.Input);
                using (var con = NewCarMySqlReadConnection)
                {
                    return con.Query<City>("GetPqCityGroupsByModelId_v16_11_7", param, commandType: CommandType.StoredProcedure).AsList();
                }
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
                return null;
            }
        }

        public List<City> GetMasterGroupCities()
        {
            try
            {
                using (var con = NewCarMySqlReadConnection)
                {
                    return con.Query<City>("FetchGroupMasterCities", commandType: CommandType.StoredProcedure).AsList();
                }
            }
            catch (Exception err)
            {
                Logger.LogException(err);
                return null;
            }
        }

        public List<City> GetAllGroupCities()
        {
            try
            {
                using (var con = NewCarMySqlReadConnection)
                {
                    return con.Query<City>("FetchAllGroupCities", commandType: CommandType.StoredProcedure).AsList();
                }
            }
            catch (Exception err)
            {
                Logger.LogException(err);
                return null;
            }
        }

        /// <summary>
        /// This function returns the cities which are  where price for modelId exists
        /// </summary>
        /// <param name="modelId"></param>
        /// <returns></returns>
        public bool IsAreaAvailable(int cityId)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("v_CityId", cityId);

                using (var con = CarDataMySqlReadConnection)
                {
                    LogLiveSps.LogSpInGrayLog("CheckAreaExistence_v16_11_7");
                    return con.Query<bool>("CheckAreaExistence_v16_11_7", param, commandType: CommandType.StoredProcedure).Single();
                }
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
                return false;
            }
        }

        public List<City> GetClassifiedPopularCities()
        {
            List<City> cityList = null;
            try
            {
                using (var con = ClassifiedMySqlReadConnection)
                {
                    cityList = con.Query<City>("GetPopularCities", commandType: CommandType.StoredProcedure).ToList();
                    LogLiveSps.LogSpInGrayLog("GetPopularCities");
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "GetPopularCities");
                objErr.SendMail();
            }
            return cityList;
        }
        public Cities GetCityDetailsById(int cityId)
        {
            Cities cityDetails = new Cities();
            try
            {
                var param = new DynamicParameters();
                param.Add("v_CityId", cityId);
                using (var con = CarDataMySqlReadConnection)
                {
                    cityDetails =  con.Query<Cities>("GetCityDetailsById_18_9_4", param,commandType: CommandType.StoredProcedure).SingleOrDefault();
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex, "GeoCityRepository.GetCityDetailsById(int cityId)");
            }
            return cityDetails;
        }

        public IEnumerable<Zone> GetZonesByCity(int id)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("v_cityId", id);
                using (var con = CarDataMySqlReadConnection)
                {
                    return con.Query<Zone>("cwmasterdb.GetCityZones_v16_11_7", param, commandType: CommandType.StoredProcedure).Select(t => { t.CityId = id; return t; }).ToList();
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex, System.Reflection.MethodBase.GetCurrentMethod().Name);
                return new List<Zone>();
            }
        }
        public List<AreaCode> GetAreaCodeByCity(int cityId)
        {
            List<AreaCode> areaCodes = null;
            try
            {
                var param = new DynamicParameters();
                param.Add("v_cityId", cityId);
                using (var con = CarDataMySqlReadConnection)
                {
                    areaCodes = con.Query<AreaCode>("GetCityAreaCode", param, commandType: CommandType.StoredProcedure).ToList();
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return areaCodes;
        }

        public List<Zone> GetZonesByState(int stateId)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("v_StateId", stateId, dbType: DbType.Int32, direction: ParameterDirection.Input);
                using (var con = NewCarMySqlReadConnection)
                {
                    return con.Query<Zone>("FetchZonesByState", param, commandType: CommandType.StoredProcedure).AsList();
                }
            }
            catch (Exception err)
            {
                Logger.LogException(err);
                return null;
            }
        }

        public City GetCityDetailsByMaskingName(string maskingName)
        {
            City cityDetails = null;
            try
            {

                var param = new DynamicParameters();
                param.Add("v_MaskingName", maskingName);
                using (var con = CarDataMySqlReadConnection)
                {
                    cityDetails = con.Query("GetCityDetailsByMaskingName_18_9_4", param, commandType: CommandType.StoredProcedure).Select(t => new City{
						CityId = (int)t.Id,
						CityName = (string)t.Name,
						CityMaskingName = (string)t.CityMaskingName,
						StateId = (int)t.StateId,
						StateName = (string)t.StateName,
						StateMaskingName = (string)t.StateMaskingName,
                        IsDuplicateCityName = (bool)t.IsDuplicateCityName
                    }).FirstOrDefault();
                }
            }
            catch (Exception err)
            {
                Logger.LogException(err);
                return null;
            }
            return cityDetails;
        }
    }
}
