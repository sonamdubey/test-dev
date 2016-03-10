using BikewaleOpr.Entities;
using BikeWaleOpr.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace BikewaleOpr.common
{
    public class ManageDealerBenefit
    {
        /// <summary>
        /// Written By : Sangram on 10 Mar 2016
        /// Summary    : Retrieves all benefits for dealers
        /// </summary>
        /// <param name="basicId"></param>
        /// <returns></returns>
        public List<DealerBenefitEntity> GetDealerBenefits(string dealerId)
        {
            DataSet ds = null;
            List<DealerBenefitEntity> benefits = null;
            try
            {
                Database db = new Database();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "BW_GetDealerBenefits";
                    cmd.Parameters.Add("@DealerId", SqlDbType.BigInt).Value = dealerId;
                    ds = db.SelectAdaptQry(cmd);
                    if(ds!= null && ds.Tables!= null && ds.Tables.Count > 0)
                    {
                        benefits = new List<DealerBenefitEntity>();
                        foreach(DataRow dr in ds.Tables[0].Rows)
                        {
                            var benefit = new DealerBenefitEntity();
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                HttpContext.Current.Trace.Warn("sql ex : ", ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, "GetDealerBenefits");
                objErr.SendMail();
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("ex : ", ex.Message);
                ErrorClass objErr = new ErrorClass(ex, "GetDealerBenefits");
                objErr.SendMail();
            }
            return benefits;
        }

        /// <summary>
        /// Written By : Sangram on 10 Mar 2016
        /// Summary    : Retrieves all benefits for dealers
        /// </summary>
        /// <param name="basicId"></param>
        /// <returns></returns>
        public Dictionary<int,string> GetDealerCategories(string dealerId)
        {
            DataSet ds = null;
            Dictionary<int, string> catList = null;
            try
            {
                Database db = new Database();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "GetDealerBenefitCategory";
                    ds = db.SelectAdaptQry(cmd);
                    if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
                    {
                        catList = new Dictionary<int, string>();
                        catList.Add(0, "Select Benefit category Id");
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            catList.Add(Convert.ToInt32(dr["id"].ToString()), dr["name"].ToString());
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                HttpContext.Current.Trace.Warn("sql ex : ", ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, "GetDealerCategories");
                objErr.SendMail();
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("ex : ", ex.Message);
                ErrorClass objErr = new ErrorClass(ex, "GetDealerCategories");
                objErr.SendMail();
            }
            return catList;
        }

    }
}