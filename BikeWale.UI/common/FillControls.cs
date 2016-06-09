using Bikewale.Notifications;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI.WebControls;

/// <summary>
/// Summary description for FillControls
/// </summary>

namespace Bikewale.Common
{
    public class FillControls
    {
        /******************************************************************************/
        // This static function bind all versions having reviews count greter then zero
        // for selected model.
        /******************************************************************************/
        public static void FillReviewedVersions(DropDownList drpVersions, string modelId)
        {
            CommonOpn op = new CommonOpn();
            uint _modelid;
            uint.TryParse(modelId, out _modelid);
            try
            {
                string sql = @" select distinct vs.id versionid, vs.name versionname 
                     from bikeversions vs, customerreviews cr   
                     where vs.bikemodelid = @par_modelid and vs.id = cr.versionid and cr.isverified = 1 and cr.isactive = 1 
                     order by versionname ";

                HttpContext.Current.Trace.Warn(sql);

                DbParameter[] param = new[] { Bikewale.CoreDAL.DbFactory.GetDbParam("@par_modelid", DbType.Int32, _modelid) };

                op.FillDropDown(sql, drpVersions, "VersionName", "VersionId", param);

                ListItem item = new ListItem("--All Versions--", "0");
                drpVersions.Items.Insert(0, item);
            }
            catch (SqlException err)
            {
                HttpContext.Current.Trace.Warn(err.Message);
                ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            } // catch Exception
        }
    }
}