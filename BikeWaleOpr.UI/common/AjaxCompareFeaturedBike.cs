using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Configuration;
using System.IO;
using Ajax;
using AjaxPro;

using BikeWaleOpr.Common;

namespace BikeWaleOpr
{
	public class AjaxCompareFeaturedBike
	{
		
		[AjaxPro.AjaxMethod()]
        public bool SaveCompareFeaturedBike(string comparisonBikes, string featuredBike)
		{
            bool status = false;
      
			string[] arrCompareBike = comparisonBikes.Split(',');
			for (int i=0; i<arrCompareBike.Length; i++)
			{
                 if (SaveData(arrCompareBike[i], featuredBike))
                     status = true;
                  else
                      status = false;
            }

            return status;
		}
        /*
         * Modified :   Vaibhav K (2-May-2012)
         * Summary  :   Copy compared Bikes from one featuredversion against new featured version
        */
        [AjaxPro.AjaxMethod()]
        public bool CopyComparedBikes(string copyBike, string featuredBike)
        {
            throw new Exception("Method not used/commented");


            //bool status = false;
            //string[] copyData = copyBike.Split('|');
            //SqlConnection con;
            //SqlCommand cmd;

            //SqlParameter prm;
            //Database db = new Database();

            //string conStr = db.GetConString();
            //con = new SqlConnection(conStr);

            //try
            //{
            //    cmd = new SqlCommand("CopyComparedBikes", con);
            //    cmd.CommandType = CommandType.StoredProcedure;
                
            //    prm = cmd.Parameters.Add("@NewVersion", SqlDbType.Int);
            //    prm.Value = featuredBike;

            //    prm = cmd.Parameters.Add("@FeaturedBikeId", SqlDbType.Int);
            //    prm.Value = copyData[0];

            //    prm = cmd.Parameters.Add("@IsCompare", SqlDbType.Bit);
            //    prm.Value = Convert.ToBoolean(Convert.ToInt32(copyData[2]));

            //    prm = cmd.Parameters.Add("@IsNewSearch", SqlDbType.Bit);
            //    prm.Value = Convert.ToBoolean(Convert.ToInt32(copyData[3]));

            //    prm = cmd.Parameters.Add("@IsRecommend", SqlDbType.Bit);
            //    prm.Value = Convert.ToBoolean(Convert.ToInt32(copyData[4]));
                
            //    prm = cmd.Parameters.Add("@IsResearch", SqlDbType.Bit);
            //    prm.Value = Convert.ToBoolean(Convert.ToInt32(copyData[5]));
                
            //    prm = cmd.Parameters.Add("@SpotLightURL", SqlDbType.VarChar, 150);
            //    prm.Value = copyData[1].ToString();
                
            //    con.Open();
            //    cmd.ExecuteNonQuery();
                
            //    status = true;
            //}
            //catch (SqlException exerr)
            //{

            //    ErrorClass objErr = new ErrorClass(exerr, "AjaxFunctions.UpdateLaunchDet");
            //    objErr.SendMail();
            //    HttpContext.Current.Trace.Warn("error: " + exerr);
            //    //str = exerr.Message;
            //    status = false;
            //}
            //catch (Exception err)
            //{
            //    ErrorClass objErr = new ErrorClass(err, "AjaxCompareFeaturedBike.CopyComparedBikes");
            //    objErr.SendMail();
            //    HttpContext.Current.Trace.Warn("ex err: " + err);
            //    status = false;
            //    //str = err.Message;
            //}
            //finally
            //{
            //    if (con.State == ConnectionState.Open)
            //    {
            //        con.Close();
            //    }
            //}
            //return status;
        }
        private bool SaveData(string compareBike, string featuredBike)
		{
            throw new Exception("Method not used/commented");
            //SqlConnection con;
            //SqlCommand cmd;
			
            //SqlParameter prm;
            //Database db = new Database();
           			
            //string conStr = db.GetConString();
            //con = new SqlConnection( conStr );
            //bool isInserted = false;
            //string[] arrDetails = compareBike.Split('|');
            ////string str = "";
			
            //try
            //{
				
            //    cmd = new SqlCommand("SaveCompareFeaturedBike", con);
            //    cmd.CommandType = CommandType.StoredProcedure;
			
            //    prm = cmd.Parameters.Add("@VersionId", SqlDbType.Int);
            //    //prm.Value = compareBike;
            //    prm.Value = arrDetails[0];
				
            //    prm = cmd.Parameters.Add("@FeatureVersionId", SqlDbType.Int);
            //    prm.Value = featuredBike;

            //    prm = cmd.Parameters.Add("@IsCompare", SqlDbType.Bit);
            //    prm.Value = Convert.ToBoolean(Convert.ToInt32(arrDetails[2]));

            //    prm = cmd.Parameters.Add("@IsNewSearch", SqlDbType.Bit);
            //    prm.Value = Convert.ToBoolean(Convert.ToInt32(arrDetails[3]));

            //    prm = cmd.Parameters.Add("@IsRecommend", SqlDbType.Bit);
            //    prm.Value = Convert.ToBoolean(Convert.ToInt32(arrDetails[4]));

            //    prm = cmd.Parameters.Add("@IsResearch", SqlDbType.Bit);
            //    prm.Value = Convert.ToBoolean(Convert.ToInt32(arrDetails[5]));

            //    prm = cmd.Parameters.Add("@SpotlightUrl", SqlDbType.VarChar,150);
            //    prm.Value = arrDetails[1];
								
            //    con.Open();
            //    cmd.ExecuteNonQuery();

            //    isInserted = true;
            //}
            //catch (SqlException exerr)
            //{

            //    ErrorClass objErr = new ErrorClass(exerr, "AjaxFunctions.UpdateLaunchDet");
            //    objErr.SendMail();
            //    //str = exerr.Message;
            //    isInserted = false;
            //}
            //catch(Exception err)
            //{
            //    ErrorClass objErr = new ErrorClass(err,"AjaxCompareFeaturedBike.SaveCompareFeaturedBike");
            //    objErr.SendMail();
            //    isInserted = false;
            //    //str = err.Message;
            //} 
            //finally
            //{
            //    if(con.State == ConnectionState.Open)
            //    {
            //        con.Close();
            //    }
            //}
            //return isInserted;
		}
		
