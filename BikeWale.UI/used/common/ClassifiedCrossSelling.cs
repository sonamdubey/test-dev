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
using Bikewale.Notifications.CoreDAL;
using System.Data.Common;

namespace Bikewale.Used
{
	public class ClassifiedCrossSelling
	{			
		//used for writing the debug messages
		private HttpContext objTrace = HttpContext.Current;
		
		public void GetOtherModels(string versionId, string modelId, string cityId, Repeater rptBikeDetails, string profileId)
		{
			string sql = "";

            sql = @"select  concat(makename,' ', modelname , ' ' ,versionname) as bikename, makename, modelname, c.name as cityname, c.maskingname as citymaskingname, profileid,  bm.maskingname as makemaskingname,bmo.maskingname as modelmaskingname,
				versionid, cityid, price, kilometers, year(makeyear) makeyear
                from livelistings 
                inner join bwcities as c   on c.id = cityid 
                inner join bikemakes bm on bm.id = makeid 
                inner join bikemodels bmo   on bmo.id = modelid 
				where showdetails = 1 and cityid = @v_cityid
                and modelid = @v_modelid and profileid <> @v_profileid
				order by newid() limit 5";

			try
			{

                using (DbCommand cmd = DbFactory.GetDBCommand(sql))
                {
                    //cmd.Parameters.Add("@CityId", SqlDbType.BigInt).Value = cityId;
                    //cmd.Parameters.Add("@ModelId", SqlDbType.BigInt).Value = modelId;
                    //cmd.Parameters.Add("@VersionId", SqlDbType.BigInt).Value = versionId;
                    //cmd.Parameters.Add("@ProfileId", SqlDbType.VarChar, 50).Value = profileId;

                    cmd.Parameters.Add(DbFactory.GetDbParam("@v_cityid", DbParamTypeMapper.GetInstance[SqlDbType.Int], cityId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("@v_modelid", DbParamTypeMapper.GetInstance[SqlDbType.Int], modelId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("@v_versionid", DbParamTypeMapper.GetInstance[SqlDbType.Int], versionId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("@v_profileid", DbParamTypeMapper.GetInstance[SqlDbType.Int], profileId));

                    using (DataSet ds = MySqlDatabase.SelectAdapterQuery(cmd))
                    {

                        rptBikeDetails.DataSource = ds;
                        rptBikeDetails.DataBind();  
                    }
                }
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