using Bikewale.Cache.BikeData;
using Bikewale.Cache.Core;
using Bikewale.DAL.BikeData;
using Bikewale.Entities.AppDeepLinking;
using Bikewale.Entities.BikeData;
using Bikewale.Interfaces.AppDeepLinking;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Notifications;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Bikewale.BAL.AppDeepLinking
{
    /// <summary>
    /// Created By : Lucky Rathore
    /// Created On : 10 March 2016
    /// Description : Class Implement IDeepLinking. 
    /// </summary>
    public class DeepLinking : IDeepLinking
    {
        /// <summary>
        /// Created By : Lucky Rathore
        /// Created On : 10 March 2016
        /// Description : Class to implement funtionality used for deeplinking.
        /// Modified By : Lucky Rathore on 06 May 2016
        /// Description : New rules for upcoming bike detail and landing page added. Regular Expression added to consider url with '/m/' msite url.
        /// Modified By : Lucky Rathore on 10 May 2016
        /// Description : New rules for series page URL is added with make page url responce.
        /// Modified by :   Sumit Kate on 20 Sep 2016
        /// Description :   Handle make and model rename for 301 scenarios
        /// </summary>
        /// <param name="url">Bikewale.com's URL</param>
        /// <returns>DeepLinkingEntity</returns>
        public DeepLinkingEntity GetParameters(string url)
        {
            if (string.IsNullOrEmpty(url)) return null;
            DeepLinkingEntity deepLinking = null;
            Match match = null;
            try
            {
                if (((match = Regex.Match(url, @"\/([A-Za-z0-9\-]+)-bikes\/upcoming\/?$")) != null) && match.Success) //for Upcoming Bikes Detail
                {
                    string makeId = GetMakeId(match.Groups[1].Value);
                    if (!string.IsNullOrEmpty(makeId))
                    {
                        deepLinking = new DeepLinkingEntity();
                        deepLinking.ScreenID = Bikewale.Entities.AppDeepLinking.ScreenIdEnum.UpcomingBikesDetail;
                        deepLinking.Params = new Dictionary<string, string>();
                        deepLinking.Params.Add("makeId", makeId);
                    }
                }
                else if (((match = Regex.Match(url, @"\/upcoming-bikes\/?$")) != null) && match.Success) //for Upcoming Bikes Landing.
                {
                    deepLinking = new DeepLinkingEntity();
                    deepLinking.ScreenID = Bikewale.Entities.AppDeepLinking.ScreenIdEnum.UpcomingBikesLanding;
                }
                else if (
                (((match = Regex.Match(url, @"([A-Za-z0-9\-]+)-bikes\/([A-Za-z0-9\-]+)-series\/?$")) != null) && match.Success)
                || (((match = Regex.Match(url, @"([A-Za-z0-9\-]+)-bikes\/?$")) != null) && match.Success)
                ) //for series page and make page url MakeScreenId.
                {
                    string makeId = GetMakeId(match.Groups[1].Value);
                    if (!(string.IsNullOrEmpty(makeId)))
                    {
                        deepLinking = new DeepLinkingEntity();
                        deepLinking.Params = new Dictionary<string, string>();
                        deepLinking.ScreenID = Bikewale.Entities.AppDeepLinking.ScreenIdEnum.BrandScreen;
                        deepLinking.Params.Add("makeId", makeId);
                    }
                }
                else if (((match = Regex.Match(url, @"([A-Za-z0-9\-]+)-bikes\/([A-Za-z0-9\-]+)\/?$")) != null) && match.Success) //for  ModelScreenId.
                {
                    string makeId = GetMakeId(match.Groups[1].Value),
                        modelId = GetModelId(match.Groups[2].Value);
                    if (!(string.IsNullOrEmpty(makeId) || string.IsNullOrEmpty(modelId)))
                    {
                        deepLinking = new DeepLinkingEntity();
                        deepLinking.ScreenID = Bikewale.Entities.AppDeepLinking.ScreenIdEnum.ModelScreen;
                        deepLinking.Params = new Dictionary<string, string>();
                        deepLinking.Params.Add("makeId", makeId);
                        deepLinking.Params.Add("modelId", modelId);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "GetParameters()");
                objErr.SendMail();
            }

            return deepLinking;
        }

        /// <summary>
        /// Created By : Lucky Rathore
        /// Created On : 10 March 2016
        /// Description : To get Model ID
        /// </summary>
        /// <param name="modelName">e.g. tvs</param>
        /// <returns>modelId e.g. 15</returns>
        private string GetModelId(string modelName)
        {
            ModelMaskingResponse objResponse = null;
            string modelId = string.Empty;
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IBikeMaskingCacheRepository<BikeModelEntity, int>, BikeModelMaskingCache<BikeModelEntity, int>>()
                             .RegisterType<ICacheManager, MemcacheManager>()
                             .RegisterType<IBikeModelsRepository<BikeModelEntity, int>, BikeModelsRepository<BikeModelEntity, int>>()
                            ;
                    var objCache = container.Resolve<IBikeMaskingCacheRepository<BikeModelEntity, int>>();

                    objResponse = objCache.GetModelMaskingResponse(modelName);
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Bikewale.BAL.AndroidApp.GetModelId");
                objErr.SendMail();
            }
            finally
            {
                if (objResponse != null && (objResponse.StatusCode == 200 || objResponse.StatusCode == 301))
                {
                    modelId = Convert.ToString(objResponse.ModelId);
                }

            }

            return modelId;
        }


        private string GetMakeId(string makeMaskingName)
        {
            MakeMaskingResponse objResponse = null;
            string makeId = string.Empty;
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IBikeMakesCacheRepository<int>, BikeMakesCacheRepository<BikeMakeEntity, int>>()
                          .RegisterType<ICacheManager, MemcacheManager>()
                          .RegisterType<IBikeMakes<BikeMakeEntity, int>, BikeMakesRepository<BikeMakeEntity, int>>()
                         ;
                    var objCache = container.Resolve<IBikeMakesCacheRepository<int>>();

                    objResponse = objCache.GetMakeMaskingResponse(makeMaskingName);
                }
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, String.Format("GetMakeId({0})", makeMaskingName));
                objErr.SendMail();
            }
            finally
            {
                if (objResponse != null)
                {
                    if (objResponse.StatusCode == 200 || objResponse.StatusCode == 301)
                    {
                        makeId = Convert.ToString(objResponse.MakeId);
                    }
                }
            }
            return makeId;
        }

    }
}
