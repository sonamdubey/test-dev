using Bikewale.Entities.Location;
using Bikewale.Interfaces.Location;
using Bikewale.Notifications;
using MySql.CoreDAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Web;

namespace Bikewale.DAL.Location
{
    public class AreaRepository : IArea
    {
        /// <summary>
        /// Created By : Sadhana Upadhyay on 22nd Oct 2014
        /// Summary : Get Areas list by city id
        /// </summary>
        /// <param name="cityId"></param>
        /// <returns></returns>
        public List<AreaEntityBase> GetAreas(string cityId)
        {
            List<AreaEntityBase> objAreaList = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("getareas"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_cityid", DbType.Int32, cityId));

                    objAreaList = new List<AreaEntityBase>();
                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            while (dr.Read())
                            {
                                objAreaList.Add(new AreaEntityBase
                                {
                                    AreaId = Convert.ToUInt32(dr["Value"]),
                                    AreaName = Convert.ToString(dr["Text"]),
                                    AreaMaskingName = Convert.ToString(dr["MaskingName"])
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

            return objAreaList;
        }   // End of GetAreas method

        /// <summary>
        /// Created By Sumit Kate
        /// </summary>
        /// <param name="cityId">City Id</param>
        /// <returns></returns>
        public IEnumerable<AreaEntityBase> GetAreasByCity(UInt16 cityId)
        {
            List<AreaEntityBase> lstArea = null;
            AreaEntityBase area = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("getareas"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    //cmd.Parameters.Add("@CityId", SqlDbType.Int).Value = cityId;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_cityid", DbType.Int32, cityId));

                    using (IDataReader reader = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (reader != null)
                        {
                            lstArea = new List<AreaEntityBase>();
                            while (reader.Read())
                            {
                                area = new AreaEntityBase();
                                area.AreaId = Convert.ToUInt32(reader["Value"]);
                                area.AreaName = Convert.ToString(reader["Text"]);
                                lstArea.Add(area);
                            }
                            reader.Close();
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

            return lstArea;
        }
    }
}
