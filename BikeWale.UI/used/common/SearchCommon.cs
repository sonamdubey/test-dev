using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using Bikewale.Common;

namespace Bikewale.Used
{
	public class SearchCommon
	{
		private string _searchCriteria;
		public string test;
		
		public string _sessionID,_model, _make, _priceFrom, _priceTo, _priceToMax , _yearFrom, _yearTo;
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
		public SqlParameter [] SParams
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
				
		public bool UpdateViewCount( string inquiryId, bool isDealer )
		{
			bool retVal = false;
			SqlConnection con;
			SqlCommand cmd;
			SqlParameter prm;
			Database db = new Database();
						
			string conStr = db.GetConString();
			con = new SqlConnection( conStr );
			
			try
			{
				cmd = new SqlCommand("SP_Classified_UpdateViewCount", con);
				cmd.CommandType = CommandType.StoredProcedure;
				
				prm = cmd.Parameters.Add("@InquiryId", SqlDbType.BigInt);
				prm.Value = inquiryId;
				
				prm = cmd.Parameters.Add("@IsDealer", SqlDbType.Bit);
				prm.Value = isDealer;
                Bikewale.Notifications.LogLiveSps.LogSpInGrayLog(cmd);
				con.Open();
				
    			int rowsUpdated = (int)cmd.ExecuteNonQuery();
				
				if( rowsUpdated > 0 )
					retVal = true;
				else
					retVal = false;
			}		
			catch(SqlException err)
			{
				retVal = false;
				HttpContext.Current.Trace.Warn("UpdateViewCountSqlErr : " + err.Message);
				ErrorClass objErr = new ErrorClass(err, objTrace.Request.ServerVariables["URL"]);
				objErr.SendMail();
			}	
			catch(Exception err)
			{
				retVal = false;
				HttpContext.Current.Trace.Warn("UpdateViewCountErr : " + err.Message);
				ErrorClass objErr = new ErrorClass(err, objTrace.Request.ServerVariables["URL"]);
				objErr.SendMail();
			}
			finally
			{
			    if(con.State == ConnectionState.Open)
				{
					con.Close();
				}				
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
            Database db = null;
            DataSet ds = null;
            try
            {
                using(SqlCommand cmd = new SqlCommand())
                {                   
                     cmd.CommandType = CommandType.StoredProcedure;
                     cmd.CommandText = "GetUsedBikeByCityWithCount";

                     db = new Database();
                     ds = db.SelectAdaptQry(cmd);
                }
            }
            catch (SqlException err)
            {
                HttpContext.Current.Trace.Warn(err.Message);
                ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            catch (Exception err)
            {
                HttpContext.Current.Trace.Warn(err.Message);
                ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            finally
            {
                db.CloseConnection();
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
            Database db = null;
            DataSet ds = null;
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "GetUsedBikeModelsWithCount";

                    db = new Database();
                    ds = db.SelectAdaptQry(cmd);
                }
            }
            catch (SqlException err)
            {
                HttpContext.Current.Trace.Warn(err.Message);
                ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            catch (Exception err)
            {
                HttpContext.Current.Trace.Warn(err.Message);
                ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            finally
            {
                db.CloseConnection();
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
            Database db = null;
            DataSet ds = null;
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "GetUsedBikeMakesWithCount";

                    db = new Database();
                    ds = db.SelectAdaptQry(cmd);
                }
            }
            catch (SqlException err)
            {
                HttpContext.Current.Trace.Warn(err.Message);
                ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            catch (Exception err)
            {
                HttpContext.Current.Trace.Warn(err.Message);
                ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            finally
            {
                db.CloseConnection();
            }
            return ds;
        }//End of GetUsedBikeMakesWithCount
	}
}