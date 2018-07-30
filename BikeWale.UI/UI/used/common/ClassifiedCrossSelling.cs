using Bikewale.Common;
using MySql.CoreDAL;
/*
	This class will contain all the common function related to Sell Bike process
*/
using System;
using System.Data;
using System.Data.Common;
using System.Web;
using System.Web.UI.WebControls;

namespace Bikewale.Used
{
    public class ClassifiedCrossSelling
    {
        //used for writing the debug messages
        private HttpContext objTrace = HttpContext.Current;

        public void GetOtherModels(string versionId, string modelId, string cityId, Repeater rptBikeDetails, string profileId)
        {
            string sql = "";

            sql = @"select  concat(ll.makename,' ', ll.modelname , ' ' ,ll.versionname) as bikename, ll.makename, ll.modelname, c.name as cityname, c.maskingname as citymaskingname, ll.profileid,  bmo.makemaskingname as makemaskingname,bmo.maskingname as modelmaskingname,
				ll.versionid, ll.cityid, ll.price, ll.kilometers, year(ll.makeyear) makeyear
                from livelistings ll
                inner join bwcities as c   on c.id = ll.cityid                 
                inner join bikemodels bmo   on bmo.id = ll.modelid 
				where ll.showdetails = 1 and ll.cityid = @v_cityid
                and ll.modelid = @v_modelid and ll.profileid <> @v_profileid
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

                        if (ds != null)
                        {
                            rptBikeDetails.DataSource = ds;
                            rptBikeDetails.DataBind();
                        }
                    }
                }
            }
            catch (Exception err)
            {
                objTrace.Trace.Warn(err.Message + err.Source);
                ErrorClass.LogError(err, objTrace.Request.ServerVariables["URL"]);
                
            }
        }
    }
}