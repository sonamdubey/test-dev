using AjaxPro;
using BikewaleOpr.Common;
using BikewaleOpr.Entities;
using BikeWaleOpr.Common;
using Bikewale.Cache.Core;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Bikewale.Interfaces.Cache.Core;

namespace BikewaleOpr.Common
{
    /// <summary>
    /// Created by : Sumit Kate
    /// Created On : 03 Feb 2016
    /// Description : Provide Functionality to Save , update and get all versions detail for Mobile App versions. 
    /// </summary>
    public class AjaxAppVersion
    {
        private readonly ManageAppVersion objManageAppVersion = null;
        public AjaxAppVersion()
        {
            objManageAppVersion = new ManageAppVersion();
        }

        /// <summary>
        /// Created by : Sumit Kate
        /// Created On : 03 Feb 2016
        /// Description : Provide Functionality to Get Detail for Mobile App versions. 
        /// </summary>
        /// <param name="appType">3 for Android App.</param>
        /// <returns>Json form of all the version and its deatil.</returns>
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
        
        /// <summary>
        /// Created by : Sumit Kate
        /// Created On : 03 Feb 2016
        /// Description : Provide Functionality to Save or update for Mobile App versions. 
        /// </summary>
        /// <param name="isLatest">whether app version is latest or not</param>
        /// <param name="isSupported">whether app version is supoorted or not</param>
        /// <param name="appType">3 for android</param>
        /// <param name="appVersionId">Version id for app</param>
        /// <param name="description">for corresponding version</param>
        /// <param name="userId">user who edit or update app</param>
        /// <returns>Json object save/edit opration status.</returns>
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

                 //Refresh memcache object for app versions
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<ICacheManager, MemcacheManager>();
                    ICacheManager objCache = container.Resolve<ICacheManager>();

                    objCache.RefreshCache(string.Format("BW_AppVersion_{0}_Src_{1}", appVersionId, appType));
                }

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