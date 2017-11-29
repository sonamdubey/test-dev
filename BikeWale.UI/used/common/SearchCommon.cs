using Bikewale.Common;
using MySql.CoreDAL;
using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Web;
namespace Bikewale.Used
{
    public class SearchCommon
    {
        private string _searchCriteria;
        public string test;

        public string _sessionID, _model, _make, _priceFrom, _priceTo, _priceToMax, _yearFrom, _yearTo;
        public string _kmFrom, _kmTo, _kmToMax, _city, _dist, _st, _li, searchCriteriaProfileIds;
        public DateTime _listedFrom;
        private HttpContext objTrace = HttpContext.Current;
        public string _lattitude, _longitude;

        private SqlParameter[] _SParams = null;

        public string SearchCriteria
        {
            get
            {
                return _searchCriteria;
            }
            set
            {
                _searchCriteria = value;
            }
        }

        // This property hold all the sqlParameters
        public SqlParameter[] SParams
        {
            get
            {
                return _SParams;
            }
            set
            {
                _SParams = value;
            }
        } // SParams

        public bool UpdateViewCount(string inquiryId, bool isDealer)
        {
            bool retVal = false;

            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("sp_classified_updateviewcountw"))
                {
                    //cmd = new SqlCommand("sp_classified_updateviewcount", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    //prm = cmd.Parameters.Add("@InquiryId", SqlDbType.BigInt);
                    //prm.Value = inquiryId;

                    //prm = cmd.Parameters.Add("@IsDealer", SqlDbType.Bit);
                    //prm.Value = isDealer;


                    cmd.Parameters.Add(DbFactory.GetDbParam("par_inquiryid", DbType.Int32, inquiryId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_isdealer", DbType.Boolean, isDealer));

                    //Bikewale.Notifications.// LogLiveSps.LogSpInGrayLog(cmd);
                    int rowsUpdated = (int)MySqlDatabase.ExecuteNonQuery(cmd, ConnectionType.MasterDatabase);

                    if (rowsUpdated > 0)
                        retVal = true;
                    else
                        retVal = false;
                }
            }
            catch (SqlException err)
            {
                retVal = false;
                HttpContext.Current.Trace.Warn("UpdateViewCountSqlErr : " + err.Message);
                ErrorClass.LogError(err, objTrace.Request.ServerVariables["URL"]);
                
            }
            catch (Exception err)
            {
                retVal = false;
                HttpContext.Current.Trace.Warn("UpdateViewCountErr : " + err.Message);
                ErrorClass.LogError(err, objTrace.Request.ServerVariables["URL"]);
                
            }

            return retVal;
        }


        /// <summary>
        /// Written By : Ashwini Todkar on 4 April 2014
        /// Summary    : PopulateWhere to get city wise used bikes count from live listing  
        /// </summary>
        /// <returns>city name,city masking name,city id and bike count</returns>
        public DataSet GetUsedBikeByCityWithCount()
        {
            DataSet ds = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "getusedbikebycitywithcount";

                    ds = MySqlDatabase.SelectAdapterQuery(cmd, ConnectionType.ReadOnly);
                }
            }
            catch (SqlException err)
            {
                HttpContext.Current.Trace.Warn(err.Message);
                ErrorClass.LogError(err, HttpContext.Current.Request.ServerVariables["URL"]);
                
            }
            catch (Exception err)
            {
                HttpContext.Current.Trace.Warn(err.Message);
                ErrorClass.LogError(err, HttpContext.Current.Request.ServerVariables["URL"]);
                
            }

            return ds;
        }//End of GetUsedBikeByCityWithCount


        /// <summary>
        /// Written By : Ashwini Todkar on 4 April 2014
        /// Summary    : PopulateWhere to get model wise used bikes count and its details from live listing  
        /// </summary>
        /// <returns>model name,make name,make masking name,model masking name,model id and model count</returns>
        public DataSet GetUsedBikeModelsWithCount()
        {
            DataSet ds = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "getusedbikemodelswithcount";

                    ds = MySqlDatabase.SelectAdapterQuery(cmd, ConnectionType.ReadOnly);
                }
            }
            catch (SqlException err)
            {
                HttpContext.Current.Trace.Warn(err.Message);
                ErrorClass.LogError(err, HttpContext.Current.Request.ServerVariables["URL"]);
                
            }
            catch (Exception err)
            {
                HttpContext.Current.Trace.Warn(err.Message);
                ErrorClass.LogError(err, HttpContext.Current.Request.ServerVariables["URL"]);
                
            }

            return ds;
        }//End of GetUsedBikeModelsWithCount


        /// <summary>
        /// Written By : Ashwini Todkar on 4 April 2014
        /// Summary    : PopulateWhere to get make wise used bikes count and details from live listing  
        /// </summary>
        /// <returns>make name,make masking name,make id and bike count</returns>
        public DataSet GetUsedBikeMakesWithCount()
        {
            DataSet ds = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "getusedbikemakeswithcount";

                    ds = MySqlDatabase.SelectAdapterQuery(cmd, ConnectionType.ReadOnly);
                }
            }
            catch (SqlException err)
            {
                HttpContext.Current.Trace.Warn(err.Message);
                ErrorClass.LogError(err, HttpContext.Current.Request.ServerVariables["URL"]);
                
            }
            catch (Exception err)
            {
                HttpContext.Current.Trace.Warn(err.Message);
                ErrorClass.LogError(err, HttpContext.Current.Request.ServerVariables["URL"]);
                
            }
            return ds;
        }//End of GetUsedBikeMakesWithCount
    }
}