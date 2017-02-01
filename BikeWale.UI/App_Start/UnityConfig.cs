using System.Web.Mvc;
using Bikewale.BAL.BikeData;
using Bikewale.BAL.EditCMS;
using Bikewale.Cache.CMS;
using Bikewale.Cache.Core;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.CMS;
using Bikewale.Interfaces.EditCMS;
using Bikewale.Interfaces.GenericBikes;
using Microsoft.Practices.Unity;
using Unity.Mvc5;

namespace Bikewale
{
    /// <summary>
    /// Created by : Ashish G. Kamble on 6 Jan 2017
    /// </summary>
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
            var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            container.RegisterType<IArticles, Articles>();
            container.RegisterType<ICMSCacheContent, CMSCacheRepository>();
            container.RegisterType<ICacheManager, MemcacheManager>();
            container.RegisterType<IBikeInfo, BikeInfo>();            

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}