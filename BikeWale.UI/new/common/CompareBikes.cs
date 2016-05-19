/***********************************************************/
	// Desc: Common class for research compare section
	// Written By: Satish Sharma On 2009-09-29 5:35 PM	
/***********************************************************/
using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Collections;
using Bikewale.Common;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Configuration;
using System.Data.Common;
//using BikeWale.Controls;

namespace Bikewale.New
{
	public class CompareBikes
	{
		//used for writing the debug messages
		private HttpContext objTrace = HttpContext.Current;
		protected ArrayList arObj = null;
		
		DataSet dsRating;
		
		public CompareBikes(){ }
		
		public CompareBikes( string versions )
		{
			GetAllModelRatings( versions );
		}
		
		/***********************************************************/
			// Input: Version id of Bikes selected by the user to compare
			// Output: Version id of featured Bike
			// Written By: Satish Sharma On 2009-09-29 5:40 PM
            // Modified By : Sadhana Upadhyay on 9 Sept 2014
            // Summary : to get sponsored bike by web api
		/***********************************************************/	
		public static string GetFeaturedBike(string versions)
		{
			string featuredBikeId = "";
			//string sql = "";
            //sql = " Select Top 1 FeaturedVersionId, COUNT(FeaturedVersionId) FeaturedCount, SpotlightUrl from CompareFeaturedBike With(NoLock) "
            //    + " WHERE VersionId IN("+ db.GetInClauseValue(versions, "VersionId", cmd) +") AND IsActive = 1 AND FeaturedVersionId NOT IN("+ db.GetInClauseValue(versions, "VersionId1", cmd) +") AND IsCompare = 1 "
            //    + " GROUP BY FeaturedVersionId, SpotlightUrl "
            //    + " ORDER BY FeaturedCount Desc ";				
			
			try
			{
                //cmd.CommandText = sql;
                //dr = db.SelectQry(cmd);

                //if (dr.Read())
                //{
                //    featuredBikeId = dr["FeaturedVersionId"].ToString() + "#" + dr["SpotlightUrl"].ToString();
                //}
                
                //Commented By : Sadhana Upadhyay on 9 Sept 2014 to get data from WebApi 
                /*
                using (SqlConnection con = new SqlConnection(db.GetConString()))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.CommandText = "GetFeaturedVersionIDByVersionID";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Connection = con;

                        cmd.Parameters.Add("@Versions", SqlDbType.VarChar, 255).Value = versions;
                        cmd.Parameters.Add("@FeaturedVersionId", SqlDbType.Int).Direction = ParameterDirection.Output;

                        con.Open();

                        cmd.ExecuteNonQuery();

                        featuredBikeId = cmd.Parameters["@FeaturedVersionId"].Value.ToString();
                    }
                }
                */
                // Get Sponsored bike by Web Api
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(ConfigurationManager.AppSettings["cwApiHostUrl"]);

                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    HttpResponseMessage response = client.GetAsync("/webapi/SponsoredCarVersion/GetSponsoredCarVersion/?vids=" + versions + "&categoryId=1&platformId=2").Result;

                    HttpContext.Current.Trace.Warn("response", response.ToString());

                    if (response.IsSuccessStatusCode)
                    {
                        featuredBikeId = JsonConvert.DeserializeObject<Int64>(response.Content.ReadAsStringAsync().Result).ToString();
                    }
                }
			}			
			catch (SqlException err)
			{
				ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
				objErr.SendMail();
			}

			return featuredBikeId;
		}
		
		
		// This function will get current versionId from dataset and will match it with the version in the array. 
		// if it matches, it will return index.this is just to match the order of Bikes being compared!
		public int GetVersionIndex( string versionNo, int arrSize, string[] versionId )
		{
			int index = -1;
			
			for ( int i = 0; i < arrSize; i++ )
			{
				//Trace.Warn( "Version Passed - Version Array - Index : " + versionNo + "-" + versionId[i] + "-" + i  );
				
				if ( versionNo == versionId[i] )
				{
					//Trace.Warn( "Version Passed - Version Array - Index : " + versionNo + "-" + versionId[i] + "-" + i  );
					index = i;
					break;
				}
			}
			
			return index;
		}
		
