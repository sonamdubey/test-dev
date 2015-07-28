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

namespace Bikewale.DAL.Location
{
    public class CityRepository : ICity
    {
        /// <summary>
        /// Created By : Sadhana Upadhyay on 21 July 2015
        /// Summary : to get PriceQuote Cities 
        /// </summary>
        /// <param name="modelId"></param>
        /// <returns></returns>
        public List<CityEntityBase> GetPriceQuoteCities(uint modelId)
        {
            List<CityEntityBase> objCities=null;
            Database db = null;
            try
            {
                using (SqlCommand cmd = new SqlCommand("GetPriceQuoteCities"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@modelId", SqlDbType.BigInt).Value = modelId;

                    db = new Database();
                    using(SqlDataReader dr=db.SelectQry(cmd))
                    {
                        if(dr!=null)
                        {
                            objCities=new List<CityEntityBase>();
                            while (dr.Read())
                                objCities.Add(new CityEntityBase()
                                {
                                    CityId = Convert.ToUInt32(dr["Value"]),
                                    CityName = dr["Text"].ToString()
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
    }
}
