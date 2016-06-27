
﻿using Bikewale.CoreDAL;
using Bikewale.Entities.Location;
using Bikewale.Interfaces.Location;
using Bikewale.Notifications;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Collections;

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
            Database db = null;
            List<StateEntityBase> objStateList = null;

            try
            {
                using (SqlCommand cmd = new SqlCommand("GetStates"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    db = new Database();
                    objStateList = new List<StateEntityBase>();
                    using (SqlDataReader dr = db.SelectQry(cmd))
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
            return objStateList;
        }   // End of GetStates method

        /// <summary>
        /// Function to get the state masking names
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
                    cmd.CommandText = "GetStateMappingNames";
                    cmd.CommandType = CommandType.StoredProcedure;

                    db = new Database();

                    using (SqlDataReader dr = db.SelectQry(cmd))
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
                HttpContext.Current.Trace.Warn("StateRepository.GetMaskingNames ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            finally
            {
                db.CloseConnection();
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
            Database db = null;
            List<DealerStateEntity> objStateList = null;

            try
            {
                using (SqlCommand cmd = new SqlCommand("GetStatewiseDealersCnt"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@MakeId", SqlDbType.Int).Value = makeId;

                    db = new Database();
                    objStateList = new List<DealerStateEntity>();
                    using (SqlDataReader dr = db.SelectQry(cmd))
                    {
                        if (dr != null)
                        {
                            while (dr.Read())
                            {
                                objStateList.Add(new DealerStateEntity
                                {
                                    StateId = Convert.ToUInt32(dr["StateId"]),
                                    StateName = Convert.ToString(dr["StateName"]),
                                    StateMaskingName = Convert.ToString(dr["StateMaskingName"]),
                                    StateLatitude = Convert.ToString(dr["StateLattitude"]),
                                    StateLongitude = Convert.ToString(dr["StateLongitude"]),
                                    StateCount = Convert.ToUInt32(dr["StateCnt"])
                                });
                            }
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            finally
            {
                db.CloseConnection();
            }
            return objStateList;
        }
    }
}