		public void HideComparison( int column, int arrSize, HtmlTable table )
		{
			
			while ( arrSize - column > 0 )
			{	
				for ( int i=0; i < table.Rows.Count; i++ )
				{
					table.Rows[i].Cells.RemoveAt( table.Rows[i].Cells.Count - 1 );
				}
				
				column++; // One column has been deleted, increase the index by 1.
			}
		}
				
		
		void GetAllModelRatings(string versions)
		{
            throw new Exception("Method not used/commented");

            //string sql = string.Empty;
            //SqlCommand cmd =  new SqlCommand();
            //Database db = new Database();
            //dsRating = new DataSet();

            //sql = " SELECT (SELECT MaskingName FROM BikeMakes With(NoLock) WHERE ID = MO.BikeMakeId) AS MakeMaskingName, MO.ID as ModelId, MO.Name AS ModelName,MO.MaskingName AS ModelMaskingName, CV.ID As BikeVersionId, IsNull(MO.ReviewRate, 0) AS ModelRate, IsNull(MO.ReviewCount, 0) AS ModelTotal, "
            //    + " IsNull(CV.ReviewRate, 0) AS VersionRate, IsNull(CV.ReviewCount, 0) AS VersionTotal "
            //    + " FROM BikeModels AS MO, BikeVersions AS CV With(NoLock) WHERE CV.ID in ( " + db.GetInClauseValue(versions, "BikeVersionId", cmd) + " ) AND MO.ID = CV.BikeModelId ";
			
            //cmd.CommandText = sql;
			
            //try
            //{
            //    dsRating = db.SelectAdaptQry(cmd);				
            //}
            //catch ( SqlException err )
            //{
            //    //Trace.Warn("GetAllModelRatings Error = " + err.Message);			
            //    ErrorClass objErr = new ErrorClass(err,objTrace.Request.ServerVariables["URL"]);
            //    objErr.SendMail();
            //}
		}
		
		public string GetModelRatings( string versionId )
		{
			string reviewString = string.Empty;
			try
			{
				if( dsRating.Tables[0].Rows.Count > 0 )			
				{					
					DataRow[] drRating = dsRating.Tables[0].Select("BikeVersionId=" + versionId );

					if( drRating.Length > 0 )
					{
						for(int i = 0; i < drRating.Length; ++i)
						{
							if( Convert.ToDouble(drRating[i]["ModelRate"]) > 0 )
							{
								string reviews = Convert.ToDouble(drRating[i]["ModelTotal"]) > 1 ? " reviews" : " review";
								reviewString += "<div>" + CommonOpn.GetRateImage(Convert.ToDouble(drRating[i]["ModelRate"].ToString())) + "</div>"
											 + " <div style='margin-top:5px;'><a href='/research/" + drRating[i]["MakeMaskingName"].ToString() + "-bikes/" + drRating[i]["ModelMaskingName"].ToString() +"/userreviews/'>"+ drRating[i]["ModelTotal"].ToString() + reviews +" </a></div>";
							}
							else
								reviewString = "<div style='margin-top:10px;'><a href='/research/userreviews-bikem-"+ drRating[i]["ModelId"].ToString() +".html'>Write a review</a></div>";
						}
					}
				}			
				return reviewString;
			}
			catch( Exception err )
			{
				//Trace.Warn("GetModelRatings Error = " + err.Message);
				ErrorClass objErr = new ErrorClass(err,objTrace.Request.ServerVariables["URL"]);
				objErr.SendMail();
				return string.Empty;
			}
        }	//End of GetModelRatings

        /// <summary>
        /// Created By : Sadhana Upadhyay on 24 Sept 2014
        /// Summary : To get featured Compare bike list
        /// </summary>
        /// <param name="topCount"></param>
        /// <returns></returns>
        public DataSet GetComparisonBikeList(UInt16 topCount)
        {
            DataSet ds = null;
            try
            {
                using (System.Data.Common.DbCommand cmd = Bikewale.CoreDAL.DbFactory.GetDBCommand("getbikecomparisonmin"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(Bikewale.CoreDAL.DbFactory.GetDbParam("par_topcount", Bikewale.CoreDAL.DbParamTypeMapper.GetInstance[SqlDbType.SmallInt], topCount));

                    ds = Bikewale.CoreDAL.MySqlDatabase.SelectAdapterQuery(cmd);
                }
            }
            catch (SqlException exSql)
            {
                ErrorClass objErr = new ErrorClass(exSql, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            catch (Exception ex)
            {
                //Response.Write(ex.Message);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            return ds;
        }   // End of GetComparisonBikeList

        /// <summary>
        /// Created By : Sadhana Upadhyay on 24 Sept 2014
        /// Summary : To get compare bike details by version id list
        /// Modified By : Lucky Rathore
        /// Modified On : 15 Feb 2016
        /// Summary : SP name Changed.
        /// Modified By : Lucky Rathore
        /// Modified On : 26 Feb 2016
        /// Summary : SP name Changed.
        /// </summary>
        /// <param name="versionList"></param>
        /// <returns></returns>
        public DataSet GetComparisonBikeListByVersion(string versionList)
        {
            DataSet ds = null;
            try
            {
                using (DbCommand cmd = Bikewale.CoreDAL.DbFactory.GetDBCommand("getcomparisondetails_26022016"))   
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    //cmd.Parameters.Add("@bikeversions", SqlDbType.VarChar, 50).Value = versionList;
                    cmd.Parameters.Add(Bikewale.CoreDAL.DbFactory.GetDbParam("par_bikeversions", Bikewale.CoreDAL.DbParamTypeMapper.GetInstance[SqlDbType.VarChar], 50, versionList));

                    ds = Bikewale.CoreDAL.MySqlDatabase.SelectAdapterQuery(cmd);
                }
            }
            catch (SqlException exSql)
            {
                ErrorClass objErr = new ErrorClass(exSql, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            catch (Exception ex)
            {
                //Response.Write(ex.Message);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            return ds;
        }   
	}
}