using System;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using Bikewale.Notifications.CoreDAL;
using System.Data.Common;

namespace Bikewale.Common
{
    /// <summary>
    /// Created By : Ashish G. Kamble on 28/8/2012
    /// </summary>
    public class Classified
    {

        // this function tells if a particular customer has shown request for a particular Bike?
        public static bool HasShownInterestInUsedBike(bool isDealer, string bikeId, string customerId)
        {
            bool shownInterest = false;
            string sql = "";

            if (!isDealer) // if it's an individual's Bike
            {
                sql = " select id as requestid  from classifiedrequests where sellinquiryid=@inquiryid and customerid=@customerid";
            }

            try
            {
                if (!String.IsNullOrEmpty(sql))
                {
                    using (DbCommand cmd = DbFactory.GetDBCommand(sql))
                    {
                        cmd.Parameters.Add(DbFactory.GetDbParam("@inquiryid", DbParamTypeMapper.GetInstance[SqlDbType.BigInt], bikeId));
                        cmd.Parameters.Add(DbFactory.GetDbParam("@customerid", DbParamTypeMapper.GetInstance[SqlDbType.BigInt], customerId));

                        using (IDataReader dr = MySqlDatabase.SelectQuery(cmd))
                        {
                            if (dr.Read())
                            {
                                shownInterest = true;
                            } 
                        }
                    } 
                }
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }

            return shownInterest;
        }   // End of HasShownInterestInUsedBike function


    }   // End of class
}   // End of namespace