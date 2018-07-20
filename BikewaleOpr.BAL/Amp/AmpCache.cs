
using AmpCacheRefreshLibrary;
using Bikewale.Notifications;
using Bikewale.Utility;
using BikewaleOpr.Interface.Amp;
using BikewaleOpr.Interface.BikeData;
using System;
using System.Collections.Generic;
using System.Web;

namespace BikewaleOpr.BAL.Amp
{
    /// <summary>
    /// Created by  : Pratibha Verma on 17 July 2018
    /// Description : BAL for AMP Cache clear
    /// </summary>
    public class AmpCache : IAmpCache
    {
        private readonly IBikeModelsRepository _bikeModelsRepository;
        private readonly IBikeMakesRepository _bikeMakesRepository;
        public AmpCache(IBikeModelsRepository bikeModelsRepository, IBikeMakesRepository bikeMakesRepository)
        {
            _bikeMakesRepository = bikeMakesRepository;
            _bikeModelsRepository = bikeModelsRepository;
        }
        /// <summary>
        /// Make amp cache clear by makeid
        /// </summary>
        /// <param name="makeId"></param>
        public void UpdateMakeAmpCache(uint makeId)
        {
            try
            {
                var makeDetails = _bikeMakesRepository.GetMakeDetailsById(makeId);
                string makeUrl = string.Format("{0}/m/{1}amp", BWConfiguration.Instance.BwHostUrl, UrlFormatter.CreateMakeUrl(makeDetails.MaskingName));
                string privateKeyPath = HttpContext.Current.Server.MapPath("~/App_Data/private-key.pem");
                GoogleAmpCacheRefreshCall.UpdateAmpCache(makeUrl, privateKeyPath);
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("BikewaleOpr.BAL.Amp.UpdateMakeAmpCache({0})", makeId));
            }
        }
        /// <summary>
        /// Created by  : Pratibha Verma on 17 July 2018
        /// Description : Model amp cache clear by modelid
        /// </summary>
        /// <param name="modelId"></param>
        public void UpdateModelAmpCache(uint modelId)
        {
            try
            {
                var modelDetails = _bikeModelsRepository.GetModelDetailsById(modelId);
                string modelUrl = String.Format("{0}/m/{1}amp/", BWConfiguration.Instance.BwHostUrl, UrlFormatter.CreateModelUrl(modelDetails.BikeMake.MaskingName,modelDetails.BikeModel.MaskingName));
                string privateKeyPath = HttpContext.Current.Server.MapPath("~/App_Data/private-key.pem");
                GoogleAmpCacheRefreshCall.UpdateAmpCache(modelUrl, privateKeyPath);
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("BikewaleOpr.BAL.Amp.UpdateModelAmpCache({0})", modelId));
            }
        }

        /// <summary>
        /// Created by  : Pratibha Verma on 17 July 2018
        /// Description : Model amp cache clear for list of modelds
        /// </summary>
        /// <param name="modelIds"></param>
        public void UpdateModelAmpCache(IEnumerable<uint> modelIds)
        {
            try
            {
                if (modelIds != null)
                {
                    foreach (uint modelId in modelIds)
                    {
                        var modelDetails = _bikeModelsRepository.GetModelDetailsById(modelId);
                        string modelUrl = String.Format("{0}/m/{1}amp/", BWConfiguration.Instance.BwHostUrl, UrlFormatter.CreateModelUrl(modelDetails.BikeMake.MaskingName, modelDetails.BikeModel.MaskingName));
                        string privateKeyPath = HttpContext.Current.Server.MapPath("~/App_Data/private-key.pem");
                        GoogleAmpCacheRefreshCall.UpdateAmpCache(modelUrl, privateKeyPath);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "BikewaleOpr.BAL.Amp.UpdateModelAmpCache(modelIds)");
            }
        }
    }
}
