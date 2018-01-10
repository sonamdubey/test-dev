
using Bikewale.Entities.DealerLocator;
using Bikewale.Entities.Location;
using Bikewale.Interfaces.Location;
using Bikewale.Notifications;
using Bikewale.Utility;
using MySql.CoreDAL;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Web;
namespace Bikewale.DAL.Location
{
    public class StateRepository : IState
    {
        /// <summary>
        /// Written By : Ashish G. Kamble on 3/8/2012
        /// Getting States id and states names
        /// </summary>
        /// <returns></returns>
        public List<StateEntityBase> GetStates()
        {
            List<StateEntityBase> objStateList = null;

            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("getstates"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    objStateList = new List<StateEntityBase>();
                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            while (dr.Read())
                            {
                                objStateList.Add(new StateEntityBase
                                {
                                    StateId = Convert.ToUInt32(dr["Value"]),
                                    StateName = Convert.ToString(dr["Text"]),
                                    StateMaskingName = Convert.ToString(dr["MaskingName"])
                                });
                            }
                            dr.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);

            }
            return objStateList;
        }   // End of GetStates method

        /// <summary>
        /// Function to get the state masking names
        /// </summary>
        /// <returns></returns>
        public Hashtable GetMaskingNames()
        {
            Hashtable ht = null;

            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandText = "getstatemappingnames";
                    cmd.CommandType = CommandType.StoredProcedure;

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            ht = new Hashtable();

                            while (dr.Read())
                            {
                                if (!ht.ContainsKey(dr["StateMaskingName"]))
                                    ht.Add(dr["StateMaskingName"], dr["ID"]);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);

            }
            return ht;
        }
        /// Create By : Vivek Gupta 
        /// Date : 24 june 2016
        /// desc : get dealer states
        /// </summary>
        /// <param name="makeId"></param>
        /// <returns></returns>
        public IEnumerable<DealerStateEntity> GetDealerStates(uint makeId)
        {
            List<DealerStateEntity> objStateList = null;

            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("getstatewisedealerscnt"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_makeid", DbType.Int32, makeId));

                    objStateList = new List<DealerStateEntity>();
                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            while (dr.Read())
                            {
                                objStateList.Add(new DealerStateEntity
                                {
                                    StateId = !Convert.IsDBNull(dr["StateId"]) ? Convert.ToUInt32(dr["StateId"]) : default(UInt32),
                                    StateName = !Convert.IsDBNull(dr["StateName"]) ? Convert.ToString(dr["StateName"]) : default(String),
                                    StateMaskingName = !Convert.IsDBNull(dr["StateMaskingName"]) ? Convert.ToString(dr["StateMaskingName"]) : default(String),
                                    StateLatitude = !Convert.IsDBNull(dr["StateLattitude"]) ? Convert.ToString(dr["StateLattitude"]) : default(String),
                                    StateLongitude = !Convert.IsDBNull(dr["StateLongitude"]) ? Convert.ToString(dr["StateLongitude"]) : default(String),
                                    StateCount = !Convert.IsDBNull(dr["StateCnt"]) ? Convert.ToUInt32(dr["StateCnt"]) : default(UInt32)
                                });
                            }

                            dr.Close();
                        }

                    }
                }
            }

            catch (Exception ex)
            {
                ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"] + String.Format(" :GetDealerStates, makeId = {0} ", makeId));

            }
            return objStateList;
        }
        /// <summary>
        /// Created By:- Subodh Jain 29 may 2016
        /// Description :- Fetch Dealers for make in all states with cities
        /// </summary>
        /// <param name="makeId"></param>
        /// <returns></returns>
        public DealerLocatorList GetDealerStatesCities(uint makeId)
        {
            IList<DealerCityEntity> objCityList = null;
            IList<StateCityEntity> objStateList = null;
            DealerLocatorList objStateCityList = null;
            string makeMaskingname = string.Empty;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("getstatecitybymake"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_makeid", DbType.Int32, makeId));


                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            objCityList = new List<DealerCityEntity>();
                            if (dr.Read())
                            {
                                makeMaskingname = Convert.ToString(dr["maskingname"]);
                            }

                            if (dr.NextResult())
                            {
                                while (dr.Read())
                                {
                                    objCityList.Add(new DealerCityEntity
                                    {
                                        CityId = SqlReaderConvertor.ToUInt16(dr["cityid"]),
                                        CityName = Convert.ToString(dr["name"]),
                                        CityMaskingName = Convert.ToString(dr["citymaskingname"]),
                                        Lattitude = Convert.ToString(dr["lattitude"]),
                                        Longitude = Convert.ToString(dr["longitude"]),
                                        DealersCount = SqlReaderConvertor.ToUInt16(dr["dealerscnt"]),
                                        stateId = SqlReaderConvertor.ToUInt16(dr["stateid"]),
                                        Link = string.Format("{0}/dealer-showrooms/{1}/{2}/", Bikewale.Utility.BWConfiguration.Instance.BwHostUrlForJs, makeMaskingname, Convert.ToString(dr["citymaskingname"]))
                                    });
                                }
                            }

                            if (dr.NextResult())
                            {
                                objStateList = new List<StateCityEntity>();
                                while (dr.Read())
                                {
                                    objStateList.Add(new StateCityEntity
                                    {
                                        Id = SqlReaderConvertor.ToUInt16(dr["Id"]),
                                        Lat = Convert.ToString(dr["statelattitude"]),
                                        Long = Convert.ToString(dr["statelongitude"]),
                                        Name = Convert.ToString(dr["name"]),
                                        stateMaskingName = Convert.ToString(dr["statemaskingname"]),
                                        DealerCountState = SqlReaderConvertor.ToUInt16(dr["statedealercnt"])
                                    });

                                }

                            }

                            // Now statelist and city list is ready
                            // Now iterate on states and find cities for that state
                            // Add the cities for that state
                            if (objStateList != null)
                            {
                                objStateCityList = new DealerLocatorList();
                                objStateCityList.StateCityList = objStateList;
                                foreach (var st in objStateCityList.StateCityList)
                                {
                                    var curStateCityList = from curCity in objCityList
                                                           where curCity.stateId == st.Id
                                                           select curCity;
                                    if (curStateCityList != null)
                                    {
                                        st.Cities = curStateCityList.ToList();
                                    }
                                }

                                objStateCityList.TotalCities = Convert.ToUInt32(objStateCityList.StateCityList.Sum(m => m.Cities.Count()));
                                objStateCityList.TotalDealers = Convert.ToUInt32(objStateCityList.StateCityList.Sum(m => m.DealerCountState));
                            }
                            dr.Close();
                        }

                    }
                }


            }


            catch (Exception ex)
            {
                ErrorClass.LogError(ex, String.Format(" :GetDealerStates, makeId = {0} ", makeId));

            }
            return objStateCityList;
        }
    }
}