		[AjaxPro.AjaxMethod()]		
		public bool DeleteCompareFeaturedBike(string comparisonBike, string featuredBike)
		{
            throw new Exception("Method not used/commented");

            //bool retVal	 = false;
            //string sql = "UPDATE CompareFeaturedBike SET IsActive = 0 WHERE VersionId = "+ comparisonBike +" AND FeaturedVersionId = " + featuredBike;
            //Database db = new Database();
			
            //try
            //{
            //    db.UpdateQry(sql);
            //    retVal = true;
            //}
            //catch(Exception err)
            //{
            //    retVal = false;
            //    ErrorClass objErr = new ErrorClass(err,"AjaxCompareFeaturedBike.DeleteCompareFeaturedBike");
            //    objErr.SendMail();
            //} 
			
            //return retVal;
		}
		
		
		[AjaxPro.AjaxMethod()]		
		public bool DeleteFeaturedBike(string featuredBike)
		{
            throw new Exception("Method not used/commented");

            //bool retVal	 = false;
            //string sql = "UPDATE CompareFeaturedBike SET IsActive = 0 WHERE FeaturedVersionId = " + featuredBike;
            //Database db = new Database();
			
            //try
            //{
            //    db.DeleteQry(sql);
            //    retVal = true;
            //}
            //catch(Exception err)
            //{
            //    retVal = false;
            //    ErrorClass objErr = new ErrorClass(err,"AjaxCompareFeaturedBike.DeleteFeaturedBike");
            //    objErr.SendMail();
            //} 
			
			//return retVal;
		}
	}
}		