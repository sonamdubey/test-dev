using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Bikewale.Entities.AppDeepLinking;
using Bikewale.Entities.BikeData;
using Bikewale.Interfaces.AppDeepLinking;
using Bikewale.Interfaces.BikeData;
using Bikewale.Notifications;

namespace Bikewale.BAL.AppDeepLinking
{
    /// <summary>
    /// Created By : Lucky Rathore
    /// Created On : 10 March 2016
    /// Description : Class Implement IDeepLinking. 
    /// Modified by :   Sumit Kate on 09 Feb 2017
    /// Description :   Added private member variables for IBikeMakesCacheRepository and IBikeMaskingCacheRepository
    /// </summary>
    public class DeepLinking : IDeepLinking
    {
        private readonly IBikeMakesCacheRepository _makeCache;
        private readonly IBikeMaskingCacheRepository<BikeModelEntity, int> _modelCache;
        public DeepLinking(IBikeMakesCacheRepository makeCache, IBikeMaskingCacheRepository<BikeModelEntity, int> modelCache)
        {
            _makeCache = makeCache;
            _modelCache = modelCache;
        }
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
        /// Modified by :   Sumit Kate on 09 Feb 2017
        /// Description :   removed send mail and other opimization
        /// </summary>
        /// <param name="url">Bikewale.com's URL</param>
        /// <returns>DeepLinkingEntity</returns>
        public DeepLinkingEntity GetParameters(string url)
        {
            if (string.IsNullOrEmpty(url)) return null;
            DeepLinkingEntity deepLinking = null;
            Match match = null;
            uint makeId, modelId;
            string newMakeMaskingName = string.Empty;
            try
            {
                if (((match = Regex.Match(url, @"\/([A-Za-z0-9\-]+)-bikes\/upcoming\/?$")) != null) && match.Success) //for Upcoming Bikes Detail
                {
                    makeId = GetMakeId(match.Groups[1].Value, out newMakeMaskingName);
                    if (makeId > 0)
                    {
                        deepLinking = new DeepLinkingEntity();
                        deepLinking.ScreenID = Bikewale.Entities.AppDeepLinking.ScreenIdEnum.UpcomingBikesDetail;
                        deepLinking.Params = new Dictionary<string, string>();
                        deepLinking.Params.Add("makeId", Convert.ToString(makeId));
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
                    makeId = GetMakeId(match.Groups[1].Value, out newMakeMaskingName);
                    if (makeId > 0)
                    {
                        deepLinking = new DeepLinkingEntity();
                        deepLinking.Params = new Dictionary<string, string>();
                        deepLinking.ScreenID = Bikewale.Entities.AppDeepLinking.ScreenIdEnum.BrandScreen;
                        deepLinking.Params.Add("makeId", Convert.ToString(makeId));
                    }
                }
                else if (((match = Regex.Match(url, @"([A-Za-z0-9\-]+)-bikes\/([A-Za-z0-9\-]+)\/?$")) != null) && match.Success) //for  ModelScreenId.
                {
                    makeId = GetMakeId(match.Groups[1].Value, out newMakeMaskingName);
                    modelId = GetModelId(newMakeMaskingName, match.Groups[2].Value);
                    if (makeId > 0 && modelId > 0)
                    {
                        deepLinking = new DeepLinkingEntity();
                        deepLinking.ScreenID = Bikewale.Entities.AppDeepLinking.ScreenIdEnum.ModelScreen;
                        deepLinking.Params = new Dictionary<string, string>();
                        deepLinking.Params.Add("makeId", Convert.ToString(makeId));
                        deepLinking.Params.Add("modelId", Convert.ToString(modelId));
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "GetParameters()");
            }

            return deepLinking;
        }

        /// <summary>
        /// Created By : Lucky Rathore
        /// Created On : 10 March 2016
        /// Description : To get Model ID
        /// Modified by :   Sumit Kate on 09 Feb 2017
        /// Description :   removed unity container
        /// </summary>
        /// <param name="modelName">e.g. tvs</param>
        /// <returns>modelId e.g. 15</returns>
        private uint GetModelId(string makeName, string modelName)
        {
            ModelMaskingResponse objResponse = null;
            uint modelId = 0;
            try
            {
                objResponse = _modelCache.GetModelMaskingResponse(string.Format("{0}_{1}", makeName, modelName));
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.BAL.AndroidApp.GetModelId");
            }
            finally
            {
                if (objResponse != null)
                {
                    if (objResponse.StatusCode == 200)
                    {
                        modelId = objResponse.ModelId;
                    }
                    else if (objResponse.StatusCode == 301)
                    {
                        modelId = GetModelId(makeName, objResponse.MaskingName);
                    }
                }
            }

            return modelId;
        }

        /// <summary>
        /// Modified by :   Sumit Kate on 09 Feb 2017
        /// Description :   removed UnityContainer
        /// </summary>
        /// <param name="makeMaskingName"></param>
        /// <returns></returns>
        private uint GetMakeId(string makeMaskingName, out string newMakeMaskingName)
        {
            MakeMaskingResponse objResponse = null;
            newMakeMaskingName = string.Empty;
            uint makeId = 0;
            try
            {
                objResponse = _makeCache.GetMakeMaskingResponse(makeMaskingName);
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("GetMakeId({0})", makeMaskingName));
            }
            finally
            {
                if (objResponse != null)
                {
                    if (objResponse.StatusCode == 200)
                    {
                        makeId = objResponse.MakeId;
                        newMakeMaskingName = makeMaskingName;
                    }
                    else if (objResponse.StatusCode == 301)
                    {
                        makeId = GetMakeId(objResponse.MaskingName, out newMakeMaskingName);
                    }
                }
            }
            return makeId;
        }

    }
}
