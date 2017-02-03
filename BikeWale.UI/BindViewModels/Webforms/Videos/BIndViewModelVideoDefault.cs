
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
using System.Linq;
namespace Bikewale.BindViewModels.Webforms.Videos
{
    /// <summary>
    /// Created By :- Subodh Jain 3 feb 2017
    /// Summary :- Bind view model for videos page
    /// </summary>
    public class BIndViewModelVideoDefault
    {
        public IEnumerable<BikeMakeEntityBase> TopMakeList;
        public IEnumerable<BikeMakeEntityBase> OtherMakeList;
        public int TopCount { get; set; }

        private readonly IBikeMaskingCacheRepository<BikeModelEntity, int> _objModelCache = null;
        public BIndViewModelVideoDefault()
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
                ErrorClass objErr = new ErrorClass(ex, "Bikewale.BindViewModels.Webforms.Videos.BIndViewModelVideoDefault");
            }
        }
        /// <summary>
        /// Modified By :- Subodh Jain on 17 Jan 2017
        /// Summary :- get makedetails if videos is present
        /// </summary>
        public IEnumerable<BikeMakeEntityBase> GetMakeIfVideo()
        {
            IEnumerable<BikeMakeEntityBase> objVideoMake = null;
            try
            {
                objVideoMake = _objModelCache.GetMakeIfVideo();
                if (objVideoMake != null)
                {
                    if (objVideoMake.Count() > TopCount)
                    {
                        TopMakeList = objVideoMake.Where(m => m.PopularityIndex > 0).Take(TopCount);
                        OtherMakeList = objVideoMake.Where(m => (m.PopularityIndex == 0 || m.PopularityIndex > TopCount));

                    }
                    else
                    {
                        TopMakeList = objVideoMake;

                    }
                }

            }
            catch (Exception ex)
            {

                ErrorClass objErr = new ErrorClass(ex, "Bikewale.BindViewModels.Webforms.Videos.BIndViewModelVideoDefault.GetMakeIfVideo");
            }
            return objVideoMake;
        }

    }
}