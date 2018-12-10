using Carwale.DTOs.Advertisment;
using Carwale.Entity.Advertizings.Apps;
using Carwale.Interfaces.Advertizings.App;
using Carwale.Notifications;
using System;
using System.Configuration;

namespace Carwale.BL.Advertizing.Apps
{
    public class AppSplashScreenBL : IAppSplashScreenBL
    {
        private readonly IAppSplashRepository _splashRepo;
        private readonly IAppSplashCache _splashCache;
        private string hostUrl = "", dirPath = "", fileName = "";
        public AppSplashScreenBL(IAppSplashRepository splashRepo, IAppSplashCache splashCache)
        {
            _splashRepo = splashRepo;
            _splashCache = splashCache;

        }        
        
        public void GetURLParts(string URL)
        {
            if (URL != "")
            {
                URL = URL.Replace("https://", "");
                var URLParts = URL.Split('/');
                if (URLParts.Length > 0)
                {
                    hostUrl = URLParts[0];
                    fileName = URLParts[URLParts.Length - 1];
                    dirPath = URL.Replace(fileName, "").Replace(hostUrl, "").TrimEnd('/').TrimStart('/');
                }
            }
        }

        /// <summary>
        /// Author: Supreksha Singh
        /// Desc: This function will return single splash screen image url randomly and timeout
        /// from list of active splash screen for platformId
        /// </summary>
        /// <param name="platformId"></param>
        /// <returns>splash screen image url and timeout</returns>
        public SplashScreenBanner GetRandomSplashScreen(int platformId, int applicationId)
        {
            SplashScreenBanner customeSplash = new SplashScreenBanner();

            int randomIndex = 0;
            try
            {
                var splashList = _splashCache.GetSplashSreenBanner(platformId, applicationId);
                if (splashList != null && splashList.Count > 0)
                {
                    if (splashList.Count == 1)
                    {
                        customeSplash.Splashurl = splashList[0].Splashurl;
                        customeSplash.AdTimeOut = splashList[0].AdTimeOut;
                        customeSplash.IsDefault = splashList[0].IsDefault;
                    }
                    else
                    {
                        Random randomSplashScreen = new Random();
                        randomIndex = randomSplashScreen.Next(0, splashList.Count);
                        customeSplash.Splashurl = splashList[randomIndex].Splashurl;
                        customeSplash.AdTimeOut = splashList[randomIndex].AdTimeOut;
                        customeSplash.IsDefault = splashList[randomIndex].IsDefault;
                    }

                    return customeSplash;
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "AppSplashScreenBL.GetRandomSplashScreen()");
                objErr.LogException();
            }

            return customeSplash;
        }

        /// <summary>
        /// Author: Supreksha Singh
        /// This function will return highest priority splash screen, 
        /// here cache object will be in sorted order on priority hence will return 1st element of list;
        /// </summary>
        /// <param name="platformId"></param>
        /// <returns>splash screen image url</returns>
        public CustomSplashDTO GetSplashScreenByPriority(int platformId,int applicationId)
        {
            CustomSplashDTO customeSplash = new CustomSplashDTO();
            try
            {
                var splashList = _splashCache.GetSplashSreenBanner(platformId, applicationId);
                if (splashList != null && splashList.Count > 0)
                    customeSplash.SplashImgUrl = splashList[0].Splashurl;

                return customeSplash;
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "AppSplashScreenBL.GetSplashScreenByPriority()");
                objErr.LogException();
            }
            customeSplash.SplashImgUrl = ConfigurationManager.AppSettings["CustomSplashImgUrl"] != null ? ConfigurationManager.AppSettings["CustomSplashImgUrl"].ToString() : "";

            return customeSplash;
        }
    }
}
