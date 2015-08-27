using Bikewale.CoreDAL;
using Bikewale.Entities.Location;
using Bikewale.Interfaces.Location;
using Bikewale.Notifications;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Bikewale.DAL.Location
{
    public class AreaRepository : IArea
    {
        public IEnumerable<AreaEntityBase> GetAreasByCity(UInt16 cityId)
        {
            Database db = null;
            List<AreaEntityBase> lstArea = null;
            AreaEntityBase area = null;
            SqlDataReader reader = null;
            try
            {
                using (SqlCommand cmd = new SqlCommand("GetAreas"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CityId", SqlDbType.Int).Value = cityId;

                    db = new Database();
                    reader = db.SelectQry(cmd);
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
            return lstArea;
        }
    }
}
