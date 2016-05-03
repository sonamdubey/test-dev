using Bikewale.CoreDAL;
using System;
using System.Data;
using System.Data.Common;
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
                using (DbCommand cmd = DbFactory.GetDBCommand("deletecomparebikedate"))
                {
                    //cmd.CommandText = "DeleteCompareBikeDate";
                    cmd.CommandType = CommandType.StoredProcedure;

                    //cmd.Parameters.Add("@Id", SqlDbType.Int).Value = deleteId;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_id", DbParamTypeMapper.GetInstance[SqlDbType.Int], deleteId)); 
                    MySqlDatabase.UpdateQuery(cmd);
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
                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandText = "setbikecomparisonpriority";
                    cmd.CommandType = CommandType.StoredProcedure;

                    //cmd.Parameters.Add("@PrioritiesList", SqlDbType.VarChar, 1000).Value = prioritiesList;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_modelid", DbParamTypeMapper.GetInstance[SqlDbType.VarChar],1000, prioritiesList));

                    MySqlDatabase.UpdateQuery(cmd);
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
