using System;
using System.Collections.Generic;
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
            string sql;

            try
            {
                sql = " SELECT Distinct Vs.ID VersionId, Vs.Name VersionName "
                    + " FROM BikeVersions Vs, CustomerReviews Cr With(NoLock) "
                    + " WHERE Vs.BikeModelId = @ModelId AND Vs.ID = Cr.VersionId AND Cr.IsVerified = 1 AND Cr.IsActive = 1 "
                    + " ORDER BY VersionName ";

                HttpContext.Current.Trace.Warn(sql);

                SqlParameter[] param = 
				{
					new SqlParameter("@ModelId", modelId)
				};

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