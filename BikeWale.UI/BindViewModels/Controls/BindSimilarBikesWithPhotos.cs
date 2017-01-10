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
    /// Created By : Sushil Kumar on 6th Jan 2016
    /// Summary :  Viewmodel for similar bikes with photos count
    /// </summary>
    public class BindSimilarBikesWithPhotos
    {
        public ushort TotalRecords { get; set; }
        public int ModelId { get; set; }

        /// <summary>
        /// Created By : Sushil Kumar on 6th Jan 2016
        /// Summary :  To bind similar bikes with photos count from cache
        /// </summary>
        public IEnumerable<SimilarBikesWithPhotos> SimilarBikesWithPhotosCount()
        {

            IEnumerable<SimilarBikesWithPhotos> objSimilarBikes = null;

            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IBikeMaskingCacheRepository<BikeModelEntity, int>, BikeModelMaskingCache<BikeModelEntity, int>>()
                             .RegisterType<ICacheManager, MemcacheManager>()
                             .RegisterType<IBikeModelsRepository<BikeModelEntity, int>, BikeModelsRepository<BikeModelEntity, int>>()
                            ;
                    var objCache = container.Resolve<IBikeMaskingCacheRepository<BikeModelEntity, int>>();
                    objSimilarBikes = objCache.GetSimilarBikeWithPhotos(ModelId, TotalRecords);
                }

            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Bikewale.BindViewModels.Controls.BindSimilarBikesWithPhotos_ModelId_" + ModelId);
            }
            return objSimilarBikes;
        }
    }
}