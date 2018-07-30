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
    public class BindUserReviewsDetails
    {
        /// <summary>
        /// Created By :- Subodh Jain 17 Jan 2016
        /// Summary :- Bind user review details
        /// </summary>
        private readonly IBikeMaskingCacheRepository<BikeModelEntity, int> _objModelCache = null;
        public BindUserReviewsDetails()
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
                ErrorClass.LogError(ex, "Bikewale.BindViewModels.Webforms.BindUserReviewsDetails");
            }
        }
        /// <summary>
        /// Created By :- Subodh Jain 17 Jan 2017
        /// Summary :- Get details for review
        /// </summary>
        /// <param name="reviewId"></param>
        /// <param name="isAlreadyViewed"></param>
        /// <returns></returns>
        public ReviewDetailsEntity GetDetails(string reviewId, bool isAlreadyViewed)
        {
            ReviewDetailsEntity objReview = null;
            try
            {
                objReview = _objModelCache.GetDetails(reviewId, isAlreadyViewed);
            }
            catch (Exception ex)
            {

                ErrorClass.LogError(ex, string.Format("Bikewale.BindViewModels.Webforms.BindUserReviewsDetails.GetDetails_{0}", reviewId));
            }
            return objReview;
        }
    }
}