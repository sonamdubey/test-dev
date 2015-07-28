/***********************************************************/
	// Desc: Common class for research compare section
	// Written By: Satish Sharma On 2009-09-29 5:35 PM	
/***********************************************************/
using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Web;

namespace Bikewale.Common
{
    /// <summary>
    /// Added By : Ashish G. Kamble on 14/8/2012
    /// </summary>
	public class FeaturedBike
	{		
		/***********************************************************/
			// Input: Version id of bikes selected by the user to compare
			// Output: Version id of featured bike
			// Written By: Satish Sharma On 2009-09-29 5:40 PM
		/***********************************************************/	
		public static string GetFeaturedVersion(string versions)
		{
			string featuredBikeId = "";
			Database db = new Database();
			string sql = "";
			
			SqlCommand cmd = new SqlCommand();
			
			// IsNewSearch set to 1 for new search page 
			sql = " Select Top 1 FeaturedVersionId, SpotlightUrl "
                + " FROM CompareFeaturedBike Cf With(NoLock) "
				+ " WHERE VersionId IN("+ db.GetInClauseValue(versions, "VersionId", cmd) +") AND "
				+ " IsActive = 1 AND IsNewSearch = 1";// IsNewSearch = 1 for new search page 
				
				//AND FeaturedVersionId NOT IN("+ db.GetInClauseValue(versions, "VersionExd", cmd) +") 
			
			SqlDataReader dr = null;
			
			try
			{
				cmd.CommandText = sql;
				dr = db.SelectQry(cmd);
				
				if(dr.Read())
				{					
					featuredBikeId = dr["FeaturedVersionId"].ToString() + "#" + dr["SpotlightUrl"].ToString();					
				}				
			}			
			catch (SqlException err)
			{
				ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
				objErr.SendMail();
			}
			finally
			{
				dr.Close();
				db.CloseConnection();
			}
			
			return featuredBikeId;
		}
		
