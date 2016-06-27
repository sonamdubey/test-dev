using Bikewale.CoreDAL;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.Location;
using Bikewale.Interfaces.Location;
using Bikewale.Notifications;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
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
            Database db = null;
            List<CityEntityBase> objCityList = null;
            try
            {
                using (SqlCommand cmd = new SqlCommand("GetCities"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@RequestType", requestType);

                    db = new Database();
                    objCityList = new List<CityEntityBase>();

                    using (SqlDataReader dr = db.SelectQry(cmd))
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
            finally
            {
                db.CloseConnection();
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
            Database db = null;
            List<CityEntityBase> objCityList = null;

            try
            {
                using (SqlCommand cmd = new SqlCommand("GetCities"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@RequestType", SqlDbType.VarChar, 20).Value = requestType;
                    cmd.Parameters.Add("@StateId", SqlDbType.BigInt).Value = stateId;

                    db = new Database();
                    objCityList = new List<CityEntityBase>();

                    using (SqlDataReader dr = db.SelectQry(cmd))
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
            finally
            {
                db.CloseConnection();
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
            Database db = null;
            try
            {
                using (SqlCommand cmd = new SqlCommand("GetPriceQuoteCities_05022016"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@modelId", SqlDbType.BigInt).Value = modelId;

                    db = new Database();
                    using (SqlDataReader dr = db.SelectQry(cmd))
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
            finally
            {
                db.CloseConnection();
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
            Database db = null;
            Hashtable ht = null;

            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = "GetCityMappingNames";
                    cmd.CommandType = CommandType.StoredProcedure;

                    db = new Database();

                    using (SqlDataReader dr = db.SelectQry(cmd))
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
            finally
            {
                db.CloseConnection();
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
            Database db = null;
            Hashtable ht = null;

            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = "GetOldCityMappingNames";
                    cmd.CommandType = CommandType.StoredProcedure;

                    db = new Database();

                    using (SqlDataReader dr = db.SelectQry(cmd))
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
            finally
            {
                db.CloseConnection();
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
            Database db = null;

            DealerStateCities objStateCities = null;

            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = "GetCitywiseDealersCnt";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@MakeId", SqlDbType.Int).Value = makeId;
                    cmd.Parameters.Add("@StateId", SqlDbType.Int).Value = stateId;

                    db = new Database();

                    using (SqlDataReader dr = db.SelectQry(cmd))
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

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, String.Format("ServerVariable: {0} , Parameters : makeId({1}), stateId({2})", HttpContext.Current.Request.ServerVariables["URL"], makeId, stateId));
                objErr.SendMail();
            }
            finally
            {
                db.CloseConnection();
            }
            return objStateCities;
        }
    }
}
