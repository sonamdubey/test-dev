/*
	This class will contain all the common function related to Sell Bike process
*/
using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI.WebControls;
using Bikewale.Common;

namespace Bikewale.Used
{
	public class ClassifiedCrossSelling
	{			
		//used for writing the debug messages
		private HttpContext objTrace = HttpContext.Current;
		
		public void GetOtherModels(string versionId, string modelId, string cityId, Repeater rptBikeDetails, string profileId)
		{
			string sql = "";

            sql = "SELECT TOP 5 (MakeName + ' ' + ModelName + ' ' + VersionName) AS BikeName, MakeName, ModelName, C.Name AS CityName, C.MaskingName AS CityMaskingName, ProfileId,  BM.MaskingName AS MakeMaskingName,BMO.MaskingName AS ModelMaskingName,"
				+ " VersionId, CityId, Price, Kilometers, Year(MakeYear) MakeYear"
                + " FROM LiveListings With(NoLock) "
                + " INNER JOIN BWCities AS C With(NoLock) on C.Id = CityId "
                + " INNER JOIN BikeMakes BM With(NoLock) ON BM.ID = MakeId "
                + " INNER JOIN BikeModels BMO With(NoLock) ON BMO.ID = ModelId "
				+ " WHERE ShowDetails = 1 AND CityId = @CityId "
                + " AND ModelId = @ModelId AND ProfileId <> @ProfileId "
				+ " ORDER BY NewId()";

            objTrace.Trace.Warn("GetOtherModels cityId : ", cityId);
            objTrace.Trace.Warn("GetOtherModels modelId : ", modelId);            
            objTrace.Trace.Warn("GetOtherModels ProfileId : ", profileId);
			objTrace.Trace.Warn("GetOtherModels sql : ", sql);

			try
			{
				SqlCommand cmd =  new SqlCommand(sql);
				cmd.Parameters.Add("@CityId", SqlDbType.BigInt).Value = cityId;
				cmd.Parameters.Add("@ModelId", SqlDbType.BigInt).Value = modelId;
				cmd.Parameters.Add("@VersionId", SqlDbType.BigInt).Value = versionId;
                cmd.Parameters.Add("@ProfileId", SqlDbType.VarChar, 50).Value = profileId;
				
				Database db = new Database();
				rptBikeDetails.DataSource = db.SelectAdaptQry(cmd);
				rptBikeDetails.DataBind();
			}
			catch(Exception err)
			{
				objTrace.Trace.Warn(err.Message + err.Source);
				ErrorClass objErr = new ErrorClass(err, objTrace.Request.ServerVariables["URL"]);
				objErr.SendMail();
			}
		}
	}
}