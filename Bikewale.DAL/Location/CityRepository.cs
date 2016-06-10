using Bikewale.CoreDAL;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.Location;
using Bikewale.Interfaces.Location;
using Bikewale.Notifications;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Web;

namespace Bikewale.DAL.Location
{
    public class CityRepository : ICity
    {
        /// <summary>
        /// Written By : Ashish G. Kamble on 3/8/2012
        /// Function returns city id and city names by providing state id and request type
        /// </summary>
        /// <param name="stateId"></param>
        /// <param name="RequestType">Pass value All or PriceQuote</param>
        /// <returns></returns>
        public List<CityEntityBase> GetAllCities(EnumBikeType requestType)
        {

            List<CityEntityBase> objCityList = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("getcities"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_requesttype", DbType.Int32, requestType));
                    //cmd.Parameters.AddWithValue("@RequestType", requestType);


                    objCityList = new List<CityEntityBase>();

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd))
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
                    }
                }
            }
            catch (SqlException ex)
            {
                HttpContext.Current.Trace.Warn(ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn(ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
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

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd))
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
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                HttpContext.Current.Trace.Warn(ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn(ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
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
                using (DbCommand cmd = DbFactory.GetDBCommand("getpricequotecities_05022016"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    //cmd.Parameters.Add("@modelId", SqlDbType.BigInt).Value = modelId;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_modelid", DbType.Int64, modelId));

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd))
                    {
                        if (dr != null)
                        {
                            objCities = new List<CityEntityBase>();
                            while (dr.Read())
                                objCities.Add(new CityEntityBase()
                                {
                                    CityId = Convert.ToUInt32(dr["Value"]),
                                    CityName = Convert.ToString(dr["Text"]),
                                    IsPopular = Convert.ToBoolean(dr["IsPopular"]),
                                    HasAreas = Convert.ToBoolean(dr["HasAreas"])
                                });
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "sqlex in CityRepository : " + ex.Message);
                objErr.SendMail();
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "ex in CityRepository : " + ex.Message);
                objErr.SendMail();
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

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd))
                    {
                        if (dr != null)
                        {
                            ht = new Hashtable();

                            while (dr.Read())
                            {
                                if (!ht.ContainsKey(dr["CityMaskingName"]))
                                    ht.Add(dr["CityMaskingName"], dr["ID"]);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("CityRepository.GetMaskingNames ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
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

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd))
                    {
                        if (dr != null)
                        {
                            ht = new Hashtable();

                            while (dr.Read())
                            {
                                if (!ht.ContainsKey(dr["OldMaskingName"]))
                                    ht.Add(dr["OldMaskingName"], dr["NewMaskingName"]);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("CityRepository.GetOldMaskingNamesList ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            return ht;
        }
    }
}
