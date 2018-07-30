
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
    public class BindViewModelVideoDefault
    {
        public IEnumerable<BikeMakeEntityBase> TopMakeList;
        public IEnumerable<BikeMakeEntityBase> OtherMakeList;
        public int TopCount { get; set; }

        private readonly IBikeMaskingCacheRepository<BikeModelEntity, int> _objModelCache = null;
        public BindViewModelVideoDefault()
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
                ErrorClass.LogError(ex, "Bikewale.BindViewModels.Webforms.Videos.BindViewModelVideoDefault");
            }
        }
        /// <summary>
        /// Modified By :- Subodh Jain on 17 Jan 2017
        /// Summary :- get makedetails if videos is present
        /// Modified By :- Subodh Jain 7 Feb 2017
        /// Summary :- OtherMakeList ordered by make
        /// </summary>
        public IEnumerable<BikeMakeEntityBase> GetMakeIfVideo()
        {
            IEnumerable<BikeMakeEntityBase> objVideoMake = null;
            try
            {
                objVideoMake = _objModelCache.GetMakeIfVideo();
                if (objVideoMake != null)
                {
                    TopMakeList = objVideoMake.Take(TopCount);
                    OtherMakeList = objVideoMake.Skip(TopCount).OrderBy(m => m.MakeName);
                }

            }
            catch (Exception ex)
            {

                ErrorClass.LogError(ex, "Bikewale.BindViewModels.Webforms.Videos.BindViewModelVideoDefault.GetMakeIfVideo");
            }
            return objVideoMake;
        }

    }
}