		public static DataSet GetFeaturedBikeForNewSearch(string featuredVersion)
		{
			DataSet ds = new DataSet();
			string sql = "";
			
            //sql = " SELECT   CV.ID  AS VersionId, MO.ID AS ModelId, MA.Name + ' ' + MO.Name AS BikeModel," 
            //    + " MA.ID AS MakeId, Ma.Name MakeName, Mo.Name ModelName, CV.Name VersionName, "
            //    + " MA.Name + ' ' + MO.Name + ' ' + CV.Name AS Bike, MO.SmallPic, CB.Name AS BodyStyle, "
            //    + " SD.Displacement, SD.FuelType, SD.SeatingCapacity, SD.Speeds, SD.MileageOverall, "
            //    + " SP.AvgPrice AS Price, SD.TransmissionType, "
            //    + " COUNT(MO.ID) OVER( PARTITION BY MO.ID ) AS ModelCount, "
            //    + " MIN(ISNULL(SP.AvgPrice , 0)) OVER( PARTITION BY MO.ID ) AS MinPrice, " 
            //    + " MAX(ISNULL(SP.AvgPrice ,0)) OVER( PARTITION BY MO.ID ) AS MaxPrice, "
            //    + " CD.PowerWindows,  CD.PowerSteering, CD.ABS, CD.AirConditioner, CD.CentralLocking, "
            //    + " CD.DriverAirBags,  ISNULL(SUBSTRING(sd.power, 0, CHARINDEX('@',sd.power)),'0') AS Power, "
            //    + " ISNULL(MO.ReviewRate, 0) MoReviewRate, ISNULL(MO.ReviewCount,0) MoReviewCount, "
            //    + " ISNULL(CV.ReviewRate, 0) VsReviewRate, ISNULL(CV.ReviewCount,0) VsReviewCount  "
				
            //    + " From  BikeMakes AS MA, BikeModels AS MO, BikeBodyStyles AS CB, NewBikeSpecifications AS SD, "
            //    + " Con_NewBikeNationalPrices AS SP,  "
            //    + " ( BikeVersions AS CV  LEFT JOIN NewBikeStandardFeatures  CD ON CD.BikeVersionId = CV.Id ) "
            //    + " Where  MA.ID = MO.BikeMakeId AND MO.ID = CV.BikeModelId AND CV.New = 1 AND  SP.VersionId = CV.ID "
            //    + " AND CB.ID = CV.BodyStyleId AND  SD.BikeVersionId = CV.ID   AND MO.Id IN ( SELECT BikeModelId FROM BikeVersions WHERE Id = "+ featuredVersion +" ) ";

            sql = " SELECT   CV.ID  AS VersionId, MO.ID AS ModelId, MA.Name + ' ' + MO.Name AS BikeModel, "
                + " MA.ID AS MakeId, Ma.Name MakeName,Ma.MaskingName AS MakeMaskingName, Mo.Name ModelName, Mo.MaskingName AS ModelMaskingName, CV.Name VersionName, "
				+ " MA.Name + ' ' + MO.Name + ' ' + CV.Name AS Bike, MO.SmallPic, CB.Name AS BodyStyle, "
				+ " SD.Displacement, SD.FuelType, SD.TopSpeed, SD.FuelEfficiencyOverall, "
				+ " SP.Price AS Price, SD.TransmissionType, "
				+ " COUNT(MO.ID) OVER( PARTITION BY MO.ID ) AS ModelCount, "
				+ " MIN(ISNULL(SP.Price , 0)) OVER( PARTITION BY MO.ID ) AS MinPrice, "
				+ " MAX(ISNULL(SP.Price ,0)) OVER( PARTITION BY MO.ID ) AS MaxPrice, "
				+ " SD.AntilockBrakingSystem AS ABS, ISNULL(sd.MaxPower, 0) AS Power, "
				+ " ISNULL(MO.ReviewRate, 0) MoReviewRate, ISNULL(MO.ReviewCount,0) MoReviewCount, "
				+ " ISNULL(CV.ReviewRate, 0) VsReviewRate, ISNULL(CV.ReviewCount,0) VsReviewCount "

                + " FROM BikeVersions AS CV  With(NoLock) "
                + "	INNER JOIN BikeModels AS MO With(NoLock) ON MO.ID = CV.BikeModelId "
                + "	INNER JOIN BikeMakes AS MA With(NoLock) ON MA.ID = MO.BikeMakeId "
                + "	LEFT JOIN BikeBodyStyles AS CB With(NoLock) ON CB.ID = CV.BodyStyleId "
                + "	LEFT JOIN NewBikeSpecifications AS SD With(NoLock) ON SD.BikeVersionId = CV.ID "
                + "	LEFT JOIN NewBikeShowroomPrices AS SP With(NoLock) ON SP.BikeVersionId = CV.ID "
					
				+ " WHERE "
				+ "	CV.New = 1 AND CV.IsDeleted = 0 AND CV.Futuristic = 0 AND "
				+ "	MO.Futuristic = 0 AND MO.IsDeleted = 0 AND MO.New = 1 AND "
                + " MO.Id IN ( SELECT BikeModelId FROM BikeVersions  With(NoLock) WHERE Id = " + featuredVersion + " ) ";

			HttpContext.Current.Trace.Warn(sql);
			try
			{
				Database db = new Database();
				ds = db.SelectAdaptQry(sql);
			}
			catch (SqlException err)
			{
				ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
				objErr.SendMail();
			}
			
			return ds;
		}
		
		/***********************************************************/
			// Input: Model id of bikes selected by the user to compare
			// Output: Model id of featured bike
			// Written By: Rajeev On 2011-02-28 4:15 PM
		/***********************************************************/	
		public static string GetFeaturedModel(string models)
		{
			string featuredBikeId = "";
			Database db = new Database();
			string sql = "";
			
			SqlCommand cmd = new SqlCommand();
			
			// IsResearch = 1 for research bikes similar bikes 
            sql = " Select Top 1 CVF.BikeModelId, Cf.SpotlightUrl FROM CompareFeaturedBike Cf, BikeVersions AS CVF, BikeVersions AS CVS With(NoLock) WHERE "
				+ " CVS.BikeModelId IN ("+ db.GetInClauseValue(models, "ModelId", cmd) +") AND "
				+ " CF.VersionId = CVS.ID AND CVF.ID = CF.FeaturedVersionId AND CVF.BikeModelId NOt IN ("+ db.GetInClauseValue(models, "ModelExId", cmd) +") AND "
				+ " CF.IsActive = 1 AND CF.IsResearch = 1 ";

			
			SqlDataReader dr = null;
			
			try
			{
				cmd.CommandText = sql;
				dr = db.SelectQry(cmd);
				
				if(dr.Read())
				{					
					featuredBikeId = dr["BikeModelId"].ToString() + "#" + dr["SpotlightUrl"].ToString();					
				}				
			}			
			catch (SqlException err)
			{
				ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
				objErr.SendMail();
			}
			finally
			{
				dr.Close();
				db.CloseConnection();
			}
			
			return featuredBikeId;
		}
	}
}