using Bikewale.Cache.BikeData;
using Bikewale.Cache.Core;
using Bikewale.Cache.Memcache;
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
        /// </summary>
        /// <param name="url">Bikewale.com's URL</param>
        /// <returns>DeepLinkingEntity</returns>
        DeepLinkingEntity IDeepLinking.GetParameters(string url)
        {
            if (string.IsNullOrEmpty(url)) return null;
            DeepLinkingEntity deepLinking = null;
            Match match = null;
            try
            {
                url = ProcessUrl(url);
                if (((match = Regex.Match(url, "^(.*)-bikes/(.*)/$")) != null) && match.Success) //for ModelScreenId
                {
                    string makeId = string.Empty, modelId = string.Empty;
                    //    match = Regex.Match(url, "^(.*)-bikes/(.*)/");
                    makeId = GetMakelId(match.Groups[1].Value);
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
                else if ((match = Regex.Match(url, "^(.*)-bikes/$")) != null && match.Success) //for MakeScreenId
                {
                    string makeId = string.Empty;
                    makeId = GetMakelId(match.Groups[1].Value);
                    if (!(string.IsNullOrEmpty(makeId)))
                    {
                        deepLinking = new DeepLinkingEntity();
                        deepLinking.Params = new Dictionary<string, string>();
                        deepLinking.ScreenID = Bikewale.Entities.AppDeepLinking.ScreenIdEnum.BrandScreen;
                        deepLinking.Params.Add("makeId", makeId);
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
        /// Description : To process URL string
        /// </summary>
        /// <param name="url"></param>
        /// <returns>return url string after after bikewale.com/ </returns>
        private string ProcessUrl(string url)
        {
            try
            {
                int startIndex = url.IndexOf(".com/");
                return (startIndex >= 0 && startIndex < url.Length - 1) ? url.Substring(startIndex + 5) : null;
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "ProcessUrl()");
                objErr.SendMail();
                return null;
            }
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
                //Handle here
            }
            finally
            {
                if (objResponse != null && objResponse.StatusCode == 200)
                {
                    modelId = objResponse.ModelId.ToString();
                }

            }

            return modelId;
        }

        /// <summary>
        /// Created By : Lucky Rathore
        /// Created On : 10 March 2016
        /// Description : To get Model ID
        /// </summary>
        /// <param name="makeName">e.g. tvs</param>
        /// <returns>makeId for e.g. 99</returns>
        private string GetMakelId(string makeName)
        {
            return MakeMapping.GetMakeId(makeName);//MakeMapping.GetMakeId(makeName); 
        }

        /// <summary>
        /// need to be implemented
        /// </summary>
        /// <returns></returns>
        private string GetBasicId()//for news article and similar cases.
        {
            return null;
        }



    }
}
