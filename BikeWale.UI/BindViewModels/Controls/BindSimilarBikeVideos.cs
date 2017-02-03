
using Bikewale.Cache.BikeData;
using Bikewale.Cache.Core;
using Bikewale.DAL.BikeData;
using Bikewale.Entities.BikeData;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Notifications;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
namespace Bikewale.BindViewModels.Controls
{
    /// <summary>
    /// Created By :- Subodh Jain 3 feb 2017
    /// Summary :- Bind view model For similar bikes video
    /// </summary>
    public class BindSimilarBikeVideos
    {

        private readonly IBikeMaskingCacheRepository<BikeModelEntity, int> _objCache = null;
        public BindSimilarBikeVideos()
        {
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IBikeMaskingCacheRepository<BikeModelEntity, int>, BikeModelMaskingCache<BikeModelEntity, int>>()
                           .RegisterType<ICacheManager, MemcacheManager>()
                           .RegisterType<IBikeModelsRepository<BikeModelEntity, int>, BikeModelsRepository<BikeModelEntity, int>>()
                          ;
                    _objCache = container.Resolve<IBikeMaskingCacheRepository<BikeModelEntity, int>>();
                }
            }
            catch (Exception ex)
            {

                ErrorClass objErr = new ErrorClass(ex, "Bikewale.BindViewModels.Controls.BindSimilarBikeVideos");
            }
        }
        /// <summary>
        /// Created by :- Subodh Jain 3 feb 2017
        /// Summary :- Bind Video details for similar bikes
        /// </summary>
        /// <param name="modelId"></param>
        /// <param name="totalcount"></param>
        /// <returns></returns>
        public IEnumerable<SimilarBikeWithVideo> GetSimilarBikesVideos(uint modelId, uint totalcount)
        {
            IEnumerable<SimilarBikeWithVideo> objSimilarVideo = null;
            try
            {
                objSimilarVideo = _objCache.GetSimilarBikesVideos(modelId, totalcount);

            }
            catch (Exception ex)
            {

                ErrorClass objErr = new ErrorClass(ex, String.Format("Bikewale.BindViewModels.Controls.GetSimilarBikesVideos-modelid: {0}", modelId));
            }
            return objSimilarVideo;
        }


    }
}