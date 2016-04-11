using BikewaleOpr.Entities;
using BikeWaleOpr.Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace BikewaleOpr.common
{
    /// <summary>
    /// Written By : Sangram on 10 Mar 2016
    /// Summary    : Class to hold DB layer for Manage Dealer Benefits
    /// </summary>
    public class ManageDealerBenefit
    {
        /// <summary>
        /// Written By : Sangram on 10 Mar 2016
        /// Summary    : Retrieves all benefits for dealers
        /// Modified by :   Sumit Kate on 19 Mar 2016
        /// Description :   Close the db connection in finally
        /// </summary>
        /// <param name="dealerId"> DealerID</param>
        /// <returns></returns>
        public DataTable GetDealerCategories(string dealerId)
        {
            DataSet ds = null;
            DataTable dt = null;
            Database db = null;
            try
            {
                db = new Database();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "GetDealerBenefitCategory";
                    ds = db.SelectAdaptQry(cmd);
                    if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
                    {
                        dt = ds.Tables[0];
                        DataRow dr= dt.NewRow();
                        dr[0] = 0;
                        dr[1] = "Select";
                        dt.Rows.InsertAt(dr, 0);
                    }
                }
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("ex : ", ex.Message);
                ErrorClass objErr = new ErrorClass(ex, "GetDealerCategories");
                objErr.SendMail();
            }
            finally
            {
                if (db != null)
                    db.CloseConnection();
                db = null;
            }
            return dt;
        }
    }
}