using AjaxPro;
using BikewaleOpr.Common;
using BikewaleOpr.Entities;
using BikeWaleOpr.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BikewaleOpr.Common
{
    public class AjaxAppVersion
    {
        private readonly ManageAppVersion objManageAppVersion = null;
        public AjaxAppVersion()
        {
            objManageAppVersion = new ManageAppVersion();
        }
        [AjaxMethod]
        public string GetAppVersions(int appType)
        {
            IEnumerable<AppVersionEntity> appVersions = null;
            string json = string.Empty;
            try
            {
                appVersions = objManageAppVersion.GetAppVersions((AppEnum)appType);
                if (appVersions != null && appVersions.Count() > 0)
                {
                    json = JavaScriptSerializer.Serialize(appVersions);
                }
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            return json;
        }
        [AjaxMethod]
        public string SaveAppVersion(bool isLatest, bool isSupported, Int16 appType, Int32 appVersionId, string description, string userId)
        {
            IEnumerable<AppVersionEntity> appVersions = null;
            AppVersionEntity entity = null;
            bool isSuccess = false;
            string json = string.Empty;
            try
            {
                entity = new AppVersionEntity();
                entity.AppType = (AppEnum)appType;
                entity.Description = description;
                entity.Id = appVersionId;
                entity.IsLatest = isLatest;
                entity.IsSupported = isSupported;
                isSuccess = objManageAppVersion.SaveAppVersion(entity, userId);
                appVersions = objManageAppVersion.GetAppVersions((AppEnum)appType);
                if (appVersions != null && appVersions.Count() > 0)
                {
                    json = JavaScriptSerializer.Serialize(appVersions);
                }
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            return json;
        }
    }
}