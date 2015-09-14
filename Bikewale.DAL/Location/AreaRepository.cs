using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bikewale.Interfaces.Location;
using Bikewale.Entities.Location;
using System.Data;
using System.Data.SqlClient;
using Bikewale.CoreDAL;
using System.Web;
using Bikewale.Notifications;

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
            Database db = null;
            List<AreaEntityBase> objAreaList = null;
            try
            {
                using (SqlCommand cmd = new SqlCommand("GetAreas"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CityId", SqlDbType.Int).Value = cityId;

                    db = new Database();

                    objAreaList = new List<AreaEntityBase>();
                    using (SqlDataReader dr = db.SelectQry(cmd))
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

            return objAreaList;
        }   // End of GetAreas method

        /// <summary>
        /// Created By Sumit Kate
        /// </summary>
        /// <param name="cityId">City Id</param>
        /// <returns></returns>
        public IEnumerable<AreaEntityBase> GetAreasByCity(UInt16 cityId)
        {
            Database db = null;
            List<AreaEntityBase> lstArea = null;
            AreaEntityBase area = null;
            try
            {
                using (SqlCommand cmd = new SqlCommand("GetAreas"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CityId", SqlDbType.Int).Value = cityId;

                    db = new Database();
                    using (SqlDataReader reader = db.SelectQry(cmd))
                    {
                        if (reader != null && reader.HasRows)
                        {
                            lstArea = new List<AreaEntityBase>();
                            while (reader.Read())
                            {
                                area = new AreaEntityBase();
                                area.AreaId = Convert.ToUInt32(reader["Value"]);
                                area.AreaName = Convert.ToString(reader["Text"]);
                                lstArea.Add(area);
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
            return lstArea;
        }
    }
}
