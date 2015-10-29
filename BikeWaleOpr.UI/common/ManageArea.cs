using BikeWaleOpr.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace BikeWaleOpr.Common
{
    /// <summary>
    /// Created By : Sadhana Upadhyay on 28 Oct 2015
    /// </summary>
    public class ManageArea
    {
        /// <summary>
        /// Created By : Sadhana Upadhyay on 28 Oct 2015
        /// Summary : To get area list for perticular city
        /// </summary>
        /// <param name="cityId"></param>
        /// <returns></returns>
        public DataSet GetArea(uint cityId)
        {
            DataSet ds = null;
            Database db = null;
            try
            {
                using(SqlCommand cmd=new SqlCommand())
                {
                    cmd.CommandText = "GetAreas";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CityId", SqlDbType.Int).Value = cityId;
                    db = new Database();

                    ds = db.SelectAdaptQry(cmd);
                }
            }
            catch(Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "BikewaleOpr.Common.ManageArea");
                objErr.SendMail();
            }
            return ds;
        }   //end of GetArea Method
    }   //End of class
}   //End of namespace