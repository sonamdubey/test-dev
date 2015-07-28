using System;
using System.Data;
using System.Data.SqlClient;
using System.Web;

namespace BikeWaleOpr.Common
{
    /// <summary>
    /// Created By : Sadhana Upadhyay on 19th Feb 2014
    /// Summary : To Add Bike Comparison Functions
    /// </summary>
    public class CompareBike
    {
        /// <summary>
        /// Created By : Sadhana Upadhyay on 19th Feb 2014
        /// Summary : To delete Compare Bike records
        /// </summary>
        /// <param name="deleteId"></param>
        public void DeleteCompareBike(string deleteId)
        {
            try
            {
                Database db = new Database();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = "DeleteCompareBikeDate";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@Id", SqlDbType.Int).Value = deleteId;
                    db.UpdateQry(cmd);
                }
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }   //End of DeleteCompareBike

        /// <summary>
        /// Created By : Sadhana Upadhyay on 19th Feb 2014
        /// Summary : To update Compare Bike priority
        /// </summary>
        /// <param name="deleteId"></param>
        public void UpdatePriorities(string prioritiesList)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    Database db = new Database();
                    cmd.CommandText = "SetBikeComparisonPriority";
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

        }   //End of UpdatePriorities

    }
}
