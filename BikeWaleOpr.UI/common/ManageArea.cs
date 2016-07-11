
using BikeWaleOPR.Utilities;
using MySql.CoreDAL;
using System;
using System.Data;
using System.Data.Common;

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
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandText = "getareas";
                    cmd.CommandType = CommandType.StoredProcedure; 
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_cityid", DbType.Int32, cityId));

                    ds = MySqlDatabase.SelectAdapterQuery(cmd, ConnectionType.ReadOnly);
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