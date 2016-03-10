using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bikewale.Interfaces.AndroidApp;
using Bikewale.Entities.AndroidApp;
using System.Text.RegularExpressions;
using System.Collections.Specialized;
using Microsoft.Practices.Unity;
using Bikewale.Interfaces.BikeData;
using Bikewale.Entities.BikeData;
using Bikewale.DAL.BikeData;
using Bikewale.Cache.BikeData;
using Bikewale.Cache.Core;
using Bikewale.Notifications;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Cache.Memcache;
using System.Collections;

namespace Bikewale.BAL.AndroidApp
{
    /// <summary>
    /// Created By : Lucky Rathore
    /// Created On : 10 March 2016
    /// </summary>
    public class DeepLinking : IDeepLinking
    {
        /// <summary>
        /// Created By : Lucky Rathore
        /// Created On : 10 March 2016
        /// </summary>
        /// <param name="url">Bikewale.com's URL</param>
        /// <returns>DeepLinkingEntity</returns>
        public DeepLinkingEntity GetParameters(string url)
        {
            url = ProcessUrl(url);
            DeepLinkingEntity deepLinkinp = null;
            if (Regex.IsMatch(url, "^(.*)-bikes/(.*)/")) //for ModelScreenId
            {
                string makeId = string.Empty, modelId = string.Empty;
                var match = Regex.Match(url, "^(.*)-bikes/(.*)/");
                makeId = GetMakelId(match.Groups[1].Value);
                modelId = GetModelId(match.Groups[2].Value);
                if (!(string.IsNullOrEmpty(makeId) || string.IsNullOrEmpty(modelId)))
                {
                    deepLinkinp = new DeepLinkingEntity();
                    deepLinkinp.ScreenID = ScreenIdEnum.ModelScreen;
                    deepLinkinp.Params = new Dictionary<string, string>();
                    deepLinkinp.Params.Add("makeId", makeId);
                    deepLinkinp.Params.Add("modelId", modelId);
                }
            }
            else if (Regex.IsMatch(url, "^(.*)-bikes/$")) //for MakeScreenId
            {
                string makeId = string.Empty;
                var match = Regex.Match(url, "^(.*)-bikes/$");
                makeId = GetMakelId(match.Groups[1].Value);
                if (!(string.IsNullOrEmpty(makeId))) 
                {
                    deepLinkinp = new DeepLinkingEntity();
                    deepLinkinp.Params = new Dictionary<string, string>();
                    deepLinkinp.ScreenID = ScreenIdEnum.BrandScreen;
                    deepLinkinp.Params.Add("makeId", makeId);
                }
            }

            return deepLinkinp;
        }

        /// <summary>
        /// Created By : Lucky Rathore
        /// Created On : 10 March 2016
        /// </summary>
        /// <param name="url"></param>
        /// <returns>return url string after after bikewale.com/ </returns>
        private string ProcessUrl(string url)
        {  
            int startIndex = url.IndexOf(".com/");
            return (startIndex >= 0 && startIndex < url.Length-1) ? url.Substring(startIndex + 5) : null;
        }

        /// <summary>
        /// Created By : Lucky Rathore
        /// Created On : 10 March 2016
        /// </summary>
        /// <param name="modelName">e.g. tvs</param>
        /// <returns>modelId e.g. 15</returns>
        private string GetModelId(string modelName)
        {
            ModelMaskingResponse objResponse = null;
            string modelId = string.Empty; //ask
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
        /// </summary>
        /// <param name="makeName">e.g. wego</param>
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
