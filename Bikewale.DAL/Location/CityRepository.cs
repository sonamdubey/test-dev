using Bikewale.Entities.BikeData;
using Bikewale.Entities.Location;
using Bikewale.Entities.Used;
using Bikewale.Interfaces.Location;
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
using System.Web;

namespace Bikewale.DAL.Location
{
	public class CityRepository : ICity,ICityRepository
	{
		/// <summary>
		/// Written By : Ashish G. Kamble on 3/8/2012
		/// Function returns city id and city names by providing state id and request type
		/// Modeified by : Pratibha Verma on 10 May 2018
		/// Description  : changed sp 'getcities' to 'getcities_10052018'
		/// </summary>
		/// <param name="stateId"></param>
		/// <param name="RequestType">Pass value All or PriceQuote</param>
		/// <returns></returns>
		public IEnumerable<CityEntityBase> GetAllCities(EnumBikeType requestType)
		{

			IList<CityEntityBase> objCityList = null;
			try
			{
				using (DbCommand cmd = DbFactory.GetDBCommand("getcities_10052018"))
				{
					cmd.CommandType = CommandType.StoredProcedure;
					cmd.Parameters.Add(DbFactory.GetDbParam("par_requesttype", DbType.Int32, requestType));
					cmd.Parameters.Add(DbFactory.GetDbParam("par_stateid", DbType.Int64, Convert.DBNull));


					objCityList = new List<CityEntityBase>();

					using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.MasterDatabase))
					{
						if (dr != null)
						{
							while (dr.Read())
							{
								objCityList.Add(new CityEntityBase
								{
									CityId = Convert.ToUInt32(dr["Value"]),
									CityName = Convert.ToString(dr["Text"]),
									CityMaskingName = Convert.ToString(dr["MaskingName"])
								});
							}
							dr.Close();
						}
					}
				}
			}
			catch (SqlException ex)
			{
				HttpContext.Current.Trace.Warn(ex.Message + ex.Source);
				ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);

			}
			catch (Exception ex)
			{
				HttpContext.Current.Trace.Warn(ex.Message + ex.Source);
				ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);

			}
			return objCityList;
		}   // End of GetCities method

		/// <summary>
		/// Written By : Ashish G. Kamble on 3/8/2012
		/// Function returns city id and city names by providing state id and request type
		/// </summary>
		/// <param name="stateId"></param>
		/// <param name="RequestType">Pass value All or PriceQuote</param>
		/// <returns></returns>
		public List<CityEntityBase> GetCities(string stateId, EnumBikeType requestType)
		{
			List<CityEntityBase> objCityList = null;

			try
			{
				using (DbCommand cmd = DbFactory.GetDBCommand("getcities"))
				{
					cmd.CommandType = CommandType.StoredProcedure;
					cmd.Parameters.Add(DbFactory.GetDbParam("par_requesttype", DbType.String, 20, requestType));
					cmd.Parameters.Add(DbFactory.GetDbParam("par_stateid", DbType.Int64, stateId));

					objCityList = new List<CityEntityBase>();

					using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
					{
						if (dr != null)
						{
							while (dr.Read())
							{
								objCityList.Add(new CityEntityBase
								{
									CityId = Convert.ToUInt32(dr["Value"]),
									CityName = Convert.ToString(dr["Text"]),
									CityMaskingName = Convert.ToString(dr["MaskingName"])
								});
							}
							dr.Close();
						}
					}
				}
			}
			catch (SqlException ex)
			{
				HttpContext.Current.Trace.Warn(ex.Message + ex.Source);
				ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);

			}
			catch (Exception ex)
			{
				HttpContext.Current.Trace.Warn(ex.Message + ex.Source);
				ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);

			}
			return objCityList;
		}   // End of GetCities method

		/// <summary>
		/// Created By : Sadhana Upadhyay on 21 July 2015
		/// Summary : to get PriceQuote Cities 
		/// </summary>
		/// <param name="modelId"></param>
		/// <returns></returns>
		public List<CityEntityBase> GetPriceQuoteCities(uint modelId)
		{
			List<CityEntityBase> objCities = null;
			try
			{
				using (DbCommand cmd = DbFactory.GetDBCommand("getpricequotecities_18072017"))
				{
					cmd.CommandType = CommandType.StoredProcedure;
					cmd.Parameters.Add(DbFactory.GetDbParam("par_modelid", DbType.Int64, modelId));

					using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
					{
						if (dr != null)
						{
							objCities = new List<CityEntityBase>();
							while (dr.Read())
								objCities.Add(new CityEntityBase()
								{
									CityId = SqlReaderConvertor.ToUInt32(dr["CityId"]),
									CityName = Convert.ToString(dr["CityName"]),
									IsPopular = SqlReaderConvertor.ToBoolean(dr["IsPopular"]),
									HasAreas = SqlReaderConvertor.ToBoolean(dr["HasAreas"]),
									CityMaskingName = Convert.ToString(dr["citymaskingname"])
								});
							dr.Close();
						}
					}
				}
			}
			catch (SqlException ex)
			{
				ErrorClass.LogError(ex, "sqlex in CityRepository : " + ex.Message);

			}
			catch (Exception ex)
			{
				ErrorClass.LogError(ex, "ex in CityRepository : " + ex.Message);

			}

			return objCities;
		}

		/// <summary>
		/// Written By : Ashish G. Kamble on 7 June 2016        
		/// Function to get the new city masking names 
		/// </summary>
		/// <returns></returns>
		public Hashtable GetMaskingNames()
		{
			Hashtable ht = null;

			try
			{
				using (DbCommand cmd = DbFactory.GetDBCommand())
				{
					cmd.CommandText = "getcitymappingnames";
					cmd.CommandType = CommandType.StoredProcedure;

					using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
					{
						if (dr != null)
						{
							ht = new Hashtable();

							while (dr.Read())
							{
								if (!ht.ContainsKey(dr["CityMaskingName"]))
									ht.Add(dr["CityMaskingName"], dr["ID"]);
							}
							dr.Close();
						}
					}
				}
			}
			catch (Exception ex)
			{
				HttpContext.Current.Trace.Warn("CityRepository.GetMaskingNames ex : " + ex.Message + ex.Source);
				ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);

			}

			return ht;
		}

		/// <summary>
		/// Written By : Ashish G. Kamble on 7 June 2016        
		/// Function to get the old city masking names 
		/// </summary>
		/// <returns></returns>
		public Hashtable GetOldMaskingNames()
		{
			Hashtable ht = null;

			try
			{
				using (DbCommand cmd = DbFactory.GetDBCommand())
				{
					cmd.CommandText = "getoldcitymappingnames";
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
				HttpContext.Current.Trace.Warn("CityRepository.GetOldMaskingNamesList ex : " + ex.Message + ex.Source);
				ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);

			}
			return ht;
		}

		/// <summary>
		/// Written By : Vivek Gupta
		/// Date : 24 june 2016
		/// Desc : to get dealer cities for dealer locator
		/// </summary>
		/// <returns></returns>
		public DealerStateCities GetDealerStateCities(uint makeId, uint stateId)
		{

			DealerStateCities objStateCities = null;

			try
			{
				using (DbCommand cmd = DbFactory.GetDBCommand())
				{
					cmd.CommandText = "getcitywisedealerscnt";
					cmd.CommandType = CommandType.StoredProcedure;

					cmd.Parameters.Add(DbFactory.GetDbParam("par_makeid", DbType.Int32, makeId));
					cmd.Parameters.Add(DbFactory.GetDbParam("par_stateid", DbType.Int32, stateId));


					using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
					{
						if (dr != null)
						{
							objStateCities = new DealerStateCities();

							List<DealerCityEntity> dealerCities = new List<DealerCityEntity>();

							while (dr.Read())
							{
								dealerCities.Add(new DealerCityEntity()
								{
									CityId = !Convert.IsDBNull(dr["CityId"]) ? Convert.ToUInt32(dr["CityId"]) : default(UInt32),
									CityName = !Convert.IsDBNull(dr["Name"]) ? Convert.ToString(dr["Name"]) : default(String),
									CityMaskingName = !Convert.IsDBNull(dr["CityMaskingName"]) ? Convert.ToString(dr["CityMaskingName"]) : default(String),
									Lattitude = !Convert.IsDBNull(dr["Lattitude"]) ? Convert.ToString(dr["Lattitude"]) : default(String),
									Longitude = !Convert.IsDBNull(dr["Longitude"]) ? Convert.ToString(dr["Longitude"]) : default(String),
									DealersCount = !Convert.IsDBNull(dr["DealersCnt"]) ? Convert.ToUInt32(dr["DealersCnt"]) : default(UInt32)
								});
							}

							objStateCities.dealerCities = dealerCities;

							if (dr.NextResult())
							{
								DealerStateEntity dealerStates = new DealerStateEntity();

								if (dr.Read())
								{
									dealerStates.StateId = !Convert.IsDBNull(dr["Id"]) ? Convert.ToUInt32(dr["Id"]) : default(UInt32);
									dealerStates.StateName = !Convert.IsDBNull(dr["Name"]) ? Convert.ToString(dr["Name"]) : default(String);
									dealerStates.StateMaskingName = !Convert.IsDBNull(dr["StateMaskingName"]) ? Convert.ToString(dr["StateMaskingName"]) : default(String);
									dealerStates.StateLatitude = !Convert.IsDBNull(dr["StateLattitude"]) ? Convert.ToString(dr["StateLattitude"]) : default(String);
									dealerStates.StateLongitude = !Convert.IsDBNull(dr["StateLongitude"]) ? Convert.ToString(dr["StateLongitude"]) : default(String);
								}

								objStateCities.dealerStates = dealerStates;
							}

							dr.Close();

						}
					}
				}
			}
			catch (Exception ex)
			{
				ErrorClass.LogError(ex, String.Format("ServerVariable: {0} , Parameters : makeId({1}), stateId({2})", HttpContext.Current.Request.ServerVariables["URL"], makeId, stateId));

			}
			return objStateCities;
		}
		/// <summary>
		/// Created by Subodh jain 6 oct 2016
		/// Describtion To get Top 6 cities order by poplarity and remaining by alphabetic order
		/// Created By : Sajal Gupta on 05-01-2017
		/// Desc : Changed SP getusedbikebycitywithcount_04012017
		/// </summary>
		/// <returns></returns>
		public IEnumerable<UsedBikeCities> GetUsedBikeByCityWithCount()
		{
			IList<UsedBikeCities> usedBikeCities = null;
			try
			{
				using (DbCommand cmd = DbFactory.GetDBCommand("getusedbikebycitywithcount_04012017"))
				{
					cmd.CommandType = CommandType.StoredProcedure;

					using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
					{
						if (dr != null)
						{
							usedBikeCities = new List<UsedBikeCities>();
							while (dr.Read())
							{
								usedBikeCities.Add(new UsedBikeCities
								{
									BikesCount = SqlReaderConvertor.ToUInt32(dr["bikecount"]),
									CityMaskingName = Convert.ToString(dr["citymaskingname"]),
									CityName = Convert.ToString(dr["city"]),
									CityId = SqlReaderConvertor.ToUInt32(dr["cityid"]),
									Priority = SqlReaderConvertor.ToUInt32(dr["priority"]),
								});
							}
							dr.Close();
						}
					}
				}
			}
			catch (Exception err)
			{
				ErrorClass.LogError(err, String.Format("GetUsedBikeByCityWithCount"));

			}

			return usedBikeCities;
		}

		/// <summary>
		/// Created by : Subodh Jain 29 Dec 2016
		/// Summary: Get Used bikes by make in cities
		/// </summary>
		public IEnumerable<UsedBikeCities> GetUsedBikeByMakeCityWithCount(uint makeId)
		{
			IList<UsedBikeCities> usedBikeCities = null;
			try
			{
				using (DbCommand cmd = DbFactory.GetDBCommand("getusedbikeincitybymake"))
				{
					cmd.CommandType = CommandType.StoredProcedure;
					cmd.Parameters.Add(DbFactory.GetDbParam("par_makeid", DbType.Int32, makeId));

					using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
					{
						if (dr != null)
						{
							usedBikeCities = new List<UsedBikeCities>();
							while (dr.Read())
							{
								usedBikeCities.Add(new UsedBikeCities
								{
									BikesCount = SqlReaderConvertor.ToUInt32(dr["bikecount"]),
									CityMaskingName = Convert.ToString(dr["citymaskingname"]),
									CityName = Convert.ToString(dr["city"]),
									CityId = SqlReaderConvertor.ToUInt32(dr["cityid"]),
									Priority = SqlReaderConvertor.ToUInt32(dr["priority"]),
								});
							}
							dr.Close();
						}
					}
				}
			}
			catch (Exception err)
			{
				ErrorClass.LogError(err, string.Format("CityRepository.GetUsedBikeByMakeCityWithCount_{0}", makeId));

			}

			return usedBikeCities;
        }

        /// <summary>
        /// Author  :   Kartik Rathod on 8 jun 2018
        /// Desc    :   Get cities based on state name 
        /// </summary>
        /// <param name="stateName"></param>
        /// <returns>List of CityEntityBase</returns>
        public ICollection<CityEntityBase> GetCitiesByStateName(string stateName)
        {
            ICollection<CityEntityBase> objCityList = null;
            try
            {
                string strQuery = "select id as CityId,Name,CityMaskingName from cities where soundex(StateMaskingName) = soundex(@stateName);";
                using (DbCommand cmd = DbFactory.GetDBCommand(strQuery))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add(DbFactory.GetDbParam("@stateName", DbType.String, stateName));
                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            objCityList = new Collection<CityEntityBase>();
                            while (dr.Read())
                            {
                                objCityList.Add(new CityEntityBase
                                {
                                    CityId = SqlReaderConvertor.ToUInt32(dr["CityId"]),
                                    CityName = Convert.ToString(dr["Name"]),
                                    CityMaskingName = Convert.ToString(dr["CityMaskingName"])
                                });
                            }
                            dr.Close();
                        }
                        
                    }
                }
            }
            catch (Exception err)
            {
                ErrorClass.LogError(err, string.Format("CityRepository.GetCitiesByStateName_{0}", stateName));
            }
            return objCityList;
		}

		/// <summary>
		/// Created By  : Pratibha Verma on 17 May 2018
		/// Description : returns all cities where model Price is available
		/// </summary>
		/// <param name="modelId"></param>
		/// <returns></returns>
		public IEnumerable<CityEntityBase> GetModelPriceCities(uint modelId, byte popularCityCount)
		{
			ICollection<CityEntityBase> objCityList = null;
			try
			{
				using (DbCommand cmd = DbFactory.GetDBCommand("getfinancecities"))
				{
					cmd.CommandType = CommandType.StoredProcedure;
					cmd.Parameters.Add(DbFactory.GetDbParam("par_modelid", DbType.Int32, modelId));
					cmd.Parameters.Add(DbFactory.GetDbParam("par_popularcitycount", DbType.Byte, popularCityCount));
					using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
					{
						if (dr != null)
						{
							objCityList = new Collection<CityEntityBase>();
							while (dr.Read())
							{
								objCityList.Add(new CityEntityBase
								{
									CityId = SqlReaderConvertor.ToUInt32(dr["CityId"]),
									CityName = Convert.ToString(dr["CityName"]),
									CityMaskingName = Convert.ToString(dr["CityMaskingName"]),
									PopularityOrder = SqlReaderConvertor.ToUInt32(dr["PopularityOrder"])
								});
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				ErrorClass.LogError(ex, "Bikewale.DAL.Location.CityRepository.GetModelPriceCities");
			}
			return objCityList;
		}
	}
}
