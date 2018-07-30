using Bikewale.Cache.Core;
using Bikewale.Cache.Used;
using Bikewale.DAL.Used;
using Bikewale.Entities.Used;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.Used;
using Bikewale.Notifications;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bikewale.BindViewModels.Controls
{
    /// Created by : Sangram Nandkhile on 30th Aug 2016
    /// Summary: To get recent used bikes

    public class BindUsedRecentBikes
    {
        public IEnumerable<OtherUsedBikeDetails> RecentUsedBikes;
        public int FetchedRecordsCount { get; set; }

        public BindUsedRecentBikes(ushort topCount)
        {
            RecentUsedBikes = GetRecentUsedBikes(topCount);
            if (RecentUsedBikes.Any())
                FetchedRecordsCount = RecentUsedBikes.Count();
        }

        /// <summary>
        /// Created by : Sangram Nandkhile on 30th Aug 2016
        /// Summary: To get recent used bikes
        public IEnumerable<OtherUsedBikeDetails> GetRecentUsedBikes(ushort topCount)
        {
            IEnumerable<OtherUsedBikeDetails> usedRecentBikes = null;
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IUsedBikeDetailsCacheRepository, UsedBikeDetailsCache>()
                        .RegisterType<IUsedBikeDetails, UsedBikeDetailsRepository>()
                        .RegisterType<ICacheManager, MemcacheManager>();
                    var objCache = container.Resolve<IUsedBikeDetailsCacheRepository>();
                    usedRecentBikes = objCache.GetRecentUsedBikesInIndia(topCount);
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"] + "BindUsedRecentBikes.GetRecentUsedBikes");
                
            }
            return usedRecentBikes;
        }
    }
}