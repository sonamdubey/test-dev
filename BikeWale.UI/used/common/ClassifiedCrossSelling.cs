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
using System.Data.Common;
using MySql.CoreDAL;

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
				order by rand() limit 5";

			try
			{

                using (DbCommand cmd = DbFactory.GetDBCommand(sql))
                {

                    cmd.Parameters.Add(DbFactory.GetDbParam("@v_cityid", DbType.Int32, cityId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("@v_modelid", DbType.Int32, modelId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("@v_versionid", DbType.Int32, versionId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("@v_profileid", DbType.String, profileId));

                    using (DataSet ds = MySqlDatabase.SelectAdapterQuery(cmd, ConnectionType.ReadOnly))
                    {

                        if (ds!=null)
                        {
                            rptBikeDetails.DataSource = ds;
                            rptBikeDetails.DataBind();  
                        } 
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