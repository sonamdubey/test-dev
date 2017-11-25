using Bikewale.Cache.BikeData;
using Bikewale.Cache.Core;
using Bikewale.DAL.BikeData;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.UserReviews;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Notifications;
using Microsoft.Practices.Unity;
using System;

namespace Bikewale.BindViewModels.Webforms.UserReviews
{
    /// <summary>
    /// Created By :Subodh Jain 16 Jan 2017
    /// Summary : Bind View Model for User review Listing Page
    /// </summary>
    public class BindUserReviews
    {
        private readonly IBikeMaskingCacheRepository<BikeModelEntity, int> _objModelCache = null;
        public BindUserReviews()
        {
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IBikeMaskingCacheRepository<BikeModelEntity, int>, BikeModelMaskingCache<BikeModelEntity, int>>()
                             .RegisterType<ICacheManager, MemcacheManager>()
                             .RegisterType<IBikeModelsRepository<BikeModelEntity, int>, BikeModelsRepository<BikeModelEntity, int>>()
                            ;
                    _objModelCache = container.Resolve<IBikeMaskingCacheRepository<BikeModelEntity, int>>();

                }

            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.BindViewModels.Webforms.BindUserReviews");
            }
        }
        /// <summary>
        /// Created By :Subodh Jain 16 Jan 2017 
        /// Summary : Get Details By Modelid and cityid  for User review Listing Page 
        /// <param name="modelId"></param>
        /// <param name="cityId"></param>
        /// <returns></returns>
        public ReviewDetailsEntity GetDetailsByModel(int modelId, uint cityId)
        {
            ReviewDetailsEntity objReview = null;
            try
            {
                objReview = _objModelCache.GetDetailsByModel(modelId, cityId);
            }
            catch (Exception ex)
            {

                ErrorClass.LogError(ex, string.Format("Bikewale.BindViewModels.Webforms.BindUserReviews.GetDetailsByModel_ModelId_{0}_cityId_{1}", modelId, cityId));
            }
            return objReview;
        }
        /// <summary>
        /// Created By :Subodh Jain 16 Jan 2017 
        /// Summary : Get Details By Version and cityid  for User review Listing Page 
        /// <param name="modelId"></param>
        /// <param name="cityId"></param>
        /// <returns></returns>
        public ReviewDetailsEntity GetDetailsByVersion(int versionId, uint cityId)
        {
            ReviewDetailsEntity objReview = null;
            try
            {
                objReview = _objModelCache.GetDetailsByVersion(versionId, cityId);
            }
            catch (Exception ex)
            {

                ErrorClass.LogError(ex, string.Format("Bikewale.BindViewModels.Webforms.BindUserReviews.GetDetailsByVersion_versionId_{0}_cityId_{1}", versionId, cityId));
            }
            return objReview;
        }

    }
}