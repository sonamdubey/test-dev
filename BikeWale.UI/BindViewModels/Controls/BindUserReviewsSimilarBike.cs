
using Bikewale.BAL.BikeData;
using Bikewale.BAL.Pager;
using Bikewale.Cache.BikeData;
using Bikewale.Cache.Core;
using Bikewale.DAL.BikeData;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.UserReviews;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.Pager;
using Bikewale.Notifications;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
namespace Bikewale.BindViewModels.Controls
{
    /// <summary>
    /// Created By :- Subodh Jain 17 Jan 2017
    /// Summary :- Get User Review Similar Bike 
    /// </summary>
    public class BindUserReviewsSimilarBike
    {
        private readonly IBikeModelsCacheRepository<int> _objCache = null;
        public BindUserReviewsSimilarBike()
        {
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IPager, Pager>()
                        .RegisterType<IBikeModelsCacheRepository<int>, BikeModelsCacheRepository<BikeModelEntity, int>>()
                        .RegisterType<IBikeModelsRepository<BikeModelEntity, int>, BikeModelsRepository<BikeModelEntity, int>>()
                        .RegisterType<IBikeModels<BikeModelEntity, int>, BikeModels<BikeModelEntity, int>>()
                        .RegisterType<ICacheManager, MemcacheManager>();

                    _objCache = container.Resolve<IBikeModelsCacheRepository<int>>();



                }
            }
            catch (Exception ex)
            {

                ErrorClass.LogError(ex, " Bikewale.BindViewModels.Controls.BindUserReviewsSimilarBike");
            }
        }
        /// <summary>
        /// Created By :- Subodh Jain 17 Jan 2017
        /// Summary :- Get User Review Similar Bike 
        /// </summary>
        public IEnumerable<BikeUserReviewRating> GetUserReviewSimilarBike(uint modelId, uint topCount)
        {
            IEnumerable<BikeUserReviewRating> objReviewUser = null;
            try
            {
                objReviewUser = _objCache.GetUserReviewSimilarBike(modelId, topCount);

            }
            catch (Exception ex)
            {

                ErrorClass.LogError(ex, string.Format(" Bikewale.BindViewModels.Controls.GetUserReviewSimilarBike_modelid_{0}_topcount_{1}", modelId, topCount));
            }
            return objReviewUser;
        }

    }
}