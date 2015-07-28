using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web;

/// <summary>
/// to Manage Featured Bike 
/// </summary>
namespace BikeWaleOpr.Common
{
    public class ManageFeaturedBike
    {
        /// <summary>
        /// Created By : Sadhana Upadhyay on 22 July 2014
        /// Summary : to update featured bike priorities
        /// </summary>
        /// <param name="prioritiesList"></param>
        public void SetFeaturedBikePriorities(string prioritiesList)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    Database db = new Database();
                    cmd.CommandText = "SetFeaturedBikePriority";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@PrioritiesList", SqlDbType.VarChar, 1000).Value = prioritiesList;

                    db.UpdateQry(cmd);
                }
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }   //End of SetFeaturedBikePriorities
    }   //End of Class
}   //End of namespace